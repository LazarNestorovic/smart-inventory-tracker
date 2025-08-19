using SmartInventoryTracker.DTOs;
using SmartInventoryTracker.Enums;
using SmartInventoryTracker.Interfaces;
using SmartInventoryTracker.Models;
using SmartInventoryTracker.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SmartInventoryTracker.ViewModels
{

    public class AddLogViewModel : INotifyPropertyChanged
    {
        private readonly ILogRepository _logRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IProductRepository _productRepository;

        private string _selectedType;
        private ProductDTO _selectedProduct;
        private int _amount;
        private SupplierDTO _selectedSupplier;
        public List<string> LogTypes { get; set; }
        public List<ProductDTO> Products { get; set; }
        public List<SupplierDTO> Suppliers { get; set; }
        private bool _isValid = true;

        public AddLogViewModel(ILogRepository logRepository,
            ISupplierRepository supplierRepository,
            IProductRepository productRepository)
        {
            _logRepository = logRepository;
            _supplierRepository = supplierRepository;
            _productRepository = productRepository;

            Suppliers = new List<SupplierDTO>();
            Products = new List<ProductDTO>();

            Amount = 0;
            SelectedType = LogType.EntryLog.ToString();
            LoadData();
            _productRepository = productRepository;
        }

        public string SelectedType
        {
            get => _selectedType;
            set
            {
                _selectedType = value;
                ValidateForm();
                OnPropertyChanged();
            }
        }
        public int Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                ValidateAmount();
                ValidateForm();
                OnPropertyChanged();
            }
        }
        public ProductDTO SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                ValidateProduct();
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
                if (_selectedSupplier != null)
                {
                    LoadProducts();
                }
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
                LoadTypes();
                LoadSuppliers();
            }
            catch (Exception ex)
            {
                IsValid = false;
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

        private void LoadProducts()
        {
            try
            {
                if(SelectedSupplier == null)
                {
                    Products.Clear();
                    MessageBox.Show("Please select a supplier first.");
                    return;
                }
                var products = _productRepository.GetAllProducts();

                Products.Clear();
                foreach (var product in products)
                {
                    if (product.SupplierId == SelectedSupplier.Id)
                    {
                        Products.Add(new ProductDTO(product));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading suppliers: {ex.Message}");
            }
        }


        private void LoadTypes()
        {
            LogTypes = new List<string> { "EntryLog", "ExitLog" };
        }

        private void ValidateSupplier()
        {
            if (SelectedSupplier == null)
            {
                IsValid = false;
                return;
            }
        }

        private void ValidateProduct()
        {
            if (SelectedProduct == null)
            {
                IsValid = false;
                return;
            }
        }

        private void ValidateAmount()
        {
            if (Amount < 0)
            {
                IsValid = false;
                return;
            }

            if (Amount > 99999)
            {
                IsValid = false;
                return;
            }
        }

        private void ValidateForm()
        {
            IsValid = true;

            ValidateSupplier();
            if (!IsValid) return;

            ValidateProduct();
            if (!IsValid) return;

            ValidateAmount();
            if (!IsValid) return;

            CanShipQuantity();
            if (!IsValid) return;

            IsValid = true;
        }

        private void CanShipQuantity()
        { 
            if(SelectedType == "ExitLog" && Amount > SelectedProduct.Quantity)
            {
                IsValid = false;
                MessageBox.Show($"Cannot ship more than available quantity. Quantity of selected product: {SelectedProduct.Quantity}");
                return;
            }
        }

        public bool SaveLog()
        {
            try
            {
                ValidateForm();
                if (!IsValid)
                {
                    return false;
                }

                UpdateProductQuantity();

                Log newLog = new Log();

                if (SelectedType == "EntryLog")
                {
                    newLog.Type = LogType.EntryLog;
                }
                else if (SelectedType == "ExitLog")
                {
                    newLog.Type = LogType.ExitLog;
                }
                newLog.Amount = Amount;
                newLog.Product = SelectedProduct.ToProduct();
                newLog.ProductId = SelectedProduct.Id;
                newLog.Supplier = SelectedSupplier.ToSupplier();
                newLog.SupplierId = SelectedSupplier.Id;

                _logRepository.AddLog(newLog);

                return true;
            }
            catch (Exception ex)
            {
                IsValid = false;
                return false;
            }
        }

        private void UpdateProductQuantity()
        {
            if (SelectedType == "EntryLog")
            {
                SelectedProduct.Quantity += Amount;
            }
            else if (SelectedType == "ExitLog")
            {
                SelectedProduct.Quantity -= Amount;
            }
            _productRepository.UpdateProduct(SelectedProduct.ToProduct());
        }
        public void ResetForm()
        {

            Amount = 0;
            SelectedType = LogType.EntryLog.ToString();
            SelectedProduct = null;
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
