using Microsoft.Extensions.DependencyInjection;
using SmartInventoryTracker.Database;
using SmartInventoryTracker.Interfaces;
using SmartInventoryTracker.Repositories;
using SmartInventoryTracker.ViewModels;
using SmartInventoryTracker.Views;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Windows;

namespace SmartInventoryTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        string connString = @"Data Source=C:\Users\lazar\OneDrive\Desktop\SE_praksa\SmartInventoryTracker\Resources\Data\SmartInventoryTracker.db;Version=3;";

        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DatabaseInitializer.Initialize(connString);

            var services = new ServiceCollection();
            services.AddSingleton<IUserRepository>(new UserRepository(connString));
            services.AddSingleton<ISupplierRepository>(new SupplierRepository(connString));
            services.AddSingleton<IProductRepository>(new ProductRepository(connString));
            services.AddSingleton<ILogRepository>(new LogRepository(connString));
            services.AddSingleton<ICategoryRepository>(new CategoryRepository(connString));
            services.AddTransient<LoginViewModel>();
            services.AddTransient<WorkersMainWindowViewModel>();
            services.AddTransient<AddProductViewModel>();
            services.AddTransient<EditProductViewModel>();
            services.AddTransient<RestockProductViewModel>();
            services.AddTransient<AddCategoryViewModel>();
            services.AddTransient<EditCategoryViewModel>();
            services.AddTransient<AddSupplierViewModel>();
            services.AddTransient<EditSupplierViewModel>();
            services.AddTransient<AddUserViewModel>();
            services.AddTransient<EditUserViewModel>();
            services.AddTransient<AddLogViewModel>();

            ServiceProvider = services.BuildServiceProvider();

            var loginWindow = new LoginWindow(ServiceProvider.GetService<LoginViewModel>());
            loginWindow.Show();
        }
        
    }

}
