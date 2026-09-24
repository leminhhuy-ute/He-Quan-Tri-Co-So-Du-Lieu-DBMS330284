namespace CuoiKyWinForms;

public partial class FrmBai6
{
    private Button btn61 = null!;
    private Button btn62 = null!;
    private Button btn63 = null!;
    private Button btn64 = null!;
    private Label fieldLabel1 = null!;
    private Label fieldLabel2 = null!;
    private Label fieldLabel3 = null!;
    private Label fieldLabel4 = null!;
    private TextBox field1 = null!;
    private TextBox field2 = null!;
    private TextBox field3 = null!;
    private TextBox field4 = null!;
    private ComboBox statusBox = null!;
    private Button primaryButton = null!;
    private Button secondButton = null!;
    private Button reloadButton = null!;

    private void InitializeComponent()
    {
        btn61 = new Button(); btn62 = new Button(); btn63 = new Button(); btn64 = new Button();
        fieldLabel1 = new Label(); fieldLabel2 = new Label();
        fieldLabel3 = new Label(); fieldLabel4 = new Label();
        field1 = new TextBox(); field2 = new TextBox();
        field3 = new TextBox(); field4 = new TextBox();
        statusBox = new ComboBox();
        primaryButton = new Button(); secondButton = new Button(); reloadButton = new Button();

        Name = "FrmBai6";
        Text = "Bài 6 — Bài tập lớn SQL DBMS";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(1140, 760);
        MinimumSize = new Size(1000, 640);
        titleLabel.Text = "Bài 6 — Trigger thư viện";
        parameterPanel.Visible = false;
        extraPanel.Height = 212;
        extraPanel.Visible = true;
        extraPanel.BackColor = Color.FromArgb(242, 245, 250);
        extraPanel.BorderStyle = BorderStyle.FixedSingle;

        btn61.Name = "btn61"; btn61.Text = "6.1. Xóa mượn"; btn61.Tag = 0;
        btn61.Size = new Size(220, 50); btn61.Margin = new Padding(0, 0, 12, 8);
        btn61.AutoSize = true; btn61.AutoSizeMode = AutoSizeMode.GrowOnly; btn61.Padding = new Padding(16, 0, 16, 0);
        btn62.Name = "btn62"; btn62.Text = "6.2. Thêm mượn"; btn62.Tag = 1;
        btn62.Size = new Size(220, 50); btn62.Margin = new Padding(0, 0, 12, 8);
        btn62.AutoSize = true; btn62.AutoSizeMode = AutoSizeMode.GrowOnly; btn62.Padding = new Padding(16, 0, 16, 0);
        btn63.Name = "btn63"; btn63.Text = "6.3. Cập nhật cuốn sách"; btn63.Tag = 2;
        btn63.Size = new Size(250, 50); btn63.Margin = new Padding(0, 0, 12, 8);
        btn63.AutoSize = true; btn63.AutoSizeMode = AutoSizeMode.GrowOnly; btn63.Padding = new Padding(16, 0, 16, 0);
        btn64.Name = "btn64"; btn64.Text = "6.4. Thông báo tựa sách"; btn64.Tag = 3;
        btn64.Size = new Size(250, 50); btn64.Margin = new Padding(0, 0, 12, 8);
        btn64.AutoSize = true; btn64.AutoSizeMode = AutoSizeMode.GrowOnly; btn64.Padding = new Padding(16, 0, 16, 0);
        subButtons.Controls.AddRange(new Control[] { btn61, btn62, btn63, btn64 });

        fieldLabel1.Name = "fieldLabel1"; fieldLabel1.Location = new Point(18, 18);
        fieldLabel1.Size = new Size(150, 40); fieldLabel1.Text = "Mã ISBN:";
        field1.Name = "field1"; field1.Location = new Point(180, 23);
        field1.Size = new Size(190, 32); field1.Clear();
        fieldLabel2.Name = "fieldLabel2"; fieldLabel2.Location = new Point(390, 18);
        fieldLabel2.Size = new Size(150, 40); fieldLabel2.Text = "Mã cuốn sách:";
        field2.Name = "field2"; field2.Location = new Point(550, 23);
        field2.Size = new Size(230, 32); field2.Clear();
        fieldLabel3.Name = "fieldLabel3"; fieldLabel3.Location = new Point(18, 78);
        fieldLabel3.Size = new Size(150, 40); fieldLabel3.Text = "Mã độc giả:";
        field3.Name = "field3"; field3.Location = new Point(180, 83);
        field3.Size = new Size(190, 32); field3.Clear();
        fieldLabel4.Name = "fieldLabel4"; fieldLabel4.Location = new Point(390, 78);
        fieldLabel4.Size = new Size(150, 40); fieldLabel4.Text = "Tóm tắt:";
        field4.Name = "field4"; field4.Location = new Point(550, 83);
        field4.Size = new Size(230, 32);
        statusBox.Name = "statusBox"; statusBox.Location = new Point(550, 83);
        statusBox.Size = new Size(200, 32);
        statusBox.DropDownStyle = ComboBoxStyle.DropDownList;
        statusBox.Items.AddRange(new object[] { "yes", "no" });
        statusBox.Visible = false;

        primaryButton.Name = "primaryButton";
        primaryButton.Location = new Point(18, 145);
        primaryButton.Size = new Size(260, 48);
        primaryButton.Text = "Xóa lượt mượn";
        primaryButton.BackColor = Color.FromArgb(29, 63, 111);
        primaryButton.ForeColor = Color.White;
        primaryButton.FlatStyle = FlatStyle.Flat;
        primaryButton.Click += primaryButton_Click;
        secondButton.Name = "secondButton";
        secondButton.Location = new Point(300, 145);
        secondButton.Size = new Size(260, 48);
        secondButton.Text = "Sửa tựa sách/tác giả";
        secondButton.FlatStyle = FlatStyle.Flat;
        secondButton.Click += secondButton_Click;
        reloadButton.Name = "reloadButton";
        reloadButton.Location = new Point(582, 145);
        reloadButton.Size = new Size(260, 48);
        reloadButton.Text = "Tải lại bảng";
        reloadButton.FlatStyle = FlatStyle.Flat;
        reloadButton.Click += reloadButton_Click;

        extraPanel.Controls.AddRange(new Control[]
        {
            fieldLabel1, field1, fieldLabel2, field2, fieldLabel3, field3,
            fieldLabel4, field4, statusBox, primaryButton, secondButton, reloadButton
        });
    }
}
