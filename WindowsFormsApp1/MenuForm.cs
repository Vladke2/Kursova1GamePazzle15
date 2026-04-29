using System;
using System.Drawing;
using System.Windows.Forms;

namespace Puzzle15
{
    public class MenuForm : Form
    {
        private Panel titleBar;
        private Label lblTitle;
        private Button btnClose;
        private Label lblSelect;
        private NumericUpDown numSize;
        private Button btnContinue;
        private Button btnStart;
        private Button btnAuth;
        private Button btnSettings;

        private Label lblUserStatus;
        private Label lblStats;
        private Point dragOffset;

        public MenuForm()
        {
            UserManager.LoadUsers();
            InitializeMenuUI();
            UpdateUITexts();
        }

        private void InitializeMenuUI()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = new Size(450, 550);
            this.BackColor = Color.FromArgb(32, 32, 32);
            this.StartPosition = FormStartPosition.CenterScreen;

            // --- ВЕРХНЯ ПАНЕЛЬ ---
            titleBar = new Panel { Height = 40, Dock = DockStyle.Top, BackColor = Color.FromArgb(45, 45, 48) };
            lblTitle = new Label { ForeColor = Color.White, Location = new Point(10, 10), AutoSize = true, Font = new Font("Segoe UI", 10) };
            btnClose = new Button { Text = "✕", Size = new Size(40, 40), Dock = DockStyle.Right, FlatStyle = FlatStyle.Flat, ForeColor = Color.White };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => Application.Exit();
            btnClose.MouseEnter += (s, e) => btnClose.BackColor = Color.Red;
            btnClose.MouseLeave += (s, e) => btnClose.BackColor = Color.Transparent;

            titleBar.Controls.Add(lblTitle);
            titleBar.Controls.Add(btnClose);
            titleBar.MouseDown += StartDrag;
            titleBar.MouseMove += DoDrag;
            lblTitle.MouseDown += StartDrag;
            lblTitle.MouseMove += DoDrag;

            // --- СТАТУС ТА СТАТИСТИКА ---
            lblUserStatus = new Label { ForeColor = Color.FromArgb(0, 122, 204), Location = new Point(100, 60), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            lblStats = new Label { ForeColor = Color.LightGray, Location = new Point(100, 85), AutoSize = true, Font = new Font("Segoe UI", 9) };

            // --- ВИБІР РОЗМІРУ ---
            lblSelect = new Label { ForeColor = Color.Gray, Location = new Point(100, 130), AutoSize = true, Font = new Font("Segoe UI", 10) };
            numSize = new NumericUpDown { Location = new Point(100, 160), Width = 250, Font = new Font("Segoe UI", 12), Minimum = 3, Maximum = 10, Value = 4, BackColor = Color.FromArgb(60, 60, 65), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };

            // --- ІГРОВІ КНОПКИ ---
            btnContinue = CreateMenuButton("", 0);
            btnContinue.BackColor = Color.FromArgb(0, 122, 204);
            btnContinue.Click += (s, e) => {
                GameForm game = new GameForm(UserManager.CurrentUser.SavedSize, true);
                this.Hide();
                game.ShowDialog();
                this.Show();
                UpdateUITexts();
            };

            btnStart = CreateMenuButton("", 0);
            btnStart.Click += (s, e) => {
                int size = (int)numSize.Value;
                GameForm game = new GameForm(size);
                this.Hide();
                game.ShowDialog();
                this.Show();
                UpdateUITexts();
            };

            btnSettings = CreateMenuButton("", 0);
            btnSettings.Click += (s, e) => {
                new SettingsForm().ShowDialog(this);
                UpdateUITexts();
            };

            btnAuth = CreateMenuButton("", 0);
            btnAuth.Click += (s, e) => {
                if (UserManager.CurrentUser == null) { new AuthForm().ShowDialog(this); }
                else { UserManager.Logout(); }
                UpdateUITexts();
            };

            this.Controls.AddRange(new Control[] { titleBar, lblUserStatus, lblStats, lblSelect, numSize, btnContinue, btnStart, btnSettings, btnAuth });
        }

        private void UpdateUITexts()
        {
            lblTitle.Text = LocalizationManager.Get("MenuTitle");
            lblSelect.Text = LocalizationManager.Get("InputSize");
            btnContinue.Text = LocalizationManager.Get("ContinueGame");
            btnStart.Text = LocalizationManager.Get("StartGame");
            btnSettings.Text = LocalizationManager.Get("SettingsBtn");

            // Задаємо стартову позицію для першої кнопки
            int currentY = 220;
            int stepY = 60; // 45px висота кнопки + 15px відступ (завжди однаковий!)

            if (UserManager.CurrentUser == null)
            {
                lblUserStatus.Text = LocalizationManager.Get("ProfileGuest");
                lblStats.Text = LocalizationManager.Get("AuthPrompt");
                btnAuth.Text = LocalizationManager.Get("AuthBtn");
                btnAuth.BackColor = Color.FromArgb(60, 60, 65);

                btnContinue.Visible = false;
            }
            else
            {
                lblUserStatus.Text = LocalizationManager.Get("Profile") + UserManager.CurrentUser.Name;
                string timeStr = UserManager.CurrentUser.BestTime == 0 ? "--:--" : $"{UserManager.CurrentUser.BestTime / 60:D2}:{UserManager.CurrentUser.BestTime % 60:D2}";
                string movesStr = UserManager.CurrentUser.BestMoves == 0 ? "-" : UserManager.CurrentUser.BestMoves.ToString();
                lblStats.Text = string.Format(LocalizationManager.Get("Record"), movesStr, timeStr);

                btnAuth.Text = LocalizationManager.Get("LogoutBtn");
                btnAuth.BackColor = Color.FromArgb(180, 50, 50);

                if (UserManager.CurrentUser.HasSavedGame)
                {
                    btnContinue.Visible = true;
                    btnContinue.Location = new Point(100, currentY);
                    currentY += stepY; // Зсуваємо координату для наступної кнопки
                }
                else
                {
                    btnContinue.Visible = false;
                }
            }

            // Всі наступні кнопки прив'язуються одна до одної без "дірок"
            btnStart.Location = new Point(100, currentY);
            currentY += stepY;

            btnSettings.Location = new Point(100, currentY);
            currentY += stepY;

            btnAuth.Location = new Point(100, currentY);
        }

        private void StartDrag(object sender, MouseEventArgs e) { if (e.Button == MouseButtons.Left) dragOffset = this.PointToClient(Cursor.Position); }
        private void DoDrag(object sender, MouseEventArgs e) { if (e.Button == MouseButtons.Left) this.Location = new Point(Cursor.Position.X - dragOffset.X, Cursor.Position.Y - dragOffset.Y); }

        private Button CreateMenuButton(string text, int y)
        {
            Button btn = new Button { Text = text, Location = new Point(100, y), Size = new Size(250, 45), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(60, 60, 65), ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold), Cursor = Cursors.Hand };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }
    }
}