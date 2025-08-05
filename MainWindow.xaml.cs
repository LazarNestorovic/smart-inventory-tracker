using SmartInventoryTracker.Database;
using SmartInventoryTracker.Enums;
using SmartInventoryTracker.Models;
using SmartInventoryTracker.Repositories;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartInventoryTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var userRepo = new UserRepository(@"Data Source=C:\Users\lazar\OneDrive\Desktop\SE_praksa\SmartInventoryTracker\Resources\Data\SmartInventoryTracker.db;Version=3;");

            // Dodavanje novog korisnika
            var user = new User
            {
                Username = "admin2",
                Password = "1234",
                Role = UserRoles.Administrator
            };

            userRepo.AddUser(user);
            Console.WriteLine($"Inserted user with ID: {user.Id}");

            // Provera da li je dodat
            var loadedUser = userRepo.GetUserById(user.Id);
            Console.WriteLine($"Loaded: {loadedUser.Username}, Role: {loadedUser.Role}");

            // Brisanje
            userRepo.DeleteUser(user.Id);
            Console.WriteLine("Deleted test user.");
        }
    }
}