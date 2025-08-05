using SmartInventoryTracker.Serializer;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventoryTracker.Models
{
    public class Product : ISQLiteSerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProductCode { get; set; }
        public int MinimumStock { get; set; }
        public int Quantity { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        public string TableName => "Products";

        public Dictionary<string, object> ToSqlParams()
        {
            return new Dictionary<string, object>
            {
                { "Id", Id },
                { "Name", Name },
                { "ProductCode", ProductCode },
                { "MinimumStock", MinimumStock },
                { "Quantity", Quantity },
                { "CategoryId", CategoryId },
                { "SupplierId", SupplierId }
            };
        }

        public void FromSqlReader(SQLiteDataReader reader)
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id"));
            Name = reader.GetString(reader.GetOrdinal("Name"));
            ProductCode = reader.GetString(reader.GetOrdinal("ProductCode"));
            MinimumStock = reader.GetInt32(reader.GetOrdinal("MinimumStock"));
            Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"));
            CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId"));
            SupplierId = reader.GetInt32(reader.GetOrdinal("SupplierId"));
        }
    }
}
