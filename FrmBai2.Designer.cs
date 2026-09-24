namespace CuoiKyWinForms;

public partial class FrmBai2
{
    private void InitializeComponent()
    {
        Name = "FrmBai2";
        parameterPanel.Height = 142;
        Text = "Bài 2 — Bài tập lớn SQL DBMS";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(1140, 760);
        MinimumSize = new Size(1000, 640);
        titleLabel.Text = "Bài 2 — Phương trình bậc 2 (ax² + bx + c = 0)";
        descriptionBox.Text = "Nhập hệ số a, b, c rồi chọn Thực hiện. Hệ số mẫu từ CSDL chỉ tự điền vào ô còn trống; số bạn đã nhập được giữ nguyên.\r\nSQL Server: SELECT * FROM dbo.fn_GiaiPTBac2(@a, @b, @c);";
        paramLabel1.Text = "Hệ số a:";
        paramLabel2.Text = "Hệ số b:";
        paramLabel3.Text = "Hệ số c:";
        executeButton.Text = "Thực hiện";
        subButtons.Visible = false;
    }
}
