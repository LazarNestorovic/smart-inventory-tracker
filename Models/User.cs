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
    public class User : ISQLiteSerializable
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserRoles Role { get; set; }

        public string TableName => "Users";

        public User(int id, string username, string password, UserRoles role)
        {
            Id = id;
            Username = username;
            Password = password;
            Role = role;
        }

        public Dictionary<string, object> ToSqlParams()
        {
            return new Dictionary<string, object>
            {
                //{ "Id", Id },
                { "Username", Username },   
                { "Password", Password },
                { "Role", (int)Role }
            };
        }

        public void FromSqlReader(SQLiteDataReader reader)
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id"));
            Username = reader.GetString(reader.GetOrdinal("Username"));
            Password = reader.GetString(reader.GetOrdinal("Password"));
            Role = (UserRoles)reader.GetInt32(reader.GetOrdinal("Role"));
        }
    }
}
