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
    CONSTRAINT UQ_Xe_BienSo UNIQUE (BienSo),
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
('NV', N'Nhân viên', N'Nhân viên trực bãi, kiểm soát ra vào'),
('CD', N'Cư dân', N'Cư dân chung cư, xem lịch sử xe cá nhân');

INSERT INTO NhanVien (MaNhanVien, HoTen, NgaySinh, DiaChi, GioiTinh, Email, SoDienThoai, TenDangNhap, MatKhau, MaVaiTro, CaLamViec, Luong) VALUES
('NV001', N'Cổ Trần Sa', '1990-01-15', N'Nha Trang', N'Nam', 'sa.ct@gmail.com', '0905123456', 'sa_admin', '123@admin', 'AD', N'Hành chính', 15000000),
('NV002', N'Lê Thị Bình', '1992-05-20', N'Diên Khánh', N'Nữ', 'binh.lt@gmail.com', '0905123457', 'binh_nv', '123@nv', 'NV', N'Sáng', 8000000),
('NV003', N'Trần Văn Cường', '1995-11-10', N'Cam Ranh', N'Nam', 'cuong.tv@gmail.com', '0905123458', 'cuong_nv', '123@nv', 'NV', N'Chiều', 8000000),
('NV004', N'Phạm Minh Đức', '1993-02-25', N'Nha Trang', N'Nam', 'duc.pm@gmail.com', '0905123459', 'duc_nv', '123@nv', 'NV', N'Tối', 8500000),
('NV005', N'Hoàng Anh Tuấn', '1991-08-14', N'Nha Trang', N'Nam', 'tuan.ha@gmail.com', '0905123460', 'tuan_nv', '123@nv', 'NV', N'Sáng', 8000000),
('NV006', N'Đỗ Minh Tâm', '1994-03-12', N'Nha Trang', N'Nữ', 'tam.dm@gmail.com', '0905123461', 'tam_nv', '123@nv', 'NV', N'Chiều', 8000000),
('NV007', N'Vũ Quốc Việt', '1996-07-07', N'Nha Trang', N'Nam', 'viet.vq@gmail.com', '0905123462', 'viet_nv', '123@nv', 'NV', N'Tối', 8500000),
('NV008', N'Bùi Quang Hải', '1992-09-09', N'Nha Trang', N'Nam', 'hai.bq@gmail.com', '0905123463', 'hai_nv', '123@nv', 'NV', N'Sáng', 8000000),
('NV009', N'Ngô Thanh Vân', '1990-12-30', N'Nha Trang', N'Nữ', 'van.nt@gmail.com', '0905123464', 'van_nv', '123@nv', 'NV', N'Chiều', 8000000),
('NV010', N'Lý Hùng', '1988-04-18', N'Nha Trang', N'Nam', 'hung.l@gmail.com', '0905123465', 'hung_nv', '123@nv', 'NV', N'Tối', 8500000),
('NV011', N'Trương Ngọc Ánh', '1995-10-22', N'Nha Trang', N'Nữ', 'anh.tn@gmail.com', '0905123466', 'anh_nv', '123@nv', 'NV', N'Sáng', 8000000),
('NV012', N'Phan Anh', '1993-01-05', N'Nha Trang', N'Nam', 'anh.p@gmail.com', '0905123467', 'panh_nv', '123@nv', 'NV', N'Chiều', 8000000),
('NV013', N'Hồng Hoài Linh', '1985-06-15', N'Nha Trang', N'Nam', 'linh.hh@gmail.com', '0905123468', 'linh_nv', '123@nv', 'NV', N'Tối', 8500000),
('NV014', N'Lê Minh Quân', '1989-08-21', N'Nha Trang', N'Nam', 'quan.lm@gmail.com', '0905123469', 'bac_nv', '123@nv', 'NV', N'Sáng', 8000000),
('NV015', N'Hồ Lê Minh', '1990-11-11', N'Nha Trang', N'Nam', 'long.t@gmail.com', '0905123470', 'long_nv', '123@nv', 'NV', N'Chiều', 8000000),
('NV016', N'Công Lý', '1987-02-02', N'Nha Trang', N'Nam', 'ly.c@gmail.com', '0905123471', 'ly_nv', '123@nv', 'NV', N'Tối', 8500000),
('NV017', N'Quang Thắng', '1988-03-03', N'Nha Trang', N'Nam', 'thang.q@gmail.com', '0905123472', 'thang_nv', '123@nv', 'NV', N'Sáng', 8000000),
('NV018', N'Vân Dung', '1991-04-04', N'Nha Trang', N'Nữ', 'dung.v@gmail.com', '0905123473', 'dung_nv', '123@nv', 'NV', N'Chiều', 8000000),
('NV019', N'Vũ Hoài Phong Thành', '1994-05-05', N'Nha Trang', N'Nam', 'thanh.vhp@gmail.com', '0905123474', 'thanh_nv', '123@nv', 'NV', N'Tối', 8500000),
('NV020', N'Trường Thành Giang', '1992-06-06', N'Nha Trang', N'Nam', 'giang.tt@gmail.com', '0905123475', 'giang_nv', '123@nv', 'NV', N'Sáng', 8000000),
('NV021', N'Hoa Ngọc Tâm', '1995-07-07', N'Nha Trang', N'Nữ', 'tam.hn@gmail.com', '0905123476', 'nhu_nv', '123@nv', 'NV', N'Chiều', 8000000),
('NV022', N'Nguyễn Thị Nhi', '1996-08-08', N'Nha Trang', N'Nữ', 'nhi.nt@gmail.com', '0905123477', 'nhi_nv', '123@nv', 'NV', N'Tối', 8500000),
('NV023', N'Võ Thiên Tú', '1993-09-09', N'Nha Trang', N'Nam', 'tu.vt@gmail.com', '0905123478', 'tu_nv', '123@nv', 'NV', N'Sáng', 8000000),
('NV024', N'Ngô Hải Tú', '1997-10-10', N'Nha Trang', N'Nữ', 'tu.nh@gmail.com', '0905123479', 'haitu_nv', '123@nv', 'NV', N'Chiều', 8000000),
('NV025', N'Đặng Thanh Tùng', '1994-07-05', N'Nha Trang', N'Nam', 'tung.dt@gmail.com', '0905123480', 'tung_nv', '123@nv', 'NV', N'Tối', 8500000);

