using Data.Entities;
using Data.Repositories;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace WpfDesigb.ViewModels
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        private readonly UserRepository _userRepository;
        public ObservableCollection<UserEntity> Employees { get; set; }

        private UserEntity? _selectedEmployee;
        public UserEntity? SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged(); // Notify UI of selection change
            }
        }

        public EmployeeViewModel(UserRepository userRepository)
        {
            _userRepository = userRepository;
            Employees = new ObservableCollection<UserEntity>();
            LoadEmployees(); // Load employees immediately
        }

        public async Task LoadEmployees()
        {
            var employees = await _userRepository.GetAllAsync();
            App.Current.Dispatcher.Invoke(() =>
            {
                Employees.Clear();
                foreach (var employee in employees)
                {
                    Employees.Add(employee);
                }
            });
        }

        public async Task AddEmployeeAsync(UserEntity employee)
        {
            if (employee == null) return;

            await _userRepository.CreateAsync(employee);
            await LoadEmployees(); // Refresh UI after adding an employee
        }

        public async Task UpdateEmployeeAsync(UserEntity employee)
        {
            if (employee == null) return;

            await _userRepository.UpdateAsync(employee);
            await LoadEmployees(); // Refresh UI after updating
        }

        public async Task DeleteEmployeeAsync()
        {
            if (SelectedEmployee == null)
            {
                MessageBox.Show("Välj en anställd att ta bort!", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Är du säker på att du vill ta bort {SelectedEmployee.FirstName} {SelectedEmployee.LastName}?",
                                         "Bekräfta borttagning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                await _userRepository.DeleteAsync(SelectedEmployee.Id);
                await LoadEmployees();
            }
        }

        // 🛑 Implement INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}