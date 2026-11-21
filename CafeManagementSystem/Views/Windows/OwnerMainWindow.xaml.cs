using System;
using System.Windows;
using System.Windows.Controls;

using CafeManagementSystem.Views.UserControls;

namespace CafeManagementSystem.Views
{
    /// <summary>
    /// Interaction logic for OwnerMainWindow.xaml (Cửa sổ chính cho Chủ quán - Tổng quan, Nhân sự, Ca làm)
    /// </summary>
    public partial class OwnerMainWindow : Window
    {
        private UserControl? _dashboardView;
        private UserControl? _menuView;
        private UserControl? _invoiceView;
        private UserControl? _staffView;
        private UserControl? _shiftView;
        private UserControl? _settingsView;

        public OwnerMainWindow()
        {
            InitializeComponent();
            OwnerNameDisplay.Text = UserSession.CurrentAccount?.DisplayName ?? "Trần Chủ Quán";
            Loaded += OwnerMainWindow_Loaded;
        }

        private void OwnerMainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Mặc định nạp OwnerDashboardView
            NavigateTo("Dashboard");
        }

        private void OnNavigateClick(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton rb && rb.Tag is string tag)
            {
                NavigateTo(tag);
            }
        }

        public void NavigateTo(string viewName)
        {
            switch (viewName)
            {
                case "Dashboard":
                    _dashboardView ??= new OwnerDashboardView();
                    OwnerContentArea.Content = _dashboardView;
                    break;
                case "Menu":
                    _menuView ??= new MenuView();
                    OwnerContentArea.Content = _menuView;
                    break;
                case "Invoices":
                    _invoiceView ??= new InvoiceView();
                    OwnerContentArea.Content = _invoiceView;
                    break;
                case "Staff":
                    _staffView ??= new StaffManagementView();
                    OwnerContentArea.Content = _staffView;
                    break;
                case "Shifts":
                    _shiftView ??= new ShiftManagementView();
                    OwnerContentArea.Content = _shiftView;
                    break;
                case "Settings":
                    _settingsView ??= new SettingsView();
                    OwnerContentArea.Content = _settingsView;
                    break;
            }
        }

        private void OnLogoutClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận đăng xuất", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                UserSession.ClearSession();
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
        }
    }
}
