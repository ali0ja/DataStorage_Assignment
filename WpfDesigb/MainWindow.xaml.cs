using Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfDesigb.ViewModel;
using Data.Repositories;
using Data.Entities;
using WpfDesigb.ViewModels;
using Data.Contexts;

namespace WpfDesigb
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly CustomerViewModel _customerViewModel;
        private readonly ProductViewModel _productViewModel;
        private readonly EmployeeViewModel _employeeViewModel;
        private readonly StatusTypeViewModel _statusTypeViewModel;
        private readonly ProjectViewModel _projectViewModel;
        public MainWindow()
        {
            InitializeComponent();

            _customerViewModel = new CustomerViewModel(App.ServiceProvider.GetService<CustomerRepository>());
            _productViewModel = new ProductViewModel(App.ServiceProvider.GetService<ProductRepository>());
            _employeeViewModel = new EmployeeViewModel(App.ServiceProvider.GetService<UserRepository>());
            _statusTypeViewModel = new StatusTypeViewModel(App.ServiceProvider.GetService<StatusTypeRepository>());

            var projectRepository = new ProjectRepository(new DataContextFactory());
            _projectViewModel = new ProjectViewModel(projectRepository);

            
           

            // Bind ViewModel to DataContext


            DataContext = this;
            CustomersPanel.DataContext = _customerViewModel;
            ProductsPanel.DataContext = _productViewModel;
            EmployeePanel.DataContext = _employeeViewModel;
            ProjectPanel.DataContext = _projectViewModel;
            DataContext = _projectViewModel;
        }
        
        


       
        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            HomPanel.Visibility = Visibility.Visible;
            EmployeePanel.Visibility = Visibility.Collapsed;
            CustomersPanel.Visibility = Visibility.Collapsed;
            ProductsPanel.Visibility = Visibility.Collapsed;
            ProjectPanel.Visibility = Visibility.Collapsed;


        }

        private void btnCustomer_Click(object sender, RoutedEventArgs e)
        {
            HomPanel.Visibility = Visibility.Collapsed;
            EmployeePanel.Visibility = Visibility.Collapsed;
            CustomersPanel.Visibility = Visibility.Visible;
            ProductsPanel.Visibility = Visibility.Collapsed;
            ProjectPanel.Visibility = Visibility.Collapsed;
        }

        private void btnEmployees_Click(object sender, RoutedEventArgs e)
        {
            HomPanel.Visibility = Visibility.Collapsed;
            EmployeePanel.Visibility = Visibility.Visible;
            CustomersPanel.Visibility = Visibility.Collapsed;
            ProductsPanel.Visibility = Visibility.Collapsed;
            ProjectPanel.Visibility = Visibility.Collapsed;
        }

        private void btnProduct_Click(object sender, RoutedEventArgs e)
        {
            HomPanel.Visibility = Visibility.Collapsed;
            EmployeePanel.Visibility = Visibility.Collapsed;
            CustomersPanel.Visibility = Visibility.Collapsed;
            ProductsPanel.Visibility = Visibility.Visible;
            ProjectPanel.Visibility = Visibility.Collapsed;
        }

        private void btnProject_Click(object sender, RoutedEventArgs e)
        {
            HomPanel.Visibility = Visibility.Collapsed;
            EmployeePanel.Visibility = Visibility.Collapsed;
            CustomersPanel.Visibility = Visibility.Collapsed;
            ProductsPanel.Visibility = Visibility.Collapsed;
            ProjectPanel.Visibility=  Visibility.Visible;
        }

        private void btnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            var employeeViewModel = new EmployeeViewModel(App.ServiceProvider.GetService<UserRepository>());
            var addEditEmployeeWindow = new AddEditEmployee(employeeViewModel);
            addEditEmployeeWindow.ShowDialog();
        }

        private void btnEditEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeeGrid.SelectedItem is UserEntity selectedEmployee)
            {
                var employeeViewModel = new EmployeeViewModel(App.ServiceProvider.GetService<UserRepository>());
                var editEmployeeWindow = new AddEditEmployee(employeeViewModel, selectedEmployee);
                editEmployeeWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Välj en anställd att redigera!", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void btnDeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (_employeeViewModel.SelectedEmployee != null)  // Använd _employeeViewModel här
            {
                await _employeeViewModel.DeleteEmployeeAsync();
            }
            else
            {
                MessageBox.Show("Välj en anställd att ta bort!", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            var addCustomerWindow = new AddEditCustomer(_customerViewModel);
            addCustomerWindow.ShowDialog();
        }

        private async void btnDeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (_customerViewModel.SelectedCustomer == null)
            {
                MessageBox.Show("Välj en kund att ta bort!", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Är du säker på att du vill ta bort {_customerViewModel.SelectedCustomer.CustomerName}?",
                                         "Bekräfta borttagning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                await _customerViewModel.DeleteCustomerAsync();
            }
        }

        private void btnEditCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (_customerViewModel.SelectedCustomer == null)
            {
                MessageBox.Show("Välj en kund att redigera!", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var editCustomerWindow = new AddEditCustomer(_customerViewModel, _customerViewModel.SelectedCustomer);
            editCustomerWindow.ShowDialog();
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            var addProductWindow = new AddEditProduct(_productViewModel);
            addProductWindow.ShowDialog();
        }

        private async void btnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (_productViewModel.SelectedProduct == null)
            {
                MessageBox.Show("Välj en produkt att ta bort!", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            await _productViewModel.DeleteProductAsync();
        }

        

        private void btnEditProduct_Click(object sender, RoutedEventArgs e)
        {
            if (_productViewModel.SelectedProduct == null)
            {
                MessageBox.Show("Välj en produkt att redigera!", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var editProductWindow = new AddEditProduct(_productViewModel, _productViewModel.SelectedProduct);
            editProductWindow.ShowDialog();
        }

        //  Add Project
        private void btnAddProject_Click(object sender, RoutedEventArgs e)
        {
            //  Open the Add Project window (No second argument)
            var addProjectWindow = new AddEditProject(_projectViewModel);
            addProjectWindow.ShowDialog();
        }

        private void btnEditProject_Click(object sender, RoutedEventArgs e)
        {
            if (_projectViewModel.SelectedProject == null)
            {
                MessageBox.Show("Please select a project to edit!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //  Open the Edit Project window (Pass the selected project)
            var editProjectWindow = new AddEditProject(_projectViewModel, _projectViewModel.SelectedProject);
            editProjectWindow.ShowDialog();
        }

        //  Delete Project
        private async void btnDeleteProject_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProjectViewModel viewModel)
            {
                await viewModel.DeleteProjectAsync(); //  Calls the correct method
            }

            var result = MessageBox.Show($"Are you sure you want to delete {_projectViewModel.SelectedProject.Title}?",
                                         "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                await _projectViewModel.DeleteProjectAsync();
            }
        }
    }
}