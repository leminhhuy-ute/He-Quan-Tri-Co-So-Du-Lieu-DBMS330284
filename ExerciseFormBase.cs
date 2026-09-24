using System.ComponentModel;
using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;

namespace CuoiKyWinForms;

public partial class ExerciseFormBase : Form
{
    protected int CurrentSubIndex { get; private set; }
    protected bool IsDesignPreview => LicenseManager.UsageMode == LicenseUsageMode.Designtime;
    private bool databaseAvailable;
    private string requiredDatabaseName = "CSDL tương ứng";

    public ExerciseFormBase()
    {
        InitializeComponent();
    }

    public void Open()
    {
        if (IsDesignPreview) return;
        if (!databaseAvailable) ShowDatabaseUnavailable();
        else ExecuteCurrent();
    }

    public void SetDatabaseAvailability(bool available, string databaseName)
    {
        databaseAvailable = available;
        requiredDatabaseName = databaseName;
        if (executeButton != null) executeButton.Enabled = available;
        OnDatabaseAvailabilityChanged(available);
    }

    protected virtual void OnDatabaseAvailabilityChanged(bool available) { }

    protected bool IsRequiredDatabaseAvailable => databaseAvailable;

    private void ShowDatabaseUnavailable()
    {
        resultGrid.DataSource = null;
        resultText.ForeColor = Color.DarkOrange;
        resultText.Text = $"Chưa kết nối/nạp {requiredDatabaseName}. Quay lại menu chính, kết nối và nạp CSDL này trước khi thực hiện.";
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        if (!IsDesignPreview) Open();
    }

    protected virtual void ExecuteCurrent() { }
    protected virtual void ConfigureSub() { }

    protected void SelectSub(int index, bool execute = true)
    {
        CurrentSubIndex = index;
        foreach (Control control in subButtons.Controls)
        {
            if (control is not Button button || button.Tag is not int buttonIndex) continue;
            bool selected = buttonIndex == index;
            button.BackColor = selected ? Color.FromArgb(29, 63, 111) : Color.White;
            button.ForeColor = selected ? Color.White : Color.FromArgb(30, 40, 55);
        }
        ConfigureSub();
        if (execute && !IsDesignPreview)
        {
            if (!databaseAvailable) ShowDatabaseUnavailable();
            else ExecuteCurrent();
        }
    }

    protected void WireSubButtons()
    {
        foreach (Control control in subButtons.Controls)
        {
            if (control is Button button && button.Tag is int)
                button.Click += (_, _) => SelectSub((int)button.Tag);
        }
    }

    protected void SetHeading(string title, string description)
    {
        titleLabel.Text = title;
        descriptionBox.Text = description;
    }

    protected void SetParameters(string? name1, string? value1, string? name2 = null, string? value2 = null,
        string? name3 = null, string? value3 = null)
    {
        SetParameter(paramLabel1, paramText1, name1, value1);
        SetParameter(paramLabel2, paramText2, name2, value2);
        SetParameter(paramLabel3, paramText3, name3, value3);
    }

    private static void SetParameter(Label label, TextBox box, string? name, string? value)
    {
        label.Visible = box.Visible = !string.IsNullOrEmpty(name);
        if (name != null) label.Text = name;
        if (value != null) box.Text = value;
    }

    protected void SetError(string message)
    {
        resultGrid.DataSource = null;
        resultText.ForeColor = Color.Firebrick;
        resultText.Text = "LỖI: " + message;
    }

    protected void SetSuccess(string message)
    {
        resultText.ForeColor = Color.DarkGreen;
        resultText.Text = message;
    }

    protected void RunTable(string database, string sql, params SqlParameter[] parameters)
    {
        try
        {
            var data = QueryTable(database, sql, parameters);
            resultGrid.DataSource = data;
            SetSuccess($"Thành công! Trả về {data.Rows.Count} dòng dữ liệu.");
        }
        catch (Exception ex) { SetError(ex.Message); }
    }

    protected void RunScalar(string database, string sql, params SqlParameter[] parameters)
    {
        resultGrid.DataSource = null;
        try { SetSuccess(QueryScalar(database, sql, parameters)?.ToString() ?? "(NULL / Không có giá trị)"); }
        catch (Exception ex) { SetError(ex.Message); }
    }

    protected static DataTable QueryTable(string database, string sql, params SqlParameter[] parameters)
    {
        using var connection = Db.CreateConnection(database);
        using var command = new SqlCommand(sql, connection);
        AddParameters(command, parameters);
        Db.OpenConnection(connection);
        using var adapter = new SqlDataAdapter(command);
        var table = new DataTable();
        adapter.Fill(table);
        return NormalizeTableText(table);
    }

    protected static DataTable QueryTable(SqlConnection connection, SqlTransaction transaction, string sql)
    {
        using var command = new SqlCommand(sql, connection, transaction);
        using var adapter = new SqlDataAdapter(command);
        var table = new DataTable();
        adapter.Fill(table);
        return NormalizeTableText(table);
    }

    protected static DataTable NormalizeTableText(DataTable table)
    {
        foreach (DataColumn column in table.Columns)
        {
            if (column.DataType != typeof(string) || !string.IsNullOrEmpty(column.Expression))
                continue;

            if (column.ReadOnly) column.ReadOnly = false;
            foreach (DataRow row in table.Rows)
            {
                if (row.IsNull(column)) continue;
                var value = (string)row[column];
                var normalized = NormalizeLegacyUtf8(value);
                if (!string.Equals(value, normalized, StringComparison.Ordinal))
                    row[column] = normalized;
            }
        }
        return table;
    }

    protected static string NormalizeLegacyUtf8(string value)
    {
        if (!value.Contains('Ã') && !value.Contains('Ä') && !value.Contains("á»", StringComparison.Ordinal))
            return value;

        try
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var decoded = Encoding.UTF8.GetString(Encoding.GetEncoding(1252).GetBytes(value));
            return decoded.Contains('\uFFFD') ? value : decoded;
        }
        catch
        {
            return value;
        }
    }

    private static object? QueryScalar(string database, string sql, SqlParameter[] parameters)
    {
        using var connection = Db.CreateConnection(database);
        using var command = new SqlCommand(sql, connection);
        AddParameters(command, parameters);
        Db.OpenConnection(connection);
        return command.ExecuteScalar();
    }

    private static void AddParameters(SqlCommand command, SqlParameter[] parameters)
    {
        foreach (var parameter in parameters)
        {
            command.Parameters.Add(new SqlParameter(parameter.ParameterName, parameter.SqlDbType)
            {
                Size = parameter.Size,
                Precision = parameter.Precision,
                Scale = parameter.Scale,
                Value = parameter.Value
            });
        }
    }

    private void executeButton_Click(object? sender, EventArgs e) => ExecuteCurrent();

    private void backButton_Click(object? sender, EventArgs e) => Close();
}
