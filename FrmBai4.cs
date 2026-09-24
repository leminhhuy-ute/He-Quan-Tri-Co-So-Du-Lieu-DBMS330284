using System.Data;
using Microsoft.Data.SqlClient;

namespace CuoiKyWinForms;

public partial class FrmBai4 : ExerciseFormBase
{
    public FrmBai4() => InitializeComponent();

    protected override void OnDatabaseAvailabilityChanged(bool available)
    {
        if (!available) return;
        try
        {
            DataTable sample = QueryTable(Db.CalculationDatabase,
                "SELECT CONVERT(NVARCHAR(10), CONVERT(DATE, '20020101', 112), 103) AS NgaySinh;");
            if (sample.Rows.Count > 0 && sample.Rows[0][0] != DBNull.Value)
                paramText1.Text = sample.Rows[0][0].ToString() ?? "";
        }
        catch (Exception ex) { SetError($"Không tải được ngày sinh mẫu từ CSDL Tính toán: {ex.Message}"); }
    }

    protected override void ExecuteCurrent()
    {
        try
        {
            DataTable data = QueryTable(Db.CalculationDatabase,
                "SELECT SoNam, TongSoNgay, TongSoThang, Loi FROM dbo.fn_TinhTuoiChiTiet(@NgaySinhText);",
                new SqlParameter("@NgaySinhText", SqlDbType.NVarChar, 20) { Value = paramText1.Text.Trim() });

            if (data.Rows.Count == 0)
            {
                SetError("Hàm SQL không trả về kết quả.");
                return;
            }

            if (data.Rows[0]["Loi"] != DBNull.Value)
            {
                SetError(data.Rows[0]["Loi"].ToString() ?? "Ngày sinh không hợp lệ.");
                return;
            }

            data.Columns.Remove("Loi");
            resultGrid.DataSource = data;
            SetSuccess("Đã đọc kết quả tính tuổi từ hàm SQL Server.");
        }
        catch (Exception ex) { SetError(ex.Message); }
    }
}
