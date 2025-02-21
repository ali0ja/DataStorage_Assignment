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
    public class CustomerViewModel : INotifyPropertyChanged
    {
        private readonly CustomerRepository _customerRepository;
        public ObservableCollection<CustomerEntity> Customers { get; set; }

        private CustomerEntity? _selectedCustomer;
        public CustomerEntity? SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged();
            }
        }
        

        public CustomerViewModel(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
            Customers = new ObservableCollection<CustomerEntity>();
            LoadCustomers();
        }

        public async Task LoadCustomers()
        {
            var customers = await _customerRepository.GetAllAsync();
            App.Current.Dispatcher.Invoke(() =>
            {
                Customers.Clear();
                foreach (var customer in customers)
                {
                    Customers.Add(customer);
                }
            });
        }

        public async Task AddCustomerAsync(CustomerEntity customer)
        {
            if (customer == null) return;

            await _customerRepository.CreateAsync(customer);
            await LoadCustomers(); // Refresh UI
        }

        public async Task UpdateCustomerAsync(CustomerEntity customer)
        {
            if (customer == null) return;

            await _customerRepository.UpdateAsync(customer);
            await LoadCustomers(); // Refresh UI
        }

        public async Task DeleteCustomerAsync()
        {
            if (SelectedCustomer == null)
            {
                MessageBox.Show("Välj en kund att ta bort!", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Är du säker på att du vill ta bort {SelectedCustomer.CustomerName}?",
                                         "Bekräfta borttagning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                await _customerRepository.DeleteAsync(SelectedCustomer.Id);
                await LoadCustomers();
            }
        }

        // Notify UI of changes


        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
