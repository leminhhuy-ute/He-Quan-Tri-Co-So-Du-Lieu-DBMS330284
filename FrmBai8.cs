namespace CuoiKyWinForms;

public partial class FrmBai8 : ExerciseFormBase
{
    public FrmBai8()
    {
        InitializeComponent();
        WireSubButtons();
        SelectSub(0, false);
    }

    protected override void ConfigureSub()
    {
        SetParameters(null, null);
        switch (CurrentSubIndex)
        {
            case 0: SetHeading("Bài 8.1 — Dự án có hơn 2 nhân viên", "Trả về mã, tên dự án và số nhân viên tham gia.\r\nSQL Server: SELECT * FROM dbo.fn_Bai8_1_DuAnNhieuHonHaiNhanVien();"); break;
            case 1: SetHeading("Bài 8.2 — Phòng nhiều nhân viên lương cao", "Phòng có hơn 2 nhân viên, đếm người lương trên 25000.\r\nSQL Server: SELECT * FROM dbo.fn_Bai8_2_PhongNhieuHonHaiNhanVienLuongCao();"); break;
            case 2: SetHeading("Bài 8.3 — Phòng lương trung bình trên 30000", "Trả về mã, tên phòng và số nhân viên.\r\nSQL Server: SELECT * FROM dbo.fn_Bai8_3_PhongLuongTBCao();"); break;
            case 3: SetHeading("Bài 8.4 — Phòng lương cao và nhân viên nam", "Đếm nhân viên nam của phòng lương trung bình trên 30000.\r\nSQL Server: SELECT * FROM dbo.fn_Bai8_4_PhongLuongTBCaoNam();"); break;
            case 4: SetHeading("Bài 8.5 — Nhân viên phòng 5 theo dự án", "Với mỗi dự án, đếm nhân viên phòng 5 tham gia.\r\nSQL Server: SELECT * FROM dbo.fn_Bai8_5_DuAnVaNhanVienPhong5();"); break;
        }
    }

    protected override void ExecuteCurrent()
    {
        switch (CurrentSubIndex)
        {
            case 0: RunTable(Db.ProjectDatabase, "SELECT * FROM dbo.fn_Bai8_1_DuAnNhieuHonHaiNhanVien() ORDER BY MaDA;"); break;
            case 1: RunTable(Db.ProjectDatabase, "SELECT * FROM dbo.fn_Bai8_2_PhongNhieuHonHaiNhanVienLuongCao() ORDER BY MaPB;"); break;
            case 2: RunTable(Db.ProjectDatabase, "SELECT * FROM dbo.fn_Bai8_3_PhongLuongTBCao() ORDER BY MaPB;"); break;
            case 3: RunTable(Db.ProjectDatabase, "SELECT * FROM dbo.fn_Bai8_4_PhongLuongTBCaoNam() ORDER BY MaPB;"); break;
            case 4: RunTable(Db.ProjectDatabase, "SELECT * FROM dbo.fn_Bai8_5_DuAnVaNhanVienPhong5() ORDER BY MaDA;"); break;
        }
    }
}
