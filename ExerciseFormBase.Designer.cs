#nullable enable
namespace CuoiKyWinForms;

public partial class ExerciseFormBase
{
    private System.ComponentModel.IContainer components = new System.ComponentModel.Container();
    protected Label titleLabel = null!;
    protected TextBox descriptionBox = null!;
    protected FlowLayoutPanel subButtons = null!;
    protected Panel parameterPanel = null!;
    protected Label paramLabel1 = null!;
    protected Label paramLabel2 = null!;
    protected Label paramLabel3 = null!;
    protected TextBox paramText1 = null!;
    protected TextBox paramText2 = null!;
    protected TextBox paramText3 = null!;
    protected Button executeButton = null!;
    protected Panel extraPanel = null!;
    protected TextBox resultText = null!;
    protected DataGridView resultGrid = null!;
    private Panel bottomPanel = null!;
    private Button backButton = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        titleLabel = new Label();
        descriptionBox = new TextBox();
        subButtons = new FlowLayoutPanel();
        parameterPanel = new Panel();
        paramLabel1 = new Label();
        paramLabel2 = new Label();
        paramLabel3 = new Label();
        paramText1 = new TextBox();
        paramText2 = new TextBox();
        paramText3 = new TextBox();
        executeButton = new Button();
        extraPanel = new Panel();
        resultText = new TextBox();
        resultGrid = new DataGridView();
        bottomPanel = new Panel();
        backButton = new Button();
        parameterPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)resultGrid).BeginInit();
        SuspendLayout();

        titleLabel.Dock = DockStyle.Top;
        titleLabel.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        titleLabel.ForeColor = Color.FromArgb(29, 63, 111);
        titleLabel.Height = 88;
        titleLabel.Padding = new Padding(12, 18, 0, 0);
        titleLabel.Name = "titleLabel";
        titleLabel.Text = "Bài tập";

        descriptionBox.Dock = DockStyle.Top;
        descriptionBox.Multiline = true;
        descriptionBox.ReadOnly = true;
        descriptionBox.ScrollBars = ScrollBars.Vertical;
        descriptionBox.BackColor = Color.FromArgb(246, 248, 251);
        descriptionBox.BorderStyle = BorderStyle.FixedSingle;
        descriptionBox.Font = new Font("Segoe UI", 10F);
        descriptionBox.Height = 108;
        descriptionBox.Name = "descriptionBox";

        subButtons.Dock = DockStyle.Top;
        subButtons.AutoSize = true;
        subButtons.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        subButtons.MinimumSize = new Size(0, 14);
        subButtons.Padding = new Padding(0, 12, 0, 14);
        subButtons.WrapContents = true;
        subButtons.Name = "subButtons";

        parameterPanel.Dock = DockStyle.Top;
        parameterPanel.Size = new Size(1116, 90);
        parameterPanel.BackColor = Color.FromArgb(242, 245, 250);
        parameterPanel.BorderStyle = BorderStyle.FixedSingle;
        parameterPanel.Name = "parameterPanel";
        parameterPanel.Controls.Add(paramLabel1);
        parameterPanel.Controls.Add(paramText1);
        parameterPanel.Controls.Add(paramLabel2);
        parameterPanel.Controls.Add(paramText2);
        parameterPanel.Controls.Add(paramLabel3);
        parameterPanel.Controls.Add(paramText3);
        parameterPanel.Controls.Add(executeButton);

        paramLabel1.Location = new Point(18, 24);
        paramLabel1.Size = new Size(150, 40);
        paramLabel1.TextAlign = ContentAlignment.MiddleLeft;
        paramLabel1.Name = "paramLabel1";
        paramLabel1.Text = "Tham số 1:";
        paramText1.Location = new Point(180, 26);
        paramText1.Size = new Size(190, 32);
        paramText1.Name = "paramText1";
        paramLabel2.Location = new Point(370, 24);
        paramLabel2.Size = new Size(150, 40);
        paramLabel2.TextAlign = ContentAlignment.MiddleLeft;
        paramLabel2.Name = "paramLabel2";
        paramLabel2.Text = "Tham số 2:";
        paramText2.Location = new Point(535, 26);
        paramText2.Size = new Size(190, 32);
        paramText2.Name = "paramText2";
        paramLabel3.Location = new Point(18, 82);
        paramLabel3.Size = new Size(150, 40);
        paramLabel3.TextAlign = ContentAlignment.MiddleLeft;
        paramLabel3.Name = "paramLabel3";
        paramLabel3.Text = "Tham số 3:";
        paramText3.Location = new Point(180, 84);
        paramText3.Size = new Size(190, 32);
        paramText3.Name = "paramText3";
        executeButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        executeButton.BackColor = Color.FromArgb(29, 63, 111);
        executeButton.ForeColor = Color.White;
        executeButton.FlatStyle = FlatStyle.Flat;
        executeButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        executeButton.Location = new Point(900, 20);
        executeButton.Size = new Size(180, 48);
        executeButton.Name = "executeButton";
        executeButton.Text = "Thực hiện";
        executeButton.UseVisualStyleBackColor = false;
        executeButton.Click += executeButton_Click;

        extraPanel.Dock = DockStyle.Top;
        extraPanel.Height = 0;
        extraPanel.Name = "extraPanel";

        resultText.Dock = DockStyle.Top;
        resultText.Multiline = true;
        resultText.ReadOnly = true;
        resultText.ScrollBars = ScrollBars.Vertical;
        resultText.Font = new Font("Consolas", 10F, FontStyle.Bold);
        resultText.Height = 84;
        resultText.Name = "resultText";
        resultText.Text = "Bấm Thực hiện để chạy truy vấn.";

        resultGrid.Dock = DockStyle.Fill;
        resultGrid.ReadOnly = true;
        resultGrid.AllowUserToAddRows = false;
        resultGrid.AllowUserToDeleteRows = false;
        resultGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        resultGrid.BackgroundColor = Color.White;
        resultGrid.RowTemplate.Height = 34;
        resultGrid.ColumnHeadersHeight = 38;
        resultGrid.Name = "resultGrid";

        bottomPanel.Dock = DockStyle.Bottom;
        bottomPanel.Size = new Size(1116, 60);
        bottomPanel.Name = "bottomPanel";
        bottomPanel.BackColor = Color.FromArgb(242, 245, 250);
        bottomPanel.Controls.Add(backButton);
        backButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        backButton.Location = new Point(920, 10);
        backButton.Size = new Size(170, 40);
        backButton.Name = "backButton";
        backButton.Text = "Về menu chính";
        backButton.UseVisualStyleBackColor = true;
        backButton.Click += backButton_Click;

        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.White;
        Font = new Font("Segoe UI", 9F);
        MinimumSize = new Size(1000, 640);
        Name = "ExerciseFormBase";
        Padding = new Padding(12);
        ClientSize = new Size(1140, 760);
        Controls.Add(resultGrid);
        Controls.Add(resultText);
        Controls.Add(extraPanel);
        Controls.Add(parameterPanel);
        Controls.Add(subButtons);
        Controls.Add(descriptionBox);
        Controls.Add(titleLabel);
        Controls.Add(bottomPanel);
        parameterPanel.ResumeLayout(false);
        parameterPanel.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)resultGrid).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
