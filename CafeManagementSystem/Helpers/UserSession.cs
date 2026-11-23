using System;
using CafeManagementSystem.Models;

namespace CafeManagementSystem.Helpers
{
    public static class UserSession
    {
        private static Account? _currentAccount;

        public static Account? CurrentAccount
        {
            get => _currentAccount;
            set
            {
                _currentAccount = value;
                SessionChanged?.Invoke();
            }
        }

        public static string DisplayName => CurrentAccount?.DisplayName ?? string.Empty;
        public static event Action? SessionChanged;

        public static bool IsLoggedIn => CurrentAccount != null;
        public static bool IsOwner => CurrentAccount?.IsOwner ?? false;
        public static bool IsStaff => CurrentAccount?.IsStaff ?? false;

        public static void StartSession(Account account)
        {
            CurrentAccount = account;
        }

        public static void ClearSession()
        {
            CurrentAccount = null;
        }
    }
}
