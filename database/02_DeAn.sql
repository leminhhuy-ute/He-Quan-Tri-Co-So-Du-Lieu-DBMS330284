USE cuoiky_dean;
GO

-- BÀI 7.1: Lương trung bình của một phòng ban
CREATE OR ALTER FUNCTION dbo.fn_LuongTBPhongBan(@MaPB VARCHAR(10))
RETURNS DECIMAL(18,2)
AS
BEGIN
    RETURN ISNULL((SELECT AVG(Luong) FROM dbo.NHANVIEN WHERE MaPB = @MaPB), 0);
END;
GO

-- BÀI 7.2: Tổng lương nhân viên theo dự án
CREATE OR ALTER FUNCTION dbo.fn_TongLuongNhanVienTheoDuAn
(
    @MaNV VARCHAR(10),
    @MaDA VARCHAR(10)
)
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @Luong DECIMAL(18,2);
    DECLARE @GioDuAn DECIMAL(18,2);
    DECLARE @TongGio DECIMAL(18,2);

    SELECT @Luong = MAX(nv.Luong),
           @GioDuAn = SUM(pc.Time_Total)
    FROM dbo.NHANVIEN AS nv
    INNER JOIN dbo.PHANCONG AS pc ON pc.MaNV = nv.MaNV
    WHERE pc.MaNV = @MaNV AND pc.MaDA = @MaDA
    GROUP BY nv.MaNV;

    SELECT @TongGio = SUM(pc.Time_Total)
    FROM dbo.PHANCONG AS pc
    WHERE pc.MaNV = @MaNV;

    IF @Luong IS NULL OR @GioDuAn IS NULL OR ISNULL(@TongGio, 0) <= 0
        RETURN 0;

    RETURN ISNULL(
        @Luong * @GioDuAn / @TongGio,
        0
    );
END;
GO

-- BÀI 7.3: Tổng lương trung bình của các phòng ban
CREATE OR ALTER FUNCTION dbo.fn_LuongTBAllPhongBan()
RETURNS DECIMAL(18,2)
AS
BEGIN
    RETURN ISNULL(
        (
            SELECT SUM(pb.LuongTB)
            FROM
            (
                SELECT MaPB, AVG(Luong) AS LuongTB
                FROM dbo.NHANVIEN
                WHERE MaPB IS NOT NULL
                GROUP BY MaPB
            ) pb
        ),
        0
    );
END;
GO

-- BÀI 7.4: Tiền thưởng theo tổng số giờ
CREATE OR ALTER FUNCTION dbo.fn_TienThuongTheoGio(@TimeTotal DECIMAL(10,2))
RETURNS DECIMAL(18,2)
AS
BEGIN
    RETURN CASE
        WHEN @TimeTotal >= 30 AND @TimeTotal <= 60 THEN 500
        WHEN @TimeTotal > 60 AND @TimeTotal < 100 THEN 1000
        WHEN @TimeTotal >= 100 AND @TimeTotal < 150 THEN 1200
        WHEN @TimeTotal >= 150 THEN 1600
        ELSE 0
    END;
END;
GO

-- BÀI 7.5: Tổng số dự án theo từng phòng ban
CREATE OR ALTER FUNCTION dbo.fn_TongDuAnTheoPhongBan(@MaPB VARCHAR(10))
RETURNS INT
AS
BEGIN
    RETURN (SELECT COUNT(*) FROM dbo.DuAn WHERE MaPB = @MaPB);
END;
GO

-- BÀI 7.6A: Thông tin nhân viên bằng inline table valued function
CREATE OR ALTER FUNCTION dbo.fn_Bai7_6_Inline()
RETURNS TABLE
AS
RETURN
(
    SELECT nv.MaNV,
           CONCAT_WS(N' ', nv.Ho, nv.TenLot, nv.Ten) AS HoTen,
           nv.NgaySinh,
           STRING_AGG(CONVERT(NVARCHAR(MAX), nt.TenNguoiThan), N', ') AS NguoiThan,
           dbo.fn_LuongTBPhongBan(nv.MaPB) AS TongLuongTB
    FROM dbo.NHANVIEN nv
    LEFT JOIN dbo.NguoiThan nt ON nt.MaNV = nv.MaNV
    GROUP BY nv.MaNV, nv.Ho, nv.TenLot, nv.Ten, nv.NgaySinh, nv.MaPB
);
GO

-- BÀI 7.6B: Thông tin nhân viên bằng multistatement table valued function
CREATE OR ALTER FUNCTION dbo.fn_Bai7_6_Multi()
RETURNS @KetQua TABLE
(
    MaNV VARCHAR(10),
    HoTen NVARCHAR(152),
    NgaySinh DATE,
    NguoiThan NVARCHAR(MAX),
    TongLuongTB DECIMAL(18,2)
)
AS
BEGIN
    INSERT INTO @KetQua
    SELECT nv.MaNV,
           CONCAT_WS(N' ', nv.Ho, nv.TenLot, nv.Ten),
           nv.NgaySinh,
           STRING_AGG(CONVERT(NVARCHAR(MAX), nt.TenNguoiThan), N', '),
           dbo.fn_LuongTBPhongBan(nv.MaPB)
    FROM dbo.NHANVIEN nv
    LEFT JOIN dbo.NguoiThan nt ON nt.MaNV = nv.MaNV
    GROUP BY nv.MaNV, nv.Ho, nv.TenLot, nv.Ten, nv.NgaySinh, nv.MaPB;

    RETURN;