-- 2.Cu dan va can ho
INSERT INTO CanHo (MaCanHo, SoCanHo, ToaNha, Tang, TrangThai) VALUES
('CH001', 'P101', 'Block A', 1, N'Đã thuê'), ('CH002', 'P102', 'Block A', 1, N'Đã thuê'),
('CH003', 'P201', 'Block A', 2, N'Đã thuê'), ('CH004', 'P202', 'Block A', 2, N'Trống'),
('CH005', 'P301', 'Block A', 3, N'Đã thuê'), ('CH006', 'P101', 'Block B', 1, N'Đã thuê'),
('CH007', 'P102', 'Block B', 1, N'Đã thuê'), ('CH008', 'P201', 'Block B', 2, N'Đã thuê'),
('CH009', 'P202', 'Block B', 2, N'Đã thuê'), ('CH010', 'P301', 'Block B', 3, N'Đã thuê'),
('CH011', 'P401', 'Block A', 4, N'Đã thuê'), ('CH012', 'P402', 'Block A', 4, N'Đã thuê'),
('CH013', 'P501', 'Block A', 5, N'Đã thuê'), ('CH014', 'P502', 'Block A', 5, N'Trống'),
('CH015', 'P601', 'Block A', 6, N'Đã thuê'), ('CH016', 'P401', 'Block B', 4, N'Đã thuê'),
('CH017', 'P402', 'Block B', 4, N'Đã thuê'), ('CH018', 'P501', 'Block B', 5, N'Đã thuê'),
('CH019', 'P502', 'Block B', 5, N'Đã thuê'), ('CH020', 'P601', 'Block B', 6, N'Đã thuê'),
('CH021', 'P701', 'Block A', 7, N'Đã thuê'), ('CH022', 'P702', 'Block A', 7, N'Đã thuê'),
('CH023', 'P701', 'Block B', 7, N'Đã thuê'), ('CH024', 'P801', 'Block B', 8, N'Đã thuê'),
('CH025', 'P901', 'Block B', 9, N'Sửa chữa');

