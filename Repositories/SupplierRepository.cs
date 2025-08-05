using SmartInventoryTracker.Models;
using SmartInventoryTracker.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventoryTracker.Repositories
{
    public class SupplierRepository
    {
        private readonly SQLiteSerializable<Supplier> _serializer;
        public SupplierRepository(string connectionString)
        {
            _serializer = new SQLiteSerializable<Supplier>(connectionString);
        }

        public void AddSupplier(Supplier supplier)
        {
            _serializer.Insert(new List<Supplier> { supplier });
        }

        public Supplier? GetSupplierById(int id)
        {
            return _serializer.GetById(id);
        }

        public List<Supplier> GetAllSuppliers()
        {
            return _serializer.GetAll();
        }

        public void UpdateSupplier(Supplier supplier)
        {
            _serializer.Update(supplier);
        }

        public void DeleteSupplier(int id)
        {
            _serializer.Delete(id);
        }
    }
}
