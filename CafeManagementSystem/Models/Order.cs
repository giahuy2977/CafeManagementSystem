using System;
using System.Collections.Generic;
using System.Text;

namespace CafeManagementSystem.Models
{
    // Class này dùng để chuẩn hóa dữ liệu khi đọc từ Database
    // Phục vụ cho việc hiển thị trạng thái của hóa đơn
    public class Order
    {
        public const string StatusPending = "Pending";
        public const string StatusCompleted = "Completed";
        public const string StatusCancelled = "Cancelled";

        private string _status = StatusPending;

        public int OrderId { get; set; }
        public string OrderCode { get; set; } = string.Empty;

        // Tự động cắt khoảng trắng thừa khi đọc từ SQL Server lên
        public string Status
        {
            get => _status;
            set => _status = string.IsNullOrWhiteSpace(value) ? StatusPending : value.Trim();
        }
    }
}
