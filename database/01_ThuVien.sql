USE cuoiky_thuvien;
GO

-- BÀI 3 VÀ 5B: Thông tin đầu sách theo ISBN
CREATE OR ALTER PROCEDURE dbo.sp_ThongtinDausach
    @ISBN VARCHAR(20)
AS
BEGIN
    SELECT ds.isbn, ds.ma_tuasach, ts.tuasach, ts.tacgia, ts.tomtat,
           ds.ngonngu, ds.bia, ds.trangthai,
           (
               SELECT COUNT(*)
               FROM dbo.Cuonsach cs
               WHERE cs.isbn = ds.isbn AND cs.tinhtrang = 'yes'
           ) AS SoLuongChuaMuon
    FROM dbo.Dausach ds
    INNER JOIN dbo.Tuasach ts ON ts.ma_tuasach = ds.ma_tuasach
    WHERE ds.isbn = @ISBN;
END;
GO

EXEC dbo.sp_ThongtinDausach @ISBN = 'ISBN001';
GO

-- BÀI 5A: Thông tin độc giả người lớn hoặc trẻ em
CREATE OR ALTER PROCEDURE dbo.sp_ThongtinDocGia
    @MaDocGia INT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM dbo.Nguoilon WHERE ma_DocGia = @MaDocGia)
    BEGIN
        SELECT dg.ma_DocGia, dg.ho, dg.tenlot, dg.ten, dg.ngaysinh,
               nl.sonha, nl.duong, nl.quan, nl.dienthoai, nl.han_sd,
               N'Người lớn' AS LoaiDocGia
        FROM dbo.DocGia dg
        INNER JOIN dbo.Nguoilon nl ON nl.ma_DocGia = dg.ma_DocGia
        WHERE dg.ma_DocGia = @MaDocGia;
    END
    ELSE IF EXISTS (SELECT 1 FROM dbo.Treem WHERE ma_DocGia = @MaDocGia)
    BEGIN
        SELECT dg.ma_DocGia, dg.ho, dg.tenlot, dg.ten, dg.ngaysinh,
               te.ma_DocGia_nguoilon,
               N'Trẻ em' AS LoaiDocGia
        FROM dbo.DocGia dg
        INNER JOIN dbo.Treem te ON te.ma_DocGia = dg.ma_DocGia
        WHERE dg.ma_DocGia = @MaDocGia;
    END
    ELSE
    BEGIN
        RAISERROR(N'Mã độc giả không tồn tại hoặc chưa được phân loại.', 16, 1);
        RETURN;
    END;
END;
GO

IF EXISTS (SELECT 1 FROM dbo.Nguoilon WHERE ma_DocGia = 1)
    OR EXISTS (SELECT 1 FROM dbo.Treem WHERE ma_DocGia = 1)
    EXEC dbo.sp_ThongtinDocGia @MaDocGia = 1;
GO

-- BÀI 5C: Danh sách độc giả người lớn đang mượn sách
CREATE OR ALTER PROCEDURE dbo.sp_ThongtinNguoilonDangmuon
AS
BEGIN
    SELECT dg.ma_DocGia, dg.ho, dg.tenlot, dg.ten, dg.ngaysinh,
           nl.sonha, nl.duong, nl.quan, nl.dienthoai, nl.han_sd,
           m.isbn, m.ma_cuonsach, m.ngay_muon, m.ngay_hethan, ts.tuasach
    FROM dbo.Nguoilon nl
    INNER JOIN dbo.DocGia dg ON dg.ma_DocGia = nl.ma_DocGia
    INNER JOIN dbo.Muon m ON m.ma_DocGia = nl.ma_DocGia
    INNER JOIN dbo.Dausach ds ON ds.isbn = m.isbn
    INNER JOIN dbo.Tuasach ts ON ts.ma_tuasach = ds.ma_tuasach
    ORDER BY dg.ma_DocGia, m.ngay_muon;
END;
GO

EXEC dbo.sp_ThongtinNguoilonDangmuon;
GO

