using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CafeManagementSystem.Models;

namespace CafeManagementSystem.Views.UserControls
{
    public partial class SalesView : UserControl
    {
        public ObservableCollection<Product> Products { get; set; } = new();
        public ObservableCollection<CoffeeTable> Tables { get; set; } = new();

        private CoffeeTable? _selectedTable;
        private string _selectedCategory = "All";
        private decimal _discountRate = 0m;

        public SalesView()
        {
            InitializeComponent();
            SeedData();
            InitView();
        }

        private void SeedData()
        {
            Products.Add(new Product { Id = "001", Name = "Espresso", Category = "Coffee", Price = 3.50m, IsAvailable = true, Icon = "☕" });
            Products.Add(new Product { Id = "002", Name = "Cappuccino", Category = "Coffee", Price = 4.20m, IsAvailable = true, Icon = "☕" });
            Products.Add(new Product { Id = "003", Name = "Caffè Latte", Category = "Coffee", Price = 4.50m, IsAvailable = true, Icon = "🥛" });
            Products.Add(new Product { Id = "004", Name = "Americano", Category = "Coffee", Price = 3.80m, IsAvailable = true, Icon = "☕" });
            Products.Add(new Product { Id = "005", Name = "Mocha", Category = "Coffee", Price = 4.80m, IsAvailable = true, Icon = "🍫" });
            Products.Add(new Product { Id = "006", Name = "Croissant", Category = "Bakery", Price = 3.50m, IsAvailable = true, Icon = "🥐" });
            Products.Add(new Product { Id = "007", Name = "Matcha Latte", Category = "Tea", Price = 4.50m, IsAvailable = true, Icon = "🍵" });
            Products.Add(new Product { Id = "008", Name = "Peach Tea", Category = "Tea", Price = 3.90m, IsAvailable = true, Icon = "🍑" });
            Products.Add(new Product { Id = "009", Name = "Chocolate Muffin", Category = "Bakery", Price = 3.00m, IsAvailable = true, Icon = "🧁" });

            for (int i = 1; i <= 10; i++)
            {
                Tables.Add(new CoffeeTable
                {
                    Id = i,
                    Name = $"Table {i:00}",
                    Seats = (i == 2 || i == 8) ? 2 : 4,
                    IsOccupied = false
                });
            }

            // Tạo sẵn đơn mẫu cho Table 02
            var table2 = Tables[1];
            table2.IsOccupied = true;
            table2.Orders.Add(new OrderDetail { Product = Products[1], Quantity = 2 }); // 2x Cappuccino
            table2.Orders.Add(new OrderDetail { Product = Products[5], Quantity = 1 }); // 1x Croissant
        }

        private void InitView()
        {
            TxtClock.Text = DateTime.Now.ToString("ddd MM/dd, hh:mm tt");
            TablesListControl.ItemsSource = Tables;
            SelectTable(Tables[1]);
            RefreshProducts();
        }

        private void SelectTable(CoffeeTable table)
        {
            _selectedTable = table;
            _discountRate = 0m;
            TxtOrderHeader.Text = table.HeaderTitle;
            OrderItemsControl.ItemsSource = table.Orders;
            RecalculateTotals();
        }

        private void TableTile_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is CoffeeTable table)
                SelectTable(table);
        }

        private void CategoryPill_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton rb && rb.Content != null)
            {
                _selectedCategory = rb.Content.ToString()!;
                RefreshProducts();
            }
        }

        private void SalesFilter_Changed(object sender, TextChangedEventArgs e) => RefreshProducts();

        private void RefreshProducts()
        {
            if (SalesProductsControl == null) return;
            string keyword = TxtSalesSearch?.Text.Trim().ToLower() ?? "";

            SalesProductsControl.ItemsSource = Products.Where(p =>
                p.IsAvailable &&
                (_selectedCategory == "All" || p.Category.Equals(_selectedCategory, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(keyword) || p.Name.ToLower().Contains(keyword))
            ).ToList();
        }

        private void AddProductToOrder_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedTable == null) return;
            if (sender is Button btn && btn.Tag is Product prod)
            {
                var existing = _selectedTable.Orders.FirstOrDefault(x => x.Product.Id == prod.Id);
                if (existing != null) existing.Quantity++;
                else _selectedTable.Orders.Add(new OrderDetail { Product = prod, Quantity = 1 });

                _selectedTable.IsOccupied = true;
                TxtOrderHeader.Text = _selectedTable.HeaderTitle;
                RecalculateTotals();
            }
        }

        private void IncreaseQty_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is OrderDetail item)
            {
                item.Quantity++;
                RecalculateTotals();
            }
        }

        private void DecreaseQty_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedTable == null) return;
            if (sender is Button btn && btn.Tag is OrderDetail item)
            {
                if (item.Quantity > 1) item.Quantity--;
                else _selectedTable.Orders.Remove(item);

                if (_selectedTable.Orders.Count == 0) _selectedTable.IsOccupied = false;
                TxtOrderHeader.Text = _selectedTable.HeaderTitle;
                RecalculateTotals();
            }
        }

        private void RemoveOrderItem_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedTable == null) return;
            if (sender is Button btn && btn.Tag is OrderDetail item)
            {
                _selectedTable.Orders.Remove(item);
                if (_selectedTable.Orders.Count == 0) _selectedTable.IsOccupied = false;
                TxtOrderHeader.Text = _selectedTable.HeaderTitle;
                RecalculateTotals();
            }
        }

        private void ApplyCode_Click(object sender, MouseButtonEventArgs e)
        {
            if (_selectedTable == null || _selectedTable.Orders.Count == 0) return;
            _discountRate = _discountRate == 0m ? 0.10m : 0m;
            RecalculateTotals();
        }

        private void RecalculateTotals()
        {
            if (_selectedTable == null) return;
            decimal sub = _selectedTable.Subtotal;
            decimal discount = Math.Round(sub * _discountRate, 2);
            decimal tax = Math.Round((sub - discount) * 0.08m, 2);
            decimal total = sub - discount + tax;

            TxtSubtotal.Text = $"${sub:F2}";
            TxtDiscount.Text = $"Discount: ${discount:F2}";
            TxtTax.Text = $"${tax:F2}";
            TxtTotal.Text = $"${total:F2}";
        }

        private void PayNow_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedTable == null || _selectedTable.Orders.Count == 0)
            {
                MessageBox.Show("Bàn hiện tại chưa có món nào để thanh toán.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            MessageBox.Show($"Thanh toán thành công cho {_selectedTable.Name}!\nTổng tiền: {TxtTotal.Text}",
                            "In Hóa Đơn", MessageBoxButton.OK, MessageBoxImage.Information);

            _selectedTable.Orders.Clear();
            _selectedTable.IsOccupied = false;
            _discountRate = 0m;
            TxtOrderHeader.Text = _selectedTable.HeaderTitle;
            RecalculateTotals();
        }
    }
}