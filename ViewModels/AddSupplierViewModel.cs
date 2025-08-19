using SmartInventoryTracker.Interfaces;
using SmartInventoryTracker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventoryTracker.ViewModels
{
    public class AddSupplierViewModel : INotifyPropertyChanged
    {
        private readonly ISupplierRepository _supplierRepository;

        private string _supplierName;
        private string _contactInfo;
        private string _email;
        private bool _isValid = true;
        public AddSupplierViewModel(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;

            SupplierName = string.Empty;
            ContactInfo = string.Empty;
            Email = string.Empty;
        }

        public string SupplierName
        {
            get => _supplierName;
            set
            {
                _supplierName = value;
                ValidateCategoryName();
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
                ValidateContactInfo();
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
                ValidateEmail();
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

        private void ValidateCategoryName()
        {
            if (string.IsNullOrWhiteSpace(SupplierName))
            {
                IsValid = false;
                return;
            }

            if (SupplierName.Length < 2)
            {
                IsValid = false;
                return;
            }

            if (SupplierName.Length > 100)
            {
                IsValid = false;
                return;
            }
        }

        private void ValidateContactInfo()
        {
            if (string.IsNullOrWhiteSpace(ContactInfo))
            {
                IsValid = false;
                return;
            }

            if (ContactInfo.Length < 2)
            {
                IsValid = false;
                return;
            }

            if (ContactInfo.Length > 100)
            {
                IsValid = false;
                return;
            }
        }

        private void ValidateEmail()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                IsValid = false;
                return;
            }

            if (Email.Length < 2)
            {
                IsValid = false;
                return;
            }

            if (Email.Length > 100)
            {
                IsValid = false;
                return;
            }
        }

        private void ValidateForm()
        {
            IsValid = true;

            ValidateCategoryName();
            if (!IsValid) return;

            ValidateContactInfo();
            if (!IsValid) return;

            ValidateEmail();
            if (!IsValid) return;

            IsValid = true;
        }

        public bool SaveSupplier()
        {
            try
            {
                ValidateForm();
                if (!IsValid)
                {
                    return false;
                }

                var newSupplier = new Supplier
                {
                    Name = SupplierName.Trim(),
                    ContactInfo = ContactInfo.Trim(),
                    Email = Email.Trim()
                };

                _supplierRepository.AddSupplier(newSupplier);

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
            SupplierName = string.Empty;
            ContactInfo = string.Empty;
            Email = string.Empty;
            IsValid = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
