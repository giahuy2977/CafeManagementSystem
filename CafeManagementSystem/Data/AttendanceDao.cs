using System;
using Microsoft.Data.SqlClient;

namespace CafeManagementSystem.Data
{
    public class AttendanceDao
    {
        public bool CheckIn(int accountId, int? workShiftId, string? note = null)
        {
            const string sql = @"
                INSERT INTO Attendance (AccountId, WorkShiftId, CheckInTime, WorkHours, Note)
                VALUES (@AccountId, @WorkShiftId, SYSDATETIME(), 0, @Note);";

            using var conn = DatabaseConnection.GetConnection();
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@AccountId", accountId);
            // Xử lý tham số có thể null bằng DBNull.Value
            cmd.Parameters.AddWithValue("@WorkShiftId", (object?)workShiftId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Note", (object?)note ?? DBNull.Value);

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool CheckOut(int attendanceId, decimal workHours)
        {
            const string sql = @"
                UPDATE Attendance
                SET CheckOutTime = SYSDATETIME(), WorkHours = @WorkHours
                WHERE AttendanceId = @AttendanceId AND CheckOutTime IS NULL;";

            using var conn = DatabaseConnection.GetConnection();
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@AttendanceId", attendanceId);
            cmd.Parameters.AddWithValue("@WorkHours", workHours);

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
