using SmartInventoryTracker.Interfaces;
using SmartInventoryTracker.Models;
using SmartInventoryTracker.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventoryTracker.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly SQLiteSerializable<Category> _serializer;
        private const int uncategorizedId = 2;
        public CategoryRepository(string connectionString)
        {
            _serializer = new SQLiteSerializable<Category>(connectionString);
        }

        public void AddCategory(Category category)
        {
            _serializer.Insert(new List<Category> { category });
        }

        public Category? GetCategoryById(int id)
        {
            return _serializer.GetById(id);
        }

        public List<Category> GetAllCategories()
        {
            return _serializer.GetAll();
        }

        public void UpdateCategory(Category category)
        {
            _serializer.Update(category);
        }

        public void DeleteCategory(int id)
        {
            _serializer.Delete(id);
        }

        public Category GetUncategorizedCategory()
        {
            return GetCategoryById(uncategorizedId);
        }
    }
}
