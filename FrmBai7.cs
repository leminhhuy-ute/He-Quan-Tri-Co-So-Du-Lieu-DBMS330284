using Microsoft.Data.SqlClient;

namespace CuoiKyWinForms;

public partial class FrmBai7 : ExerciseFormBase
{
    public FrmBai7()
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
                SetHeading("Bài 7.1 — Lương trung bình một phòng ban", "Tính lương trung bình của phòng ban được nhập.\r\nSQL Server: SELECT dbo.fn_LuongTBPhongBan(@MaPB);");
                SetParameters("Mã phòng ban:", ""); break;
            case 1:
                SetHeading("Bài 7.2 — Tổng lương nhân viên theo dự án", "Tổng lương theo nhân viên và dự án.\r\nSQL Server: SELECT dbo.fn_TongLuongNhanVienTheoDuAn(@MaNV, @MaDA);");
                SetParameters("Mã nhân viên:", "", "Mã dự án:", ""); break;
            case 2:
                SetHeading("Bài 7.3 — Lương trung bình các phòng ban", "Tính mức lương trung bình của tất cả phòng ban.\r\nSQL Server: SELECT dbo.fn_LuongTBAllPhongBan();");
                SetParameters(null, null); break;
            case 3:
                SetHeading("Bài 7.4 — Tiền thưởng theo số giờ", "Tính thưởng theo tổng số giờ tham gia.\r\nSQL Server: SELECT dbo.fn_TienThuongTheoGio(@TimeTotal);");
                SetParameters("Tổng giờ:", ""); break;
            case 4:
                SetHeading("Bài 7.5 — Số dự án theo từng phòng", "Trả về số dự án của từng phòng ban.\r\nSQL Server: SELECT MaPB, TenPB, dbo.fn_TongDuAnTheoPhongBan(MaPB) FROM dbo.PHONGBAN;");
                SetParameters(null, null); break;
            case 5:
                SetHeading("Bài 7.6a — Bảng nhân viên Inline TVF", "Trả về thông tin nhân viên và người thân bằng Inline TVF.\r\nSQL Server: SELECT * FROM dbo.fn_Bai7_6_Inline();");
                SetParameters(null, null); break;
            case 6:
                SetHeading("Bài 7.6b — Bảng nhân viên Multi TVF", "Cùng nội dung 7.6a bằng Multistatement TVF.\r\nSQL Server: SELECT * FROM dbo.fn_Bai7_6_Multi();");
                SetParameters(null, null); break;
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
            0 => "SELECT TOP (1) MaPB FROM dbo.PHONGBAN ORDER BY MaPB;",
            1 => "SELECT TOP (1) nv.MaNV, pc.MaDA FROM dbo.PHANCONG AS pc INNER JOIN dbo.NHANVIEN AS nv ON nv.MaNV = pc.MaNV ORDER BY nv.MaNV, pc.MaDA;",
            3 => "SELECT CAST(75 AS DECIMAL(10,2)) AS TimeTotal;",
            _ => null
        };
        if (sql == null) return;

        try
        {
            var sample = QueryTable(Db.ProjectDatabase, sql);
            if (sample.Rows.Count == 0 || sample.Rows[0][0] == DBNull.Value) return;

            if (CurrentSubIndex == 1 && sample.Rows[0][1] != DBNull.Value)
                SetParameters("Mã nhân viên:", sample.Rows[0][0].ToString(), "Mã dự án:", sample.Rows[0][1].ToString());
            else
                SetParameters(CurrentSubIndex == 0 ? "Mã phòng ban:" : "Tổng giờ:", sample.Rows[0][0].ToString());
        }
        catch (Exception ex) { SetError($"Không tải được tham số mẫu từ CSDL Đề án: {ex.Message}"); }
    }

    protected override void ExecuteCurrent()
    {
        switch (CurrentSubIndex)
        {
            case 0:
                RunScalar(Db.ProjectDatabase, "SELECT dbo.fn_LuongTBPhongBan(@MaPB);",
                    new SqlParameter("@MaPB", System.Data.SqlDbType.VarChar, 10) { Value = paramText1.Text.Trim() }); break;
            case 1:
                RunScalar(Db.ProjectDatabase, "SELECT dbo.fn_TongLuongNhanVienTheoDuAn(@MaNV, @MaDA);",
                    new SqlParameter("@MaNV", System.Data.SqlDbType.VarChar, 10) { Value = paramText1.Text.Trim() },
                    new SqlParameter("@MaDA", System.Data.SqlDbType.VarChar, 10) { Value = paramText2.Text.Trim() }); break;
            case 2: RunScalar(Db.ProjectDatabase, "SELECT dbo.fn_LuongTBAllPhongBan();"); break;
            case 3:
                if (!decimal.TryParse(paramText1.Text, out decimal hours)) { SetError("Tổng số giờ phải là số."); return; }
                RunScalar(Db.ProjectDatabase, "SELECT dbo.fn_TienThuongTheoGio(@TimeTotal);",
                    new SqlParameter("@TimeTotal", System.Data.SqlDbType.Decimal) { Value = hours }); break;
            case 4: RunTable(Db.ProjectDatabase, "SELECT MaPB, TenPB, dbo.fn_TongDuAnTheoPhongBan(MaPB) AS TongDuAn FROM dbo.PHONGBAN ORDER BY MaPB;"); break;
            case 5: RunTable(Db.ProjectDatabase, "SELECT * FROM dbo.fn_Bai7_6_Inline() ORDER BY MaNV;"); break;
            case 6: RunTable(Db.ProjectDatabase, "SELECT * FROM dbo.fn_Bai7_6_Multi() ORDER BY MaNV;"); break;
        }
    }
}
