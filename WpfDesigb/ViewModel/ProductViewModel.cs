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
    public class ProductViewModel : INotifyPropertyChanged
    {
        private readonly ProductRepository _productRepository;
        public ObservableCollection<ProductEntity> Products { get; set; }

        private ProductEntity? _selectedProduct;
        public ProductEntity? SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
            }
        }

        public ProductViewModel(ProductRepository productRepository)
        {
            _productRepository = productRepository;
            Products = new ObservableCollection<ProductEntity>();
            LoadProducts();
        }

        public async Task LoadProducts()
        {
            var products = await _productRepository.GetAllAsync();
            App.Current.Dispatcher.Invoke(() =>
            {
                Products.Clear();
                foreach (var product in products)
                {
                    Products.Add(product);
                }
            });
        }

        public async Task AddProductAsync(ProductEntity product)
        {
            if (product == null) return;

            await _productRepository.CreateAsync(product);
            await LoadProducts();
        }

        public async Task UpdateProductAsync(ProductEntity product)
        {
            if (product == null) return;

            await _productRepository.UpdateAsync(product);
            await LoadProducts();
        }

        public async Task DeleteProductAsync()
        {
            if (SelectedProduct == null)
            {
                MessageBox.Show("Välj en produkt att ta bort!", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Är du säker på att du vill ta bort {SelectedProduct.ProductName}?",
                                         "Bekräfta borttagning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                await _productRepository.DeleteAsync(SelectedProduct.Id);
                await LoadProducts();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

