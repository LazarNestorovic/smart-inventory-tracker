using Microsoft.Extensions.DependencyInjection;
using SmartInventoryTracker.DTOs;
using SmartInventoryTracker.Models;
using SmartInventoryTracker.ViewModels;
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
using System.Windows.Shapes;

namespace SmartInventoryTracker.Views.WorkerViews
{
    public partial class WorkersMainWindow : Window
    {
        WorkersMainWindowViewModel _viewModel;
        private string _currentView = "Products";
        public WorkersMainWindow(User user, WorkersMainWindowViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            CheckAdmin(user);
            LoadInitialData();
        }

        private void CheckAdmin(User user)
        {
            if (user.Role == Enums.UserRoles.Administrator)
            {
                UsersButton.Visibility = Visibility.Visible;
            }
        }

        private async void LoadInitialData()
        {
            try
            {
                _viewModel.Update();

                SetupFilterDropdowns();

                LoadProductsView();
                SetProductButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetupFilterDropdowns()
        {
            var categoryFilterComboBox = FindName("CategoryComboBox") as ComboBox;
            var supplierFilterComboBox = FindName("SupplierComboBox") as ComboBox;

            if (categoryFilterComboBox != null)
            {
                if (_currentView == "Products")
                {
                    CategoryComboBox.SelectionChanged += CategoryFilter_SelectionChanged;
                    PopulateCategoryFilter(categoryFilterComboBox);
                } else if (_currentView == "Logs")
                {
                    CategoryComboBox.SelectionChanged += CategoryFilter_SelectionChanged;
                    PopulateLogFilter(categoryFilterComboBox);
                }
                else if (_currentView == "Users")
                {
                    CategoryComboBox.SelectionChanged += CategoryFilter_SelectionChanged;
                    PopulateUserFilter(categoryFilterComboBox);
                }
            }

            if (supplierFilterComboBox != null)
            {
                supplierFilterComboBox.SelectionChanged += SupplierFilter_SelectionChanged;
                PopulateSupplierFilter(supplierFilterComboBox);
            }
        }

        private void PopulateCategoryFilter(ComboBox categoryComboBox)
        {
            categoryComboBox.Items.Clear();
            categoryComboBox.Items.Add(new ComboBoxItem { Content = "All" });

            var categoryOptions = _viewModel.GetCategoryFilterOptions();
            foreach (var category in categoryOptions.Skip(1))
            {
                categoryComboBox.Items.Add(new ComboBoxItem { Content = category });
            }

            categoryComboBox.SelectedIndex = 0; 
        }

        private void PopulateUserFilter(ComboBox logComboBox)
        {
            logComboBox.Items.Clear();
            logComboBox.Items.Add(new ComboBoxItem { Content = "All" });

            var categoryOptions = _viewModel.GetUserFilterOptions();
            foreach (var category in categoryOptions.Skip(1))
            {
                logComboBox.Items.Add(new ComboBoxItem { Content = category });
            }

            logComboBox.SelectedIndex = 0; 
        }

        private void PopulateLogFilter(ComboBox logComboBox)
        {
            logComboBox.Items.Clear();
            logComboBox.Items.Add(new ComboBoxItem { Content = "All" });

            var categoryOptions = _viewModel.GetLogFilterOptions();
            foreach (var category in categoryOptions.Skip(1)) 
            {
                logComboBox.Items.Add(new ComboBoxItem { Content = category });
            }

            logComboBox.SelectedIndex = 0; 
        }

        private void PopulateSupplierFilter(ComboBox supplierComboBox)
        {
            supplierComboBox.Items.Clear();
            supplierComboBox.Items.Add(new ComboBoxItem { Content = "All" });

            var supplierOptions = _viewModel.GetSupplierFilterOptions();
            foreach (var supplier in supplierOptions.Skip(1)) 
            {
                supplierComboBox.Items.Add(new ComboBoxItem { Content = supplier });
            }

            supplierComboBox.SelectedIndex = 0; 
        }

        private void CategoryFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox?.SelectedItem is ComboBoxItem selectedItem)
            {
                if (_currentView == "Products")
                {
                    _viewModel.SelectedFilterCategory = selectedItem.Content.ToString();
                } else if (_currentView == "Logs")
                {
                    _viewModel.SelectedFilterLog = selectedItem.Content.ToString();
                }
                else if (_currentView == "Users")
                {
                    _viewModel.SelectedFilterUser = selectedItem.Content.ToString();
                }
            }
        }