-- BÀI 5D: Độc giả người lớn mượn quá hạn trên 14 ngày
CREATE OR ALTER PROCEDURE dbo.sp_ThongtinNguoilonQuahan
AS
BEGIN
    SELECT dg.ma_DocGia, dg.ho, dg.tenlot, dg.ten, dg.ngaysinh,
           nl.sonha, nl.duong, nl.quan, nl.dienthoai, nl.han_sd,
           m.isbn, m.ma_cuonsach, m.ngay_muon, m.ngay_hethan,
           DATEDIFF(DAY, m.ngay_hethan, GETDATE()) AS SoNgayQuaHan,
           ts.tuasach
    FROM dbo.Nguoilon nl
    INNER JOIN dbo.DocGia dg ON dg.ma_DocGia = nl.ma_DocGia
    INNER JOIN dbo.Muon m ON m.ma_DocGia = nl.ma_DocGia
    INNER JOIN dbo.Dausach ds ON ds.isbn = m.isbn
    INNER JOIN dbo.Tuasach ts ON ts.ma_tuasach = ds.ma_tuasach
    WHERE DATEDIFF(DAY, m.ngay_hethan, GETDATE()) > 14
    ORDER BY dg.ma_DocGia, m.ngay_hethan;
END;
GO

EXEC dbo.sp_ThongtinNguoilonQuahan;
GO

-- BÀI 5E: Người lớn và trẻ em được bảo lãnh cùng đang mượn
CREATE OR ALTER PROCEDURE dbo.sp_DocGiaCoTreEmMuon
AS
BEGIN
    SELECT DISTINCT
           nl.ma_DocGia AS MaNguoiLon,
           dgNL.ho AS HoNguoiLon,
           dgNL.tenlot AS TenLotNguoiLon,
           dgNL.ten AS TenNguoiLon,
           te.ma_DocGia AS MaTreEm,
           dgTE.ho AS HoTreEm,
           dgTE.tenlot AS TenLotTreEm,
           dgTE.ten AS TenTreEm,
           mNL.isbn AS ISBNNguoiLon,
           mNL.ma_cuonsach AS CuonNguoiLon,
           mTE.isbn AS ISBNTreEm,
           mTE.ma_cuonsach AS CuonTreEm,
           mNL.ngay_muon AS NgayMuonNguoiLon,
           mTE.ngay_muon AS NgayMuonTreEm
    FROM dbo.Nguoilon nl
    INNER JOIN dbo.DocGia dgNL ON dgNL.ma_DocGia = nl.ma_DocGia
    INNER JOIN dbo.Muon mNL ON mNL.ma_DocGia = nl.ma_DocGia
    INNER JOIN dbo.Treem te ON te.ma_DocGia_nguoilon = nl.ma_DocGia
    INNER JOIN dbo.DocGia dgTE ON dgTE.ma_DocGia = te.ma_DocGia
    INNER JOIN dbo.Muon mTE ON mTE.ma_DocGia = te.ma_DocGia
    ORDER BY nl.ma_DocGia, te.ma_DocGia;
END;
GO

EXEC dbo.sp_DocGiaCoTreEmMuon;
GO

-- BÀI 6.1: Xóa lượt mượn và cập nhật tình trạng cuốn sách
CREATE OR ALTER TRIGGER dbo.tg_delMuon
ON dbo.Muon
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE cs
    SET tinhtrang = 'yes'
    FROM dbo.Cuonsach cs
    INNER JOIN deleted d
        ON d.isbn = cs.isbn AND d.ma_cuonsach = cs.ma_cuonsach;
END;
GO

-- BÀI 6.2: Thêm lượt mượn và cập nhật tình trạng cuốn sách
CREATE OR ALTER TRIGGER dbo.tg_insMuon
ON dbo.Muon
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE cs
    SET tinhtrang = 'no'
    FROM dbo.Cuonsach cs
    INNER JOIN inserted i
        ON i.isbn = cs.isbn AND i.ma_cuonsach = cs.ma_cuonsach;
