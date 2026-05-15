-- Tao dabatase
CREATE DATABASE QL_BaiDoXe;
USE QL_BaiDoXe;

-- Tao bang
-- 1. Phan quyen va nhan vien
CREATE TABLE VaiTro (
    MaVaiTro VARCHAR(10) NOT NULL,
    TenVaiTro NVARCHAR(50) NOT NULL,
    MoTa NVARCHAR(255),
    TrangThai NVARCHAR(50) DEFAULT N'Đang hoạt động',
    CONSTRAINT PK_VaiTro PRIMARY KEY (MaVaiTro)
);

CREATE TABLE NhanVien (
    MaNhanVien VARCHAR(10) NOT NULL,
    HoTen NVARCHAR(100) NOT NULL,
    NgaySinh DATE,
    DiaChi NVARCHAR(255),
    GioiTinh NVARCHAR(10),
    Email VARCHAR(100),
    SoDienThoai VARCHAR(15),
    TenDangNhap VARCHAR(50) NOT NULL,
    MatKhau VARCHAR(255) NOT NULL,
    MaVaiTro VARCHAR(10),
    CaLamViec NVARCHAR(50),
    NgayVaoLam DATE DEFAULT GETDATE(),
    Luong DECIMAL(18, 2),
    TrangThai NVARCHAR(50) DEFAULT N'Đang hoạt động',
    CONSTRAINT PK_NhanVien PRIMARY KEY (MaNhanVien),
    CONSTRAINT UQ_NhanVien_Email UNIQUE (Email),
    CONSTRAINT UQ_NhanVien_User UNIQUE (TenDangNhap),
    CONSTRAINT FK_NhanVien_VaiTro FOREIGN KEY (MaVaiTro) REFERENCES VaiTro(MaVaiTro)
);

-- 2.Cu dan va can ho
CREATE TABLE CanHo (
    MaCanHo VARCHAR(10) NOT NULL,
    SoCanHo VARCHAR(20) NOT NULL,
    ToaNha NVARCHAR(50),
    Tang INT,
    TrangThai NVARCHAR(50), -- Da thue, con trong, dang sua chua
    GhiChu NVARCHAR(255),
    CONSTRAINT PK_CanHo PRIMARY KEY (MaCanHo)
);

CREATE TABLE CuDan (
    MaCuDan VARCHAR(10) NOT NULL,
    HoTen NVARCHAR(100) NOT NULL,
    CCCD VARCHAR(20) NOT NULL,
    Email VARCHAR(100),
    SoDienThoai VARCHAR(15),
    DiaChiCanHo NVARCHAR(255),
    NgayDangKy DATETIME DEFAULT GETDATE(),
    MaVaiTro VARCHAR(10),
    TrangThai NVARCHAR(50) DEFAULT N'Đang cư trú',
    GhiChu NVARCHAR(255),
    CONSTRAINT PK_CuDan PRIMARY KEY (MaCuDan),
    CONSTRAINT UQ_CuDan_CCCD UNIQUE (CCCD),
    CONSTRAINT FK_CuDan_VaiTro FOREIGN KEY (MaVaiTro) REFERENCES VaiTro(MaVaiTro)
);

CREATE TABLE CuDan_CanHo (
    MaCuDan VARCHAR(10) NOT NULL,
    MaCanHo VARCHAR(10) NOT NULL,
    VaiTroCuDan NVARCHAR(50), -- Chu can ho, thanh vien, khach thue
    NgayBatDau DATE,
    NgayKetThuc DATE,
    CONSTRAINT PK_CuDanCanHo PRIMARY KEY (MaCuDan, MaCanHo),
    CONSTRAINT FK_CH_CuDan FOREIGN KEY (MaCuDan) REFERENCES CuDan(MaCuDan),
    CONSTRAINT FK_CH_CanHo FOREIGN KEY (MaCanHo) REFERENCES CanHo(MaCanHo)
);

-- 3.Xe va bang gia
CREATE TABLE LoaiXe (
    MaLoaiXe VARCHAR(10) NOT NULL,
    TenLoaiXe NVARCHAR(50) NOT NULL,
    MoTa NVARCHAR(255),
    GiaTienThang DECIMAL(18, 2) DEFAULT 0,
    GiaTienNgay DECIMAL(18, 2) DEFAULT 0,
    TrangThai NVARCHAR(50),
    CONSTRAINT PK_LoaiXe PRIMARY KEY (MaLoaiXe)
);

CREATE TABLE BangGia (
    MaBangGia VARCHAR(10) NOT NULL,
    MaLoaiXe VARCHAR(10),
    LoaiTinhPhi NVARCHAR(50), -- Theo ngay, theo thang
    DonGia DECIMAL(18, 2),
    NgayApDung DATE,
    TrangThai NVARCHAR(50),
    CONSTRAINT PK_BangGia PRIMARY KEY (MaBangGia),
    CONSTRAINT FK_BangGia_LoaiXe FOREIGN KEY (MaLoaiXe) REFERENCES LoaiXe(MaLoaiXe)
);

CREATE TABLE Xe (
    MaXe VARCHAR(10) NOT NULL,
    BienSo VARCHAR(20) NOT NULL,
    HangXe NVARCHAR(50),
    TenDongXe NVARCHAR(50),
    MauXe NVARCHAR(30),
    SoKhung VARCHAR(50),
    SoMay VARCHAR(50),
    NamSanXuat INT,
    NgayDangKyXe DATE,
    MaLoaiXe VARCHAR(10),
    MaCuDan VARCHAR(10),
    TrangThai NVARCHAR(50) DEFAULT N'Đang sử dụng',
    CONSTRAINT PK_Xe PRIMARY KEY (MaXe),
    CONSTRAINT FK_Xe_LoaiXe FOREIGN KEY (MaLoaiXe) REFERENCES LoaiXe(MaLoaiXe),
    CONSTRAINT FK_Xe_CuDan FOREIGN KEY (MaCuDan) REFERENCES CuDan(MaCuDan) ON DELETE SET NULL
);

-- 4.The xe
CREATE TABLE TheXe (
    MaThe VARCHAR(50) NOT NULL, -- UID RFID
    SoThe VARCHAR(20),
    MaXe VARCHAR(10),
    LoaiThe NVARCHAR(20) NOT NULL,
    NgayCap DATE DEFAULT GETDATE(),
    NgayKichHoat DATE,
    NgayHetHan DATE,
    TienCoc DECIMAL(18, 2) DEFAULT 0,
    TrangThai NVARCHAR(20) DEFAULT N'Đang hoạt động',
    GhiChu NVARCHAR(255),
    CONSTRAINT PK_TheXe PRIMARY KEY (MaThe),
    CONSTRAINT FK_TheXe_Xe FOREIGN KEY (MaXe) REFERENCES Xe(MaXe)
);

CREATE TABLE LichSuTheXe (
    MaLichSu INT IDENTITY(1,1),
    MaThe VARCHAR(50),
    TrangThaiCu NVARCHAR(50),
    TrangThaiMoi NVARCHAR(50),
    NgayCapNhat DATETIME DEFAULT GETDATE(),
    GhiChu NVARCHAR(255),
    CONSTRAINT PK_LichSuTheXe PRIMARY KEY (MaLichSu),
    CONSTRAINT FK_LichSuThe_TheXe FOREIGN KEY (MaThe) REFERENCES TheXe(MaThe)
);

-- 5.Bai do xe
CREATE TABLE KhuVuc (
    MaKhu VARCHAR(5) NOT NULL,
    TenKhu NVARCHAR(50) NOT NULL,
    Tang INT,
    MoTa NVARCHAR(255),
    SucChuaToiDa INT,
    TrangThai NVARCHAR(50),
    CONSTRAINT PK_KhuVuc PRIMARY KEY (MaKhu)
);

CREATE TABLE ViTriDo (
    MaViTri VARCHAR(5) NOT NULL,
    MaKhu VARCHAR(5),
    TenViTri VARCHAR(10) NOT NULL,
    LoaiViTri NVARCHAR(50), -- Vip, thuong, xe dien
    SucChua INT DEFAULT 1,
    TrangThai BIT DEFAULT 0,
    GhiChu NVARCHAR(255),
    CONSTRAINT PK_ViTriDo PRIMARY KEY (MaViTri),
    CONSTRAINT FK_ViTriDo_KhuVuc FOREIGN KEY (MaKhu) REFERENCES KhuVuc(MaKhu)
);

-- 6.Gui xe va su co
CREATE TABLE LuotGuiXe (
    MaLuotGui VARCHAR(20) NOT NULL,
    MaThe VARCHAR(50),
    MaViTri VARCHAR(5),
    ThoiGianVao DATETIME DEFAULT GETDATE(),
    ThoiGianRa DATETIME,
    AnhVao NVARCHAR(500), -- Luu duong dan
    AnhRa NVARCHAR(500),
    MaNVVao VARCHAR(10),
    MaNVRa VARCHAR(10),
    PhuongThucTinhPhi NVARCHAR(50), -- The thang, the ngay
    TrangThaiLuotGui NVARCHAR(50), -- Trong bai, da ra
    TongTien DECIMAL(18, 2) DEFAULT 0,
    GhiChu NVARCHAR(255),
    CONSTRAINT PK_LuotGui PRIMARY KEY (MaLuotGui),
    CONSTRAINT FK_LuotGui_TheXe FOREIGN KEY (MaThe) REFERENCES TheXe(MaThe),
    CONSTRAINT FK_LuotGui_ViTri FOREIGN KEY (MaViTri) REFERENCES ViTriDo(MaViTri),
    CONSTRAINT FK_LuotGui_NVVao FOREIGN KEY (MaNVVao) REFERENCES NhanVien(MaNhanVien),
    CONSTRAINT FK_LuotGui_NVRa FOREIGN KEY (MaNVRa) REFERENCES NhanVien(MaNhanVien)
);

CREATE TABLE SuCoBaiXe (
    MaSuCo INT IDENTITY(1,1),
    MaLuotGui VARCHAR(20),
    NoiDung NVARCHAR(MAX),
    NgayBao DATETIME DEFAULT GETDATE(),
    NgayXuLy DATETIME,
    TrangThai NVARCHAR(50), -- Dang cho xu ly, xu ly xong
    ChiPhi DECIMAL(18, 2) DEFAULT 0,
    CONSTRAINT PK_SuCo PRIMARY KEY (MaSuCo),
    CONSTRAINT FK_SuCo_LuotGui FOREIGN KEY (MaLuotGui) REFERENCES LuotGuiXe(MaLuotGui)
);

CREATE TABLE LichSuViTriDo (
    MaLichSu INT IDENTITY(1,1),
    MaViTri VARCHAR(5),
    MaThe VARCHAR(50),
    ThoiGianBatDau DATETIME,
    ThoiGianKetThuc DATETIME,
    GhiChu NVARCHAR(255),
    CONSTRAINT PK_LichSuViTri PRIMARY KEY (MaLichSu),
    CONSTRAINT FK_LichSuVT_ViTri FOREIGN KEY (MaViTri) REFERENCES ViTriDo(MaViTri),
    CONSTRAINT FK_LichSuVT_TheXe FOREIGN KEY (MaThe) REFERENCES TheXe(MaThe)
);

-- 7. Thanh toan
CREATE TABLE ThanhToan (
    MaThanhToan VARCHAR(20) NOT NULL,
    LoaiThanhToan NVARCHAR(50), -- Ngay, thang, su co
    SoTien DECIMAL(18, 2) NOT NULL,
    NgayThanhToan DATETIME DEFAULT GETDATE(),
    PhuongThuc NVARCHAR(50),
    MaNhanVien VARCHAR(10),
    TrangThai NVARCHAR(50), -- Thanh cong, dang xu ly, hoan tien
    GhiChu NVARCHAR(255),
    CONSTRAINT PK_ThanhToan PRIMARY KEY (MaThanhToan),
    CONSTRAINT FK_ThanhToan_NhanVien FOREIGN KEY (MaNhanVien) REFERENCES NhanVien(MaNhanVien)
);

CREATE TABLE ThanhToanNgay (
    MaThanhToan VARCHAR(20) NOT NULL,
    MaLuotGui VARCHAR(20),
    CONSTRAINT PK_ThanhToanNgay PRIMARY KEY (MaThanhToan),
    CONSTRAINT FK_TTNgay_Parent FOREIGN KEY (MaThanhToan) REFERENCES ThanhToan(MaThanhToan) ON DELETE CASCADE,
    CONSTRAINT FK_TTNgay_LuotGui FOREIGN KEY (MaLuotGui) REFERENCES LuotGuiXe(MaLuotGui)
);

CREATE TABLE ThanhToanThang (
    MaThanhToan VARCHAR(20) NOT NULL,
    MaThe VARCHAR(50),
    ThanhToanTuNgay DATE NOT NULL,
    ThanhToanDenNgay DATE NOT NULL,
    CONSTRAINT PK_ThanhToanThang PRIMARY KEY (MaThanhToan),
    CONSTRAINT FK_TTThang_Parent FOREIGN KEY (MaThanhToan) REFERENCES ThanhToan(MaThanhToan) ON DELETE CASCADE,
    CONSTRAINT FK_TTThang_TheXe FOREIGN KEY (MaThe) REFERENCES TheXe(MaThe)
);

