using SmartInventoryTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventoryTracker.DTOs
{
    public class ProductDTO
    {
        private int id;
        public int Id
        {
            get => id;
            set
            {
                if (value != id)
                {
                    id = value;
                }
            }
        }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                if (value != name)
                {
                    name = value;
                }
            }
        }
        private string productCode;
        public string ProductCode
        {
            get => productCode;
            set
            {
                if (value != productCode)
                {
                    productCode = value;
                }
            }
        }
        private int minimumStock;
        public int MinimumStock
        {
            get => minimumStock;
            set
            {
                if (value != minimumStock)
                {
                    minimumStock = value;
                }
            }
        }
        private int quantity;
        public int Quantity
        {
            get => quantity;
            set
            {
                if (value != quantity)
                {
                    quantity = value;
                }
            }
        }

        private int categoryId;
        public int CategoryId
        {
            get => categoryId;
            set
            {
                if (value != categoryId)
                {
                    categoryId = value;
                }
            }
        }
        private CategoryDTO? category;
        public CategoryDTO? Category
        {
            get => category;
            set
            {
                if (value != category)
                {
                    category = value;
                }
            }
        }
        private int supplierId;
        public int SupplierId
        {
            get => supplierId;
            set
            {
                if (value != supplierId)
                {
                    supplierId = value;
                }
            }
        }
        private SupplierDTO? supplier;
        public SupplierDTO? Supplier
        {
            get => supplier;
            set
            {
                if (value != supplier)
                {
                    supplier = value;
                }
            }
        }

        private bool deleted;
        public bool Deleted
        {
            get => deleted;
            set
            {
                if (value != deleted)
                {
                    deleted = value;
                }
            }
        }

        private string status;
        public string Status
        {
            get => status;
            set
            {
                if (value != status)
                {
                    status = value;
                }
            }
        }
        public ProductDTO(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            ProductCode = product.ProductCode;
            MinimumStock = product.MinimumStock;
            Quantity = product.Quantity;
            CategoryId = product.CategoryId;
            Category = product.Category != null ? new CategoryDTO(product.Category) : null;
            SupplierId = product.SupplierId;
            Supplier = product.Supplier != null ? new SupplierDTO(product.Supplier) : null;
            Deleted = product.Deleted;
        }
        public Product ToProduct()
        {
            return new Product
            {
                Id = Id,
                Name = Name,
                ProductCode = ProductCode,
                MinimumStock = MinimumStock,
                Quantity = Quantity,
                CategoryId = CategoryId,
                Category = Category != null ? Category.ToCategory() : null,
                SupplierId = SupplierId,
                Supplier = Supplier != null ? Supplier.ToSupplier() : null,
                Deleted = Deleted
            };
        }
    }
}
