using System;
using System.Drawing;
using System.Windows.Forms;

namespace Puzzle15
{
    public class GameForm : Form
    {
        private GameEngine engine;
        private Button[,] buttons;
        private int size;
        private Panel titleBar;
        private Label labelTitle;
        private Button btnClose;
        private TableLayoutPanel gameGrid;

        private Panel statsPanel;
        private Label labelStats;
        private Button btnPause;
        private Panel pausePanel;
        private Label lblPauseMsg;

        private Timer gameTimer;
        private int secondsElapsed;
        private bool isPaused = false;
        private Point dragOffset;

        // --- ДОДАНО ПАРАМЕТР loadSave ---
        public GameForm(int size, bool loadSave = false)
        {
            this.size = size;
            engine = new GameEngine(size);

            // Якщо ми продовжуємо гру, завантажуємо дані з профілю
            if (loadSave && UserManager.CurrentUser != null)
            {
                engine.LoadState(UserManager.CurrentUser.SavedMoves, UserManager.CurrentUser.SavedBoard);
                secondsElapsed = UserManager.CurrentUser.SavedTime;
            }

            buttons = new Button[size, size];
            this.DoubleBuffered = true;
            InitializeCustomUI();
            UpdateUITexts();
            UpdateUI();
            UpdateStats();

            gameTimer = new Timer { Interval = 1000 };
            gameTimer.Tick += (s, e) => { secondsElapsed++; UpdateStats(); };
            gameTimer.Start();
        }

        private void InitializeCustomUI()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = new Size(450, 550);
            this.MinimumSize = new Size(350, 450);
            this.BackColor = Color.FromArgb(32, 32, 32);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Padding = new Padding(4);

            titleBar = new Panel { Height = 40, Dock = DockStyle.Top, BackColor = Color.FromArgb(45, 45, 48) };
            labelTitle = new Label { ForeColor = Color.White, Font = new Font("Segoe UI", 10), Location = new Point(10, 10), AutoSize = true };
            btnClose = new Button { Text = "✕", Size = new Size(40, 40), Dock = DockStyle.Right, FlatStyle = FlatStyle.Flat, ForeColor = Color.White };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();

            titleBar.Controls.Add(labelTitle);
            titleBar.Controls.Add(btnClose);
            titleBar.MouseDown += StartDrag;
            titleBar.MouseMove += DoDrag;
            labelTitle.MouseDown += StartDrag;
            labelTitle.MouseMove += DoDrag;

            statsPanel = new Panel { Height = 50, Dock = DockStyle.Bottom, BackColor = Color.FromArgb(45, 45, 48) };
            btnPause = new Button { Dock = DockStyle.Right, Width = 130, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(60, 60, 65), ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold), Cursor = Cursors.Hand };
            btnPause.FlatAppearance.BorderSize = 0;
            btnPause.Click += TogglePause;
            labelStats = new Label { ForeColor = Color.White, Font = new Font("Segoe UI", 12, FontStyle.Bold), Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };

            statsPanel.Controls.Add(labelStats);
            statsPanel.Controls.Add(btnPause);

