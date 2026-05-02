using System;
using System.Drawing;
using System.Windows.Forms;

namespace Puzzle15
{
    public class MenuForm : Form
    {
        private Panel titleBar;
        private Label labelTitle;
        private Button btnClose;

        private Label lblProfile;
        private Label lblAuthPrompt;
        private Label lblSizePrompt;
        private NumericUpDown numSize;

        private Button btnNewGame;
        private Button btnStats;
        private Button btnSettings;
        private Button btnAuth;

        private Point dragOffset;

        public MenuForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = new Size(400, 500);
            this.BackColor = Color.FromArgb(32, 32, 32);
            this.StartPosition = FormStartPosition.CenterScreen;

            titleBar = new Panel { Height = 40, Dock = DockStyle.Top, BackColor = Color.FromArgb(45, 45, 48) };
            labelTitle = new Label { ForeColor = Color.White, Location = new Point(10, 10), AutoSize = true, Font = new Font("Segoe UI", 10) };
            btnClose = new Button { Text = "✕", Size = new Size(40, 40), Dock = DockStyle.Right, FlatStyle = FlatStyle.Flat, ForeColor = Color.White };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => Application.Exit();

            titleBar.Controls.Add(labelTitle);
            titleBar.Controls.Add(btnClose);
            titleBar.MouseDown += StartDrag;
            titleBar.MouseMove += DoDrag;
            labelTitle.MouseDown += StartDrag;
            labelTitle.MouseMove += DoDrag;

            lblProfile = new Label { ForeColor = Color.FromArgb(0, 122, 204), Font = new Font("Segoe UI", 12, FontStyle.Bold), Location = new Point(0, 60), Size = new Size(400, 25), TextAlign = ContentAlignment.MiddleCenter };
            lblAuthPrompt = new Label { ForeColor = Color.Gray, Font = new Font("Segoe UI", 9), Location = new Point(0, 85), Size = new Size(400, 20), TextAlign = ContentAlignment.MiddleCenter };

            lblSizePrompt = new Label { ForeColor = Color.White, Font = new Font("Segoe UI", 10), Location = new Point(0, 130), Size = new Size(400, 20), TextAlign = ContentAlignment.MiddleCenter };
            numSize = new NumericUpDown { Location = new Point(75, 160), Size = new Size(250, 30), Minimum = 3, Maximum = 10, Value = 4, Font = new Font("Segoe UI", 12), BackColor = Color.FromArgb(60, 60, 65), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };

            btnNewGame = new Button { Location = new Point(75, 210), Size = new Size(250, 45), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(0, 122, 204), ForeColor = Color.White, Font = new Font("Segoe UI", 10, FontStyle.Bold), Cursor = Cursors.Hand };
            btnNewGame.FlatAppearance.BorderSize = 0;
            btnNewGame.Click += BtnNewGame_Click;

            btnStats = new Button { Location = new Point(75, 270), Size = new Size(250, 45), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(60, 60, 65), ForeColor = Color.White, Font = new Font("Segoe UI", 10, FontStyle.Bold), Cursor = Cursors.Hand };
            btnStats.FlatAppearance.BorderSize = 0;
            btnStats.Click += (s, e) => { new StatsForm().ShowDialog(this); };

            btnSettings = new Button { Location = new Point(75, 330), Size = new Size(250, 45), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(60, 60, 65), ForeColor = Color.White, Font = new Font("Segoe UI", 10, FontStyle.Bold), Cursor = Cursors.Hand };
            btnSettings.FlatAppearance.BorderSize = 0;
            btnSettings.Click += (s, e) => { new SettingsForm().ShowDialog(this); UpdateUITexts(); };

            btnAuth = new Button { Location = new Point(75, 390), Size = new Size(250, 45), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(60, 60, 65), ForeColor = Color.White, Font = new Font("Segoe UI", 10, FontStyle.Bold), Cursor = Cursors.Hand };
            btnAuth.FlatAppearance.BorderSize = 0;
            btnAuth.Click += BtnAuth_Click;

            this.Controls.AddRange(new Control[] { titleBar, lblProfile, lblAuthPrompt, lblSizePrompt, numSize, btnNewGame, btnStats, btnSettings, btnAuth });

            UpdateUITexts();
        }

        private void BtnNewGame_Click(object sender, EventArgs e)
        {
            this.Hide();
            GameForm game = new GameForm((int)numSize.Value);
            game.ShowDialog();
            this.Show();
            UpdateUITexts();
        }

        private void BtnAuth_Click(object sender, EventArgs e)
        {
            if (UserManager.CurrentUser == null || UserManager.CurrentUser.Name == "Гість" || UserManager.CurrentUser.Name == "Guest")
            {
                this.Hide();
                new AuthForm().ShowDialog(this);
                this.Show();
            }
            else
            {
                UserManager.Logout();
            }
            UpdateUITexts();
        }

        private void UpdateUITexts()
        {
            labelTitle.Text = LocalizationManager.Get("MenuTitle");
            lblSizePrompt.Text = LocalizationManager.Get("InputSize");
            btnNewGame.Text = LocalizationManager.Get("StartGame");
            btnStats.Text = LocalizationManager.Get("StatsBtn");
            btnSettings.Text = LocalizationManager.Get("SettingsBtn");

            if (UserManager.CurrentUser != null && !string.IsNullOrEmpty(UserManager.CurrentUser.Name))
            {
                if (UserManager.CurrentUser.Name == "Гість" || UserManager.CurrentUser.Name == "Guest")
                {
                    lblProfile.Text = LocalizationManager.Get("ProfileGuest");
                    lblAuthPrompt.Text = LocalizationManager.Get("AuthPrompt");
                    btnAuth.Text = LocalizationManager.Get("AuthBtn");
                }
                else
                {
                    lblProfile.Text = LocalizationManager.Get("Profile") + UserManager.CurrentUser.Name;
                    lblAuthPrompt.Text = "";
                    btnAuth.Text = LocalizationManager.Get("LogoutBtn");
                }
            }
            else
            {
                lblProfile.Text = LocalizationManager.Get("ProfileGuest");
                lblAuthPrompt.Text = LocalizationManager.Get("AuthPrompt");
                btnAuth.Text = LocalizationManager.Get("AuthBtn");
            }
        }

        private void StartDrag(object sender, MouseEventArgs e) { if (e.Button == MouseButtons.Left) dragOffset = this.PointToClient(Cursor.Position); }
        private void DoDrag(object sender, MouseEventArgs e) { if (e.Button == MouseButtons.Left) this.Location = new Point(Cursor.Position.X - dragOffset.X, Cursor.Position.Y - dragOffset.Y); }
    }
}