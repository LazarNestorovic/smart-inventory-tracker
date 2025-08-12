using SmartInventoryTracker.Enums;
using SmartInventoryTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventoryTracker.DTOs
{
    public class LogDTO
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
        private LogType type;
        public LogType Type
        {
            get => type;
            set
            {
                if (value != type)
                {
                    type = value;
                }
            }
        }
        private int amount;
        public int Amount
        {
            get => amount;
            set
            {
                if (value != amount)
                {
                    amount = value;
                }
            }
        }
        private int productId;
        public int ProductId
        {
            get => productId;
            set
            {
                if (value != productId)
                {
                    productId = value;
                }
            }
        }
        private ProductDTO? product;
        public ProductDTO? Product
        {
            get => product;
            set
            {
                if (value != product)
                {
                    product = value;
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
        public LogDTO(Log log)
        {
            Id = log.Id;
            Type = log.Type;
            Amount = log.Amount;
            ProductId = log.ProductId;
            Product = log.Product != null ? new ProductDTO(log.Product) : null;
            SupplierId = log.SupplierId;
            Supplier = log.Supplier != null ? new SupplierDTO(log.Supplier) : null;
        }
        public Log ToLog()
        {
            return new Log
            {
                Id = Id,
                Type = Type,
                Amount = Amount,
                ProductId = ProductId,
                Product = Product != null ? Product.ToProduct() : null,
                SupplierId = SupplierId,
                Supplier = Supplier != null ? Supplier.ToSupplier() : null
            };
        }
    }
}
