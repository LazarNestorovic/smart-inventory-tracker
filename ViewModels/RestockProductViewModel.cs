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
        private int _restockAmount;
        private bool _isValid = true;

        public RestockProductViewModel(
            IProductRepository productRepository,
            ILogRepository logRepository)
        {
            _productRepository = productRepository;
            _logRepository = logRepository;
            _restockAmount = 0;
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
       
        public int RestockAmount
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
            if (RestockAmount < 0)
            {
                IsValid = false;
                return;
            }

            if (RestockAmount > 999999)
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

                SelectedProduct.Quantity += RestockAmount;
                _productRepository.UpdateProduct(SelectedProduct.ToProduct());

                _logRepository.AddLog(new Log
                {
                    Type = Enums.LogType.EntryLog,
                    Amount = RestockAmount,
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
            _restockAmount = 0;
            IsValid = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
