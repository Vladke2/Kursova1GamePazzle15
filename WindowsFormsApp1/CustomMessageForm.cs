using System;
using System.Drawing;
using System.Windows.Forms;

namespace Puzzle15
{
    public class CustomMessageForm : Form
    {
        private Point dragOffset;

        public CustomMessageForm(string message, string title)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = new Size(350, 180);
            this.BackColor = Color.FromArgb(45, 45, 48);
            this.StartPosition = FormStartPosition.CenterParent;

            Panel titleBar = new Panel { Height = 35, Dock = DockStyle.Top, BackColor = Color.FromArgb(30, 30, 30) };
            Label lblTitle = new Label { Text = title, ForeColor = Color.Gray, Font = new Font("Segoe UI", 9), Location = new Point(10, 8), AutoSize = true };
            titleBar.Controls.Add(lblTitle);

            titleBar.MouseDown += StartDrag;
            titleBar.MouseMove += DoDrag;
            lblTitle.MouseDown += StartDrag;
            lblTitle.MouseMove += DoDrag;

            Label lblMessage = new Label
            {
                Text = message,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 85
            };

            Button btnOk = new Button
            {
                Text = LocalizationManager.Get("OkBtn"), // Локалізована кнопка ОК
                Size = new Size(120, 35),
                Location = new Point(115, 130),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnOk.FlatAppearance.BorderSize = 0;
            btnOk.Click += (s, e) => this.Close();

            this.Controls.Add(btnOk);
            this.Controls.Add(lblMessage);
            this.Controls.Add(titleBar);
        }

        private void StartDrag(object sender, MouseEventArgs e) { if (e.Button == MouseButtons.Left) dragOffset = this.PointToClient(Cursor.Position); }
        private void DoDrag(object sender, MouseEventArgs e) { if (e.Button == MouseButtons.Left) this.Location = new Point(Cursor.Position.X - dragOffset.X, Cursor.Position.Y - dragOffset.Y); }

        public static void Show(string message, string title)
        {
            using (CustomMessageForm msgForm = new CustomMessageForm(message, title))
            {
                msgForm.ShowDialog();
            }
        }
    }
}