using SmartInventoryTracker.Models;
using SmartInventoryTracker.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventoryTracker.Repositories
{
    public class ProductRepository
    {
        private readonly SQLiteSerializable<Product> _serializer;
        public ProductRepository(string connectionString)
        {
            _serializer = new SQLiteSerializable<Product>(connectionString);
        }

        public void AddProduct(Product product)
        {
            _serializer.Insert(new List<Product> { product });
        }

        public Product? GetProductById(int id)
        {
            return _serializer.GetById(id);
        }

        public List<Product> GetAllProducts()
        {
            return _serializer.GetAll();
        }

        public void UpdateProduct(Product product)
        {
            _serializer.Update(product);
        }

        public void DeleteProduct(int id)
        {
            _serializer.Delete(id);
        }
    }
}
