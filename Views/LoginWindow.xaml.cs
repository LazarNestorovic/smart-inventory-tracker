using Microsoft.Extensions.DependencyInjection;
using SmartInventoryTracker.ViewModels;
using SmartInventoryTracker.Views.WorkerViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartInventoryTracker.Views
{
    public partial class LoginWindow : Window
    {
        private LoginViewModel _loginViewModel;
        public LoginWindow(LoginViewModel loginViewModel)
        {
            InitializeComponent();
            _loginViewModel = loginViewModel;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            var user = _loginViewModel.Login(username, password);

            if (user != null)
            {
                MessageBox.Show($"Welcome, {user.Username}!");
                WorkersMainWindow workersMainWindow = new WorkersMainWindow(user, App.ServiceProvider.GetService<WorkersMainWindowViewModel>());
                workersMainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
