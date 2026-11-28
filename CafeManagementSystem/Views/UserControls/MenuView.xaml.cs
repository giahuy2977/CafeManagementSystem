using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CafeManagementSystem.Models;

namespace CafeManagementSystem.Views.UserControls
{
    public partial class MenuView : UserControl
    {
        public ObservableCollection<Product> Products { get; set; } = new();
        private Product? _editingProduct;

        public MenuView()
        {
            InitializeComponent();
            LoadSampleData();
            MenuDataGrid.ItemsSource = Products;
        }

        private void LoadSampleData()
        {
            Products.Add(new Product { Id = "001", Name = "Espresso", Category = "Coffee", Price = 3.50m, IsAvailable = true });
            Products.Add(new Product { Id = "002", Name = "Caramel Macchiato", Category = "Coffee", Price = 4.75m, IsAvailable = true });
            Products.Add(new Product { Id = "003", Name = "Chocolate Muffin", Category = "Bakery", Price = 3.00m, IsAvailable = false });
            Products.Add(new Product { Id = "004", Name = "Caramel Finchiato", Category = "Coffee", Price = 3.50m, IsAvailable = true });
            Products.Add(new Product { Id = "005", Name = "Caramel Lake", Category = "Coffee", Price = 3.50m, IsAvailable = true });
            Products.Add(new Product { Id = "006", Name = "Chocolate Muffin", Category = "Bakery", Price = 3.00m, IsAvailable = true });
            Products.Add(new Product { Id = "007", Name = "Coffee Cake", Category = "Bakery", Price = 3.00m, IsAvailable = false });
        }

        private void MenuFilter_Changed(object sender, EventArgs e)
        {
            if (MenuDataGrid == null) return;

            string keyword = TxtMenuSearch?.Text.Trim().ToLower() ?? "";
            string selectedCat = (CmbMenuFilterCategory?.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "All Categories";

            MenuDataGrid.ItemsSource = Products.Where(p =>
                (selectedCat == "All Categories" || p.Category == selectedCat) &&
                (string.IsNullOrEmpty(keyword) || p.Name.ToLower().Contains(keyword) || p.Id.ToLower().Contains(keyword))
            ).ToList();
        }

        private void MenuDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MenuDataGrid.SelectedItem is Product selected)
            {
                _editingProduct = selected;
                TxtEditName.Text = selected.Name;
                TxtEditPrice.Text = selected.Price.ToString("F2");
                ChkEditAvailable.IsChecked = selected.IsAvailable;

                foreach (ComboBoxItem item in CmbEditCategory.Items)
                {
                    if (item.Content?.ToString() == selected.Category)
                    {
                        CmbEditCategory.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void BtnSaveProduct_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtEditName.Text) || !decimal.TryParse(TxtEditPrice.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("Vui lòng nhập tên sản phẩm và giá hợp lệ!", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string category = (CmbEditCategory.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Coffee";
            bool isAvail = ChkEditAvailable.IsChecked == true;

            if (_editingProduct != null)
            {
                _editingProduct.Name = TxtEditName.Text.Trim();
                _editingProduct.Category = category;
                _editingProduct.Price = price;
                _editingProduct.IsAvailable = isAvail;
            }
            else
            {
                string nextId = (Products.Count + 1).ToString("000");
                Products.Add(new Product
                {
                    Id = nextId,
                    Name = TxtEditName.Text.Trim(),
                    Category = category,
                    Price = price,
                    IsAvailable = isAvail
                });
            }

            MenuFilter_Changed(this, EventArgs.Empty);
            ClearForm();
        }

        private void BtnClearForm_Click(object sender, RoutedEventArgs e) => ClearForm();

        private void ClearForm()
        {
            _editingProduct = null;
            MenuDataGrid.SelectedItem = null;
            TxtEditName.Clear();
            TxtEditPrice.Clear();
            CmbEditCategory.SelectedIndex = 0;
            ChkEditAvailable.IsChecked = true;
        }

        private void BtnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (_editingProduct == null)
            {
                MessageBox.Show("Hãy chọn một sản phẩm trên bảng trước khi xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var confirm = MessageBox.Show($"Bạn có chắc muốn xóa món '{_editingProduct.Name}'?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm == MessageBoxResult.Yes)
            {
                Products.Remove(_editingProduct);
                MenuFilter_Changed(this, EventArgs.Empty);
                ClearForm();
            }
        }
    }
}