END;
GO

-- BÀI 6.3: Cập nhật trạng thái đầu sách
CREATE OR ALTER TRIGGER dbo.tg_updCuonSach
ON dbo.Cuonsach
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF UPDATE(tinhtrang)
    BEGIN
        UPDATE ds
        SET trangthai = CASE
            WHEN EXISTS
            (
                SELECT 1
                FROM dbo.Cuonsach cs
                WHERE cs.isbn = ds.isbn AND cs.tinhtrang = 'yes'
            ) THEN 'yes'
            ELSE 'no'
        END
        FROM dbo.Dausach ds
        INNER JOIN
        (
            SELECT isbn FROM inserted
            UNION
            SELECT isbn FROM deleted
        ) AS sach ON sach.isbn = ds.isbn;
    END;
END;
GO

-- BÀI 6.4: Thêm/sửa tựa sách và tác giả
CREATE OR ALTER TRIGGER dbo.tg_InfThongBao
ON dbo.Tuasach
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM inserted)
       AND
       (
           NOT EXISTS (SELECT 1 FROM deleted)
           OR UPDATE(tuasach)
           OR UPDATE(tacgia)
       )
    BEGIN
        PRINT N'Đã thêm mới tựa sách';
    END;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Bai6_XoaMuon
    @ISBN VARCHAR(20),
    @MaCuonText NVARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    IF NULLIF(LTRIM(RTRIM(@ISBN)), '') IS NULL
        THROW 50001, N'Nhập mã ISBN.', 1;
    IF TRY_CONVERT(INT, @MaCuonText) IS NULL
        THROW 50002, N'Mã cuốn sách phải là số nguyên.', 1;

    DECLARE @SoDongThayDoi INT;
    DECLARE @CuonSachThu TABLE
    (
        isbn VARCHAR(20) NOT NULL,
        ma_cuonsach INT NOT NULL,
        tinhtrang VARCHAR(3) NOT NULL,
        PRIMARY KEY (isbn, ma_cuonsach)
    );
    BEGIN TRY
        BEGIN TRANSACTION;
        DELETE FROM dbo.Muon WHERE isbn = @ISBN AND ma_cuonsach = TRY_CONVERT(INT, @MaCuonText);
        SET @SoDongThayDoi = @@ROWCOUNT;
        IF @SoDongThayDoi > 0
            INSERT INTO @CuonSachThu (isbn, ma_cuonsach, tinhtrang)
            SELECT cs.isbn, cs.ma_cuonsach, cs.tinhtrang
            FROM dbo.Cuonsach AS cs
            WHERE cs.isbn = @ISBN AND cs.ma_cuonsach = TRY_CONVERT(INT, @MaCuonText);
        ROLLBACK TRANSACTION;
        SELECT @SoDongThayDoi AS SoDongThayDoi;
        SELECT ketqua.isbn, ketqua.ma_cuonsach, ketqua.tinhtrang
        FROM
        (
            SELECT cs.isbn, cs.ma_cuonsach, cs.tinhtrang
            FROM dbo.Cuonsach AS cs
            WHERE NOT EXISTS
            (
                SELECT 1 FROM @CuonSachThu AS thu
                WHERE thu.isbn = cs.isbn AND thu.ma_cuonsach = cs.ma_cuonsach
            )
            UNION ALL
            SELECT thu.isbn, thu.ma_cuonsach, thu.tinhtrang
            FROM @CuonSachThu AS thu
        ) AS ketqua
        ORDER BY ketqua.isbn, ketqua.ma_cuonsach;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Bai6_ThemMuon
    @ISBN VARCHAR(20),
    @MaCuonText NVARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    IF NULLIF(LTRIM(RTRIM(@ISBN)), '') IS NULL
        THROW 50003, N'Nhập mã ISBN.', 1;
    IF TRY_CONVERT(INT, @MaCuonText) IS NULL
        THROW 50004, N'Mã cuốn sách phải là số nguyên.', 1;
    IF NOT EXISTS (SELECT 1 FROM dbo.DocGia)
        THROW 50005, N'CSDL chưa có độc giả để tạo lượt mượn.', 1;
    IF NOT EXISTS (SELECT 1 FROM dbo.Cuonsach WHERE isbn = @ISBN AND ma_cuonsach = TRY_CONVERT(INT, @MaCuonText) AND tinhtrang = 'yes')
        THROW 50006, N'Không tìm thấy cuốn sách đang sẵn sàng để mượn.', 1;
    IF EXISTS (SELECT 1 FROM dbo.Muon WHERE isbn = @ISBN AND ma_cuonsach = TRY_CONVERT(INT, @MaCuonText))
        THROW 50007, N'Cuốn sách này đã có lượt mượn.', 1;

    DECLARE @SoDongThayDoi INT;
    DECLARE @CuonSachThu TABLE
    (
        isbn VARCHAR(20) NOT NULL,
        ma_cuonsach INT NOT NULL,
        tinhtrang VARCHAR(3) NOT NULL,
        PRIMARY KEY (isbn, ma_cuonsach)
    );
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO dbo.Muon (isbn, ma_cuonsach, ma_DocGia, ngay_muon, ngay_hethan)
        SELECT @ISBN, TRY_CONVERT(INT, @MaCuonText), MIN(dg.ma_DocGia), GETDATE(), DATEADD(DAY, 14, GETDATE())
        FROM dbo.DocGia AS dg;
        SET @SoDongThayDoi = @@ROWCOUNT;
        IF @SoDongThayDoi > 0
            INSERT INTO @CuonSachThu (isbn, ma_cuonsach, tinhtrang)
            SELECT cs.isbn, cs.ma_cuonsach, cs.tinhtrang
            FROM dbo.Cuonsach AS cs
            WHERE cs.isbn = @ISBN AND cs.ma_cuonsach = TRY_CONVERT(INT, @MaCuonText);
        ROLLBACK TRANSACTION;
        SELECT @SoDongThayDoi AS SoDongThayDoi;
        SELECT ketqua.isbn, ketqua.ma_cuonsach, ketqua.tinhtrang
        FROM
        (
            SELECT cs.isbn, cs.ma_cuonsach, cs.tinhtrang
            FROM dbo.Cuonsach AS cs
            WHERE NOT EXISTS
            (
                SELECT 1 FROM @CuonSachThu AS thu
                WHERE thu.isbn = cs.isbn AND thu.ma_cuonsach = cs.ma_cuonsach
            )
            UNION ALL
            SELECT thu.isbn, thu.ma_cuonsach, thu.tinhtrang
            FROM @CuonSachThu AS thu
        ) AS ketqua
        ORDER BY ketqua.isbn, ketqua.ma_cuonsach;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Bai6_CapNhatCuonSach
    @ISBN VARCHAR(20),
    @MaCuonText NVARCHAR(30),
    @TinhTrang VARCHAR(3)
