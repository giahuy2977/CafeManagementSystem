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
    /// ViewModel chính điều khiển giao diện Cửa sổ Chủ quán (OwnerMainWindow)
    /// Thuộc trách nhiệm của Gia Huy (Bước 1 - Lõi nền tảng & 2 MainWindow)
    /// </summary>
    public class OwnerMainViewModel : BaseViewModel
    {
        private object? _currentView;
        private string _selectedTab = "Dashboard";
        private string _displayName = "Chủ quán";

        // Cache các View để lưu giữ trạng thái khi chuyển tab
        private OwnerDashboardView? _dashboardView;
        private MenuView? _menuView;
        private InvoiceView? _invoiceView;
        private StaffManagementView? _staffView;
        private ShiftManagementView? _shiftView;
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
                    OnPropertyChanged(nameof(IsDashboardSelected));
                    OnPropertyChanged(nameof(IsMenuSelected));
                    OnPropertyChanged(nameof(IsInvoicesSelected));
                    OnPropertyChanged(nameof(IsStaffSelected));
                    OnPropertyChanged(nameof(IsShiftsSelected));
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
        public bool IsDashboardSelected
        {
            get => string.Equals(_selectedTab, "Dashboard", StringComparison.OrdinalIgnoreCase);
            set { if (value) NavigateTo("Dashboard"); }
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

        public bool IsStaffSelected
        {
            get => string.Equals(_selectedTab, "Staff", StringComparison.OrdinalIgnoreCase);
            set { if (value) NavigateTo("Staff"); }
        }

        public bool IsShiftsSelected
        {
            get => string.Equals(_selectedTab, "Shifts", StringComparison.OrdinalIgnoreCase);
            set { if (value) NavigateTo("Shifts"); }
        }

        public bool IsSettingsSelected
        {
            get => string.Equals(_selectedTab, "Settings", StringComparison.OrdinalIgnoreCase);
            set { if (value) NavigateTo("Settings"); }
        }

        // Commands điều hướng
        public ICommand NavigateCommand { get; }
        public ICommand NavigateDashboardCommand { get; }
        public ICommand NavigateMenuCommand { get; }
        public ICommand NavigateInvoicesCommand { get; }
        public ICommand NavigateStaffCommand { get; }
        public ICommand NavigateShiftsCommand { get; }
        public ICommand NavigateSettingsCommand { get; }
        public ICommand LogoutCommand { get; }

        public OwnerMainViewModel()
        {
            DisplayName = UserSession.IsLoggedIn && !string.IsNullOrWhiteSpace(UserSession.DisplayName) && UserSession.DisplayName != "Người dùng"
                ? UserSession.DisplayName 
                : "Trần Chủ Quán";

            // Đăng ký lắng nghe sự kiện từ UserSession khi phiên làm việc thay đổi
            UserSession.SessionChanged += () =>
            {
                DisplayName = UserSession.IsLoggedIn && !string.IsNullOrWhiteSpace(UserSession.DisplayName) && UserSession.DisplayName != "Người dùng"
                    ? UserSession.DisplayName 
                    : "Trần Chủ Quán";
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

            NavigateDashboardCommand = new RelayCommand(() => NavigateTo("Dashboard"));
            NavigateMenuCommand = new RelayCommand(() => NavigateTo("Menu"));
            NavigateInvoicesCommand = new RelayCommand(() => NavigateTo("Invoices"));
            NavigateStaffCommand = new RelayCommand(() => NavigateTo("Staff"));
            NavigateShiftsCommand = new RelayCommand(() => NavigateTo("Shifts"));
            NavigateSettingsCommand = new RelayCommand(() => NavigateTo("Settings"));

            LogoutCommand = new RelayCommand(ExecuteLogout);

            // Khởi tạo tab mặc định (đặt _selectedTab rỗng trước để SetProperty kích hoạt thông báo đầy đủ)
            _selectedTab = string.Empty;
            NavigateTo("Dashboard");
        }

        public void NavigateTo(string tab)
        {
            SelectedTab = tab;
            switch (tab)
            {
                case "Dashboard":
                    _dashboardView ??= new OwnerDashboardView { DataContext = new OwnerDashboardViewModel() };
                    CurrentView = _dashboardView;
                    break;
                case "Menu":
                    _menuView ??= new MenuView { DataContext = new MenuViewModel() };
                    CurrentView = _menuView;
                    break;
                case "Invoices":
                    _invoiceView ??= new InvoiceView { DataContext = new InvoiceViewModel() };
                    CurrentView = _invoiceView;
                    break;
                case "Staff":
                    _staffView ??= new StaffManagementView { DataContext = new StaffManagementViewModel() };
                    CurrentView = _staffView;
                    break;
                case "Shifts":
                    _shiftView ??= new ShiftManagementView { DataContext = new ShiftManagementViewModel() };
                    CurrentView = _shiftView;
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
                "Bạn có chắc chắn muốn đăng xuất khỏi phân hệ Quản trị Chủ quán?",
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
                    if (window is OwnerMainWindow)
                    {
                        window.Close();
                        break;
                    }
                }
            }
        }
    }
}
