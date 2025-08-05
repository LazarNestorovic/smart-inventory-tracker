using SmartInventoryTracker.Models;
using SmartInventoryTracker.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventoryTracker.Repositories
{
    public class UserRepository
    {
        // This class will handle user-related database operations
        // For now, it is empty, but you can implement methods like AddUser, GetUser, UpdateUser, DeleteUser, etc.
        private readonly SQLiteSerializable<User> _serializer;
        public UserRepository(string connectionString)
        {
            _serializer = new SQLiteSerializable<User>(connectionString);
        }

        public void AddUser(User user)
        {

            _serializer.Insert(new List<User> { user });
        }

        public User? GetUserById(int id)
        {
            return _serializer.GetById(id);
        }

        public List<User> GetAllUsers()
        {
            return _serializer.GetAll();
        }

        public void UpdateUser(User user)
        {
            _serializer.Update(user);
        }

        public void DeleteUser(int id)
        {
            _serializer.Delete(id);
        }
    }
}
