namespace CuoiKyWinForms;

public partial class FrmBai5
{
    private Button btn5a = null!;
    private Button btn5b = null!;
    private Button btn5c = null!;
    private Button btn5d = null!;
    private Button btn5e = null!;

    private void InitializeComponent()
    {
        btn5a = new Button();
        btn5b = new Button();
        btn5c = new Button();
        btn5d = new Button();
        btn5e = new Button();
        Name = "FrmBai5";
        Text = "Bài 5 — Bài tập lớn SQL DBMS";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(1140, 760);
        MinimumSize = new Size(1000, 640);
        titleLabel.Text = "Bài 5 — Quản lý thư viện";
        parameterPanel.Height = 90;
        btn5a.Name = "btn5a"; btn5a.Text = "5a. Thông tin độc giả"; btn5a.Tag = 0;
        btn5a.Size = new Size(225, 50); btn5a.Margin = new Padding(0, 0, 12, 8);
        btn5a.AutoSize = true; btn5a.AutoSizeMode = AutoSizeMode.GrowOnly; btn5a.Padding = new Padding(16, 0, 16, 0);
        btn5b.Name = "btn5b"; btn5b.Text = "5b. Thông tin đầu sách"; btn5b.Tag = 1;
        btn5b.Size = new Size(230, 50); btn5b.Margin = new Padding(0, 0, 12, 8);
        btn5b.AutoSize = true; btn5b.AutoSizeMode = AutoSizeMode.GrowOnly; btn5b.Padding = new Padding(16, 0, 16, 0);
        btn5c.Name = "btn5c"; btn5c.Text = "5c. Người lớn đang mượn"; btn5c.Tag = 2;
        btn5c.Size = new Size(265, 50); btn5c.Margin = new Padding(0, 0, 12, 8);
        btn5c.AutoSize = true; btn5c.AutoSizeMode = AutoSizeMode.GrowOnly; btn5c.Padding = new Padding(16, 0, 16, 0);
        btn5d.Name = "btn5d"; btn5d.Text = "5d. Người lớn quá hạn"; btn5d.Tag = 3;
        btn5d.Size = new Size(230, 50); btn5d.Margin = new Padding(0, 0, 12, 8);
        btn5d.AutoSize = true; btn5d.AutoSizeMode = AutoSizeMode.GrowOnly; btn5d.Padding = new Padding(16, 0, 16, 0);
        btn5e.Name = "btn5e"; btn5e.Text = "5e. Người lớn và trẻ em"; btn5e.Tag = 4;
        btn5e.Size = new Size(245, 50); btn5e.Margin = new Padding(0, 0, 12, 8);
        btn5e.AutoSize = true; btn5e.AutoSizeMode = AutoSizeMode.GrowOnly; btn5e.Padding = new Padding(16, 0, 16, 0);
        subButtons.Controls.AddRange(new Control[] { btn5a, btn5b, btn5c, btn5d, btn5e });
    }
}
