using SmartInventoryTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventoryTracker.Interfaces
{
    public interface ISupplierRepository
    {
        public void AddSupplier(Supplier supplier);

        public Supplier? GetSupplierById(int id);

        public List<Supplier> GetAllSuppliers();

        public void UpdateSupplier(Supplier supplier);

        public void DeleteSupplier(int id);
    }
}
