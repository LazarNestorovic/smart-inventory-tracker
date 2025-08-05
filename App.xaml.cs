using SmartInventoryTracker.Database;
using System.Configuration;
using System.Data;
using System.Windows;

namespace SmartInventoryTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        string connString = @"Data Source=C:\Users\lazar\OneDrive\Desktop\SE_praksa\SmartInventoryTracker\Resources\Data\SmartInventoryTracker.db;Version=3;";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DatabaseInitializer.Initialize(connString);
        }
        
    }

}
