using SmartInventoryTracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventoryTracker.DTOs
{
    public class CategoryDTO
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
        public CategoryDTO(Category category)
        { 
            Id = category.Id;
            Name = category.Name;
            Products = new ObservableCollection<ProductDTO>(
                category.Products.Select(p => new ProductDTO(p)));
        }
        public Category ToCategory() 
        { 
            return new Category
            {
                Id = this.Id,
                Name = this.Name,
                Products = this.Products.Select(dto => dto.ToProduct()).ToList()
            };
        }
    }
}
