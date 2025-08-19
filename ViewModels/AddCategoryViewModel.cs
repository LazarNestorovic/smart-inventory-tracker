using SmartInventoryTracker.DTOs;
using SmartInventoryTracker.Interfaces;
using SmartInventoryTracker.Models;
using SmartInventoryTracker.Repositories;
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
    public class AddCategoryViewModel : INotifyPropertyChanged
    {
        private readonly ICategoryRepository _categoryRepository;

        private string _categoryName;
        private bool _isValid = true;
        public AddCategoryViewModel( ICategoryRepository categoryRepository )
        {
            _categoryRepository = categoryRepository;

            CategoryName = string.Empty;
        }

        public string CategoryName
        {
            get => _categoryName;
            set
            {
                _categoryName = value;
                ValidateCategoryName();
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
            if (string.IsNullOrWhiteSpace(CategoryName))
            {
                IsValid = false;
                return;
            }

            if (CategoryName.Length < 2)
            {
                IsValid = false;
                return;
            }

            if (CategoryName.Length > 100)
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

            IsValid = true;
        }

        public bool SaveCategory()
        {
            try
            {
                ValidateForm();
                if (!IsValid)
                {
                    return false;
                }

                var newCategory = new Category
                {
                    Name = CategoryName.Trim()
                };

                _categoryRepository.AddCategory(newCategory);

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
            CategoryName = string.Empty;
            IsValid = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
