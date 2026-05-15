using System.Drawing;

namespace QL_BaiDoXe
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        // ── Controls ────────────────────────────────────────────────
        private MaterialSkin.Controls.MaterialLabel lblAccountType;
        private MaterialSkin.Controls.MaterialLabel lblUsername;
        private MaterialSkin.Controls.MaterialLabel lblPassword;
        private MaterialSkin.Controls.MaterialComboBox cmbAccountType;
        private MaterialSkin.Controls.MaterialTextBox txtUsername;
        private MaterialSkin.Controls.MaterialTextBox txtPassword;
        private MaterialSkin.Controls.MaterialButton btnLogin;
        private MaterialSkin.Controls.MaterialButton btnToggleTheme;
        private System.Windows.Forms.Panel pnlCard;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlCard = new System.Windows.Forms.Panel();
            this.lblAccountType = new MaterialSkin.Controls.MaterialLabel();
            this.cmbAccountType = new MaterialSkin.Controls.MaterialComboBox();
            this.lblUsername = new MaterialSkin.Controls.MaterialLabel();
            this.txtUsername = new MaterialSkin.Controls.MaterialTextBox();
            this.lblPassword = new MaterialSkin.Controls.MaterialLabel();
            this.txtPassword = new MaterialSkin.Controls.MaterialTextBox();
            this.btnLogin = new MaterialSkin.Controls.MaterialButton();
            this.btnToggleTheme = new MaterialSkin.Controls.MaterialButton();

            this.SuspendLayout();

            // ── Form ─────────────────────────────────────────────────
            this.Text = "Quản lý Bãi đỗ xe – Đăng nhập";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ClientSize = new System.Drawing.Size(480, 540);
            this.MinimumSize = new System.Drawing.Size(480, 560);
            this.MaximizeBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            // ── Card Panel ───────────────────────────────────────────
            // MaterialForm title bar cao ~64px, đặt card ngay bên dưới
            this.pnlCard.Size = new System.Drawing.Size(420, 440);
            this.pnlCard.Location = new System.Drawing.Point(30, 72);
            this.pnlCard.BackColor = System.Drawing.Color.Transparent;
            // ── Card Panel (nền trắng bo góc) ──────────────────────
            this.pnlCard.Size = new System.Drawing.Size(420, 480);
            this.pnlCard.Location = new System.Drawing.Point(30, 85);
            this.pnlCard.BackColor = System.Drawing.Color.White;
            // Bo góc mượt bằng Paint event
            this.pnlCard.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                var skinManager = MaterialSkin.MaterialSkinManager.Instance;

                Color cardColor = (skinManager.Theme == MaterialSkin.MaterialSkinManager.Themes.DARK)
                    ? Color.FromArgb(50, 50, 50)  
                    : Color.White;

                using (var path = RoundedRect(this.pnlCard.ClientRectangle, 16))
                using (var brush = new System.Drawing.SolidBrush(cardColor))
                {
                    g.FillPath(brush, path);
                }
            };

            // ── ComboBox: Loại tài khoản ─────────────────────────────
            this.lblAccountType.Text = "Loại tài khoản";
            this.lblAccountType.Font = new System.Drawing.Font("Roboto", 9F);
            this.lblAccountType.AutoSize = true;
            this.lblAccountType.Location = new System.Drawing.Point(20, 10);
            this.lblAccountType.Depth = 0;
            this.lblAccountType.MouseState = MaterialSkin.MouseState.HOVER;

            this.cmbAccountType.Size = new System.Drawing.Size(380, 48);
            this.cmbAccountType.Location = new System.Drawing.Point(20, 30);
            this.cmbAccountType.Hint = "Chọn loại tài khoản";
            this.cmbAccountType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAccountType.Items.AddRange(new object[] { "Nhân viên", "Cư dân" });

            // ── TextBox: Tên đăng nhập ───────────────────────────────
            this.lblUsername.Text = "Tên đăng nhập / Số điện thoại";
            this.lblUsername.Font = new System.Drawing.Font("Roboto", 9F);
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(20, 100);
            this.lblUsername.Depth = 0;
            this.lblUsername.MouseState = MaterialSkin.MouseState.HOVER;

            this.txtUsername.Size = new System.Drawing.Size(380, 48);
            this.txtUsername.Location = new System.Drawing.Point(20, 120);
            this.txtUsername.Hint = "Nhập tên đăng nhập hoặc số điện thoại";
            this.txtUsername.MaxLength = 50;

            // ── TextBox: Mật khẩu ────────────────────────────────────
            this.lblPassword.Text = "Mật khẩu / Số CCCD";
            this.lblPassword.Font = new System.Drawing.Font("Roboto", 9F);
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(20, 190);
            this.lblPassword.Depth = 0;
            this.lblPassword.MouseState = MaterialSkin.MouseState.HOVER;

            this.txtPassword.Size = new System.Drawing.Size(380, 48);
            this.txtPassword.Location = new System.Drawing.Point(20, 210);
            this.txtPassword.Hint = "Nhập mật khẩu hoặc số CCCD";
            this.txtPassword.Password = true;
            this.txtPassword.MaxLength = 255;

            // ── Ghi chú ──────────────────────────────────────────────
            var lblNote = new System.Windows.Forms.Label
            {
                Text = "※ Cư dân: Username = Số ĐT  |  Password = Số CCCD",
                Font = new System.Drawing.Font("Roboto", 8F, System.Drawing.FontStyle.Italic),
                ForeColor = System.Drawing.Color.Gray,
                AutoSize = false,
                Size = new System.Drawing.Size(380, 28),
                Location = new System.Drawing.Point(20, 270),
                BackColor = System.Drawing.Color.Transparent,
            };

            // ── Nút Đăng nhập ────────────────────────────────────────
            this.btnLogin.Text = "ĐĂNG NHẬP";
            this.btnLogin.Size = new System.Drawing.Size(380, 50);
            this.btnLogin.Location = new System.Drawing.Point(20, 310);
            this.btnLogin.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnLogin.UseAccentColor = true;
            this.btnLogin.Font = new System.Drawing.Font("Roboto", 11F, System.Drawing.FontStyle.Bold);
            this.btnLogin.Depth = 0;
            this.btnLogin.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            this.AcceptButton = this.btnLogin;

            // ── Nút Toggle Theme ─────────────────────────────────────
            this.btnToggleTheme.Text = "🌙 Dark Mode";
            this.btnToggleTheme.Size = new System.Drawing.Size(380, 38);
            this.btnToggleTheme.Location = new System.Drawing.Point(20, 375);
            this.btnToggleTheme.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Text;
            this.btnToggleTheme.Depth = 0;
            this.btnToggleTheme.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnToggleTheme.Click += new System.EventHandler(this.btnToggleTheme_Click);

            // ── Thêm vào Card ─────────────────────────────────────────
            this.pnlCard.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.lblAccountType,
                this.cmbAccountType,
                this.lblUsername,
                this.txtUsername,
                this.lblPassword,
                this.txtPassword,
                lblNote,
                this.btnLogin,
                this.btnToggleTheme,
            });

            // ── Thêm vào Form ─────────────────────────────────────────
            this.Controls.Add(this.pnlCard);

            this.ResumeLayout(false);
        }

        private static System.Drawing.Drawing2D.GraphicsPath RoundedRect(
            System.Drawing.Rectangle bounds, int radius)
        {
            int d = radius * 2;
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}