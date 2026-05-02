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
        private Button btnLangToggle;
        private Label lblSpeedrun;
        private Button btnSpeedrunToggle;
        private Button btnCloseBottom;

        public SettingsForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = new Size(300, 290); // Трохи збільшив висоту для відступів
            this.BackColor = Color.FromArgb(32, 32, 32);
            this.StartPosition = FormStartPosition.CenterParent;

            Panel titleBar = new Panel { Height = 40, Dock = DockStyle.Top, BackColor = Color.FromArgb(45, 45, 48) };
            lblTitle = new Label { ForeColor = Color.White, Location = new Point(10, 10), AutoSize = true, Font = new Font("Segoe UI", 10) };

            Button btnClose = new Button { Text = "✕", Size = new Size(40, 40), Dock = DockStyle.Right, FlatStyle = FlatStyle.Flat, ForeColor = Color.White };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();

            titleBar.Controls.Add(lblTitle);
            titleBar.Controls.Add(btnClose);
            titleBar.MouseDown += StartDrag;
            titleBar.MouseMove += DoDrag;
            lblTitle.MouseDown += StartDrag;
            lblTitle.MouseMove += DoDrag;

            // --- БЛОК МОВИ ---
            lblLang = new Label { ForeColor = Color.Gray, Location = new Point(0, 60), Size = new Size(300, 20), AutoSize = false, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 10) };

            btnLangToggle = new Button { Location = new Point(50, 90), Size = new Size(200, 40), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(60, 60, 65), ForeColor = Color.White, Font = new Font("Segoe UI", 10, FontStyle.Bold), Cursor = Cursors.Hand };
            btnLangToggle.FlatAppearance.BorderSize = 0;
            btnLangToggle.Click += BtnLangToggle_Click;

            // --- БЛОК СПІДРАНУ ---
            lblSpeedrun = new Label { ForeColor = Color.Gray, Location = new Point(0, 150), Size = new Size(300, 20), AutoSize = false, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 10) };

            btnSpeedrunToggle = new Button { Location = new Point(50, 180), Size = new Size(200, 40), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(60, 60, 65), ForeColor = Color.White, Font = new Font("Segoe UI", 10, FontStyle.Bold), Cursor = Cursors.Hand };
            btnSpeedrunToggle.FlatAppearance.BorderSize = 0;
            btnSpeedrunToggle.Click += BtnSpeedrunToggle_Click;

            // --- КНОПКА ЗАКРИТТЯ ---
            btnCloseBottom = new Button { Location = new Point(90, 240), Size = new Size(120, 35), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(0, 122, 204), ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold), Cursor = Cursors.Hand };
            btnCloseBottom.FlatAppearance.BorderSize = 0;
            btnCloseBottom.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] { titleBar, lblLang, btnLangToggle, lblSpeedrun, btnSpeedrunToggle, btnCloseBottom });
            UpdateUITexts();
        }

        private void BtnLangToggle_Click(object sender, EventArgs e)
        {
            LocalizationManager.CurrentLanguage = LocalizationManager.CurrentLanguage == AppLanguage.Ukrainian ? AppLanguage.English : AppLanguage.Ukrainian;
            UpdateUITexts();
        }

        private void BtnSpeedrunToggle_Click(object sender, EventArgs e)
        {
            UserManager.IsSpeedrunMode = !UserManager.IsSpeedrunMode;
            UpdateUITexts();
        }

        private void UpdateUITexts()
        {
            lblTitle.Text = LocalizationManager.Get("SettingsTitle");
            lblLang.Text = LocalizationManager.Get("LangLabel");
            lblSpeedrun.Text = LocalizationManager.Get("SpeedrunLabel");
            btnCloseBottom.Text = LocalizationManager.Get("CloseBtn");

            // Оновлюємо текст кнопки мови
            btnLangToggle.Text = LocalizationManager.CurrentLanguage == AppLanguage.Ukrainian ? "УКРАЇНСЬКА" : "ENGLISH";

            // Оновлюємо текст і колір кнопки спідрану
            if (UserManager.IsSpeedrunMode)
            {
                btnSpeedrunToggle.Text = LocalizationManager.Get("On");
                btnSpeedrunToggle.BackColor = Color.FromArgb(0, 122, 204); // Активний синій колір
            }
            else
            {
                btnSpeedrunToggle.Text = LocalizationManager.Get("Off");
                btnSpeedrunToggle.BackColor = Color.FromArgb(60, 60, 65); // Звичайний сірий колір
            }
        }

        private void StartDrag(object sender, MouseEventArgs e) { if (e.Button == MouseButtons.Left) dragOffset = this.PointToClient(Cursor.Position); }
        private void DoDrag(object sender, MouseEventArgs e) { if (e.Button == MouseButtons.Left) this.Location = new Point(Cursor.Position.X - dragOffset.X, Cursor.Position.Y - dragOffset.Y); }
    }
}