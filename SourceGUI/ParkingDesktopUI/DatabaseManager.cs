using System;
using System.Data;
using System.Data.SqlClient;

namespace QL_BaiDoXe
{
    /// <summary>
    /// Lớp quản lý CRUD cho tất cả các bảng trong hệ thống quản lý bãi đỗ xe.
    /// Sử dụng DatabaseHelper để thực thi các truy vấn SQL.
    /// </summary>
    public static class DatabaseManager
    {
        #region ============= NHÂN VIÊN - EMPLOYEE CRUD =============

        /// <summary>Lấy danh sách tất cả nhân viên</summary>
        public static DataTable GetAllNhanVien()
        {
            string sql = @"
                SELECT MaNhanVien, HoTen, NgaySinh, DiaChi, GioiTinh, Email, SoDienThoai,
                       TenDangNhap, MatKhau, MaVaiTro, CaLamViec, NgayVaoLam, Luong, TrangThai
                FROM NhanVien
                ORDER BY TRY_CAST(SUBSTRING(MaNhanVien, PATINDEX('%[0-9]%', MaNhanVien), LEN(MaNhanVien)) AS INT), MaNhanVien";
            return DatabaseHelper.ExecuteQuery(sql);
        }

        /// <summary>Lấy nhân viên theo mã</summary>
        public static DataTable GetNhanVienById(string maNhanVien)
        {
            string sql = @"
                SELECT *
                FROM NhanVien
                WHERE MaNhanVien = @MaNhanVien";
            return DatabaseHelper.ExecuteQuery(sql,
                new SqlParameter("@MaNhanVien", SqlDbType.VarChar) { Value = maNhanVien }
            );
        }

        /// <summary>Thêm nhân viên mới</summary>
        public static int AddNhanVien(string maNhanVien, string hoTen, DateTime ngaySinh, 
            string diaChi, string gioiTinh, string email, string soDienThoai, 
            string tenDangNhap, string matKhau, string maVaiTro, string caLamViec, decimal luong)
        {
            string sql = @"
                INSERT INTO NhanVien 
                (MaNhanVien, HoTen, NgaySinh, DiaChi, GioiTinh, Email, SoDienThoai, 
                 TenDangNhap, MatKhau, MaVaiTro, CaLamViec, Luong, NgayVaoLam, TrangThai)
                VALUES (@MaNhanVien, @HoTen, @NgaySinh, @DiaChi, @GioiTinh, @Email, 
                        @SoDienThoai, @TenDangNhap, @MatKhau, @MaVaiTro, @CaLamViec, 
                        @Luong, GETDATE(), N'Đang hoạt động')";

            return DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MaNhanVien", SqlDbType.VarChar) { Value = maNhanVien },
                new SqlParameter("@HoTen", SqlDbType.NVarChar) { Value = hoTen },
                new SqlParameter("@NgaySinh", SqlDbType.Date) { Value = ngaySinh },
                new SqlParameter("@DiaChi", SqlDbType.NVarChar) { Value = diaChi ?? "" },
                new SqlParameter("@GioiTinh", SqlDbType.NVarChar) { Value = gioiTinh ?? "" },
                new SqlParameter("@Email", SqlDbType.VarChar) { Value = email ?? "" },
                new SqlParameter("@SoDienThoai", SqlDbType.VarChar) { Value = soDienThoai ?? "" },
                new SqlParameter("@TenDangNhap", SqlDbType.VarChar) { Value = tenDangNhap },
                new SqlParameter("@MatKhau", SqlDbType.VarChar) { Value = matKhau },
                new SqlParameter("@MaVaiTro", SqlDbType.VarChar) { Value = maVaiTro },
                new SqlParameter("@CaLamViec", SqlDbType.NVarChar) { Value = caLamViec ?? "" },
                new SqlParameter("@Luong", SqlDbType.Decimal) { Value = luong }
            );
        }

