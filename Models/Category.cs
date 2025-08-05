using SmartInventoryTracker.Serializer;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventoryTracker.Models
{
    public class Category : ISQLiteSerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();

        public string TableName => "Categories";

        public Dictionary<string, object> ToSqlParams()
        {
            return new Dictionary<string, object>
            {
                
                { "Name", Name }
            };
        }

        public void FromSqlReader(SQLiteDataReader reader)
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id"));
            Name = reader.GetString(reader.GetOrdinal("Name"));
        }
    }
}