AS
BEGIN
    SET NOCOUNT ON;
    IF NULLIF(LTRIM(RTRIM(@ISBN)), '') IS NULL OR TRY_CONVERT(INT, @MaCuonText) IS NULL
        THROW 50008, N'Nhập mã ISBN và mã cuốn sách hợp lệ.', 1;
    IF @TinhTrang NOT IN ('yes', 'no')
        THROW 50009, N'Tình trạng phải là yes hoặc no.', 1;

    DECLARE @SoDongThayDoi INT;
    DECLARE @CuonSachThu TABLE
    (
        isbn VARCHAR(20) NOT NULL,
        ma_cuonsach INT NOT NULL,
        tinhtrang VARCHAR(3) NOT NULL,
        trangthai VARCHAR(3) NOT NULL,
        PRIMARY KEY (isbn, ma_cuonsach)
    );
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE dbo.Cuonsach SET tinhtrang = @TinhTrang
        WHERE isbn = @ISBN AND ma_cuonsach = TRY_CONVERT(INT, @MaCuonText);
        SET @SoDongThayDoi = @@ROWCOUNT;
        IF @SoDongThayDoi > 0
            INSERT INTO @CuonSachThu (isbn, ma_cuonsach, tinhtrang, trangthai)
            SELECT cs.isbn, cs.ma_cuonsach, cs.tinhtrang, ds.trangthai
            FROM dbo.Cuonsach AS cs
            INNER JOIN dbo.Dausach AS ds ON ds.isbn = cs.isbn
            WHERE cs.isbn = @ISBN AND cs.ma_cuonsach = TRY_CONVERT(INT, @MaCuonText);
        ROLLBACK TRANSACTION;
        SELECT @SoDongThayDoi AS SoDongThayDoi;
        SELECT ketqua.isbn, ketqua.ma_cuonsach, ketqua.tinhtrang, ketqua.trangthai
        FROM
        (
            SELECT cs.isbn, cs.ma_cuonsach, cs.tinhtrang, ds.trangthai
            FROM dbo.Cuonsach AS cs
            INNER JOIN dbo.Dausach AS ds ON ds.isbn = cs.isbn
            WHERE NOT EXISTS
            (
                SELECT 1 FROM @CuonSachThu AS thu
                WHERE thu.isbn = cs.isbn AND thu.ma_cuonsach = cs.ma_cuonsach
            )
            UNION ALL
            SELECT thu.isbn, thu.ma_cuonsach, thu.tinhtrang, thu.trangthai
            FROM @CuonSachThu AS thu
        ) AS ketqua
        ORDER BY ketqua.isbn, ketqua.ma_cuonsach;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Bai6_ThemTuaSach_KiemThu
    @MaTuaSachText NVARCHAR(30),
    @TenTuaSach NVARCHAR(200),
    @TenTacGia NVARCHAR(100),
    @TomTat NVARCHAR(1000)
