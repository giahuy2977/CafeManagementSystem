namespace CafeManagementSystem.Models
{
    public class WorkShift
    {
        public const string StatusScheduled = "Scheduled";
        public const string StatusCompleted = "Completed";
        public const string StatusAbsent = "Absent";

        public int WorkShiftId { get; set; }
        public int AccountId { get; set; }
        public string ShiftName { get; set; } = string.Empty;
        public DateTime ShiftDate { get; set; } = DateTime.Today;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; } = StatusScheduled;

        // Thuộc tính phụ trợ hiển thị tên nhân viên trên 
        // ShiftManagementView (lấy qua SQL JOIN)
        public string StaffName {  get; set; } = string.Empty;
    }
}