-- Them du lieu
-- 1. Phan quyen va nhan vien
INSERT INTO VaiTro (MaVaiTro, TenVaiTro, MoTa) VALUES 
('AD', N'Quản trị viên', N'Toàn quyền hệ thống'),
('NV', N'Nhân viên', N'Trực bãi, kiểm soát ra vào'),
('CD', N'Cư dân', N'Cư dân chung cư');

INSERT INTO NhanVien (MaNhanVien, HoTen, NgaySinh, DiaChi, GioiTinh, Email, SoDienThoai, TenDangNhap, MatKhau, MaVaiTro, CaLamViec, Luong) VALUES
('NV001', N'Nguyễn Thành Nam', '1990-05-10', N'Vĩnh Hải, Nha Trang', N'Nam', 'nam.nt@gmail.com', '0905111222', 'admin_nam', '123', 'AD', N'Hành chính', 15000000),
('NV002', N'Lê Thị Thu Thủy', '1992-03-15', N'Vĩnh Phước, Nha Trang', N'Nữ', 'thuy.ltt@gmail.com', '0905222333', 'thuy_nv', '123', 'NV', N'Sáng', 8500000),
('NV003', N'Trần Quốc Bảo', '1995-11-20', N'Phước Long, Nha Trang', N'Nam', 'bao.tq@gmail.com', '0905333444', 'bao_nv', '123', 'NV', N'Chiều', 8500000),
('NV004', N'Phạm Hồng Đăng', '1993-08-25', N'Vĩnh Điềm Trung, Nha Trang', N'Nam', 'dang.ph@gmail.com', '0905444555', 'dang_nv', '123', 'NV', N'Tối', 9000000),
('NV005', N'Nguyễn Hoàng Kim', '1994-01-12', N'Lộc Thọ, Nha Trang', N'Nữ', 'kim.nh@gmail.com', '0905555666', 'kim_nv', '123', 'NV', N'Sáng', 8500000),
('NV006', N'Đặng Minh Khôi', '1991-07-30', N'Vạn Thạnh, Nha Trang', N'Nam', 'khoi.dm@gmail.com', '0905666777', 'khoi_nv', '123', 'NV', N'Chiều', 8500000),
('NV007', N'Vũ Phương Ly', '1996-04-05', N'Xương Huân, Nha Trang', N'Nữ', 'ly.vp@gmail.com', '0905777888', 'ly_nv', '123', 'NV', N'Tối', 9000000),
('NV008', N'Bùi Tiến Dũng', '1992-12-10', N'Vĩnh Nguyên, Nha Trang', N'Nam', 'dung.bt@gmail.com', '0905888999', 'dung_nv', '123', 'NV', N'Sáng', 8500000),
('NV009', N'Ngô Bảo Châu', '1990-10-10', N'Vĩnh Trường, Nha Trang', N'Nữ', 'chau.nb@gmail.com', '0905999000', 'chau_nv', '123', 'NV', N'Chiều', 8500000),
('NV010', N'Lý Hải Nam', '1988-06-18', N'Phước Hải, Nha Trang', N'Nam', 'nam.lh@gmail.com', '0905000111', 'nam_nv', '123', 'NV', N'Tối', 9000000),
('NV011', N'Trương Mỹ Linh', '1995-02-22', N'Vĩnh Hiệp, Nha Trang', N'Nữ', 'linh.tm@gmail.com', '0905121212', 'linh_nv', '123', 'NV', N'Sáng', 8500000),
('NV012', N'Phan Thanh Bình', '1993-09-05', N'Vĩnh Thái, Nha Trang', N'Nam', 'binh.pt@gmail.com', '0905131313', 'binh_nv', '123', 'NV', N'Chiều', 8500000),
('NV013', N'Hồ Ngọc Hà', '1985-04-15', N'Vĩnh Thạnh, Nha Trang', N'Nữ', 'ha.hn@gmail.com', '0905141414', 'ha_nv', '123', 'NV', N'Tối', 9000000),
('NV014', N'Lê Minh Anh', '1989-12-21', N'Vĩnh Trung, Nha Trang', N'Nam', 'anh.lm@gmail.com', '0905151515', 'manh_nv', '123', 'NV', N'Sáng', 8500000),
('NV015', N'Nguyễn Cao Kỳ', '1990-08-11', N'Vĩnh Ngọc, Nha Trang', N'Nam', 'ky.nc@gmail.com', '0905161616', 'ky_nv', '123', 'NV', N'Chiều', 8500000),
('NV016', N'Trịnh Kim Chi', '1987-11-02', N'Ngọc Hiệp, Nha Trang', N'Nữ', 'chi.tk@gmail.com', '0905171717', 'chi_nv', '123', 'NV', N'Tối', 9000000),
('NV017', N'Quách Ngọc Ngoan', '1988-05-03', N'Vĩnh Phương, Nha Trang', N'Nam', 'ngoan.qn@gmail.com', '0905181818', 'ngoan_nv', '123', 'NV', N'Sáng', 8500000),
('NV018', N'Vương Thu Phương', '1991-01-04', N'Vĩnh Lương, Nha Trang', N'Nữ', 'phuong.vt@gmail.com', '0905191919', 'phuong_nv', '123', 'NV', N'Chiều', 8500000),
('NV019', N'Tạ Quang Thắng', '1994-06-05', N'Phước Tiến, Nha Trang', N'Nam', 'thang.tq@gmail.com', '0905202020', 'thang_nv', '123', 'NV', N'Tối', 9000000),
('NV020', N'Mai Phương Thúy', '1992-07-06', N'Tân Lập, Nha Trang', N'Nữ', 'thuy.mp@gmail.com', '0905212121', 'mpthuy_nv', '123', 'NV', N'Sáng', 8500000),
('NV021', N'Hoàng Thùy Linh', '1995-10-07', N'Phương Sài, Nha Trang', N'Nữ', 'linh.ht@gmail.com', '0905222222', 'hlinh_nv', '123', 'NV', N'Chiều', 8500000),
('NV022', N'Nguyễn Trần Khánh Vân', '1996-03-08', N'Phước Hòa, Nha Trang', N'Nữ', 'van.ntk@gmail.com', '0905232323', 'van_nv', '123', 'NV', N'Tối', 9000000),
('NV023', N'Võ Hoàng Yến', '1993-12-09', N'Vĩnh Hải, Nha Trang', N'Nữ', 'yen.vh@gmail.com', '0905242424', 'yen_nv', '123', 'NV', N'Sáng', 8500000),
('NV024', N'Trần Thành', '1997-02-10', N'Vĩnh Phước, Nha Trang', N'Nam', 'thanh.t@gmail.com', '0905252525', 'tthanh_nv', '123', 'NV', N'Chiều', 8500000),
('NV025', N'Đặng Thanh Tùng', '1994-07-05', N'Nha Trang', N'Nam', 'tung.dt@gmail.com', '0905123480', 'tung_nv', '123', 'NV', N'Tối', 8500000),
('NV026', N'Huỳnh Lập', '1993-05-17', N'Vạn Thạnh, Nha Trang', N'Nam', 'lap.h@gmail.com', '0905272727', 'lap_nv', '123', 'NV', N'Sáng', 8500000),
('NV027', N'Diệu Nhi', '1991-05-21', N'Ngọc Hiệp, Nha Trang', N'Nữ', 'nhi.d@gmail.com', '0905282828', 'dnhi_nv', '123', 'NV', N'Chiều', 8500000),
('NV028', N'Anh Tú', '1993-10-03', N'Xương Huân, Nha Trang', N'Nam', 'tu.a@gmail.com', '0905292929', 'atu_nv', '123', 'NV', N'Tối', 9000000),
('NV029', N'Puka', '1990-10-30', N'Tân Lập, Nha Trang', N'Nữ', 'puka@gmail.com', '0905303030', 'puka_nv', '123', 'NV', N'Sáng', 8500000),
('NV030', N'Phạm Tuấn Anh', '1994-09-06', N'Phước Tiến, Nha Trang', N'Nam', 'kiet.gt@gmail.com', '0905313131', 'kiet_nv', '123', 'NV', N'Chiều', 8500000);

INSERT INTO NhanVien (MaNhanVien, HoTen, NgaySinh, DiaChi, GioiTinh, Email, SoDienThoai, TenDangNhap, MatKhau, MaVaiTro, CaLamViec, Luong) VALUES
('NV031', N'Danh', '2006-04-22', N'Vĩnh Hải, Nha Trang', N'Nam', 'danh@gmail.com', '0905000001', 'admin_danh', '123', 'AD', N'Hành chính', 15000000),
('NV032', N'Quyên', '2006-01-26', N'Diên Điền, Diên Khánh', N'Nữ', 'quyen@gmail.com', '0905000002', 'admin_quyen', '123', 'AD', N'Hành chính', 15000000),
('NV033', N'Việt', '2006-05-16', N'Vĩnh Trường, Nha Trang', N'Nam', 'viet@gmail.com', '0905000003', 'admin_viet', '123', 'AD', N'Hành chính', 15000000);
-- 2.Cu dan va can ho
INSERT INTO CanHo (MaCanHo, SoCanHo, ToaNha, Tang, TrangThai) VALUES
('CH001', 'A-101', 'Block A', 1, N'Đã thuê'), ('CH002', 'A-102', 'Block A', 1, N'Đã thuê'),
('CH003', 'A-201', 'Block A', 2, N'Đã thuê'), ('CH004', 'A-202', 'Block A', 2, N'Đã thuê'),
('CH005', 'A-301', 'Block A', 3, N'Đã thuê'), ('CH006', 'B-101', 'Block B', 1, N'Đã thuê'),
('CH007', 'B-102', 'Block B', 1, N'Đã thuê'), ('CH008', 'B-201', 'Block B', 2, N'Đã thuê'),
('CH009', 'B-202', 'Block B', 2, N'Đã thuê'), ('CH010', 'B-301', 'Block B', 3, N'Đã thuê'),
('CH011', 'A-401', 'Block A', 4, N'Đã thuê'), ('CH012', 'A-402', 'Block A', 4, N'Đã thuê'),
('CH013', 'A-501', 'Block A', 5, N'Đã thuê'), ('CH014', 'A-502', 'Block A', 5, N'Trống'),
('CH015', 'A-601', 'Block A', 6, N'Đã thuê'), ('CH016', 'B-401', 'Block B', 4, N'Đã thuê'),
('CH017', 'B-402', 'Block B', 4, N'Đã thuê'), ('CH018', 'B-501', 'Block B', 5, N'Đã thuê'),
('CH019', 'B-502', 'Block B', 5, N'Đã thuê'), ('CH020', 'B-601', 'Block B', 6, N'Đã thuê'),
('CH021', 'A-701', 'Block A', 7, N'Đã thuê'), ('CH022', 'A-702', 'Block A', 7, N'Đã thuê'),
('CH023', 'B-701', 'Block B', 7, N'Đã thuê'), ('CH024', 'B-801', 'Block B', 8, N'Đã thuê'),
('CH025', 'B-901', 'Block B', 9, N'Sửa chữa'), ('CH026', 'A-801', 'Block A', 8, N'Đã thuê'),
('CH027', 'A-802', 'Block A', 8, N'Đã thuê'), ('CH028', 'B-802', 'Block B', 8, N'Đã thuê'),
('CH029', 'A-901', 'Block A', 9, N'Đã thuê'), ('CH030', 'A-902', 'Block A', 9, N'Trống');

