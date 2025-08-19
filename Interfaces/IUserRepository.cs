using SmartInventoryTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventoryTracker.Interfaces
{
    public interface IUserRepository
    {
        public void AddUser(User user);

        public User? GetUserById(int id);

        public User? GetByUsername(string username);

        public List<User> GetAllUsers();

        public void UpdateUser(User user);

        public void DeleteUser(int id);
    }
}
