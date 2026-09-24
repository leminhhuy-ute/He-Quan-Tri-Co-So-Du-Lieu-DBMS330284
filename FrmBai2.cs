using System.Data;
using System.Globalization;
using Microsoft.Data.SqlClient;

namespace CuoiKyWinForms;

public partial class FrmBai2 : ExerciseFormBase
{
    private bool defaultsLoaded;

    public FrmBai2() => InitializeComponent();

    protected override void OnDatabaseAvailabilityChanged(bool available)
    {
        if (!available) return;
        LoadDefaultsFromSql();
    }

    protected override void ExecuteCurrent()
    {
        try
        {
            if (!defaultsLoaded)
            {
                LoadDefaultsFromSql();
            }

            if (!TryParseCoefficient(paramText1.Text, out double a)
                || !TryParseCoefficient(paramText2.Text, out double b)
                || !TryParseCoefficient(paramText3.Text, out double c))
            {
                SetError("Nhập hệ số a, b và c bằng số hợp lệ.");
                return;
            }

            RunTable(Db.CalculationDatabase,
                "SELECT * FROM dbo.fn_GiaiPTBac2(@a, @b, @c);",
                new SqlParameter("@a", SqlDbType.Float) { Value = a },
                new SqlParameter("@b", SqlDbType.Float) { Value = b },
                new SqlParameter("@c", SqlDbType.Float) { Value = c });
        }
        catch (Exception ex)
        {
            SetError($"Không chạy được Bài 2. Hãy kiểm tra kết nối và nạp CSDL Tính toán. Chi tiết: {ex.Message}");
        }
    }

    private void LoadDefaultsFromSql()
    {
        try
        {
            DataTable defaults = QueryTable(Db.CalculationDatabase,
                "SELECT CAST(1 AS FLOAT) AS HeSoA, CAST(-3 AS FLOAT) AS HeSoB, CAST(2 AS FLOAT) AS HeSoC;");
            if (defaults.Rows.Count == 0) return;

            DataRow row = defaults.Rows[0];
            if (string.IsNullOrWhiteSpace(paramText1.Text))
                paramText1.Text = Convert.ToString(row["HeSoA"], CultureInfo.InvariantCulture) ?? "";
            if (string.IsNullOrWhiteSpace(paramText2.Text))
                paramText2.Text = Convert.ToString(row["HeSoB"], CultureInfo.InvariantCulture) ?? "";
            if (string.IsNullOrWhiteSpace(paramText3.Text))
                paramText3.Text = Convert.ToString(row["HeSoC"], CultureInfo.InvariantCulture) ?? "";
            defaultsLoaded = true;
        }
        catch (Exception ex)
        {
            SetError($"Không tải được hệ số mẫu từ CSDL Tính toán: {ex.Message}");
        }
    }

    private static bool TryParseCoefficient(string text, out double value)
    {
        bool parsed = double.TryParse(text, NumberStyles.Float, CultureInfo.CurrentCulture, out value)
            || double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
        return parsed && double.IsFinite(value);
    }
}