INSERT INTO CuDan (MaCuDan, HoTen, CCCD, Email, SoDienThoai, DiaChiCanHo, MaVaiTro) VALUES
('CD001', N'Phạm Văn Đồng', '123456789001', 'dong.pv@gmail.com', '0381000001', 'Block A - A101', 'CD'),
('CD002', N'Lê Hồng Phong', '123456789002', 'phong.lh@gmail.com', '0381000002', 'Block A - A102', 'CD'),
('CD003', N'Trần Phú', '123456789003', 'phu.t@gmail.com', '0381000003', 'Block A - A201', 'CD'),
('CD004', N'Nguyễn Thị Minh Khai', '123456789004', 'khai.ntm@gmail.com', '0381000004', 'Block A - A301', 'CD'),
('CD005', N'Võ Thị Sáu', '123456789005', 'sau.vt@gmail.com', '0381000005', 'Block B - B101', 'CD'),
('CD006', N'Nguyễn Văn Trỗi', '123456789006', 'troi.nv@gmail.com', '0381000006', 'Block B - B102', 'CD'),
('CD007', N'Lý Tự Trọng', '123456789007', 'trong.lt@gmail.com', '0381000007', 'Block B - B201', 'CD'),
('CD008', N'Kim Đồng', '123456789008', 'dong.k@gmail.com', '0381000008', 'Block B - B202', 'CD'),
('CD009', N'Phan Bội Châu', '123456789009', 'chau.pb@gmail.com', '0381000009', 'Block B - B301', 'CD'),
('CD010', N'Nguyễn Thái Học', '123456789101', 'hoc.nt@gmail.com', '0381000010', 'Block A - A401', 'CD'),
('CD011', N'Nguyễn Cao Kỳ', '123456789102', 'ky.nc@gmail.com', '0381000011', 'Block A - A402', 'CD'),
('CD012', N'Đỗ Văn Quân', '123456789103', 'quan.dv@gmail.com', '0381000012', 'Block A - A501', 'CD'),
('CD013', N'Ngô Thị Lan', '123456789104', 'lan.nt@gmail.com', '0381000013', 'Block A - A601', 'CD'),
('CD014', N'Trịnh Văn Đại', '123456789105', 'dai.tv@gmail.com', '0381000014', 'Block B - B401', 'CD'),
('CD015', N'Lưu Thị Huệ', '123456789106', 'hue.lt@gmail.com', '0381000015', 'Block B - B402', 'CD'),
('CD016', N'Mai Văn Sơn', '123456789107', 'son.mv@gmail.com', '0381000016', 'Block B - B501', 'CD'),
('CD017', N'Tạ Thị Đào', '123456789108', 'dao.tt@gmail.com', '0381000017', 'Block B - B502', 'CD'),
('CD018', N'Hà Văn Hùng', '123456789109', 'hung.hv@gmail.com', '0381000018', 'Block B - B601', 'CD'),
('CD019', N'Kiều Thị Mai', '123456789110', 'mai.kt@gmail.com', '0381000019', 'Block A - A701', 'CD'),
('CD020', N'Thân Văn Thắng', '123456789111', 'thang.tv@gmail.com', '0381000020', 'Block A - A702', 'CD'),
('CD021', N'Nguyễn Võ Quốc Việt', '123456789112', 'viet.nvq@gmail.com', '0381000021', 'Block B - B701', 'CD'),
('CD022', N'Lê Tấn Danh', '123456789113', 'danh.lt@gmail.com', '0381000022', 'Block B - B801', 'CD'),
('CD023', N'Nguyễn Hạnh Quyên', '123456789114', 'quyen.nh@gmail.com', '0381000023', 'Block B - B901', 'CD'),
('CD024', N'Trần Anh Tuấn', '123456789115', 'tuan.ta@gmail.com', '0381000024', 'Block B - B101', 'CD'),
('CD025', N'Lương Thị Hồng', '123456789116', 'hong.lt@gmail.com', '0381000025', 'Block A - A101', 'CD'),
('CD026', N'Hoàng Văn Bốn', '123456789117', 'bon.hv@gmail.com', '0381000026', 'Block A - A801', 'CD'),
('CD027', N'Phan Thị Bảy', '123456789118', 'bay.pt@gmail.com', '0381000027', 'Block A - A802', 'CD'),
('CD028', N'Vũ Văn Sáu', '123456789119', 'sau.vv@gmail.com', '0381000028', 'Block B - B802', 'CD'),
('CD029', N'Đặng Văn Hai', '123456789120', 'hai.dv@gmail.com', '0381000029', 'Block A - A901', 'CD'),
('CD030', N'Bùi Thị Một', '123456789121', 'mot.bt@gmail.com', '0381000030', 'Block A - A102', 'CD'),
('CD031', N'Cao Văn Ba', '123456789122', 'ba.cv@gmail.com', '0381000031', 'Block A - A201', 'CD'),
('CD032', N'Đỗ Minh Tâm', '123456789123', 'tam.dm@gmail.com', '0381000032', 'Block B - B102', 'CD'),
('CD033', N'Ngô Thanh Vân', '123456789124', 'van.nt@gmail.com', '0381000033', 'Block B - B201', 'CD'),
('CD034', N'Lý Hùng', '123456789125', 'hung.l@gmail.com', '0381000034', 'Block B - B202', 'CD'),
('CD035', N'Nguyễn Thị Nhi', '123456789126', 'nhi.nt@gmail.com', '0381000035', 'Block B - B301', 'CD');

INSERT INTO CuDan_CanHo (MaCuDan, MaCanHo, VaiTroCuDan, NgayBatDau) VALUES
('CD001', 'CH001', N'Chủ hộ', '2025-01-01'), ('CD025', 'CH001', N'Thành viên', '2025-01-01'),
('CD002', 'CH002', N'Chủ hộ', '2025-01-05'), ('CD030', 'CH002', N'Thành viên', '2025-01-05'),
('CD003', 'CH003', N'Chủ hộ', '2025-01-10'), ('CD031', 'CH003', N'Thành viên', '2025-01-10'),
('CD004', 'CH005', N'Chủ hộ', '2025-01-15'), ('CD005', 'CH006', N'Chủ hộ', '2025-01-20'),
('CD006', 'CH007', N'Chủ hộ', '2025-02-01'), ('CD032', 'CH007', N'Thành viên', '2025-02-01'),
('CD007', 'CH008', N'Chủ hộ', '2025-02-05'), ('CD033', 'CH008', N'Thành viên', '2025-02-05'),
('CD008', 'CH009', N'Chủ hộ', '2025-02-10'), ('CD034', 'CH009', N'Thành viên', '2025-02-10'),
('CD009', 'CH010', N'Chủ hộ', '2025-02-15'), ('CD035', 'CH010', N'Thành viên', '2025-02-15'),
('CD010', 'CH011', N'Chủ hộ', '2025-03-01'), ('CD011', 'CH012', N'Chủ hộ', '2025-03-05'),
('CD012', 'CH013', N'Chủ hộ', '2025-03-10'), ('CD013', 'CH015', N'Chủ hộ', '2025-03-15'),
('CD014', 'CH016', N'Chủ hộ', '2025-04-01'), ('CD015', 'CH017', N'Chủ hộ', '2025-04-05'),
('CD016', 'CH018', N'Chủ hộ', '2025-04-10'), ('CD017', 'CH019', N'Chủ hộ', '2025-04-15'),
('CD018', 'CH020', N'Chủ hộ', '2025-05-01'), ('CD019', 'CH021', N'Chủ hộ', '2025-05-05'),
('CD020', 'CH022', N'Chủ hộ', '2025-05-10'), ('CD021', 'CH023', N'Chủ hộ', '2025-05-15'),
('CD022', 'CH024', N'Chủ hộ', '2025-06-01'), ('CD023', 'CH025', N'Khách thuê', '2026-05-01'),
('CD026', 'CH026', N'Chủ hộ', '2025-06-05'), ('CD027', 'CH027', N'Chủ hộ', '2025-06-10'),
('CD028', 'CH028', N'Chủ hộ', '2025-06-15'), ('CD029', 'CH029', N'Chủ hộ', '2025-06-20');

-- 3. Xe va bang gia
INSERT INTO LoaiXe (MaLoaiXe, TenLoaiXe, GiaTienThang, GiaTienNgay) VALUES
('XM', N'Xe máy', 100000, 5000),
('OT', N'Ô tô', 1000000, 30000);

INSERT INTO BangGia (MaBangGia, MaLoaiXe, LoaiTinhPhi, DonGia, NgayApDung) VALUES
('BG01', 'XM', N'Thẻ tháng', 100000, '2025-01-01'),
('BG02', 'OT', N'Thẻ tháng', 1000000, '2025-01-01'),
('BG03', 'XM', N'Thẻ ngày', 5000, '2025-01-01'),
('BG04', 'OT', N'Thẻ ngày', 30000, '2025-01-01');

INSERT INTO Xe (MaXe, BienSo, HangXe, TenDongXe, MauXe, MaLoaiXe, MaCuDan, NamSanXuat) VALUES
('XE001', '79-H1 111.11', 'Honda', 'Vision', N'Trắng', 'XM', 'CD001', 2022),
('XE002', '79-H1 222.22', 'Yamaha', 'Exciter', N'Xanh', 'XM', 'CD002', 2021),
('XE003', '79-A 333.33', 'Toyota', 'Vios', N'Đen', 'OT', 'CD003', 2023),
('XE004', '79-H1 444.44', 'Honda', 'AirBlade', N'Đỏ', 'XM', 'CD004', 2020),
('XE005', '79-A 555.55', 'Hyundai', 'Accent', N'Trắng', 'OT', 'CD005', 2022),
('XE006', '79-H1 666.66', 'Honda', 'SH', N'Đen', 'XM', 'CD006', 2023),
('XE007', '79-H1 777.77', 'Suzuki', 'Raider', N'Tím', 'XM', 'CD007', 2019),
('XE008', '79-A 888.88', 'Kia', 'Cerato', N'Đỏ', 'OT', 'CD008', 2021),
('XE009', '79-H1 999.99', 'Yamaha', 'Janus', N'Hồng', 'XM', 'CD009', 2022),
('XE010', '79-H1 101.01', 'Honda', 'Wave', N'Xanh', 'XM', 'CD010', 2018),
('XE011', '79-A 202.02', 'Mazda', 'CX-5', N'Xám', 'OT', 'CD011', 2023),
('XE012', '79-H1 303.03', 'Honda', 'Lead', N'Vàng', 'XM', 'CD012', 2021),
('XE013', '79-H1 404.04', 'Piaggio', 'Vespa', N'Trắng', 'XM', 'CD013', 2022),
('XE014', '79-A 505.05', 'VinFast', 'Lux A2.0', N'Đen', 'OT', 'CD014', 2022),
('XE015', '79-H1 606.06', 'Honda', 'Future', N'Đen', 'XM', 'CD015', 2020),
('XE016', '79-H1 707.07', 'Yamaha', 'Grande', N'Trắng', 'XM', 'CD016', 2021),
('XE017', '79-A 808.08', 'Ford', 'Ranger', N'Cam', 'OT', 'CD017', 2023),
('XE018', '79-H1 909.09', 'Suzuki', 'Satria', N'Xanh', 'XM', 'CD018', 2022),
('XE019', '79-H1 010.10', 'Honda', 'Winner X', N'Bạc', 'XM', 'CD019', 2021),
('XE020', '79-A 111.21', 'Honda', 'City', N'Trắng', 'OT', 'CD020', 2022),
('XE021', '79-H1 212.12', 'Yamaha', 'NVX', N'Đen', 'XM', 'CD021', 2023),
('XE022', '79-H1 313.13', 'Honda', 'Blade', N'Đỏ', 'XM', 'CD022', 2017),
('XE023', '79-A 414.14', 'Mitsubishi', 'Xpander', N'Nâu', 'OT', 'CD023', 2023),
('XE024', '79-H1 515.15', 'Honda', 'Vario', N'Xám', 'XM', 'CD024', 2022),
('XE025', '79-H1 616.16', 'Sym', 'Attila', N'Trắng', 'XM', 'CD001', 2015),
('XE026', '79-A 717.17', 'Toyota', 'Camry', N'Đen', 'OT', 'CD026', 2023),
('XE027', '79-H1 818.18', 'Honda', 'Sonic', N'Đen', 'XM', 'CD027', 2021),
('XE028', '79-H1 919.19', 'Yamaha', 'Luvias', N'Đỏ', 'XM', 'CD028', 2016),
('XE029', '79-A 020.20', 'VinFast', 'VF8', N'Xanh', 'OT', 'CD029', 2024),
('XE030', '79-H1 121.21', 'Honda', '67', N'Đen', 'XM', 'CD030', 1967);

