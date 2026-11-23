using System;
using System.Windows.Input;
using CafeManagementSystem.Helpers;
using CafeManagementSystem.Services;
using CafeManagementSystem.Views;
using CafeManagementSystem.Views.UserControls;
using UserSession = CafeManagementSystem.Helpers.UserSession;

namespace CafeManagementSystem.ViewModels
{
    /// <summary>
    /// ViewModel chính điều khiển giao diện Cửa sổ Nhân viên (StaffMainWindow)
    /// Thuộc trách nhiệm của Gia Huy (Bước 1 - Lõi nền tảng & 2 MainWindow)
    /// </summary>
    public class StaffMainViewModel : BaseViewModel
    {
        private object? _currentView;
        private string _selectedTab = "Sales";
        private string _displayName = "Nhân viên";

        // Cache các View để lưu giữ trạng thái POS và dữ liệu khi chuyển tab
        private SalesView? _salesView;
        private MenuView? _menuView;
        private InvoiceView? _invoiceView;
        private AttendanceView? _attendanceView;
        private SettingsView? _settingsView;

        public object? CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public string SelectedTab
        {
            get => _selectedTab;
            set
            {
                if (SetProperty(ref _selectedTab, value))
                {
                    OnPropertyChanged(nameof(IsSalesSelected));
                    OnPropertyChanged(nameof(IsMenuSelected));
                    OnPropertyChanged(nameof(IsInvoicesSelected));
                    OnPropertyChanged(nameof(IsAttendanceSelected));
                    OnPropertyChanged(nameof(IsSettingsSelected));
                }
            }
        }

        public string DisplayName
        {
            get => _displayName;
            set => SetProperty(ref _displayName, value);
        }

        // Thuộc tính phục vụ TwoWay Binding với RadioButton trên Sidebar
        public bool IsSalesSelected
        {
            get => string.Equals(_selectedTab, "Sales", StringComparison.OrdinalIgnoreCase);
            set { if (value) NavigateTo("Sales"); }
        }

        public bool IsMenuSelected
        {
            get => string.Equals(_selectedTab, "Menu", StringComparison.OrdinalIgnoreCase);
            set { if (value) NavigateTo("Menu"); }
        }

        public bool IsInvoicesSelected
        {
            get => string.Equals(_selectedTab, "Invoices", StringComparison.OrdinalIgnoreCase);
            set { if (value) NavigateTo("Invoices"); }
        }

        public bool IsAttendanceSelected
        {
            get => string.Equals(_selectedTab, "Attendance", StringComparison.OrdinalIgnoreCase);
            set { if (value) NavigateTo("Attendance"); }
        }

        public bool IsSettingsSelected
        {
            get => string.Equals(_selectedTab, "Settings", StringComparison.OrdinalIgnoreCase);
            set { if (value) NavigateTo("Settings"); }
        }

        // Commands điều hướng
        public ICommand NavigateCommand { get; }
        public ICommand NavigateSalesCommand { get; }
        public ICommand NavigateMenuCommand { get; }
        public ICommand NavigateInvoicesCommand { get; }
        public ICommand NavigateAttendanceCommand { get; }
        public ICommand NavigateSettingsCommand { get; }
        public ICommand NavigateDashboardCommand { get; } // Fallback tương thích
        public ICommand LogoutCommand { get; }

        public StaffMainViewModel()
        {
            DisplayName = UserSession.IsLoggedIn && !string.IsNullOrWhiteSpace(UserSession.DisplayName) && UserSession.DisplayName != "Người dùng"
                ? UserSession.DisplayName 
                : "Nguyễn Thu Ngân";

            // Đăng ký lắng nghe sự kiện từ UserSession khi phiên làm việc thay đổi
            UserSession.SessionChanged += () =>
            {
                DisplayName = UserSession.IsLoggedIn && !string.IsNullOrWhiteSpace(UserSession.DisplayName) && UserSession.DisplayName != "Người dùng"
                    ? UserSession.DisplayName 
                    : "Nguyễn Thu Ngân";
            };

            // Đăng ký nhận yêu cầu chuyển tab từ NavigationService tập trung
            NavigationService.Instance.TabNavigationRequested += tab =>
            {
                if (!string.IsNullOrWhiteSpace(tab))
                {
                    NavigateTo(tab);
                }
            };

            NavigationService.Instance.CurrentViewChanged += view =>
            {
                if (view != null && !ReferenceEquals(_currentView, view))
                {
                    CurrentView = view;
                }
            };

            NavigateCommand = new RelayCommand(param =>
            {
                if (param is string tab) NavigateTo(tab);
            });

            NavigateSalesCommand = new RelayCommand(() => NavigateTo("Sales"));
            NavigateMenuCommand = new RelayCommand(() => NavigateTo("Menu"));
            NavigateInvoicesCommand = new RelayCommand(() => NavigateTo("Invoices"));
            NavigateAttendanceCommand = new RelayCommand(() => NavigateTo("Attendance"));
            NavigateSettingsCommand = new RelayCommand(() => NavigateTo("Settings"));
            NavigateDashboardCommand = new RelayCommand(() => NavigateTo("Sales"));

            LogoutCommand = new RelayCommand(ExecuteLogout);

            // Mặc định nạp màn hình Bán hàng (Sales POS)
            _selectedTab = string.Empty;
            NavigateTo("Sales");
        }

        public void NavigateTo(string tab)
        {
            SelectedTab = tab;
            switch (tab)
            {
                case "Sales":
                    _salesView ??= new SalesView { DataContext = new SalesViewModel() };
                    CurrentView = _salesView;
                    break;
                case "Menu":
                    _menuView ??= new MenuView { DataContext = new MenuViewModel() };
                    CurrentView = _menuView;
                    break;
                case "Invoices":
                    _invoiceView ??= new InvoiceView { DataContext = new InvoiceViewModel() };
                    CurrentView = _invoiceView;
                    break;
                case "Attendance":
                    _attendanceView ??= new AttendanceView { DataContext = new AttendanceViewModel() };
                    CurrentView = _attendanceView;
                    break;
                case "Settings":
                    _settingsView ??= new SettingsView { DataContext = new SettingsViewModel() };
                    CurrentView = _settingsView;
                    break;
            }

            // Đồng bộ trạng thái vào NavigationService tập trung
            if (CurrentView != null)
            {
                NavigationService.Instance.NavigateTo(CurrentView);
            }
        }

        private void ExecuteLogout()
        {
            var result = System.Windows.MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất khỏi phân hệ Nhân viên?",
                "Xác nhận đăng xuất",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Question);

            if (result == System.Windows.MessageBoxResult.Yes)
            {
                UserSession.ClearSession();
                var loginWindow = new LoginWindow();
                loginWindow.Show();

                foreach (System.Windows.Window window in System.Windows.Application.Current.Windows)
                {
                    if (window is StaffMainWindow)
                    {
                        window.Close();
                        break;
                    }
                }
            }
        }
    }
}