INSERT INTO CuDan (MaCuDan, HoTen, CCCD, Email, SoDienThoai, DiaChiCanHo, MaVaiTro) VALUES
('CD001', N'Lê Tấn Danh', '123456789001', 'danh.lt@gmail.com', '0381000001', 'Block A - P101', 'CD'),
('CD002', N'Nguyễn Hạnh Quyên', '123456789002', 'quyen.nh@gmail.com', '0381000002', 'Block A - P102', 'CD'),
('CD003', N'Nguyễn Võ Quốc Việt', '123456789003', 'viet.nvq@gmail.com', '0381000003', 'Block A - P201', 'CD'),
('CD004', N'Hoàng Văn Bốn', '123456789004', 'bon.hv@gmail.com', '0381000004', 'Block A - P301', 'CD'),
('CD005', N'Nguyễn Thị Năm', '123456789005', 'nam.nt@gmail.com', '0381000005', 'Block B - P101', 'CD'),
('CD006', N'Vũ Văn Sáu', '123456789006', 'sau.vv@gmail.com', '0381000006', 'Block B - P102', 'CD'),
('CD007', N'Phan Thị Bảy', '123456789007', 'bay.pt@gmail.com', '0381000007', 'Block B - P201', 'CD'),
('CD008', N'Đặng Văn Hai', '123456789008', 'hai.dv@gmail.com', '0381000008', 'Block B - P202', 'CD'),
('CD009', N'Bùi Thị Một', '123456789009', 'mot.bt@gmail.com', '0381000009', 'Block B - P301', 'CD'),
('CD010', N'Cao Văn Ba', '123456789010', 'ba.cv@gmail.com', '0381000010', 'Block A - P401', 'CD'),
('CD011', N'Lương Thị Hồng', '123456789011', 'hong.lt@gmail.com', '0381000011', 'Block A - P402', 'CD'),
('CD012', N'Đỗ Văn Quân', '123456789012', 'quan.dv@gmail.com', '0381000012', 'Block A - P501', 'CD'),
('CD013', N'Ngô Thị Lan', '123456789013', 'lan.nt@gmail.com', '0381000013', 'Block A - P601', 'CD'),
('CD014', N'Trịnh Văn Đại', '123456789014', 'dai.tv@gmail.com', '0381000014', 'Block B - P401', 'CD'),
('CD015', N'Lưu Thị Huệ', '123456789015', 'hue.lt@gmail.com', '0381000015', 'Block B - P402', 'CD'),
('CD016', N'Mai Văn Sơn', '123456789016', 'son.mv@gmail.com', '0381000016', 'Block B - P501', 'CD'),
('CD017', N'Tạ Thị Đào', '123456789017', 'dao.tt@gmail.com', '0381000017', 'Block B - P502', 'CD'),
('CD018', N'Hà Văn Hùng', '123456789018', 'hung.hv@gmail.com', '0381000018', 'Block B - P601', 'CD'),
('CD019', N'Kiều Thị Mai', '123456789019', 'mai.kt@gmail.com', '0381000019', 'Block A - P701', 'CD'),
('CD020', N'Thân Văn Thắng', '123456789020', 'thang.tv@gmail.com', '0381000020', 'Block A - P702', 'CD'),
('CD021', N'Sầm Thị Tuyết', '123456789021', 'tuyet.st@gmail.com', '0381000021', 'Block B - P701', 'CD'),
('CD022', N'Dương Văn Đông', '123456789022', 'dong.dv@gmail.com', '0381000022', 'Block B - P801', 'CD'),
('CD023', N'Lê Thị Phương', '123456789023', 'phuong.lt@gmail.com', '0381000023', 'Block B - P901', 'CD'),
('CD024', N'Nguyễn Văn Giang', '123456789024', 'giang.nv@gmail.com', '0381000024', 'Block B - P101', 'CD'),
('CD025', N'Trần Anh Tuấn', '123456789025', 'tuan.ta@gmail.com', '0381000025', 'Block A - P101', 'CD');

INSERT INTO CuDan_CanHo (MaCuDan, MaCanHo, VaiTroCuDan, NgayBatDau) VALUES
('CD001', 'CH001', N'Chủ hộ', '2025-01-01'), ('CD002', 'CH002', N'Chủ hộ', '2025-01-05'),
('CD003', 'CH003', N'Chủ hộ', '2025-01-10'), ('CD004', 'CH005', N'Chủ hộ', '2025-01-15'),
('CD005', 'CH006', N'Chủ hộ', '2025-01-20'), ('CD006', 'CH007', N'Chủ hộ', '2025-02-01'),
('CD007', 'CH008', N'Chủ hộ', '2025-02-05'), ('CD008', 'CH009', N'Chủ hộ', '2025-02-10'),
('CD009', 'CH010', N'Chủ hộ', '2025-02-15'), ('CD010', 'CH011', N'Chủ hộ', '2025-03-01'),
('CD011', 'CH012', N'Chủ hộ', '2025-03-05'), ('CD012', 'CH013', N'Chủ hộ', '2025-03-10'),
('CD013', 'CH015', N'Chủ hộ', '2025-03-15'), ('CD014', 'CH016', N'Chủ hộ', '2025-04-01'),
('CD015', 'CH017', N'Chủ hộ', '2025-04-05'), ('CD016', 'CH018', N'Chủ hộ', '2025-04-10'),
('CD017', 'CH019', N'Chủ hộ', '2025-04-15'), ('CD018', 'CH020', N'Chủ hộ', '2025-05-01'),
('CD019', 'CH021', N'Chủ hộ', '2025-05-05'), ('CD020', 'CH022', N'Chủ hộ', '2025-05-10'),
('CD021', 'CH023', N'Chủ hộ', '2025-05-15'), ('CD022', 'CH024', N'Chủ hộ', '2025-06-01'),
('CD023', 'CH025', N'Khách thuê', '2026-05-01'), ('CD024', 'CH006', N'Thành viên', '2025-06-10'),
('CD025', 'CH001', N'Thành viên', '2025-06-15');

-- 3. Xe va bang gia
INSERT INTO LoaiXe (MaLoaiXe, TenLoaiXe, GiaTienThang, GiaTienNgay) VALUES
('XM', N'Xe máy', 100000, 5000),
('OT', N'Ô tô', 1000000, 30000);

INSERT INTO BangGia (MaBangGia, MaLoaiXe, LoaiTinhPhi, DonGia, NgayApDung) VALUES
('BG01', 'XM', N'Thẻ tháng', 100000, '2025-01-01'),
('BG02', 'OT', N'Thẻ tháng', 1000000, '2025-01-01'),
('BG03', 'XM', N'Thẻ ngày', 5000, '2025-01-01'),
('BG04', 'OT', N'Thẻ ngày', 30000, '2025-01-01');

