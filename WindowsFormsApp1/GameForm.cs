using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading.Tasks;

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
        private PictureBox statsBox;
        private Button btnPause;
        private Button btnAutoSolve;
        private Panel pausePanel;
        private Label lblPauseMsg;

        private Timer uiTimer;
        private Stopwatch gameStopwatch;
        private int baseTimeMs = 0;

        private Timer autoTimer;
        private bool isPaused = false;
        private bool isAutoSolving = false;
        private Point dragOffset;

        private string lastStatsText = "";

        public GameForm(int size, bool loadSave = false)
        {
            this.size = size;
            engine = new GameEngine(size);
            gameStopwatch = new Stopwatch();

            if (loadSave && UserManager.CurrentUser != null)
            {
                engine.LoadState(UserManager.CurrentUser.SavedMoves, UserManager.CurrentUser.SavedBoard);
                baseTimeMs = UserManager.CurrentUser.SavedTime;
            }

            buttons = new Button[size, size];
            this.DoubleBuffered = true;
            InitializeCustomUI();
            UpdateUITexts();
            UpdateUI();
            UpdateStats();

            uiTimer = new Timer { Interval = 40 };
            uiTimer.Tick += (s, e) => { UpdateStats(); };
            uiTimer.Start();
            gameStopwatch.Start();

            autoTimer = new Timer { Interval = 120 };
            autoTimer.Tick += AutoTimer_Tick;
        }

        private void InitializeCustomUI()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = new Size(450, 550);
            this.MinimumSize = new Size(450, 550);
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

            // Трохи звужуємо кнопки, щоб додати третю
            btnPause = new Button { Dock = DockStyle.Right, Width = 80, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(60, 60, 65), ForeColor = Color.White, Font = new Font("Segoe UI", 8, FontStyle.Bold), Cursor = Cursors.Hand };
            btnPause.FlatAppearance.BorderSize = 0;
            btnPause.Click += TogglePause;

            Button btnHint = new Button { Dock = DockStyle.Right, Width = 95, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(204, 122, 0), ForeColor = Color.White, Font = new Font("Segoe UI", 8, FontStyle.Bold), Cursor = Cursors.Hand, Text = "ПІДКАЗКА" };
            btnHint.FlatAppearance.BorderSize = 0;
            btnHint.Click += (s, e) => ShowSmartHint();

            btnAutoSolve = new Button { Dock = DockStyle.Right, Width = 100, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(0, 122, 204), ForeColor = Color.White, Font = new Font("Segoe UI", 8, FontStyle.Bold), Cursor = Cursors.Hand };
            btnAutoSolve.FlatAppearance.BorderSize = 0;
            btnAutoSolve.Click += BtnAutoSolve_Click;

            statsBox = new PictureBox { Dock = DockStyle.Fill };
            statsBox.Paint += (s, e) => {
                using (Font f = new Font("Consolas", 10, FontStyle.Bold))
                {
                    TextRenderer.DrawText(e.Graphics, lastStatsText, f, statsBox.ClientRectangle, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
            };

            statsPanel.Controls.Add(statsBox);
            statsPanel.Controls.Add(btnHint);      // Додали підказку
            statsPanel.Controls.Add(btnAutoSolve);
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

                    buttons[i, j].MouseEnter += (s, e) => {
                        // Правильна умова для Спідрану
                        if (UserManager.IsSpeedrunMode && !isPaused && !isAutoSolving)
                        {
                            Button_Click(s, e);
                        }
                    };

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
            btnAutoSolve.Text = LocalizationManager.Get("AutoSolveBtn");
            lblPauseMsg.Text = LocalizationManager.Get("PauseOverlay");
            UpdateStats();
        }

        private void TogglePause(object sender, EventArgs e)
        {
            if (isAutoSolving) return;

            isPaused = !isPaused;
            if (isPaused)
            {
                gameStopwatch.Stop();
                uiTimer.Stop();
                pausePanel.Visible = true;
                btnPause.Text = LocalizationManager.Get("ResumeBtn");
                btnPause.BackColor = Color.FromArgb(0, 122, 204);
            }
            else
            {
                gameStopwatch.Start();
                uiTimer.Start();
                pausePanel.Visible = false;
                btnPause.Text = LocalizationManager.Get("PauseBtn");
                btnPause.BackColor = Color.FromArgb(60, 60, 65);
            }
        }

        private void BtnAutoSolve_Click(object sender, EventArgs e)
        {
            if (isAutoSolving || isPaused) return;

            if (engine.History.Count == 0)
            {
                CustomMessageForm.Show(LocalizationManager.Get("NoHistory"), LocalizationManager.Get("ErrorTitle"));
                return;
            }

            isAutoSolving = true;
            gameStopwatch.Stop();
            uiTimer.Stop();
            btnPause.Enabled = false;
            btnAutoSolve.BackColor = Color.FromArgb(180, 50, 50);

            autoTimer.Start();
        }

        private void AutoTimer_Tick(object sender, EventArgs e)
        {
            if (engine.History.Count > 0)
            {
                Point lastPos = engine.History[engine.History.Count - 1];
                engine.History.RemoveAt(engine.History.Count - 1);

                engine.ReverseMove(lastPos.X, lastPos.Y);
                UpdateUI();
                UpdateStats();
            }
            else
            {
                autoTimer.Stop();
                CheckVictoryLogic();
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if (isPaused || isAutoSolving) return;

            Button btn = (Button)sender;
            Point p = (Point)btn.Tag;

            if (engine.TryMove(p.X, p.Y))
            {
                UpdateUI();
                UpdateStats();

                if (engine.CheckWin())
                {
                    CheckVictoryLogic();
                }
            }
        }

        private void CheckVictoryLogic()
        {
            gameStopwatch.Stop();
            uiTimer.Stop();
            int totalMs = baseTimeMs + (int)gameStopwatch.ElapsedMilliseconds;
            TimeSpan ts = TimeSpan.FromMilliseconds(totalMs);
            string timeStr = $"{ts.Minutes:D2}:{ts.Seconds:D2}.{ts.Milliseconds / 10:D2}";

            DialogResult result;

            if (isAutoSolving)
            {
                isAutoSolving = false;
                btnPause.Enabled = true;
                btnAutoSolve.BackColor = Color.FromArgb(0, 122, 204);

                if (UserManager.CurrentUser != null)
                {
                    UserManager.CurrentUser.HasSavedGame = false;
                    UserManager.SaveUsers();
                }

                VictoryForm autoSolveWin = new VictoryForm(
                    LocalizationManager.Get("SuccessTitle"),
                    LocalizationManager.Get("AutoSolveComplete")
                );
                result = autoSolveWin.ShowDialog(this);
            }
            else
            {
                if (UserManager.CurrentUser != null)
                {
                    bool recordUpdated = false;
                    if (UserManager.CurrentUser.BestMoves == 0 || engine.Moves < UserManager.CurrentUser.BestMoves)
                    {
                        UserManager.CurrentUser.BestMoves = engine.Moves;
                        recordUpdated = true;
                    }
                    if (UserManager.CurrentUser.BestTime == 0 || totalMs < UserManager.CurrentUser.BestTime)
                    {
                        UserManager.CurrentUser.BestTime = totalMs;
                        recordUpdated = true;
                    }

                    // ЗБЕРЕЖЕННЯ ПЕРЕМОГ ПО РОЗМІРУ ПОЛЯ (РЯДКОВИЙ КЛЮЧ)
                    string sizeKey = size.ToString();

                    if (UserManager.CurrentUser.WinsPerSize == null)
                        UserManager.CurrentUser.WinsPerSize = new Dictionary<string, int>();

                    if (!UserManager.CurrentUser.WinsPerSize.ContainsKey(sizeKey))
                        UserManager.CurrentUser.WinsPerSize[sizeKey] = 0;

                    UserManager.CurrentUser.WinsPerSize[sizeKey]++;
                    recordUpdated = true;

                    UserManager.CurrentUser.HasSavedGame = false;
                    if (recordUpdated) UserManager.SaveUsers();
                }

                VictoryForm humanWin = new VictoryForm(
                    LocalizationManager.Get("VicTitle"),
                    string.Format(LocalizationManager.Get("VicMsg"), timeStr, engine.Moves)
                );
                result = humanWin.ShowDialog(this);
            }

            if (result == DialogResult.Abort)
            {
                this.Close();
            }
            else
            {
                baseTimeMs = 0;
                gameStopwatch.Restart();
                uiTimer.Start();
                engine.StartNewGame();
                UpdateUI();
                UpdateStats();
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

                    // --- ЕЛЕГАНТНИЙ ВІЗУАЛЬНИЙ ЕФЕКТ ---
                    int expectedValue = i * size + j + 1;

                    if (val == expectedValue && val != 0)
                    {
                        // Світліший сірий колір для плиток на своєму місці
                        buttons[i, j].BackColor = Color.FromArgb(145, 145, 150);
                    }
                    else
                    {
                        // Стандартний темніший сірий
                        buttons[i, j].BackColor = Color.FromArgb(60, 60, 65);
                    }
                }
            }
        }
        private async void ShowSmartHint()
        {
            if (isPaused || isAutoSolving) return;

            int emptyR = -1, emptyC = -1;

            // 1. Шукаємо координати порожньої клітинки (нуля)
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (engine.GetValue(i, j) == 0)
                    {
                        emptyR = i;
                        emptyC = j;
                        break;
                    }
                }
            }

            // 2. Аналізуємо 4 можливих напрямки для ходу (вгору, вниз, вліво, вправо)
            int[] dR = { -1, 1, 0, 0 };
            int[] dC = { 0, 0, -1, 1 };

            Point bestTile = new Point(-1, -1);
            int bestDelta = int.MaxValue;

            for (int i = 0; i < 4; i++)
            {
                int nR = emptyR + dR[i];
                int nC = emptyC + dC[i];

                if (nR >= 0 && nR < size && nC >= 0 && nC < size)
                {
                    int val = engine.GetValue(nR, nC);

                    // Де фішка має бути в ідеалі
                    int targetR = (val - 1) / size;
                    int targetC = (val - 1) % size;

                    // Відстань до цілі зараз
                    int oldDist = Math.Abs(nR - targetR) + Math.Abs(nC - targetC);
                    // Відстань до цілі, якщо посунути її на порожнє місце
                    int newDist = Math.Abs(emptyR - targetR) + Math.Abs(emptyC - targetC);

                    // Різниця (чим менше/від'ємніше, тим краще)
                    int delta = newDist - oldDist;

                    if (delta < bestDelta)
                    {
                        bestDelta = delta;
                        bestTile = new Point(nR, nC);
                    }
                }
            }

            // 3. Підсвічуємо найкращу фішку золотим кольором на півсекунди
            if (bestTile.X != -1)
            {
                Button btn = buttons[bestTile.X, bestTile.Y];
                Color oldColor = btn.BackColor;

                btn.BackColor = Color.FromArgb(204, 153, 0); // Золотий колір
                await Task.Delay(400); // Чекаємо 400 мілісекунд

                // Якщо після затримки колір не змінився логікою гри, повертаємо старий
                if (btn.BackColor == Color.FromArgb(204, 153, 0))
                {
                    btn.BackColor = oldColor;
                }
            }
        }
        private void UpdateStats()
        {
            int totalMs = baseTimeMs + (int)gameStopwatch.ElapsedMilliseconds;
            TimeSpan ts = TimeSpan.FromMilliseconds(totalMs);
            string timeStr = $"{ts.Minutes:D2}:{ts.Seconds:D2}.{ts.Milliseconds / 10:D2}";

            string currentText = string.Format(LocalizationManager.Get("StatsFmt"), engine.Moves, timeStr);

            if (currentText != lastStatsText)
            {
                lastStatsText = currentText;
                if (statsBox != null && !statsBox.IsDisposed)
                {
                    statsBox.Invalidate();
                }
            }
        }

        private void StartDrag(object sender, MouseEventArgs e) { if (e.Button == MouseButtons.Left) dragOffset = this.PointToClient(Cursor.Position); }
        private void DoDrag(object sender, MouseEventArgs e) { if (e.Button == MouseButtons.Left) this.Location = new Point(Cursor.Position.X - dragOffset.X, Cursor.Position.Y - dragOffset.Y); }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            autoTimer?.Stop();
            uiTimer?.Stop();
            gameStopwatch?.Stop();
            isAutoSolving = false;

            base.OnFormClosing(e);

            if (UserManager.CurrentUser != null)
            {
                if (!engine.CheckWin())
                {
                    UserManager.CurrentUser.HasSavedGame = true;
                    UserManager.CurrentUser.SavedSize = size;
                    UserManager.CurrentUser.SavedMoves = engine.Moves;
                    UserManager.CurrentUser.SavedTime = baseTimeMs + (int)gameStopwatch.ElapsedMilliseconds;
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