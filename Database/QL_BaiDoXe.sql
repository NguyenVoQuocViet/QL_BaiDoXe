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
-- Cau hoi truy van
