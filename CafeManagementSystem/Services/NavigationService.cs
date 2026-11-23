using System;

namespace CafeManagementSystem.Services
{
    /// <summary>
    /// Dịch vụ điều hướng tập trung giữa các View và Tab trong ứng dụng.
    /// Thuộc trách nhiệm của Gia Huy (Bước 1 - Lõi nền tảng)
    /// </summary>
    public class NavigationService
    {
        private static NavigationService? _instance;
        private static readonly object _lock = new();

        public static NavigationService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance ??= new NavigationService();
                    }
                }
                return _instance;
            }
        }

        private object? _currentView;

        public object? CurrentView
        {
            get => _currentView;
            private set
            {
                _currentView = value;
                CurrentViewChanged?.Invoke(_currentView);
            }
        }

        public event Action<string>? TabNavigationRequested;
        public event Action<object?>? CurrentViewChanged;

        public void NavigateTo(object? view)
        {
            CurrentView = view;
        }

        public void RequestTabNavigation(string tab)
        {
            if (!string.IsNullOrWhiteSpace(tab))
            {
                TabNavigationRequested?.Invoke(tab);
            }
        }
    }
}
