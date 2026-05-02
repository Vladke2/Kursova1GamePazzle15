using System;
using System.Drawing;
using System.Windows.Forms;

namespace Puzzle15
{
    public class AuthForm : Form
    {
        private Panel titleBar;
        private Label lblTitle;
        private Button btnClose;
        private Label lblName;
        private TextBox txtName;
        private Label lblPass;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnRegister;
        private Point dragOffset;

        public AuthForm()
        {
            InitializeAuthUI();
            UpdateUITexts();
        }

        private void InitializeAuthUI()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = new Size(300, 360);
            this.BackColor = Color.FromArgb(32, 32, 32);
            this.StartPosition = FormStartPosition.CenterParent;

            titleBar = new Panel { Height = 40, Dock = DockStyle.Top, BackColor = Color.FromArgb(45, 45, 48) };
            lblTitle = new Label { ForeColor = Color.White, Location = new Point(10, 10), AutoSize = true, Font = new Font("Segoe UI", 10) };
            btnClose = new Button { Text = "✕", Size = new Size(40, 40), Dock = DockStyle.Right, FlatStyle = FlatStyle.Flat, ForeColor = Color.White };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();

            titleBar.Controls.Add(lblTitle);
            titleBar.Controls.Add(btnClose);
            titleBar.MouseDown += StartDrag;
            titleBar.MouseMove += DoDrag;
            lblTitle.MouseDown += StartDrag;
            lblTitle.MouseMove += DoDrag;

            lblName = new Label { ForeColor = Color.Gray, Location = new Point(40, 60), AutoSize = true, Font = new Font("Segoe UI", 10) };
            txtName = new TextBox { Location = new Point(40, 90), Width = 220, Font = new Font("Segoe UI", 14), BackColor = Color.FromArgb(60, 60, 65), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };

            lblPass = new Label { ForeColor = Color.Gray, Location = new Point(40, 140), AutoSize = true, Font = new Font("Segoe UI", 10) };
            txtPassword = new TextBox { Location = new Point(40, 170), Width = 220, Font = new Font("Segoe UI", 14), BackColor = Color.FromArgb(60, 60, 65), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle, PasswordChar = '•' };

            btnLogin = new Button { Location = new Point(40, 230), Size = new Size(105, 40), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(0, 122, 204), ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold), Cursor = Cursors.Hand };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;

            btnRegister = new Button { Location = new Point(155, 230), Size = new Size(105, 40), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(60, 60, 65), ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold), Cursor = Cursors.Hand };
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Click += BtnRegister_Click;

            this.Controls.AddRange(new Control[] { titleBar, lblName, txtName, lblPass, txtPassword, btnLogin, btnRegister });
        }

        private void UpdateUITexts()
        {
            lblTitle.Text = LocalizationManager.Get("AuthTitle");
            lblName.Text = LocalizationManager.Get("NameLbl");
            lblPass.Text = LocalizationManager.Get("PassLbl");
            btnLogin.Text = LocalizationManager.Get("LoginBtn");
            btnRegister.Text = LocalizationManager.Get("RegBtn");
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                if (UserManager.Login(txtName.Text.Trim(), txtPassword.Text)) this.Close();
                else CustomMessageForm.Show(LocalizationManager.Get("ErrLogin"), LocalizationManager.Get("ErrorTitle"));
            }
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                if (UserManager.Register(txtName.Text.Trim(), txtPassword.Text))
                {
                    CustomMessageForm.Show(LocalizationManager.Get("SuccReg"), LocalizationManager.Get("SuccessTitle"));
                    this.Close();
                }
                else CustomMessageForm.Show(LocalizationManager.Get("ErrRegExists"), LocalizationManager.Get("ErrorTitle"));
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                CustomMessageForm.Show(LocalizationManager.Get("ErrFillFields"), LocalizationManager.Get("ErrorTitle"));
                return false;
            }
            if (txtPassword.Text.Length < 4)
            {
                CustomMessageForm.Show(LocalizationManager.Get("ErrPassLen"), LocalizationManager.Get("ErrorTitle"));
                return false;
            }
            return true;
        }

        private void StartDrag(object sender, MouseEventArgs e) { if (e.Button == MouseButtons.Left) dragOffset = this.PointToClient(Cursor.Position); }
        private void DoDrag(object sender, MouseEventArgs e) { if (e.Button == MouseButtons.Left) this.Location = new Point(Cursor.Position.X - dragOffset.X, Cursor.Position.Y - dragOffset.Y); }
    }
}