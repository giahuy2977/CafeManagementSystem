using System;
using Microsoft.Data.SqlClient;
using CafeManagementSystem.Models;

namespace CafeManagementSystem.Data
{
    public class WorkShiftDao
    {
        public WorkShift? GetTodayShiftByAccount(int accountId)
        {
            const string sql = @"
                SELECT TOP 1 WorkShiftId, AccountId, ShiftName, ShiftDate, StartTime, EndTime, Status
                FROM WorkShift
                WHERE AccountId = @AccountId AND ShiftDate = CAST(GETDATE() AS DATE)
                ORDER BY StartTime ASC;";
            using var conn = DatabaseConnection.GetConnection();
            using var cmd  = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@AccountId", accountId);

            conn.Open();
            using var reader = cmd.ExecuteReader();

            if(reader.Read())
            {
                return new WorkShift
                {
                    WorkShiftId = reader.GetInt32(reader.GetOrdinal("WorkShiftId")),
                    AccountId = reader.GetInt32(reader.GetOrdinal("AccountId")),
                    ShiftName = reader.GetString(reader.GetOrdinal("ShiftName")),
                    ShiftDate = reader.GetDateTime(reader.GetOrdinal("ShiftDate")),
                    StartTime = reader.GetTimeSpan(reader.GetOrdinal("StartTime")),
                    EndTime = reader.GetTimeSpan(reader.GetOrdinal("EndTime")),
                    Status = reader.GetString(reader.GetOrdinal("Status"))
                };
            }
            return null;
        }

    }
}
