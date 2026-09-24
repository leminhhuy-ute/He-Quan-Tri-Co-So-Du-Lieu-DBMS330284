namespace CuoiKyWinForms;

public partial class FrmBai7
{
    private Button btn71 = null!;
    private Button btn72 = null!;
    private Button btn73 = null!;
    private Button btn74 = null!;
    private Button btn75 = null!;
    private Button btn76a = null!;
    private Button btn76b = null!;

    private void InitializeComponent()
    {
        btn71 = new Button();
        btn72 = new Button();
        btn73 = new Button();
        btn74 = new Button();
        btn75 = new Button();
        btn76a = new Button();
        btn76b = new Button();
        Name = "FrmBai7";
        Text = "Bài 7 — Bài tập lớn SQL DBMS";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(1140, 760);
        MinimumSize = new Size(1000, 640);
        titleLabel.Text = "Bài 7";
        btn71.Name = "btn71";
        btn71.Text = "7.1. Lương TB phòng";
        btn71.Tag = 0;
        btn71.Size = new Size(225, 50);
        btn71.AutoSize = true;
        btn71.AutoSizeMode = AutoSizeMode.GrowOnly;
        btn71.Padding = new Padding(16, 0, 16, 0);
        btn71.Margin = new Padding(0, 0, 12, 8);
        btn72.Name = "btn72";
        btn72.Text = "7.2. Lương NV theo DA";
        btn72.Tag = 1;
        btn72.Size = new Size(245, 50);
        btn72.AutoSize = true;
        btn72.AutoSizeMode = AutoSizeMode.GrowOnly;
        btn72.Padding = new Padding(16, 0, 16, 0);
        btn72.Margin = new Padding(0, 0, 12, 8);
        btn73.Name = "btn73";
        btn73.Text = "7.3. Lương TB các phòng";
        btn73.Tag = 2;
        btn73.Size = new Size(265, 50);
        btn73.AutoSize = true;
        btn73.AutoSizeMode = AutoSizeMode.GrowOnly;
        btn73.Padding = new Padding(16, 0, 16, 0);
        btn73.Margin = new Padding(0, 0, 12, 8);
        btn74.Name = "btn74";
        btn74.Text = "7.4. Thưởng theo giờ";
        btn74.Tag = 3;
        btn74.Size = new Size(235, 50);
        btn74.AutoSize = true;
        btn74.AutoSizeMode = AutoSizeMode.GrowOnly;
        btn74.Padding = new Padding(16, 0, 16, 0);
        btn74.Margin = new Padding(0, 0, 12, 8);
        btn75.Name = "btn75";
        btn75.Text = "7.5. Số DA theo phòng";
        btn75.Tag = 4;
        btn75.Size = new Size(245, 50);
        btn75.AutoSize = true;
        btn75.AutoSizeMode = AutoSizeMode.GrowOnly;
        btn75.Padding = new Padding(16, 0, 16, 0);
        btn75.Margin = new Padding(0, 0, 12, 8);
        btn76a.Name = "btn76a";
        btn76a.Text = "7.6a. TVF Inline";
        btn76a.Tag = 5;
        btn76a.Size = new Size(195, 50);
        btn76a.AutoSize = true;
        btn76a.AutoSizeMode = AutoSizeMode.GrowOnly;
        btn76a.Padding = new Padding(16, 0, 16, 0);
        btn76a.Margin = new Padding(0, 0, 12, 8);
        btn76b.Name = "btn76b";
        btn76b.Text = "7.6b. TVF Multi";
        btn76b.Tag = 6;
        btn76b.Size = new Size(195, 50);
        btn76b.AutoSize = true;
        btn76b.AutoSizeMode = AutoSizeMode.GrowOnly;
        btn76b.Padding = new Padding(16, 0, 16, 0);
        btn76b.Margin = new Padding(0, 0, 12, 8);
        subButtons.Controls.AddRange(new Control[] { btn71, btn72, btn73, btn74, btn75, btn76a, btn76b });
    }
}