INSERT INTO Xe (MaXe, BienSo, HangXe, TenDongXe, MauXe, MaLoaiXe, MaCuDan) VALUES
('XE001', '79-H1 12345', 'Honda', 'Vision', N'Trắng', 'XM', 'CD001'),
('XE002', '79-H1 23456', 'Yamaha', 'Exciter', N'Xanh', 'XM', 'CD002'),
('XE003', '79-A 001.23', 'Toyota', 'Vios', N'Đen', 'OT', 'CD003'),
('XE004', '79-H1 34567', 'Honda', 'AirBlade', N'Đỏ', 'XM', 'CD004'),
('XE005', '79-A 005.67', 'Hyundai', 'Accent', N'Trắng', 'OT', 'CD005'),
('XE006', '79-H1 45678', 'Honda', 'SH', N'Đen', 'XM', 'CD006'),
('XE007', '79-H1 56789', 'Suzuki', 'Raider', N'Tím', 'XM', 'CD007'),
('XE008', '79-A 008.90', 'Kia', 'Cerato', N'Đỏ', 'OT', 'CD008'),
('XE009', '79-H1 67890', 'Yamaha', 'Janus', N'Hồng', 'XM', 'CD009'),
('XE010', '79-H1 78901', 'Honda', 'Wave', N'Xanh', 'XM', 'CD010'),
('XE011', '79-A 011.22', 'Mazda', 'CX-5', N'Xám', 'OT', 'CD011'),
('XE012', '79-H1 89012', 'Honda', 'Lead', N'Vàng', 'XM', 'CD012'),
('XE013', '79-H1 90123', 'Piaggio', 'Vespa', N'Trắng', 'XM', 'CD013'),
('XE014', '79-A 014.33', 'VinFast', 'Lux A2.0', N'Đen', 'OT', 'CD014'),
('XE015', '79-H1 01234', 'Honda', 'Future', N'Đen', 'XM', 'CD015'),
('XE016', '79-H1 11223', 'Yamaha', 'Grande', N'Trắng', 'XM', 'CD016'),
('XE017', '79-A 017.44', 'Ford', 'Ranger', N'Cam', 'OT', 'CD017'),
('XE018', '79-H1 22334', 'Suzuki', 'Satria', N'Xanh', 'XM', 'CD018'),
('XE019', '79-H1 33445', 'Honda', 'Winner X', N'Bạc', 'XM', 'CD019'),
('XE020', '79-A 020.55', 'Honda', 'City', N'Trắng', 'OT', 'CD020'),
('XE021', '79-H1 44556', 'Yamaha', 'NVX', N'Đen', 'XM', 'CD021'),
('XE022', '79-H1 55667', 'Honda', 'Blade', N'Đỏ', 'XM', 'CD022'),
('XE023', '79-A 023.66', 'Mitsubishi', 'Xpander', N'Nâu', 'OT', 'CD023'),
('XE024', '79-H1 66778', 'Honda', 'Vario', N'Xám', 'XM', 'CD024'),
('XE025', '79-H1 77889', 'Sym', 'Attila', N'Trắng', 'XM', 'CD025');

-- 4.The xe
INSERT INTO TheXe (MaThe, SoThe, MaXe, LoaiThe, NgayHetHan) VALUES
('UID001', 'A1B2C3D4', 'XE001', N'Thẻ tháng', '2026-12-31'),
('UID002', '9F3A7C1E', 'XE002', N'Thẻ tháng', '2026-12-31'),
('UID003', '4D8E2B9A', 'XE003', N'Thẻ tháng', '2026-12-31'),
('UID004', '7C1F5A3D', 'XE004', N'Thẻ tháng', '2026-12-31'),
('UID005', 'E2A4B6C8', 'XE005', N'Thẻ tháng', '2026-12-31'),
('UID006', '5F9D3E1A', 'XE006', N'Thẻ tháng', '2026-12-31'),
('UID007', 'B7C2D4E6', 'XE007', N'Thẻ tháng', '2026-12-31'),
('UID008', '3A8F1C5B', 'XE008', N'Thẻ tháng', '2026-12-31'),
('UID009', 'D9E4A2C7', 'XE009', N'Thẻ tháng', '2026-12-31'),
('UID010', '6B1D3F8A', 'XE010', N'Thẻ tháng', '2026-12-31'),
('UID011', 'C4E7B2A9', 'XE011', N'Thẻ tháng', '2026-12-31'),
('UID012', '8A3C5F1D', 'XE012', N'Thẻ tháng', '2026-12-31'),
('UID013', 'F1B2C3A4', 'XE013', N'Thẻ tháng', '2026-12-31'),
('UID014', '2D6E8F1B', 'XE014', N'Thẻ tháng', '2026-12-31'),
('UID015', 'A9C3E5F7', 'XE015', N'Thẻ tháng', '2026-12-31'),
('UID016', '1F4B7D2A', NULL, N'Thẻ ngày', NULL),
('UID017', 'E8C2A1B3', NULL, N'Thẻ ngày', NULL),
('UID018', '7D5F3A9C', NULL, N'Thẻ ngày', NULL),
('UID019', 'B1E2C4D6', NULL, N'Thẻ ngày', NULL),
('UID020', '3F7A9E1C', NULL, N'Thẻ ngày', NULL),
('UID021', 'D2B4A6C8', 'XE021', N'Thẻ tháng', '2026-12-31'),
('UID022', '9A1C3E5F', 'XE022', N'Thẻ tháng', '2026-12-31'),
('UID023', '6E8F2D4B', 'XE023', N'Thẻ tháng', '2026-12-31'),
('UID024', 'C7A9B3E1', 'XE024', N'Thẻ tháng', '2026-12-31'),
('UID025', '5A3D7F1C', 'XE025', N'Thẻ tháng', '2026-12-31');