            gameGrid = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(10), RowCount = size, ColumnCount = size };
            float percent = 100f / size;
            for (int i = 0; i < size; i++)
            {
                gameGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, percent));
                gameGrid.RowStyles.Add(new RowStyle(SizeType.Percent, percent));
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    buttons[i, j] = new Button { Dock = DockStyle.Fill, Margin = new Padding(3), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(60, 60, 65), ForeColor = Color.White };
                    buttons[i, j].FlatAppearance.BorderSize = 0;
                    float fontSize = size >= 9 ? 9f : size >= 7 ? 12f : 14f;
                    buttons[i, j].Font = new Font("Segoe UI", fontSize, FontStyle.Bold);
                    buttons[i, j].Tag = new Point(i, j);
                    buttons[i, j].Click += Button_Click;
                    gameGrid.Controls.Add(buttons[i, j], j, i);
                }
            }

            pausePanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(32, 32, 32), Visible = false };
            lblPauseMsg = new Label { ForeColor = Color.Gray, Font = new Font("Segoe UI", 20, FontStyle.Bold), Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
            pausePanel.Controls.Add(lblPauseMsg);

            this.Controls.Add(pausePanel);
            this.Controls.Add(gameGrid);
            this.Controls.Add(titleBar);
            this.Controls.Add(statsPanel);
            pausePanel.BringToFront();
        }

        private void UpdateUITexts()
        {
            labelTitle.Text = $"{LocalizationManager.Get("GameTitle")} ({size}x{size})";
            btnPause.Text = isPaused ? LocalizationManager.Get("ResumeBtn") : LocalizationManager.Get("PauseBtn");
            lblPauseMsg.Text = LocalizationManager.Get("PauseOverlay");
            UpdateStats();
        }

        private void TogglePause(object sender, EventArgs e)
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                gameTimer.Stop();
                pausePanel.Visible = true;
                btnPause.Text = LocalizationManager.Get("ResumeBtn");
                btnPause.BackColor = Color.FromArgb(0, 122, 204);
            }
            else
            {
                gameTimer.Start();
                pausePanel.Visible = false;
                btnPause.Text = LocalizationManager.Get("PauseBtn");
                btnPause.BackColor = Color.FromArgb(60, 60, 65);
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if (isPaused) return;
            Button btn = (Button)sender;
            Point p = (Point)btn.Tag;

            if (engine.TryMove(p.X, p.Y))
            {
                UpdateUI();
                UpdateStats();

                if (engine.CheckWin())
                {
                    gameTimer.Stop();
                    string timeStr = $"{secondsElapsed / 60:D2}:{secondsElapsed % 60:D2}";

                    if (UserManager.CurrentUser != null)
                    {
                        bool recordUpdated = false;
                        if (UserManager.CurrentUser.BestMoves == 0 || engine.Moves < UserManager.CurrentUser.BestMoves)
                        {
                            UserManager.CurrentUser.BestMoves = engine.Moves;
                            recordUpdated = true;
                        }
                        if (UserManager.CurrentUser.BestTime == 0 || secondsElapsed < UserManager.CurrentUser.BestTime)
                        {
                            UserManager.CurrentUser.BestTime = secondsElapsed;
                            recordUpdated = true;
                        }

                        // Якщо гравець переміг, ми очищаємо збережений стан, бо гра закінчена
                        UserManager.CurrentUser.HasSavedGame = false;
                        UserManager.SaveUsers();
                    }

                    VictoryForm victory = new VictoryForm(timeStr, engine.Moves);
                    DialogResult result = victory.ShowDialog(this);

                    if (result == DialogResult.Abort)
                    {
                        // Якщо натиснуто "Головне меню", просто закриваємо гру
                        this.Close();
                    }
                    else
                    {
                        // Якщо натиснуто "Продовжити", скидаємо таймер і створюємо нове поле
                        secondsElapsed = 0;
                        engine.StartNewGame();
                        UpdateUI();
                        UpdateStats();
                        gameTimer.Start();
                    }
                }
            }
        }

        private void UpdateUI()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int val = engine.GetValue(i, j);
                    buttons[i, j].Text = val == 0 ? "" : val.ToString();
                    buttons[i, j].Visible = val != 0;
                }
            }
        }

        private void UpdateStats()
        {
            string timeStr = $"{secondsElapsed / 60:D2}:{secondsElapsed % 60:D2}";
            labelStats.Text = string.Format(LocalizationManager.Get("StatsFmt"), engine.Moves, timeStr);
        }

        private void StartDrag(object sender, MouseEventArgs e) { if (e.Button == MouseButtons.Left) dragOffset = this.PointToClient(Cursor.Position); }
        private void DoDrag(object sender, MouseEventArgs e) { if (e.Button == MouseButtons.Left) this.Location = new Point(Cursor.Position.X - dragOffset.X, Cursor.Position.Y - dragOffset.Y); }

        // --- ПЕРЕХОПЛЕННЯ ЗАКРИТТЯ ВІКНА ДЛЯ ЗБЕРЕЖЕННЯ ---
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (UserManager.CurrentUser != null)
            {
                if (!engine.CheckWin())
                {
                    // Записуємо все в профіль
                    UserManager.CurrentUser.HasSavedGame = true;
                    UserManager.CurrentUser.SavedSize = size;
                    UserManager.CurrentUser.SavedMoves = engine.Moves;
                    UserManager.CurrentUser.SavedTime = secondsElapsed;
                    UserManager.CurrentUser.SavedBoard = engine.GetFlatBoard();
                }
                UserManager.SaveUsers();
            }
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x84;
            const int HTLEFT = 10, HTRIGHT = 11, HTTOP = 12, HTTOPLEFT = 13, HTTOPRIGHT = 14, HTBOTTOM = 15, HTBOTTOMLEFT = 16, HTBOTTOMRIGHT = 17;
            if (m.Msg == WM_NCHITTEST)
            {
                int x = (int)(m.LParam.ToInt64() & 0xFFFF);
                int y = (int)((m.LParam.ToInt64() & 0xFFFF0000) >> 16);
                Point pt = PointToClient(new Point(x, y));
                int border = 5;
                if (pt.X <= border && pt.Y <= border) { m.Result = (IntPtr)HTTOPLEFT; return; }
                if (pt.X >= ClientSize.Width - border && pt.Y <= border) { m.Result = (IntPtr)HTTOPRIGHT; return; }
                if (pt.X <= border && pt.Y >= ClientSize.Height - border) { m.Result = (IntPtr)HTBOTTOMLEFT; return; }
                if (pt.X >= ClientSize.Width - border && pt.Y >= ClientSize.Height - border) { m.Result = (IntPtr)HTBOTTOMRIGHT; return; }
                if (pt.X <= border) { m.Result = (IntPtr)HTLEFT; return; }
                if (pt.X >= ClientSize.Width - border) { m.Result = (IntPtr)HTRIGHT; return; }
                if (pt.Y <= border) { m.Result = (IntPtr)HTTOP; return; }
                if (pt.Y >= ClientSize.Height - border) { m.Result = (IntPtr)HTBOTTOM; return; }
            }
            base.WndProc(ref m);
        }
    }
}