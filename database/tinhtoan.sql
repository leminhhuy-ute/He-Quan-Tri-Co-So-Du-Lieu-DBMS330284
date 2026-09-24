USE cuoiky_tinhtoan;
GO

-- BÀI 1: Giải phương trình bậc nhất ax + b = 0
CREATE OR ALTER PROCEDURE dbo.usp_GiaiPTBac1
    @a FLOAT,
    @b FLOAT
AS
BEGIN
    IF @a = 0 AND @b = 0
        SELECT N'Vô số nghiệm' AS KetQua, NULL AS NghiemX;
    ELSE IF @a = 0
        SELECT N'Vô nghiệm' AS KetQua, NULL AS NghiemX;
    ELSE
        SELECT N'Có một nghiệm' AS KetQua, -@b / @a AS NghiemX;
END;
GO

EXEC dbo.usp_GiaiPTBac1 @a = 2, @b = -6;
GO

-- BÀI 2: Giải phương trình bậc hai ax² + bx + c = 0
CREATE OR ALTER FUNCTION dbo.fn_GiaiPTBac2
(
    @a FLOAT,
    @b FLOAT,
    @c FLOAT
)
RETURNS TABLE
AS
RETURN
(
    SELECT
        CASE
            WHEN @a = 0 AND @b = 0 AND @c = 0 THEN N'Vô số nghiệm'
            WHEN @a = 0 AND @b = 0 THEN N'Vô nghiệm'
            WHEN @a = 0 THEN N'Phương trình bậc nhất'
            WHEN @b * @b - 4 * @a * @c < 0 THEN N'Vô nghiệm thực'
            WHEN @b * @b - 4 * @a * @c = 0 THEN N'Nghiệm kép'
            ELSE N'Hai nghiệm'
        END AS KetQua,
        CASE
            WHEN @a = 0 AND @b <> 0 THEN -@c / @b
            WHEN @a <> 0 AND @b * @b - 4 * @a * @c = 0 THEN -@b / (2 * @a)
            ELSE NULL
        END AS NghiemX,
        CASE
            WHEN @a <> 0 AND @b * @b - 4 * @a * @c > 0
                THEN (-@b + SQRT(@b * @b - 4 * @a * @c)) / (2 * @a)
            ELSE NULL
        END AS NghiemX1,
        CASE
            WHEN @a <> 0 AND @b * @b - 4 * @a * @c > 0
                THEN (-@b - SQRT(@b * @b - 4 * @a * @c)) / (2 * @a)
            ELSE NULL
        END AS NghiemX2
);
GO

SELECT * FROM dbo.fn_GiaiPTBac2(1, -3, 2);
GO

-- BÀI 4: Tính tuổi theo ngày sinh
CREATE OR ALTER FUNCTION dbo.fn_TinhTuoiTaiNgay
(
    @p_ngay_sinh DATE,
    @p_ngay_tinh_tuoi DATE
)
RETURNS INT
AS
BEGIN
    DECLARE @v_tuoi INT;

    IF @p_ngay_sinh IS NULL OR @p_ngay_tinh_tuoi IS NULL
       OR @p_ngay_sinh > @p_ngay_tinh_tuoi
        RETURN NULL;

    SET @v_tuoi = DATEDIFF(YEAR, @p_ngay_sinh, @p_ngay_tinh_tuoi);
    IF DATEADD(YEAR, @v_tuoi, @p_ngay_sinh) > @p_ngay_tinh_tuoi
        SET @v_tuoi = @v_tuoi - 1;

    RETURN @v_tuoi;
END;
GO

CREATE OR ALTER FUNCTION dbo.fn_TongSoThangTuNgay
(
    @p_ngay_sinh DATE,
    @p_ngay_tinh_tuoi DATE
)
RETURNS INT
AS
BEGIN
    DECLARE @v_so_thang INT;

    IF @p_ngay_sinh IS NULL OR @p_ngay_tinh_tuoi IS NULL
       OR @p_ngay_sinh > @p_ngay_tinh_tuoi
        RETURN NULL;

    SET @v_so_thang = DATEDIFF(MONTH, @p_ngay_sinh, @p_ngay_tinh_tuoi);
    IF DATEADD(MONTH, @v_so_thang, @p_ngay_sinh) > @p_ngay_tinh_tuoi
        SET @v_so_thang = @v_so_thang - 1;

    RETURN @v_so_thang;
END;
GO

CREATE OR ALTER FUNCTION dbo.fn_TongSoNgayTuNgay
(
    @p_ngay_sinh DATE,
    @p_ngay_tinh_tuoi DATE
)
RETURNS INT
AS
BEGIN
    IF @p_ngay_sinh IS NULL OR @p_ngay_tinh_tuoi IS NULL
       OR @p_ngay_sinh > @p_ngay_tinh_tuoi
        RETURN NULL;

    RETURN DATEDIFF(DAY, @p_ngay_sinh, @p_ngay_tinh_tuoi);
END;
GO

CREATE OR ALTER FUNCTION dbo.fn_TinhTuoiChiTiet(@NgaySinhText NVARCHAR(20))
RETURNS TABLE
AS
RETURN
(
    SELECT
        CASE WHEN v.Loi IS NULL
             THEN dbo.fn_TinhTuoiTaiNgay(d.NgaySinh, d.HomNay) END AS SoNam,
        CASE WHEN v.Loi IS NULL
             THEN dbo.fn_TongSoNgayTuNgay(d.NgaySinh, d.HomNay) END AS TongSoNgay,
        CASE WHEN v.Loi IS NULL
             THEN dbo.fn_TongSoThangTuNgay(d.NgaySinh, d.HomNay) END AS TongSoThang,
        v.Loi
    FROM
    (
        SELECT
            @NgaySinhText AS NgaySinhText,
            TRY_CONVERT(DATE, @NgaySinhText, 103) AS NgaySinh,
            CAST(GETDATE() AS DATE) AS HomNay
    ) AS d
    CROSS APPLY
    (
        SELECT CASE
            WHEN d.NgaySinhText IS NULL
                 OR LEN(d.NgaySinhText) <> 10
                 OR d.NgaySinhText NOT LIKE N'[0-3][0-9]/[0-1][0-9]/[0-9][0-9][0-9][0-9]'
                THEN N'Nhập ngày sinh theo định dạng dd/MM/yyyy, ví dụ 01/01/2002.'
            WHEN d.NgaySinh IS NULL
                 AND SUBSTRING(d.NgaySinhText, 4, 2) = N'02'
                 AND TRY_CONVERT(INT, LEFT(d.NgaySinhText, 2)) BETWEEN 1 AND 31
                 AND TRY_CONVERT(INT, RIGHT(d.NgaySinhText, 4)) BETWEEN 1 AND 9999
                THEN N'Tháng 2 của năm ' + RIGHT(d.NgaySinhText, 4)
                     + N' không có ngày ' + LEFT(d.NgaySinhText, 2) + N'.'
            WHEN d.NgaySinh IS NULL THEN N'Ngày sinh không hợp lệ.'
            WHEN d.NgaySinh > d.HomNay THEN N'Ngày sinh không được ở tương lai.'
            ELSE NULL
        END AS Loi
    ) AS v
);
GO

SELECT SoNam, TongSoNgay, TongSoThang, Loi
FROM dbo.fn_TinhTuoiChiTiet(N'01/01/2002');
GO
