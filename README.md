# Quản lý Bãi Đỗ Xe - Parking Lot Management System

[![Build Status](https://img.shields.io/badge/build-passing-brightgreen)]()
[![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.7.2-blue)]()
[![License](https://img.shields.io/badge/license-MIT-green)]()
[![Language](https://img.shields.io/badge/language-C%23-yellow)]()

## 🎯 Tổng quan

**Quản lý Bãi Đỗ Xe** (Parking Lot Management):
- 🔐 **Role-Based Authorization** - Admin, Staff, Resident roles with customized interfaces
- 📊 **CRUD Operations** - Complete management of 15 database tables
- 💰 **Revenue Tracking** - Monthly statistics and real-time revenue monitoring
- 🚗 **Vehicle Management** - Registration, card management, parking sessions
- 📈 **Analytics** - Charts, occupancy rates, incident tracking
- 🛡️ **Security** - SQL Injection prevention, parameterized queries, Unicode support

## ✨ Chức năng chính

### 1. Đăng nhập & bảo mật
- **Multi-role system**: Admin (AD), Staff (NV), Resident (CD)
- **Secure login** with role detection
- **Dynamic UI** - Tabs and features adjust based on user role

### 2. Quản lý dữ liệu (CRUD)
- **15 database tables** fully supported
- **50+ CRUD operations** via DatabaseManager
- **Parameterized queries** for SQL injection prevention
- **Vietnamese language support** (Unicode/NVARCHAR)

### 3. Giao diện người dùng

#### Admin Dashboard (10 Tabs)
```
1. Nhân viên (Employees) - Full CRUD
2. Cư dân (Residents) - Full CRUD
3. Xe (Vehicles) - Full CRUD
4. Thẻ Xe (Parking Cards) - Full CRUD
5. Vị trí đỗ (Parking Spots) - Full CRUD
6. Loại xe (Vehicle Types) - Full CRUD
7. Khu vực (Zones) - Full CRUD
8. Lượt gửi xe (Parking Sessions) - View/Add
9. Truy vấn (Query) - Search/Filter
10. Thống kê (Statistics) - Revenue & Charts
```

#### Tổng quan cho nhân viên (5 Tabs)
```
1. Xe - Manage vehicles
2. Thẻ Xe - Manage cards
3. Vị trí đỗ - Monitor spots
4. Lượt gửi xe - Track sessions
5. Truy vấn - Search
```

#### Tổng quan cho cư dân (1 Tab)
```
1. Thông tin của tôi - View personal vehicles & history (Read-only)
```

### 4. Truy vấn và thống kê
- 📊 Monthly revenue breakdown
- 🥧 Vehicle type distribution
- 📈 Parking occupancy rates
- 💹 Daily/Total revenue summaries

### 5. Công nghệ
- MaterialSkin 2.3.1 for modern UI
- LiveCharts integration for visualizations
- ADO.NET with parameterized queries
- Responsive DataGridView controls
- Dialog forms for data entry
- Real-time error handling

## 🏗️ Kiến trúc

### Database Schema
```
15 Tables:
├── VaiTro (Roles)
├── NhanVien (Employees)
├── CuDan (Residents)
├── CanHo (Apartments)
├── Xe (Vehicles)
├── LoaiXe (Vehicle Types)
├── BangGia (Price Lists)
├── TheXe (Parking Cards)
├── KhuVuc (Zones)
├── ViTriDo (Parking Spots)
├── LuotGuiXe (Parking Sessions)
├── SuCoBaiXe (Incidents)
└── ThanhToan (Payments)
```

### Code Structure
```
ParkingDesktopUI.csproj
├── Program.cs - Entry point
├── LoginForm.cs - Authentication
├── MainForm.cs - Main dashboard (850+ lines)
├── DatabaseHelper.cs - DB connection & utilities
├── DatabaseManager.cs - CRUD operations (638 lines)
├── AdvancedFeatures.cs - Optional advanced features
└── Properties/
	└── Resources/
```

## 🚀 Cài đặt nhanh

### Cấu hình và yêu cầu
- Windows 7 SP1+
- .NET Framework 4.7.2
- SQL Server 2016+
- Visual Studio 2019+ (for development)

### Cài đặt

1. **Clone Repository**
```bash
git clone https://github.com/NguyenVoQuocViet/QL_BaiDoXe.git
cd QL_BaiDoXe/Source/ParkingDesktopUI
```

2. **Chuẩn bị database**
```sql
-- Run provided SQL script to create tables and insert sample data
-- File: QL_BaiDoXe_Script.sql
```

3. **Cấu hình Connection string**
Edit `DatabaseHelper.cs` line 25:
```csharp
private const string ConnectionString =
	"Data Source=YOUR_SERVER;Initial Catalog=QL_BaiDoXe;...";
```

4. **Chạy thử**
```
Visual Studio: Build → Build Solution (Ctrl+Shift+B)
Debug → Start Debugging (F5)
```

5. **Đăng nhập**
- **Admin**: Username `admin_viet` / Password `123`
- **Staff**: Username `thuy_nv` / Password `123`
- **Resident**: Phone `0381000001` / CCCD `123456789001`

## 📖 T

| Document | Purpose |
|----------|---------|
| `IMPLEMENTATION_GUIDE.md` | Complete feature reference |
| `DEPLOYMENT_GUIDE.md` | Installation & maintenance |
| `CODE_REFERENCE.md` | Code examples & patterns |

## 🛠️ Công nghệ sử dụng

- **Language**: C# (.NET Framework 4.7.2)
- **UI Framework**: Windows Forms + MaterialSkin 2.3.1
- **Database**: SQL Server 2016+
- **Data Access**: ADO.NET
- **Charts**: LiveCharts 0.9.7
- **Version Control**: Git
