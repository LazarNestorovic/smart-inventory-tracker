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
    public class RestockProductViewModel : INotifyPropertyChanged
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogRepository _logRepository;
        private ProductDTO selectedProduct;
        private string _restockAmount;
        private bool _isValid = true;

        public RestockProductViewModel(
            IProductRepository productRepository,
            ILogRepository logRepository)
        {
            _productRepository = productRepository;
            _logRepository = logRepository;
        }

        public ProductDTO SelectedProduct
        {
            get => selectedProduct;
            set
            {
                selectedProduct = value;
                OnPropertyChanged();
            }
        }
       
        public string RestockAmount
        {
            get => _restockAmount;
            set
            {
                _restockAmount = value;
                OnPropertyChanged();
                ValidateRestockAmount();
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

        public void LoadData(ProductDTO selectedProduct)
        {
            try
            {
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

            SelectedProduct = product;
        }

        private void ValidateRestockAmount()
        {
            if (string.IsNullOrWhiteSpace(RestockAmount))
            {
                IsValid = false;
                return;
            }
            if (!int.TryParse(RestockAmount, out int number))
            {
                IsValid = false;
                return;
            }
            if (number < 0)
            {
                IsValid = false;
                return;
            }
            if (number > 999999)
            {
                IsValid = false;
                return;
            }
        }

        private void ValidateForm()
        {
            IsValid = true;

            ValidateRestockAmount();
            if (!IsValid) return;

            IsValid = true;
        }

        public bool RestockProduct()
        {
            try
            {
                ValidateForm();
                if (!IsValid)
                {
                    return false;
                }

                SelectedProduct.Quantity += int.Parse(RestockAmount);
                _productRepository.UpdateProduct(SelectedProduct.ToProduct());

                _logRepository.AddLog(new Log
                {
                    Type = Enums.LogType.EntryLog,
                    Amount = int.Parse(RestockAmount),
                    ProductId = SelectedProduct.Id,
                    Product = SelectedProduct.ToProduct(),
                    SupplierId = SelectedProduct.SupplierId,
                    Supplier = SelectedProduct.Supplier?.ToSupplier()
                });

                return true;
            }
            catch (Exception ex)
            {
                IsValid = false;
                return false;
            }
        }

        public void ResetForm()
        {
            _restockAmount = string.Empty;
            IsValid = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
