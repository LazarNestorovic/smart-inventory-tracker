using QRCoder;
using SmartInventoryTracker.DTOs;
using SmartInventoryTracker.Interfaces;
using SmartInventoryTracker.Models;
using SmartInventoryTracker.Views.WorkerViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Core.Mapping;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SmartInventoryTracker.ViewModels
{
    public class WorkersMainWindowViewModel : INotifyPropertyChanged
    {

        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly ILogRepository _logRepository;
        private readonly IUserRepository _userRepository;
        public ObservableCollection<ProductDTO> Products { get; set; }
        public ObservableCollection<CategoryDTO> Categories { get; set; }
        public ObservableCollection<SupplierDTO> Suppliers { get; set; }
        public ObservableCollection<UserDTO> Users { get; set; }
        public ObservableCollection<LogDTO> Logs { get; set; }

        private ObservableCollection<ProductDTO> _allProducts;
        private ObservableCollection<CategoryDTO> _allCategories;
        private ObservableCollection<SupplierDTO> _allSuppliers;
        private ObservableCollection<UserDTO> _allUsers;
        private ObservableCollection<LogDTO> _allLogs;

        private string _selectedFilterCategory = "All";
        private string _selectedFilterSupplier = "All";
        private string _selectedFilterLog = "All";
        private string _selectedFilterLogSupplier = "All";
        private string _selectedFilterUser = "All";
        private string _searchQueary = "";

        public WorkersMainWindowViewModel( ILogRepository logRepository, IUserRepository userRepository, IProductRepository productRepository, ICategoryRepository categoryRepository, ISupplierRepository supplierRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _supplierRepository = supplierRepository;
            _logRepository = logRepository;
            _userRepository = userRepository;

            Products = new ObservableCollection<ProductDTO>();
            Categories = new ObservableCollection<CategoryDTO>();
            Suppliers = new ObservableCollection<SupplierDTO>();
            Users = new ObservableCollection<UserDTO>();
            Logs = new ObservableCollection<LogDTO>();

            _allProducts = new ObservableCollection<ProductDTO>();
            _allCategories = new ObservableCollection<CategoryDTO>();
            _allSuppliers = new ObservableCollection<SupplierDTO>();
            _allUsers = new ObservableCollection<UserDTO>();
            _allLogs = new ObservableCollection<LogDTO>();
        }

        public string SearchQuery
        {
            get => _searchQueary;
            set
            {
                _searchQueary = value;
                OnPropertyChanged();
                ResetProducts();
            }
        }

        public string SelectedFilterCategory
        {
            get => _selectedFilterCategory;
            set
            {
                _selectedFilterCategory = value;
                OnPropertyChanged(); 
                FilterProducts();
            }
        }

        public string SelectedFilterLog
        {
            get => _selectedFilterLog;
            set
            {
                _selectedFilterLog = value;
                OnPropertyChanged();
                FilterLogs();
            }
        }

        public string SelectedFilterUser
        {
            get => _selectedFilterUser;
            set
            {
                _selectedFilterUser = value;
                OnPropertyChanged();
                FilterUsers();
            }
        }


        public string SelectedFilterLogSupplier
        {
            get => _selectedFilterLogSupplier;
            set
            {
                _selectedFilterLogSupplier = value;
                OnPropertyChanged();
                FilterLogs();
            }
        }

        public string SelectedFilterSupplier
        {
            get => _selectedFilterSupplier;
            set
            {
                _selectedFilterSupplier = value;
                OnPropertyChanged();
                FilterProducts();
            }
        }

        public int GetLowStockProductsNumber ()
        {
            return _allProducts.Count(p => p.Quantity <= p.MinimumStock && !p.Deleted);
        }

        public void GenerateQRCode(ProductDTO selectedProduct)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(selectedProduct.ProductCode, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);

            Bitmap qrCodeImage = qrCode.GetGraphic(50);
            QRCodeWindow window = new QRCodeWindow(qrCodeImage);
            window.ShowDialog();
        }

        public async Task Update()
        {
            try
            {

                LoadCategories();
                LoadSuppliers();
                LoadProducts();
                LoadUsers();
                LoadLogs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ObservableCollection<string> GetSupplierFilterOptions()
        {
            var options = new ObservableCollection<string> { "All" };
            var uniqueSuppliers = _allSuppliers.Select(s => s.Name).OrderBy(name => name);

            foreach (var supplier in uniqueSuppliers)
            {
                if (!string.IsNullOrEmpty(supplier))
                {
                    options.Add(supplier);
                }
            }

            return options;
        }

        public ObservableCollection<string> GetCategoryFilterOptions()
        {
            var options = new ObservableCollection<string> { "All" };
            var uniqueCategory = _allCategories.Select(s => s.Name).OrderBy(name => name);

            foreach (var category in uniqueCategory)
            {
                if (!string.IsNullOrEmpty(category))
                {
                    options.Add(category);
                }
            }

            return options;
        }

        public ObservableCollection<string> GetLogFilterOptions()
        {
            var options = new ObservableCollection<string> { "All", "EntryLog", "ExitLog" };

            return options;
        }

        public ObservableCollection<string> GetUserFilterOptions()
        {
            var options = new ObservableCollection<string> { "All", "Administrator", "Worker" };

            return options;
        }

        public void FilterLowStock() 
        {
            var filteredProducts = _allProducts.Where(p => p.Quantity <= p.MinimumStock && !p.Deleted);

            Products.Clear();
            foreach (var product in filteredProducts)
            {
                Products.Add(product);
            }
        }

        private void LoadLogs()
        {
            try
            {
                var logs = _logRepository.GetAllLogs();

                _allLogs.Clear();
                foreach (var log in logs)
                {
                    log.Product = _productRepository.GetProductById(log.ProductId);
                    log.Supplier = _supplierRepository.GetSupplierById(log.SupplierId);
                    _allLogs.Add(new LogDTO(log));
                }
                FilterLogs();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load products: {ex.Message}", ex);
            }
        }

        public void FilterLogs()
        {
            var filteredLogs = _allLogs.AsEnumerable();

            if (!string.IsNullOrEmpty(SelectedFilterLog) && SelectedFilterLog != "All")
            {
                filteredLogs = filteredLogs.Where(l =>
                l.Type.ToString().Equals(SelectedFilterLog, StringComparison.OrdinalIgnoreCase) == true);
            }

            if (!string.IsNullOrEmpty(SelectedFilterLogSupplier) && SelectedFilterLogSupplier != "All")
            {
                filteredLogs = filteredLogs.Where(p =>
                    p.Supplier?.Name?.Equals(SelectedFilterLogSupplier, StringComparison.OrdinalIgnoreCase) == true);
            }

            if (!string.IsNullOrEmpty(SearchQuery))
            {
                filteredLogs = filteredLogs.Where(p =>
                    p.Product.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase));
            }

            Logs.Clear();
            foreach (var log in filteredLogs)
            {
                Logs.Add(log);
            }
        }

        private void LoadUsers()
        {
            try
            {
                var users = _userRepository.GetAllUsers();

                _allUsers.Clear();
                foreach (var user in users)
                {
                    _allUsers.Add(new UserDTO(user));
                }
                FilterUsers();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load products: {ex.Message}", ex);
            }
        }

        public void FilterUsers()
        {
            var filteredUsers = _allUsers.AsEnumerable();

            if (!string.IsNullOrEmpty(SelectedFilterUser) && SelectedFilterUser != "All")
            {
                filteredUsers = filteredUsers.Where(u =>
                u.Role.ToString().Equals(SelectedFilterUser, StringComparison.OrdinalIgnoreCase) == true);
            }

            if (!string.IsNullOrEmpty(SearchQuery))
            {
                filteredUsers = filteredUsers.Where(p =>
                    p.Username.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase));
            }

            Users.Clear();
            foreach (var user in filteredUsers)
            {
                Users.Add(user);
            }
        }


        private void ResetProducts() {
            if (string.IsNullOrEmpty(SearchQuery)) {
                LoadCategories();
                LoadSuppliers();
                LoadProducts();
                LoadUsers();
                LoadLogs();
            }
        }

        private void LoadProducts()
        {
            try
            {
                var products = _productRepository.GetAllProducts();

                _allProducts.Clear();
                foreach (var product in products)
                {
                    if (product.Deleted)
                    {
                        continue;
                    }
                    product.Category = _categoryRepository.GetCategoryById(product.CategoryId);
                    product.Supplier = _supplierRepository.GetSupplierById(product.SupplierId);
                    ProductDTO productDTO = new ProductDTO(product);
                    LoadStatus(productDTO);
                    _allProducts.Add(productDTO);
                }

                FilterProducts();   
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load products: {ex.Message}", ex);
            }
        }

        public void FilterProducts()
        {
            var filteredProducts = _allProducts.AsEnumerable();

            if (!string.IsNullOrEmpty(SelectedFilterCategory) && SelectedFilterCategory != "All")
            {
                    filteredProducts = filteredProducts.Where(p =>
                    p.Category?.Name?.Equals(SelectedFilterCategory, StringComparison.OrdinalIgnoreCase) == true);
            }

            if (!string.IsNullOrEmpty(SelectedFilterSupplier) && SelectedFilterSupplier != "All")
            {
                filteredProducts = filteredProducts.Where(p =>
                    p.Supplier?.Name?.Equals(SelectedFilterSupplier, StringComparison.OrdinalIgnoreCase) == true);
            }

            if (!string.IsNullOrEmpty(SearchQuery))
            {
                filteredProducts = filteredProducts.Where(p =>
                    p.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase));
            }

            Products.Clear();
            foreach (var product in filteredProducts)
            {
                Products.Add(product);
            }
        }

        private void LoadStatus(ProductDTO product)
        {
            if (product.Quantity > product.MinimumStock)
            {
                product.Status = "In Stock";
            }
            else if (product.Quantity == 0)
            { 
                product.Status = "Out of Stock";
            }
            else if (product.Quantity <= product.MinimumStock)
            {
                product.Status = "Low Stock";
            }
        }

        private void LoadCategories()
        {
            try
            {
                var categories = _categoryRepository.GetAllCategories();
                var products = _productRepository.GetAllProducts();

                _allCategories.Clear();
                foreach (var category in categories)
                {
                    foreach (var product in products)
                    {
                        if ( !product.Deleted && product.CategoryId == category.Id)
                        { 
                            category.Products.Add(product);
                        }
                    }
                    if (category.Id != 2)
                    {
                        _allCategories.Add(new CategoryDTO(category));
                    }
                }
                FilterCategories();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load categories: {ex.Message}", ex);
            }
        }

        public void FilterCategories()
        {
            var filteredCategories = _allCategories.AsEnumerable();

            if (!string.IsNullOrEmpty(SelectedFilterSupplier) && SelectedFilterSupplier != "All")
            {
                filteredCategories = filteredCategories.Where(c =>
                    c.Products.Any(p => p.Category?.Name?.Equals(SelectedFilterCategory, StringComparison.OrdinalIgnoreCase) == true));
            }

            if (!string.IsNullOrEmpty(SearchQuery))
            {
                filteredCategories = filteredCategories.Where(p =>
                    p.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase));
            }

            Categories.Clear();
            foreach (var category in filteredCategories)
            {
                Categories.Add(category);
            }
        }

        private void LoadSuppliers()
        {
            try
            {
                var suppliers = _supplierRepository.GetAllSuppliers();
                var products = _productRepository.GetAllProducts();

                _allSuppliers.Clear();
                foreach (var supplier in suppliers)
                {
                    foreach (var product in products)
                    {
                        if (!product.Deleted && product.SupplierId == supplier.Id)
                        {
                            supplier.Products.Add(product);
                        }
                    }
                    _allSuppliers.Add(new SupplierDTO(supplier));
                }

                FilterSuppliers();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load suppliers: {ex.Message}", ex);
            }
        }
        public void FilterSuppliers()
        {
            var filteredSuppliers = _allSuppliers.AsEnumerable();

            if (!string.IsNullOrEmpty(SelectedFilterSupplier) && SelectedFilterSupplier != "All")
            {
                filteredSuppliers = filteredSuppliers.Where(s =>
                    s.Name?.Equals(SelectedFilterSupplier, StringComparison.OrdinalIgnoreCase) == true);
            }

            if (!string.IsNullOrEmpty(SearchQuery))
            {
                filteredSuppliers = filteredSuppliers.Where(p =>
                    p.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase));
            }

            Suppliers.Clear();
            foreach (var supplier in filteredSuppliers)
            {
                Suppliers.Add(supplier);
            }
        }

        public void DeleteProduct(ProductDTO p)
        {
            _productRepository.DeleteProduct(p.Id);
            foreach (SupplierDTO s in Suppliers)
            {
                if (s.Id == p.SupplierId)
                {
                    s.Products.Remove(p);    
                }
            }

            foreach (CategoryDTO c in Categories)
            {
                if (c.Id == p.SupplierId)
                {
                    c.Products.Remove(p);
                }
            }
        }

        public void SoftDeleteProduct(ProductDTO product)
        {
            product.Deleted = true;
            _productRepository.UpdateProduct(product.ToProduct());
            foreach (SupplierDTO s in Suppliers)
            {
                if (s.Id == product.SupplierId)
                {
                    s.Products.Remove(product);
                }
            }

            foreach (CategoryDTO c in Categories)
            {
                if (c.Id == product.SupplierId)
                {
                    c.Products.Remove(product);
                }
            }
        }

        public void DeleteCategory(CategoryDTO category)
        {
            if (category.Products.Any())
            {
                var uncategorizedCategory = _categoryRepository.GetUncategorizedCategory();

                var result = MessageBox.Show(
                    $"The category '{category.Name}' contains {category.Products.Count} products. " +
                    $"Do you want to move the products to the '{uncategorizedCategory.Name}' category and delete this category?",
                    "Move Products",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.Yes)
                {
                    MoveProductsToCategory(category.Id, uncategorizedCategory.Id);
                    _categoryRepository.DeleteCategory(category.Id);
                }
                else if (result == MessageBoxResult.No)
                {
                    return;
                }
            }
            else
            {
                _categoryRepository.DeleteCategory(category.Id);
            }
        }
        public void MoveProductsToCategory(int categoryId, int uncategorizedCategoryId)
        {
            var products = _productRepository.GetAllProducts();
            foreach (var product in products)
            {
                if (product.CategoryId == categoryId)
                {
                    product.CategoryId = uncategorizedCategoryId;
                    _productRepository.UpdateProduct(product);
                }
            }
        }

        public bool DeleteSupplier(SupplierDTO supplier) 
        {
            var activeProducts = supplier.Products.Where(p => p.Quantity > 0).ToList();

            if (activeProducts.Any())
            {
                MessageBox.Show(
                    $"You cannot delete supplier '{supplier.Name}' because there are {activeProducts.Count} " +
                    $"products with stock available.\n\n" +
                    $"You need to:\n" +
                    $"1. Sell or move all products\n" +
                    $"2. Or change the supplier for those products\n",
                    "Supplier In Use",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );

                return false;
            }
            else 
            {
                try
                {
                    _supplierRepository.DeleteSupplier(supplier.Id);
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error while deleting supplier: {ex.Message}",
                                     "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    return false;
                }
            }
        }

        public bool DeleteUser(UserDTO user)
        {
            if (user.Role == Enums.UserRoles.Worker)
            {
                _userRepository.DeleteUser(user.Id);
                return true;
            }
            else
            {
                MessageBox.Show(
                    "You cannot delete a user with an administrator role.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );

                return false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
