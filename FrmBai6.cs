using System.Data;
using Microsoft.Data.SqlClient;

namespace CuoiKyWinForms;

public partial class FrmBai6 : ExerciseFormBase
{
    public FrmBai6()
    {
        InitializeComponent();
        WireSubButtons();
        SelectSub(0, false);
    }

    protected override void OnDatabaseAvailabilityChanged(bool available)
    {
        primaryButton.Enabled = available;
        secondButton.Enabled = available;
        reloadButton.Enabled = available;
        if (available) LoadSampleParameters();
    }

    protected override void ConfigureSub()
    {
        resultGrid.DataSource = null;
        resultText.Text = "Bấm Thực hiện để tải bảng, hoặc dùng nút thao tác Bài 6.";
        statusBox.Visible = false;
        secondButton.Visible = CurrentSubIndex == 3;
        switch (CurrentSubIndex)
        {
            case 0:
                SetHeading("Bài 6.1 — Trigger tg_delMuon", "SQL chạy thử xóa lượt mượn trong transaction rồi rollback; trigger chuyển cuốn sách thành yes, dữ liệu gốc được giữ nguyên.");
                SetField(1, "Mã ISBN:", ""); SetField(2, "Mã cuốn sách:", "");
                SetField(3, null, null); SetField(4, null, null);
                primaryButton.Text = "Xóa lượt mượn"; break;
            case 1:
                SetHeading("Bài 6.2 — Trigger tg_insMuon", "Chạy thử thêm lượt mượn; trigger chuyển cuốn sách thành no. Bảng xem trước phản ánh thay đổi, dữ liệu CSDL thật được giữ nguyên.");
                SetField(1, "Mã ISBN:", ""); SetField(2, "Mã cuốn sách:", "");
                SetField(3, null, null); SetField(4, null, null);
                primaryButton.Text = "Thêm lượt mượn"; break;
            case 2:
                SetHeading("Bài 6.3 — Trigger tg_updCuonSach", "SQL chạy thử cập nhật tình trạng cuốn sách trong transaction rồi rollback; trigger đồng bộ đầu sách, dữ liệu gốc được giữ nguyên.");
                SetField(1, "Mã ISBN:", ""); SetField(2, "Mã cuốn sách:", "");
                SetField(3, null, null); SetField(4, "Tình trạng:", null);
                field4.Visible = false; statusBox.Visible = true; statusBox.SelectedIndex = -1;
                primaryButton.Text = "Cập nhật tình trạng"; break;
            case 3:
                SetHeading("Bài 6.4 — Trigger tg_InfThongBao", "Thử thêm hoặc sửa tựa sách để kiểm tra thông báo của trigger; dữ liệu CSDL được giữ nguyên.");
                SetField(1, "Mã tựa sách:", ""); SetField(2, "Tên tựa sách:", "");
                SetField(3, "Tên tác giả:", ""); SetField(4, "Tóm tắt:", "");
                primaryButton.Text = "Thêm tựa sách"; break;
        }

        if (IsRequiredDatabaseAvailable) LoadSampleParameters();
    }

    private void LoadSampleParameters()
    {
        string? sql = CurrentSubIndex switch
        {
            0 => "SELECT TOP (1) isbn, ma_cuonsach FROM dbo.Muon ORDER BY isbn, ma_cuonsach;",
            1 => "SELECT TOP (1) isbn, ma_cuonsach FROM dbo.Cuonsach WHERE tinhtrang = 'yes' ORDER BY isbn, ma_cuonsach;",
            2 => "SELECT TOP (1) isbn, ma_cuonsach, tinhtrang FROM dbo.Cuonsach ORDER BY isbn, ma_cuonsach;",
            3 => "SELECT CONVERT(NVARCHAR(30), ISNULL(MAX(ma_tuasach), 0) + 1) AS MaTuaSach, N'Tựa sách kiểm thử trigger' AS TenTuaSach, N'Tác giả kiểm thử' AS TenTacGia, N'Tóm tắt dùng để kiểm tra trigger' AS TomTat FROM dbo.Tuasach;",
            _ => null
        };
        if (sql == null) return;

        try
        {
            var sample = QueryTable(Db.LibraryDatabase, sql);
            if (sample.Rows.Count == 0) return;
            var row = sample.Rows[0];
            if (row[0] == DBNull.Value) return;

            field1.Text = row[0].ToString() ?? "";
            if (CurrentSubIndex is 0 or 1 or 2 && row[1] != DBNull.Value)
                field2.Text = row[1].ToString() ?? "";
            if (CurrentSubIndex == 2 && row[2] != DBNull.Value)
                statusBox.SelectedItem = string.Equals(row[2].ToString(), "yes", StringComparison.OrdinalIgnoreCase) ? "no" : "yes";
            if (CurrentSubIndex == 3)
            {
                field2.Text = row["TenTuaSach"].ToString() ?? "";
                field3.Text = row["TenTacGia"].ToString() ?? "";
                field4.Text = row["TomTat"].ToString() ?? "";
            }
        }
        catch (Exception ex) { SetError($"Không tải được dữ liệu mẫu từ CSDL Thư viện: {ex.Message}"); }
    }

    private void SetField(int number, string? caption, string? value)
    {
        var label = number switch { 1 => fieldLabel1, 2 => fieldLabel2, 3 => fieldLabel3, _ => fieldLabel4 };
        var box = number switch { 1 => field1, 2 => field2, 3 => field3, _ => field4 };
        if (number == 2)
        {
            label.Size = CurrentSubIndex == 3 ? new Size(190, 40) : new Size(150, 40);
            box.Location = CurrentSubIndex == 3 ? new Point(590, 23) : new Point(550, 23);
        }
        label.Visible = box.Visible = caption != null;
        if (caption != null) label.Text = caption;
        if (value != null) box.Text = value;
    }