-- 5.Khu vuc do xe
INSERT INTO KhuVuc (MaKhu, TenKhu, Tang, SucChuaToiDa) VALUES
('K01', 'Khu A-H1', -1, 100), ('K02', 'Khu B-H1', -1, 100),
('K03', 'Khu C-H1', -1, 50), ('K04', 'Khu A-H2', -2, 100),
('K05', 'Khu B-H2', -2, 100), ('K06', 'Khu C-H2', -2, 50),
('K07', 'Khu Vip', -1, 20), ('K08', 'Khu Xe Dien', -1, 30),
('K09', 'Khu vang lai', 1, 50), ('K10', 'Block C1', -1, 100),
('K11', 'Block C2', -1, 100), ('K12', 'Block D1', -2, 100),
('K13', 'Block D2', -2, 100), ('K14', 'Tang tret 1', 0, 50),
('K15', 'Tang tret 2', 0, 50), ('K16', 'Ham B3-A', -3, 80),
('K17', 'Ham B3-B', -3, 80), ('K18', 'Khu xe tai', 1, 10),
('K19', 'Khu rua xe', -1, 5), ('K20', 'Khu bao tri', -1, 5),
('K21', 'Khu vuc cho', 1, 20), ('K22', 'Block E1', -1, 100),
('K23', 'Block E2', -2, 100), ('K24', 'Khu A-H3', -3, 100),
('K25', 'Khu B-H3', -3, 100);

INSERT INTO ViTriDo (MaViTri, MaKhu, TenViTri, LoaiViTri, TrangThai) VALUES
('V001', 'K01', 'A1-01', N'Thường', 1), ('V002', 'K01', 'A1-02', N'Thường', 1),
('V003', 'K01', 'A1-03', N'Thường', 0), ('V004', 'K02', 'B1-01', N'Thường', 1),
('V005', 'K02', 'B1-02', N'Thường', 1), ('V006', 'K03', 'C1-01', N'Thường', 1),
('V007', 'K04', 'A2-01', N'Thường', 1), ('V008', 'K04', 'A2-02', N'Thường', 0),
('V009', 'K07', 'VIP-01', N'Vip', 1), ('V010', 'K07', 'VIP-02', N'Vip', 0),
('V011', 'K08', 'ED-01', N'Xe điện', 0), ('V012', 'K09', 'VL-01', N'Vãng lai', 1),
('V013', 'K09', 'VL-02', N'Vãng lai', 1), ('V014', 'K10', 'C1-10', N'Thường', 0),
('V015', 'K11', 'C2-11', N'Thường', 1), ('V016', 'K12', 'D1-12', N'Thường', 1),
('V017', 'K13', 'D2-13', N'Thường', 0), ('V018', 'K14', 'T1-14', N'Vãng lai', 1),
('V019', 'K15', 'T2-15', N'Vãng lai', 0), ('V020', 'K16', 'B3-01', N'Thường', 1),
('V021', 'K17', 'B3-20', N'Thường', 1), ('V022', 'K22', 'E1-01', N'Thường', 1),
('V023', 'K23', 'E2-01', N'Thường', 1), ('V024', 'K24', 'A3-01', N'Thường', 1),
('V025', 'K25', 'B3-01', N'Thường', 1);

