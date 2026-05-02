using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Puzzle15
{
    public class StatsForm : Form
    {
        private Point dragOffset;
        private Label lblTitle;
        private Button btnPersonal;
        private Button btnGlobal;
        private Label lblStatsDisplay;
        private Button btnCloseBottom;

        public StatsForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = new Size(350, 450);
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
            labelTitle_MouseDown(lblTitle);

            btnPersonal = new Button { Location = new Point(20, 50), Size = new Size(150, 35), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(0, 122, 204), ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold), Cursor = Cursors.Hand };
            btnPersonal.FlatAppearance.BorderSize = 0;
            btnPersonal.Click += (s, e) => ShowPersonalStats();

            btnGlobal = new Button { Location = new Point(180, 50), Size = new Size(150, 35), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(60, 60, 65), ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold), Cursor = Cursors.Hand };
            btnGlobal.FlatAppearance.BorderSize = 0;
            btnGlobal.Click += (s, e) => ShowGlobalStats();

            lblStatsDisplay = new Label
            {
                Location = new Point(20, 100),
                Size = new Size(310, 280),
                ForeColor = Color.White,
                Font = new Font("Consolas", 10, FontStyle.Regular),
                BackColor = Color.FromArgb(45, 45, 48),
                Padding = new Padding(10),
                TextAlign = ContentAlignment.TopLeft,
                AutoSize = false
            };

            btnCloseBottom = new Button { Location = new Point(115, 395), Size = new Size(120, 35), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(60, 60, 65), ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold), Cursor = Cursors.Hand };
            btnCloseBottom.FlatAppearance.BorderSize = 0;
            btnCloseBottom.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] { titleBar, btnPersonal, btnGlobal, lblStatsDisplay, btnCloseBottom });

            UpdateUITexts();
            ShowPersonalStats();
        }

        private void labelTitle_MouseDown(Label lbl)
        {
            lbl.MouseDown += StartDrag;
            lbl.MouseMove += DoDrag;
        }

        private void ShowPersonalStats()
        {
            btnPersonal.BackColor = Color.FromArgb(0, 122, 204);
            btnGlobal.BackColor = Color.FromArgb(60, 60, 65);

            // Використовуємо Name замість Username
            if (UserManager.CurrentUser == null || UserManager.CurrentUser.Name == "Гість" || UserManager.CurrentUser.Name == "Guest")
            {
                lblStatsDisplay.Text = LocalizationManager.Get("NoStats");
                return;
            }

            string timeStr = "--:--.--";
            if (UserManager.CurrentUser.BestTime > 0)
            {
                TimeSpan ts = TimeSpan.FromMilliseconds(UserManager.CurrentUser.BestTime);
                timeStr = $"{ts.Minutes:D2}:{ts.Seconds:D2}.{ts.Milliseconds / 10:D2}";
            }
            string movesStr = UserManager.CurrentUser.BestMoves == 0 ? "-" : UserManager.CurrentUser.BestMoves.ToString();

            string stats = string.Format(LocalizationManager.Get("PersonalStatsText"), UserManager.CurrentUser.Name, timeStr, movesStr);

            if (UserManager.CurrentUser.WinsPerSize != null && UserManager.CurrentUser.WinsPerSize.Count > 0)
            {
                foreach (var win in UserManager.CurrentUser.WinsPerSize.OrderBy(w => w.Key))
                {
                    stats += $"- Поле {win.Key}x{win.Key}: {win.Value}\n";
                }
            }
            else
            {
                stats += "0\n";
            }

            lblStatsDisplay.Text = stats;
        }

        private void ShowGlobalStats()
        {
            btnGlobal.BackColor = Color.FromArgb(0, 122, 204);
            btnPersonal.BackColor = Color.FromArgb(60, 60, 65);

            string globalStats = LocalizationManager.Get("GlobalStatsText");

            // ДОДАНО ДУЖКИ ДО ToList()
            var topPlayers = UserManager.AllUsers
                .Where(u => u.Name != "Гість" && u.Name != "Guest" && u.BestTime > 0)
                .OrderBy(u => u.BestTime)
                .Take(5)
                .ToList();

            if (topPlayers.Count == 0)
            {
                globalStats += LocalizationManager.Get("NoStats");
            }
            else
            {
                for (int i = 0; i < topPlayers.Count; i++)
                {
                    TimeSpan ts = TimeSpan.FromMilliseconds(topPlayers[i].BestTime);
                    string timeStr = $"{ts.Minutes:D2}:{ts.Seconds:D2}.{ts.Milliseconds / 10:D2}";
                    globalStats += $"{i + 1}. {topPlayers[i].Name.PadRight(15)} {timeStr}\n";
                }
            }

            lblStatsDisplay.Text = globalStats;
        }

        private void UpdateUITexts()
        {
            lblTitle.Text = LocalizationManager.Get("StatsFormTitle");
            btnPersonal.Text = LocalizationManager.Get("TabPersonal");
            btnGlobal.Text = LocalizationManager.Get("TabGlobal");
            btnCloseBottom.Text = LocalizationManager.Get("CloseBtn");
        }

        private void StartDrag(object sender, MouseEventArgs e) { if (e.Button == MouseButtons.Left) dragOffset = this.PointToClient(Cursor.Position); }
        private void DoDrag(object sender, MouseEventArgs e) { if (e.Button == MouseButtons.Left) this.Location = new Point(Cursor.Position.X - dragOffset.X, Cursor.Position.Y - dragOffset.Y); }
    }
}