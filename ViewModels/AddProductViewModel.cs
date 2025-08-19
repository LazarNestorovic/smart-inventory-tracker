using SmartInventoryTracker.DTOs;
using SmartInventoryTracker.Interfaces;
using SmartInventoryTracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace SmartInventoryTracker.ViewModels
{
    public class AddProductViewModel : INotifyPropertyChanged
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISupplierRepository _supplierRepository;

        // Private fields for properties
        private string _productName;
        private string _productCode;
        private int _quantity;
        private int _minimumStock;
        private CategoryDTO _selectedCategory;
        private SupplierDTO _selectedSupplier;
        private string _validationMessage;
        private bool _isValid = true;

        // Collections
        public ObservableCollection<CategoryDTO> Categories { get; set; }
        public ObservableCollection<SupplierDTO> Suppliers { get; set; }

        // Constructor
        public AddProductViewModel(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            ISupplierRepository supplierRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _supplierRepository = supplierRepository;

            Categories = new ObservableCollection<CategoryDTO>();
            Suppliers = new ObservableCollection<SupplierDTO>();

            ProductName = string.Empty;
            ProductCode = string.Empty;
            Quantity = 0;
            MinimumStock = 1;

            LoadData();
        }

        public string ProductName
        {
            get => _productName;
            set
            {
                _productName = value;
                ValidateProductName();  
                ValidateForm();
                OnPropertyChanged();
            }
        }

        public string ProductCode
        {
            get => _productCode;
            set
            {
                _productCode = value;
                ValidateProductCode();
                ValidateForm();
                OnPropertyChanged();
            }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                ValidateQuantity();
                ValidateForm();
                OnPropertyChanged();
            }
        }

        public int MinimumStock
        {
            get => _minimumStock;
            set
            {
                _minimumStock = value;
                ValidateMinimumStock();
                ValidateForm();
                OnPropertyChanged();
            }
        }

        public CategoryDTO SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                ValidateCategory();
                ValidateForm();
                OnPropertyChanged();
            }
        }

        public SupplierDTO SelectedSupplier
        {
            get => _selectedSupplier;
            set
            {
                _selectedSupplier = value;
                ValidateSupplier();
                ValidateForm();
                OnPropertyChanged();
            }
        }

        public bool IsValid
        {
            get => _isValid;
            set
            {
                _isValid = value;
                OnPropertyChanged();
            }
        }

        private void LoadData()
        {
            try
            {
                LoadCategories();
                LoadSuppliers();
            }
            catch (Exception ex)
            {
                IsValid = false;
            }
        }

        private void LoadCategories()
        {
            try
            {
                var categories = _categoryRepository.GetAllCategories();
                Categories.Clear();
                foreach (var category in categories.OrderBy(c => c.Name))
                {
                    if (category.Id != 2)
                    {
                        Categories.Add(new CategoryDTO(category));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show ($"Error loading categories: {ex.Message}");
            }
        }

        private void LoadSuppliers()
        {
            try
            {
                var suppliers = _supplierRepository.GetAllSuppliers();

                Suppliers.Clear();
                foreach (var supplier in suppliers.OrderBy(s => s.Name))
                {
                    Suppliers.Add( new SupplierDTO(supplier));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading suppliers: {ex.Message}");
            }
        }

        private void ValidateProductName()
        {
            if (string.IsNullOrWhiteSpace(ProductName))
            {
                IsValid = false;
                return;
            }

            if (ProductName.Length < 2)
            {
                IsValid = false;
                return;
            }

            if (ProductName.Length > 100)
            {
                IsValid = false;
                return;
            }
        }

        private void ValidateProductCode()
        {
            if (string.IsNullOrWhiteSpace(ProductCode))
            {
                IsValid = false;
                return;
            }

            if (ProductCode.Length < 3)
            {
                IsValid = false;
                return;
            }

            if (ProductCode.Length > 20)
            {
                IsValid = false;
                return;
            }

            try
            {
                if (ProductCodeExists(ProductCode))
                {
                    MessageBox.Show("Product code already exists. Please choose a different code.");
                    IsValid = false;
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking product code: {ex.Message}");
                IsValid = false;
                return;
            }
        }

        private bool ProductCodeExists(string productCode)
        {
            var existingProduct = _productRepository.GetAllProducts();
            foreach (var product in existingProduct)
            {
                if (!product.Deleted && product.ProductCode == productCode)
                {
                    return true;
                }
            }
            return false;
        }

        private void ValidateQuantity()
        {
            if (Quantity < 0)
            {
                IsValid = false;
                return;
            }

            if (Quantity > 999999)
            {
                IsValid = false;
                return;
            }
        }

        private void ValidateMinimumStock()
        {
            if (MinimumStock < 0)
            {
                IsValid = false;
                return;
            }

            if (MinimumStock > 99999)
            {
                IsValid = false;
                return;
            }
        }

        private void ValidateCategory()
        {
            if (SelectedCategory == null)
            {
                IsValid = false;
                return;
            }
        }

        private void ValidateSupplier()
        {
            if (SelectedSupplier == null)
            {
                IsValid = false;
                return;
            }
        }

        private void ValidateForm()
        {
            IsValid = true;

            ValidateProductName();
            if (!IsValid) return;

            ValidateProductCode();
            if (!IsValid) return;

            ValidateQuantity();
            if (!IsValid) return;

            ValidateMinimumStock();
            if (!IsValid) return;

            ValidateCategory();
            if (!IsValid) return;

            ValidateSupplier();
            if (!IsValid) return;

            IsValid = true;
        }

        public bool SaveProduct()
        {
            try
            {
                ValidateForm();
                if (!IsValid)
                {
                    return false;
                }

                string trimmedCode = ProductCode.Trim().ToUpperInvariant();

                var deletedProduct = _productRepository.GetDeletedProductByCode(trimmedCode);

                if (deletedProduct != null)
                {
                    var result = MessageBox.Show(
                        $"A product with the code '{trimmedCode}' already exists in the deleted products.\n\n" +
                        $"Existing product: {deletedProduct.Name}\n" +
                        $"New product: {ProductName.Trim()}\n\n" +
                        "Would you like to:\n" +
                        "• Click 'Yes' to restore and update the existing product\n" +
                        "• Click 'No' to change the code of the new product",
                        "Product already exists",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question,
                        MessageBoxResult.No);

                    if (result == MessageBoxResult.Yes)
                    {
                        return RestoreAndUpdateProduct(deletedProduct);
                    }
                    else
                    {
                        MessageBox.Show("Please change the product code before saving.",
                                        "Product Code Change Required",
                                       MessageBoxButton.OK,
                                       MessageBoxImage.Information);
                        IsValid = false;
                        return false;
                    }
                }

                var newProduct = new Product
                {
                    Name = ProductName.Trim(),
                    ProductCode = ProductCode.Trim().ToUpperInvariant(),
                    Quantity = Quantity,
                    MinimumStock = MinimumStock,
                    CategoryId = SelectedCategory.Id,
                    Category = SelectedCategory.ToCategory(),
                    SupplierId = SelectedSupplier.Id,
                    Supplier = SelectedSupplier.ToSupplier()
                };

                _productRepository.AddProduct(newProduct);

                return true;
            }
            catch (Exception ex)
            {
                IsValid = false;
                return false;
            }
        }

        private bool RestoreAndUpdateProduct(Product deletedProduct)
        {
            deletedProduct.Deleted = false;
            deletedProduct.Name = ProductName.Trim();
            deletedProduct.ProductCode = ProductCode.Trim().ToUpperInvariant();
            deletedProduct.Quantity = Quantity;
            deletedProduct.MinimumStock = MinimumStock;
            deletedProduct.CategoryId = SelectedCategory.Id;
            deletedProduct.Category = SelectedCategory.ToCategory();
            deletedProduct.SupplierId = SelectedSupplier.Id;
            deletedProduct.Supplier = SelectedSupplier.ToSupplier();
            try
            {
                _productRepository.UpdateProduct(deletedProduct);
                MessageBox.Show("Product has been successfully restored and updated.",
                                "Success",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while restoring the product: {ex.Message}",
                                 "Error",

                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return false;
            }
        }

        public void ResetForm()
        {
            ProductName = string.Empty;
            ProductCode = string.Empty;
            Quantity = 0;
            MinimumStock = 1;
            SelectedCategory = null;
            SelectedSupplier = null;
            IsValid = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
