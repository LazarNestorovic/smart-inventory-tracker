using SmartInventoryTracker.Interfaces;
using SmartInventoryTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventoryTracker.ViewModels
{
    public class LoginViewModel
    {
        private readonly IUserRepository _userRepository;

        public LoginViewModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User Login(string username, string password)
        {
            var user = _userRepository.GetByUsername(username);

            if (user == null)
                return null;

            if (user.Password == password)
            {
                return user;
            }

            return null;
        }
    }
}
