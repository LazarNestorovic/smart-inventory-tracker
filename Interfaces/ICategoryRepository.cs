using SmartInventoryTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventoryTracker.Interfaces
{
    public interface ICategoryRepository
    {
        public void AddCategory(Category category);

        public Category? GetCategoryById(int id);

        public List<Category> GetAllCategories();

        public void UpdateCategory(Category category);

        public void DeleteCategory(int id);

        public Category GetUncategorizedCategory();
    }
}
