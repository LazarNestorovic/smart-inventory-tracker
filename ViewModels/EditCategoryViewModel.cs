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
    public class EditCategoryViewModel
    {
        private int selectedCategoryId;
        private string _categoryName;
        private CategoryDTO _originalCategory;

        public string CategoryName
        {
            get => _categoryName;
            set
            {
                _categoryName = value;
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

        private readonly ICategoryRepository _categoryRepository;

        public EditCategoryViewModel( ICategoryRepository categoryRepository )
        {
            _categoryRepository = categoryRepository;
        }

        public void LoadData(CategoryDTO selectedCategory)
        {
            try
            {
                LoadCategoryData(selectedCategory);
            }
            catch (Exception ex)
            {
                IsValid = false;
            }
        }

        private void LoadCategoryData(CategoryDTO category)
        {
            if (category == null) return;

            CategoryName = category.Name;
            selectedCategoryId = category.Id;
            _originalCategory = category;
        }

        public bool UpdateCategory()
        {
            var updatedCategory = GetUpdatedCategory();
            if (updatedCategory == null)
            {
                MessageBox.Show("Please fill in all required fields.");
                ResetForm();
                return false;
            }
            else
            {
                var idDTO = updatedCategory.Id;
                Category p = updatedCategory.ToCategory();
                var id = p.Id;
                _categoryRepository.UpdateCategory(updatedCategory.ToCategory());
                return true;
            }
        }

        public CategoryDTO GetUpdatedCategory()
        {
            if (!IsFormValid)
                return null;

            Category c = new Category
            {
                Id = selectedCategoryId,
                Name = CategoryName?.Trim()
            };
            return new CategoryDTO(c);
        }

        private void ValidateForm()
        {
            bool correct = true;

            if (string.IsNullOrWhiteSpace(CategoryName))
            {
                correct &= false;
                IsValid = false;
            }
            else if (CategoryName.Trim().Length < 2)
            {
                correct &= false;
                IsValid = false;
            }

            IsFormValid = correct;
        }

        public void ResetForm()
        {
            CategoryName = _originalCategory.Name;
            IsValid = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
