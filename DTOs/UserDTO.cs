using SmartInventoryTracker.Enums;
using SmartInventoryTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventoryTracker.DTOs
{
    public class UserDTO
    {
        private int id;
        public int Id
        {
            get => id;
            set 
            {
                if (value != id)
                {
                    id = value;
                }
            }
        }
        private string username;
        public string Username
        {
            get => username;
            set
            {
                if (value != username)
                {
                    username = value;
                }
            }
        }
        private string password;
        public string Password
        {
            get => password;
            set
            {
                if (value != password)
                {
                    password = value;
                }
            }
        }
        private UserRoles role;
        public UserRoles Role
        {
            get => role;
            set
            {
                if (value != role)
                {
                    role = value;
                }
            }
        }

        public UserDTO(User user)
        {
            Id = user.Id;
            Username = user.Username;
            Password = user.Password;
            Role = user.Role;
        }

        public User ToUser()
        {
            return new User(Id, Username, Password, Role);
        }
    }
}
