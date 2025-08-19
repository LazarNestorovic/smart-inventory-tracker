using SmartInventoryTracker.DTOs;
using SmartInventoryTracker.Interfaces;
using SmartInventoryTracker.Models;
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
    public class EditSupplierViewModel
    {
        private int selectedSupplierId;
        private string _supplierName;
        private string _contactInfo;
        private string _email;
        private SupplierDTO _originalSupplier;

        public string SupplierName
        {
            get => _supplierName;
            set
            {
                _supplierName = value;
                ValidateForm();
                OnPropertyChanged();
            }
        }
        public string ContactInfo
        {
            get => _contactInfo;
            set
            {
                _contactInfo = value;
                ValidateForm();
                OnPropertyChanged();
            }
        }
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                ValidateForm();
                OnPropertyChanged();
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

        private readonly ISupplierRepository _supplierRepository;

        public EditSupplierViewModel(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public void LoadData(SupplierDTO selectedSupplier)
        {
            try
            {
                LoadSupplierData(selectedSupplier);
            }
            catch (Exception ex)
            {
                IsValid = false;
            }
        }

        private void LoadSupplierData(SupplierDTO supplier)
        {
            if (supplier == null) return;

            selectedSupplierId = supplier.Id;
            SupplierName = supplier.Name;
            ContactInfo = supplier.ContactInfo;
            Email= supplier.Email;
            _originalSupplier = supplier;
        }

        public bool UpdateSupplier()
        {
            var updatedSupplier = GetUpdatedSupplier();
            if (updatedSupplier == null)
            {
                MessageBox.Show("Please fill in all required fields.");
                ResetForm();
                return false;
            }
            else
            {
                var idDTO = updatedSupplier.Id;
                Supplier p = updatedSupplier.ToSupplier();
                var id = p.Id;
                _supplierRepository.UpdateSupplier(updatedSupplier.ToSupplier());
                return true;
            }
        }

        public SupplierDTO GetUpdatedSupplier()
        {
            if (!IsFormValid)
                return null;

            Supplier s = new Supplier
            {
                Id = selectedSupplierId,
                Name = SupplierName?.Trim(),
                ContactInfo = ContactInfo?.Trim(),
                Email = Email?.Trim()
            };
            return new SupplierDTO(s);
        }

        private void ValidateForm()
        {
            bool correct = true;

            if (string.IsNullOrWhiteSpace(SupplierName))
            {
                correct &= false;
                IsValid = false;
            }
            else if (SupplierName.Trim().Length < 2)
            {
                correct &= false;
                IsValid = false;
            }

            if (string.IsNullOrWhiteSpace(ContactInfo))
            {
                correct &= false;
                IsValid = false;
            }
            else if (ContactInfo.Trim().Length < 2)
            {
                correct &= false;
                IsValid = false;
            }

            if (string.IsNullOrWhiteSpace(Email))
            {
                correct &= false;
                IsValid = false;
            }
            else if (Email.Trim().Length < 2)
            {
                correct &= false;
                IsValid = false;
            }

            IsFormValid = correct;
        }

        public void ResetForm()
        {
            SupplierName = _originalSupplier.Name;
            ContactInfo = _originalSupplier.ContactInfo;
            Email = _originalSupplier.Email;
            IsValid = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
