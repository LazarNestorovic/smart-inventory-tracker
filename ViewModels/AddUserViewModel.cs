using SmartInventoryTracker.Enums;
using SmartInventoryTracker.Interfaces;
using SmartInventoryTracker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SmartInventoryTracker.ViewModels
{
    public class AddUserViewModel : INotifyPropertyChanged
    {
        private readonly IUserRepository _userRepository;

        private string _username;
        private string _password; 
        public List<string> UserRoles { get; set; }
        private string _selectedRole { get; set; }
        private bool _isValid = true;
        public AddUserViewModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            Username = string.Empty;
            Password = string.Empty;
            UserRoles = new List<string> { "Administrator", "Worker" };
            SelectedRole = UserRoles.First().ToString();
        }

        public string SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                OnPropertyChanged();
            }
        }
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                ValidateUsername();
                ValidateForm();
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                ValidatePassword();
                ValidateForm();
                OnPropertyChanged();
            }
        }
        public bool IsValid
        {
            get => _isValid;
            set
            {
                _isValid = value;
                OnPropertyChanged();
            }
        }

        private void ValidateUsername()
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                IsValid = false;
                return;
            }

            if (Username.Length < 2)
            {
                IsValid = false;
                return;
            }

            if (Username.Length > 100)
            {
                IsValid = false;
                return;
            }

            try
            {
                if (UsernameExists(Username))
                {
                    MessageBox.Show("Product code already exists. Please choose a different code.");
                    IsValid = false;
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking product code: {ex.Message}");
                IsValid = false;
                return;
            }
        }

        private bool UsernameExists(string username)
        {
            return _userRepository.GetAllUsers().Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        private void ValidatePassword()
        {
            if (string.IsNullOrWhiteSpace(Password))
            {
                IsValid = false;
                return;
            }

            if (Password.Length < 2)
            {
                IsValid = false;
                return;
            }

            if (Password.Length > 100)
            {
                IsValid = false;
                return;
            }
        }

        private void ValidateForm()
        {
            IsValid = true;

            ValidateUsername();
            if (!IsValid) return;

            ValidatePassword();
            if (!IsValid) return;

            IsValid = true;
        }

        public bool SaveUser()
        {
            try
            {
                ValidateForm();
                if (!IsValid)
                {
                    return false;
                }

                User newUser = new User();

                if(SelectedRole == "Administrator")
                {
                    newUser.Role = Enums.UserRoles.Administrator;
                }
                else if (SelectedRole == "Worker")
                {
                    newUser.Role = Enums.UserRoles.Worker;
                }
                newUser.Username = Username.Trim();
                newUser.Password = Password.Trim();

                _userRepository.AddUser(newUser);

                return true;
            }
            catch (Exception ex)
            {
                IsValid = false;
                return false;
            }
        }
        public void ResetForm()
        {
            Username = string.Empty;
            Password = string.Empty;
            SelectedRole = UserRoles.First().ToString();
            IsValid = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