AS
BEGIN
    SET NOCOUNT ON;
    IF TRY_CONVERT(INT, @MaTuaSachText) IS NULL OR NULLIF(LTRIM(RTRIM(@TenTuaSach)), N'') IS NULL
       OR NULLIF(LTRIM(RTRIM(@TenTacGia)), N'') IS NULL OR NULLIF(LTRIM(RTRIM(@TomTat)), N'') IS NULL
        THROW 50012, N'Nhập mã tựa sách dạng số, tên tựa sách, tên tác giả và tóm tắt.', 1;
    IF EXISTS (SELECT 1 FROM dbo.Tuasach WHERE ma_tuasach = TRY_CONVERT(INT, @MaTuaSachText))
        THROW 50013, N'Mã tựa sách đã tồn tại.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.Tuasach)
        THROW 50014, N'Cần có dữ liệu tựa sách để kiểm thử an toàn.', 1;

    DECLARE @IdentityTruoc BIGINT = CONVERT(BIGINT, IDENT_CURRENT(N'dbo.Tuasach'));
    DECLARE @SoDongThayDoi INT = 0;
    DECLARE @IdentityInsertOn BIT = 0;
    DECLARE @TuaSachThu TABLE
    (
        ma_tuasach INT NOT NULL PRIMARY KEY,
        tuasach NVARCHAR(MAX),
        tacgia NVARCHAR(MAX),
        tomtat NVARCHAR(MAX)
    );
    BEGIN TRY
        BEGIN TRANSACTION;
        SET IDENTITY_INSERT dbo.Tuasach ON;
        SET @IdentityInsertOn = 1;
        INSERT INTO dbo.Tuasach (ma_tuasach, tuasach, tacgia, tomtat)
        OUTPUT inserted.ma_tuasach, inserted.tuasach, inserted.tacgia, inserted.tomtat
            INTO @TuaSachThu (ma_tuasach, tuasach, tacgia, tomtat)
        VALUES (TRY_CONVERT(INT, @MaTuaSachText), @TenTuaSach, @TenTacGia, @TomTat);
        SET @SoDongThayDoi = @@ROWCOUNT;
        SET IDENTITY_INSERT dbo.Tuasach OFF;
        SET @IdentityInsertOn = 0;

        ROLLBACK TRANSACTION;

        IF CONVERT(BIGINT, IDENT_CURRENT(N'dbo.Tuasach')) <> @IdentityTruoc
        BEGIN
            DECLARE @LenhReseed NVARCHAR(400) =
                N'DBCC CHECKIDENT (''dbo.Tuasach'', RESEED, '
                + CONVERT(VARCHAR(30), @IdentityTruoc) + N') WITH NO_INFOMSGS;';
            EXEC sys.sp_executesql @LenhReseed;
        END;

        SELECT @SoDongThayDoi AS SoDongThayDoi;
        SELECT ketqua.ma_tuasach, ketqua.tuasach, ketqua.tacgia, ketqua.tomtat
        FROM
        (
            SELECT ts.ma_tuasach, ts.tuasach, ts.tacgia, ts.tomtat
            FROM dbo.Tuasach AS ts
            WHERE NOT EXISTS
            (
                SELECT 1 FROM @TuaSachThu AS thu
                WHERE thu.ma_tuasach = ts.ma_tuasach
            )
            UNION ALL
            SELECT thu.ma_tuasach, thu.tuasach, thu.tacgia, thu.tomtat
            FROM @TuaSachThu AS thu
        ) AS ketqua
        ORDER BY ketqua.ma_tuasach;
    END TRY
    BEGIN CATCH
        IF @IdentityInsertOn = 1
        BEGIN TRY
            SET IDENTITY_INSERT dbo.Tuasach OFF;
        END TRY
        BEGIN CATCH
        END CATCH;
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Bai6_SuaTuaSach
    @MaTuaSachText NVARCHAR(30),
    @TenTuaSach NVARCHAR(200),
    @TenTacGia NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    IF TRY_CONVERT(INT, @MaTuaSachText) IS NULL OR NULLIF(LTRIM(RTRIM(@TenTuaSach)), N'') IS NULL
       OR NULLIF(LTRIM(RTRIM(@TenTacGia)), N'') IS NULL
        THROW 50011, N'Nhập mã tựa sách, tên tựa sách và tên tác giả.', 1;

    DECLARE @SoDongThayDoi INT;
    DECLARE @TuaSachThu TABLE
    (
        ma_tuasach INT NOT NULL PRIMARY KEY,
        tuasach NVARCHAR(MAX),
        tacgia NVARCHAR(MAX),
        tomtat NVARCHAR(MAX)
    );
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE dbo.Tuasach SET tuasach = @TenTuaSach, tacgia = @TenTacGia
        OUTPUT inserted.ma_tuasach, inserted.tuasach, inserted.tacgia, inserted.tomtat
            INTO @TuaSachThu (ma_tuasach, tuasach, tacgia, tomtat)
        WHERE ma_tuasach = TRY_CONVERT(INT, @MaTuaSachText);
        SET @SoDongThayDoi = @@ROWCOUNT;
        ROLLBACK TRANSACTION;
        SELECT @SoDongThayDoi AS SoDongThayDoi;
        SELECT ketqua.ma_tuasach, ketqua.tuasach, ketqua.tacgia, ketqua.tomtat
        FROM
        (
            SELECT ts.ma_tuasach, ts.tuasach, ts.tacgia, ts.tomtat
            FROM dbo.Tuasach AS ts
            WHERE NOT EXISTS
            (
                SELECT 1 FROM @TuaSachThu AS thu
                WHERE thu.ma_tuasach = ts.ma_tuasach
            )
            UNION ALL
            SELECT thu.ma_tuasach, thu.tuasach, thu.tacgia, thu.tomtat
            FROM @TuaSachThu AS thu
        ) AS ketqua
        ORDER BY ketqua.ma_tuasach;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO
