using SmartInventoryTracker.Enums;
using SmartInventoryTracker.Serializer;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventoryTracker.Models
{
    public class Log : ISQLiteSerializable
    {
        public int Id { get; set; }
        public LogType Type { get; set; }
        public int Amount { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        public string TableName => "Logs";

        public Dictionary<string, object> ToSqlParams()
        {
            return new Dictionary<string, object>
            {
                { "Id", Id },
                { "Type", (int)Type },
                { "Amount", Amount },
                { "ProductId", ProductId },
                { "SupplierId", SupplierId }
            };
        }

        public void FromSqlReader(SQLiteDataReader reader)
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id"));
            Type = (LogType)reader.GetInt32(reader.GetOrdinal("Type"));
            Amount = reader.GetInt32(reader.GetOrdinal("Amount"));
            ProductId = reader.GetInt32(reader.GetOrdinal("ProductId"));
            SupplierId = reader.GetInt32(reader.GetOrdinal("SupplierId"));
        }

    }
}
