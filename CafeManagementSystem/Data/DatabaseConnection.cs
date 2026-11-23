using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace CafeManagementSystem.Data
{
    /// <summary>
    /// Quản lý chuỗi kết nối và khởi tạo kết nối cơ sở dữ liệu SQL Server theo mô hình Singleton
    /// </summary>
    public class DatabaseConnection
    {
        private static DatabaseConnection? _instance;
        private static readonly object _lock = new();

        public static DatabaseConnection Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance ??= new DatabaseConnection();
                    }
                }
                return _instance;
            }
        }

        public string ConnectionString { get; set; } =
            "Server=localhost;Database=CafeManagementDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(Instance.ConnectionString);
        }

        public SqlConnection CreateConnection()
        {
            return GetConnection();
        }

        public static bool TestConnection()
        {
            try
            {
                using var conn = GetConnection();
                conn.Open();
                return conn.State == ConnectionState.Open;
            }
            catch
            {
                return false;
            }
        }

        public bool CheckConnection()
        {
            return TestConnection();
        }
    }
}
