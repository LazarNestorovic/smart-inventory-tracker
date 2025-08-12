using SmartInventoryTracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventoryTracker.DTOs
{
    public class SupplierDTO
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
        private string contactInfo;
        public string ContactInfo
        {
            get => contactInfo;
            set
            {
                if (value != contactInfo)
                {
                    contactInfo = value;
                }
            }
        }
        private string email;
        public string Email
        {
            get => email;
            set
            {
                if (value != email)
                {
                    email = value;
                }
            }
        }
        private ObservableCollection<ProductDTO> products;
        public ObservableCollection<ProductDTO> Products
        {
            get => products;
            set
            {
                if (value != products)
                {
                    products = value;
                }
            }
        }
        public SupplierDTO(Supplier supplier)
        {
            Id = supplier.Id;
            Name = supplier.Name;
            ContactInfo = supplier.ContactInfo;
            Email = supplier.Email;
            Products = new ObservableCollection<ProductDTO>(
                supplier.Products.Select(p => new ProductDTO(p)));
        }

        public Supplier ToSupplier()
        {
            return new Supplier
            {
                Id = this.Id,
                Name = this.Name,
                ContactInfo = this.ContactInfo,
                Email = this.Email,
                Products = this.Products.Select(dto => dto.ToProduct()).ToList()
            };
        }
    }
}
