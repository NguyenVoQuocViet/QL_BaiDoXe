using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QL_BaiDoXe
{
    /// <summary>
    /// Advanced Features Extension for future use.
    /// Uncomment and integrate as needed.
    /// </summary>
    public static class AdvancedFeatures
    {
        /*
        ═══════════════════════════════════════════════════════════════════════════
          FEATURE 1: ADVANCED SEARCH & FILTERING
        ═══════════════════════════════════════════════════════════════════════════
        */

        /// <summary>Tìm kiếm nhân viên theo tiêu chí</summary>
        public static DataTable SearchNhanVien(string searchTerm, string searchType = "HoTen")
        {
            string sql = $@"
                SELECT MaNhanVien, HoTen, Email, SoDienThoai, MaVaiTro, CaLamViec, Luong, TrangThai
                FROM NhanVien
                WHERE {searchType} LIKE @SearchTerm
                ORDER BY MaNhanVien";

            return DatabaseHelper.ExecuteQuery(sql,
                new SqlParameter("@SearchTerm", SqlDbType.NVarChar) { Value = "%" + searchTerm + "%" }
            );
        }

        /// <summary>Tìm xe theo biển số</summary>
        public static DataTable SearchXeByBienSo(string bienSo)
        {
            string sql = @"
                SELECT x.MaXe, x.BienSo, x.HangXe, x.TenDongXe, x.MauXe, 
                       lx.TenLoaiXe, cd.HoTen, x.TrangThai
                FROM Xe x
                LEFT JOIN LoaiXe lx ON x.MaLoaiXe = lx.MaLoaiXe
                LEFT JOIN CuDan cd ON x.MaCuDan = cd.MaCuDan
                WHERE x.BienSo LIKE @BienSo";

            return DatabaseHelper.ExecuteQuery(sql,
                new SqlParameter("@BienSo", SqlDbType.VarChar) { Value = "%" + bienSo + "%" }
            );
        }

        /// <summary>Lọc lượt gửi xe theo ngày</summary>
        public static DataTable FilterLuotGuiByDateRange(DateTime fromDate, DateTime toDate, string status = "")
        {
            string sql = @"
                SELECT l.MaLuotGui, t.SoThe, x.BienSo, v.TenViTri, l.ThoiGianVao, 
                       l.ThoiGianRa, l.TrangThaiLuotGui, l.TongTien
                FROM LuotGuiXe l
                LEFT JOIN TheXe t ON l.MaThe = t.MaThe
                LEFT JOIN Xe x ON t.MaXe = x.MaXe
                LEFT JOIN ViTriDo v ON l.MaViTri = v.MaViTri
                WHERE CAST(l.ThoiGianVao AS DATE) BETWEEN @FromDate AND @ToDate";

            if (!string.IsNullOrEmpty(status))
                sql += " AND l.TrangThaiLuotGui = @Status";

            sql += " ORDER BY l.ThoiGianVao DESC";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@FromDate", SqlDbType.Date) { Value = fromDate },
                new SqlParameter("@ToDate", SqlDbType.Date) { Value = toDate }
            };

            if (!string.IsNullOrEmpty(status))
            {
                var paramList = new System.Collections.Generic.List<SqlParameter>(parameters);
                paramList.Add(new SqlParameter("@Status", SqlDbType.NVarChar) { Value = status });
                parameters = paramList.ToArray();
            }

            return DatabaseHelper.ExecuteQuery(sql, parameters);
        }

        /*
        ═══════════════════════════════════════════════════════════════════════════
          FEATURE 2: PAYMENT CALCULATIONS & REVENUE REPORTS
        ═══════════════════════════════════════════════════════════════════════════
        */

        /// <summary>Tính phí gửi xe theo thời gian</summary>
        public static decimal CalculateParkingFee(string maXe, DateTime timeIn, DateTime timeOut)
        {
            try
            {
                // Lấy loại xe
                var xeData = DatabaseManager.GetXeById(maXe);
                if (xeData.Rows.Count == 0) return 0;

                string maLoaiXe = xeData.Rows[0]["MaLoaiXe"].ToString();

                // Lấy giá tiền
                string sql = @"
                    SELECT GiaTienNgay
                    FROM LoaiXe
                    WHERE MaLoaiXe = @MaLoaiXe";

                object result = DatabaseHelper.ExecuteScalar(sql,
                    new SqlParameter("@MaLoaiXe", SqlDbType.VarChar) { Value = maLoaiXe }
                );

                if (result == null) return 0;

                decimal giaTienNgay = Convert.ToDecimal(result);
                TimeSpan duration = timeOut - timeIn;

                // Tính tiền: nếu <= 1 giờ thì tính 1 giờ, sau đó tính theo giờ
                int hours = (int)Math.Ceiling(duration.TotalHours);
                if (hours == 0) hours = 1;

                return giaTienNgay * hours;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>Lấy doanh thu theo khoảng thời gian</summary>
        public static decimal GetRevenueByDateRange(DateTime fromDate, DateTime toDate)
        {
            string sql = @"
                SELECT ISNULL(SUM(SoTien), 0)
                FROM ThanhToan
                WHERE NgayThanhToan BETWEEN @FromDate AND @ToDate
                  AND TrangThai = N'Thành công'";

            object result = DatabaseHelper.ExecuteScalar(sql,
                new SqlParameter("@FromDate", SqlDbType.DateTime) { Value = fromDate },
                new SqlParameter("@ToDate", SqlDbType.DateTime) { Value = toDate }
            );

            return Convert.ToDecimal(result ?? 0);
        }

        /// <summary>Lấy doanh thu theo loại thẻ (ngày/tháng)</summary>
        public static DataTable GetRevenueByCardType(int year, int month)
        {
            string sql = @"
                SELECT 
                    CASE 
                        WHEN tt.LoaiThanhToan = N'Ngày' THEN N'Thẻ ngày'
                        WHEN tt.LoaiThanhToan = N'Tháng' THEN N'Thẻ tháng'
                        ELSE tt.LoaiThanhToan
                    END AS LoaiThe,
                    SUM(tt.SoTien) AS TongDoanh,
                    COUNT(*) AS SoGiao
                FROM ThanhToan tt
                WHERE YEAR(tt.NgayThanhToan) = @Year
                  AND MONTH(tt.NgayThanhToan) = @Month
                  AND tt.TrangThai = N'Thành công'
                GROUP BY tt.LoaiThanhToan";

            return DatabaseHelper.ExecuteQuery(sql,
                new SqlParameter("@Year", SqlDbType.Int) { Value = year },
                new SqlParameter("@Month", SqlDbType.Int) { Value = month }
            );
        }

        /*
        ═══════════════════════════════════════════════════════════════════════════
          FEATURE 3: INCIDENT & MAINTENANCE TRACKING
        ═══════════════════════════════════════════════════════════════════════════
        */

        /// <summary>Thêm báo cáo sự cố mới</summary>
        public static int ReportIncident(string maLuotGui, string noiDung, decimal chiPhi = 0)
        {
            string sql = @"
                INSERT INTO SuCoBaiXe (MaLuotGui, NoiDung, NgayBao, TrangThai, ChiPhi)
                VALUES (@MaLuotGui, @NoiDung, GETDATE(), N'Dang cho xu ly', @ChiPhi)";

            return DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MaLuotGui", SqlDbType.VarChar) { Value = maLuotGui },
                new SqlParameter("@NoiDung", SqlDbType.NVarChar) { Value = noiDung },
                new SqlParameter("@ChiPhi", SqlDbType.Decimal) { Value = chiPhi }
            );
        }

        /// <summary>Cập nhật trạng thái xử lý sự cố</summary>
        public static int ResolveIncident(int maSuCo, string ghiChu = "")
        {
            string sql = @"
                UPDATE SuCoBaiXe
                SET TrangThai = N'Xu ly xong', NgayXuLy = GETDATE(), NoiDung = NoiDung + ' - ' + @GhiChu
                WHERE MaSuCo = @MaSuCo";

            return DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MaSuCo", SqlDbType.Int) { Value = maSuCo },
                new SqlParameter("@GhiChu", SqlDbType.NVarChar) { Value = ghiChu ?? "" }
            );
        }

        /// <summary>Lấy danh sách sự cố chưa xử lý</summary>
        public static DataTable GetPendingIncidents()
        {
            string sql = @"
                SELECT s.MaSuCo, s.MaLuotGui, x.BienSo, s.NoiDung, s.NgayBao, s.ChiPhi
                FROM SuCoBaiXe s
                LEFT JOIN LuotGuiXe l ON s.MaLuotGui = l.MaLuotGui
                LEFT JOIN TheXe t ON l.MaThe = t.MaThe
                LEFT JOIN Xe x ON t.MaXe = x.MaXe
                WHERE s.TrangThai = N'Dang cho xu ly'
                ORDER BY s.NgayBao DESC";

            return DatabaseHelper.ExecuteQuery(sql);
        }

        /*
        ═══════════════════════════════════════════════════════════════════════════
          FEATURE 4: CARD MANAGEMENT & EXPIRATION ALERTS
        ═══════════════════════════════════════════════════════════════════════════
        */

        /// <summary>Lấy danh sách thẻ sắp hết hạn (trong 7 ngày)</summary>
        public static DataTable GetExpiringCards(int daysUntilExpiry = 7)
        {
            string sql = @"
                SELECT t.MaThe, t.SoThe, t.LoaiThe, x.BienSo, x.HangXe, cd.HoTen,
                       t.NgayHetHan, DATEDIFF(DAY, GETDATE(), t.NgayHetHan) AS NgayConLai
                FROM TheXe t
                LEFT JOIN Xe x ON t.MaXe = x.MaXe
                LEFT JOIN CuDan cd ON x.MaCuDan = cd.MaCuDan
                WHERE t.TrangThai = N'Dang hoat dong'
                  AND t.NgayHetHan IS NOT NULL
                  AND DATEDIFF(DAY, GETDATE(), t.NgayHetHan) BETWEEN 0 AND @DaysUntilExpiry
                ORDER BY t.NgayHetHan ASC";

            return DatabaseHelper.ExecuteQuery(sql,
                new SqlParameter("@DaysUntilExpiry", SqlDbType.Int) { Value = daysUntilExpiry }
            );
        }

        /// <summary>Gia hạn thẻ xe</summary>
        public static int RenewCard(string maThe, DateTime newExpiryDate, string ghiChu = "")
        {
            string sql = @"
                BEGIN TRANSACTION;

                -- Cập nhật ngày hết hạn
                UPDATE TheXe
                SET NgayHetHan = @NewExpiryDate
                WHERE MaThe = @MaThe;

                -- Ghi lịch sử
                INSERT INTO LichSuTheXe (MaThe, TrangThaiCu, TrangThaiMoi, NgayCapNhat, GhiChu)
                SELECT @MaThe, TrangThai, TrangThai, GETDATE(), @GhiChu
                FROM TheXe
                WHERE MaThe = @MaThe;

                COMMIT TRANSACTION;";

            return DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MaThe", SqlDbType.VarChar) { Value = maThe },
                new SqlParameter("@NewExpiryDate", SqlDbType.Date) { Value = newExpiryDate },
                new SqlParameter("@GhiChu", SqlDbType.NVarChar) { Value = ghiChu ?? "Gia hạn thẻ" }
            );
        }

        /*
        ═══════════════════════════════════════════════════════════════════════════
          FEATURE 5: PARKING LOT OCCUPANCY & CAPACITY MANAGEMENT
        ═══════════════════════════════════════════════════════════════════════════
        */

        /// <summary>Lấy tình trạng sử dụng vị trí đỗ</summary>
        public static DataTable GetParkingSpotOccupancy(string maKhu = "")
        {
            string sql = @"
                SELECT 
                    k.TenKhu,
                    COUNT(v.MaViTri) AS TongViTri,
                    SUM(CASE WHEN v.TrangThai = 1 THEN 1 ELSE 0 END) AS ViTriDangDung,
                    SUM(CASE WHEN v.TrangThai = 0 THEN 1 ELSE 0 END) AS ViTriTrong,
                    CAST(SUM(CASE WHEN v.TrangThai = 1 THEN 1 ELSE 0 END) * 100.0 / COUNT(v.MaViTri) AS DECIMAL(5,2)) AS TyLeChemPhanTram
                FROM ViTriDo v
                LEFT JOIN KhuVuc k ON v.MaKhu = k.MaKhu";

            if (!string.IsNullOrEmpty(maKhu))
                sql += " WHERE v.MaKhu = @MaKhu";

            sql += " GROUP BY k.TenKhu";

            if (string.IsNullOrEmpty(maKhu))
                return DatabaseHelper.ExecuteQuery(sql);
            else
                return DatabaseHelper.ExecuteQuery(sql,
                    new SqlParameter("@MaKhu", SqlDbType.VarChar) { Value = maKhu }
                );
        }

        /// <summary>Cảnh báo khu vực gần đầy</summary>
        public static DataTable GetOvercrowdedZones(int occupancyThreshold = 80)
        {
            string sql = @"
                SELECT 
                    k.TenKhu,
                    CAST(SUM(CASE WHEN v.TrangThai = 1 THEN 1 ELSE 0 END) * 100.0 / COUNT(v.MaViTri) AS DECIMAL(5,2)) AS TyLeChemPhanTram,
                    COUNT(v.MaViTri) AS TongViTri
                FROM ViTriDo v
                LEFT JOIN KhuVuc k ON v.MaKhu = k.MaKhu
                GROUP BY k.MaKhu, k.TenKhu
                HAVING CAST(SUM(CASE WHEN v.TrangThai = 1 THEN 1 ELSE 0 END) * 100.0 / COUNT(v.MaViTri) AS DECIMAL(5,2)) >= @Threshold
                ORDER BY TyLeChemPhanTram DESC";

            return DatabaseHelper.ExecuteQuery(sql,
                new SqlParameter("@Threshold", SqlDbType.Int) { Value = occupancyThreshold }
            );
        }

        /*
        ═══════════════════════════════════════════════════════════════════════════
          FEATURE 6: STAFF PERFORMANCE ANALYTICS
        ═══════════════════════════════════════════════════════════════════════════
        */

        /// <summary>Lấy thống kê công việc của nhân viên theo tháng</summary>
        public static DataTable GetStaffPerformance(int year, int month)
        {
            string sql = @"
                SELECT 
                    n.MaNhanVien,
                    n.HoTen,
                    COUNT(l.MaLuotGui) AS SoLuotGui,
                    SUM(CASE WHEN l.TrangThaiLuotGui = N'Da ra' THEN 1 ELSE 0 END) AS LuotXeRa
                FROM NhanVien n
                LEFT JOIN LuotGuiXe l ON (n.MaNhanVien = l.MaNVVao OR n.MaNhanVien = l.MaNVRa)
                    AND YEAR(l.ThoiGianVao) = @Year
                    AND MONTH(l.ThoiGianVao) = @Month
                WHERE n.MaVaiTro = 'NV'
                GROUP BY n.MaNhanVien, n.HoTen
                ORDER BY SoLuotGui DESC";

            return DatabaseHelper.ExecuteQuery(sql,
                new SqlParameter("@Year", SqlDbType.Int) { Value = year },
                new SqlParameter("@Month", SqlDbType.Int) { Value = month }
            );
        }

        /*
        ═══════════════════════════════════════════════════════════════════════════
          FEATURE 7: BATCH OPERATIONS
        ═══════════════════════════════════════════════════════════════════════════
        */

        /// <summary>Khóa tất cả thẻ quá hạn</summary>
        public static int DeactivateExpiredCards()
        {
            string sql = @"
                UPDATE TheXe
                SET TrangThai = N'Het han', NgayHetHan = GETDATE()
                WHERE NgayHetHan < GETDATE() AND TrangThai = N'Dang hoat dong'";

            return DatabaseHelper.ExecuteNonQuery(sql);
        }

        /// <summary>Báo cáo hàng ngày: Xe đang gửi</summary>
        public static DataTable DailyReport_CurrentParking()
        {
            string sql = @"
                SELECT 
                    lt.TenLoaiXe AS LoaiXe,
                    COUNT(DISTINCT lg.MaLuotGui) AS SoXe,
                    MIN(lg.ThoiGianVao) AS VaoSom,
                    MAX(lg.ThoiGianVao) AS VaoMuon
                FROM LuotGuiXe lg
                LEFT JOIN TheXe t ON lg.MaThe = t.MaThe
                LEFT JOIN Xe x ON t.MaXe = x.MaXe
                LEFT JOIN LoaiXe lt ON x.MaLoaiXe = lt.MaLoaiXe
                WHERE lg.TrangThaiLuotGui = N'Trong bai'
                  AND CAST(lg.ThoiGianVao AS DATE) = CAST(GETDATE() AS DATE)
                GROUP BY lt.TenLoaiXe";

            return DatabaseHelper.ExecuteQuery(sql);
        }
    }
}
