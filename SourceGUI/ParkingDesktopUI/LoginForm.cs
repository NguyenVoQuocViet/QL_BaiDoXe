using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;

namespace QL_BaiDoXe
{
    public partial class LoginForm : MaterialForm
    {
        private readonly MaterialSkinManager _skinManager;

        public LoginForm()
        {
            InitializeComponent();

            _skinManager = MaterialSkinManager.Instance;
            _skinManager.AddFormToManage(this);
            _skinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            _skinManager.ColorScheme = new ColorScheme(
                Primary.Blue800,
                Primary.Blue900,
                Primary.Blue500,
                Accent.Blue700,
                TextShade.WHITE
            );
        }

        // ============================================================
        //  NÚT ĐĂNG NHẬP
        // ============================================================
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string accountType = cmbAccountType.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MaterialMessageBox.Show(
                    "Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(accountType))
            {
                MaterialMessageBox.Show(
                    "Vui lòng chọn loại tài khoản!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (accountType == "Nhân viên")
                    LoginAsEmployee(username, password);
                else
                    LoginAsResident(username, password);
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show(
                    "Lỗi kết nối Database:\n" + ex.Message,
                    "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        //  ĐĂNG NHẬP NHÂN VIÊN — so sánh plain text trực tiếp
        // ============================================================
        private void LoginAsEmployee(string username, string password)
        {
            string sql = @"
                SELECT MaNhanVien, HoTen, MaVaiTro, TrangThai
                FROM   NhanVien
                WHERE  TenDangNhap = @username
                  AND  MatKhau     = @password";

            var dt = DatabaseHelper.ExecuteQuery(sql,
                new SqlParameter("@username", SqlDbType.NVarChar) { Value = username },
                new SqlParameter("@password", SqlDbType.NVarChar) { Value = password }
            );

            if (dt.Rows.Count == 0)
            {
                MaterialMessageBox.Show(
                    "Tên đăng nhập hoặc mật khẩu không đúng!",
                    "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataRow nv = dt.Rows[0];

            if (nv["TrangThai"].ToString() != "Đang hoạt động")
            {
                MaterialMessageBox.Show(
                    "Tài khoản đã bị vô hiệu hóa.\nVui lòng liên hệ quản trị viên!",
                    "Tài khoản bị khóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maVaiTro = nv["MaVaiTro"].ToString();
            string hoTen = nv["HoTen"].ToString();
            string maNhanVien = nv["MaNhanVien"].ToString();

            this.Hide();

            // ── Mở MainForm với thông tin nhân viên ──────────────────
            var mainForm = new MainForm(hoTen, maVaiTro, maNhanVien, loginForm: this);
            mainForm.FormClosed += (s, a) =>
            {
                if (!this.Visible)
                    this.Close();
            };
            mainForm.Show();
            this.Hide();
        }

        // ============================================================
        //  ĐĂNG NHẬP CƯ DÂN — Username=SĐT, Password=CCCD
        // ============================================================
        private void LoginAsResident(string username, string password)
        {
            string sql = @"
                SELECT MaCuDan, HoTen, TrangThai
                FROM   CuDan
                WHERE  SoDienThoai = @phone
                  AND  CCCD        = @cccd";

            var dt = DatabaseHelper.ExecuteQuery(sql,
                new SqlParameter("@phone", SqlDbType.VarChar) { Value = username },
                new SqlParameter("@cccd", SqlDbType.VarChar) { Value = password }
            );

            if (dt.Rows.Count == 0)
            {
                MaterialMessageBox.Show(
                    "Số điện thoại hoặc số CCCD không đúng!",
                    "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataRow cd = dt.Rows[0];

            if (cd["TrangThai"].ToString() != "Đang cư trú")
            {
                MaterialMessageBox.Show(
                    "Tài khoản cư dân không còn hoạt động!\nVui lòng liên hệ Ban Quản lý.",
                    "Tài khoản bị khóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Hide();

            // ── Mở MainForm với thông tin cư dân ────────────────────
            string maCuDan = cd["MaCuDan"].ToString();
            string hoTen = cd["HoTen"].ToString();
            var mainForm = new MainForm(hoTen, "CD", maCuDan, loginForm: this);
            mainForm.FormClosed += (s, a) =>
            {
                if (!this.Visible)
                    this.Close();
            };
            mainForm.Show();
            this.Hide();
        }

        // ============================================================
        //  TOGGLE LIGHT / DARK
        // ============================================================
        private void btnToggleTheme_Click(object sender, EventArgs e)
        {
            if (_skinManager.Theme == MaterialSkinManager.Themes.LIGHT)
            {
                _skinManager.Theme = MaterialSkinManager.Themes.DARK;
                btnToggleTheme.Text = "☀ Light Mode";
            }
            else
            {
                _skinManager.Theme = MaterialSkinManager.Themes.LIGHT;
                btnToggleTheme.Text = "🌙 Dark Mode";
            }
            this.pnlCard.Invalidate();
        }
    }
}