using System;
using System.Windows;
using System.Windows.Controls;

using CafeManagementSystem.Views.UserControls;

namespace CafeManagementSystem.Views
{
    /// <summary>
    /// Interaction logic for StaffMainWindow.xaml (Cửa sổ chính cho Nhân viên - Bán hàng & Chấm công)
    /// </summary>
    public partial class StaffMainWindow : Window
    {
        // Giữ lại các instance view để bảo toàn trạng thái khi chuyển tab (D10)
        private UserControl? _salesView;
        private UserControl? _menuView;
        private UserControl? _invoiceView;
        private UserControl? _attendanceView;
        private UserControl? _settingsView;

        public StaffMainWindow()
        {
            InitializeComponent();
            StaffNameDisplay.Text = UserSession.CurrentAccount?.DisplayName ?? "Nhân viên thu ngân";
            Loaded += StaffMainWindow_Loaded;
        }

        private void StaffMainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Mặc định nạp SalesView
            NavigateTo("Sales");
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
                case "Sales":
                    _salesView ??= new SalesView();
                    StaffContentArea.Content = _salesView;
                    break;
                case "Menu":
                    _menuView ??= new MenuView();
                    StaffContentArea.Content = _menuView;
                    break;
                case "Invoices":
                    _invoiceView ??= new InvoiceView();
                    StaffContentArea.Content = _invoiceView;
                    break;
                case "Attendance":
                    _attendanceView ??= new AttendanceView();
                    StaffContentArea.Content = _attendanceView;
                    break;
                case "Settings":
                    _settingsView ??= new SettingsView();
                    StaffContentArea.Content = _settingsView;
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