        /// <summary>Cập nhật thông tin nhân viên</summary>
        public static int UpdateNhanVien(string maNhanVien, string hoTen, string email, 
            string soDienThoai, string caLamViec, decimal luong, string trangThai)
        {
            string sql = @"
                UPDATE NhanVien
                SET HoTen = @HoTen, Email = @Email, SoDienThoai = @SoDienThoai,
                    CaLamViec = @CaLamViec, Luong = @Luong, TrangThai = @TrangThai
                WHERE MaNhanVien = @MaNhanVien";

            return DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MaNhanVien", SqlDbType.VarChar) { Value = maNhanVien },
                new SqlParameter("@HoTen", SqlDbType.NVarChar) { Value = hoTen },
                new SqlParameter("@Email", SqlDbType.VarChar) { Value = email ?? "" },
                new SqlParameter("@SoDienThoai", SqlDbType.VarChar) { Value = soDienThoai ?? "" },
                new SqlParameter("@CaLamViec", SqlDbType.NVarChar) { Value = caLamViec ?? "" },
                new SqlParameter("@Luong", SqlDbType.Decimal) { Value = luong },
                new SqlParameter("@TrangThai", SqlDbType.NVarChar) { Value = trangThai }
            );
        }

        /// <summary>Xóa nhân viên (Admin only) - xử lý FK trước khi xóa</summary>
        public static int DeleteNhanVien(string maNhanVien)
        {
            // NhanVien được tham chiếu bởi: ThanhToan.MaNhanVien, LuotGuiXe.MaNVVao, LuotGuiXe.MaNVRa
            // Giải pháp: SET NULL các cột FK trỏ đến nhân viên này, sau đó xóa
            var p = new SqlParameter("@MaNhanVien", SqlDbType.VarChar) { Value = maNhanVien };
            DatabaseHelper.ExecuteTransaction(new System.Collections.Generic.List<(string, SqlParameter[])>
            {
                ("UPDATE ThanhToan SET MaNhanVien = NULL WHERE MaNhanVien = @MaNhanVien",
                    new[] { new SqlParameter("@MaNhanVien", SqlDbType.VarChar) { Value = maNhanVien } }),
                ("UPDATE LuotGuiXe SET MaNVVao = NULL WHERE MaNVVao = @MaNhanVien",
                    new[] { new SqlParameter("@MaNhanVien", SqlDbType.VarChar) { Value = maNhanVien } }),
                ("UPDATE LuotGuiXe SET MaNVRa = NULL WHERE MaNVRa = @MaNhanVien",
                    new[] { new SqlParameter("@MaNhanVien", SqlDbType.VarChar) { Value = maNhanVien } }),
                ("DELETE FROM NhanVien WHERE MaNhanVien = @MaNhanVien",
                    new[] { new SqlParameter("@MaNhanVien", SqlDbType.VarChar) { Value = maNhanVien } }),
            });
            return 1;
        }

        #endregion

        #region ============= CƯ DÂN - RESIDENT CRUD =============

        /// <summary>Lấy danh sách tất cả cư dân</summary>
        public static DataTable GetAllCuDan()
        {
            string sql = @"
                SELECT MaCuDan, HoTen, CCCD, Email, SoDienThoai, DiaChiCanHo,
                       NgayDangKy, MaVaiTro, TrangThai, GhiChu
                FROM CuDan
                ORDER BY TRY_CAST(SUBSTRING(MaCuDan, PATINDEX('%[0-9]%', MaCuDan), LEN(MaCuDan)) AS INT), MaCuDan";
            return DatabaseHelper.ExecuteQuery(sql);
        }

        /// <summary>Lấy cư dân theo mã</summary>
        public static DataTable GetCuDanById(string maCuDan)
        {
            string sql = @"
                SELECT *
                FROM CuDan
                WHERE MaCuDan = @MaCuDan";
            return DatabaseHelper.ExecuteQuery(sql,
                new SqlParameter("@MaCuDan", SqlDbType.VarChar) { Value = maCuDan }
            );
        }

        /// <summary>Thêm cư dân mới</summary>
        public static int AddCuDan(string maCuDan, string hoTen, string cccd, string email, 
            string soDienThoai, string diaChiCanHo)
        {
            string sql = @"
                INSERT INTO CuDan 
                (MaCuDan, HoTen, CCCD, Email, SoDienThoai, DiaChiCanHo, MaVaiTro, TrangThai, NgayDangKy)
                VALUES (@MaCuDan, @HoTen, @CCCD, @Email, @SoDienThoai, @DiaChiCanHo, 'CD', N'Đang cư trú', GETDATE())";

            return DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MaCuDan", SqlDbType.VarChar) { Value = maCuDan },
                new SqlParameter("@HoTen", SqlDbType.NVarChar) { Value = hoTen },
                new SqlParameter("@CCCD", SqlDbType.VarChar) { Value = cccd },
                new SqlParameter("@Email", SqlDbType.VarChar) { Value = email ?? "" },
                new SqlParameter("@SoDienThoai", SqlDbType.VarChar) { Value = soDienThoai ?? "" },
                new SqlParameter("@DiaChiCanHo", SqlDbType.NVarChar) { Value = diaChiCanHo ?? "" }
            );
        }

        /// <summary>Cập nhật cư dân</summary>
        public static int UpdateCuDan(string maCuDan, string hoTen, string email, 
            string soDienThoai, string trangThai)
        {
            string sql = @"
                UPDATE CuDan
                SET HoTen = @HoTen, Email = @Email, SoDienThoai = @SoDienThoai, TrangThai = @TrangThai
                WHERE MaCuDan = @MaCuDan";

            return DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MaCuDan", SqlDbType.VarChar) { Value = maCuDan },
                new SqlParameter("@HoTen", SqlDbType.NVarChar) { Value = hoTen },
                new SqlParameter("@Email", SqlDbType.VarChar) { Value = email ?? "" },
                new SqlParameter("@SoDienThoai", SqlDbType.VarChar) { Value = soDienThoai ?? "" },
                new SqlParameter("@TrangThai", SqlDbType.NVarChar) { Value = trangThai }
            );
        }

        /// <summary>Xóa cư dân - xử lý FK theo đúng thứ tự</summary>
        public static int DeleteCuDan(string maCuDan)
        {
            // Chuỗi phụ thuộc:
            // CuDan -> Xe -> TheXe -> LuotGuiXe -> SuCoBaiXe / ThanhToanNgay
            //                      -> LichSuTheXe
            //                      -> ThanhToanThang
            //       -> CuDan_CanHo
            DatabaseHelper.ExecuteTransaction(new System.Collections.Generic.List<(string, SqlParameter[])>
            {
                // 1. Xóa SuCoBaiXe liên quan đến LuotGuiXe của xe thuộc cư dân
                (@"DELETE sc FROM SuCoBaiXe sc
                   INNER JOIN LuotGuiXe lg ON sc.MaLuotGui = lg.MaLuotGui
                   INNER JOIN TheXe tx ON lg.MaThe = tx.MaThe
                   INNER JOIN Xe x ON tx.MaXe = x.MaXe
                   WHERE x.MaCuDan = @MaCuDan",
                    new[] { new SqlParameter("@MaCuDan", SqlDbType.VarChar) { Value = maCuDan } }),

                // 2. Xóa ThanhToanNgay liên quan
                (@"DELETE tn FROM ThanhToanNgay tn
                   INNER JOIN LuotGuiXe lg ON tn.MaLuotGui = lg.MaLuotGui
                   INNER JOIN TheXe tx ON lg.MaThe = tx.MaThe
                   INNER JOIN Xe x ON tx.MaXe = x.MaXe
                   WHERE x.MaCuDan = @MaCuDan",
                    new[] { new SqlParameter("@MaCuDan", SqlDbType.VarChar) { Value = maCuDan } }),

                // 3. Xóa ThanhToan (cha) tương ứng ThanhToanNgay đã xóa (nếu còn)
                (@"DELETE tt FROM ThanhToan tt
                   WHERE NOT EXISTS (SELECT 1 FROM ThanhToanNgay tn2 WHERE tn2.MaThanhToan = tt.MaThanhToan)
                     AND NOT EXISTS (SELECT 1 FROM ThanhToanThang ts2 WHERE ts2.MaThanhToan = tt.MaThanhToan)
                     AND tt.LoaiThanhToan = N'Ngày'
                     AND NOT EXISTS (
                         SELECT 1 FROM LuotGuiXe lg2
                         INNER JOIN TheXe tx2 ON lg2.MaThe = tx2.MaThe
                         INNER JOIN Xe x2 ON tx2.MaXe = x2.MaXe
                         WHERE x2.MaCuDan <> @MaCuDan AND lg2.MaLuotGui IN
                             (SELECT MaLuotGui FROM ThanhToanNgay WHERE MaThanhToan = tt.MaThanhToan)
                     )",
                    new[] { new SqlParameter("@MaCuDan", SqlDbType.VarChar) { Value = maCuDan } }),

                // 4. Xóa LichSuViTriDo liên quan đến TheXe của xe cư dân
                (@"DELETE lv FROM LichSuViTriDo lv
                   INNER JOIN TheXe tx ON lv.MaThe = tx.MaThe
                   INNER JOIN Xe x ON tx.MaXe = x.MaXe
                   WHERE x.MaCuDan = @MaCuDan",
                    new[] { new SqlParameter("@MaCuDan", SqlDbType.VarChar) { Value = maCuDan } }),

                // 5. Xóa LuotGuiXe liên quan đến TheXe của xe cư dân
                (@"DELETE lg FROM LuotGuiXe lg
                   INNER JOIN TheXe tx ON lg.MaThe = tx.MaThe
                   INNER JOIN Xe x ON tx.MaXe = x.MaXe
                   WHERE x.MaCuDan = @MaCuDan",
                    new[] { new SqlParameter("@MaCuDan", SqlDbType.VarChar) { Value = maCuDan } }),

                // 6. Xóa ThanhToanThang liên quan đến TheXe của xe cư dân
                (@"DELETE ts FROM ThanhToanThang ts
                   INNER JOIN TheXe tx ON ts.MaThe = tx.MaThe
                   INNER JOIN Xe x ON tx.MaXe = x.MaXe
                   WHERE x.MaCuDan = @MaCuDan",
                    new[] { new SqlParameter("@MaCuDan", SqlDbType.VarChar) { Value = maCuDan } }),

                // 7. Xóa LichSuTheXe
                (@"DELETE lt FROM LichSuTheXe lt
                   INNER JOIN TheXe tx ON lt.MaThe = tx.MaThe
                   INNER JOIN Xe x ON tx.MaXe = x.MaXe
                   WHERE x.MaCuDan = @MaCuDan",
                    new[] { new SqlParameter("@MaCuDan", SqlDbType.VarChar) { Value = maCuDan } }),

                // 8. Xóa TheXe liên quan đến Xe của cư dân
                (@"DELETE tx FROM TheXe tx
                   INNER JOIN Xe x ON tx.MaXe = x.MaXe
                   WHERE x.MaCuDan = @MaCuDan",
                    new[] { new SqlParameter("@MaCuDan", SqlDbType.VarChar) { Value = maCuDan } }),

                // 9. Xóa Xe của cư dân
                ("DELETE FROM Xe WHERE MaCuDan = @MaCuDan",
                    new[] { new SqlParameter("@MaCuDan", SqlDbType.VarChar) { Value = maCuDan } }),

                // 10. Xóa CuDan_CanHo
                ("DELETE FROM CuDan_CanHo WHERE MaCuDan = @MaCuDan",
                    new[] { new SqlParameter("@MaCuDan", SqlDbType.VarChar) { Value = maCuDan } }),

                // 11. Xóa cư dân
                ("DELETE FROM CuDan WHERE MaCuDan = @MaCuDan",
                    new[] { new SqlParameter("@MaCuDan", SqlDbType.VarChar) { Value = maCuDan } }),
            });
            return 1;
        }

        #endregion

        #region ============= XE - VEHICLE CRUD =============

        /// <summary>Lấy danh sách tất cả xe</summary>
        public static DataTable GetAllXe()
        {
            string sql = @"
                SELECT MaXe, BienSo, HangXe, TenDongXe, MauXe, SoKhung, SoMay,
                       NamSanXuat, NgayDangKyXe, MaLoaiXe, MaCuDan, TrangThai
                FROM Xe
                ORDER BY TRY_CAST(SUBSTRING(MaXe, PATINDEX('%[0-9]%', MaXe), LEN(MaXe)) AS INT), MaXe";
            return DatabaseHelper.ExecuteQuery(sql);
        }

        /// <summary>Lấy xe theo mã</summary>
        public static DataTable GetXeById(string maXe)
        {
            string sql = @"
                SELECT *
                FROM Xe
                WHERE MaXe = @MaXe";
            return DatabaseHelper.ExecuteQuery(sql,
                new SqlParameter("@MaXe", SqlDbType.VarChar) { Value = maXe }
            );
        }

        /// <summary>Lấy danh sách xe của cư dân</summary>
        public static DataTable GetXeByMaCuDan(string maCuDan)
        {
            string sql = @"
                SELECT x.MaXe, x.BienSo, x.HangXe, x.TenDongXe, x.MauXe, 
                       lx.TenLoaiXe, x.NamSanXuat, x.TrangThai
                FROM Xe x
                LEFT JOIN LoaiXe lx ON x.MaLoaiXe = lx.MaLoaiXe
                WHERE x.MaCuDan = @MaCuDan
                ORDER BY TRY_CAST(SUBSTRING(x.MaXe, PATINDEX('%[0-9]%', x.MaXe), LEN(x.MaXe)) AS INT), x.MaXe";
            return DatabaseHelper.ExecuteQuery(sql,
                new SqlParameter("@MaCuDan", SqlDbType.VarChar) { Value = maCuDan }
            );
        }

        /// <summary>Thêm xe mới</summary>
        public static int AddXe(string maXe, string bienSo, string hangXe, string tenDongXe,
            string mauXe, string soKhung, string soMay, int namSanXuat, string maLoaiXe, string maCuDan)
        {
            string sql = @"
                INSERT INTO Xe 
                (MaXe, BienSo, HangXe, TenDongXe, MauXe, SoKhung, SoMay, NamSanXuat, 
                 NgayDangKyXe, MaLoaiXe, MaCuDan, TrangThai)
                VALUES (@MaXe, @BienSo, @HangXe, @TenDongXe, @MauXe, @SoKhung, @SoMay, 
                        @NamSanXuat, GETDATE(), @MaLoaiXe, @MaCuDan, N'Đang sử dụng')";

            return DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MaXe", SqlDbType.VarChar) { Value = maXe },
                new SqlParameter("@BienSo", SqlDbType.VarChar) { Value = bienSo },
                new SqlParameter("@HangXe", SqlDbType.NVarChar) { Value = hangXe ?? "" },
                new SqlParameter("@TenDongXe", SqlDbType.NVarChar) { Value = tenDongXe ?? "" },
                new SqlParameter("@MauXe", SqlDbType.NVarChar) { Value = mauXe ?? "" },
                new SqlParameter("@SoKhung", SqlDbType.VarChar) { Value = soKhung ?? "" },
                new SqlParameter("@SoMay", SqlDbType.VarChar) { Value = soMay ?? "" },
                new SqlParameter("@NamSanXuat", SqlDbType.Int) { Value = namSanXuat },
                new SqlParameter("@MaLoaiXe", SqlDbType.VarChar) { Value = maLoaiXe },
                new SqlParameter("@MaCuDan", SqlDbType.VarChar) { Value = string.IsNullOrEmpty(maCuDan) ? (object)DBNull.Value : maCuDan }
            );
        }

        /// <summary>Cập nhật thông tin xe</summary>
        public static int UpdateXe(string maXe, string hangXe, string tenDongXe, 
            string mauXe, string maLoaiXe, string trangThai)
        {
            string sql = @"
                UPDATE Xe
                SET HangXe = @HangXe, TenDongXe = @TenDongXe, MauXe = @MauXe,
                    MaLoaiXe = @MaLoaiXe, TrangThai = @TrangThai
                WHERE MaXe = @MaXe";

            return DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MaXe", SqlDbType.VarChar) { Value = maXe },
                new SqlParameter("@HangXe", SqlDbType.NVarChar) { Value = hangXe ?? "" },
                new SqlParameter("@TenDongXe", SqlDbType.NVarChar) { Value = tenDongXe ?? "" },
                new SqlParameter("@MauXe", SqlDbType.NVarChar) { Value = mauXe ?? "" },
                new SqlParameter("@MaLoaiXe", SqlDbType.VarChar) { Value = maLoaiXe },
                new SqlParameter("@TrangThai", SqlDbType.NVarChar) { Value = trangThai }
            );
        }

        /// <summary>Xóa xe - xử lý FK theo đúng thứ tự</summary>
        public static int DeleteXe(string maXe)
        {
            // Xe -> TheXe -> LuotGuiXe -> SuCoBaiXe, ThanhToanNgay
            //             -> LichSuTheXe, ThanhToanThang, LichSuViTriDo
            DatabaseHelper.ExecuteTransaction(new System.Collections.Generic.List<(string, SqlParameter[])>
            {
                // 1. Xóa SuCoBaiXe
                (@"DELETE sc FROM SuCoBaiXe sc
                   INNER JOIN LuotGuiXe lg ON sc.MaLuotGui = lg.MaLuotGui
                   INNER JOIN TheXe tx ON lg.MaThe = tx.MaThe
                   WHERE tx.MaXe = @MaXe",
                    new[] { new SqlParameter("@MaXe", SqlDbType.VarChar) { Value = maXe } }),

                // 2. Xóa ThanhToanNgay
                (@"DELETE tn FROM ThanhToanNgay tn
                   INNER JOIN LuotGuiXe lg ON tn.MaLuotGui = lg.MaLuotGui
                   INNER JOIN TheXe tx ON lg.MaThe = tx.MaThe
                   WHERE tx.MaXe = @MaXe",
                    new[] { new SqlParameter("@MaXe", SqlDbType.VarChar) { Value = maXe } }),

                // 3. Xóa LichSuViTriDo
                (@"DELETE lv FROM LichSuViTriDo lv
                   INNER JOIN TheXe tx ON lv.MaThe = tx.MaThe
                   WHERE tx.MaXe = @MaXe",
                    new[] { new SqlParameter("@MaXe", SqlDbType.VarChar) { Value = maXe } }),

                // 4. Xóa LuotGuiXe
                (@"DELETE lg FROM LuotGuiXe lg
                   INNER JOIN TheXe tx ON lg.MaThe = tx.MaThe
                   WHERE tx.MaXe = @MaXe",
                    new[] { new SqlParameter("@MaXe", SqlDbType.VarChar) { Value = maXe } }),

                // 5. Xóa ThanhToanThang
                (@"DELETE ts FROM ThanhToanThang ts
                   INNER JOIN TheXe tx ON ts.MaThe = tx.MaThe
                   WHERE tx.MaXe = @MaXe",
                    new[] { new SqlParameter("@MaXe", SqlDbType.VarChar) { Value = maXe } }),

                // 6. Xóa LichSuTheXe
                (@"DELETE lt FROM LichSuTheXe lt
                   INNER JOIN TheXe tx ON lt.MaThe = tx.MaThe
                   WHERE tx.MaXe = @MaXe",
                    new[] { new SqlParameter("@MaXe", SqlDbType.VarChar) { Value = maXe } }),

                // 7. Xóa TheXe
                ("DELETE FROM TheXe WHERE MaXe = @MaXe",
                    new[] { new SqlParameter("@MaXe", SqlDbType.VarChar) { Value = maXe } }),

                // 8. Xóa Xe
                ("DELETE FROM Xe WHERE MaXe = @MaXe",
                    new[] { new SqlParameter("@MaXe", SqlDbType.VarChar) { Value = maXe } }),
            });
            return 1;
        }

        #endregion

        #region ============= THẺ XE - CARD CRUD =============

        /// <summary>Lấy danh sách tất cả thẻ xe</summary>
        public static DataTable GetAllTheXe()
        {
            string sql = @"
                SELECT MaThe, SoThe, MaXe, LoaiThe, NgayCap, NgayKichHoat,
                       NgayHetHan, TienCoc, TrangThai, GhiChu
                FROM TheXe
                ORDER BY TRY_CAST(SUBSTRING(MaThe, PATINDEX('%[0-9]%', MaThe), LEN(MaThe)) AS INT), MaThe";
            return DatabaseHelper.ExecuteQuery(sql);
        }

        /// <summary>Lấy thẻ xe theo mã</summary>
        public static DataTable GetTheXeById(string maThe)
        {
            string sql = @"
                SELECT *
                FROM TheXe
                WHERE MaThe = @MaThe";
            return DatabaseHelper.ExecuteQuery(sql,
                new SqlParameter("@MaThe", SqlDbType.VarChar) { Value = maThe }
            );
        }

        /// <summary>Thêm thẻ xe mới</summary>
        public static int AddTheXe(string maThe, string soThe, string maXe, string loaiThe, 
            DateTime? ngayHetHan, decimal tienCoc)
        {
            string sql = @"
                INSERT INTO TheXe 
                (MaThe, SoThe, MaXe, LoaiThe, NgayCap, NgayHetHan, TienCoc, TrangThai, GhiChu)
                VALUES (@MaThe, @SoThe, @MaXe, @LoaiThe, GETDATE(), @NgayHetHan, @TienCoc, N'Đang hoạt động', '')";

            return DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MaThe", SqlDbType.VarChar) { Value = maThe },
                new SqlParameter("@SoThe", SqlDbType.VarChar) { Value = soThe ?? "" },
                new SqlParameter("@MaXe", SqlDbType.VarChar) { Value = string.IsNullOrEmpty(maXe) ? (object)DBNull.Value : maXe },
                new SqlParameter("@LoaiThe", SqlDbType.NVarChar) { Value = loaiThe },
                new SqlParameter("@NgayHetHan", SqlDbType.Date) { Value = ngayHetHan ?? (object)DBNull.Value },
                new SqlParameter("@TienCoc", SqlDbType.Decimal) { Value = tienCoc }
            );
        }

        /// <summary>Cập nhật thẻ xe</summary>
        public static int UpdateTheXe(string maThe, string maXe, DateTime? ngayHetHan, string trangThai)
        {
            string sql = @"
                UPDATE TheXe
                SET MaXe = @MaXe, NgayHetHan = @NgayHetHan, TrangThai = @TrangThai
                WHERE MaThe = @MaThe";

            return DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MaThe", SqlDbType.VarChar) { Value = maThe },
                new SqlParameter("@MaXe", SqlDbType.VarChar) { Value = string.IsNullOrEmpty(maXe) ? (object)DBNull.Value : maXe },
                new SqlParameter("@NgayHetHan", SqlDbType.Date) { Value = ngayHetHan ?? (object)DBNull.Value },
                new SqlParameter("@TrangThai", SqlDbType.NVarChar) { Value = trangThai }
            );
        }

        /// <summary>Xóa thẻ xe - xử lý FK theo đúng thứ tự</summary>
        public static int DeleteTheXe(string maThe)
        {
            // TheXe -> LuotGuiXe -> SuCoBaiXe, ThanhToanNgay
            //       -> LichSuTheXe, ThanhToanThang, LichSuViTriDo
            DatabaseHelper.ExecuteTransaction(new System.Collections.Generic.List<(string, SqlParameter[])>
            {
                // 1. Xóa SuCoBaiXe
                (@"DELETE sc FROM SuCoBaiXe sc
                   INNER JOIN LuotGuiXe lg ON sc.MaLuotGui = lg.MaLuotGui
                   WHERE lg.MaThe = @MaThe",
                    new[] { new SqlParameter("@MaThe", SqlDbType.VarChar) { Value = maThe } }),

                // 2. Xóa ThanhToanNgay
                (@"DELETE tn FROM ThanhToanNgay tn
                   INNER JOIN LuotGuiXe lg ON tn.MaLuotGui = lg.MaLuotGui
                   WHERE lg.MaThe = @MaThe",
                    new[] { new SqlParameter("@MaThe", SqlDbType.VarChar) { Value = maThe } }),

                // 3. Xóa LichSuViTriDo
                ("DELETE FROM LichSuViTriDo WHERE MaThe = @MaThe",
                    new[] { new SqlParameter("@MaThe", SqlDbType.VarChar) { Value = maThe } }),

                // 4. Xóa LuotGuiXe
                ("DELETE FROM LuotGuiXe WHERE MaThe = @MaThe",
                    new[] { new SqlParameter("@MaThe", SqlDbType.VarChar) { Value = maThe } }),

                // 5. Xóa ThanhToanThang
                ("DELETE FROM ThanhToanThang WHERE MaThe = @MaThe",
                    new[] { new SqlParameter("@MaThe", SqlDbType.VarChar) { Value = maThe } }),

                // 6. Xóa LichSuTheXe
                ("DELETE FROM LichSuTheXe WHERE MaThe = @MaThe",
                    new[] { new SqlParameter("@MaThe", SqlDbType.VarChar) { Value = maThe } }),

                // 7. Xóa TheXe
                ("DELETE FROM TheXe WHERE MaThe = @MaThe",
                    new[] { new SqlParameter("@MaThe", SqlDbType.VarChar) { Value = maThe } }),
            });
            return 1;
        }

        #endregion

        #region ============= VỊ TRÍ ĐỖ - PARKING SPOT CRUD =============

        /// <summary>Lấy danh sách tất cả vị trí đỗ</summary>
        public static DataTable GetAllViTriDo()
        {
            string sql = @"
                SELECT MaViTri, MaKhu, TenViTri, LoaiViTri, SucChua, TrangThai, GhiChu
                FROM ViTriDo
                ORDER BY TRY_CAST(SUBSTRING(MaKhu, PATINDEX('%[0-9]%', MaKhu), LEN(MaKhu)) AS INT), TRY_CAST(SUBSTRING(MaViTri, PATINDEX('%[0-9]%', MaViTri), LEN(MaViTri)) AS INT), MaViTri";
            return DatabaseHelper.ExecuteQuery(sql);
        }

        /// <summary>Lấy vị trí đỗ theo mã khu</summary>
        public static DataTable GetViTriByKhu(string maKhu)
        {
            string sql = @"
                SELECT MaViTri, TenViTri, LoaiViTri, SucChua, TrangThai
                FROM ViTriDo
                WHERE MaKhu = @MaKhu
                ORDER BY TenViTri";
            return DatabaseHelper.ExecuteQuery(sql,
                new SqlParameter("@MaKhu", SqlDbType.VarChar) { Value = maKhu }
            );
        }

        /// <summary>Thêm vị trí đỗ mới</summary>
        public static int AddViTriDo(string maViTri, string maKhu, string tenViTri, 
            string loaiViTri, int sucChua)
        {
            string sql = @"
                INSERT INTO ViTriDo 
                (MaViTri, MaKhu, TenViTri, LoaiViTri, SucChua, TrangThai, GhiChu)
                VALUES (@MaViTri, @MaKhu, @TenViTri, @LoaiViTri, @SucChua, 0, '')";

            return DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MaViTri", SqlDbType.VarChar) { Value = maViTri },
                new SqlParameter("@MaKhu", SqlDbType.VarChar) { Value = maKhu },
                new SqlParameter("@TenViTri", SqlDbType.VarChar) { Value = tenViTri },
                new SqlParameter("@LoaiViTri", SqlDbType.NVarChar) { Value = loaiViTri ?? "" },
                new SqlParameter("@SucChua", SqlDbType.Int) { Value = sucChua }
            );
        }

        /// <summary>Cập nhật vị trí đỗ</summary>
        public static int UpdateViTriDo(string maViTri, int trangThai)
        {
            string sql = @"
                UPDATE ViTriDo
                SET TrangThai = @TrangThai
                WHERE MaViTri = @MaViTri";

            return DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MaViTri", SqlDbType.VarChar) { Value = maViTri },
                new SqlParameter("@TrangThai", SqlDbType.Bit) { Value = trangThai }
            );
        }

        /// <summary>Xóa vị trí đỗ - xử lý FK theo đúng thứ tự</summary>
        public static int DeleteViTriDo(string maViTri)
        {
            // ViTriDo -> LuotGuiXe -> SuCoBaiXe, ThanhToanNgay
            //         -> LichSuViTriDo
            DatabaseHelper.ExecuteTransaction(new System.Collections.Generic.List<(string, SqlParameter[])>
            {
                // 1. Xóa SuCoBaiXe liên quan đến LuotGuiXe tại vị trí này
                (@"DELETE sc FROM SuCoBaiXe sc
                   INNER JOIN LuotGuiXe lg ON sc.MaLuotGui = lg.MaLuotGui
                   WHERE lg.MaViTri = @MaViTri",
                    new[] { new SqlParameter("@MaViTri", SqlDbType.VarChar) { Value = maViTri } }),

                // 2. Xóa ThanhToanNgay liên quan
                (@"DELETE tn FROM ThanhToanNgay tn
                   INNER JOIN LuotGuiXe lg ON tn.MaLuotGui = lg.MaLuotGui
                   WHERE lg.MaViTri = @MaViTri",
                    new[] { new SqlParameter("@MaViTri", SqlDbType.VarChar) { Value = maViTri } }),

                // 3. Xóa LuotGuiXe
                ("DELETE FROM LuotGuiXe WHERE MaViTri = @MaViTri",
                    new[] { new SqlParameter("@MaViTri", SqlDbType.VarChar) { Value = maViTri } }),

                // 4. Xóa LichSuViTriDo
                ("DELETE FROM LichSuViTriDo WHERE MaViTri = @MaViTri",
                    new[] { new SqlParameter("@MaViTri", SqlDbType.VarChar) { Value = maViTri } }),

                // 5. Xóa ViTriDo
                ("DELETE FROM ViTriDo WHERE MaViTri = @MaViTri",
                    new[] { new SqlParameter("@MaViTri", SqlDbType.VarChar) { Value = maViTri } }),
            });
            return 1;
        }

        #endregion

        #region ============= LOẠI XE - VEHICLE TYPE CRUD =============

        /// <summary>Lấy danh sách tất cả loại xe</summary>
        public static DataTable GetAllLoaiXe()
        {
            string sql = @"
                SELECT MaLoaiXe, TenLoaiXe, MoTa, GiaTienThang, GiaTienNgay, TrangThai
                FROM LoaiXe
                ORDER BY TRY_CAST(SUBSTRING(MaLoaiXe, PATINDEX('%[0-9]%', MaLoaiXe), LEN(MaLoaiXe)) AS INT), MaLoaiXe";
            return DatabaseHelper.ExecuteQuery(sql);
        }

        /// <summary>Thêm loại xe mới</summary>
        public static int AddLoaiXe(string maLoaiXe, string tenLoaiXe, decimal giaTienThang, 
            decimal giaTienNgay, string moTa)
        {
            string sql = @"
                INSERT INTO LoaiXe 
                (MaLoaiXe, TenLoaiXe, GiaTienThang, GiaTienNgay, TrangThai, MoTa)
                VALUES (@MaLoaiXe, @TenLoaiXe, @GiaTienThang, @GiaTienNgay, N'Đang hoạt động', @MoTa)";

            return DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MaLoaiXe", SqlDbType.VarChar) { Value = maLoaiXe },
                new SqlParameter("@TenLoaiXe", SqlDbType.NVarChar) { Value = tenLoaiXe },
                new SqlParameter("@GiaTienThang", SqlDbType.Decimal) { Value = giaTienThang },
                new SqlParameter("@GiaTienNgay", SqlDbType.Decimal) { Value = giaTienNgay },
                new SqlParameter("@MoTa", SqlDbType.NVarChar) { Value = moTa ?? "" }
            );
        }

        /// <summary>Cập nhật loại xe</summary>
        public static int UpdateLoaiXe(string maLoaiXe, string tenLoaiXe, decimal giaTienThang, 
            decimal giaTienNgay, string trangThai)
        {
            string sql = @"
                UPDATE LoaiXe
                SET TenLoaiXe = @TenLoaiXe, GiaTienThang = @GiaTienThang, 
                    GiaTienNgay = @GiaTienNgay, TrangThai = @TrangThai
                WHERE MaLoaiXe = @MaLoaiXe";

            return DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MaLoaiXe", SqlDbType.VarChar) { Value = maLoaiXe },
                new SqlParameter("@TenLoaiXe", SqlDbType.NVarChar) { Value = tenLoaiXe },
                new SqlParameter("@GiaTienThang", SqlDbType.Decimal) { Value = giaTienThang },
                new SqlParameter("@GiaTienNgay", SqlDbType.Decimal) { Value = giaTienNgay },
                new SqlParameter("@TrangThai", SqlDbType.NVarChar) { Value = trangThai }
            );
        }

        /// <summary>Xóa loại xe - xử lý FK theo đúng thứ tự</summary>
        public static int DeleteLoaiXe(string maLoaiXe)
        {
            // LoaiXe -> BangGia
            //        -> Xe -> TheXe -> (LuotGuiXe, LichSu, ThanhToan...)
            // Chiến lược: xóa BangGia, SET NULL MaLoaiXe trong Xe (không xóa xe)
            DatabaseHelper.ExecuteTransaction(new System.Collections.Generic.List<(string, SqlParameter[])>
            {
                // 1. Xóa BangGia liên quan
                ("DELETE FROM BangGia WHERE MaLoaiXe = @MaLoaiXe",
                    new[] { new SqlParameter("@MaLoaiXe", SqlDbType.VarChar) { Value = maLoaiXe } }),

                // 2. SET NULL MaLoaiXe trong Xe (giữ xe, chỉ ngắt liên kết)
                ("UPDATE Xe SET MaLoaiXe = NULL WHERE MaLoaiXe = @MaLoaiXe",
                    new[] { new SqlParameter("@MaLoaiXe", SqlDbType.VarChar) { Value = maLoaiXe } }),

                // 3. Xóa LoaiXe
                ("DELETE FROM LoaiXe WHERE MaLoaiXe = @MaLoaiXe",
                    new[] { new SqlParameter("@MaLoaiXe", SqlDbType.VarChar) { Value = maLoaiXe } }),
            });
            return 1;
        }

        #endregion

        #region ============= BẢNG GIÁ - PRICE LIST CRUD =============

        /// <summary>Lấy danh sách bảng giá</summary>
        public static DataTable GetAllBangGia()
        {
            string sql = @"
                SELECT b.MaBangGia, b.MaLoaiXe, l.TenLoaiXe, b.LoaiTinhPhi, b.DonGia, 
                       b.NgayApDung, b.TrangThai
                FROM BangGia b
                LEFT JOIN LoaiXe l ON b.MaLoaiXe = l.MaLoaiXe
                ORDER BY TRY_CAST(SUBSTRING(b.MaBangGia, PATINDEX('%[0-9]%', b.MaBangGia), LEN(b.MaBangGia)) AS INT), b.MaBangGia";
            return DatabaseHelper.ExecuteQuery(sql);
        }

        /// <summary>Thêm bảng giá mới</summary>
        public static int AddBangGia(string maBangGia, string maLoaiXe, string loaiTinhPhi, 
            decimal donGia, DateTime ngayApDung)
        {
            string sql = @"
                INSERT INTO BangGia 
                (MaBangGia, MaLoaiXe, LoaiTinhPhi, DonGia, NgayApDung, TrangThai)
                VALUES (@MaBangGia, @MaLoaiXe, @LoaiTinhPhi, @DonGia, @NgayApDung, N'Đang hoạt động')";

            return DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MaBangGia", SqlDbType.VarChar) { Value = maBangGia },
                new SqlParameter("@MaLoaiXe", SqlDbType.VarChar) { Value = maLoaiXe },
                new SqlParameter("@LoaiTinhPhi", SqlDbType.NVarChar) { Value = loaiTinhPhi },
                new SqlParameter("@DonGia", SqlDbType.Decimal) { Value = donGia },
                new SqlParameter("@NgayApDung", SqlDbType.Date) { Value = ngayApDung }
            );
        }

        #endregion

        #region ============= KHU VỰC - ZONE CRUD =============

        /// <summary>Lấy danh sách tất cả khu vực</summary>
        public static DataTable GetAllKhuVuc()
        {
            string sql = @"
                SELECT MaKhu, TenKhu, Tang, MoTa, SucChuaToiDa, TrangThai
                FROM KhuVuc
                ORDER BY TRY_CAST(SUBSTRING(MaKhu, PATINDEX('%[0-9]%', MaKhu), LEN(MaKhu)) AS INT), MaKhu";
            return DatabaseHelper.ExecuteQuery(sql);
        }

        /// <summary>Thêm khu vực mới</summary>
        public static int AddKhuVuc(string maKhu, string tenKhu, int tang, int sucChuaToiDa, string moTa)
        {
            string sql = @"
                INSERT INTO KhuVuc 
                (MaKhu, TenKhu, Tang, SucChuaToiDa, TrangThai, MoTa)
                VALUES (@MaKhu, @TenKhu, @Tang, @SucChuaToiDa, N'Đang hoạt động', @MoTa)";

            return DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MaKhu", SqlDbType.VarChar) { Value = maKhu },
                new SqlParameter("@TenKhu", SqlDbType.NVarChar) { Value = tenKhu },
                new SqlParameter("@Tang", SqlDbType.Int) { Value = tang },
                new SqlParameter("@SucChuaToiDa", SqlDbType.Int) { Value = sucChuaToiDa },
                new SqlParameter("@MoTa", SqlDbType.NVarChar) { Value = moTa ?? "" }
            );
        }

        /// <summary>Cập nhật khu vực</summary>
        public static int UpdateKhuVuc(string maKhu, string tenKhu, int tang, int sucChuaToiDa, string trangThai)
        {
            string sql = @"
                UPDATE KhuVuc
                SET TenKhu = @TenKhu, Tang = @Tang, SucChuaToiDa = @SucChuaToiDa, TrangThai = @TrangThai
                WHERE MaKhu = @MaKhu";

            return DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MaKhu", SqlDbType.VarChar) { Value = maKhu },
                new SqlParameter("@TenKhu", SqlDbType.NVarChar) { Value = tenKhu },
                new SqlParameter("@Tang", SqlDbType.Int) { Value = tang },
                new SqlParameter("@SucChuaToiDa", SqlDbType.Int) { Value = sucChuaToiDa },
                new SqlParameter("@TrangThai", SqlDbType.NVarChar) { Value = trangThai }
            );
        }

        /// <summary>Xóa khu vực - xử lý FK theo đúng thứ tự (xóa tất cả vị trí đỗ trong khu trước)</summary>
        public static int DeleteKhuVuc(string maKhu)
        {
            // KhuVuc -> ViTriDo -> LuotGuiXe -> SuCoBaiXe, ThanhToanNgay
            //                   -> LichSuViTriDo
            DatabaseHelper.ExecuteTransaction(new System.Collections.Generic.List<(string, SqlParameter[])>
            {
                // 1. Xóa SuCoBaiXe liên quan đến vị trí trong khu này
                (@"DELETE sc FROM SuCoBaiXe sc
                   INNER JOIN LuotGuiXe lg ON sc.MaLuotGui = lg.MaLuotGui
                   INNER JOIN ViTriDo vt ON lg.MaViTri = vt.MaViTri
                   WHERE vt.MaKhu = @MaKhu",
                    new[] { new SqlParameter("@MaKhu", SqlDbType.VarChar) { Value = maKhu } }),

                // 2. Xóa ThanhToanNgay liên quan
                (@"DELETE tn FROM ThanhToanNgay tn
                   INNER JOIN LuotGuiXe lg ON tn.MaLuotGui = lg.MaLuotGui
                   INNER JOIN ViTriDo vt ON lg.MaViTri = vt.MaViTri
                   WHERE vt.MaKhu = @MaKhu",
                    new[] { new SqlParameter("@MaKhu", SqlDbType.VarChar) { Value = maKhu } }),

                // 3. Xóa LuotGuiXe
                (@"DELETE lg FROM LuotGuiXe lg
                   INNER JOIN ViTriDo vt ON lg.MaViTri = vt.MaViTri
                   WHERE vt.MaKhu = @MaKhu",
                    new[] { new SqlParameter("@MaKhu", SqlDbType.VarChar) { Value = maKhu } }),

                // 4. Xóa LichSuViTriDo
                (@"DELETE lv FROM LichSuViTriDo lv
                   INNER JOIN ViTriDo vt ON lv.MaViTri = vt.MaViTri
                   WHERE vt.MaKhu = @MaKhu",
                    new[] { new SqlParameter("@MaKhu", SqlDbType.VarChar) { Value = maKhu } }),

                // 5. Xóa ViTriDo trong khu
                ("DELETE FROM ViTriDo WHERE MaKhu = @MaKhu",
                    new[] { new SqlParameter("@MaKhu", SqlDbType.VarChar) { Value = maKhu } }),

                // 6. Xóa KhuVuc
                ("DELETE FROM KhuVuc WHERE MaKhu = @MaKhu",
                    new[] { new SqlParameter("@MaKhu", SqlDbType.VarChar) { Value = maKhu } }),
            });
            return 1;
        }

        #endregion

        #region ============= LUỘT GỬI XE - PARKING SESSION CRUD =============

        /// <summary>Lấy danh sách lượt gửi xe (có thể lọc theo trạng thái)</summary>
        public static DataTable GetAllLuotGuiXe(string trangThai = "")
        {
            string sql = @"
                SELECT MaLuotGui, MaThe, MaViTri, ThoiGianVao, ThoiGianRa,
                       AnhVao, AnhRa, MaNVVao, MaNVRa, PhuongThucTinhPhi,
                       TrangThaiLuotGui, TongTien, GhiChu
                FROM LuotGuiXe";

            if (!string.IsNullOrEmpty(trangThai))
                sql += " WHERE TrangThaiLuotGui = @TrangThai";

            sql += " ORDER BY ThoiGianVao DESC";

            if (!string.IsNullOrEmpty(trangThai))
                return DatabaseHelper.ExecuteQuery(sql,
                    new SqlParameter("@TrangThai", SqlDbType.NVarChar) { Value = trangThai }
                );
            else
                return DatabaseHelper.ExecuteQuery(sql);
        }

        /// <summary>Lấy lịch sử gửi xe của cư dân</summary>
        public static DataTable GetLuotGuiByMaCuDan(string maCuDan)
        {
            string sql = @"
                SELECT l.MaLuotGui, t.SoThe, x.BienSo, v.TenViTri, l.ThoiGianVao, 
                       l.ThoiGianRa, l.TongTien, l.TrangThaiLuotGui
                FROM LuotGuiXe l
                LEFT JOIN TheXe t ON l.MaThe = t.MaThe
                LEFT JOIN Xe x ON t.MaXe = x.MaXe
                LEFT JOIN ViTriDo v ON l.MaViTri = v.MaViTri
                WHERE x.MaCuDan = @MaCuDan
                ORDER BY l.ThoiGianVao DESC";

            return DatabaseHelper.ExecuteQuery(sql,
                new SqlParameter("@MaCuDan", SqlDbType.VarChar) { Value = maCuDan }
            );
        }

        /// <summary>Thêm lượt gửi xe mới (check-in)</summary>
        public static int AddLuotGuiXe(string maLuotGui, string maThe, string maViTri, DateTime thoiGianVao)
        {
            string sql = @"
                INSERT INTO LuotGuiXe (MaLuotGui, MaThe, MaViTri, ThoiGianVao, TrangThaiLuotGui)
                VALUES (@MaLuotGui, @MaThe, @MaViTri, @ThoiGianVao, N'Đang gửi')";

            return DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MaLuotGui", SqlDbType.VarChar) { Value = maLuotGui },
                new SqlParameter("@MaThe", SqlDbType.VarChar) { Value = maThe },
                new SqlParameter("@MaViTri", SqlDbType.VarChar) { Value = maViTri },
                new SqlParameter("@ThoiGianVao", SqlDbType.DateTime) { Value = thoiGianVao }
            );
        }

        /// <summary>Cập nhật lượt gửi xe (check-out / sửa trạng thái)</summary>
        public static int UpdateLuotGuiXe(string maLuotGui, DateTime? thoiGianRa, string trangThai, decimal tongTien)
        {
            string sql = @"
                UPDATE LuotGuiXe
                SET ThoiGianRa = @ThoiGianRa, TrangThaiLuotGui = @TrangThai, TongTien = @TongTien
                WHERE MaLuotGui = @MaLuotGui";

            return DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MaLuotGui", SqlDbType.VarChar) { Value = maLuotGui },
                new SqlParameter("@ThoiGianRa", SqlDbType.DateTime) { Value = (object?)thoiGianRa ?? DBNull.Value },
                new SqlParameter("@TrangThai", SqlDbType.NVarChar) { Value = trangThai },
                new SqlParameter("@TongTien", SqlDbType.Decimal) { Value = tongTien }
            );
        }

        /// <summary>Xóa lượt gửi xe - xử lý FK theo đúng thứ tự</summary>
        public static int DeleteLuotGuiXe(string maLuotGui)
        {
            // LuotGuiXe -> SuCoBaiXe
            //           -> ThanhToanNgay (-> ThanhToan cha sẽ cascade)
            DatabaseHelper.ExecuteTransaction(new System.Collections.Generic.List<(string, SqlParameter[])>
            {
                // 1. Xóa SuCoBaiXe
                ("DELETE FROM SuCoBaiXe WHERE MaLuotGui = @MaLuotGui",
                    new[] { new SqlParameter("@MaLuotGui", SqlDbType.VarChar) { Value = maLuotGui } }),

                // 2. Xóa ThanhToanNgay (ThanhToan cha sẽ tự cascade xóa do ON DELETE CASCADE)
                ("DELETE FROM ThanhToanNgay WHERE MaLuotGui = @MaLuotGui",
                    new[] { new SqlParameter("@MaLuotGui", SqlDbType.VarChar) { Value = maLuotGui } }),

                // 3. Xóa LuotGuiXe
                ("DELETE FROM LuotGuiXe WHERE MaLuotGui = @MaLuotGui",
                    new[] { new SqlParameter("@MaLuotGui", SqlDbType.VarChar) { Value = maLuotGui } }),
            });
            return 1;
        }

        #endregion

        #region ============= THỐNG KÊ - STATISTICS =============

        /// <summary>Lấy doanh thu theo tháng trong năm hiện tại</summary>
        public static DataTable GetMonthlyRevenue(int year)
        {
            string sql = @"
                SELECT MONTH(NgayThanhToan) AS Thang, SUM(SoTien) AS DoanhThu
                FROM ThanhToan
                WHERE YEAR(NgayThanhToan) = @Year AND TrangThai = N'Thành công'
                GROUP BY MONTH(NgayThanhToan)
                ORDER BY Thang";

            return DatabaseHelper.ExecuteQuery(sql,
                new SqlParameter("@Year", SqlDbType.Int) { Value = year }
            );
        }

        /// <summary>Lấy thống kê loại xe đang có trong bãi</summary>
        public static DataTable GetVehicleTypeStatistics()
        {
            string sql = @"
                SELECT l.TenLoaiXe, COUNT(x.MaXe) AS SoLuong
                FROM Xe x
                LEFT JOIN LoaiXe l ON x.MaLoaiXe = l.MaLoaiXe
                WHERE x.TrangThai = N'Đang sử dụng'
                GROUP BY l.TenLoaiXe";

            return DatabaseHelper.ExecuteQuery(sql);
        }

        /// <summary>Lấy số xe đang gửi trong bãi theo loại</summary>
        public static DataTable GetCurrentParkedVehicles()
        {
            string sql = @"
                SELECT l.TenLoaiXe, COUNT(DISTINCT l.MaLuotGui) AS SoXeDangDo
                FROM LuotGuiXe l
                LEFT JOIN TheXe t ON l.MaThe = t.MaThe
                LEFT JOIN Xe x ON t.MaXe = x.MaXe
                LEFT JOIN LoaiXe lt ON x.MaLoaiXe = lt.MaLoaiXe
                WHERE l.TrangThaiLuotGui = N'Trong bãi'
                GROUP BY lt.TenLoaiXe";

            return DatabaseHelper.ExecuteQuery(sql);
        }

        /// <summary>Lấy tổng doanh thu</summary>
        public static decimal GetTotalRevenue()
        {
            string sql = @"
                SELECT ISNULL(SUM(SoTien), 0)
                FROM ThanhToan
                WHERE TrangThai = N'Thành công'";

            object result = DatabaseHelper.ExecuteScalar(sql);
            return Convert.ToDecimal(result ?? 0);
        }

        /// <summary>Lấy doanh thu hôm nay</summary>
        public static decimal GetTodayRevenue()
        {
            string sql = @"
                SELECT ISNULL(SUM(SoTien), 0)
                FROM ThanhToan
                WHERE CAST(NgayThanhToan AS DATE) = CAST(GETDATE() AS DATE)
                  AND TrangThai = N'Thành công'";

            object result = DatabaseHelper.ExecuteScalar(sql);
            return Convert.ToDecimal(result ?? 0);
        }

        #endregion

        #region ============= VAI TRÒ - ROLE CRUD =============

        public static DataTable GetAllVaiTro()
        {
            return DatabaseHelper.ExecuteQuery(
                "SELECT MaVaiTro, TenVaiTro, MoTa, TrangThai FROM VaiTro ORDER BY TRY_CAST(SUBSTRING(MaVaiTro, PATINDEX('%[0-9]%', MaVaiTro), LEN(MaVaiTro)) AS INT), MaVaiTro");
        }

        public static int AddVaiTro(string maVaiTro, string tenVaiTro, string moTa, string trangThai)
        {
            string sql = @"INSERT INTO VaiTro (MaVaiTro, TenVaiTro, MoTa, TrangThai)
                           VALUES (@MaVaiTro, @TenVaiTro, @MoTa, @TrangThai)";
            return DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MaVaiTro",   SqlDbType.VarChar)  { Value = maVaiTro },
                new SqlParameter("@TenVaiTro",  SqlDbType.NVarChar) { Value = tenVaiTro },
                new SqlParameter("@MoTa",       SqlDbType.NVarChar) { Value = moTa ?? "" },
                new SqlParameter("@TrangThai",  SqlDbType.NVarChar) { Value = trangThai ?? "Đang hoạt động" });
        }

        public static int UpdateVaiTro(string maVaiTro, string tenVaiTro, string moTa, string trangThai)
        {
            string sql = @"UPDATE VaiTro SET TenVaiTro=@TenVaiTro, MoTa=@MoTa, TrangThai=@TrangThai
                           WHERE MaVaiTro=@MaVaiTro";
            return DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MaVaiTro",   SqlDbType.VarChar)  { Value = maVaiTro },
                new SqlParameter("@TenVaiTro",  SqlDbType.NVarChar) { Value = tenVaiTro },
                new SqlParameter("@MoTa",       SqlDbType.NVarChar) { Value = moTa ?? "" },
                new SqlParameter("@TrangThai",  SqlDbType.NVarChar) { Value = trangThai });
        }

        /// <summary>Xóa vai trò - kiểm tra FK NhanVien và CuDan trước</summary>
        public static int DeleteVaiTro(string maVaiTro)
        {
            // Kiểm tra còn NhanVien hay CuDan đang dùng vai trò này không
            object cntNV = DatabaseHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM NhanVien WHERE MaVaiTro=@M",
                new SqlParameter("@M", SqlDbType.VarChar) { Value = maVaiTro });
            object cntCD = DatabaseHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM CuDan WHERE MaVaiTro=@M",
                new SqlParameter("@M", SqlDbType.VarChar) { Value = maVaiTro });
            int usedNV = Convert.ToInt32(cntNV ?? 0);
            int usedCD = Convert.ToInt32(cntCD ?? 0);
            if (usedNV + usedCD > 0)
                throw new Exception(
                    $"Không thể xóa: Vai trò đang được dùng bởi {usedNV} nhân viên và {usedCD} cư dân.\n" +
                    "Hãy đổi vai trò của họ trước khi xóa.");
            return DatabaseHelper.ExecuteNonQuery(
                "DELETE FROM VaiTro WHERE MaVaiTro=@M",
                new SqlParameter("@M", SqlDbType.VarChar) { Value = maVaiTro });
        }

        #endregion

        #region ============= CĂN HỘ - APARTMENT CRUD =============

        public static DataTable GetAllCanHo()
        {
            return DatabaseHelper.ExecuteQuery(
                "SELECT MaCanHo, SoCanHo, ToaNha, Tang, TrangThai, GhiChu FROM CanHo ORDER BY ToaNha, TRY_CAST(SUBSTRING(SoCanHo, PATINDEX('%[0-9]%', SoCanHo), LEN(SoCanHo)) AS INT), SoCanHo");
        }

        public static int AddCanHo(string maCanHo, string soCanHo, string toaNha, int tang, string trangThai, string ghiChu)
        {
            string sql = @"INSERT INTO CanHo (MaCanHo, SoCanHo, ToaNha, Tang, TrangThai, GhiChu)
                           VALUES (@MaCanHo, @SoCanHo, @ToaNha, @Tang, @TrangThai, @GhiChu)";
            return DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MaCanHo",   SqlDbType.VarChar)  { Value = maCanHo },
                new SqlParameter("@SoCanHo",   SqlDbType.VarChar)  { Value = soCanHo },
                new SqlParameter("@ToaNha",    SqlDbType.NVarChar) { Value = toaNha ?? "" },
                new SqlParameter("@Tang",      SqlDbType.Int)      { Value = tang },
                new SqlParameter("@TrangThai", SqlDbType.NVarChar) { Value = trangThai ?? "" },
                new SqlParameter("@GhiChu",    SqlDbType.NVarChar) { Value = ghiChu ?? "" });
        }

        public static int UpdateCanHo(string maCanHo, string soCanHo, string toaNha, int tang, string trangThai, string ghiChu)
        {
            string sql = @"UPDATE CanHo SET SoCanHo=@SoCanHo, ToaNha=@ToaNha, Tang=@Tang, TrangThai=@TrangThai, GhiChu=@GhiChu
                           WHERE MaCanHo=@MaCanHo";
            return DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MaCanHo",   SqlDbType.VarChar)  { Value = maCanHo },
                new SqlParameter("@SoCanHo",   SqlDbType.VarChar)  { Value = soCanHo },
                new SqlParameter("@ToaNha",    SqlDbType.NVarChar) { Value = toaNha ?? "" },
                new SqlParameter("@Tang",      SqlDbType.Int)      { Value = tang },
                new SqlParameter("@TrangThai", SqlDbType.NVarChar) { Value = trangThai ?? "" },
                new SqlParameter("@GhiChu",    SqlDbType.NVarChar) { Value = ghiChu ?? "" });
        }

        /// <summary>Xóa căn hộ - kiểm tra CuDan_CanHo trước</summary>
        public static int DeleteCanHo(string maCanHo)
        {
            object cnt = DatabaseHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM CuDan_CanHo WHERE MaCanHo=@M",
                new SqlParameter("@M", SqlDbType.VarChar) { Value = maCanHo });
            if (Convert.ToInt32(cnt ?? 0) > 0)
                throw new Exception("Không thể xóa: Căn hộ đang có cư dân liên kết.\nHãy xóa liên kết CuDan-CanHo trước.");
            return DatabaseHelper.ExecuteNonQuery(
                "DELETE FROM CanHo WHERE MaCanHo=@M",
                new SqlParameter("@M", SqlDbType.VarChar) { Value = maCanHo });
        }

        #endregion

        #region ============= CƯ DÂN - CĂN HỘ (CuDan_CanHo) CRUD =============

        public static DataTable GetAllCuDanCanHo()
        {
            string sql = @"
                SELECT cc.MaCuDan, cd.HoTen AS TenCuDan, cc.MaCanHo,
                       CONCAT(ch.SoCanHo, ' - ', ch.ToaNha) AS CanHo,
                       cc.VaiTroCuDan, cc.NgayBatDau, cc.NgayKetThuc
                FROM CuDan_CanHo cc
                LEFT JOIN CuDan cd ON cc.MaCuDan = cd.MaCuDan
                LEFT JOIN CanHo ch ON cc.MaCanHo = ch.MaCanHo
                ORDER BY TRY_CAST(SUBSTRING(cc.MaCuDan, PATINDEX('%[0-9]%', cc.MaCuDan), LEN(cc.MaCuDan)) AS INT), cc.MaCuDan";
            return DatabaseHelper.ExecuteQuery(sql);
        }

        public static int AddCuDanCanHo(string maCuDan, string maCanHo, string vaiTro, DateTime? ngayBatDau, DateTime? ngayKetThuc)
        {
            string sql = @"INSERT INTO CuDan_CanHo (MaCuDan, MaCanHo, VaiTroCuDan, NgayBatDau, NgayKetThuc)
                           VALUES (@MaCuDan, @MaCanHo, @VaiTro, @NgayBatDau, @NgayKetThuc)";
            return DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MaCuDan",     SqlDbType.VarChar)  { Value = maCuDan },
                new SqlParameter("@MaCanHo",     SqlDbType.VarChar)  { Value = maCanHo },
                new SqlParameter("@VaiTro",      SqlDbType.NVarChar) { Value = vaiTro ?? "" },
                new SqlParameter("@NgayBatDau",  SqlDbType.Date)     { Value = (object?)ngayBatDau ?? DBNull.Value },
                new SqlParameter("@NgayKetThuc", SqlDbType.Date)     { Value = (object?)ngayKetThuc ?? DBNull.Value });
        }

        public static int UpdateCuDanCanHo(string maCuDan, string maCanHo, string vaiTro, DateTime? ngayKetThuc)
        {
            string sql = @"UPDATE CuDan_CanHo SET VaiTroCuDan=@VaiTro, NgayKetThuc=@NgayKetThuc
                           WHERE MaCuDan=@MaCuDan AND MaCanHo=@MaCanHo";
            return DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MaCuDan",     SqlDbType.VarChar)  { Value = maCuDan },
                new SqlParameter("@MaCanHo",     SqlDbType.VarChar)  { Value = maCanHo },
                new SqlParameter("@VaiTro",      SqlDbType.NVarChar) { Value = vaiTro ?? "" },
                new SqlParameter("@NgayKetThuc", SqlDbType.Date)     { Value = (object?)ngayKetThuc ?? DBNull.Value });
        }

        public static int DeleteCuDanCanHo(string maCuDan, string maCanHo)
        {
            return DatabaseHelper.ExecuteNonQuery(
                "DELETE FROM CuDan_CanHo WHERE MaCuDan=@MaCuDan AND MaCanHo=@MaCanHo",
                new SqlParameter("@MaCuDan", SqlDbType.VarChar) { Value = maCuDan },
                new SqlParameter("@MaCanHo", SqlDbType.VarChar) { Value = maCanHo });
        }

        #endregion

        #region ============= BẢNG GIÁ - PRICE CRUD (full) =============

        public static int UpdateBangGia(string maBangGia, string maLoaiXe, string loaiTinhPhi, decimal donGia, string trangThai)
        {
            string sql = @"UPDATE BangGia SET MaLoaiXe=@MaLoaiXe, LoaiTinhPhi=@LoaiTinhPhi,
                           DonGia=@DonGia, TrangThai=@TrangThai WHERE MaBangGia=@MaBangGia";
            return DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MaBangGia",   SqlDbType.VarChar)  { Value = maBangGia },
                new SqlParameter("@MaLoaiXe",    SqlDbType.VarChar)  { Value = maLoaiXe },
                new SqlParameter("@LoaiTinhPhi", SqlDbType.NVarChar) { Value = loaiTinhPhi },
                new SqlParameter("@DonGia",      SqlDbType.Decimal)  { Value = donGia },
                new SqlParameter("@TrangThai",   SqlDbType.NVarChar) { Value = trangThai });
        }

        /// <summary>Xóa bảng giá - không có bảng con tham chiếu</summary>
        public static int DeleteBangGia(string maBangGia)
        {
            return DatabaseHelper.ExecuteNonQuery(
                "DELETE FROM BangGia WHERE MaBangGia=@M",
                new SqlParameter("@M", SqlDbType.VarChar) { Value = maBangGia });
        }

        #endregion

        #region ============= SỰ CỐ - INCIDENT CRUD =============

        public static DataTable GetAllSuCo()
        {
            string sql = @"
                SELECT s.MaSuCo, s.MaLuotGui, l.MaThe, x.BienSo, s.NoiDung,
                       s.NgayBao, s.NgayXuLy, s.TrangThai, s.ChiPhi
                FROM SuCoBaiXe s
                LEFT JOIN LuotGuiXe l ON s.MaLuotGui = l.MaLuotGui
                LEFT JOIN TheXe t ON l.MaThe = t.MaThe
                LEFT JOIN Xe x ON t.MaXe = x.MaXe
                ORDER BY s.NgayBao DESC";
            return DatabaseHelper.ExecuteQuery(sql);
        }

        public static int AddSuCo(string maLuotGui, string noiDung, decimal chiPhi)
        {
            string sql = @"INSERT INTO SuCoBaiXe (MaLuotGui, NoiDung, NgayBao, TrangThai, ChiPhi)
                           VALUES (@MaLuotGui, @NoiDung, GETDATE(), N'Đang chờ xử lý', @ChiPhi)";
            return DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MaLuotGui", SqlDbType.VarChar)  { Value = maLuotGui },
                new SqlParameter("@NoiDung",   SqlDbType.NVarChar) { Value = noiDung },
                new SqlParameter("@ChiPhi",    SqlDbType.Decimal)  { Value = chiPhi });
        }

        public static int UpdateSuCo(int maSuCo, string trangThai, decimal chiPhi, DateTime? ngayXuLy)
        {
            string sql = @"UPDATE SuCoBaiXe SET TrangThai=@TrangThai, ChiPhi=@ChiPhi, NgayXuLy=@NgayXuLy
                           WHERE MaSuCo=@MaSuCo";
            return DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MaSuCo",   SqlDbType.Int)      { Value = maSuCo },
                new SqlParameter("@TrangThai",SqlDbType.NVarChar) { Value = trangThai },
                new SqlParameter("@ChiPhi",   SqlDbType.Decimal)  { Value = chiPhi },
                new SqlParameter("@NgayXuLy", SqlDbType.DateTime) { Value = (object?)ngayXuLy ?? DBNull.Value });
        }

        public static int DeleteSuCo(int maSuCo)
        {
            return DatabaseHelper.ExecuteNonQuery(
                "DELETE FROM SuCoBaiXe WHERE MaSuCo=@M",
                new SqlParameter("@M", SqlDbType.Int) { Value = maSuCo });
        }

        #endregion

        #region ============= THANH TOÁN - PAYMENT CRUD =============

        public static DataTable GetAllThanhToan()
        {
            string sql = @"
                SELECT tt.MaThanhToan, tt.LoaiThanhToan, tt.SoTien, tt.NgayThanhToan,
                       tt.PhuongThuc, nv.HoTen AS NhanVien, tt.TrangThai, tt.GhiChu
                FROM ThanhToan tt
                LEFT JOIN NhanVien nv ON tt.MaNhanVien = nv.MaNhanVien
                ORDER BY tt.NgayThanhToan DESC, TRY_CAST(SUBSTRING(tt.MaThanhToan, PATINDEX('%[0-9]%', tt.MaThanhToan), LEN(tt.MaThanhToan)) AS INT)";
            return DatabaseHelper.ExecuteQuery(sql);
        }

        public static int AddThanhToan(string maThanhToan, string loaiThanhToan, decimal soTien,
            string phuongThuc, string maNhanVien, string trangThai, string ghiChu)
        {
            string sql = @"INSERT INTO ThanhToan (MaThanhToan, LoaiThanhToan, SoTien, NgayThanhToan, PhuongThuc, MaNhanVien, TrangThai, GhiChu)
                           VALUES (@MaThanhToan, @LoaiThanhToan, @SoTien, GETDATE(), @PhuongThuc, @MaNhanVien, @TrangThai, @GhiChu)";
            return DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MaThanhToan",   SqlDbType.VarChar)  { Value = maThanhToan },
                new SqlParameter("@LoaiThanhToan", SqlDbType.NVarChar) { Value = loaiThanhToan ?? "" },
                new SqlParameter("@SoTien",        SqlDbType.Decimal)  { Value = soTien },
                new SqlParameter("@PhuongThuc",    SqlDbType.NVarChar) { Value = phuongThuc ?? "" },
                new SqlParameter("@MaNhanVien",    SqlDbType.VarChar)  { Value = string.IsNullOrEmpty(maNhanVien) ? (object)DBNull.Value : maNhanVien },
                new SqlParameter("@TrangThai",     SqlDbType.NVarChar) { Value = trangThai ?? "Thành công" },
                new SqlParameter("@GhiChu",        SqlDbType.NVarChar) { Value = ghiChu ?? "" });
        }

        public static int UpdateThanhToan(string maThanhToan, string trangThai, string ghiChu)
        {
            string sql = @"UPDATE ThanhToan SET TrangThai=@TrangThai, GhiChu=@GhiChu WHERE MaThanhToan=@MaThanhToan";
            return DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MaThanhToan", SqlDbType.VarChar)  { Value = maThanhToan },
                new SqlParameter("@TrangThai",   SqlDbType.NVarChar) { Value = trangThai },
                new SqlParameter("@GhiChu",      SqlDbType.NVarChar) { Value = ghiChu ?? "" });
        }

        /// <summary>Xóa thanh toán - xóa ThanhToanNgay/Thang con trước (cascade đã có nhưng chắc chắn hơn)</summary>
        public static int DeleteThanhToan(string maThanhToan)
        {
            DatabaseHelper.ExecuteTransaction(new System.Collections.Generic.List<(string, SqlParameter[])>
            {
                ("DELETE FROM ThanhToanNgay  WHERE MaThanhToan=@M",
                    new[] { new SqlParameter("@M", SqlDbType.VarChar) { Value = maThanhToan } }),
                ("DELETE FROM ThanhToanThang WHERE MaThanhToan=@M",
                    new[] { new SqlParameter("@M", SqlDbType.VarChar) { Value = maThanhToan } }),
                ("DELETE FROM ThanhToan WHERE MaThanhToan=@M",
                    new[] { new SqlParameter("@M", SqlDbType.VarChar) { Value = maThanhToan } }),
            });
            return 1;
        }

        // ThanhToanNgay
        public static DataTable GetAllThanhToanNgay()
        {
            string sql = @"
                SELECT tn.MaThanhToan, tt.SoTien, tt.NgayThanhToan, tt.PhuongThuc,
                       tt.TrangThai, tn.MaLuotGui, x.BienSo
                FROM ThanhToanNgay tn
                LEFT JOIN ThanhToan tt ON tn.MaThanhToan = tt.MaThanhToan
                LEFT JOIN LuotGuiXe lg ON tn.MaLuotGui = lg.MaLuotGui
                LEFT JOIN TheXe tx ON lg.MaThe = tx.MaThe
                LEFT JOIN Xe x ON tx.MaXe = x.MaXe
                ORDER BY tt.NgayThanhToan DESC, TRY_CAST(SUBSTRING(tt.MaThanhToan, PATINDEX('%[0-9]%', tt.MaThanhToan), LEN(tt.MaThanhToan)) AS INT)";
            return DatabaseHelper.ExecuteQuery(sql);
        }

        public static int AddThanhToanNgay(string maThanhToan, string maLuotGui, decimal soTien,
            string phuongThuc, string maNhanVien)
        {
            DatabaseHelper.ExecuteTransaction(new System.Collections.Generic.List<(string, SqlParameter[])>
            {
                (@"INSERT INTO ThanhToan (MaThanhToan, LoaiThanhToan, SoTien, NgayThanhToan, PhuongThuc, MaNhanVien, TrangThai)
                   VALUES (@MaThanhToan, N'Ngày', @SoTien, GETDATE(), @PhuongThuc, @MaNhanVien, N'Thành công')",
                    new[] {
                        new SqlParameter("@MaThanhToan", SqlDbType.VarChar)  { Value = maThanhToan },
                        new SqlParameter("@SoTien",      SqlDbType.Decimal)  { Value = soTien },
                        new SqlParameter("@PhuongThuc",  SqlDbType.NVarChar) { Value = phuongThuc ?? "" },
                        new SqlParameter("@MaNhanVien",  SqlDbType.VarChar)  { Value = string.IsNullOrEmpty(maNhanVien) ? (object)DBNull.Value : maNhanVien },
                    }),
                ("INSERT INTO ThanhToanNgay (MaThanhToan, MaLuotGui) VALUES (@MaThanhToan, @MaLuotGui)",
                    new[] {
                        new SqlParameter("@MaThanhToan", SqlDbType.VarChar) { Value = maThanhToan },
                        new SqlParameter("@MaLuotGui",   SqlDbType.VarChar) { Value = maLuotGui },
                    }),
            });
            return 1;
        }

        // ThanhToanThang
        public static DataTable GetAllThanhToanThang()
        {
            string sql = @"
                SELECT ts.MaThanhToan, tt.SoTien, tt.NgayThanhToan, tt.PhuongThuc,
                       tt.TrangThai, ts.MaThe, ts.ThanhToanTuNgay, ts.ThanhToanDenNgay
                FROM ThanhToanThang ts
                LEFT JOIN ThanhToan tt ON ts.MaThanhToan = tt.MaThanhToan
                ORDER BY tt.NgayThanhToan DESC, TRY_CAST(SUBSTRING(tt.MaThanhToan, PATINDEX('%[0-9]%', tt.MaThanhToan), LEN(tt.MaThanhToan)) AS INT)";
            return DatabaseHelper.ExecuteQuery(sql);
        }

        public static int AddThanhToanThang(string maThanhToan, string maThe, decimal soTien,
            string phuongThuc, string maNhanVien, DateTime tuNgay, DateTime denNgay)
        {
            DatabaseHelper.ExecuteTransaction(new System.Collections.Generic.List<(string, SqlParameter[])>
            {
                (@"INSERT INTO ThanhToan (MaThanhToan, LoaiThanhToan, SoTien, NgayThanhToan, PhuongThuc, MaNhanVien, TrangThai)
                   VALUES (@MaThanhToan, N'Tháng', @SoTien, GETDATE(), @PhuongThuc, @MaNhanVien, N'Thành công')",
                    new[] {
                        new SqlParameter("@MaThanhToan", SqlDbType.VarChar)  { Value = maThanhToan },
                        new SqlParameter("@SoTien",      SqlDbType.Decimal)  { Value = soTien },
                        new SqlParameter("@PhuongThuc",  SqlDbType.NVarChar) { Value = phuongThuc ?? "" },
                        new SqlParameter("@MaNhanVien",  SqlDbType.VarChar)  { Value = string.IsNullOrEmpty(maNhanVien) ? (object)DBNull.Value : maNhanVien },
                    }),
                (@"INSERT INTO ThanhToanThang (MaThanhToan, MaThe, ThanhToanTuNgay, ThanhToanDenNgay)
                   VALUES (@MaThanhToan, @MaThe, @TuNgay, @DenNgay)",
                    new[] {
                        new SqlParameter("@MaThanhToan", SqlDbType.VarChar) { Value = maThanhToan },
                        new SqlParameter("@MaThe",       SqlDbType.VarChar) { Value = maThe },
                        new SqlParameter("@TuNgay",      SqlDbType.Date)    { Value = tuNgay },
                        new SqlParameter("@DenNgay",     SqlDbType.Date)    { Value = denNgay },
                    }),
            });
            return 1;
        }

        #endregion

        #region ============= LỊCH SỬ THẺ XE - CARD HISTORY =============

        public static DataTable GetAllLichSuTheXe()
        {
            string sql = @"
                SELECT ls.MaLichSu, ls.MaThe, ls.TrangThaiCu, ls.TrangThaiMoi,
                       ls.NgayCapNhat, ls.GhiChu
                FROM LichSuTheXe ls
                ORDER BY ls.NgayCapNhat DESC";
            return DatabaseHelper.ExecuteQuery(sql);
        }

        public static int AddLichSuTheXe(string maThe, string trangThaiCu, string trangThaiMoi, string ghiChu)
        {
            string sql = @"INSERT INTO LichSuTheXe (MaThe, TrangThaiCu, TrangThaiMoi, NgayCapNhat, GhiChu)
                           VALUES (@MaThe, @TrangThaiCu, @TrangThaiMoi, GETDATE(), @GhiChu)";
            return DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MaThe",       SqlDbType.VarChar)  { Value = maThe },
                new SqlParameter("@TrangThaiCu", SqlDbType.NVarChar) { Value = trangThaiCu ?? "" },
                new SqlParameter("@TrangThaiMoi",SqlDbType.NVarChar) { Value = trangThaiMoi ?? "" },
                new SqlParameter("@GhiChu",      SqlDbType.NVarChar) { Value = ghiChu ?? "" });
        }

        public static int DeleteLichSuTheXe(int maLichSu)
        {
            return DatabaseHelper.ExecuteNonQuery(
                "DELETE FROM LichSuTheXe WHERE MaLichSu=@M",
                new SqlParameter("@M", SqlDbType.Int) { Value = maLichSu });
        }

        #endregion

        #region ============= LỊCH SỬ VỊ TRÍ ĐỖ - PARKING SPOT HISTORY =============

        public static DataTable GetAllLichSuViTriDo()
        {
            string sql = @"
                SELECT ls.MaLichSu, ls.MaViTri, vt.TenViTri, ls.MaThe,
                       ls.ThoiGianBatDau, ls.ThoiGianKetThuc, ls.GhiChu
                FROM LichSuViTriDo ls
                LEFT JOIN ViTriDo vt ON ls.MaViTri = vt.MaViTri
                ORDER BY ls.ThoiGianBatDau DESC";
            return DatabaseHelper.ExecuteQuery(sql);
        }

        public static int AddLichSuViTriDo(string maViTri, string maThe, DateTime thoiGianBatDau, DateTime? thoiGianKetThuc, string ghiChu)
        {
            string sql = @"INSERT INTO LichSuViTriDo (MaViTri, MaThe, ThoiGianBatDau, ThoiGianKetThuc, GhiChu)
                           VALUES (@MaViTri, @MaThe, @ThoiGianBatDau, @ThoiGianKetThuc, @GhiChu)";
            return DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MaViTri",          SqlDbType.VarChar)  { Value = maViTri },
                new SqlParameter("@MaThe",            SqlDbType.VarChar)  { Value = maThe },
                new SqlParameter("@ThoiGianBatDau",   SqlDbType.DateTime) { Value = thoiGianBatDau },
                new SqlParameter("@ThoiGianKetThuc",  SqlDbType.DateTime) { Value = (object?)thoiGianKetThuc ?? DBNull.Value },
                new SqlParameter("@GhiChu",           SqlDbType.NVarChar) { Value = ghiChu ?? "" });
        }

        public static int DeleteLichSuViTriDo(int maLichSu)
        {
            return DatabaseHelper.ExecuteNonQuery(
                "DELETE FROM LichSuViTriDo WHERE MaLichSu=@M",
                new SqlParameter("@M", SqlDbType.Int) { Value = maLichSu });
        }

        #endregion

        #region ============= TRUY VẤN & BÁO CÁO =============

        public static DataTable GetLichSuGui(string bienSo)
        {
            var dt = new DataTable();
            using (var conn = DatabaseHelper.GetConnection())
            using (var cmd = new SqlCommand("sp_LichSuGui", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@BienSo", SqlDbType.VarChar) { Value = bienSo ?? "" });
                using (var adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        public static DataTable GetDoanhThu(DateTime tuNgay, DateTime denNgay)
        {
            try
            {
                var dt = new DataTable();
                using (var conn = DatabaseHelper.GetConnection())
                using (var cmd = new SqlCommand("sp_DoanhThu", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@TuNgay", SqlDbType.Date) { Value = tuNgay.Date });
                    cmd.Parameters.Add(new SqlParameter("@DenNgay", SqlDbType.Date) { Value = denNgay.Date });
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
                return dt;
            }
            catch (SqlException)
            {
                string sql = @"
                    SELECT 
                        COUNT(*) AS TongLuotGui,
                        SUM(TongTien) AS TongDoanhThu,
                        COUNT(CASE WHEN PhuongThucTinhPhi = N'Thẻ ngày' THEN 1 END) AS LuotTheNgay,
                        COUNT(CASE WHEN PhuongThucTinhPhi = N'Thẻ tháng' THEN 1 END) AS LuotTheThang,
                        SUM(CASE WHEN PhuongThucTinhPhi = N'Thẻ ngày' THEN TongTien ELSE 0 END) AS DoanhThuTheNgay
                    FROM LuotGuiXe
                    WHERE CAST(ThoiGianVao AS DATE) BETWEEN @TuNgay AND @DenNgay
                      AND TrangThaiLuotGui = N'Đã ra'";
                return DatabaseHelper.ExecuteQuery(sql,
                    new SqlParameter("@TuNgay", SqlDbType.Date) { Value = tuNgay.Date },
                    new SqlParameter("@DenNgay", SqlDbType.Date) { Value = denNgay.Date });
            }
        }

        public static DataTable GetTraCuuViTriTrong(string tenKhu, string loaiViTri)
        {
            try
            {
                var dt = new DataTable();
                using (var conn = DatabaseHelper.GetConnection())
                using (var cmd = new SqlCommand("sp_TraCuuViTriTrong", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@TenKhu", SqlDbType.NVarChar) { Value = tenKhu ?? "" });
                    cmd.Parameters.Add(new SqlParameter("@LoaiViTri", SqlDbType.NVarChar) { Value = loaiViTri ?? "" });
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
                return dt;
            }
            catch (SqlException)
            {
                string sql = @"
                    SELECT 
                        KV.TenKhu,
                        KV.Tang,
                        VT.MaViTri,
                        VT.TenViTri,
                        VT.GhiChu
                    FROM ViTriDo VT
                    JOIN KhuVuc KV ON VT.MaKhu = KV.MaKhu
                    WHERE VT.TrangThai = 0
                      AND KV.TenKhu LIKE '%' + @TenKhu + '%'
                      AND VT.LoaiViTri = @LoaiViTri
                    ORDER BY KV.Tang, VT.TenViTri";
                return DatabaseHelper.ExecuteQuery(sql,
                    new SqlParameter("@TenKhu", SqlDbType.NVarChar) { Value = tenKhu ?? "" },
                    new SqlParameter("@LoaiViTri", SqlDbType.NVarChar) { Value = loaiViTri ?? "" });
            }
        }

        public static int GetSoNgayConHan(string maThe)
        {
            object result = DatabaseHelper.ExecuteScalar(
                "SELECT dbo.fn_SoNgayConHan(@MaThe)",
                new SqlParameter("@MaThe", SqlDbType.VarChar) { Value = maThe ?? "" });
            return result == DBNull.Value || result == null ? 0 : Convert.ToInt32(result);
        }

        public static DataTable GetXeTrongBai(string bienSo = null, string tenKhu = null)
        {
            string sql = @"
                SELECT lg.MaLuotGui, x.BienSo, v.TenViTri, kv.TenKhu, lg.ThoiGianVao, lg.PhuongThucTinhPhi
                FROM LuotGuiXe lg
                LEFT JOIN TheXe tx ON lg.MaThe = tx.MaThe
                LEFT JOIN Xe x ON tx.MaXe = x.MaXe
                LEFT JOIN ViTriDo v ON lg.MaViTri = v.MaViTri
                LEFT JOIN KhuVuc kv ON v.MaKhu = kv.MaKhu
                WHERE lg.TrangThaiLuotGui = N'Trong bãi'
                  AND (@BienSo IS NULL OR x.BienSo LIKE '%' + @BienSo + '%')
                  AND (@TenKhu IS NULL OR kv.TenKhu LIKE '%' + @TenKhu + '%')
                ORDER BY lg.ThoiGianVao DESC";
            return DatabaseHelper.ExecuteQuery(sql,
                new SqlParameter("@BienSo", SqlDbType.VarChar) { Value = string.IsNullOrWhiteSpace(bienSo) ? (object)DBNull.Value : bienSo },
                new SqlParameter("@TenKhu", SqlDbType.NVarChar) { Value = string.IsNullOrWhiteSpace(tenKhu) ? (object)DBNull.Value : tenKhu });
        }

        public static DataTable GetCuDanXe(string keyword = null)
        {
            string sql = @"
                SELECT cd.MaCuDan, cd.HoTen, cd.SoDienThoai, x.MaXe, x.BienSo, x.HangXe, x.TenDongXe
                FROM CuDan cd
                LEFT JOIN Xe x ON cd.MaCuDan = x.MaCuDan
                WHERE (@Keyword IS NULL OR cd.HoTen LIKE '%' + @Keyword + '%' OR x.BienSo LIKE '%' + @Keyword + '%')
                ORDER BY cd.MaCuDan";
            return DatabaseHelper.ExecuteQuery(sql,
                new SqlParameter("@Keyword", SqlDbType.NVarChar) { Value = string.IsNullOrWhiteSpace(keyword) ? (object)DBNull.Value : keyword });
        }

        public static DataTable GetSuCoSummary()
        {
            string sql = @"
                SELECT 
                    COUNT(*) AS TongSuCo,
                    SUM(ChiPhi) AS TongChiPhi,
                    SUM(CASE WHEN TrangThai LIKE N'%Đang chờ%' THEN 1 ELSE 0 END) AS ChuaXuLy,
                    SUM(CASE WHEN TrangThai LIKE N'%Đã xử lý%' THEN 1 ELSE 0 END) AS DaXuLy
                FROM SuCoBaiXe";
            return DatabaseHelper.ExecuteQuery(sql);
        }

        public static DataTable GetOccupancySummary()
        {
            string sql = @"
                SELECT 
                    COUNT(*) AS TongViTri,
                    SUM(CASE WHEN TrangThai = 1 THEN 1 ELSE 0 END) AS DaDung,
                    SUM(CASE WHEN TrangThai = 0 THEN 1 ELSE 0 END) AS ConTrong,
                    CAST(SUM(CASE WHEN TrangThai = 1 THEN 1 ELSE 0 END) * 100.0 / NULLIF(COUNT(*), 0) AS DECIMAL(5,2)) AS TyLeLapDay
                FROM ViTriDo";
            return DatabaseHelper.ExecuteQuery(sql);
        }

        public static DataTable GetBaoCaoSuCo(string trangThai)
        {
            try
            {
                var dt = new DataTable();
                using (var conn = DatabaseHelper.GetConnection())
                using (var cmd = new SqlCommand("sp_BaoCaoSuCo", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@TrangThai", SqlDbType.NVarChar)
                    {
                        Value = string.IsNullOrWhiteSpace(trangThai) ? (object)DBNull.Value : trangThai
                    });
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
                return dt;
            }
            catch (SqlException)
            {
                string sql = @"
                    SELECT 
                        kv.TenKhu,
                        COUNT(sc.MaSuCo) AS TongSuCo,
                        SUM(sc.ChiPhi) AS TongChiPhi,
                        COUNT(CASE WHEN sc.TrangThai LIKE N'%Đang chờ%' THEN 1 END) AS ChuaXuLy,
                        COUNT(CASE WHEN sc.TrangThai LIKE N'%Đã xử lý%' THEN 1 END) AS DaXuLy
                    FROM SuCoBaiXe sc
                    JOIN LuotGuiXe lgx ON sc.MaLuotGui = lgx.MaLuotGui
                    JOIN ViTriDo vtd ON lgx.MaViTri = vtd.MaViTri
                    JOIN KhuVuc kv ON vtd.MaKhu = kv.MaKhu
                    WHERE @TrangThai IS NULL OR @TrangThai LIKE N'%' + sc.TrangThai + N'%'
                    GROUP BY kv.MaKhu, kv.TenKhu
                    ORDER BY TongChiPhi DESC";
                return DatabaseHelper.ExecuteQuery(sql,
                    new SqlParameter("@TrangThai", SqlDbType.NVarChar)
                    {
                        Value = string.IsNullOrWhiteSpace(trangThai) ? (object)DBNull.Value : trangThai
                    });
            }
        }

        public static DataTable GetTiLeLapDay()
        {
            string sql = @"
                SELECT MaKhu, TenKhu, dbo.fn_TiLeLapDay(MaKhu) AS PhanTramLapDay
                FROM KhuVuc
                ORDER BY MaKhu";
            return DatabaseHelper.ExecuteQuery(sql);
        }

        // ============================================================
        //  QUERY METHODS NÂNG CAO
        // ============================================================

        /// <summary>Lịch sử gửi xe nâng cao - lọc theo biển số, khoảng ngày, loại thẻ, trạng thái</summary>
        public static DataTable GetLichSuGuiNangCao(string bienSo, DateTime tuNgay, DateTime denNgay,
            string loaiThe = null, string trangThai = null)
        {
            string sql = @"
                SELECT
                    lg.MaLuotGui      AS [Mã lượt],
                    x.BienSo          AS [Biển số],
                    x.HangXe          AS [Hãng xe],
                    x.TenDongXe       AS [Dòng xe],
                    lx.TenLoaiXe      AS [Loại xe],
                    tx.LoaiThe        AS [Loại thẻ],
                    lg.ThoiGianVao    AS [Thời gian vào],
                    lg.ThoiGianRa     AS [Thời gian ra],
                    kv.TenKhu         AS [Khu vực],
                    vt.TenViTri       AS [Vị trí],
                    nv1.HoTen         AS [NV vào],
                    nv2.HoTen         AS [NV ra],
                    lg.TrangThaiLuotGui AS [Trạng thái],
                    lg.TongTien       AS [Tổng tiền]
                FROM LuotGuiXe lg
                LEFT JOIN TheXe tx  ON lg.MaThe   = tx.MaThe
                LEFT JOIN Xe x      ON tx.MaXe     = x.MaXe
                LEFT JOIN LoaiXe lx ON x.MaLoaiXe  = lx.MaLoaiXe
                LEFT JOIN ViTriDo vt ON lg.MaViTri = vt.MaViTri
                LEFT JOIN KhuVuc kv  ON vt.MaKhu   = kv.MaKhu
                LEFT JOIN NhanVien nv1 ON lg.MaNVVao = nv1.MaNhanVien
                LEFT JOIN NhanVien nv2 ON lg.MaNVRa  = nv2.MaNhanVien
                WHERE CAST(lg.ThoiGianVao AS DATE) BETWEEN @TuNgay AND @DenNgay
                  AND (@BienSo IS NULL OR x.BienSo LIKE '%' + @BienSo + '%')
                  AND (@LoaiThe IS NULL OR tx.LoaiThe = @LoaiThe)
                  AND (@TrangThai IS NULL OR lg.TrangThaiLuotGui LIKE '%' + @TrangThai + '%')
                ORDER BY lg.ThoiGianVao DESC";
            return DatabaseHelper.ExecuteQuery(sql,
                new SqlParameter("@BienSo",    SqlDbType.VarChar)  { Value = string.IsNullOrWhiteSpace(bienSo)    ? (object)DBNull.Value : bienSo },
                new SqlParameter("@TuNgay",    SqlDbType.Date)     { Value = tuNgay },
                new SqlParameter("@DenNgay",   SqlDbType.Date)     { Value = denNgay },
                new SqlParameter("@LoaiThe",   SqlDbType.NVarChar) { Value = string.IsNullOrWhiteSpace(loaiThe)   ? (object)DBNull.Value : loaiThe },
                new SqlParameter("@TrangThai", SqlDbType.NVarChar) { Value = string.IsNullOrWhiteSpace(trangThai) ? (object)DBNull.Value : trangThai });
        }

        /// <summary>Tra cứu vị trí trống nâng cao - lọc theo khu, loại, tầng</summary>
        public static DataTable GetTraCuuViTriTrongNangCao(string tenKhu, string loaiViTri, int? tang = null)
        {
            string sql = @"
                SELECT
                    kv.MaKhu          AS [Mã khu],
                    kv.TenKhu         AS [Tên khu],
                    kv.Tang           AS [Tầng],
                    vt.MaViTri        AS [Mã vị trí],
                    vt.TenViTri       AS [Tên vị trí],
                    vt.LoaiViTri      AS [Loại vị trí],
                    kv.SucChuaToiDa   AS [Sức chứa khu]
                FROM ViTriDo vt
                JOIN KhuVuc kv ON vt.MaKhu = kv.MaKhu
                WHERE vt.TrangThai = 0
                  AND (@TenKhu IS NULL OR kv.TenKhu LIKE '%' + @TenKhu + '%')
                  AND (@LoaiViTri IS NULL OR vt.LoaiViTri = @LoaiViTri)
                  AND (@Tang IS NULL OR kv.Tang = @Tang)
                ORDER BY kv.Tang, kv.TenKhu, vt.TenViTri";
            return DatabaseHelper.ExecuteQuery(sql,
                new SqlParameter("@TenKhu",   SqlDbType.NVarChar) { Value = string.IsNullOrWhiteSpace(tenKhu)   ? (object)DBNull.Value : tenKhu },
                new SqlParameter("@LoaiViTri",SqlDbType.NVarChar) { Value = string.IsNullOrWhiteSpace(loaiViTri)? (object)DBNull.Value : loaiViTri },
                new SqlParameter("@Tang",     SqlDbType.Int)      { Value = tang.HasValue ? (object)tang.Value  : DBNull.Value });
        }

        /// <summary>Danh sách thẻ tháng sắp hết hạn trong N ngày</summary>
        public static DataTable GetDanhSachTheSapHetHan(int soNgay, string bienSo = null)
        {
            string sql = @"
                SELECT
                    tx.MaThe          AS [Mã thẻ],
                    tx.SoThe          AS [Số thẻ],
                    tx.LoaiThe        AS [Loại thẻ],
                    x.BienSo          AS [Biển số],
                    x.HangXe          AS [Hãng xe],
                    x.TenDongXe       AS [Dòng xe],
                    lx.TenLoaiXe      AS [Loại xe],
                    cd.HoTen          AS [Chủ xe],
                    cd.SoDienThoai    AS [Số điện thoại],
                    tx.NgayHetHan     AS [Ngày hết hạn],
                    DATEDIFF(DAY, GETDATE(), tx.NgayHetHan) AS [Số ngày còn lại],
                    tx.TrangThai      AS [Trạng thái thẻ]
                FROM TheXe tx
                LEFT JOIN Xe x      ON tx.MaXe     = x.MaXe
                LEFT JOIN LoaiXe lx ON x.MaLoaiXe  = lx.MaLoaiXe
                LEFT JOIN CuDan cd  ON x.MaCuDan   = cd.MaCuDan
                WHERE tx.LoaiThe = N'Thẻ tháng'
                  AND tx.NgayHetHan IS NOT NULL
                  AND DATEDIFF(DAY, GETDATE(), tx.NgayHetHan) BETWEEN 0 AND @SoNgay
                  AND (@BienSo IS NULL OR x.BienSo LIKE '%' + @BienSo + '%')
                ORDER BY tx.NgayHetHan ASC";
            return DatabaseHelper.ExecuteQuery(sql,
                new SqlParameter("@SoNgay", SqlDbType.Int)     { Value = soNgay },
                new SqlParameter("@BienSo", SqlDbType.VarChar) { Value = string.IsNullOrWhiteSpace(bienSo) ? (object)DBNull.Value : bienSo });
        }

        /// <summary>Xe đang trong bãi - nâng cao lọc theo loại thẻ và loại xe</summary>
        public static DataTable GetXeTrongBaiNangCao(string bienSo, string tenKhu,
            string loaiThe = null, string tenLoaiXe = null)
        {
            string sql = @"
                SELECT
                    lg.MaLuotGui      AS [Mã lượt],
                    x.BienSo          AS [Biển số],
                    x.HangXe          AS [Hãng xe],
                    x.TenDongXe       AS [Dòng xe],
                    lx.TenLoaiXe      AS [Loại xe],
                    tx.LoaiThe        AS [Loại thẻ],
                    kv.TenKhu         AS [Khu vực],
                    vt.TenViTri       AS [Vị trí],
                    lg.ThoiGianVao    AS [Thời gian vào],
                    DATEDIFF(MINUTE, lg.ThoiGianVao, GETDATE()) AS [Số phút đã đỗ],
                    cd.HoTen          AS [Chủ xe],
                    cd.SoDienThoai    AS [SĐT chủ xe],
                    nv1.HoTen         AS [NV quét vào]
                FROM LuotGuiXe lg
                LEFT JOIN TheXe tx   ON lg.MaThe    = tx.MaThe
                LEFT JOIN Xe x       ON tx.MaXe      = x.MaXe
                LEFT JOIN LoaiXe lx  ON x.MaLoaiXe   = lx.MaLoaiXe
                LEFT JOIN CuDan cd   ON x.MaCuDan    = cd.MaCuDan
                LEFT JOIN ViTriDo vt ON lg.MaViTri   = vt.MaViTri
                LEFT JOIN KhuVuc kv  ON vt.MaKhu     = kv.MaKhu
                LEFT JOIN NhanVien nv1 ON lg.MaNVVao = nv1.MaNhanVien
                WHERE lg.TrangThaiLuotGui = N'Trong bãi'
                  AND (@BienSo  IS NULL OR x.BienSo   LIKE '%' + @BienSo  + '%')
                  AND (@TenKhu  IS NULL OR kv.TenKhu  LIKE '%' + @TenKhu  + '%')
                  AND (@LoaiThe IS NULL OR tx.LoaiThe  = @LoaiThe)
                  AND (@TenLoaiXe IS NULL OR lx.TenLoaiXe = @TenLoaiXe)
                ORDER BY lg.ThoiGianVao DESC";
            return DatabaseHelper.ExecuteQuery(sql,
                new SqlParameter("@BienSo",    SqlDbType.VarChar)  { Value = string.IsNullOrWhiteSpace(bienSo)     ? (object)DBNull.Value : bienSo },
                new SqlParameter("@TenKhu",    SqlDbType.NVarChar) { Value = string.IsNullOrWhiteSpace(tenKhu)     ? (object)DBNull.Value : tenKhu },
                new SqlParameter("@LoaiThe",   SqlDbType.NVarChar) { Value = string.IsNullOrWhiteSpace(loaiThe)    ? (object)DBNull.Value : loaiThe },
                new SqlParameter("@TenLoaiXe", SqlDbType.NVarChar) { Value = string.IsNullOrWhiteSpace(tenLoaiXe)  ? (object)DBNull.Value : tenLoaiXe });
        }

        /// <summary>Cư dân - xe nâng cao lọc theo loại xe, trạng thái cư dân, tòa nhà</summary>
        public static DataTable GetCuDanXeNangCao(string keyword, string tenLoaiXe = null,
            string trangThaiCuDan = null, string toaNha = null)
        {
            string sql = @"
                SELECT
                    cd.MaCuDan        AS [Mã cư dân],
                    cd.HoTen          AS [Họ tên],
                    cd.CCCD           AS [CCCD],
                    cd.SoDienThoai    AS [Số điện thoại],
                    cd.DiaChiCanHo    AS [Địa chỉ căn hộ],
                    cd.TrangThai      AS [Trạng thái cư dân],
                    ch.ToaNha         AS [Tòa nhà],
                    ch.SoCanHo        AS [Số căn hộ],
                    x.BienSo          AS [Biển số],
                    x.HangXe          AS [Hãng xe],
                    x.TenDongXe       AS [Dòng xe],
                    x.MauXe           AS [Màu xe],
                    lx.TenLoaiXe      AS [Loại xe],
                    tx.LoaiThe        AS [Loại thẻ],
                    tx.TrangThai      AS [Trạng thái thẻ]
                FROM CuDan cd
                LEFT JOIN CuDan_CanHo cc ON cd.MaCuDan = cc.MaCuDan AND cc.NgayKetThuc IS NULL
                LEFT JOIN CanHo ch       ON cc.MaCanHo = ch.MaCanHo
                LEFT JOIN Xe x           ON cd.MaCuDan = x.MaCuDan
                LEFT JOIN LoaiXe lx      ON x.MaLoaiXe = lx.MaLoaiXe
                LEFT JOIN TheXe tx       ON x.MaXe     = tx.MaXe
                WHERE (@Keyword    IS NULL OR cd.HoTen  LIKE '%' + @Keyword    + '%'
                                          OR x.BienSo   LIKE '%' + @Keyword    + '%')
                  AND (@TenLoaiXe    IS NULL OR lx.TenLoaiXe = @TenLoaiXe)
                  AND (@TrangThai    IS NULL OR cd.TrangThai  LIKE '%' + @TrangThai + '%')
                  AND (@ToaNha       IS NULL OR ch.ToaNha     LIKE '%' + @ToaNha + '%')
                ORDER BY cd.MaCuDan";
            return DatabaseHelper.ExecuteQuery(sql,
                new SqlParameter("@Keyword",    SqlDbType.NVarChar) { Value = string.IsNullOrWhiteSpace(keyword)        ? (object)DBNull.Value : keyword },
                new SqlParameter("@TenLoaiXe",  SqlDbType.NVarChar) { Value = string.IsNullOrWhiteSpace(tenLoaiXe)      ? (object)DBNull.Value : tenLoaiXe },
                new SqlParameter("@TrangThai",  SqlDbType.NVarChar) { Value = string.IsNullOrWhiteSpace(trangThaiCuDan) ? (object)DBNull.Value : trangThaiCuDan },
                new SqlParameter("@ToaNha",     SqlDbType.NVarChar) { Value = string.IsNullOrWhiteSpace(toaNha)         ? (object)DBNull.Value : toaNha });
        }

        /// <summary>Lịch sử thanh toán lọc theo ngày, loại, phương thức</summary>
        public static DataTable GetLichSuThanhToan(DateTime tuNgay, DateTime denNgay,
            string loaiThanhToan = null, string phuongThuc = null)
        {
            string sql = @"
                SELECT
                    tt.MaThanhToan    AS [Mã TT],
                    tt.LoaiThanhToan  AS [Loại TT],
                    tt.SoTien         AS [Số tiền],
                    tt.NgayThanhToan  AS [Ngày thanh toán],
                    tt.PhuongThuc     AS [Phương thức],
                    tt.TrangThai      AS [Trạng thái],
                    nv.HoTen          AS [Nhân viên thu],
                    -- Thông tin thẻ ngày
                    ttn.MaLuotGui     AS [Mã lượt gửi],
                    x1.BienSo         AS [Biển số (ngày)],
                    -- Thông tin thẻ tháng
                    ttt.MaThe         AS [Mã thẻ],
                    x2.BienSo         AS [Biển số (tháng)],
                    ttt.ThanhToanTuNgay AS [Từ ngày],
                    ttt.ThanhToanDenNgay AS [Đến ngày]
                FROM ThanhToan tt
                LEFT JOIN NhanVien nv ON tt.MaNhanVien = nv.MaNhanVien
                LEFT JOIN ThanhToanNgay ttn ON tt.MaThanhToan = ttn.MaThanhToan
                LEFT JOIN LuotGuiXe lg      ON ttn.MaLuotGui  = lg.MaLuotGui
                LEFT JOIN TheXe tx1         ON lg.MaThe        = tx1.MaThe
                LEFT JOIN Xe x1             ON tx1.MaXe        = x1.MaXe
                LEFT JOIN ThanhToanThang ttt ON tt.MaThanhToan = ttt.MaThanhToan
                LEFT JOIN TheXe tx2          ON ttt.MaThe      = tx2.MaThe
                LEFT JOIN Xe x2              ON tx2.MaXe       = x2.MaXe
                WHERE CAST(tt.NgayThanhToan AS DATE) BETWEEN @TuNgay AND @DenNgay
                  AND (@LoaiTT   IS NULL OR tt.LoaiThanhToan = @LoaiTT)
                  AND (@PhuongThuc IS NULL OR tt.PhuongThuc  = @PhuongThuc)
                ORDER BY tt.NgayThanhToan DESC";
            return DatabaseHelper.ExecuteQuery(sql,
                new SqlParameter("@TuNgay",     SqlDbType.Date)     { Value = tuNgay },
                new SqlParameter("@DenNgay",    SqlDbType.Date)     { Value = denNgay },
                new SqlParameter("@LoaiTT",     SqlDbType.NVarChar) { Value = string.IsNullOrWhiteSpace(loaiThanhToan) ? (object)DBNull.Value : loaiThanhToan },
                new SqlParameter("@PhuongThuc", SqlDbType.NVarChar) { Value = string.IsNullOrWhiteSpace(phuongThuc)    ? (object)DBNull.Value : phuongThuc });
        }

        /// <summary>Tra cứu sự cố nâng cao lọc theo nội dung, trạng thái, khoảng ngày</summary>
        public static DataTable GetSuCoNangCao(string keyword, string trangThai,
            DateTime tuNgay, DateTime denNgay)
        {
            string sql = @"
                SELECT
                    sc.MaSuCo         AS [Mã sự cố],
                    sc.NoiDung        AS [Nội dung],
                    sc.TrangThai      AS [Trạng thái],
                    sc.ChiPhi         AS [Chi phí xử lý],
                    sc.NgayBao        AS [Ngày báo],
                    sc.NgayXuLy       AS [Ngày xử lý],
                    kv.TenKhu         AS [Khu vực],
                    vt.TenViTri       AS [Vị trí],
                    x.BienSo          AS [Biển số xe],
                    cd.HoTen          AS [Chủ xe],
                    lg.MaLuotGui      AS [Mã lượt gửi]
                FROM SuCoBaiXe sc
                JOIN LuotGuiXe lg  ON sc.MaLuotGui = lg.MaLuotGui
                LEFT JOIN ViTriDo vt ON lg.MaViTri = vt.MaViTri
                LEFT JOIN KhuVuc kv  ON vt.MaKhu   = kv.MaKhu
                LEFT JOIN TheXe tx   ON lg.MaThe   = tx.MaThe
                LEFT JOIN Xe x       ON tx.MaXe     = x.MaXe
                LEFT JOIN CuDan cd   ON x.MaCuDan   = cd.MaCuDan
                WHERE CAST(sc.NgayBao AS DATE) BETWEEN @TuNgay AND @DenNgay
                  AND (@Keyword  IS NULL OR sc.NoiDung  LIKE '%' + @Keyword  + '%')
                  AND (@TrangThai IS NULL OR sc.TrangThai LIKE '%' + @TrangThai + '%')
                ORDER BY sc.NgayBao DESC";
            return DatabaseHelper.ExecuteQuery(sql,
                new SqlParameter("@Keyword",   SqlDbType.NVarChar) { Value = string.IsNullOrWhiteSpace(keyword)    ? (object)DBNull.Value : keyword },
                new SqlParameter("@TrangThai", SqlDbType.NVarChar) { Value = string.IsNullOrWhiteSpace(trangThai)  ? (object)DBNull.Value : trangThai },
                new SqlParameter("@TuNgay",    SqlDbType.Date)     { Value = tuNgay },
                new SqlParameter("@DenNgay",   SqlDbType.Date)     { Value = denNgay });
        }

        // ============================================================
        //  THỐNG KÊ NÂNG CAO
        // ============================================================

        /// <summary>Doanh thu chi tiết từng lượt gửi trong khoảng thời gian</summary>
        public static DataTable GetDoanhThuChiTiet(DateTime tuNgay, DateTime denNgay)
        {
            string sql = @"
                SELECT
                    lg.MaLuotGui          AS [Mã lượt],
                    x.BienSo              AS [Biển số],
                    lx.TenLoaiXe          AS [Loại xe],
                    tx.LoaiThe            AS [Loại thẻ],
                    lg.ThoiGianVao        AS [Giờ vào],
                    lg.ThoiGianRa         AS [Giờ ra],
                    DATEDIFF(MINUTE, lg.ThoiGianVao, lg.ThoiGianRa) AS [Phút đỗ],
                    lg.TongTien           AS [Doanh thu (VNĐ)],
                    tt.PhuongThuc         AS [Phương thức TT],
                    nv.HoTen              AS [NV thu tiền]
                FROM LuotGuiXe lg
                LEFT JOIN TheXe tx   ON lg.MaThe     = tx.MaThe
                LEFT JOIN Xe x       ON tx.MaXe       = x.MaXe
                LEFT JOIN LoaiXe lx  ON x.MaLoaiXe    = lx.MaLoaiXe
                LEFT JOIN ThanhToanNgay ttn ON ttn.MaLuotGui = lg.MaLuotGui
                LEFT JOIN ThanhToan tt      ON ttn.MaThanhToan = tt.MaThanhToan
                LEFT JOIN NhanVien nv       ON tt.MaNhanVien   = nv.MaNhanVien
                WHERE CAST(lg.ThoiGianVao AS DATE) BETWEEN @TuNgay AND @DenNgay
                  AND lg.TrangThaiLuotGui = N'Đã ra'
                ORDER BY lg.ThoiGianVao DESC";
            return DatabaseHelper.ExecuteQuery(sql,
                new SqlParameter("@TuNgay",  SqlDbType.Date) { Value = tuNgay },
                new SqlParameter("@DenNgay", SqlDbType.Date) { Value = denNgay });
        }

        /// <summary>Thống kê công suất theo khu vực</summary>
        public static DataTable GetThongKeKhuVuc()
        {
            string sql = @"
                SELECT
                    kv.MaKhu                                                        AS [Mã khu],
                    kv.TenKhu                                                       AS [Tên khu],
                    kv.Tang                                                         AS [Tầng],
                    kv.SucChuaToiDa                                                 AS [Sức chứa tối đa],
                    COUNT(vt.MaViTri)                                               AS [Tổng vị trí],
                    SUM(CASE WHEN vt.TrangThai = 1 THEN 1 ELSE 0 END)              AS [Đang dùng],
                    SUM(CASE WHEN vt.TrangThai = 0 THEN 1 ELSE 0 END)              AS [Còn trống],
                    CAST(
                        SUM(CASE WHEN vt.TrangThai = 1 THEN 1 ELSE 0 END) * 100.0
                        / NULLIF(COUNT(vt.MaViTri), 0)
                    AS DECIMAL(5,1))                                                AS [Tỷ lệ lấp đầy %]
                FROM KhuVuc kv
                LEFT JOIN ViTriDo vt ON kv.MaKhu = vt.MaKhu
                GROUP BY kv.MaKhu, kv.TenKhu, kv.Tang, kv.SucChuaToiDa
                ORDER BY kv.Tang, kv.TenKhu";
            return DatabaseHelper.ExecuteQuery(sql);
        }

        /// <summary>Thống kê hoạt động nhân viên - số lượt vào/ra đã xử lý, tổng thu</summary>
        public static DataTable GetThongKeNhanVien(string caLamViec = null)
        {
            string sql = @"
                SELECT
                    nv.MaNhanVien                                   AS [Mã NV],
                    nv.HoTen                                        AS [Họ tên],
                    nv.CaLamViec                                    AS [Ca làm],
                    nv.Luong                                        AS [Lương (VNĐ)],
                    COUNT(DISTINCT lg_vao.MaLuotGui)                AS [Lượt quét vào],
                    COUNT(DISTINCT lg_ra.MaLuotGui)                 AS [Lượt quét ra],
                    ISNULL(SUM(tt.SoTien), 0)                       AS [Tổng thu (VNĐ)]
                FROM NhanVien nv
                LEFT JOIN LuotGuiXe lg_vao ON nv.MaNhanVien = lg_vao.MaNVVao
                LEFT JOIN LuotGuiXe lg_ra  ON nv.MaNhanVien = lg_ra.MaNVRa
                LEFT JOIN ThanhToan tt     ON nv.MaNhanVien = tt.MaNhanVien
                WHERE nv.MaVaiTro = 'NV'
                  AND (@Ca IS NULL OR nv.CaLamViec = @Ca)
                GROUP BY nv.MaNhanVien, nv.HoTen, nv.CaLamViec, nv.Luong
                ORDER BY [Lượt quét vào] DESC";
            return DatabaseHelper.ExecuteQuery(sql,
                new SqlParameter("@Ca", SqlDbType.NVarChar) { Value = string.IsNullOrWhiteSpace(caLamViec) ? (object)DBNull.Value : caLamViec });
        }

        /// <summary>Thống kê xe theo nhóm (loại xe / hãng xe / màu xe / năm sản xuất)</summary>
        public static DataTable GetThongKeXe(string nhom)
        {
            string groupCol;
            string groupLabel;
            switch (nhom)
            {
                case "Hãng xe":           groupCol = "x.HangXe";         groupLabel = "Hãng xe"; break;
                case "Màu xe":            groupCol = "x.MauXe";           groupLabel = "Màu xe"; break;
                case "Năm sản xuất":      groupCol = "CAST(x.NamSanXuat AS NVARCHAR)"; groupLabel = "Năm sản xuất"; break;
                default:                  groupCol = "lx.TenLoaiXe";      groupLabel = "Loại xe"; break;
            }

            string sql = $@"
                SELECT
                    {groupCol}                                          AS [{groupLabel}],
                    COUNT(DISTINCT x.MaXe)                             AS [Số lượng xe],
                    COUNT(DISTINCT tx.MaThe)                           AS [Số thẻ đăng ký],
                    COUNT(DISTINCT CASE WHEN lg.TrangThaiLuotGui = N'Trong bãi' THEN lg.MaLuotGui END) AS [Xe đang trong bãi],
                    ISNULL(AVG(DATEDIFF(MINUTE, lg.ThoiGianVao, lg.ThoiGianRa)), 0) AS [TG đỗ TB (phút)]
                FROM Xe x
                JOIN LoaiXe lx ON x.MaLoaiXe = lx.MaLoaiXe
                LEFT JOIN TheXe tx  ON x.MaXe   = tx.MaXe
                LEFT JOIN LuotGuiXe lg ON tx.MaThe = lg.MaThe
                GROUP BY {groupCol}
                ORDER BY [Số lượng xe] DESC";
            return DatabaseHelper.ExecuteQuery(sql);
        }

        #endregion
    }
}
