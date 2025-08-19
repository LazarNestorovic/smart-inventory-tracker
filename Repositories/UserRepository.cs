using SmartInventoryTracker.Interfaces;
using SmartInventoryTracker.Models;
using SmartInventoryTracker.Serializer;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventoryTracker.Repositories
{
    public class UserRepository : IUserRepository
    {
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

        public User? GetByUsername (string username)
        {
            List<User> temp = GetAllUsers();
            foreach (User u in temp)
            {
                if (u.Username == username)
                {
                    return u;
                }
            }
            return null;
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
