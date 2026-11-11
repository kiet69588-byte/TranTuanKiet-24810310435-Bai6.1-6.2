
CREATE DATABASE SalesDB;
GO

USE SalesDB;
GO

CREATE TABLE NhanVien
(
    MaNV INT IDENTITY(1,1) PRIMARY KEY,
    TenNV NVARCHAR(100) NOT NULL,
    SoDienThoai VARCHAR(15) UNIQUE,
    ChucVu NVARCHAR(50)
);
GO

CREATE TABLE KhachHang
(
    MaKH INT IDENTITY(1,1) PRIMARY KEY,
    TenKH NVARCHAR(100) NOT NULL,
    DienThoai VARCHAR(15) UNIQUE,
    DiemTichLuy INT DEFAULT 0
);
GO

CREATE TABLE SanPham
(
    MaSP VARCHAR(20) PRIMARY KEY,
    TenSP NVARCHAR(150) NOT NULL,
    GiaBan DECIMAL(18,2) NOT NULL CHECK (GiaBan > 0),
    SoLuongTon INT NOT NULL CHECK (SoLuongTon >= 0)
);
GO

CREATE TABLE HoaDon
(
    MaHD INT IDENTITY(1,1) PRIMARY KEY,
    NgayLap DATETIME DEFAULT GETDATE(),
    MaNV INT NOT NULL,
    MaKH INT NULL,
    TongTien DECIMAL(18,2) NOT NULL,

    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV),
    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH)
);
GO

CREATE TABLE ChiTietHoaDon
(
    MaHD INT NOT NULL,
    MaSP VARCHAR(20) NOT NULL,
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    DonGia DECIMAL(18,2) NOT NULL CHECK (DonGia > 0),

    PRIMARY KEY (MaHD, MaSP),

    FOREIGN KEY (MaHD) REFERENCES HoaDon(MaHD),
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)
);
GO

-- Dữ liệu mẫu
INSERT INTO NhanVien(TenNV, SoDienThoai, ChucVu)
VALUES
(N'Nguyễn Văn An', '0912345678', N'Thu ngân'),
(N'Trần Văn Bình', '0987654321', N'Quản lý');

INSERT INTO KhachHang(TenKH, DienThoai)
VALUES
(N'Nguyễn Minh Anh', '0901111111'),
(N'Trần Thị Hoa', '0902222222');

INSERT INTO SanPham(MaSP, TenSP, GiaBan, SoLuongTon)
VALUES
('SP001', N'Sữa tươi Vinamilk', 10000, 100),
('SP002', N'Mì Hảo Hảo', 4500, 200),
('SP003', N'Nước suối Aquafina', 7000, 150),
('SP004', N'Bánh Oreo', 12000, 80),
('SP005', N'Coca Cola', 9000, 120);
GO
