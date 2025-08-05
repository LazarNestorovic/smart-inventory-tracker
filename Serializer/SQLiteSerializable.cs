using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using SmartInventoryTracker.Models;

namespace SmartInventoryTracker.Serializer
{
    public class SQLiteSerializable<T> where T : ISQLiteSerializable, new()
    {
        private readonly string _connectionString;

        public SQLiteSerializable(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Insert(List<T> objs)
        {
            if (objs == null || objs.Count == 0) return;

            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            using var transaction = connection.BeginTransaction();

            foreach (T obj in objs)
            {
                var parameters = obj.ToSqlParams();
                var columnNames = string.Join(", ", parameters.Keys);
                var parameterNames = string.Join(", ", parameters.Keys.Select(k => "@" + k));

                string sql = $"INSERT INTO {obj.TableName} ({columnNames}) VALUES ({parameterNames})";

                using var command = new SQLiteCommand(sql, connection, transaction);

                foreach (var kvp in parameters)
                {
                    command.Parameters.AddWithValue("@" + kvp.Key, kvp.Value);
                }

                command.ExecuteNonQuery();
                var idProp = typeof(T).GetProperty("Id");
                if (idProp != null && idProp.CanWrite)
                {
                    idProp.SetValue(obj, (int)connection.LastInsertRowId);
                }
            }
            transaction.Commit();
        }

        public T GetById(int id)
        {
            T temp = new T();
            string sql = $"SELECT * FROM {temp.TableName} WHERE Id = @Id";

            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            using var command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                T obj = new T();
                obj.FromSqlReader(reader);
                return obj;
            }

            return default(T);
        }

        public List<T> GetAll()
        {
            T temp = new T();
            string sql = $"SELECT * FROM {temp.TableName}";
            List<T> result = new List<T>();

            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            using var command = new SQLiteCommand(sql, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                T obj = new T();
                obj.FromSqlReader(reader);
                result.Add(obj);
            }

            return result;
        }

        public void Update(T obj)
        {
            if (obj == null) return;

            var parameters = obj.ToSqlParams();
            var setClause = string.Join(", ", parameters.Where(kvp => kvp.Key != "Id").Select(kvp => $"{kvp.Key} = @{kvp.Key}"));

            string sql = $"UPDATE {obj.TableName} SET {setClause} WHERE Id = @Id";

            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            using var command = new SQLiteCommand(sql, connection);

            foreach (var kvp in parameters)
            {
                command.Parameters.AddWithValue("@" + kvp.Key, kvp.Value);
            }

            command.ExecuteNonQuery();

        }

        public void Delete(int id)
        {
            T temp = new T();
            string sql = $"DELETE FROM {temp.TableName} WHERE Id = @Id";

            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            using var command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            command.ExecuteNonQuery();
        }
    }
}
