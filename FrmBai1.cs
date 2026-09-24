using System.Data;
using Microsoft.Data.SqlClient;

namespace CuoiKyWinForms;

public partial class FrmBai1 : ExerciseFormBase
{
    public FrmBai1() => InitializeComponent();

    protected override void OnDatabaseAvailabilityChanged(bool available)
    {
        if (!available) return;

        try
        {
            DataTable sample = QueryTable(Db.CalculationDatabase,
                "SELECT CAST(2 AS FLOAT) AS HeSoA, CAST(-6 AS FLOAT) AS HeSoB;");
            if (sample.Rows.Count == 0) return;

            paramText1.Text = Convert.ToString(sample.Rows[0]["HeSoA"], System.Globalization.CultureInfo.InvariantCulture) ?? "";
            paramText2.Text = Convert.ToString(sample.Rows[0]["HeSoB"], System.Globalization.CultureInfo.InvariantCulture) ?? "";
        }
        catch (Exception ex)
        {
            SetError($"Không tải được hệ số mẫu từ CSDL Tính toán: {ex.Message}");
        }
    }

    protected override void ExecuteCurrent()
    {
        if (!double.TryParse(paramText1.Text, out double a) || !double.TryParse(paramText2.Text, out double b))
        {
            SetError("Hệ số a và b phải là số.");
            return;
        }
        RunTable(Db.CalculationDatabase, "EXEC dbo.usp_GiaiPTBac1 @a, @b;",
            new SqlParameter("@a", System.Data.SqlDbType.Float) { Value = a },
            new SqlParameter("@b", System.Data.SqlDbType.Float) { Value = b });
    }
}
