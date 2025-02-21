using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfDesigb.ViewModel;

namespace WpfDesigb
{
    /// <summary>
    /// Interaction logic for AddEditProject.xaml
    /// </summary>
    public partial class AddEditProject : Window
    {
        private readonly ProjectViewModel _viewModel;
        private readonly ProjectEntity? _projectToEdit;

        private async void LoadComboBoxData()
        {
            cbCustomer.ItemsSource = await _viewModel.GetCustomersAsync();
            cbProduct.ItemsSource = await _viewModel.GetProductsAsync();
            cbUser.ItemsSource = await _viewModel.GetUsersAsync();  //  Load users into the dropdown
            cbStatus.ItemsSource = new List<string> { "Ej påbörjat", "Pågående", "Avslutat" }; //  Predefined Status
        }
        public AddEditProject(ProjectViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel; //  Binds the UI to the ViewModel
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tbTitle.Text) ||
                    dpStartDate.SelectedDate == null ||
                    dpEndDate.SelectedDate == null  ||
                    cbCustomer.SelectedItem == null  ||
                    cbProduct.SelectedItem == null ||
                    cbUser.SelectedItem == null  ||
                    cbStatus.SelectedItem == null)
        {
                    MessageBox.Show("All fields must be filled!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var project = new ProjectEntity
                {
                    Title = tbTitle.Text,
                    StartDate = dpStartDate.SelectedDate.Value,
                    EndDate = dpEndDate.SelectedDate.Value,
                    Customer = cbCustomer.SelectedItem as CustomerEntity,
                    Product = cbProduct.SelectedItem as ProductEntity,
                    User = cbUser.SelectedItem as UserEntity,
                    Status = cbStatus.SelectedItem as StatusTypeEntity
                };

                await _viewModel.AddProjectAsync(project);
                MessageBox.Show("Project added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving project: {ex.Message}\n\nInner Exception: {ex.InnerException?.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
       

       

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        // ✅ Fix: Allow editing an existing project
        public AddEditProject(ProjectViewModel viewModel, ProjectEntity? project = null)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _projectToEdit = project;

            // ✅ If editing an existing project, fill in the UI fields
            if (_projectToEdit != null)
            {
                tbTitle.Text = _projectToEdit.Title;
                dpStartDate.SelectedDate = _projectToEdit.StartDate;
                dpEndDate.SelectedDate = _projectToEdit.EndDate;
                cbCustomer.SelectedItem = _projectToEdit.Customer;
                cbProduct.SelectedItem = _projectToEdit.Product;
                cbStatus.SelectedItem = _projectToEdit.Status;
            }
        }

        

    }

}

