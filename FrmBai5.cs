using Microsoft.Data.SqlClient;

namespace CuoiKyWinForms;

public partial class FrmBai5 : ExerciseFormBase
{
    public FrmBai5()
    {
        InitializeComponent();
        WireSubButtons();
        SelectSub(0, false);
    }

    protected override void ConfigureSub()
    {
        switch (CurrentSubIndex)
        {
            case 0:
                SetHeading("Bài 5a — Thông tin độc giả", "Kiểm tra độc giả là người lớn hay trẻ em.\r\nSQL Server: EXEC dbo.sp_ThongtinDocGia @MaDocGia;");
                SetParameters("Mã độc giả:", "");
                break;
            case 1:
                SetHeading("Bài 5b — Thông tin đầu sách", "Liệt kê đầu sách và số cuốn chưa mượn.\r\nSQL Server: EXEC dbo.sp_ThongtinDausach @ISBN;");
                SetParameters("Mã ISBN:", "");
                break;
            case 2:
                SetHeading("Bài 5c — Người lớn đang mượn sách", "Liệt kê người lớn đang mượn sách.\r\nSQL Server: EXEC dbo.sp_ThongtinNguoilonDangmuon;");
                SetParameters(null, null);
                break;
            case 3:
                SetHeading("Bài 5d — Người lớn mượn quá hạn", "Liệt kê người lớn mượn quá hạn trên 14 ngày.\r\nSQL Server: EXEC dbo.sp_ThongtinNguoilonQuahan;");
                SetParameters(null, null);
                break;
            case 4:
                SetHeading("Bài 5e — Người lớn và trẻ em cùng mượn", "Liệt kê người lớn và trẻ em được bảo lãnh khi cùng mượn sách.\r\nSQL Server: EXEC dbo.sp_DocGiaCoTreEmMuon;");
                SetParameters(null, null);
                break;
        }

        if (IsRequiredDatabaseAvailable) LoadSampleParameters();
    }

    protected override void OnDatabaseAvailabilityChanged(bool available)
    {
        if (available) LoadSampleParameters();
    }

    private void LoadSampleParameters()
    {
        string? sql = CurrentSubIndex switch
        {
            0 => "SELECT TOP (1) ma_DocGia FROM (SELECT ma_DocGia FROM dbo.Nguoilon UNION SELECT ma_DocGia FROM dbo.Treem) AS dg ORDER BY ma_DocGia;",
            1 => "SELECT TOP (1) isbn FROM dbo.Dausach ORDER BY isbn;",
            _ => null
        };
        if (sql == null) return;

        try
        {
            var sample = QueryTable(Db.LibraryDatabase, sql);
            if (sample.Rows.Count == 0 || sample.Rows[0][0] == DBNull.Value) return;
            SetParameters(CurrentSubIndex == 0 ? "Mã độc giả:" : "Mã ISBN:", sample.Rows[0][0].ToString());
        }
        catch (Exception ex) { SetError($"Không tải được tham số mẫu từ CSDL Thư viện: {ex.Message}"); }
    }

    protected override void ExecuteCurrent()
    {
        switch (CurrentSubIndex)
        {
            case 0:
                if (!int.TryParse(paramText1.Text, out int reader)) { SetError("Mã độc giả phải là số nguyên."); return; }
                RunTable(Db.LibraryDatabase, "EXEC dbo.sp_ThongtinDocGia @MaDocGia;",
                    new SqlParameter("@MaDocGia", System.Data.SqlDbType.Int) { Value = reader });
                break;
            case 1:
                RunTable(Db.LibraryDatabase, "EXEC dbo.sp_ThongtinDausach @ISBN;",
                    new SqlParameter("@ISBN", System.Data.SqlDbType.VarChar, 20) { Value = paramText1.Text.Trim() });
                break;
            case 2: RunTable(Db.LibraryDatabase, "EXEC dbo.sp_ThongtinNguoilonDangmuon;"); break;
            case 3: RunTable(Db.LibraryDatabase, "EXEC dbo.sp_ThongtinNguoilonQuahan;"); break;
            case 4: RunTable(Db.LibraryDatabase, "EXEC dbo.sp_DocGiaCoTreEmMuon;"); break;
        }
    }
}