-- 4.The xe
INSERT INTO TheXe (MaThe, SoThe, MaXe, LoaiThe, NgayHetHan, TrangThai) VALUES
('UID001', 'A1B2C3D4', 'XE001', N'Thẻ tháng', '2026-12-31', N'Đang hoạt động'),
('UID002', '9F3A7C1E', 'XE002', N'Thẻ tháng', '2026-12-31', N'Đang hoạt động'),
('UID003', '4D8E2B9A', 'XE003', N'Thẻ tháng', '2026-12-31', N'Đang hoạt động'),
('UID004', '7C1F5A3D', 'XE004', N'Thẻ tháng', '2026-12-31', N'Đang hoạt động'),
('UID005', 'E2A4B6C8', 'XE005', N'Thẻ tháng', '2026-12-31', N'Đang hoạt động'),
('UID006', '5F9D3E1A', 'XE006', N'Thẻ tháng', '2026-12-31', N'Đang hoạt động'),
('UID007', 'B7C2D4E6', 'XE007', N'Thẻ tháng', '2026-12-31', N'Đang hoạt động'),
('UID008', '3A8F1C5B', 'XE008', N'Thẻ tháng', '2026-12-31', N'Đang hoạt động'),
('UID009', 'D9E4A2C7', 'XE009', N'Thẻ tháng', '2026-12-31', N'Đang hoạt động'),
('UID010', '6B1D3F8A', 'XE010', N'Thẻ tháng', '2026-12-31', N'Đang hoạt động'),
('UID011', 'C4E7B2A9', 'XE011', N'Thẻ tháng', '2026-12-31', N'Đang hoạt động'),
('UID012', '8A3C5F1D', 'XE012', N'Thẻ tháng', '2026-12-31', N'Đang hoạt động'),
('UID013', 'F1B2C3A4', 'XE013', N'Thẻ tháng', '2026-12-31', N'Đang hoạt động'),
('UID014', '2D6E8F1B', 'XE014', N'Thẻ tháng', '2026-12-31', N'Đang hoạt động'),
('UID015', 'A9C3E5F7', 'XE015', N'Thẻ tháng', '2026-12-31', N'Đang hoạt động'),
('UID016', '1F4B7D2A', NULL, N'Thẻ ngày', NULL, N'Đang hoạt động'),
('UID017', 'E8C2A1B3', NULL, N'Thẻ ngày', NULL, N'Đang hoạt động'),
('UID018', '7D5F3A9C', NULL, N'Thẻ ngày', NULL, N'Đang hoạt động'),
('UID019', 'B1E2C4D6', NULL, N'Thẻ ngày', NULL, N'Đang hoạt động'),
('UID020', '3F7A9E1C', NULL, N'Thẻ ngày', NULL, N'Đang hoạt động'),
('UID021', 'D2B4A6C8', 'XE021', N'Thẻ tháng', '2026-12-31', N'Đang hoạt động'),
('UID022', '9A1C3E5F', 'XE022', N'Thẻ tháng', '2026-12-31', N'Đang hoạt động'),
('UID023', '6E8F2D4B', 'XE023', N'Thẻ tháng', '2026-12-31', N'Đang hoạt động'),
('UID024', 'C7A9B3E1', 'XE024', N'Thẻ tháng', '2026-12-31', N'Đang hoạt động'),
('UID025', '5A3D7F1C', 'XE025', N'Thẻ tháng', '2026-12-31', N'Đang hoạt động'),
('UID026', '6A2D4F1E', 'XE026', N'Thẻ tháng', '2026-12-31', N'Đang hoạt động'),
('UID027', 'B1C3D5E7', 'XE027', N'Thẻ tháng', '2026-12-31', N'Đang hoạt động'),
('UID028', 'D9F1A3C5', 'XE028', N'Thẻ tháng', '2026-12-31', N'Đang hoạt động'),
('UID029', '7E2B4D6F', 'XE029', N'Thẻ tháng', '2026-12-31', N'Đang hoạt động'),
('UID030', 'A5C7E9F1', 'XE030', N'Thẻ tháng', '2026-12-31', N'Đang hoạt động');

-- 5.Khu vuc do xe
INSERT INTO KhuVuc (MaKhu, TenKhu, Tang, SucChuaToiDa) VALUES
('K01', 'Khu A-H1', -1, 100), ('K02', 'Khu B-H1', -1, 100), ('K03', 'Khu C-H1', -1, 50),
('K04', 'Khu A-H2', -2, 100), ('K05', 'Khu B-H2', -2, 100), ('K06', 'Khu C-H2', -2, 50),
('K07', 'Khu VIP', -1, 20), ('K08', 'Khu Xe Dien', -1, 30), ('K09', 'Khu Vang Lai', 1, 50),
('K10', 'Block C1', -1, 100), ('K11', 'Block C2', -1, 100), ('K12', 'Block D1', -2, 100),
('K13', 'Block D2', -2, 100), ('K14', 'Tang Tret 1', 0, 50), ('K15', 'Tang Tret 2', 0, 50),
('K16', 'Ham B3-A', -3, 80), ('K17', 'Ham B3-B', -3, 80), ('K18', 'Khu Xe Tai', 1, 10),
('K19', 'Khu Rua Xe', -1, 5), ('K20', 'Khu Bao Tri', -1, 5), ('K21', 'Cho Doi', 1, 20),
('K22', 'Block E1', -1, 100), ('K23', 'Block E2', -2, 100), ('K24', 'Khu A-H3', -3, 100), ('K25', 'Khu B-H3', -3, 100);

INSERT INTO ViTriDo (MaViTri, MaKhu, TenViTri, LoaiViTri, TrangThai) VALUES
('V001', 'K01', 'A1-01', N'Xe máy', 1), ('V002', 'K01', 'A1-02', N'Xe máy', 1),
('V003', 'K01', 'A1-03', N'Xe máy', 1), ('V004', 'K01', 'A1-04', N'Xe máy', 0),
('V005', 'K01', 'A1-05', N'Xe máy', 0), ('V006', 'K02', 'B1-01', N'Xe máy', 1),
('V007', 'K02', 'B1-02', N'Xe máy', 1), ('V008', 'K02', 'B1-03', N'Xe máy', 1),
('V009', 'K02', 'B1-04', N'Xe máy', 0), ('V010', 'K02', 'B1-05', N'Xe máy', 0),
('V011', 'K03', 'C1-01', N'Ô tô', 1), ('V012', 'K03', 'C1-02', N'Ô tô', 1),
('V013', 'K03', 'C1-03', N'Ô tô', 1), ('V014', 'K03', 'C1-04', N'Ô tô', 0),
('V015', 'K03', 'C1-05', N'Ô tô', 0), ('V016', 'K04', 'A2-01', N'Ô tô', 1),
('V017', 'K04', 'A2-02', N'Ô tô', 1), ('V018', 'K04', 'A2-03', N'Ô tô', 1),
('V019', 'K04', 'A2-04', N'Ô tô', 0), ('V020', 'K04', 'A2-05', N'Ô tô', 0),
('V021', 'K05', 'B2-01', N'Xe máy', 1), ('V022', 'K05', 'B2-02', N'Xe máy', 1),
('V023', 'K05', 'B2-03', N'Xe máy', 1), ('V024', 'K05', 'B2-04', N'Xe máy', 0),
('V025', 'K05', 'B2-05', N'Xe máy', 0), ('V026', 'K01', 'A1-06', N'Xe máy', 1),
('V027', 'K01', 'A1-07', N'Xe máy', 1), ('V028', 'K02', 'B1-06', N'Xe máy', 1),
('V029', 'K03', 'C1-06', N'Ô tô', 1), ('V030', 'K04', 'A2-06', N'Ô tô', 1);

INSERT INTO LuotGuiXe (MaLuotGui, MaThe, MaViTri, ThoiGianVao, ThoiGianRa, MaNVVao, MaNVRa, PhuongThucTinhPhi, TrangThaiLuotGui, TongTien) VALUES
('L001', 'UID001', 'V001', '2026-05-10 07:00:00', '2026-05-10 17:00:00', 'NV002', 'NV003', N'Thẻ tháng', N'Đã ra', 0),
('L002', 'UID002', 'V002', '2026-05-10 08:00:00', '2026-05-10 18:00:00', 'NV002', 'NV003', N'Thẻ tháng', N'Đã ra', 0),
('L003', 'UID003', 'V011', '2026-05-10 09:00:00', '2026-05-10 19:00:00', 'NV005', 'NV006', N'Thẻ tháng', N'Đã ra', 0),
('L004', 'UID016', 'V021', '2026-05-11 07:30:00', '2026-05-11 11:30:00', 'NV008', 'NV008', N'Thẻ ngày', N'Đã ra', 5000),
('L005', 'UID017', 'V012', '2026-05-11 08:00:00', '2026-05-11 16:00:00', 'NV008', 'NV009', N'Thẻ ngày', N'Đã ra', 30000),
('L006', 'UID004', 'V003', '2026-05-11 09:00:00', '2026-05-11 17:00:00', 'NV011', 'NV012', N'Thẻ tháng', N'Đã ra', 0),
('L007', 'UID005', 'V013', '2026-05-11 10:00:00', '2026-05-11 20:00:00', 'NV011', 'NV012', N'Thẻ tháng', N'Đã ra', 0),
('L008', 'UID018', 'V022', '2026-05-12 07:00:00', '2026-05-12 09:00:00', 'NV014', 'NV014', N'Thẻ ngày', N'Đã ra', 5000),
('L009', 'UID006', 'V006', '2026-05-12 08:00:00', NULL, 'NV014', NULL, N'Thẻ tháng', N'Trong bãi', 0),
('L010', 'UID007', 'V007', '2026-05-12 09:00:00', NULL, 'NV014', NULL, N'Thẻ tháng', N'Trong bãi', 0),
('L011', 'UID019', 'V016', '2026-05-12 10:00:00', '2026-05-12 12:00:00', 'NV017', 'NV017', N'Thẻ ngày', N'Đã ra', 30000),
('L012', 'UID008', 'V017', '2026-05-12 13:00:00', NULL, 'NV017', NULL, N'Thẻ tháng', N'Trong bãi', 0),
('L013', 'UID020', 'V023', '2026-05-13 07:00:00', '2026-05-13 08:00:00', 'NV020', 'NV020', N'Thẻ ngày', N'Đã ra', 5000),
('L014', 'UID009', 'V008', '2026-05-13 08:00:00', NULL, 'NV020', NULL, N'Thẻ tháng', N'Trong bãi', 0),
('L015', 'UID010', 'V018', '2026-05-13 09:00:00', NULL, 'NV020', NULL, N'Thẻ tháng', N'Trong bãi', 0),
('L016', 'UID021', 'V026', '2026-05-14 07:00:00', '2026-05-14 17:00:00', 'NV023', 'NV024', N'Thẻ tháng', N'Đã ra', 0),
('L017', 'UID022', 'V027', '2026-05-14 08:00:00', '2026-05-14 18:00:00', 'NV023', 'NV024', N'Thẻ tháng', N'Đã ra', 0),
('L018', 'UID011', 'V029', '2026-05-14 09:00:00', '2026-05-14 19:00:00', 'NV023', 'NV024', N'Thẻ tháng', N'Đã ra', 0),
('L019', 'UID012', 'V028', '2026-05-14 10:00:00', NULL, 'NV026', NULL, N'Thẻ tháng', N'Trong bãi', 0),
('L020', 'UID013', 'V030', '2026-05-14 11:00:00', NULL, 'NV026', NULL, N'Thẻ tháng', N'Trong bãi', 0),
('L021', 'UID014', 'V011', '2026-05-15 07:00:00', '2026-05-15 15:00:00', 'NV029', 'NV030', N'Thẻ tháng', N'Đã ra', 0),
('L022', 'UID015', 'V001', '2026-05-15 08:00:00', '2026-05-15 17:00:00', 'NV029', 'NV030', N'Thẻ tháng', N'Đã ra', 0),
('L023', 'UID023', 'V012', '2026-05-15 09:00:00', '2026-05-15 19:00:00', 'NV029', 'NV030', N'Thẻ tháng', N'Đã ra', 0),
('L024', 'UID024', 'V002', '2026-05-15 10:00:00', NULL, 'NV002', NULL, N'Thẻ tháng', N'Trong bãi', 0),
('L025', 'UID025', 'V003', '2026-05-15 11:00:00', NULL, 'NV002', NULL, N'Thẻ tháng', N'Trong bãi', 0),
('L026', 'UID026', 'V013', '2026-05-16 07:00:00', '2026-05-16 20:00:00', 'NV005', 'NV006', N'Thẻ tháng', N'Đã ra', 0),
('L027', 'UID027', 'V006', '2026-05-16 08:00:00', '2026-05-16 17:00:00', 'NV005', 'NV006', N'Thẻ tháng', N'Đã ra', 0),
('L028', 'UID028', 'V007', '2026-05-16 09:00:00', '2026-05-16 18:00:00', 'NV008', 'NV009', N'Thẻ tháng', N'Đã ra', 0),
('L029', 'UID029', 'V016', '2026-05-16 10:00:00', '2026-05-16 22:00:00', 'NV008', 'NV009', N'Thẻ tháng', N'Đã ra', 0),
('L030', 'UID030', 'V008', '2026-05-16 11:00:00', NULL, 'NV011', NULL, N'Thẻ tháng', N'Trong bãi', 0);

-- 6. Thanh toan
INSERT INTO ThanhToan (MaThanhToan, LoaiThanhToan, SoTien, NgayThanhToan, PhuongThuc, MaNhanVien, TrangThai) VALUES
('TT001', N'Ngày', 5000, '2026-05-11 11:30:00', N'Tiền mặt', 'NV008', N'Thành công'),
('TT002', N'Ngày', 30000, '2026-05-11 16:00:00', N'Chuyển khoản', 'NV009', N'Thành công'),
('TT003', N'Ngày', 5000, '2026-05-12 09:00:00', N'Tiền mặt', 'NV014', N'Thành công'),
('TT004', N'Ngày', 30000, '2026-05-12 12:00:00', N'Chuyển khoản', 'NV017', N'Thành công'),
('TT005', N'Ngày', 5000, '2026-05-13 08:00:00', N'Tiền mặt', 'NV020', N'Thành công'),
('TT006', N'Tháng', 100000, '2026-01-01', N'Tiền mặt', 'NV001', N'Thành công'),
('TT007', N'Tháng', 100000, '2026-01-02', N'Chuyển khoản', 'NV001', N'Thành công'),
('TT008', N'Tháng', 1000000, '2026-01-03', N'Tiền mặt', 'NV001', N'Thành công'),
('TT009', N'Tháng', 100000, '2026-01-04', N'Chuyển khoản', 'NV001', N'Thành công'),
('TT010', N'Tháng', 1000000, '2026-01-05', N'Tiền mặt', 'NV001', N'Thành công'),
('TT011', N'Tháng', 100000, '2026-01-06', N'Chuyển khoản', 'NV001', N'Thành công'),
('TT012', N'Tháng', 100000, '2026-01-07', N'Tiền mặt', 'NV001', N'Thành công'),
('TT013', N'Tháng', 1000000, '2026-01-08', N'Chuyển khoản', 'NV001', N'Thành công'),
('TT014', N'Tháng', 100000, '2026-01-09', N'Tiền mặt', 'NV001', N'Thành công'),
('TT015', N'Tháng', 100000, '2026-01-10', N'Chuyển khoản', 'NV001', N'Thành công'),
('TT016', N'Tháng', 1000000, '2026-01-11', N'Tiền mặt', 'NV001', N'Thành công'),
('TT017', N'Tháng', 100000, '2026-01-12', N'Chuyển khoản', 'NV001', N'Thành công'),
('TT018', N'Tháng', 100000, '2026-01-13', N'Tiền mặt', 'NV001', N'Thành công'),
('TT019', N'Tháng', 1000000, '2026-01-14', N'Chuyển khoản', 'NV001', N'Thành công'),
('TT020', N'Tháng', 100000, '2026-01-15', N'Tiền mặt', 'NV001', N'Thành công'),
('TT021', N'Tháng', 100000, '2026-01-16', N'Chuyển khoản', 'NV001', N'Thành công'),
('TT022', N'Tháng', 1000000, '2026-01-17', N'Tiền mặt', 'NV001', N'Thành công'),
('TT023', N'Tháng', 100000, '2026-01-18', N'Chuyển khoản', 'NV001', N'Thành công'),
('TT024', N'Tháng', 100000, '2026-01-19', N'Tiền mặt', 'NV001', N'Thành công'),
('TT025', N'Tháng', 1000000, '2026-01-20', N'Chuyển khoản', 'NV001', N'Thành công'),
('TT026', N'Tháng', 100000, '2026-01-21', N'Tiền mặt', 'NV001', N'Thành công'),
('TT027', N'Tháng', 100000, '2026-01-22', N'Chuyển khoản', 'NV001', N'Thành công'),
('TT028', N'Tháng', 100000, '2026-01-23', N'Tiền mặt', 'NV001', N'Thành công'),
('TT029', N'Tháng', 1000000, '2026-01-24', N'Chuyển khoản', 'NV001', N'Thành công'),
('TT030', N'Tháng', 100000, '2026-01-25', N'Tiền mặt', 'NV001', N'Thành công');

INSERT INTO ThanhToanNgay (MaThanhToan, MaLuotGui) VALUES
('TT001', 'L004'), ('TT002', 'L005'), ('TT003', 'L008'), ('TT004', 'L011'), ('TT005', 'L013');

INSERT INTO ThanhToanThang (MaThanhToan, MaThe, ThanhToanTuNgay, ThanhToanDenNgay) VALUES
('TT006', 'UID001', '2026-01-01', '2026-12-31'), ('TT007', 'UID002', '2026-01-01', '2026-12-31'),
('TT008', 'UID003', '2026-01-01', '2026-12-31'), ('TT009', 'UID004', '2026-01-01', '2026-12-31'),
('TT010', 'UID005', '2026-01-01', '2026-12-31'), ('TT011', 'UID006', '2026-01-01', '2026-12-31'),
('TT012', 'UID007', '2026-01-01', '2026-12-31'), ('TT013', 'UID008', '2026-01-01', '2026-12-31'),
('TT014', 'UID009', '2026-01-01', '2026-12-31'), ('TT015', 'UID010', '2026-01-01', '2026-12-31'),
('TT016', 'UID011', '2026-01-01', '2026-12-31'), ('TT017', 'UID012', '2026-01-01', '2026-12-31'),
('TT018', 'UID013', '2026-01-01', '2026-12-31'), ('TT019', 'UID014', '2026-01-01', '2026-12-31'),
('TT020', 'UID015', '2026-01-01', '2026-12-31'), ('TT021', 'UID021', '2026-01-01', '2026-12-31'),
('TT022', 'UID022', '2026-01-01', '2026-12-31'), ('TT023', 'UID023', '2026-01-01', '2026-12-31'),
('TT024', 'UID024', '2026-01-01', '2026-12-31'), ('TT025', 'UID025', '2026-01-01', '2026-12-31'),
('TT026', 'UID026', '2026-01-01', '2026-12-31'), ('TT027', 'UID027', '2026-01-01', '2026-12-31'),
('TT028', 'UID028', '2026-01-01', '2026-12-31'), ('TT029', 'UID029', '2026-01-01', '2026-12-31'),
('TT030', 'UID030', '2026-01-01', '2026-12-31');

-- 7.Lich su the xe, su co, vi tri do
INSERT INTO SuCoBaiXe (MaLuotGui, NoiDung, TrangThai, ChiPhi) VALUES
('L001', N'Trầy xước nhẹ phần nhựa sau', N'Đã xử lý', 0),
('L002', N'Xe bị xì lốp tại vị trí đỗ', N'Đang chờ', 0),
('L003', N'Va chạm nhẹ khi lùi vào ô đỗ', N'Đã xử lý', 500000),
('L004', N'Khách làm mất thẻ ngày (UID016)', N'Đã xử lý', 50000),
('L005', N'Cảm biến không nhận diện được xe', N'Đã xử lý', 0),
('L006', N'Mất mũ bảo hiểm treo trên xe', N'Đang chờ', 150000),
('L007', N'Xe rò rỉ nhớt ra sàn', N'Đã xử lý', 0),
('L008', N'Quên chìa khóa trên xe', N'Đã xử lý', 0),
('L009', N'Thẻ tháng UID006 bị cong, khó quét', N'Đã xử lý', 100000),
('L010', N'Xe để sai vị trí quy định', N'Đã xử lý', 20000),
('L011', N'Gương chiếu hậu bị lỏng', N'Đã xử lý', 0),
('L012', N'Để quên đồ trên xe (đã trả lại)', N'Đã xử lý', 0),
('L013', N'Hệ thống Camera không lưu hình ảnh', N'Đã xử lý', 0),
('L014', N'Xe bốc khói nhẹ từ động cơ', N'Đang chờ', 0),
('L015', N'Va quẹt với cột điện bãi đỗ', N'Đã xử lý', 300000),
('L016', N'Khách say rượu, nhân viên hỗ trợ dắt xe', N'Đã xử lý', 0),
('L017', N'Rò rỉ nước làm mát', N'Đã xử lý', 0),
('L018', N'Lốp xe bị đinh đâm', N'Đã xử lý', 0),
('L019', N'Hỏng thanh chắn Barrier khi xe ra', N'Đã xử lý', 1000000),
('L020', N'Quên đóng cửa kính ô tô', N'Đã xử lý', 0),
('L021', N'Tranh chấp chỗ đỗ với xe khác', N'Đã xử lý', 0),
('L022', N'Bảng điện khu vực K01 bị chập', N'Đã xử lý', 0),
('L023', N'Xe quá khổ so với ô đỗ', N'Đã xử lý', 0),
('L024', N'Thẻ UID024 hết hạn nhưng vẫn cố quẹt', N'Đã xử lý', 0),
('L025', N'Chảy dầu phanh ra sàn', N'Đang chờ', 50000);

INSERT INTO LichSuTheXe (MaThe, TrangThaiCu, TrangThaiMoi, NgayCapNhat, GhiChu) VALUES
('UID001', N'Mới', N'Đang hoạt động', '2026-01-01', N'Cấp mới thẻ tháng cho cư dân'),
('UID002', N'Mới', N'Đang hoạt động', '2026-01-02', N'Cấp mới thẻ tháng cho cư dân'),
('UID003', N'Mới', N'Đang hoạt động', '2026-01-03', N'Cấp mới thẻ tháng cho cư dân'),
('UID004', N'Mới', N'Đang hoạt động', '2026-01-04', N'Cấp mới thẻ tháng cho cư dân'),
('UID005', N'Mới', N'Đang hoạt động', '2026-01-05', N'Cấp mới thẻ tháng cho cư dân'),
('UID006', N'Đang hoạt động', N'Bị khóa', '2026-05-12', N'Khóa thẻ do nợ phí gửi xe'),
('UID016', N'Đang hoạt động', N'Bị mất', '2026-05-11', N'Khách báo mất thẻ ngày'),
('UID007', N'Mới', N'Đang hoạt động', '2026-01-07', N'Cấp mới thẻ tháng cho cư dân'),
('UID008', N'Mới', N'Đang hoạt động', '2026-01-08', N'Cấp mới thẻ tháng cho cư dân'),
('UID009', N'Mới', N'Đang hoạt động', '2026-01-09', N'Cấp mới thẻ tháng cho cư dân'),
('UID010', N'Mới', N'Đang hoạt động', '2026-01-10', N'Cấp mới thẻ tháng cho cư dân'),
('UID011', N'Mới', N'Đang hoạt động', '2026-01-11', N'Cấp mới thẻ tháng cho cư dân'),
('UID012', N'Mới', N'Đang hoạt động', '2026-01-12', N'Cấp mới thẻ tháng cho cư dân'),
('UID013', N'Mới', N'Đang hoạt động', '2026-01-13', N'Cấp mới thẻ tháng cho cư dân'),
('UID014', N'Mới', N'Đang hoạt động', '2026-01-14', N'Cấp mới thẻ tháng cho cư dân'),
('UID015', N'Mới', N'Đang hoạt động', '2026-01-15', N'Cấp mới thẻ tháng cho cư dân'),
('UID017', N'Mới', N'Đang hoạt động', '2026-05-11', N'Cấp thẻ ngày cho khách'),
('UID018', N'Mới', N'Đang hoạt động', '2026-05-12', N'Cấp thẻ ngày cho khách'),
('UID019', N'Mới', N'Đang hoạt động', '2026-05-12', N'Cấp thẻ ngày cho khách'),
('UID020', N'Mới', N'Đang hoạt động', '2026-05-13', N'Cấp thẻ ngày cho khách'),
('UID021', N'Mới', N'Đang hoạt động', '2026-01-21', N'Cấp mới thẻ tháng cho cư dân'),
('UID022', N'Mới', N'Đang hoạt động', '2026-01-22', N'Cấp mới thẻ tháng cho cư dân'),
('UID023', N'Mới', N'Đang hoạt động', '2026-01-23', N'Cấp mới thẻ tháng cho cư dân'),
('UID024', N'Đang hoạt động', N'Hết hạn', '2026-04-30', N'Thẻ tháng hết hạn chưa gia hạn'),
('UID025', N'Mới', N'Đang hoạt động', '2026-01-25', N'Cấp mới thẻ tháng cho cư dân');

INSERT INTO LichSuViTriDo (MaViTri, MaThe, ThoiGianBatDau, ThoiGianKetThuc) VALUES
('V001', 'UID001', '2026-05-10 07:00:00', '2026-05-10 17:00:00'),
('V002', 'UID002', '2026-05-10 08:00:00', '2026-05-10 18:00:00'),
('V011', 'UID003', '2026-05-10 09:00:00', '2026-05-10 19:00:00'),
('V021', 'UID016', '2026-05-11 07:30:00', '2026-05-11 11:30:00'),
('V012', 'UID017', '2026-05-11 08:00:00', '2026-05-11 16:00:00'),
('V003', 'UID004', '2026-05-11 09:00:00', '2026-05-11 17:00:00'),
('V013', 'UID005', '2026-05-11 10:00:00', '2026-05-11 20:00:00'),
('V022', 'UID018', '2026-05-12 07:00:00', '2026-05-12 09:00:00'),
('V006', 'UID006', '2026-05-12 08:00:00', NULL),
('V007', 'UID007', '2026-05-12 09:00:00', NULL),
('V016', 'UID019', '2026-05-12 10:00:00', '2026-05-12 12:00:00'),
('V017', 'UID008', '2026-05-12 13:00:00', NULL),
('V023', 'UID020', '2026-05-13 07:00:00', '2026-05-13 08:00:00'),
('V008', 'UID009', '2026-05-13 08:00:00', NULL),
('V018', 'UID010', '2026-05-13 09:00:00', NULL),
('V026', 'UID021', '2026-05-14 07:00:00', '2026-05-14 17:00:00'),
('V027', 'UID022', '2026-05-14 08:00:00', '2026-05-14 18:00:00'),
('V029', 'UID011', '2026-05-14 09:00:00', '2026-05-14 19:00:00'),
('V028', 'UID012', '2026-05-14 10:00:00', NULL),
('V030', 'UID013', '2026-05-14 11:00:00', NULL),
('V011', 'UID014', '2026-05-15 07:00:00', '2026-05-15 15:00:00'),
('V001', 'UID015', '2026-05-15 08:00:00', '2026-05-15 17:00:00'),
('V012', 'UID023', '2026-05-15 09:00:00', '2026-05-15 19:00:00'),
('V002', 'UID024', '2026-05-15 10:00:00', NULL),
('V003', 'UID025', '2026-05-15 11:00:00', NULL);

--3. Cau hoi truy van

-- a. Truy van don gian (5 cau)
    -- a.1. Hien thi bien so xe, so the cac xe da lam the thang
    select BienSo, SoThe from TheXe join Xe on TheXe.MaXe = Xe.MaXe where LoaiThe = N'Thẻ tháng'

    -- a.2. Liet ke nhung khu vuc co suc chua <100
    select * from KhuVuc where SucChuaToiDa <100

    -- a.3. Cho biet ma nhan vien, ho ten, ngay sinh, dia chi nhung nhan vien o bai do xe
    select MaNhanVien, HoTen, NgaySinh, DiaChi from NhanVien where MaVaiTro = 'NV'

    -- a.4. Cho biet nhan vien lam ca sang
    select MaNhanVien, HoTen, SoDienThoai from NhanVien where CaLamViec = N'Sáng'

    -- a.5. Liet ke cac luot gui xe chua ra (con trong bai) trong thang 5/2026
    select * from LuotGuiXe
    where TrangThaiLuotGui = N'Trong bãi'
    and ThoiGianVao >= '2026-05-01' and ThoiGianVao < '2026-06-01'

--b.Truy vấn với Aggregate Functions: 7 câu  (7đ)

	--b.1 COUNT(*) : Đếm tổng số lượt gửi xe
	SELECT COUNT(*) AS TongLuotGuiXe
	FROM LuotGuiXe;

	--b1.2 COUNT(): Đếm số xe có biển số
	SELECT COUNT(BienSo) AS SoXeCoBienSo
	FROM Xe;

	--b.3. SUM(): Tính tổng doanh thu từ lượt gửi xe
	SELECT SUM(TongTien) AS TongDoanhThu
	FROM LuotGuiXe;

	--b.4. AVG() : Tính phí gửi xe trung bình
	SELECT AVG(TongTien) AS PhiGuiTrungBinh
	FROM LuotGuiXe;

	-- b.5. MAX() : Tìm phí gửi xe cao nhất
	SELECT MAX(TongTien) AS PhiGuiCaoNhat
	FROM LuotGuiXe;

	-- b.6. MIN() : Tìm phí gửi xe thấp nhất
	SELECT MIN(TongTien) AS PhiGuiThapNhat
	FROM LuotGuiXe;

	-- b.7. STRING_AGG() : Liệt kê các biển số xe trên 1 dòng
	SELECT STRING_AGG(BienSo, ', ') AS DanhSachBienSo
	FROM Xe;

-- c. Truy van voi menh de having (5 cau)
    -- c.1. Liet ke nhung nhan vien da thu tong so tien thanh toan lon hon 500000 VND
    SELECT NV.MaNhanVien, NV.HoTen, SUM(TT.SoTien) AS TongSoTienThanhToan
    FROM NhanVien NV JOIN ThanhToan TT ON NV.MaNhanVien = TT.MaNhanVien
    GROUP BY NV.MaNhanVien, NV.HoTen
    HAVING SUM(TT.SoTien) > 500000;

    -- c.2. Tim cac khu vuc dang co so luong vi tri do bi chiem dung tu 2 cho tro len
    SELECT KV.MaKhu, KV.TenKhu, COUNT(V.MaViTri) AS SoChoDaDung
    FROM KhuVuc KV JOIN ViTriDo V ON KV.MaKhu = V.MaKhu
    WHERE (V.TrangThai) = 1
    GROUP BY KV.MaKhu, KV.TenKhu
    HAVING COUNT(V.MaViTri) >= 2;

    -- c.3. Liet ke cac khu vuc co tu 2 su co tro len ma tong chi phi xu ly su co tai khu vuc do
    -- vuot qua 100000 VND
    SELECT KV.MaKhu, KV.TenKhu, COUNT(S.MaSuCo) AS SoVuSuCo, SUM(S.ChiPhi) AS TongChiPhiSuCo
    FROM KhuVuc KV 
        JOIN ViTriDo V ON KV.MaKhu = V.MaKhu
        JOIN LuotGuiXe L ON L.MaViTri = V.MaViTri
        JOIN SuCoBaiXe S ON S.MaLuotGui = L.MaLuotGui
    GROUP BY KV.MaKhu, KV.TenKhu
    HAVING COUNT(S.MaSuCo) >= 2 AND SUM(S.ChiPhi) > 100000;

    -- c.4. Tim cac toa nha co tong tien thanh toan the thang vuot qua 1500000 VND
    SELECT CH.ToaNha, COUNT(DISTINCT CD.MaCuDan) AS SoLuongCuDan,SUM(TT.SoTien) AS TongDoanhThuThang
    FROM CanHo CH
        JOIN CuDan_CanHo CDCH ON CH.MaCanHo = CDCH.MaCanHo
        JOIN CuDan CD ON CDCH.MaCuDan = CD.MaCuDan
        JOIN Xe X ON CD.MaCuDan = X.MaCuDan
        JOIN TheXe TX ON X.MaXe = TX.MaXe
        JOIN ThanhToanThang TTT ON TX.MaThe = TTT.MaThe
        JOIN ThanhToan TT ON TTT.MaThanhToan = TT.MaThanhToan
    GROUP BY CH.ToaNha
    HAVING SUM(TT.SoTien) > 1500000;

    -- c.5. Tim nhung The ngay (khach vang lai) da vao bai it nhat 1 lan va co tong so tien thanh toan
    -- ngay cao hon 10000 VND
    SELECT TX.MaThe, TX.SoThe, COUNT(LG.MaLuotGui) AS SoLuotVao, SUM(TT.SoTien) AS TongTienVangLai
    FROM TheXe TX
        JOIN LuotGuiXe LG ON TX.MaThe = LG.MaThe
        JOIN ThanhToanNgay TTN ON LG.MaLuotGui = TTN.MaLuotGui
        JOIN ThanhToan TT ON TTN.MaThanhToan = TT.MaThanhToan
    WHERE TX.LoaiThe = N'Thẻ ngày'
    GROUP BY TX.MaThe, TX.SoThe
    HAVING COUNT(LG.MaLuotGui) >= 1 AND SUM(TT.SoTien) > 10000;
    
-- d. Truy van lon nhat, nho nhat (4 cau)

    -- d.1. Nhan vien co luong thap nhat
    select top 1 MaNhanVien, HoTen, Luong from NhanVien order by Luong asc

    -- d.2. Nhan vien co luot xu li xe di vao it nhat
    select top 1 MaNVVao, count(MaNVVao) as LuotXuLy from LuotGuiXe group by MaNVVao order by count(MaNVVao) asc

    -- d.3. Su co co chi phi xu li cao nhat
    select sc.MaSuCo, sc.NoiDung, sc.ChiPhi, sc.TrangThai
    from SuCoBaiXe sc
    where sc.ChiPhi = (select max(ChiPhi) from SuCoBaiXe)

    -- d.4. Khu vuc co nhieu vi tri dang su dung nhat
    select top 1 kv.MaKhu, count(kv.MaKhu) as LuotSuDung
    from LuotGuiXe lgx
    join ViTriDo vtd on lgx.MaViTri = vtd.MaViTri
    join KhuVuc kv on vtd.MaKhu = kv.MaKhu
    where TrangThaiLuotGui = N'Trong bãi'
    group by kv.MaKhu
    order by count(kv.MaKhu) desc

-- e. Truy van Khong/chua co (Not In va Left/Right Join) (5 cau)
    -- e.1. Tim nhung can ho dang trong
    SELECT CH.MaCanHo, CH.SoCanHo, CH.ToaNha, CH.Tang
    FROM CanHo CH
    LEFT JOIN CuDan_CanHo CDCH ON CH.MaCanHo = CDCH.MaCanHo
    WHERE CDCH.MaCuDan IS NULL;

    -- e.2. Tim nhung nhan vien chua tung thuc hien quet the cho xe ra
    SELECT NV.MaNhanVien, NV.HoTen
    FROM NhanVien NV LEFT JOIN LuotGuiXe L ON NV.MaNhanVien = L.MaNVRa
    WHERE L.MaLuotGui IS NULL AND NV.MaVaiTro = 'NV';

    -- e.3. Tim nhung khu vuc hien khong co bat ky xe nao dang do
    SELECT K.MaKhu, K.TenKhu
    FROM KhuVuc K LEFT JOIN ViTriDo V ON K.MaKhu = V.MaKhu AND V.TrangThai = 1
    WHERE V.MaViTri IS NULL;

    -- e.4. Tim nhung cu dan chua tung gap su co mat mu bao hiem
    SELECT MaCuDan, HoTen
    FROM CuDan
    WHERE MaCuDan NOT IN (
        SELECT DISTINCT X.MaCuDan
        FROM Xe X
            JOIN TheXe TX ON X.MaXe = TX.MaXe
            JOIN LuotGuiXe LG ON TX.MaThe = LG.MaThe
            JOIN SuCoBaiXe SC ON LG.MaLuotGui = SC.MaLuotGui
        WHERE SC.NoiDung LIKE N'%Mất mũ bảo hiểm%'
    );
    
    -- e.5. Tim nhan vien chua tung thuc hien thu tien
    SELECT MaNhanVien, HoTen, CaLamViec
    FROM NhanVien
    WHERE MaNhanVien NOT IN (
        SELECT DISTINCT MaNhanVien FROM ThanhToan
    );

-- f. Truy van hop/giao/tru (3 cau)

-- Truy van hop

    -- Danh sach tat ca moi nguoi co trong he thong
    select HoTen, SoDienThoai, N'Nhân viên' as PhanLoai from NhanVien
    union
    select HoTen, SoDienThoai, N'Cư dân' as PhanLoai from CuDan

-- Truy vấn giao
    -- Tìm cư dân trong tháng 5 vừa gửi ô tô vừa gửi xe máy
	SELECT MaCuDan
	FROM Xe X
	JOIN LoaiXe LX ON X.MaLoaiXe = LX.MaLoaiXe
	WHERE LX.TenLoaiXe = N'Ô tô'
	AND MONTH(X.NgayDangKyXe) = 5

	INTERSECT

	SELECT MaCuDan
	FROM Xe X
	JOIN LoaiXe LX ON X.MaLoaiXe = LX.MaLoaiXe
	WHERE LX.TenLoaiXe = N'Xe máy'
	AND MONTH(X.NgayDangKyXe) = 5;

-- Truy van tru
    
    -- Tim nhung cu dan da dang ky the thang xe may nhung chua tung thuc hien thanh toan
    SELECT CD.MaCuDan, CD.HoTen, X.BienSo
    FROM CuDan CD
        JOIN Xe X ON CD.MaCuDan = X.MaCuDan
        JOIN LoaiXe LX ON X.MaLoaiXe = LX.MaLoaiXe
    WHERE LX.TenLoaiXe = N'Xe máy'
    EXCEPT
    SELECT CD.MaCuDan, CD.HoTen, X.BienSo
    FROM CuDan CD
        JOIN Xe X ON CD.MaCuDan = X.MaCuDan
        JOIN TheXe TX ON X.MaXe = TX.MaXe
        JOIN ThanhToanThang TTT ON TX.MaThe = TTT.MaThe

-- g. Truy van update/delete

-- Truy van update

    -- Tang luong them 10% cho nhan vien lam ca toi (note: da thuc hien lenh update)

        -- Xem ket qua truoc khi thuc hien (so sanh luong cu va luong moi)
        select MaNhanVien, HoTen, CaLamViec, Luong, Luong * 1.1 as LuongMoi
        from NhanVien
        where CaLamViec = N'Tối'

        -- Thuc hien cap nhat
        update NhanVien
        set Luong = Luong * 1.1
        where CaLamViec = N'Tối'

        -- Xem ket qua sau khi thuc hien
        select MaNhanVien, HoTen, CaLamViec, Luong
        from NhanVien
        where CaLamViec = N'Tối'

    -- Cap nhat lai chi phi xu ly co dinh la 50000 VND cho cac su co lien quan den 'The' hoac 'Chia khoa
        -- Thuc hien cap nhat
        UPDATE SuCoBaiXe
        SET ChiPhi = CASE WHEN NoiDung LIKE N'%Thẻ%' OR NoiDung LIKE N'%chìa khóa%' THEN 50000 
                ELSE ChiPhi 
            END;

        -- Xem du lieu
        SELECT MaSuCo, NoiDung, ChiPhi
        FROM SuCoBaiXe
        WHERE NoiDung LIKE N'%Thẻ%' OR NoiDung LIKE N'%Chìa khóa%';

    --Cập nhật phí gửi xe máy theo ngày tăng thêm 2000
        -- Thuc hien cap nhat
		UPDATE LoaiXe
		SET GiaTienNgay = GiaTienNgay + 2000
		WHERE TenLoaiXe = N'Xe máy';

        -- Xem du lieu
        SELECT GiaTienNgay
        FROM LoaiXe
        WHERE TenLoaiXe = N'Xe máy';

    -- Cập nhật trạng thái vị trí đỗ xe
        -- Thuc hien cap nhat
		BEGIN TRANSACTION;
		UPDATE ViTriDo
		SET TrangThai = 0
		WHERE MaViTri = 'V001';
		ROLLBACK;

        -- Xem du lieu
        SELECT MaViTri, TrangThai
        FROM ViTriDo
        WHERE MaViTri = 'V001';

-- Truy van delete

    -- Xoa lich su do cac xe truoc ngay 4/5/2026 

        -- Xem truoc cac dong se bi xoa
        select *
        from LichSuViTriDo
        where ThoiGianKetThuc < '2026-05-04'

        -- Thuc hien xoa 
        begin transaction
            delete from LichSuViTriDo
            where ThoiGianKetThuc IS NOT NULL
            and ThoiGianKetThuc < '2026-05-04'

            -- Kiem tra sau khi xoa
            select * from LichSuViTriDo
        rollback -- Neu chua dung co the quay lai
        
    -- Xoa du lieu trong bang lich su the xe da qua 6 thang cua nhung the van dang hoat dong
        -- Xem truoc cac dong se bi xoa
        SELECT * FROM LichSuTheXe
        WHERE NgayCapNhat < DATEADD(MONTH, -6, GETDATE());

        -- Thuc hien xoa
        DELETE FROM LichSuTheXe
        WHERE NgayCapNhat < DATEADD(month, -6, GETDATE()) AND MaThe IN 
            (SELECT MaThe FROM TheXe WHERE TrangThai = N'Đang hoạt động');

	-- Xóa thanh toán theo mã
        -- Xem truoc cac dong se bi xoa
        SELECT * FROM ThanhToan WHERE MaThanhToan = 'TT005';

        -- Thuc hien xoa
	    BEGIN TRANSACTION;
	    DELETE FROM ThanhToan
	    WHERE MaThanhToan = 'TT005';
	    ROLLBACK;
-- h. Truy van su dung phep chia

    -- h.1. Nhung nhan vien da xu li luot vao cua tat ca loai the
    select nv.MaNhanVien, nv.HoTen
    from NhanVien nv
    where not exists(
        -- Lay tat ca the ton tai trong he thong
        select distinct tx1.LoaiThe from TheXe tx1
        where not exists (
            -- Kiem tra xem nhan vien da xu li loai the nay chua
            select *
            from LuotGuiXe lg
            join TheXe tx2 on lg.MaThe = tx2.MaThe
            where lg.MaNVVao = nv.MaNhanVien
              and tx2.LoaiThe = tx1.LoaiThe
        )
    )

    -- h.2. Tim hang xe co tat ca loai xe (o to va xe may) trong bai gui xe
    SELECT DISTINCT X1.HangXe
    FROM Xe X1
    WHERE NOT EXISTS (
        -- Lấy tất cả các loại xe định nghĩa trong hệ thống (XM, OT)
        SELECT LX.MaLoaiXe 
        FROM LoaiXe LX
        WHERE NOT EXISTS (
            -- Kiểm tra xem hãng xe này có sản xuất loại xe đó không?
            SELECT * 
            FROM Xe X2 
            WHERE X2.HangXe = X1.HangXe 
              AND X2.MaLoaiXe = LX.MaLoaiXe
        )
    );

    -- h.3. Tim cu dan thue tat ca can ho o tang 9
    SELECT CD.MaCuDan, CD.HoTen, CD.SoDienThoai
    FROM CuDan CD
    WHERE EXISTS (SELECT 1 FROM CuDan_CanHo WHERE MaCuDan = CD.MaCuDan)
    AND NOT EXISTS (
        SELECT CH.MaCanHo 
        FROM CanHo CH 
        WHERE CH.Tang = 9 AND NOT EXISTS (
            SELECT * 
            FROM CuDan_CanHo CDCH
            WHERE CDCH.MaCuDan = CD.MaCuDan AND CDCH.MaCanHo = CH.MaCanHo
        )
    );

    --h.4. Tìm nhân viên đã làm việc ở tất cả ca làm việc
		SELECT nv.MaNhanVien, nv.HoTen
		FROM NhanVien nv
		WHERE NOT EXISTS (
			SELECT DISTINCT CaLamViec
			FROM NhanVien
			EXCEPT
			SELECT DISTINCT nv2.CaLamViec
			FROM NhanVien nv2
			WHERE nv2.MaNhanVien = nv.MaNhanVien
		);


--4. Thu tuc, ham, trigger

--Thu tuc

    --Xem lich su gui xe theo bien so
    if object_id('sp_LichSuGui', 'p') is not null 
        drop procedure sp_LichSuGui
    go
    create procedure sp_LichSuGui
        @BienSo varchar(20)
    as begin
        select
            lgx.MaLuotGui,
            x.BienSo,
            x.HangXe,
            x.TenDongXe,
            tx.LoaiThe,
            lgx.ThoiGianVao,
            lgx.ThoiGianRa,
            lgx.TrangThaiLuotGui,
            lgx.TongTien
        from LuotGuiXe lgx
        join TheXe tx on lgx.MaThe = tx.MaThe
        join Xe x on tx.MaXe = x.MaXe
        where x.BienSo = @BienSo
        order by lgx.ThoiGianVao desc
    end
    go

    --Vi du
    exec sp_LichSuGui '79-H1 111.11'

    --Thong ke doanh thu theo khoang thoi gian
    if object_id('sp_DoanhThu', 'p') is not null
        drop procedure sp_DoanhThu
    go
    create procedure sp_DoanhThu 
        @TuNgay date,
        @DenNgay date
    as begin
        select 
            count(*) as TongLuotGui,
            sum(TongTien) as TongDoanhThu,
            count(case when PhuongThucTinhPhi = N'Thẻ ngày' then 1 end) as LuotTheNgay,
            count(case when PhuongThucTinhPhi = N'Thẻ tháng' then 1 end) as LuotTheThang,
            sum(case when PhuongThucTinhPhi = N'Thẻ ngày' then TongTien else 0 end) as DoanhThuTheNgay
        from LuotGuiXe 
        where cast(ThoiGianVao as date) between @TuNgay and @DenNgay
            and TrangThaiLuotGui = N'Đã ra'
       end
    go
    
    --Vi du
    exec sp_DoanhThu '2026-05-10', '2026-05-12'

    --Bao cao su co theo khu vuc
    if object_id('sp_BaoCaoSuCo', 'p') is not null
        drop procedure sp_BaoCaoSuCo 
    go
    create procedure sp_BaoCaoSuCo
        @TrangThai nvarchar(50) = null
    as begin
        select 
            kv.TenKhu,
            count(sc.MaSuCo) as TongSuCo,
            sum(sc.ChiPhi) as TongChiPhi,
            count(case when sc.TrangThai LIKE N'%Đang chờ%' then 1 end) as ChuaXuLy,
            count(case when sc.TrangThai LIKE N'%Đã xử lý%' then 1 end) as DaXuLy
        from SuCoBaiXe sc
        join LuotGuiXe lgx on sc.MaLuotGui = lgx.MaLuotGui
        join ViTriDo vtd on lgx.MaViTri = vtd.MaViTri
        join KhuVuc kv on vtd.MaKhu = kv.MaKhu
        where @TrangThai is null or @TrangThai like N'%' + sc.TrangThai + N'%' 
        group by kv.MaKhu, kv.TenKhu
        order by TongChiPhi desc
    end
    go

    --Vi du
    exec sp_BaoCaoSuCo N'Đang chờ'

    -- Tu dong gia han the thang
    IF OBJECT_ID('sp_GiaHanTheThang', 'P') IS NOT NULL
        DROP PROCEDURE sp_GiaHanTheThang
    GO

    CREATE PROCEDURE sp_GiaHanTheThang
    @MaThe VARCHAR(50),
    @MaNhanVien VARCHAR(10),
    @PhuongThuc NVARCHAR(50) = N'Tiền mặt'
    AS
    BEGIN
        DECLARE @NgayHetHanCu DATE;
        DECLARE @NgayBatDau DATE;
        DECLARE @NgayKetThuc DATE;
        DECLARE @SoTien DECIMAL(18,2);
        DECLARE @MaTTMoi VARCHAR(20);
        DECLARE @MaxID INT;

        -- Tu dong tao ma thanh toan moi
        SELECT @MaxID = MAX(CAST(SUBSTRING(MaThanhToan, 3, LEN(MaThanhToan)) AS INT)) 
        FROM ThanhToan;
    
        SET @MaxID = ISNULL(@MaxID, 0) + 1;
        SET @MaTTMoi = 'TT' + RIGHT('000' + CAST(@MaxID AS VARCHAR(10)), 3);

        SELECT @SoTien = LX.GiaTienThang
        FROM TheXe TX
            JOIN Xe X ON TX.MaXe = X.MaXe
            JOIN LoaiXe LX ON X.MaLoaiXe = LX.MaLoaiXe
        WHERE TX.MaThe = @MaThe;

        -- Tinh ngay bat dau va het han moi
        SELECT @NgayHetHanCu = NgayHetHan FROM TheXe WHERE MaThe = @MaThe;
        SET @NgayBatDau = CASE WHEN @NgayHetHanCu > GETDATE() THEN @NgayHetHanCu ELSE GETDATE() END;
        SET @NgayKetThuc = DATEADD(DAY, 30, @NgayBatDau);

        -- Them du lieu vao cac bang lien quan
        INSERT INTO ThanhToan(MaThanhToan, LoaiThanhToan, SoTien, NgayThanhToan, PhuongThuc, MaNhanVien, TrangThai)
        VALUES (@MaTTMoi, N'Tháng', @SoTien, GETDATE(), @PhuongThuc, @MaNhanVien, N'Thành công');

        INSERT INTO ThanhToanThang (MaThanhToan, MaThe, ThanhToanTuNgay, ThanhToanDenNgay)
        VALUES (@MaTTMoi, @MaThe, @NgayBatDau, @NgayKetThuc);

        UPDATE TheXe 
        SET NgayHetHan = @NgayKetThuc, TrangThai = N'Đang hoạt động'
        WHERE MaThe = @MaThe
    END
    GO

    -- Vi du
    EXEC sp_GiaHanTheThang 'UID001', 'NV001', N'Chuyển khoản';
    
    -- Kiem tra co vi tri con trong theo khu vuc
    IF OBJECT_ID('sp_TraCuuViTriTrong', 'P') IS NOT NULL 
    DROP PROCEDURE sp_TraCuuViTriTrong
    GO

    CREATE PROCEDURE sp_TraCuuViTriTrong
        @TenKhu NVARCHAR(50),
        @LoaiViTri NVARCHAR(50)
    AS 
    BEGIN
        SELECT 
            KV.TenKhu,
            KV.Tang,
            VT.MaViTri,
            VT.TenViTri,
            VT.GhiChu
        FROM ViTriDo VT
        JOIN KhuVuc KV ON VT.MaKhu = KV.MaKhu
        WHERE VT.TrangThai = 0 AND KV.TenKhu LIKE '%' + @TenKhu + '%' AND VT.LoaiViTri = @LoaiViTri
        ORDER BY KV.Tang, VT.TenViTri;
    END
    GO

    -- Vi du
    EXEC sp_TraCuuViTriTrong N'A-H1', N'Xe máy'

    -- Thủ tục thêm lượt gửi xe		
	IF OBJECT_ID('sp_ThemLuotGuiXe', 'p') IS NOT NULL
    DROP PROCEDURE sp_ThemLuotGuiXe
	GO
	CREATE PROCEDURE sp_ThemLuotGuiXe
		@MaThe VARCHAR(10),
		@MaViTri INT,
		@MaNVVao INT
	AS
	BEGIN
		INSERT INTO LuotGuiXe(MaThe, MaViTri, MaNVVao, ThoiGianVao)
		VALUES (@MaThe, @MaViTri, @MaNVVao, GETDATE());
	END;

    EXEC sp_ThemLuotGuiXe 'UID002', 'V009', 'NV002';

	--Thủ tục xem danh sách xe
	IF OBJECT_ID('sp_XemDanhSachXe', 'p') IS NOT NULL
	DROP PROCEDURE sp_XemDanhSachXe;
	GO

	CREATE PROCEDURE sp_XemDanhSachXe
	AS
	BEGIN
		SELECT *
		FROM Xe;
	END;
	GO
		
    EXEC sp_XemDanhSachXe;

--Ham

    --Dem so xe dang trong bai theo khu vuc
    if object_id('fn_DemXe', 'fn') is not null
        drop function fn_DemXe
    go
    create function fn_DemXe(@Makhu varchar(5))
    returns int
    as begin
        declare @SoXe int
        select @SoXe = count(*)
        from LuotGuiXe lgx
        join ViTriDo vtd on lgx.MaViTri = vtd.MaViTri
        where vtd.MaKhu = @Makhu and lgx.TrangThaiLuotGui = N'Trong bãi'
        return @SoXe
    end
    go

    --Vi du
    select dbo.fn_DemXe('K01') as SoXeHienTai
    
    --Tinh tien gui xe theo gio va loai xe
    if object_id('fn_TinhTien', 'fn') is not null
        drop function fn_TinhTien
    go
    create function fn_TinhTien(@ThoiGianVao datetime, @ThoiGianRa datetime, @MaLoaiXe varchar(10))
    returns decimal(18,2)
    as begin
        declare @GiaNgay decimal(18,2)
        declare @SoGio int
        select @GiaNgay = GiaTienNgay
        from LoaiXe 
        where MaLoaiXe = @MaLoaiXe
        set @SoGio = datediff(hour, @ThoiGianVao, @ThoiGianRa)
        if @SoGio < 1 set @SoGio = 1
        return @SoGio * (@GiaNgay / 8.0)
    end
    go

    --Vi du
    select dbo.fn_TinhTien('2026-05-01 07:00', '2026-05-01 11:00', 'XM') as TienGui

    --Thoi gian do trung binh tai mot khu vuc(thoi gian: phut)
    if object_id('fn_ThoiGianDoTrungBinh', 'fn') is not null
        drop function fn_ThoiGianDoTrungBinh 
    go
    create function fn_ThoiGianDoTrungBinh(@MaKhu varchar(5))
    returns int
    as
    begin
        declare @TrungBinh int
        select @TrungBinh = avg(datediff(minute, lgx.ThoiGianVao, lgx.ThoiGianRa))
        from LuotGuiXe lgx
        join ViTriDo vtd on lgx.MaViTri = vtd.MaViTri
        where vtd.MaKhu = @MaKhu
          and lgx.ThoiGianRa is not null

        return isnull(@TrungBinh, 0)
    end
    go

    --Vi du
    select dbo.fn_ThoiGianDoTrungBinh('K01') as ThoiGianTrungBinh

    -- Ham tinh so ngay con lai cua the thang
    IF OBJECT_ID('fn_SoNgayConHan', 'FN') IS NOT NULL
        DROP FUNCTION fn_SoNgayConHan
    GO

    CREATE FUNCTION fn_SoNgayConHan(@MaThe varchar(50))
    RETURNS int
    AS
    BEGIN
        DECLARE @NgayHetHan date;
        DECLARE @SoNgay int;

        SELECT @NgayHetHan = NgayHetHan FROM TheXe WHERE MaThe = @MaThe;
        IF @NgayHetHan IS NULL RETURN -1;
        SET @SoNgay = DATEDIFF(DAY, GETDATE(), @NgayHetHan);

        RETURN CASE WHEN @SoNgay < 0 THEN 0 ELSE @SoNgay END;
    END
    GO

    -- Vi du
    SELECT MaThe, MaXe, dbo.fn_SoNgayConHan(MaThe) AS NgayConLai
    FROM TheXe
    WHERE LoaiThe = N'Thẻ tháng';

    -- Ham tinh ty le lap day xe cua mot khu vuc (don vi: %)
    IF OBJECT_ID('fn_TiLeLapDay', 'FN') IS NOT NULL
    DROP FUNCTION fn_TiLeLapDay
    GO

    CREATE FUNCTION fn_TiLeLapDay(@MaKhu VARCHAR(5))
    RETURNS FLOAT
    AS
    BEGIN
        DECLARE @SucChua INT;
        DECLARE @DangDo INT;

        SELECT @SucChua = SucChuaToiDa FROM KhuVuc WHERE MaKhu = @MaKhu;
        SELECT @DangDo = COUNT(*) FROM ViTriDo WHERE MaKhu = @MaKhu AND TrangThai = 1;

        IF @SucChua = 0 OR @SucChua IS NULL RETURN 0;
        RETURN ROUND((CAST(@DangDo AS FLOAT) / @SucChua) * 100, 2);
    END
    GO

    -- Vi du
    SELECT MaKhu, TenKhu, dbo.fn_TiLeLapDay(MaKhu) AS PhanTramLapDay
    FROM KhuVuc;

    -- Tim chi phi su co lon nhat cua mot toa nha
    IF OBJECT_ID('fn_ThoiGianDo', 'IF') IS NOT NULL
    DROP FUNCTION fn_ThoiGianDo
    GO

    CREATE FUNCTION fn_ThoiGianDo(@BienSo VARCHAR(20))
    RETURNS TABLE
    AS
    RETURN (
        SELECT 
            X.BienSo,
            MIN(DATEDIFF(MINUTE, LG.ThoiGianVao, LG.ThoiGianRa)) AS PhutItNhat,
            MAX(DATEDIFF(MINUTE, LG.ThoiGianVao, LG.ThoiGianRa)) AS PhutNhieuNhat
        FROM Xe X
            JOIN TheXe TX ON X.MaXe = TX.MaXe
            JOIN LuotGuiXe LG ON TX.MaThe = LG.MaThe
        WHERE X.BienSo = @BienSo AND LG.ThoiGianRa IS NOT NULL
        GROUP BY X.BienSo
    )
    GO

    -- Vi du
    SELECT * FROM dbo.fn_ThoiGianDo('79-H1 111.11');

    --Hàm đếm số lượng xe để ở bãi đỗ chung cư của một cư dân
	IF OBJECT_ID('fn_DemSoXe', 'fn') IS NOT NULL
	DROP FUNCTION fn_DemSoXe;
	GO

	CREATE FUNCTION fn_DemSoXe
	(
		@MaCuDan varchar(5)
	)
	RETURNS INT
	AS
	BEGIN
		DECLARE @SoLuongXe INT;
		SELECT @SoLuongXe = COUNT(*)
		FROM Xe
		WHERE MaCuDan = @MaCuDan;
		RETURN @SoLuongXe;
	END;
	GO

    SELECT dbo.fn_DemSoXe('CD002');

	--Hàm tính doanh thu theo tháng
	IF OBJECT_ID('fn_TinhDoanhThuTheoThang', 'FN') IS NOT NULL
	DROP FUNCTION fn_TinhDoanhThuTheoThang;
	GO

	CREATE FUNCTION fn_TinhDoanhThuTheoThang
	(
		@Thang INT,
		@Nam INT
	)
	RETURNS INT
	AS
	BEGIN
		DECLARE @TongTien INT;
		SELECT @TongTien = SUM(SoTien)
		FROM ThanhToan
		WHERE MONTH(NgayThanhToan) = @Thang
			AND YEAR(NgayThanhToan) = @Nam;
		RETURN ISNULL(@TongTien, 0);
	END;
	GO

    SELECT dbo.fn_TinhDoanhThuTheoThang(5, 2026);

--Trigger

    --Khi xe vao: tu dong danh dau vi tri ban cua vi tri do xe va ghi lich su
    if exists (select name from sysobjects
    where name='trg_GhiNhanXeVao' and type='TR')
    drop trigger trg_GhiNhanXeVao
    go

    create trigger trg_GhiNhanXeVao
    on LuotGuiXe
    after insert
    as begin
        --Danh dau vi tri do da co xe (TrangThai = 1)
        update vtd
        set vtd.TrangThai = 1
        from ViTriDo vtd
        join inserted i on vtd.MaViTri = i.MaViTri

        --Tu dong ghi vao lich su vi tri do
        insert into LichSuViTriDo(MaViTri, MaThe, ThoiGianBatDau)
        select i.MaViTri, i.MaThe, i.ThoiGianVao
        from inserted i
    end
    go

    --Vi du
    insert into LuotGuiXe(MaLuotGui, MaThe, MaViTri, ThoiGianVao, MaNVVao, PhuongThucTinhPhi, TrangThaiLuotGui, TongTien)
    values ('L026', 'UID016', 'V003', GETDATE(), 'NV002', N'Thẻ ngày', N'Trong bãi', 0)

    select MaViTri, TrangThai
    from ViTriDo 
    where MaViTri = 'V003'

    select * 
    from LichSuViTriDo 
    where MaViTri = 'V003'

    -- Kiem tra the thang co hop le truoc khi cho xe vao bai
    if exists (select name from sysobjects
    where name='trg_KiemTraTheVao' and type='TR')
    drop trigger trg_KiemTraTheVao
    GO

    CREATE TRIGGER trg_KiemTraTheVao
    ON LuotGuiXe
    FOR INSERT
    AS
    BEGIN
        IF EXISTS (
            SELECT *
            FROM inserted i JOIN TheXe TX ON i.MaThe = TX.MaThe
            WHERE TX.TrangThai <> N'Đang hoạt động' OR (TX.LoaiThe = N'Thẻ tháng' AND TX.NgayHetHan < GETDATE())
        )
        BEGIN
            RAISERROR (N'Lỗi: Thẻ không hợp lệ hoặc hết hạn. Không cho xe vào', 16, 1);
            ROLLBACK TRANSACTION;
        END
    END
    GO

    -- Vi du
    UPDATE TheXe SET NgayHetHan = '2026-04-30', TrangThai = N'Đang hoạt động' WHERE MaThe = 'UID007';

    INSERT INTO LuotGuiXe (MaLuotGui, MaThe, MaViTri, ThoiGianVao, MaNVVao, TrangThaiLuotGui)
    VALUES ('LG026', 'UID007', 'V009', GETDATE(), 'NV002', N'Trong bãi');

    -- Kiem tra khong cho bien so xe bi trung
    if exists (select name from sysobjects
    where name='trg_KiemTraBienSo' and type='TR')
    drop trigger trg_KiemTraBienSo
    GO

    CREATE TRIGGER trg_KiemTraBienSo
    ON Xe
    AFTER UPDATE
    AS
    BEGIN
        IF NOT UPDATE(BienSo) RETURN;

        IF EXISTS (
            SELECT 1
            FROM Xe x 
            JOIN inserted i ON x.BienSo = i.BienSo 
            WHERE x.MaXe <> i.MaXe
        )
        BEGIN
            RAISERROR(N'Lỗi: Biển số xe đã tồn tại trong hệ thống!', 16, 1);
            ROLLBACK TRANSACTION;
        END
    END
    GO

    -- Vi du
    UPDATE Xe 
    SET BienSo = '79-H1 23456' 
    WHERE MaXe = 'XE001';


    --Trigger: Thông báo khi thêm xe mới
    if exists (select name from sysobjects
    where name='tr_ThemXe' and type='TR')
    drop trigger tr_ThemXe
    GO

	CREATE TRIGGER tr_ThemXe
	ON Xe
	AFTER INSERT
	AS
	BEGIN
		PRINT N'Đã thêm xe mới';
	END;
	GO

    -- Vi du
    INSERT INTO Xe (MaXe, BienSo, HangXe, TenDongXe, MauXe, MaLoaiXe, MaCuDan, NamSanXuat) VALUES
    ('XE031', '79-NA 99.099', 'Honda', 'Vision', N'Trắng', 'XM', 'CD030', 2022);

	--Trigger: Thông báo khi xóa xe
    if exists (select name from sysobjects
    where name='tr_XoaXe' and type='TR')
    drop trigger tr_XoaXe
    GO
	CREATE TRIGGER tr_XoaXe
	ON Xe
	AFTER DELETE
	AS
	BEGIN
		PRINT N'Đã xóa xe khỏi hệ thống';
	END;
	GO
    
    -- Vi du
    DELETE FROM Xe
    WHERE MaXe = 'XE031';

-- Tao 5 nguoi dung va cap quyen khac nhau
CREATE LOGIN Admin_Full WITH PASSWORD = 'admin123@abc';
CREATE LOGIN NhanVien_NhanSu WITH PASSWORD = 'nhanvien123@abc';
CREATE LOGIN NhanVien_DuLieuXe WITH PASSWORD = 'nhanvien123@abc';
CREATE LOGIN NhanVien_KeToan WITH PASSWORD = 'nhanvien123@abc';
CREATE LOGIN Khach_01 WITH PASSWORD = 'khach123@abc';
GO

USE QL_BaiDoXe;
CREATE USER Admin_Full    FOR LOGIN Admin_Full;
CREATE USER NhanVien_NhanSu FOR LOGIN NhanVien_NhanSu;
CREATE USER NhanVien_DuLieuXe FOR LOGIN NhanVien_DuLieuXe;
CREATE USER NhanVien_KeToan FOR LOGIN NhanVien_KeToan;
CREATE USER Khach_01    FOR LOGIN Khach_01;
GO

-- Phan quyen
    -- Admin_Full: Toan quyen, co quyen Grant, Revoke, Deny tat ca user khac
    GRANT CONTROL TO Admin_Full;

    -- NhanVien_NhanSu: Tao them nhan vien moi
    GRANT CREATE USER TO NhanVien_NhanSu;
    GRANT ALTER ANY LOGIN TO NhanVien_NhanSu;
    GRANT ALTER ANY USER TO NhanVien_NhanSu;

    -- NhanVien_DuLieuXe: Thao tac tren xe va luot gui xe
    GRANT SELECT, INSERT, UPDATE ON LuotGuiXe TO NhanVien_DuLieuXe;
    GRANT SELECT, INSERT, UPDATE ON XE TO NhanVien_DuLieuXe;

    -- NhanVien_KeToan: Duoc xem bang ThanhToan nhung khong duoc xoa
    GRANT SELECT ON ThanhToan TO NhanVien_KeToan;
    GRANT SELECT ON ThanhToanThang TO NhanVien_KeToan;
    GRANT SELECT ON ThanhToanNgay TO NhanVien_KeToan;

    DENY DELETE ON ThanhToan TO NhanVien_KeToan;
    DENY DELETE ON ThanhToanThang TO NhanVien_KeToan;
    DENY DELETE ON ThanhToanNgay TO NhanVien_KeToan;

    -- Khach_01: Chi duoc xem
    GRANT SELECT ON ViTriDo TO Khach_01;
    GRANT SELECT ON BangGia TO Khach_01;

    REVOKE SELECT ON BangGia FROM Khach_01;
    GO