INSERT INTO LuotGuiXe (MaLuotGui, MaThe, MaViTri, ThoiGianVao, ThoiGianRa, MaNVVao, MaNVRa, PhuongThucTinhPhi, TrangThaiLuotGui, TongTien) VALUES
('L001', 'UID001', 'V001', '2026-05-01 07:00:00', '2026-05-01 17:00:00', 'NV002', 'NV003', N'Thẻ tháng', N'Đã ra', 0),
('L002', 'UID002', 'V002', '2026-05-01 08:00:00', NULL, 'NV002', NULL, N'Thẻ tháng', N'Trong bãi', 0),
('L003', 'UID016', 'V012', '2026-05-01 09:00:00', '2026-05-01 11:00:00', 'NV002', 'NV002', N'Thẻ ngày', N'Đã ra', 10000),
('L004', 'UID003', 'V004', '2026-05-01 10:00:00', NULL, 'NV003', NULL, N'Thẻ tháng', N'Trong bãi', 0),
('L005', 'UID017', 'V013', '2026-05-01 14:00:00', '2026-05-01 20:00:00', 'NV003', 'NV004', N'Thẻ ngày', N'Đã ra', 30000),
('L006', 'UID004', 'V005', '2026-05-02 07:00:00', NULL, 'NV005', NULL, N'Thẻ tháng', N'Trong bãi', 0),
('L007', 'UID005', 'V006', '2026-05-02 08:00:00', NULL, 'NV005', NULL, N'Thẻ tháng', N'Trong bãi', 0),
('L008', 'UID018', 'V018', '2026-05-02 09:00:00', '2026-05-02 10:00:00', 'NV005', 'NV005', N'Thẻ ngày', N'Đã ra', 5000),
('L009', 'UID006', 'V007', '2026-05-02 10:00:00', NULL, 'NV006', NULL, N'Thẻ tháng', N'Trong bãi', 0),
('L010', 'UID007', 'V009', '2026-05-02 11:00:00', NULL, 'NV006', NULL, N'Thẻ tháng', N'Trong bãi', 0),
('L011', 'UID019', 'V020', '2026-05-03 07:00:00', '2026-05-03 17:00:00', 'NV008', 'NV009', N'Thẻ ngày', N'Đã ra', 15000),
('L012', 'UID008', 'V021', '2026-05-03 08:00:00', NULL, 'NV008', NULL, N'Thẻ tháng', N'Trong bãi', 0),
('L013', 'UID009', 'V022', '2026-05-03 09:00:00', NULL, 'NV008', NULL, N'Thẻ tháng', N'Trong bãi', 0),
('L014', 'UID020', 'V023', '2026-05-03 10:00:00', '2026-05-03 11:00:00', 'NV009', 'NV009', N'Thẻ ngày', N'Đã ra', 10000),
('L015', 'UID010', 'V024', '2026-05-03 11:00:00', NULL, 'NV009', NULL, N'Thẻ tháng', N'Trong bãi', 0),
('L016', 'UID011', 'V025', '2026-05-04 07:00:00', NULL, 'NV011', NULL, N'Thẻ tháng', N'Trong bãi', 0),
('L017', 'UID021', 'V001', '2026-05-04 08:00:00', NULL, 'NV011', NULL, N'Thẻ tháng', N'Trong bãi', 0),
('L018', 'UID022', 'V002', '2026-05-04 09:00:00', NULL, 'NV011', NULL, N'Thẻ tháng', N'Trong bãi', 0),
('L019', 'UID012', 'V004', '2026-05-04 10:00:00', NULL, 'NV012', NULL, N'Thẻ tháng', N'Trong bãi', 0),
('L020', 'UID013', 'V005', '2026-05-04 11:00:00', NULL, 'NV012', NULL, N'Thẻ tháng', N'Trong bãi', 0),
('L021', 'UID023', 'V006', '2026-05-04 13:00:00', NULL, 'NV012', NULL, N'Thẻ tháng', N'Trong bãi', 0),
('L022', 'UID014', 'V007', '2026-05-04 14:00:00', NULL, 'NV013', NULL, N'Thẻ tháng', N'Trong bãi', 0),
('L023', 'UID024', 'V009', '2026-05-04 15:00:00', NULL, 'NV013', NULL, N'Thẻ tháng', N'Trong bãi', 0),
('L024', 'UID015', 'V012', '2026-05-04 16:00:00', NULL, 'NV013', NULL, N'Thẻ tháng', N'Trong bãi', 0),
('L025', 'UID025', 'V013', '2026-05-04 17:00:00', NULL, 'NV014', NULL, N'Thẻ tháng', N'Trong bãi', 0);

-- 6. Thanh toan
INSERT INTO ThanhToan (MaThanhToan, LoaiThanhToan, SoTien, NgayThanhToan, PhuongThuc, MaNhanVien, TrangThai) VALUES
('TT001', N'Ngày', 10000, '2026-05-01 11:00:00', N'Tiền mặt', 'NV002', N'Thành công'),
('TT002', N'Ngày', 30000, '2026-05-01 20:00:00', N'Chuyển khoản', 'NV004', N'Thành công'),
('TT003', N'Tháng', 100000, '2025-12-01', N'Tiền mặt', 'NV001', N'Thành công'),
('TT004', N'Tháng', 100000, '2025-12-01', N'Tiền mặt', 'NV001', N'Thành công'),
('TT005', N'Tháng', 1000000, '2025-12-01', N'Tiền mặt', 'NV001', N'Thành công'),
('TT006', N'Ngày', 5000, '2026-05-02 10:00:00', N'Tiền mặt', 'NV005', N'Thành công'),
('TT007', N'Ngày', 15000, '2026-05-03 17:00:00', N'Tiền mặt', 'NV009', N'Thành công'),
('TT008', N'Ngày', 10000, '2026-05-03 11:00:00', N'Tiền mặt', 'NV009', N'Thành công'),
('TT009', N'Tháng', 100000, '2025-12-01', N'Tiền mặt', 'NV001', N'Thành công'),
('TT010', N'Tháng', 1000000, '2025-12-01', N'Tiền mặt', 'NV001', N'Thành công'),
('TT011', N'Tháng', 100000, '2025-12-01', N'Tiền mặt', 'NV001', N'Thành công'),
('TT012', N'Tháng', 100000, '2025-12-01', N'Tiền mặt', 'NV001', N'Thành công'),
('TT013', N'Tháng', 1000000, '2025-12-01', N'Tiền mặt', 'NV001', N'Thành công'),
('TT014', N'Tháng', 100000, '2025-12-01', N'Tiền mặt', 'NV001', N'Thành công'),
('TT015', N'Tháng', 100000, '2025-12-01', N'Tiền mặt', 'NV001', N'Thành công'),
('TT016', N'Tháng', 1000000, '2025-12-01', N'Tiền mặt', 'NV001', N'Thành công'),
('TT017', N'Tháng', 100000, '2025-12-01', N'Tiền mặt', 'NV001', N'Thành công'),
('TT018', N'Tháng', 100000, '2025-12-01', N'Tiền mặt', 'NV001', N'Thành công'),
('TT019', N'Tháng', 1000000, '2025-12-01', N'Tiền mặt', 'NV001', N'Thành công'),
('TT020', N'Tháng', 100000, '2025-12-01', N'Tiền mặt', 'NV001', N'Thành công'),
('TT021', N'Tháng', 100000, '2025-12-01', N'Tiền mặt', 'NV001', N'Thành công'),
('TT022', N'Tháng', 1000000, '2025-12-01', N'Tiền mặt', 'NV001', N'Thành công'),
('TT023', N'Tháng', 100000, '2025-12-01', N'Tiền mặt', 'NV001', N'Thành công'),
('TT024', N'Tháng', 100000, '2025-12-01', N'Tiền mặt', 'NV001', N'Thành công'),
('TT025', N'Tháng', 100000, '2025-12-01', N'Tiền mặt', 'NV001', N'Thành công');

INSERT INTO ThanhToanNgay (MaThanhToan, MaLuotGui) VALUES
('TT001', 'L003'), ('TT002', 'L005'), ('TT006', 'L008'), ('TT007', 'L011'), ('TT008', 'L014');

INSERT INTO ThanhToanThang (MaThanhToan, MaThe, ThanhToanTuNgay, ThanhToanDenNgay) VALUES
('TT003', 'UID001', '2026-01-01', '2026-12-31'), ('TT004', 'UID002', '2026-01-01', '2026-12-31'),
('TT005', 'UID003', '2026-01-01', '2026-12-31'), ('TT009', 'UID004', '2026-01-01', '2026-12-31'),
('TT010', 'UID005', '2026-01-01', '2026-12-31'), ('TT011', 'UID006', '2026-01-01', '2026-12-31'),
('TT012', 'UID007', '2026-01-01', '2026-12-31'), ('TT013', 'UID008', '2026-01-01', '2026-12-31'),
('TT014', 'UID009', '2026-01-01', '2026-12-31'), ('TT015', 'UID010', '2026-01-01', '2026-12-31'),
('TT016', 'UID011', '2026-01-01', '2026-12-31'), ('TT017', 'UID012', '2026-01-01', '2026-12-31'),
('TT018', 'UID013', '2026-01-01', '2026-12-31'), ('TT019', 'UID014', '2026-01-01', '2026-12-31'),
('TT020', 'UID015', '2026-01-01', '2026-12-31'), ('TT021', 'UID021', '2026-01-01', '2026-12-31'),
('TT022', 'UID022', '2026-01-01', '2026-12-31'), ('TT023', 'UID023', '2026-01-01', '2026-12-31'),
('TT024', 'UID024', '2026-01-01', '2026-12-31'), ('TT025', 'UID025', '2026-01-01', '2026-12-31');

-- 7.Lich su the xe, su co, vi tri do
INSERT INTO LichSuTheXe (MaThe, TrangThaiCu, TrangThaiMoi, GhiChu) VALUES
('UID001', N'Trống', N'Đang hoạt động', N'Cấp mới thẻ tháng'),
('UID016', N'Trống', N'Đang hoạt động', N'Cấp mới thẻ ngày'),
('UID002', N'Trống', N'Đang hoạt động', N'Cấp mới thẻ tháng'),
('UID003', N'Trống', N'Đang hoạt động', N'Cấp mới thẻ tháng'),
('UID004', N'Trống', N'Đang hoạt động', N'Cấp mới thẻ tháng'),
('UID005', N'Trống', N'Đang hoạt động', N'Cấp mới thẻ tháng'),
('UID006', N'Trống', N'Đang hoạt động', N'Cấp mới thẻ tháng'),
('UID007', N'Trống', N'Đang hoạt động', N'Cấp mới thẻ tháng'),
('UID008', N'Trống', N'Đang hoạt động', N'Cấp mới thẻ tháng'),
('UID009', N'Trống', N'Đang hoạt động', N'Cấp mới thẻ tháng'),
('UID010', N'Trống', N'Đang hoạt động', N'Cấp mới thẻ tháng'),
('UID011', N'Trống', N'Đang hoạt động', N'Cấp mới thẻ tháng'),
('UID012', N'Trống', N'Đang hoạt động', N'Cấp mới thẻ tháng'),
('UID013', N'Trống', N'Đang hoạt động', N'Cấp mới thẻ tháng'),
('UID014', N'Trống', N'Đang hoạt động', N'Cấp mới thẻ tháng'),
('UID015', N'Trống', N'Đang hoạt động', N'Cấp mới thẻ tháng'),
('UID017', N'Trống', N'Đang hoạt động', N'Cấp mới thẻ ngày'),
('UID018', N'Trống', N'Đang hoạt động', N'Cấp mới thẻ ngày'),
('UID019', N'Trống', N'Đang hoạt động', N'Cấp mới thẻ ngày'),
('UID020', N'Trống', N'Đang hoạt động', N'Cấp mới thẻ ngày'),
('UID021', N'Trống', N'Đang hoạt động', N'Cấp mới thẻ tháng'),
('UID022', N'Trống', N'Đang hoạt động', N'Cấp mới thẻ tháng'),
('UID023', N'Trống', N'Đang hoạt động', N'Cấp mới thẻ tháng'),
('UID024', N'Trống', N'Đang hoạt động', N'Cấp mới thẻ tháng'),
('UID025', N'Trống', N'Đang hoạt động', N'Cấp mới thẻ tháng');