    protected override void ExecuteCurrent() => LoadTable();

    private string TableSql => CurrentSubIndex switch
    {
        0 or 1 => "SELECT isbn, ma_cuonsach, tinhtrang FROM dbo.Cuonsach ORDER BY isbn, ma_cuonsach;",
        2 => "SELECT cs.isbn, cs.ma_cuonsach, cs.tinhtrang, ds.trangthai FROM dbo.Cuonsach AS cs INNER JOIN dbo.Dausach AS ds ON ds.isbn = cs.isbn ORDER BY cs.isbn, cs.ma_cuonsach;",
        _ => "SELECT ma_tuasach, tuasach, tacgia, tomtat FROM dbo.Tuasach ORDER BY ma_tuasach;"
    };

    private void LoadTable()
    {
        try
        {
            var table = QueryTable(Db.LibraryDatabase, TableSql);
            resultGrid.DataSource = table;
            SetSuccess($"Đã tải bảng Bài 6 ({table.Rows.Count} dòng).");
        }
        catch (Exception ex) { SetError(ex.Message); }
    }

    private void reloadButton_Click(object? sender, EventArgs e) => LoadTable();

    private void primaryButton_Click(object? sender, EventArgs e)
    {
        switch (CurrentSubIndex)
        {
            case 0:
                if (MessageBox.Show(this, $"Xóa lượt mượn của {field1.Text.Trim()} - cuốn {field2.Text.Trim()}?",
                    "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
                Mutate("dbo.sp_Bai6_XoaMuon", "Đã xóa lượt mượn.",
                    new SqlParameter("@ISBN", SqlDbType.VarChar, 20) { Value = field1.Text.Trim() },
                    new SqlParameter("@MaCuonText", SqlDbType.NVarChar, 30) { Value = field2.Text.Trim() }); break;
            case 1:
                Mutate("dbo.sp_Bai6_ThemMuon",
                    "Đã thêm lượt mượn.",
                    new SqlParameter("@ISBN", SqlDbType.VarChar, 20) { Value = field1.Text.Trim() },
                    new SqlParameter("@MaCuonText", SqlDbType.NVarChar, 30) { Value = field2.Text.Trim() }); break;
            case 2:
                Mutate("dbo.sp_Bai6_CapNhatCuonSach",
                    "Đã cập nhật tình trạng sách.",
                    new SqlParameter("@ISBN", SqlDbType.VarChar, 20) { Value = field1.Text.Trim() },
                    new SqlParameter("@MaCuonText", SqlDbType.NVarChar, 30) { Value = field2.Text.Trim() },
                    new SqlParameter("@TinhTrang", SqlDbType.VarChar, 3) { Value = statusBox.SelectedItem?.ToString() ?? "" }); break;
            case 3:
                Mutate("dbo.sp_Bai6_ThemTuaSach_KiemThu",
                    "Đã thêm tựa sách.",
                    new SqlParameter("@MaTuaSachText", SqlDbType.NVarChar, 30) { Value = field1.Text.Trim() },
                    new SqlParameter("@TenTuaSach", SqlDbType.NVarChar, 200) { Value = field2.Text.Trim() },
                    new SqlParameter("@TenTacGia", SqlDbType.NVarChar, 100) { Value = field3.Text.Trim() },
                    new SqlParameter("@TomTat", SqlDbType.NVarChar, 1000) { Value = field4.Text.Trim() }); break;
        }
    }

    private void secondButton_Click(object? sender, EventArgs e)
    {
        if (CurrentSubIndex != 3) return;
        Mutate("dbo.sp_Bai6_SuaTuaSach",
            "Đã sửa tựa sách.",
            new SqlParameter("@MaTuaSachText", SqlDbType.NVarChar, 30) { Value = field1.Text.Trim() },
            new SqlParameter("@TenTuaSach", SqlDbType.NVarChar, 200) { Value = field2.Text.Trim() },
            new SqlParameter("@TenTacGia", SqlDbType.NVarChar, 100) { Value = field3.Text.Trim() });
    }

    private void Mutate(string procedure, string success, params SqlParameter[] parameters)
    {
        try
        {
            using var connection = Db.CreateConnection(Db.LibraryDatabase);
            Db.OpenConnection(connection);
            int changed;
            DataTable table;
            using (var command = new SqlCommand(procedure, connection)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                command.Parameters.AddRange(parameters);
                using var reader = command.ExecuteReader();
                if (!reader.Read())
                    throw new InvalidOperationException("Thủ tục Bài 6 không trả trạng thái thao tác.");
                changed = Convert.ToInt32(reader["SoDongThayDoi"]);
                if (!reader.NextResult())
                    throw new InvalidOperationException("Thủ tục Bài 6 không trả bảng xem trước.");
                table = new DataTable();
                table.Load(reader);
                NormalizeTableText(table);
            }

            // Each SQL demo procedure rolls its test transaction back before returning.
            resultGrid.DataSource = table;
            if (changed == 0)
            {
                resultText.ForeColor = Color.DarkOrange;
                resultText.Text = CurrentSubIndex switch
                {
                    0 => "Không tìm thấy lượt mượn.",
                    1 => "Không thể thêm lượt mượn.",
                    2 => "Không tìm thấy cuốn sách.",
                    _ => "Không tìm thấy tựa sách."
                };
            }
            else SetSuccess(success);
        }
        catch (Exception ex) { SetError(ex.Message); }
    }
}
