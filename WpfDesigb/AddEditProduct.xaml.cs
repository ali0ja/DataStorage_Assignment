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

namespace WpfDesigb
{
    /// <summary>
    /// Interaction logic for AddEditProduct.xaml
    /// </summary>
    public partial class AddEditProduct : Window
    {
        private readonly ProductViewModel _productViewModel;
        private readonly ProductEntity? _productToEdit;

        public AddEditProduct(ProductViewModel productViewModel, ProductEntity? product = null)
        {
            InitializeComponent();
            _productViewModel = productViewModel;
            _productToEdit = product;

            if (_productToEdit != null)
            {
                // If editing, pre-fill the text fields
                tbProductname.Text = _productToEdit.ProductName;
                tbProductPrice.Text = _productToEdit.Price.ToString();
            }
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbProductname.Text) ||
                string.IsNullOrWhiteSpace(tbProductPrice.Text) ||
                !decimal.TryParse(tbProductPrice.Text, out decimal price))
            {
                MessageBox.Show("Alla fält måste fyllas i korrekt!", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_productToEdit == null)
            {
                // Creating a new product
                var newProduct = new ProductEntity
                {
                    ProductName = tbProductname.Text,
                    Price = price
                };

                await _productViewModel.AddProductAsync(newProduct);
            }
            else
            {
                // Updating an existing product
                _productToEdit.ProductName = tbProductname.Text;
                _productToEdit.Price = price;

                await _productViewModel.UpdateProductAsync(_productToEdit);
            }

            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

