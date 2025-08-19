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
    public partial class AddCategoryWindow : Window
    {
        private AddCategoryViewModel _viewModel;
        public AddCategoryWindow(AddCategoryViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveButton.IsEnabled = false;
                SaveButton.Content = "Saving...";

                bool success = _viewModel.SaveCategory();

                if (success)
                {
                    MessageBox.Show(
                        "Category added successfully!",
                        "Success",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show(
                        "Please correct the errors and try again.",
                        "Validation Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"An unexpected error occurred: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                SaveButton.IsEnabled = _viewModel.IsValid;
                SaveButton.Content = "Save Category";
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (HasUserEnteredData())
            {
                var result = MessageBox.Show(
                    "You have unsaved changes. Are you sure you want to cancel?",
                    "Confirm Cancel",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                {
                    return;
                }
            }

            Close();
        }

        private bool HasUserEnteredData()
        {
            return !string.IsNullOrWhiteSpace(_viewModel.CategoryName);
        }
    }
}
