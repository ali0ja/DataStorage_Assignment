using Data.Entities;
using System.Threading.Tasks;
using System.Windows;
using WpfDesigb.ViewModel;
using WpfDesigb.ViewModels;


namespace WpfDesigb
{
    /// <summary>
    /// Interaction logic for AddEditEmployee.xaml
    /// </summary>
    public partial class AddEditEmployee : Window
    {
        private readonly EmployeeViewModel _employeeViewModel;
        private readonly UserEntity? _employeeToEdit;

        public AddEditEmployee(EmployeeViewModel employeeViewModel, UserEntity? employee = null)
        {
            InitializeComponent();
            _employeeViewModel = employeeViewModel;
            _employeeToEdit = employee;

            if (_employeeToEdit != null)
            {
                // If editing, pre-fill the text fields
                tbFirstname.Text = _employeeToEdit.FirstName;
                tbLastname.Text = _employeeToEdit.LastName;
                tbEmail.Text = _employeeToEdit.Email;
            }
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbFirstname.Text) ||
                string.IsNullOrWhiteSpace(tbLastname.Text) ||
                string.IsNullOrWhiteSpace(tbEmail.Text))
            {
                MessageBox.Show("Alla fält måste fyllas i!", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_employeeToEdit == null)
            {
                // Creating a new employee
                var newEmployee = new UserEntity
                {
                    FirstName = tbFirstname.Text,
                    LastName = tbLastname.Text,
                    Email = tbEmail.Text
                };

                await _employeeViewModel.AddEmployeeAsync(newEmployee);
            }
            else
            {
                // Updating an existing employee
                _employeeToEdit.FirstName = tbFirstname.Text;
                _employeeToEdit.LastName = tbLastname.Text;
                _employeeToEdit.Email = tbEmail.Text;

                await _employeeViewModel.UpdateEmployeeAsync(_employeeToEdit);
            }

            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}