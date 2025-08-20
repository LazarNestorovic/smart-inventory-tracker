using SmartInventoryTracker.DTOs;
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
    public partial class EditProductWindow : Window
    {
        EditProductViewModel _viewModel;
        public EditProductWindow(ProductDTO selectedProduct, EditProductViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            _viewModel.LoadData(selectedProduct);
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UpdateButton.Content = "Updating...";

                bool success = _viewModel.UpdateProduct();

                if (success)
                {
                    MessageBox.Show(
                        "Product updated successfully!",
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
                UpdateButton.Content = "Update Product";
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
            return !string.IsNullOrWhiteSpace(_viewModel.ProductName) ||
                   !string.IsNullOrWhiteSpace(_viewModel.ProductCode) ||
                   !string.IsNullOrWhiteSpace(_viewModel.Quantity) ||
                   !string.IsNullOrWhiteSpace(_viewModel.MinimumStock) ||
                   _viewModel.SelectedCategory != null ||
                   _viewModel.SelectedSupplier != null;
        }
    }
}
