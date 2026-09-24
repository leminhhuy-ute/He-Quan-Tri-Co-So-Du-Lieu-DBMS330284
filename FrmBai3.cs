using System.Data;
using Microsoft.Data.SqlClient;

namespace CuoiKyWinForms;

public partial class FrmBai3 : ExerciseFormBase
{
    public FrmBai3() => InitializeComponent();

    protected override void OnDatabaseAvailabilityChanged(bool available)
    {
        if (!available) return;
        try
        {
            DataTable sample = QueryTable(Db.LibraryDatabase,
                "SELECT TOP (1) isbn FROM dbo.Dausach ORDER BY isbn;");
            if (sample.Rows.Count > 0 && sample.Rows[0][0] != DBNull.Value)
                paramText1.Text = sample.Rows[0][0].ToString() ?? "";
        }
        catch (Exception ex) { SetError($"Không tải được ISBN mẫu từ CSDL Thư viện: {ex.Message}"); }
    }

    protected override void ExecuteCurrent() => RunTable(Db.LibraryDatabase,
        "EXEC dbo.sp_ThongtinDausach @ISBN;",
        new SqlParameter("@ISBN", System.Data.SqlDbType.VarChar, 20) { Value = paramText1.Text.Trim() });
}