INSERT INTO SuCoBaiXe (MaLuotGui, NoiDung, TrangThai, ChiPhi) VALUES
('L001', N'Trầy xước nhẹ đuôi xe', N'Xử lý xong', 0),
('L003', N'Khách làm mất vé tạm', N'Xử lý xong', 50000),
('L005', N'Hư hỏng thanh chắn khi ra', N'Xử lý xong', 200000),
('L002', N'Xe chảy dầu tại vị trí đỗ', N'Đang chờ xử lý', 0),
('L004', N'Còi báo động xe tự kêu', N'Xử lý xong', 0),
('L006', N'Mất mũ bảo hiểm', N'Đang chờ xử lý', 100000),
('L007', N'Va chạm nhẹ với xe bên cạnh', N'Xử lý xong', 0),
('L008', N'Lốp xe bị xì hơi', N'Xử lý xong', 0),
('L009', N'Quên chìa khóa trên xe', N'Xử lý xong', 0),
('L010', N'Thẻ không đọc được dữ liệu', N'Xử lý xong', 0),
('L011', N'Xe để sai vị trí quy định', N'Xử lý xong', 20000),
('L012', N'Để quên đồ trên xe', N'Xử lý xong', 0),
('L013', N'Kính chiếu hậu bị lỏng', N'Xử lý xong', 0),
('L014', N'Khách nhầm vị trí đỗ', N'Xử lý xong', 0),
('L015', N'Xe quá khổ so với ô đỗ', N'Xử lý xong', 0),
('L016', N'Hệ thống camera không nhận biển số', N'Xử lý xong', 0),
('L017', N'Xe rò rỉ xăng', N'Đang chờ xử lý', 0),
('L018', N'Cửa xe không đóng chặt', N'Xử lý xong', 0),
('L019', N'Báo cháy giả tại khu vực', N'Xử lý xong', 0),
('L020', N'Nhân viên ghi sai thông tin', N'Xử lý xong', 0),
('L021', N'Tranh chấp vị trí đỗ', N'Xử lý xong', 0),
('L022', N'Xe bị ngập nước cục bộ', N'Xử lý xong', 500000),
('L023', N'Bảng điện khu vực hỏng', N'Xử lý xong', 0),
('L024', N'Xe lạ cố tình vào khu cư dân', N'Xử lý xong', 0),
('L025', N'Quẹt thẻ nhiều lần không ăn', N'Xử lý xong', 0);

INSERT INTO LichSuViTriDo (MaViTri, MaThe, ThoiGianBatDau, ThoiGianKetThuc) VALUES
('V001', 'UID001', '2026-05-01 07:00:00', '2026-05-01 17:00:00'),
('V012', 'UID016', '2026-05-01 09:00:00', '2026-05-01 11:00:00'),
('V013', 'UID017', '2026-05-01 14:00:00', '2026-05-01 20:00:00'),
('V018', 'UID018', '2026-05-02 09:00:00', '2026-05-02 10:00:00'),
('V020', 'UID019', '2026-05-03 07:00:00', '2026-05-03 17:00:00'),
('V023', 'UID020', '2026-05-03 10:00:00', '2026-05-03 11:00:00'),
('V001', 'UID021', '2026-05-04 08:00:00', NULL),
('V002', 'UID022', '2026-05-04 09:00:00', NULL),
('V002', 'UID002', '2026-05-01 08:00:00', NULL),
('V004', 'UID003', '2026-05-01 10:00:00', NULL),
('V005', 'UID004', '2026-05-02 07:00:00', NULL),
('V006', 'UID005', '2026-05-02 08:00:00', NULL),
('V007', 'UID006', '2026-05-02 10:00:00', NULL),
('V009', 'UID007', '2026-05-02 11:00:00', NULL),
('V021', 'UID008', '2026-05-03 08:00:00', NULL),
('V022', 'UID009', '2026-05-03 09:00:00', NULL),
('V024', 'UID010', '2026-05-03 11:00:00', NULL),
('V025', 'UID011', '2026-05-04 07:00:00', NULL),
('V004', 'UID012', '2026-05-04 10:00:00', NULL),
('V005', 'UID013', '2026-05-04 11:00:00', NULL),
('V006', 'UID023', '2026-05-04 13:00:00', NULL),
('V007', 'UID014', '2026-05-04 14:00:00', NULL),
('V009', 'UID024', '2026-05-04 15:00:00', NULL),
('V012', 'UID015', '2026-05-04 16:00:00', NULL),
('V013', 'UID025', '2026-05-04 17:00:00', NULL);

-- Cau hoi truy van
