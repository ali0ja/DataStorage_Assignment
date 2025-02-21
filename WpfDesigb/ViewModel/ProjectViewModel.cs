using Data.Entities;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfDesigb.ViewModel
{
    public class ProjectViewModel : INotifyPropertyChanged
    {
        private readonly ProjectRepository _projectRepository;

        // ✅ Collections for UI Binding
        public ObservableCollection<ProjectEntity> Projects { get; set; } = new();
        public ObservableCollection<CustomerEntity> Customers { get; set; } = new();
        public ObservableCollection<ProductEntity> Products { get; set; } = new();
        public ObservableCollection<UserEntity> Users { get; set; } = new();
        public ObservableCollection<string> Statuses { get; set; } = new();

        // ✅ Selected Project Property
        private ProjectEntity? _selectedProject;
        public ProjectEntity? SelectedProject
        {
            get => _selectedProject;
            set
            {
                _selectedProject = value;
                OnPropertyChanged();
            }
        }

        // ✅ Selected User Property
        private UserEntity? _selectedUser;
        public UserEntity? SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
        }

        // ✅ Constructor - Load Data at Initialization
        public ProjectViewModel(ProjectRepository projectRepository)
        {
            _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));

            LoadStatuses(); // Load predefined status values
            Task.Run(async () => await LoadAllData()); // Load all data asynchronously
        }

        // ✅ Predefined Status List
        private void LoadStatuses()
        {
            Statuses.Clear();
            Statuses.Add("Ej påbörjat");
            Statuses.Add("Pågående");
            Statuses.Add("Avslutat");
        }

        // ✅ Load All Data (Projects, Customers, Products, Users)
        public async Task LoadAllData()
        {
            await LoadProjects();
            await LoadCustomers();
            await LoadProducts();
            await LoadUsers();
        }

        // ✅ Load Projects
        public async Task LoadProjects()
        {
            try
            {
                var projects = await _projectRepository.GetAllAsync();
                App.Current.Dispatcher.Invoke(() =>
                {
                    Projects.Clear();
                    foreach (var project in projects)
                    {
                        Projects.Add(project);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading projects: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ✅ Load Customers
        public async Task LoadCustomers()
        {
            try
            {
                var customers = await _projectRepository.GetCustomersAsync();
                App.Current.Dispatcher.Invoke(() =>
                {
                    Customers.Clear();
                    foreach (var customer in customers)
                    {
                        Customers.Add(customer);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading customers: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ✅ Load Products
        public async Task LoadProducts()
        {
            try
            {
                var products = await _projectRepository.GetProductsAsync();
                App.Current.Dispatcher.Invoke(() =>
                {
                    Products.Clear();
                    foreach (var product in products)
                    {
                        Products.Add(product);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ✅ Load Users
        public async Task LoadUsers()
        {
            try
            {
                var users = await _projectRepository.GetUsersAsync();
                App.Current.Dispatcher.Invoke(() =>
                {
                    Users.Clear();
                    foreach (var user in users)
                    {
                        Users.Add(user);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading users: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ✅ Get Data for ComboBoxes
        public async Task<ObservableCollection<UserEntity>> GetUsersAsync()
        {
            return new ObservableCollection<UserEntity>(await _projectRepository.GetUsersAsync());
        }

        public async Task<ObservableCollection<CustomerEntity>> GetCustomersAsync()
        {
            return new ObservableCollection<CustomerEntity>(await _projectRepository.GetCustomersAsync());
        }

        public async Task<ObservableCollection<ProductEntity>> GetProductsAsync()
        {
            return new ObservableCollection<ProductEntity>(await _projectRepository.GetProductsAsync());
        }

        // ✅ Add Project
        public async Task AddProjectAsync(ProjectEntity project)
        {
            try
            {
                if (project == null) return;

                await _projectRepository.CreateAsync(project);

                // Reload Projects after adding to database
                await LoadProjects();

                MessageBox.Show("Project successfully saved!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving project: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        

        // ✅ Delete Project
        public async Task DeleteProjectAsync()
        {
            if (SelectedProject == null)
            {
                MessageBox.Show("Select a project to delete!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to delete {SelectedProject.Title}?",
                                         "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                await _projectRepository.DeleteAsync(SelectedProject.Id);
                await LoadProjects();
            }
        }

        // ✅ PropertyChanged for UI updates
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}

    
