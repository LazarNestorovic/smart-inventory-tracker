using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventoryTracker.Database
{
    public static class DatabaseInitializer
    {
        public static void Initialize(string connectionString)
        {
            using var connection = new SQLiteConnection(connectionString);
            connection.Open();

            using (var pragmaCmd = new SQLiteCommand("PRAGMA foreign_keys = ON;", connection))
            {
                pragmaCmd.ExecuteNonQuery();
            }

                string createCategoriesTable = @"
                CREATE TABLE IF NOT EXISTS Categories (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL
                );";

            string createLogsTable = @"
                CREATE TABLE IF NOT EXISTS Logs (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Type INTEGER NOT NULL,
                    Amount INTEGER NOT NULL,
                    ProductId INTEGER NOT NULL,
                    SupplierId INTEGER NOT NULL,
                    FOREIGN KEY (ProductId) REFERENCES Products(Id),
                    FOREIGN KEY (SupplierId) REFERENCES Suppliers(Id)
                );";

            string createProductsTable = @"
                    CREATE TABLE IF NOT EXISTS Products (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    ProductCode TEXT NOT NULL,
                    MinimumStock INTEGER NOT NULL,
                    Quantity INTEGER NOT NULL,
                    SupplierId INTEGER NOT NULL,
                    CategoryId INTEGER NOT NULL,
                    Deleted BOOLEAN NOT NULL DEFAULT 0,
                    FOREIGN KEY (CategoryId) REFERENCES Categories(Id),
                    FOREIGN KEY (SupplierId) REFERENCES Suppliers(Id)
                );";

            string createSuppliersTable = @"
                CREATE TABLE IF NOT EXISTS Suppliers (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                ContactInfo TEXT NOT NULL,
                Email TEXT NOT NULL
                );";

            string createUsersTable = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL UNIQUE,
                    Password TEXT NOT NULL,
                    Role INTEGER NOT NULL
                );";

            using var command = new SQLiteCommand(connection);

            command.CommandText = createCategoriesTable;
            command.ExecuteNonQuery();

            command.CommandText = createSuppliersTable;
            command.ExecuteNonQuery();

            command.CommandText = createProductsTable;
            command.ExecuteNonQuery();

            command.CommandText = createLogsTable;
            command.ExecuteNonQuery();

            command.CommandText = createUsersTable;
            command.ExecuteNonQuery();
        }
    }
}
