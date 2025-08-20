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

namespace SmartInventoryTracker.ViewModels
{
    public class EditProductViewModel : INotifyPropertyChanged
    {
        private int selectedProductId;
        private string _productName;
        private string _productCode;
        private CategoryDTO _selectedCategory;
        private SupplierDTO _selectedSupplier;
        private string _quantity;
        private string _minimumStock;
        private ProductDTO _originalProduct;
        
        public string ProductName
        {
            get => _productName;
            set
            {
                _productName = value;
                OnPropertyChanged();
                ValidateForm();
            }
        }

        public string ProductCode
        {
            get => _productCode;
            set
            {
                _productCode = value;
                OnPropertyChanged();
                ValidateForm();
            }
        }

        public CategoryDTO SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
                ValidateForm();
            }
        }

        public SupplierDTO SelectedSupplier
        {
            get => _selectedSupplier;
            set
            {
                _selectedSupplier = value;
                OnPropertyChanged();
                ValidateForm();
            }
        }

        public string Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                ValidateForm();
                OnPropertyChanged();
            }
        }

        public string MinimumStock
        {
            get => _minimumStock;
            set
            {
                _minimumStock = value;
                OnPropertyChanged();
                ValidateForm();
            }
        }

        private bool _isValid = true;
        public bool IsValid
        {
            get => _isValid;
            set
            {
                _isValid = value;
                OnPropertyChanged();
            }
        }

        public bool IsFormValid { get; private set; }

        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISupplierRepository _supplierRepository;

        public ObservableCollection<CategoryDTO> Categories { get; set; }
        public ObservableCollection<SupplierDTO> Suppliers { get; set; }

        public EditProductViewModel(IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            ISupplierRepository supplierRepository)
        {
            Categories = new ObservableCollection<CategoryDTO>();
            Suppliers = new ObservableCollection<SupplierDTO>();

            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _supplierRepository = supplierRepository;
        }

        public void LoadData(ProductDTO selectedProduct)
        {
            try
            {
                LoadCategories();
                LoadSuppliers();
                LoadProductData(selectedProduct);
            }
            catch (Exception ex)
            {
                IsValid = false;
            }
        }

        private void LoadProductData(ProductDTO product)
        {
            if (product == null) return;

            ProductName = product.Name;
            ProductCode = product.ProductCode;
            Quantity = product.Quantity.ToString();
            MinimumStock = product.MinimumStock.ToString();
            selectedProductId = product.Id;

            SelectedCategory = Categories.FirstOrDefault(c => c.Id == product.CategoryId);

            SelectedSupplier = Suppliers.FirstOrDefault(s => s.Id == product.SupplierId);
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
                MessageBox.Show($"Error loading categories: {ex.Message}");
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
                    Suppliers.Add(new SupplierDTO(supplier));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading suppliers: {ex.Message}");
            }
        }

        public bool UpdateProduct()
        {
            var updatedProduct = GetUpdatedProduct();
            if (updatedProduct == null)
            {
                MessageBox.Show("Please fill in all required fields.");
                return false;
            }
            else
            {   
                var idDTO = updatedProduct.Id;
                Product p = updatedProduct.ToProduct();
                var id = p.Id;
                _productRepository.UpdateProduct(updatedProduct.ToProduct());
                return true;
            }
        }

        public ProductDTO GetUpdatedProduct()
        {
            if (!IsFormValid)
                return null;

            Product p = new Product
            {
                Id = selectedProductId,
                Name = ProductName?.Trim(),
                ProductCode = ProductCode?.Trim(),
                CategoryId = SelectedCategory?.Id ?? 0,
                SupplierId = SelectedSupplier?.Id ?? 0,
                Quantity = int.Parse(Quantity),
                MinimumStock = int.Parse(MinimumStock)
            };
            return new ProductDTO(p) ;
        }

        private void ValidateForm()
        {
            bool correct = true;

            if (string.IsNullOrWhiteSpace(ProductName))
            {
                correct &= false;
                IsValid = false;
            }
            else if (ProductName.Trim().Length < 2)
            {
                correct &= false;
                IsValid = false;
            }

            if (string.IsNullOrWhiteSpace(ProductCode))
            {
                correct &= false;
                IsValid = false;
            }
            else if (ProductCode.Trim().Length < 2)
            {
                correct &= false;
                IsValid = false;
            }

            if (SelectedCategory == null)
            {
                correct &= false;
                IsValid = false;
            }

            if (SelectedSupplier == null)
            {
                correct &= false;
                IsValid = false;
            }

           if (string.IsNullOrWhiteSpace(Quantity) || !int.TryParse(Quantity, out int quantity) || quantity < 0)
            {
                correct &= false;
                IsValid = false;
            }

            if (string.IsNullOrWhiteSpace(MinimumStock) || !int.TryParse(MinimumStock, out int minimumStock) || minimumStock < 0)
            {
                correct &= false;
                IsValid = false;
            }

            IsFormValid = correct;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
