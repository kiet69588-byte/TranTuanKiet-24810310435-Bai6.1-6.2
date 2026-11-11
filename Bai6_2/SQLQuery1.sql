
USE master;
GO

IF DB_ID('HRMDB') IS NULL
BEGIN
    CREATE DATABASE HRMDB;
END
GO

USE HRMDB;
GO

IF OBJECT_ID('dbo.BangLuong', 'U') IS NOT NULL
    DROP TABLE dbo.BangLuong;
GO

IF OBJECT_ID('dbo.BangChamCong', 'U') IS NOT NULL
    DROP TABLE dbo.BangChamCong;
GO

IF OBJECT_ID('dbo.NhanVien', 'U') IS NOT NULL
    DROP TABLE dbo.NhanVien;
GO

IF OBJECT_ID('dbo.PhongBan', 'U') IS NOT NULL
    DROP TABLE dbo.PhongBan;
GO


CREATE TABLE PhongBan
(
    MaPB INT IDENTITY(1,1) PRIMARY KEY,
    TenPB NVARCHAR(100) NOT NULL,
    Mota NVARCHAR(255)
);
GO

CREATE TABLE NhanVien
(
    MaNV INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    NgaySinh DATETIME NULL,
    CCCD VARCHAR(20) NOT NULL UNIQUE,
    MaPB INT NOT NULL,
    LuongCoBan DECIMAL(18,2) NOT NULL,
    HeSoLuong FLOAT NOT NULL DEFAULT 1.0,

    CONSTRAINT FK_NhanVien_PhongBan
    FOREIGN KEY (MaPB)
    REFERENCES PhongBan(MaPB),

    CONSTRAINT CK_NhanVien_Luong
    CHECK (LuongCoBan > 0),

    CONSTRAINT CK_NhanVien_HeSoLuong
    CHECK (HeSoLuong > 0)
);
GO


CREATE TABLE BangChamCong
(
    MaCong INT IDENTITY(1,1) PRIMARY KEY,
    MaNV INT NOT NULL,
    ThangNam VARCHAR(7) NOT NULL,
    SoNgayCong FLOAT NOT NULL DEFAULT 0,
    SoGioOT FLOAT NOT NULL DEFAULT 0,

    DaChot BIT NOT NULL DEFAULT 0,

    CONSTRAINT FK_BangChamCong_NhanVien
    FOREIGN KEY (MaNV)
    REFERENCES NhanVien(MaNV),

    CONSTRAINT UQ_BangChamCong
    UNIQUE (MaNV, ThangNam),

    CONSTRAINT CK_BangChamCong_SoNgayCong
    CHECK (SoNgayCong >= 0 AND SoNgayCong <= 31),

    CONSTRAINT CK_BangChamCong_SoGioOT
    CHECK (SoGioOT >= 0)
);
GO


CREATE TABLE BangLuong
(
    MaLuong INT IDENTITY(1,1) PRIMARY KEY,
    MaNV INT NOT NULL,
    ThangNam VARCHAR(7) NOT NULL,
    ThucLinh DECIMAL(18,2) NOT NULL,

    CONSTRAINT FK_BangLuong_NhanVien
    FOREIGN KEY (MaNV)
    REFERENCES NhanVien(MaNV),

    CONSTRAINT UQ_BangLuong
    UNIQUE (MaNV, ThangNam)
);
GO


INSERT INTO PhongBan(TenPB, Mota)
VALUES
(N'Phòng Kế toán', N'Quản lý tài chính và kế toán'),
(N'Phòng Nhân sự', N'Quản lý hồ sơ nhân viên'),
(N'Phòng Kỹ thuật', N'Quản lý kỹ thuật'),
(N'Phòng Kinh doanh', N'Quản lý kinh doanh');
GO

INSERT INTO NhanVien
(
    HoTen,
    NgaySinh,
    CCCD,
    MaPB,
    LuongCoBan,
    HeSoLuong
)
VALUES
(N'Nguyễn Văn An', '2002-01-15', '001202000001', 1, 8000000, 1.5),
(N'Trần Thị Bình', '2001-05-20', '001202000002', 2, 7500000, 1.3),
(N'Lê Văn Cường', '2000-08-10', '001202000003', 3, 9000000, 1.8),
(N'Phạm Thị Dung', '2002-12-25', '001202000004', 4, 7000000, 1.2),
(N'Hoàng Văn Đức', '2001-03-18', '001202000005', 1, 8500000, 1.4);
GO


SELECT * FROM PhongBan;
GO

SELECT * FROM NhanVien;
GO

SELECT * FROM BangChamCong;
GO

SELECT * FROM BangLuong;
GO

SELECT
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'BangChamCong';
GO
