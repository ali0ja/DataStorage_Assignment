using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using WpfDesigb.ViewModels;

namespace WpfDesigb
{
    /// <summary>
    /// Interaction logic for AddEditCustomer.xaml
    /// </summary>
    public partial class AddEditCustomer : Window
    {
        private readonly CustomerViewModel _customerViewModel;
        private readonly CustomerEntity? _customerToEdit;

        public AddEditCustomer(CustomerViewModel customerViewModel, CustomerEntity? customer = null)
        {
            InitializeComponent();
            _customerViewModel = customerViewModel;
            _customerToEdit = customer;

            if (_customerToEdit != null)
            {
                // If editing, pre-fill the text fields
                tbCustomername.Text = _customerToEdit.CustomerName;
            }
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbCustomername.Text))
            {
                MessageBox.Show("Customer name cannot be empty!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_customerToEdit == null)
            {
                // Create a new customer
                var newCustomer = new CustomerEntity
                {
                    CustomerName = tbCustomername.Text
                };

                await _customerViewModel.AddCustomerAsync(newCustomer);
            }
            else
            {
                // Update existing customer
                _customerToEdit.CustomerName = tbCustomername.Text;

                await _customerViewModel.UpdateCustomerAsync(_customerToEdit);
            }

            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
