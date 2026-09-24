namespace CuoiKyWinForms;

public partial class FrmBai1
{
    private void InitializeComponent()
    {
        Name = "FrmBai1";
        Text = "Bài 1 — Bài tập lớn SQL DBMS";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(1140, 760);
        MinimumSize = new Size(1000, 640);
        titleLabel.Text = "Bài 1 — Phương trình bậc 1 (ax + b = 0)";
        descriptionBox.Text = "Giải phương trình ax + b = 0 với a, b bất kỳ.\r\nSQL Server: EXEC dbo.usp_GiaiPTBac1 @a, @b;";
        paramLabel1.Text = "Hệ số a:";
        paramText1.Clear();
        paramLabel2.Text = "Hệ số b:";
        paramText2.Clear();
        paramLabel3.Visible = false;
        paramText3.Visible = false;
        subButtons.Visible = false;
    }
}
