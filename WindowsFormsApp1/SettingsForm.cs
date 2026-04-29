using System;
using System.Drawing;
using System.Windows.Forms;

namespace Puzzle15
{
    public class SettingsForm : Form
    {
        private Point dragOffset;
        private Label lblTitle;
        private Label lblLang;
        private PictureBox switchBox;
        private Button btnCloseBottom;

        public SettingsForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = new Size(300, 200);
            this.BackColor = Color.FromArgb(32, 32, 32);
            this.StartPosition = FormStartPosition.CenterParent;

            Panel titleBar = new Panel { Height = 40, Dock = DockStyle.Top, BackColor = Color.FromArgb(45, 45, 48) };
            lblTitle = new Label { ForeColor = Color.White, Location = new Point(10, 10), AutoSize = true, Font = new Font("Segoe UI", 10) };

            Button btnClose = new Button { Text = "✕", Size = new Size(40, 40), Dock = DockStyle.Right, FlatStyle = FlatStyle.Flat, ForeColor = Color.White };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();
            btnClose.MouseEnter += (s, e) => btnClose.BackColor = Color.Red;
            btnClose.MouseLeave += (s, e) => btnClose.BackColor = Color.Transparent;

            titleBar.Controls.Add(lblTitle);
            titleBar.Controls.Add(btnClose);
            titleBar.MouseDown += StartDrag;
            titleBar.MouseMove += DoDrag;
            lblTitle.MouseDown += StartDrag;
            lblTitle.MouseMove += DoDrag;

            // Розтягуємо Label на всю ширину вікна і вирівнюємо текст по центру
            lblLang = new Label
            {
                ForeColor = Color.Gray,
                Location = new Point(0, 60),
                Size = new Size(300, 20),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 10)
            };

            // Центруємо перемикач: (300 - 120) / 2 = 90
            switchBox = new PictureBox { Location = new Point(90, 90), Size = new Size(120, 35), Cursor = Cursors.Hand };
            switchBox.Paint += SwitchBox_Paint;
            switchBox.Click += SwitchBox_Click;

            // Кнопка закриття вже була по центру (300 - 120) / 2 = 90
            btnCloseBottom = new Button { Location = new Point(90, 150), Size = new Size(120, 35), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(0, 122, 204), ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold), Cursor = Cursors.Hand };
            btnCloseBottom.FlatAppearance.BorderSize = 0;
            btnCloseBottom.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] { titleBar, lblLang, switchBox, btnCloseBottom });
            UpdateUITexts();
        }

        private void SwitchBox_Click(object sender, EventArgs e)
        {
            LocalizationManager.CurrentLanguage = LocalizationManager.CurrentLanguage == AppLanguage.Ukrainian ? AppLanguage.English : AppLanguage.Ukrainian;
            switchBox.Invalidate();
            UpdateUITexts();
        }

        private void SwitchBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            bool isUa = LocalizationManager.CurrentLanguage == AppLanguage.Ukrainian;

            g.Clear(Color.FromArgb(60, 60, 65));

            Rectangle activeRect = isUa ? new Rectangle(0, 0, 60, 35) : new Rectangle(60, 0, 60, 35);
            g.FillRectangle(new SolidBrush(Color.FromArgb(0, 122, 204)), activeRect);

            using (Font f = new Font("Segoe UI", 10, FontStyle.Bold))
            {
                TextRenderer.DrawText(g, "UA", f, new Rectangle(0, 0, 60, 35), isUa ? Color.White : Color.Gray, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                TextRenderer.DrawText(g, "EN", f, new Rectangle(60, 0, 60, 35), !isUa ? Color.White : Color.Gray, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }

            g.DrawRectangle(Pens.Gray, 0, 0, 119, 34);
        }

        private void UpdateUITexts()
        {
            lblTitle.Text = LocalizationManager.Get("SettingsTitle");
            lblLang.Text = LocalizationManager.Get("LangLabel");
            btnCloseBottom.Text = LocalizationManager.Get("CloseBtn");
        }

        private void StartDrag(object sender, MouseEventArgs e) { if (e.Button == MouseButtons.Left) dragOffset = this.PointToClient(Cursor.Position); }
        private void DoDrag(object sender, MouseEventArgs e) { if (e.Button == MouseButtons.Left) this.Location = new Point(Cursor.Position.X - dragOffset.X, Cursor.Position.Y - dragOffset.Y); }
    }
}