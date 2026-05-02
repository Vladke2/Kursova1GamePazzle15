using System;
using System.Drawing;
using System.Windows.Forms;

namespace Puzzle15
{
    public class VictoryForm : Form
    {
        private Point dragOffset;

        // Тепер конструктор приймає готовий заголовок та текст
        public VictoryForm(string header, string message)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = new Size(350, 220);
            this.BackColor = Color.FromArgb(45, 45, 48);
            this.StartPosition = FormStartPosition.CenterParent;

            Panel titleBar = new Panel { Height = 35, Dock = DockStyle.Top, BackColor = Color.FromArgb(30, 30, 30) };
            Label lblTitle = new Label { Text = header, ForeColor = Color.Gray, Font = new Font("Segoe UI", 9), Location = new Point(10, 8), AutoSize = true };
            titleBar.Controls.Add(lblTitle);

            titleBar.MouseDown += StartDrag;
            titleBar.MouseMove += DoDrag;
            lblTitle.MouseDown += StartDrag;
            lblTitle.MouseMove += DoDrag;

            Label lblMessage = new Label
            {
                Text = message,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 120
            };

            Button btnOk = new Button
            {
                Text = LocalizationManager.Get("ContinueBtn"),
                Size = new Size(140, 40),
                Location = new Point(30, 160),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.OK
            };
            btnOk.FlatAppearance.BorderSize = 0;

            Button btnMenu = new Button
            {
                Text = LocalizationManager.Get("ToMenuBtn"),
                Size = new Size(140, 40),
                Location = new Point(180, 160),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(60, 60, 65),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.Abort
            };
            btnMenu.FlatAppearance.BorderSize = 0;

            this.Controls.Add(btnOk);
            this.Controls.Add(btnMenu);
            this.Controls.Add(lblMessage);
            this.Controls.Add(titleBar);
        }

        private void StartDrag(object sender, MouseEventArgs e) { if (e.Button == MouseButtons.Left) dragOffset = this.PointToClient(Cursor.Position); }
        private void DoDrag(object sender, MouseEventArgs e) { if (e.Button == MouseButtons.Left) this.Location = new Point(Cursor.Position.X - dragOffset.X, Cursor.Position.Y - dragOffset.Y); }
    }
}