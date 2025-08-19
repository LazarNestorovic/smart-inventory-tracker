using SmartInventoryTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventoryTracker.Interfaces
{
    public interface IProductRepository
    {
        public void AddProduct(Product product);

        public Product? GetProductById(int id);

        public List<Product> GetAllProducts();

        public void UpdateProduct(Product product);

        public void DeleteProduct(int id);

        public Product? GetDeletedProductByCode(string trimmedCode);
    }
}
