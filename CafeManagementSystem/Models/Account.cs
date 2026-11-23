namespace CafeManagementSystem.Models
{
    public class Account
    {
        public const string RoleOwner = "Owner";
        public const string RoleStaff = "Staff";

        public int AccountId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public decimal HourlyRate { get; set; }
        public string Role { get; set; } = RoleStaff;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsOwner => string.Equals(Role?.Trim(), RoleOwner, StringComparison.OrdinalIgnoreCase);
        public bool IsStaff => string.Equals(Role?.Trim(), RoleStaff, StringComparison.OrdinalIgnoreCase);
    }
}
