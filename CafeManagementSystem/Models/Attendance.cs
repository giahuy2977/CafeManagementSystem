namespace CafeManagementSystem.Models
{
    public class Attendance
    {
        public int AttendanceId { get; set; }
        public int AccountId { get; set; }
        public int? WorkShiftId { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public decimal WorkHours { get; set; }
        public string? Note { get; set; }

        // Thuộc tính phụ trợ hiển thị trên bảng chấm công (lấy qua SQL JOIN)
        public string StaffName { get; set; } = string.Empty;
        public string ShiftName { get; set; } = string.Empty;
    }
}
