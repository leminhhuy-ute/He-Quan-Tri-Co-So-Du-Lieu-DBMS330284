namespace CuoiKyWinForms;

public partial class FrmBai8
{
    private Button btn81 = null!;
    private Button btn82 = null!;
    private Button btn83 = null!;
    private Button btn84 = null!;
    private Button btn85 = null!;

    private void InitializeComponent()
    {
        btn81 = new Button();
        btn82 = new Button();
        btn83 = new Button();
        btn84 = new Button();
        btn85 = new Button();
        Name = "FrmBai8";
        Text = "Bài 8 — Bài tập lớn SQL DBMS";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(1140, 760);
        MinimumSize = new Size(1000, 640);
        titleLabel.Text = "Bài 8";
        btn81.Name = "btn81";
        btn81.Text = "8.1. Dự án > 2 NV";
        btn81.Tag = 0;
        btn81.Size = new Size(205, 50);
        btn81.AutoSize = true;
        btn81.AutoSizeMode = AutoSizeMode.GrowOnly;
        btn81.Padding = new Padding(16, 0, 16, 0);
        btn81.Margin = new Padding(0, 0, 12, 8);
        btn82.Name = "btn82";
        btn82.Text = "8.2. Phòng > 2 NV lương cao";
        btn82.Tag = 1;
        btn82.Size = new Size(285, 50);
        btn82.AutoSize = true;
        btn82.AutoSizeMode = AutoSizeMode.GrowOnly;
        btn82.Padding = new Padding(16, 0, 16, 0);
        btn82.Margin = new Padding(0, 0, 12, 8);
        btn83.Name = "btn83";
        btn83.Text = "8.3. Phòng lương TB > 30k";
        btn83.Tag = 2;
        btn83.Size = new Size(260, 50);
        btn83.AutoSize = true;
        btn83.AutoSizeMode = AutoSizeMode.GrowOnly;
        btn83.Padding = new Padding(16, 0, 16, 0);
        btn83.Margin = new Padding(0, 0, 12, 8);
        btn84.Name = "btn84";
        btn84.Text = "8.4. Phòng lương TB > 30k (Nam)";
        btn84.Tag = 3;
        btn84.Size = new Size(320, 50);
        btn84.AutoSize = true;
        btn84.AutoSizeMode = AutoSizeMode.GrowOnly;
        btn84.Padding = new Padding(16, 0, 16, 0);
        btn84.Margin = new Padding(0, 0, 12, 8);
        btn85.Name = "btn85";
        btn85.Text = "8.5. Nhân viên phòng 5";
        btn85.Tag = 4;
        btn85.Size = new Size(245, 50);
        btn85.AutoSize = true;
        btn85.AutoSizeMode = AutoSizeMode.GrowOnly;
        btn85.Padding = new Padding(16, 0, 16, 0);
        btn85.Margin = new Padding(0, 0, 12, 8);
        subButtons.Controls.AddRange(new Control[] { btn81, btn82, btn83, btn84, btn85 });
    }
}
