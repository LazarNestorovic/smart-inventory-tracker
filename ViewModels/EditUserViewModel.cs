using SmartInventoryTracker.DTOs;
using SmartInventoryTracker.Interfaces;
using SmartInventoryTracker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SmartInventoryTracker.ViewModels
{
    public class EditUserViewModel
    {
        private int selectedUserId;
        private string _username;
        private string _password;
        public List<string> UserRoles { get; set; }
        private string _selectedRole { get; set; }
        private UserDTO _originalUser;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
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
                ValidateForm();
                OnPropertyChanged();
            }
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
       
        private bool _isValid = true;
        public bool IsValid
        {
            get => _isValid;
            set
            {
                _isValid = value;
                OnPropertyChanged();
            }
        }

        public bool IsFormValid { get; private set; }

        private readonly IUserRepository _userRepository;

        public EditUserViewModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void LoadData(UserDTO selectedUser)
        {
            try
            {
                LoadUserData(selectedUser);
            }
            catch (Exception ex)
            {
                IsValid = false;
            }
        }

        private void LoadUserData(UserDTO User)
        {
            if (User == null) return;


            UserRoles = new List<string> { "Administrator", "Worker" };
            selectedUserId = User.Id;
            Username = User.Username;
            Password = User.Password;
            SelectedRole = User.Role.ToString();
            _originalUser = User;
        }

        public bool UpdateUser()
        {
            var updatedUser = GetUpdatedUser();
            if (updatedUser == null)
            {
                MessageBox.Show("Please fill in all required fields.");
                ResetForm();
                return false;
            }
            else
            {
                var idDTO = updatedUser.Id;
                User p = updatedUser.ToUser();
                var id = p.Id;
                _userRepository.UpdateUser(updatedUser.ToUser());
                return true;
            }
        }

        public UserDTO GetUpdatedUser()
        {
            if (!IsFormValid)
                return null;

            User u = new User
            {
                Id = selectedUserId,
                Username = Username?.Trim(),
                Password = Password?.Trim()
            };
            if (SelectedRole == "Administrator")
            {
                u.Role = Enums.UserRoles.Administrator;
            }
            else if (SelectedRole == "Worker")
            {
                u.Role = Enums.UserRoles.Worker;
            }
            return new UserDTO(u);
        }

        private void ValidateForm()
        {
            bool correct = true;

            if (string.IsNullOrWhiteSpace(Username))
            {
                correct &= false;
                IsValid = false;
            }
            else if (Username.Trim().Length < 2)
            {
                correct &= false;
                IsValid = false;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                correct &= false;
                IsValid = false;
            }
            else if (Password.Trim().Length < 2)
            {
                correct &= false;
                IsValid = false;
            }

            IsFormValid = correct;
        }

        public void ResetForm()
        {
            Username = _originalUser.Username;
            Password = _originalUser.Password;
            SelectedRole = _originalUser.Role.ToString();
            IsValid = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