        private void SupplierFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox?.SelectedItem is ComboBoxItem selectedItem)
            {
                if (_currentView == "Products")
                {
                    _viewModel.SelectedFilterSupplier = selectedItem.Content.ToString();
                }
                else if (_currentView == "Logs")
                {
                    _viewModel.SelectedFilterLogSupplier = selectedItem.Content.ToString();
                }
            }
        }

        private void UpdateFilterVisibility(string currentView)
        {
            var supplierFilterComboBox = FindName("SupplierComboBox") as ComboBox;
            var categoryFilterComboBox = FindName("CategoryComboBox") as ComboBox;
            var filterSection = FindName("FilterStackPanel") as StackPanel;

            if (supplierFilterComboBox != null && categoryFilterComboBox != null)
            {
                switch (currentView.ToLower())
                {
                    case "products":
                        filterSection.Visibility = Visibility.Visible;
                        SecondFilterTextBlock.Visibility = Visibility.Visible;
                        supplierFilterComboBox.Visibility = Visibility.Visible;
                        categoryFilterComboBox.Visibility = Visibility.Visible;
                        SearchTextBox.Text = "";
                        break;
                    case "categories":
                        filterSection.Visibility = Visibility.Visible;
                        filterSection.Visibility = Visibility.Hidden;
                        SearchTextBox.Text = "";
                        break;
                    case "logs":
                        filterSection.Visibility = Visibility.Visible;
                        SecondFilterTextBlock.Visibility = Visibility.Visible;
                        supplierFilterComboBox.Visibility = Visibility.Visible;
                        categoryFilterComboBox.Visibility = Visibility.Visible;
                        SearchTextBox.Text = "";
                        break;
                    case "suppliers":
                        filterSection.Visibility = Visibility.Hidden;
                        SearchTextBox.Text = "";
                        break;
                    case "users":
                        filterSection.Visibility = Visibility.Visible;
                        SecondFilterTextBlock.Visibility = Visibility.Hidden;
                        supplierFilterComboBox.Visibility = Visibility.Hidden;
                        categoryFilterComboBox.Visibility = Visibility.Visible;
                        SearchTextBox.Text = "";
                        break;
                }
            }

            if (supplierFilterComboBox != null)
            {
                PopulateSupplierFilter(supplierFilterComboBox);
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ItemTable.SelectedItem is ProductDTO selectedProduct)
            {
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            switch (_currentView)
            {
                case "Products":
                    OpenAddProductWindow();
                    break;
                case "Categories":
                    OpenAddCategoryWindow();
                    break;
                case "Suppliers":
                    OpenAddSupplierWindow();
                    break;
                case "Users":
                    OpenAddUserWindow();
                    break;
                case "Logs":
                        OpenAddLogWindow();
                        break;
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = ItemTable.SelectedItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Please select an item to edit.",
                                "No Item Selected",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);

                return;
            }

            switch (_currentView)
            {
                case "Products":
                    OpenEditProductWindow(selectedItem as ProductDTO);
                    break;
                    case "Categories":
                        OpenEditCategoryWindow(selectedItem as CategoryDTO);
                        break;
                    case "Suppliers":
                        OpenEditSupplierWindow(selectedItem as SupplierDTO);
                        break;
                    case "Users":
                        OpenEditUserWindow(selectedItem as UserDTO);
                        break;
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = ItemTable.SelectedItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Please select an item to delete.",
                                "No Item Selected",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete this item?",
                                         "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);


            if (result == MessageBoxResult.Yes)
            {
                switch (_currentView)
                {
                    case "Products":
                        DeleteProduct(selectedItem as ProductDTO);
                        break;
                        case "Categories":
                            DeleteCategory(selectedItem as CategoryDTO);
                            break;
                        case "Suppliers":
                            DeleteSupplier(selectedItem as SupplierDTO);
                            break;
                        case "Users":
                            DeleteUser(selectedItem as UserDTO);
                            break;
                }
            }
        }

        private void RestockButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentView == "Products")
            {
                var selectedProduct = ItemTable.SelectedItem as ProductDTO;
                if (selectedProduct == null)
                {
                    MessageBox.Show("Please select a product to restock.", "No Product Selected",
                                     MessageBoxButton.OK, MessageBoxImage.Information);

                    return;
                }

                OpenRestockProductWindow(selectedProduct);
            }
        }

        private void QRCodeButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentView == "Products")
            {
                var selectedProduct = ItemTable.SelectedItem as ProductDTO;
                if (selectedProduct == null)
                {
                    MessageBox.Show("Please select a product to generate a QR code.", "No Product Selected",
                                    MessageBoxButton.OK, MessageBoxImage.Information);

                    return;
                }

                _viewModel.GenerateQRCode(selectedProduct);
            }
        }

        private void OpenAddProductWindow()
        {
            try
            {
                var addProductWindow = new AddProductWindow(App.ServiceProvider.GetService<AddProductViewModel>());
                addProductWindow.Owner = this;
                if (addProductWindow.ShowDialog() == true)
                {
                    _viewModel.Update();
                    LoadProductsView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening the window for adding a product: {ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void OpenEditProductWindow(ProductDTO product)
        {
            if (product == null) return;

            try
            {
                var editProductWindow = new EditProductWindow(product, App.ServiceProvider.GetService<EditProductViewModel>());
                editProductWindow.Owner = this;
                if (editProductWindow.ShowDialog() == true)
                {
                    _viewModel.Update();
                    LoadProductsView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening the window for editing the product: {ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void OpenRestockProductWindow(ProductDTO product)
        {
            try
            {
                var restockWindow = new RestockProductWindow(product, App.ServiceProvider.GetService<RestockProductViewModel>());
                restockWindow.Owner = this;
                if (restockWindow.ShowDialog() == true)
                {
                    _viewModel.Update();
                    LoadProductsView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening the window for restocking: {ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void DeleteProduct(ProductDTO product)
        {
            try
            {
                var result = MessageBox.Show(
                                $"Are you sure you want to delete the product '{product.Name}'?\n\n" +
                                "The product will be marked as deleted but will not be permanently removed from the database.",
                                "Delete Confirmation",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question,
                                MessageBoxResult.No);


                if (result == MessageBoxResult.Yes)
                {
                    _viewModel.SoftDeleteProduct(product);
                    _viewModel.Update();
                    LoadProductsView();

                    MessageBox.Show(
                        $"The product '{product.Name}' has been successfully marked as deleted.",
                        "Success",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error deleting the product: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

            }
        }

        private void OpenAddCategoryWindow()
        {
            try
            {
                var addCategoryWindow = new AddCategoryWindow(App.ServiceProvider.GetService<AddCategoryViewModel>());
                addCategoryWindow.Owner = this;
                if (addCategoryWindow.ShowDialog() == true)
                {
                    _viewModel.Update();
                    SetupFilterDropdowns();
                    LoadCategoriesView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening the window to add a category: {ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void OpenEditCategoryWindow(CategoryDTO category)
        {
            if (category == null) return;

            try
            {
                var editCategoryWindow = new EditCategoryWindow(category, App.ServiceProvider.GetService<EditCategoryViewModel>());
                editCategoryWindow.Owner = this;
                if (editCategoryWindow.ShowDialog() == true)
                {
                    _viewModel.Update();
                    LoadCategoriesView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening the window to edit the category: {ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void DeleteCategory(CategoryDTO category)
        {
            try
            {
                _viewModel.DeleteCategory(category);
                _viewModel.Update();
                LoadCategoriesView();
                MessageBox.Show("Category was successfully deleted.", "Success",
                                 MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting category: {ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void OpenAddSupplierWindow()
        {
            try
            {
                var addSupplierWindow = new AddSupplierWindow(App.ServiceProvider.GetService<AddSupplierViewModel>());
                addSupplierWindow.Owner = this;
                if (addSupplierWindow.ShowDialog() == true)
                {
                    _viewModel.Update();
                    SetupFilterDropdowns();
                    LoadSuppliersView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening window to add supplier: {ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void OpenEditSupplierWindow(SupplierDTO supplier)
        {
            if (supplier == null) return;

            try
            {
                var editSupplierWindow = new EditSupplierWindow(supplier, App.ServiceProvider.GetService<EditSupplierViewModel>());
                editSupplierWindow.Owner = this;
                if (editSupplierWindow.ShowDialog() == true)
                {
                    _viewModel.Update();
                    LoadSuppliersView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening window to edit supplier: {ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void DeleteSupplier(SupplierDTO supplier)
        {
            try
            {
                bool deleted = _viewModel.DeleteSupplier(supplier);
                _viewModel.Update();
                LoadSuppliersView();
                if (deleted)
                {
                    MessageBox.Show("Supplier was successfully deleted.", "Success",
                                    MessageBoxButton.OK, MessageBoxImage.Information);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting supplier: {ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void OpenAddUserWindow()
        {
            try
            {
                var addUserWindow = new AddUserWindow(App.ServiceProvider.GetService<AddUserViewModel>());
                addUserWindow.Owner = this;
                if (addUserWindow.ShowDialog() == true)
                {
                    _viewModel.Update();
                    LoadUsersView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening window to add user: {ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void OpenEditUserWindow(UserDTO user)
        {
            if (user == null) return;

            try
            {
                var editUserWindow = new EditUserWindow(user, App.ServiceProvider.GetService<EditUserViewModel>());
                editUserWindow.Owner = this;
                if (editUserWindow.ShowDialog() == true)
                {
                    _viewModel.Update();
                    LoadUsersView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening window to edit user: {ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void DeleteUser(UserDTO user)
        {
            try
            {
                bool deleted = _viewModel.DeleteUser(user);
                _viewModel.Update();
                LoadUsersView();
                if (deleted)
                {
                    MessageBox.Show("User has been successfully deleted.", "Success",
                                    MessageBoxButton.OK, MessageBoxImage.Information);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting user: {ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void OpenAddLogWindow()
        {
            try
            {
                var addLogWindow = new AddLogWindow(App.ServiceProvider.GetService<AddLogViewModel>());
                addLogWindow.Owner = this;
                if (addLogWindow.ShowDialog() == true)
                {
                    _viewModel.Update();
                    LoadLogsView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening window to add log: {ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void SetProductButtons()
        {
            FirstFilterTextBlock.Text = "Category";
            SecondFilterTextBlock.Text = "Supplier";
            AddButton.Content = "+ New Product";
            EditButton.Visibility = Visibility.Visible;
            EditButton.Content = "Edit Product";
            RemoveButton.Visibility = Visibility.Visible;
            RemoveButton.Content = "Delete Product";
            RestockButton.Visibility = Visibility.Visible;
            RestockButton.Content = "Restock Product";
            QRCodeButton.Visibility = Visibility.Visible;
            _currentView = "Products";
        }

        private void SetCategoryButtons()
        {
            AddButton.Content = "+ New Category";
            EditButton.Visibility = Visibility.Visible;
            EditButton.Content = "Edit Category";
            RemoveButton.Visibility = Visibility.Visible;
            RemoveButton.Content = "Delete Category";
            RestockButton.Visibility = Visibility.Hidden;
            QRCodeButton.Visibility = Visibility.Hidden;
            _currentView = "Categories";
        }

        private void SetSupplierButtons()
        {
            AddButton.Content = "+ New Supplier";
            EditButton.Visibility = Visibility.Visible;
            EditButton.Content = "Edit Supplier";
            RemoveButton.Visibility = Visibility.Visible;
            RemoveButton.Content = "Delete Supplier";
            RestockButton.Visibility = Visibility.Hidden;
            QRCodeButton.Visibility = Visibility.Hidden;
            _currentView = "Suppliers";
        }

        private void SetUsersButtons()
        {
            FirstFilterTextBlock.Text = "User Role";
            AddButton.Content = "+ New User";
            EditButton.Visibility = Visibility.Visible;
            EditButton.Content = "Edit User";
            RemoveButton.Visibility = Visibility.Visible;
            RemoveButton.Content = "Delete User";
            RestockButton.Visibility = Visibility.Hidden;
            QRCodeButton.Visibility = Visibility.Hidden;
            _currentView = "Users";
        }

        private void SetLogsButtons()
        {
            FirstFilterTextBlock.Text = "Log Type";
            SecondFilterTextBlock.Text = "Supplier";
            AddButton.Content = "+ New Log";
            EditButton.Visibility = Visibility.Hidden;
            RemoveButton.Visibility = Visibility.Hidden;
            RestockButton.Visibility = Visibility.Hidden;
            QRCodeButton.Visibility = Visibility.Hidden;
            _currentView = "Logs";
        }

        private void ShowButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.GetLowStockProductsNumber() == 0)
            {
                return;
            }
            _viewModel.FilterLowStock();
        }
        private void ClearButtin_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Update();
        }

        private void ProductsButton_Click(object sender, RoutedEventArgs e)
        {
            SetActiveButton(sender as Button);
            LoadProductsView();
            SetProductButtons();
            UpdateFilterVisibility("Products");
            SetupFilterDropdowns();
            LowStockAlert.Visibility = Visibility.Visible;
        }

        private void CategoriesButton_Click(object sender, RoutedEventArgs e)
        {
            SetActiveButton(sender as Button);
            LoadCategoriesView();
            SetCategoryButtons();
            UpdateFilterVisibility("Categories");
            LowStockAlert.Visibility = Visibility.Collapsed;
        }

        private void SuppliersButton_Click(object sender, RoutedEventArgs e)
        {
            SetActiveButton(sender as Button);
            LoadSuppliersView();
            SetSupplierButtons();
            UpdateFilterVisibility("Suppliers");
            LowStockAlert.Visibility = Visibility.Collapsed;
        }

        private void UsersButton_Click(object sender, RoutedEventArgs e)
        {
            SetActiveButton(sender as Button);
            LoadUsersView();
            SetUsersButtons();
            UpdateFilterVisibility("Users");
            SetupFilterDropdowns();
            LowStockAlert.Visibility = Visibility.Collapsed;
        }

        private void LogsButton_Click(object sender, RoutedEventArgs e)
        {
            SetActiveButton(sender as Button);
            LoadLogsView();
            SetLogsButtons();
            UpdateFilterVisibility("Logs");
            SetupFilterDropdowns();
            LowStockAlert.Visibility = Visibility.Collapsed;
        }

        private void LoadUsersView()
        {
            ItemTable.ItemsSource = _viewModel.Users;

            ConfigureUserColumns();
        }

        private void LoadLogsView()
        {
            ItemTable.ItemsSource = _viewModel.Logs;
            ConfigureLogColumns();
        }

        private void LoadProductsView()
        {
            ItemTable.ItemsSource = _viewModel.Products;
            int count = _viewModel.GetLowStockProductsNumber();
            if (count == 1)
            {
                LowStockText.Text = count.ToString() + " Product is low on stock";
            }
            else
            {
                LowStockText.Text = count.ToString() + " Products are low on stock";
            }
            ConfigureProductColumns();
        }

        private void LoadCategoriesView()
        {
            ItemTable.ItemsSource = _viewModel.Categories;
            ConfigureCategoryColumns();
        }

        private void LoadSuppliersView()
        {
            ItemTable.ItemsSource = _viewModel.Suppliers;
            ConfigureSupplierColumns();
        }

        private void ConfigureLogColumns()
        {
            ItemTable.Columns.Clear();
            ItemTable.Columns.Add(new DataGridTextColumn
            {
                Header = "Type",
                Binding = new Binding("Type"),
                Width = new DataGridLength(200)
            });
            ItemTable.Columns.Add(new DataGridTextColumn
            {
                Header = "Amount",
                Binding = new Binding("Amount"),
                Width = new DataGridLength(200)
            });
            ItemTable.Columns.Add(new DataGridTextColumn
            {
                Header = "Product",
                Binding = new Binding("Product.Name"),
                Width = new DataGridLength(200)
            });
            ItemTable.Columns.Add(new DataGridTextColumn
            {
                Header = "Supplier",
                Binding = new Binding("Supplier.Name"),
                Width = new DataGridLength(200)
            });
        }

        private void ConfigureUserColumns()
        {
            ItemTable.Columns.Clear();
            ItemTable.Columns.Add(new DataGridTextColumn
            {
                Header = "Username",
                Binding = new Binding("Username"),
                Width = new DataGridLength(200)
            });
            ItemTable.Columns.Add(new DataGridTextColumn
            {
                Header = "Password",
                Binding = new Binding("Password"),
                Width = new DataGridLength(200)
            });
            ItemTable.Columns.Add(new DataGridTextColumn
            {
                Header = "Role",
                Binding = new Binding("Role"),
                Width = new DataGridLength(200)
            });
        }

        private void ConfigureProductColumns()
        {
            ItemTable.Columns.Clear();
            ItemTable.Columns.Add(new DataGridTextColumn
            {
                Header = "Product Name",
                Binding = new Binding("Name"),
                Width = new DataGridLength(200)
            });
            ItemTable.Columns.Add(new DataGridTextColumn
            {
                Header = "Product Code",
                Binding = new Binding("ProductCode"),
                Width = new DataGridLength(200)
            });
            ItemTable.Columns.Add(new DataGridTextColumn
            {
                Header = "Category",
                Binding = new Binding("Category.Name"),
                Width = new DataGridLength(150)
            });
            ItemTable.Columns.Add(new DataGridTextColumn
            {
                Header = "Supplier",
                Binding = new Binding("Supplier.Name"),
                Width = new DataGridLength(150)
            });
            ItemTable.Columns.Add(new DataGridTextColumn
            {
                Header = "Quantity",
                Binding = new Binding("Quantity"),
                Width = new DataGridLength(100)
            });
            ItemTable.Columns.Add(new DataGridTextColumn
            {
                Header = "Min Stock",
                Binding = new Binding("MinimumStock"),
                Width = new DataGridLength(100)
            });
            var statusColumn = new DataGridTemplateColumn
            {
                Header = "Status",
                Width = new DataGridLength(120),
                CellTemplate = (DataTemplate)FindResource("StatusTemplate")
            };
            ItemTable.Columns.Add(statusColumn);
        }

        private void ConfigureCategoryColumns()
        {
            ItemTable.Columns.Clear();
            ItemTable.Columns.Add(new DataGridTextColumn
            {
                Header = "Category Name",
                Binding = new Binding("Name"),
                Width = new DataGridLength(200)
            });
        }

        private void ConfigureSupplierColumns()
        {
            ItemTable.Columns.Clear();
            ItemTable.Columns.Add(new DataGridTextColumn
            {
                Header = "Supplier Name",
                Binding = new Binding("Name"),
                Width = new DataGridLength(200)
            });
            ItemTable.Columns.Add(new DataGridTextColumn
            {
                Header = "Contact",
                Binding = new Binding("ContactInfo"),
                Width = new DataGridLength(200)
            });
            ItemTable.Columns.Add(new DataGridTextColumn
            {
                Header = "Email",
                Binding = new Binding("Email"),
                Width = new DataGridLength(150)
            });
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow(App.ServiceProvider.GetService<LoginViewModel>());
            loginWindow.Show();

            Window parentWindow = Window.GetWindow(this);
            parentWindow?.Close();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentView == "Products")
            {
                _viewModel.FilterProducts();
            }
            else if (_currentView == "Categories")
            {
                _viewModel.FilterCategories();
            }
            else if (_currentView == "Suppliers")
            {
                _viewModel.FilterSuppliers();
            }
            else if (_currentView == "Users")
            {
                _viewModel.FilterUsers();
            }
            else if (_currentView == "Logs")
            {
                _viewModel.FilterLogs();
            }
        }

        private void SetActiveButton(Button activeButton)
        {
            ProductsButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Transparent"));
            CategoriesButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Transparent"));
            SuppliersButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Transparent"));
            LogsButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Transparent"));
            UsersButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Transparent"));

            if (activeButton != null)
            {
                activeButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#23272A"));
            }
        }
    }
}
