using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace QL_BaiDoXe
{
    /// <summary>
    /// Lớp quản lý kết nối Database và thực thi các câu lệnh SQL.
    /// Sử dụng SqlParameter để chống SQL Injection.
    /// </summary>
    public static class DatabaseHelper
    {
        // ============================================================
        //  CONNECTION STRING
        //  Hỗ trợ Unicode (NVARCHAR) qua thuộc tính charset mặc định
        //  của SQL Server Driver .NET (UTF-16).
        // ============================================================
        private const string ConnectionString =
            "Data Source=localhost;" +
            "Initial Catalog=QL_BaiDoXe;" +
            "Persist Security Info=True;" +
            "User ID=sa;" +
            "Password=lamjcopass123;" +
            "Encrypt=False;" +
            "TrustServerCertificate=True;" +
            "MultipleActiveResultSets=True;";

        // ============================================================
        //  MỞ / ĐÓNG KẾT NỐI
        // ============================================================

        /// <summary>Tạo và mở SqlConnection mới.</summary>
        public static SqlConnection GetConnection()
        {
            var conn = new SqlConnection(ConnectionString);
            conn.Open();
            return conn;
        }

        // ============================================================
        //  THỰC THI TRUY VẤN TRẢ VỀ DataTable
        // ============================================================

        /// <summary>
        /// Thực thi SELECT và trả về DataTable.
        /// </summary>
        public static DataTable ExecuteQuery(string sql, params SqlParameter[] parameters)
        {
            var dt = new DataTable();
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddRange(parameters);
                using (var adapter = new SqlDataAdapter(cmd))
                    adapter.Fill(dt);
            }
            return dt;
        }

        // ============================================================
        //  THỰC THI LỆNH KHÔNG TRẢ KẾT QUẢ (INSERT/UPDATE/DELETE)
        // ============================================================

        /// <summary>
        /// Thực thi câu lệnh INSERT / UPDATE / DELETE.
        /// Trả về số dòng bị ảnh hưởng.
        /// </summary>
        public static int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteNonQuery();
            }
        }

        // ============================================================
        //  THỰC THI TRUY VẤN TRẢ VỀ GIÁ TRỊ ĐƠN
        // ============================================================

        /// <summary>
        /// Thực thi SELECT và trả về ô đầu tiên (ExecuteScalar).
        /// </summary>
        public static object ExecuteScalar(string sql, params SqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteScalar();
            }
        }

        // ============================================================
        //  HÀM BĂM MẬT KHẨU SHA-256
        // ============================================================

        /// <summary>
        /// Băm chuỗi mật khẩu thành SHA-256 (hex lowercase).
        /// </summary>
        public static string HashPasswordSHA256(string plainText)
        {
            using (var sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(plainText));
                var sb = new StringBuilder();
                foreach (byte b in bytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        // ============================================================
        //  THỰC THI NHIỀU LỆNH TRONG MỘT TRANSACTION
        // ============================================================

        /// <summary>
        /// Thực thi danh sách câu lệnh SQL trong một transaction.
        /// Nếu bất kỳ lệnh nào lỗi, toàn bộ sẽ bị rollback.
        /// Mỗi phần tử là Tuple (sql, SqlParameter[]).
        /// </summary>
        public static void ExecuteTransaction(
            System.Collections.Generic.List<(string Sql, SqlParameter[] Params)> commands)
        {
            using (var conn = GetConnection())
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    foreach (var (sql, pars) in commands)
                    {
                        using (var cmd = new SqlCommand(sql, conn, tran))
                        {
                            if (pars != null)
                                cmd.Parameters.AddRange(pars);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }

        // ============================================================
        //  KIỂM TRA KẾT NỐI
        // ============================================================

        /// <summary>
        /// Kiểm tra xem có thể mở kết nối đến Database không.
        /// </summary>
        public static bool TestConnection()
        {
            try
            {
                using (var conn = GetConnection())
                    return conn.State == ConnectionState.Open;
            }
            catch
            {
                return false;
            }
        }
    }
}
