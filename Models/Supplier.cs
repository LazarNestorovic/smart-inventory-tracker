using SmartInventoryTracker.Serializer;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventoryTracker.Models
{
    public class Supplier : ISQLiteSerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactInfo { get; set; }
        public string Email { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();

        public string TableName => "Suppliers";

        public Dictionary<string, object> ToSqlParams()
        {
            return new Dictionary<string, object>
            {
                { "Id", Id },
                { "Name", Name },
                { "ContactInfo", ContactInfo },
                { "Email", Email }
            };
        }

        public void FromSqlReader(SQLiteDataReader reader)
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id"));
            Name = reader.GetString(reader.GetOrdinal("Name"));
            ContactInfo = reader.GetString(reader.GetOrdinal("ContactInfo"));
            Email = reader.GetString(reader.GetOrdinal("Email"));
        }
    }
}