END;
GO

-- BÀI 8.1: Dự án có nhiều hơn hai nhân viên tham gia
CREATE OR ALTER FUNCTION dbo.fn_Bai8_1_DuAnNhieuHonHaiNhanVien()
RETURNS TABLE
AS
RETURN
(
    SELECT da.MaDA, da.TenDA, COUNT(DISTINCT pc.MaNV) AS SoLuongNhanVien
    FROM dbo.DuAn da
    LEFT JOIN dbo.PHANCONG pc ON pc.MaDA = da.MaDA
    GROUP BY da.MaDA, da.TenDA
    HAVING COUNT(DISTINCT pc.MaNV) > 2
);
GO

-- BÀI 8.2: Phòng có hơn hai nhân viên và đếm người lương trên 25000
CREATE OR ALTER FUNCTION dbo.fn_Bai8_2_PhongNhieuHonHaiNhanVienLuongCao()
RETURNS TABLE
AS
RETURN
(
    SELECT MaPB,
           SUM(CASE WHEN Luong > 25000 THEN 1 ELSE 0 END) AS SoNhanVienLuongLonHon25000
    FROM dbo.NHANVIEN
    WHERE MaPB IS NOT NULL
    GROUP BY MaPB
    HAVING COUNT(*) > 2
);
GO

-- BÀI 8.3: Phòng có lương trung bình trên 30000
CREATE OR ALTER FUNCTION dbo.fn_Bai8_3_PhongLuongTBCao()
RETURNS TABLE
AS
RETURN
(
    SELECT pb.MaPB, pb.TenPB, COUNT(nv.MaNV) AS SoLuongNhanVien
    FROM dbo.PHONGBAN pb
    INNER JOIN dbo.NHANVIEN nv ON nv.MaPB = pb.MaPB
    GROUP BY pb.MaPB, pb.TenPB
    HAVING AVG(nv.Luong) > 30000
);
GO

-- BÀI 8.4: Phòng lương trung bình trên 30000 và số nhân viên nam
CREATE OR ALTER FUNCTION dbo.fn_Bai8_4_PhongLuongTBCaoNam()
RETURNS TABLE
AS
RETURN
(
    SELECT pb.MaPB, pb.TenPB,
           SUM(CASE WHEN nv.Phai = 'Nam' THEN 1 ELSE 0 END) AS SoLuongNhanVienNam
    FROM dbo.PHONGBAN pb
    INNER JOIN dbo.NHANVIEN nv ON nv.MaPB = pb.MaPB
    GROUP BY pb.MaPB, pb.TenPB
    HAVING AVG(nv.Luong) > 30000
);
GO

-- BÀI 8.5: Số nhân viên phòng 5 tham gia mỗi dự án
CREATE OR ALTER FUNCTION dbo.fn_Bai8_5_DuAnVaNhanVienPhong5()
RETURNS TABLE
AS
RETURN
(
    SELECT da.MaDA, da.TenDA,
           COUNT(DISTINCT CASE WHEN nv.MaPB = 'P005' THEN nv.MaNV END) AS SoNhanVienPhong5
    FROM dbo.DuAn da
    LEFT JOIN dbo.PHANCONG pc ON pc.MaDA = da.MaDA
    LEFT JOIN dbo.NHANVIEN nv ON nv.MaNV = pc.MaNV
    GROUP BY da.MaDA, da.TenDA
);
GO

SELECT dbo.fn_LuongTBPhongBan('P001') AS LuongTBPhong;
SELECT dbo.fn_TongLuongNhanVienTheoDuAn('NV0000001', 'DA01') AS TongLuong;
SELECT dbo.fn_LuongTBAllPhongBan() AS LuongTBAll;
SELECT dbo.fn_TienThuongTheoGio(75) AS TienThuong;
SELECT dbo.fn_TongDuAnTheoPhongBan('P001') AS TongDuAn;
SELECT * FROM dbo.fn_Bai7_6_Inline();
SELECT * FROM dbo.fn_Bai7_6_Multi();
SELECT * FROM dbo.fn_Bai8_1_DuAnNhieuHonHaiNhanVien();
SELECT * FROM dbo.fn_Bai8_2_PhongNhieuHonHaiNhanVienLuongCao();
SELECT * FROM dbo.fn_Bai8_3_PhongLuongTBCao();
SELECT * FROM dbo.fn_Bai8_4_PhongLuongTBCaoNam();
SELECT * FROM dbo.fn_Bai8_5_DuAnVaNhanVienPhong5();
GO
