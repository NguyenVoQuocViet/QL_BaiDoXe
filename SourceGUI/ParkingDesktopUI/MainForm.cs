using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using LiveCharts;
using LiveCharts.WinForms;
using LiveCharts.Wpf;

namespace QL_BaiDoXe
{
    /// <summary>
    /// Form chính của ứng dụng quản lý bãi đỗ xe.
    /// Quản lý phân quyền (Authorization) dựa trên MaVaiTro (AD/NV/CD)
    /// </summary>
    public partial class MainForm : MaterialForm
    {
        private readonly string _hoTen;
        private readonly string _maVaiTro;
        private readonly string _maUser;
        private readonly Form _loginForm;

        // ============================================================
        //  CONSTRUCTOR
        // ============================================================
        public MainForm(string hoTen, string maVaiTro, string maUser, Form loginForm = null)
        {
            _hoTen = hoTen;
            _maVaiTro = maVaiTro;
            _maUser = maUser;

            InitializeComponent();

            // ── Cấu hình MaterialSkin cho Form ──────────────────────
            var _skinManager = MaterialSkinManager.Instance;
            _skinManager.AddFormToManage(this);
            _skinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            _skinManager.ColorScheme = new ColorScheme(
                Primary.Blue800, Primary.Blue900,
                Primary.Blue500, Accent.Blue700,
                TextShade.WHITE);

            // ── Style tab control ──────────────────────────────────
            StyleTabControl(materialTabControl1);

            Text = "Quản lý Bãi Đỗ Xe - " + _hoTen;

            _loginForm = loginForm;
        }
        private void btnLogout_Click(object sender, EventArgs e)
        {
            var confirm = MaterialMessageBox.Show(
                "Bạn có chắc muốn đăng xuất khỏi hệ thống?",
                "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                if (_loginForm != null && !_loginForm.IsDisposed)
                    _loginForm.Show();
                else
                    new LoginForm().Show();

                this.Close();
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Lỗi khi đăng xuất:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadLichSuGuiData(string bienSo, DataGridView dgv)
        {
            if (string.IsNullOrWhiteSpace(bienSo))
            {
                MaterialMessageBox.Show("Vui lòng nhập biển số xe.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                dgv.DataSource = DatabaseManager.GetLichSuGui(bienSo.Trim());
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Lỗi truy vấn lịch sử gửi xe:\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private TabPage CreateStatisticsRevenueTab__DUPLICATE_PLACEHOLDER()
        {
            var tab = new TabPage("Doanh thu");
            var panel = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(10) };

            var lbl = new Label
            {
                Text = "Báo cáo doanh thu",
                Location = new Point(10, 10),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = true
            };

            var lblTuNgay = new MaterialLabel
            {
                Text = "Từ ngày",
                Location = new Point(10, 50),
                AutoSize = true
            };

            var dtTuNgay = new MaterialDateTimePicker
            {
                Location = new Point(10, 80),
                Width = 220
            };

            var lblDenNgay = new MaterialLabel
            {
                Text = "Đến ngày",
                Location = new Point(250, 50),
                AutoSize = true
            };

            var dtDenNgay = new MaterialDateTimePicker
            {
                Location = new Point(250, 80),
                Width = 220
            };

            var btnThongKe = new MaterialButton
            {
                Text = "Thống kê",
                Location = new Point(490, 80),
                Width = 120,
                Height = 36
            };

            var card = new MaterialCard
            {
                Location = new Point(10, 130),
                Width = 600,
                Height = 200
            };

            var lblTongLuotGuiTitle = new MaterialLabel
            {
                Text = "Tổng lượt gửi:",
                Location = new Point(16, 20),
                AutoSize = true
            };
            var lblTongLuotGui = new MaterialLabel
            {
                Text = "0",
                Location = new Point(220, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };

            var lblTongDoanhThuTitle = new MaterialLabel
            {
                Text = "Tổng doanh thu:",
                Location = new Point(16, 60),
                AutoSize = true
            };
            var lblTongDoanhThu = new MaterialLabel
            {
                Text = "0",
                Location = new Point(220, 60),
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };

            var lblDoanhThuTheNgayTitle = new MaterialLabel
            {
                Text = "Doanh thu thẻ ngày:",
                Location = new Point(16, 100),
                AutoSize = true
            };
            var lblDoanhThuTheNgay = new MaterialLabel
            {
                Text = "0",
                Location = new Point(220, 100),
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };

            var lblLuotTheThangTitle = new MaterialLabel
            {
                Text = "Lượt thẻ tháng:",
                Location = new Point(16, 140),
                AutoSize = true
            };
            var lblLuotTheThang = new MaterialLabel
            {
                Text = "0",
                Location = new Point(220, 140),
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };

            card.Controls.Add(lblTongLuotGuiTitle);
            card.Controls.Add(lblTongLuotGui);
            card.Controls.Add(lblTongDoanhThuTitle);
            card.Controls.Add(lblTongDoanhThu);
            card.Controls.Add(lblDoanhThuTheNgayTitle);
            card.Controls.Add(lblDoanhThuTheNgay);
            card.Controls.Add(lblLuotTheThangTitle);
            card.Controls.Add(lblLuotTheThang);

            btnThongKe.Click += (s, e) =>
            {
                LoadDoanhThuData(dtTuNgay.Value, dtDenNgay.Value, lblTongLuotGui, lblTongDoanhThu, lblDoanhThuTheNgay);
                try
                {
                    var data = DatabaseManager.GetDoanhThu(dtTuNgay.Value, dtDenNgay.Value);
                    if (data.Rows.Count > 0 && data.Columns.Contains("LuotTheThang"))
                    {
                        lblLuotTheThang.Text = data.Rows[0]["LuotTheThang"] != DBNull.Value
                            ? Convert.ToInt32(data.Rows[0]["LuotTheThang"]).ToString("N0")
                            : "0";
                    }
                }
                catch
                {
                    lblLuotTheThang.Text = "0";
                }
            };

            panel.Controls.Add(lbl);
            panel.Controls.Add(lblTuNgay);
            panel.Controls.Add(dtTuNgay);
            panel.Controls.Add(lblDenNgay);
            panel.Controls.Add(dtDenNgay);
            panel.Controls.Add(btnThongKe);
            panel.Controls.Add(card);

            tab.Controls.Add(panel);
            return tab;
        }


        private TabPage CreateQueryHistoryTab()
        {
            var tab = new TabPage("Lịch sử gửi xe");

            // TableLayoutPanel: row 0 = form, row 1 = grid
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
                Padding = new Padding(10)
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 130));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            // ── Form panel ────────────────────────────────────────
            var formPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };

            var lblTitle = new Label
            {
                Text = "🔍 Tra cứu lịch sử gửi xe",
                Location = new Point(0, 0),
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };

            var lblBienSo = new MaterialLabel { Text = "Biển số xe:", Location = new Point(0, 38), AutoSize = true };
            var txtBienSo = new MaterialTextBox2 { Hint = "Nhập biển số xe (vd: 79-H1 111.11)", Location = new Point(85, 35), Width = 230 };

            var lblTuNgay = new MaterialLabel { Text = "Từ ngày:", Location = new Point(330, 38), AutoSize = true };
            var dtTuNgay = new MaterialDateTimePicker { Location = new Point(400, 35), Width = 180 };

            var lblDenNgay = new MaterialLabel { Text = "Đến ngày:", Location = new Point(595, 38), AutoSize = true };
            var dtDenNgay = new MaterialDateTimePicker { Location = new Point(670, 35), Width = 180 };

            var lblLoaiThe = new MaterialLabel { Text = "Loại thẻ:", Location = new Point(865, 38), AutoSize = true };
            var cmbLoaiThe = new ComboBox { Location = new Point(930, 38), Width = 130, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbLoaiThe.Items.AddRange(new[] { "Tất cả", "Thẻ tháng", "Thẻ ngày" });
            cmbLoaiThe.SelectedIndex = 0;

            var lblTrangThai = new MaterialLabel { Text = "Trạng thái:", Location = new Point(0, 88), AutoSize = true };
            var cmbTrangThai = new ComboBox { Location = new Point(85, 88), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbTrangThai.Items.AddRange(new[] { "Tất cả", "Đã ra", "Trong bãi" });
            cmbTrangThai.SelectedIndex = 0;

            var btnTraCuu = new MaterialButton { Text = "Tra cứu", Location = new Point(260, 85), Width = 110, Height = 36 };

            var lblCount = new MaterialLabel { Text = "Kết quả: 0 bản ghi", Location = new Point(390, 92), AutoSize = true, ForeColor = Color.Gray };

            formPanel.Controls.AddRange(new Control[] { lblTitle, lblBienSo, txtBienSo, lblTuNgay, dtTuNgay,
                lblDenNgay, dtDenNgay, lblLoaiThe, cmbLoaiThe, lblTrangThai, cmbTrangThai, btnTraCuu, lblCount });

            // ── Grid ──────────────────────────────────────────────
            var dgv = CreateDockGrid();

            btnTraCuu.Click += (s, e) =>
            {
                try
                {
                    string loaiThe = cmbLoaiThe.SelectedItem?.ToString() == "Tất cả" ? null : cmbLoaiThe.SelectedItem?.ToString();
                    string trangThai = cmbTrangThai.SelectedItem?.ToString() == "Tất cả" ? null : cmbTrangThai.SelectedItem?.ToString();
                    var data = DatabaseManager.GetLichSuGuiNangCao(
                        txtBienSo.Text.Trim(),
                        dtTuNgay.Value.Date,
                        dtDenNgay.Value.Date,
                        loaiThe,
                        trangThai);
                    dgv.DataSource = data;
                    lblCount.Text = $"Kết quả: {data.Rows.Count} bản ghi";
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show("Lỗi tra cứu lịch sử gửi xe:\n" + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            layout.Controls.Add(formPanel, 0, 0);
            layout.Controls.Add(dgv, 0, 1);
            tab.Controls.Add(layout);
            return tab;
        }

        private TabPage CreateQueryEmptySpotTab()
        {
            var tab = new TabPage("Vị trí trống");

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
                Padding = new Padding(10)
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 130));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var formPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };

            var lblTitle = new Label
            {
                Text = "🅿️ Tra cứu vị trí còn trống",
                Location = new Point(0, 0),
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };

            var lblKhu = new MaterialLabel { Text = "Tên khu:", Location = new Point(0, 38), AutoSize = true };
            var txtKhu = new MaterialTextBox2 { Hint = "Ví dụ: A-H1 (để trống = tất cả)", Location = new Point(70, 35), Width = 220 };

            var lblLoai = new MaterialLabel { Text = "Loại vị trí:", Location = new Point(305, 38), AutoSize = true };
            var cmbLoai = new ComboBox { Location = new Point(390, 38), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbLoai.Items.AddRange(new[] { "Xe máy", "Ô tô" });
            cmbLoai.SelectedIndex = 0;

            var lblTang = new MaterialLabel { Text = "Tầng:", Location = new Point(555, 38), AutoSize = true };
            var numTang = new NumericUpDown { Location = new Point(600, 36), Width = 80, Minimum = -5, Maximum = 10, Value = -1 };

            var chkTatCaTang = new CheckBox { Text = "Tất cả tầng", Location = new Point(695, 39), AutoSize = true, Checked = true };
            chkTatCaTang.CheckedChanged += (s, e) => numTang.Enabled = !chkTatCaTang.Checked;
            numTang.Enabled = false;

            var btnTraCuu = new MaterialButton { Text = "Tra cứu", Location = new Point(0, 85), Width = 110, Height = 36 };
            var lblCount = new MaterialLabel { Text = "Kết quả: 0 vị trí trống", Location = new Point(125, 92), AutoSize = true, ForeColor = Color.Gray };

            formPanel.Controls.AddRange(new Control[] { lblTitle, lblKhu, txtKhu, lblLoai, cmbLoai,
                lblTang, numTang, chkTatCaTang, btnTraCuu, lblCount });

            var dgv = CreateDockGrid();

            btnTraCuu.Click += (s, e) =>
            {
                try
                {
                    int? tang = chkTatCaTang.Checked ? (int?)null : (int)numTang.Value;
                    var data = DatabaseManager.GetTraCuuViTriTrongNangCao(txtKhu.Text, cmbLoai.Text, tang);
                    dgv.DataSource = data;
                    lblCount.Text = $"Kết quả: {data.Rows.Count} vị trí trống";
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show("Lỗi tra cứu vị trí trống:\n" + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            layout.Controls.Add(formPanel, 0, 0);
            layout.Controls.Add(dgv, 0, 1);
            tab.Controls.Add(layout);
            return tab;
        }

        private TabPage CreateQueryCardExpiryTab()
        {
            var tab = new TabPage("Hạn thẻ tháng");

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
                Padding = new Padding(10)
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 130));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var formPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };

            var lblTitle = new Label
            {
                Text = "🗓️ Tra cứu hạn thẻ tháng",
                Location = new Point(0, 0),
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };

            var lblMaThe = new MaterialLabel { Text = "Mã thẻ:", Location = new Point(0, 38), AutoSize = true };
            var txtMaThe = new MaterialTextBox2 { Hint = "Nhập mã thẻ (vd: UID001)", Location = new Point(65, 35), Width = 200 };

            var lblBienSo = new MaterialLabel { Text = "Biển số xe:", Location = new Point(280, 38), AutoSize = true };
            var txtBienSo = new MaterialTextBox2 { Hint = "Nhập biển số xe", Location = new Point(360, 35), Width = 200 };

            var lblSapHet = new MaterialLabel { Text = "Sắp hết hạn trong:", Location = new Point(575, 38), AutoSize = true };
            var numNgay = new NumericUpDown { Location = new Point(715, 36), Width = 70, Minimum = 1, Maximum = 365, Value = 30 };
            var lblNgay = new MaterialLabel { Text = "ngày", Location = new Point(792, 38), AutoSize = true };

            var btnKiemTra = new MaterialButton { Text = "Kiểm tra", Location = new Point(0, 85), Width = 110, Height = 36 };
            var btnDsapHet = new MaterialButton { Text = "DS sắp hết hạn", Location = new Point(125, 85), Width = 150, Height = 36 };

            var lblKetQuaTitle = new MaterialLabel { Text = "Ngày còn lại của thẻ đã nhập:", Location = new Point(290, 92), AutoSize = true };
            var lblKetQua = new MaterialLabel { Text = "—", Location = new Point(490, 92), AutoSize = true, Font = new Font("Segoe UI", 11, FontStyle.Bold), ForeColor = Color.DarkBlue };

            formPanel.Controls.AddRange(new Control[] { lblTitle, lblMaThe, txtMaThe, lblBienSo, txtBienSo,
                lblSapHet, numNgay, lblNgay, btnKiemTra, btnDsapHet, lblKetQuaTitle, lblKetQua });

            var dgv = CreateDockGrid();

            btnKiemTra.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtMaThe.Text)) {
                    MaterialMessageBox.Show("Vui lòng nhập mã thẻ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                try
                {
                    int soNgay = DatabaseManager.GetSoNgayConHan(txtMaThe.Text.Trim());
                    lblKetQua.Text = soNgay == 0 ? "Đã hết hạn" : $"{soNgay} ngày";
                    lblKetQua.ForeColor = soNgay <= 7 ? Color.Red : soNgay <= 30 ? Color.Orange : Color.DarkBlue;
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show("Lỗi kiểm tra hạn thẻ:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnDsapHet.Click += (s, e) =>
            {
                try
                {
                    var data = DatabaseManager.GetDanhSachTheSapHetHan((int)numNgay.Value, txtBienSo.Text.Trim());
                    dgv.DataSource = data;
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show("Lỗi tải danh sách thẻ sắp hết hạn:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            layout.Controls.Add(formPanel, 0, 0);
            layout.Controls.Add(dgv, 0, 1);
            tab.Controls.Add(layout);
            return tab;
        }

        private TabPage CreateQueryCurrentParkingTab()
        {
            var tab = new TabPage("Xe trong bãi");

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
                Padding = new Padding(10)
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 130));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var formPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };

            var lblTitle = new Label
            {
                Text = "🚗 Danh sách xe đang trong bãi",
                Location = new Point(0, 0),
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };

            var lblBienSo = new MaterialLabel { Text = "Biển số:", Location = new Point(0, 38), AutoSize = true };
            var txtBienSo = new MaterialTextBox2 { Hint = "Biển số (tùy chọn)", Location = new Point(60, 35), Width = 195 };

            var lblKhu = new MaterialLabel { Text = "Khu:", Location = new Point(268, 38), AutoSize = true };
            var txtKhu = new MaterialTextBox2 { Hint = "Tên khu (tùy chọn)", Location = new Point(300, 35), Width = 190 };

            var lblLoaiThe = new MaterialLabel { Text = "Loại thẻ:", Location = new Point(503, 38), AutoSize = true };
            var cmbLoaiThe = new ComboBox { Location = new Point(567, 38), Width = 130, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbLoaiThe.Items.AddRange(new[] { "Tất cả", "Thẻ tháng", "Thẻ ngày" });
            cmbLoaiThe.SelectedIndex = 0;

            var lblLoaiXe = new MaterialLabel { Text = "Loại xe:", Location = new Point(710, 38), AutoSize = true };
            var cmbLoaiXe = new ComboBox { Location = new Point(766, 38), Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbLoaiXe.Items.AddRange(new[] { "Tất cả", "Xe máy", "Ô tô" });
            cmbLoaiXe.SelectedIndex = 0;

            var btnTai = new MaterialButton { Text = "Tra cứu", Location = new Point(0, 85), Width = 120, Height = 36 };
            var lblCount = new MaterialLabel { Text = "Đang trong bãi: 0 xe", Location = new Point(135, 92), AutoSize = true, ForeColor = Color.DarkGreen, Font = new Font("Segoe UI", 9, FontStyle.Bold) };

            formPanel.Controls.AddRange(new Control[] { lblTitle, lblBienSo, txtBienSo, lblKhu, txtKhu,
                lblLoaiThe, cmbLoaiThe, lblLoaiXe, cmbLoaiXe, btnTai, lblCount });

            var dgv = CreateDockGrid();

            btnTai.Click += (s, e) =>
            {
                try
                {
                    string loaiThe = cmbLoaiThe.SelectedItem?.ToString() == "Tất cả" ? null : cmbLoaiThe.SelectedItem?.ToString();
                    string loaiXe = cmbLoaiXe.SelectedItem?.ToString() == "Tất cả" ? null : cmbLoaiXe.SelectedItem?.ToString();
                    var data = DatabaseManager.GetXeTrongBaiNangCao(txtBienSo.Text, txtKhu.Text, loaiThe, loaiXe);
                    dgv.DataSource = data;
                    lblCount.Text = $"Đang trong bãi: {data.Rows.Count} xe";
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show("Lỗi tải xe trong bãi:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            layout.Controls.Add(formPanel, 0, 0);
            layout.Controls.Add(dgv, 0, 1);
            tab.Controls.Add(layout);
            return tab;
        }

        private TabPage CreateQueryResidentVehicleTab()
        {
            var tab = new TabPage("Cư dân - Xe");

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
                Padding = new Padding(10)
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 130));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var formPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };

            var lblTitle = new Label
            {
                Text = "👤 Tra cứu cư dân và xe sở hữu",
                Location = new Point(0, 0),
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };

            var lblKeyword = new MaterialLabel { Text = "Tìm kiếm:", Location = new Point(0, 38), AutoSize = true };
            var txtKeyword = new MaterialTextBox2 { Hint = "Tên cư dân hoặc biển số xe", Location = new Point(75, 35), Width = 260 };

            var lblLoaiXe = new MaterialLabel { Text = "Loại xe:", Location = new Point(350, 38), AutoSize = true };
            var cmbLoaiXe = new ComboBox { Location = new Point(410, 38), Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbLoaiXe.Items.AddRange(new[] { "Tất cả", "Xe máy", "Ô tô" });
            cmbLoaiXe.SelectedIndex = 0;

            var lblTrangThai = new MaterialLabel { Text = "Trạng thái:", Location = new Point(545, 38), AutoSize = true };
            var cmbTrangThai = new ComboBox { Location = new Point(620, 38), Width = 160, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbTrangThai.Items.AddRange(new[] { "Tất cả", "Đang cư trú", "Tạm trú", "Đã chuyển đi" });
            cmbTrangThai.SelectedIndex = 0;

            var lblToaNha = new MaterialLabel { Text = "Tòa nhà:", Location = new Point(795, 38), AutoSize = true };
            var cmbToaNha = new ComboBox { Location = new Point(855, 38), Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbToaNha.Items.AddRange(new[] { "Tất cả", "Block A", "Block B" });
            cmbToaNha.SelectedIndex = 0;

            var btnTai = new MaterialButton { Text = "Tra cứu", Location = new Point(0, 85), Width = 120, Height = 36 };
            var lblCount = new MaterialLabel { Text = "Kết quả: 0 bản ghi", Location = new Point(135, 92), AutoSize = true, ForeColor = Color.Gray };

            formPanel.Controls.AddRange(new Control[] { lblTitle, lblKeyword, txtKeyword, lblLoaiXe, cmbLoaiXe,
                lblTrangThai, cmbTrangThai, lblToaNha, cmbToaNha, btnTai, lblCount });

            var dgv = CreateDockGrid();

            btnTai.Click += (s, e) =>
            {
                try
                {
                    string loaiXe = cmbLoaiXe.SelectedItem?.ToString() == "Tất cả" ? null : cmbLoaiXe.SelectedItem?.ToString();
                    string trangThai = cmbTrangThai.SelectedItem?.ToString() == "Tất cả" ? null : cmbTrangThai.SelectedItem?.ToString();
                    string toaNha = cmbToaNha.SelectedItem?.ToString() == "Tất cả" ? null : cmbToaNha.SelectedItem?.ToString();
                    var data = DatabaseManager.GetCuDanXeNangCao(txtKeyword.Text, loaiXe, trangThai, toaNha);
                    dgv.DataSource = data;
                    lblCount.Text = $"Kết quả: {data.Rows.Count} bản ghi";
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show("Lỗi tải danh sách cư dân - xe:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            layout.Controls.Add(formPanel, 0, 0);
            layout.Controls.Add(dgv, 0, 1);
            tab.Controls.Add(layout);
            return tab;
        }

        private MaterialDataGridView CreateReportGrid(Point location)
        {
            var dgv = new MaterialDataGridView
            {
                Location = location,
                Width = 1220,
                AutoGenerateColumns = true,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToResizeColumns = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            StyleReportGrid(dgv);
            return dgv;
        }

        /// <summary>Tạo DataGridView mới dùng Dock=Fill, dùng trong TableLayoutPanel</summary>
        private MaterialDataGridView CreateDockGrid()
        {
            var dgv = new MaterialDataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = true,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToResizeColumns = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                ColumnHeadersHeight = 45
            };

            dgv.RowTemplate.Height = 40;

            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dgv.DefaultCellStyle.Padding = new Padding(8, 0, 8, 0);

            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Regular);

            StyleReportGrid(dgv);
            return dgv;
        }

        private static void StyleReportGrid(MaterialDataGridView dgv)
        {
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(21, 101, 192);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10.5f, FontStyle.Bold);
            dgv.EnableHeadersVisualStyles = false;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10f);
            dgv.DefaultCellStyle.Padding = new Padding(3);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(232, 240, 254);
        }

        private void LoadDoanhThuData(DateTime tuNgay, DateTime denNgay,
            MaterialLabel lblTongLuotGui, MaterialLabel lblTongDoanhThu, MaterialLabel lblDoanhThuTheNgay)
        {
            if (tuNgay.Date > denNgay.Date)
            {
                MaterialMessageBox.Show("Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var data = DatabaseManager.GetDoanhThu(tuNgay, denNgay);
                if (data.Rows.Count == 0)
                {
                    lblTongLuotGui.Text = "0";
                    lblTongDoanhThu.Text = "0";
                    lblDoanhThuTheNgay.Text = "0";
                    return;
                }

                var row = data.Rows[0];
                int tongLuotGui = row["TongLuotGui"] != DBNull.Value ? Convert.ToInt32(row["TongLuotGui"]) : 0;
                decimal tongDoanhThu = row["TongDoanhThu"] != DBNull.Value ? Convert.ToDecimal(row["TongDoanhThu"]) : 0;
                decimal doanhThuTheNgay = row["DoanhThuTheNgay"] != DBNull.Value ? Convert.ToDecimal(row["DoanhThuTheNgay"]) : 0;

                lblTongLuotGui.Text = tongLuotGui.ToString("N0");
                lblTongDoanhThu.Text = tongDoanhThu.ToString("N0");
                lblDoanhThuTheNgay.Text = doanhThuTheNgay.ToString("N0");
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Lỗi thống kê doanh thu:\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Áp dụng style xanh + font lớn cho TabControl</summary>
        private void StyleTabControl(System.Windows.Forms.TabControl tc)
        {
            tc.DrawMode = TabDrawMode.OwnerDrawFixed;
            tc.DrawItem += (sender, e) =>
            {
                var tab = tc.TabPages[e.Index];
                bool selected = (tc.SelectedIndex == e.Index);

                // nền
                var bgColor = selected
                    ? Color.FromArgb(21, 101, 192)   // xanh đậm khi chọn
                    : Color.FromArgb(30, 136, 229);   // xanh nhạt hơn
                using (var brush = new SolidBrush(bgColor))
                    e.Graphics.FillRectangle(brush, e.Bounds);

                // viền dưới cho tab đang chọn
                if (selected)
                {
                    using (var pen = new Pen(Color.White, 3))
                        e.Graphics.DrawLine(pen, e.Bounds.Left, e.Bounds.Bottom - 2,
                            e.Bounds.Right, e.Bounds.Bottom - 2);
                }

                // chữ
                var sf = new System.Drawing.StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                using (var brush = new SolidBrush(Color.White))
                    e.Graphics.DrawString(tab.Text,
                        new Font("Segoe UI", 10f, selected ? FontStyle.Bold : FontStyle.Regular),
                        brush, e.Bounds, sf);
            };
        }

        // ============================================================
        //  FORM LOAD - KHỞI TẠO UI THEO QUYỀN
        // ============================================================
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                lblWelcome.Text = $"Xin chào: {_hoTen} ({GetRoleName(_maVaiTro)})";

                // ── Kiểm tra vai trò và khởi tạo tabs ──────────────
                switch (_maVaiTro)
                {
                    case "AD":
                        InitializeAdminTabs();
                        break;
                    case "NV":
                        InitializeStaffTabs();
                        break;
                    case "CD":
                        InitializeResidentTabs();
                        break;
                    default:
                        MaterialMessageBox.Show(
                            "Vai trò không được nhận diện!",
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        break;
                }

                // ── Debug: In ra số tabs ─────────────────────────────
                System.Diagnostics.Debug.WriteLine($"[DEBUG] Số tabs đã tạo: {materialTabControl1.TabPages.Count}");
                string tabList = "";
                foreach (TabPage tab in materialTabControl1.TabPages)
                {
                    System.Diagnostics.Debug.WriteLine($"[DEBUG] Tab: {tab.Text}");
                    tabList += "- " + tab.Text + "\n";
                }

                // Debug info removed for normal use
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show(
                    "Lỗi khởi tạo form:\n" + ex.Message,
                    "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        //  KHỞI TẠO TABS CHO QUẢN TRỊ (ADMIN)
        // ============================================================
        private void InitializeAdminTabs()
        {
            void AddTab(string title, Func<Panel> createPanel)
            {
                try
                {
                    var t = new TabPage(title);
                    t.Controls.Add(createPanel());
                    t.AutoScroll = true;
                    t.BackColor  = Color.White;
                    materialTabControl1.TabPages.Add(t);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[ERR] Tab '{title}': {ex.Message}");
                }
            }

            // ── Thứ tự tabs (Dashboard lên đầu) ──────────────────
            AddTab("Dashboard",    CreateDashboardPanel);
            AddTab("Nhân viên",    CreateEmployeePanel);
            AddTab("Cư dân",       CreateResidentPanel);
            AddTab("Xe",           CreateVehiclePanel);
            AddTab("Thẻ Xe",       CreateCardPanel);
            AddTab("Vị trí đỗ",    CreateParkingSpotPanel);
            AddTab("Loại xe",      CreateVehicleTypePanel);
            AddTab("Khu vực",      CreateZonePanel);
            AddTab("Lượt gửi xe",  CreateParkingSessionPanel);
            AddTab("Vai trò",      CreateVaiTroPanel);
            AddTab("Căn hộ",       CreateCanHoPanel);
            AddTab("CuDan-CanHo",  CreateCuDanCanHoPanel);
            AddTab("Bảng giá",     CreateBangGiaPanel);
            AddTab("Sự cố",        CreateSuCoPanel);
            AddTab("Thanh toán",   CreateThanhToanPanel);
            AddTab("TT Ngày",      CreateThanhToanNgayPanel);
            AddTab("TT Tháng",     CreateThanhToanThangPanel);
            AddTab("LS Thẻ xe",    CreateLichSuTheXePanel);
            AddTab("LS Vị trí",    CreateLichSuViTriDoPanel);

            materialTabControl1.SelectedIndex = 0;
            System.Diagnostics.Debug.WriteLine($"[DEBUG] Total {materialTabControl1.TabPages.Count} admin tabs");
        }

        // ============================================================
        //  KHỞI TẠO TABS CHO NHÂN VIÊN (STAFF)
        // ============================================================
        private void InitializeStaffTabs()
        {
            void AddTab(string title, Func<Panel> createPanel)
            {
                try { var t = new TabPage(title); t.Controls.Add(createPanel()); t.AutoScroll = true; t.BackColor = Color.White; materialTabControl1.TabPages.Add(t); }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[ERR] Tab '{title}': {ex.Message}"); }
            }
            AddTab("Xe",          CreateVehiclePanel);
            AddTab("Thẻ Xe",      CreateCardPanel);
            AddTab("Vị trí đỗ",   CreateParkingSpotPanel);
            AddTab("Lượt gửi xe", CreateParkingSessionPanel);
            AddTab("Dashboard",   CreateDashboardPanel);
            materialTabControl1.SelectedIndex = 0;
        }

        // ============================================================
        //  KHỞI TẠO TABS CHO CƯ DÂN (RESIDENT)
        // ============================================================
        private void InitializeResidentTabs()
        {
            var t = new TabPage("Thông tin của tôi");
            t.Controls.Add(CreateResidentInfoPanel());
            t.AutoScroll = true;
            t.BackColor  = Color.White;
            materialTabControl1.TabPages.Add(t);
            materialTabControl1.SelectedIndex = 0;
        }

        // ============================================================
        //  PANEL CHO NHÂN VIÊN - CREATE EMPLOYEE PANEL
        // ============================================================
        private void ConfigureGrid(DataGridView dgv, bool readOnly = false)
        {
            dgv.AutoGenerateColumns = true;
            dgv.ReadOnly = readOnly;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToResizeColumns = true;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv.Dock = DockStyle.Fill;

            // ── Header cột ───────────────────────────────────────────
            dgv.ColumnHeadersVisible = true;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(21, 101, 192);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10.5f, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(4);

            // ── Row header ẩn ─────────────────────────────────────────
            dgv.RowHeadersVisible = false;

            // ── Selection ────────────────────────────────────────────
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;

            // ── Row style ────────────────────────────────────────────
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10f);
            dgv.DefaultCellStyle.Padding = new Padding(3);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(232, 240, 254);
            dgv.RowTemplate.Height = 28;

            // ── Highlight dòng chọn ──────────────────────────────────
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(100, 181, 246);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
        }

        /// <summary>Tạo panel chuẩn: ToolStrip trên + DataGridView chiếm phần còn lại</summary>
        private Panel MakeGridPanel(ToolStrip toolStrip, DataGridView dgv)
        {
            var tbl = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
                AutoSize = false
            };
            tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 50)); // toolbar taller
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            toolStrip.Dock = DockStyle.Fill;
            toolStrip.AutoSize = false;
            toolStrip.Height = 50;
            toolStrip.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip.BackColor = Color.FromArgb(245, 248, 255);
            toolStrip.Font = new Font("Segoe UI", 10.5f, FontStyle.Bold);

            // Tăng kích thước từng button
            foreach (ToolStripItem item in toolStrip.Items)
            {
                item.Font = new Font("Segoe UI", 10.5f, FontStyle.Bold);
                item.Padding = new Padding(8, 4, 8, 4);
            }

            dgv.Dock = DockStyle.Fill;

            tbl.Controls.Add(toolStrip, 0, 0);
            tbl.Controls.Add(dgv, 0, 1);

            var panel = new Panel { Dock = DockStyle.Fill };
            panel.Controls.Add(tbl);
            return panel;
        }

        private Panel CreateEmployeePanel()
        {
            var toolStrip = new ToolStrip();
            var btnAddNV   = new ToolStripButton("➕ Thêm nhân viên") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnEditNV  = new ToolStripButton("✏️ Sửa")           { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnDelNV   = new ToolStripButton("🗑️ Xóa")           { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnRefresh = new ToolStripButton("🔄 Tải lại")        { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            toolStrip.Items.AddRange(new ToolStripItem[] { btnAddNV, btnEditNV, btnDelNV, btnRefresh });

            var dgv = new DataGridView();
            ConfigureGrid(dgv, readOnly: true);
            LoadEmployeeData(dgv);

            btnRefresh.Click += (s, e) => LoadEmployeeData(dgv);
            btnAddNV.Click   += (s, e) => ShowAddEmployeeDialog(dgv);
            btnEditNV.Click  += (s, e) => ShowEditEmployeeDialog(dgv);
            btnDelNV.Click   += (s, e) => ShowDeleteEmployeeDialog(dgv);

            return MakeGridPanel(toolStrip, dgv);
        }

        /// <summary>Tải danh sách nhân viên vào DataGridView</summary>
        private void LoadEmployeeData(DataGridView dgv)
        {
            try
            {
                dgv.DataSource = DatabaseManager.GetAllNhanVien();
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Lỗi tải dữ liệu nhân viên:\n" + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Hiển thị hộp thoại thêm nhân viên</summary>
        private void ShowAddEmployeeDialog(DataGridView dgv)
        {
            var form = new Form
            {
                Text = "Thêm nhân viên",
                Width = 500,
                Height = 450,
                StartPosition = FormStartPosition.CenterParent
            };

            // ── Tạo các control nhập liệu ──────────────────────────
            var lbl = new Label { Text = "Mã nhân viên:", Location = new Point(10, 20) };
            var txtMa = new TextBox { Location = new Point(150, 20), Width = 300 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtMa);

            lbl = new Label { Text = "Họ tên:", Location = new Point(10, 60) };
            var txtHoTen = new TextBox { Location = new Point(150, 60), Width = 300 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtHoTen);

            lbl = new Label { Text = "Email:", Location = new Point(10, 100) };
            var txtEmail = new TextBox { Location = new Point(150, 100), Width = 300 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtEmail);

            lbl = new Label { Text = "Điện thoại:", Location = new Point(10, 140) };
            var txtPhone = new TextBox { Location = new Point(150, 140), Width = 300 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtPhone);

            lbl = new Label { Text = "Tên đăng nhập:", Location = new Point(10, 180) };
            var txtUsername = new TextBox { Location = new Point(150, 180), Width = 300 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtUsername);

            lbl = new Label { Text = "Mật khẩu:", Location = new Point(10, 220) };
            var txtPassword = new TextBox { Location = new Point(150, 220), Width = 300, UseSystemPasswordChar = true };
            form.Controls.Add(lbl);
            form.Controls.Add(txtPassword);

            var btnOK = new Button { Text = "Lưu", Location = new Point(150, 380), Width = 100 };
            var btnCancel = new Button { Text = "Hủy", Location = new Point(260, 380), Width = 100 };

            btnOK.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtMa.Text) || string.IsNullOrWhiteSpace(txtHoTen.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo");
                    return;
                }

                try
                {
                    int result = DatabaseManager.AddNhanVien(
                        txtMa.Text, txtHoTen.Text, DateTime.Now, "", "Nam", 
                        txtEmail.Text, txtPhone.Text, txtUsername.Text, txtPassword.Text, "NV", "Sáng", 8500000);

                    if (result > 0)
                    {
                        MaterialMessageBox.Show("Thêm nhân viên thành công!", "Thành công", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadEmployeeData(dgv);
                        form.Close();
                    }
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show("Lỗi thêm nhân viên:\n" + ex.Message, "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnCancel.Click += (s, e) => form.Close();
            form.Controls.Add(btnOK);
            form.Controls.Add(btnCancel);

            form.ShowDialog();
        }

        /// <summary>Hiển thị hộp thoại chỉnh sửa nhân viên</summary>
        private void ShowEditEmployeeDialog(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MaterialMessageBox.Show("Vui lòng chọn nhân viên cần sửa!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = dgv.SelectedRows[0];
            string maNhanVien = selectedRow.Cells["MaNhanVien"].Value?.ToString() ?? "";
            string hoTen = selectedRow.Cells["HoTen"].Value?.ToString() ?? "";
            string email = selectedRow.Cells["Email"].Value?.ToString() ?? "";
            string soDienThoai = selectedRow.Cells["SoDienThoai"].Value?.ToString() ?? "";
            string caLamViec = selectedRow.Cells["CaLamViec"].Value?.ToString() ?? "Sáng";
            decimal luong = Convert.ToDecimal(selectedRow.Cells["Luong"].Value ?? 0);
            string trangThai = selectedRow.Cells["TrangThai"].Value?.ToString() ?? "Đang hoạt động";

            var form = new Form
            {
                Text = "Sửa nhân viên",
                Width = 500,
                Height = 400,
                StartPosition = FormStartPosition.CenterParent
            };

            // ── Tạo các control nhập liệu ──────────────────────────
            var lbl = new Label { Text = "Mã nhân viên:", Location = new Point(10, 20) };
            var txtMa = new TextBox { Location = new Point(150, 20), Width = 300, Text = maNhanVien, ReadOnly = true };
            form.Controls.Add(lbl);
            form.Controls.Add(txtMa);

            lbl = new Label { Text = "Họ tên:", Location = new Point(10, 60) };
            var txtHoTen = new TextBox { Location = new Point(150, 60), Width = 300, Text = hoTen };
            form.Controls.Add(lbl);
            form.Controls.Add(txtHoTen);

            lbl = new Label { Text = "Email:", Location = new Point(10, 100) };
            var txtEmail = new TextBox { Location = new Point(150, 100), Width = 300, Text = email };
            form.Controls.Add(lbl);
            form.Controls.Add(txtEmail);

            lbl = new Label { Text = "Điện thoại:", Location = new Point(10, 140) };
            var txtPhone = new TextBox { Location = new Point(150, 140), Width = 300, Text = soDienThoai };
            form.Controls.Add(lbl);
            form.Controls.Add(txtPhone);

            lbl = new Label { Text = "Ca làm việc:", Location = new Point(10, 180) };
            var cmbCa = new ComboBox { Location = new Point(150, 180), Width = 300, Text = caLamViec };
            cmbCa.Items.AddRange(new[] { "Sáng", "Chiều", "Tối" });
            form.Controls.Add(lbl);
            form.Controls.Add(cmbCa);

            lbl = new Label { Text = "Lương:", Location = new Point(10, 220) };
            var txtLuong = new TextBox { Location = new Point(150, 220), Width = 300, Text = luong.ToString() };
            form.Controls.Add(lbl);
            form.Controls.Add(txtLuong);

            lbl = new Label { Text = "Trạng thái:", Location = new Point(10, 260) };
            var cmbTrangThai = new ComboBox { Location = new Point(150, 260), Width = 300, Text = trangThai };
            cmbTrangThai.Items.AddRange(new[] { "Đang hoạt động", "Tạm dừng", "Nghỉ việc" });
            form.Controls.Add(lbl);
            form.Controls.Add(cmbTrangThai);

            var btnOK = new Button { Text = "Lưu", Location = new Point(150, 320), Width = 100 };
            var btnCancel = new Button { Text = "Hủy", Location = new Point(260, 320), Width = 100 };

            btnOK.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtHoTen.Text))
                {
                    MessageBox.Show("Vui lòng nhập họ tên!", "Thông báo");
                    return;
                }

                if (!decimal.TryParse(txtLuong.Text, out decimal newLuong))
                {
                    MessageBox.Show("Lương phải là số!", "Thông báo");
                    return;
                }

                try
                {
                    int result = DatabaseManager.UpdateNhanVien(
                        maNhanVien, txtHoTen.Text, txtEmail.Text, txtPhone.Text,
                        cmbCa.Text, newLuong, cmbTrangThai.Text);

                    if (result > 0)
                    {
                        MaterialMessageBox.Show("Cập nhật nhân viên thành công!", "Thành công", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadEmployeeData(dgv);
                        form.Close();
                    }
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show("Lỗi cập nhật nhân viên:\n" + ex.Message, "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnCancel.Click += (s, e) => form.Close();
            form.Controls.Add(btnOK);
            form.Controls.Add(btnCancel);

            form.ShowDialog();
        }

        /// <summary>Hiển thị hộp thoại xóa nhân viên</summary>
        private void ShowDeleteEmployeeDialog(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MaterialMessageBox.Show("Vui lòng chọn nhân viên cần xóa!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MaterialMessageBox.Show("Bạn có chắc muốn xóa nhân viên này?", "Xác nhận", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                MaterialMessageBox.Show("Chức năng xóa sẽ được triển khai chi tiết.", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // ============================================================
        //  PANEL CHO CƯ DÂN - CREATE RESIDENT PANEL
        // ============================================================
        private Panel CreateResidentPanel()
        {
            var toolStrip = new ToolStrip();
            var btnAdd     = new ToolStripButton("➕ Thêm") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnEdit    = new ToolStripButton("✏️ Sửa")  { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnDel     = new ToolStripButton("🗑️ Xóa")  { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnRefresh = new ToolStripButton("🔄 Tải lại") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            toolStrip.Items.AddRange(new ToolStripItem[] { btnAdd, btnEdit, btnDel, btnRefresh });

            var dgv = new DataGridView();
            ConfigureGrid(dgv, readOnly: true);
            LoadResidentData(dgv);

            btnRefresh.Click += (s, e) => LoadResidentData(dgv);
            btnAdd.Click     += (s, e) => ShowAddResidentDialog(dgv);
            btnEdit.Click    += (s, e) => ShowEditResidentDialog(dgv);
            btnDel.Click     += (s, e) => ShowDeleteResidentDialog(dgv);

            return MakeGridPanel(toolStrip, dgv);
        }

        private void ShowAddResidentDialog(DataGridView dgv)
        {
            var form = new Form
            {
                Text = "Thêm cư dân",
                Width = 500,
                Height = 350,
                StartPosition = FormStartPosition.CenterParent
            };

            var lbl = new Label { Text = "Mã cư dân:", Location = new Point(10, 20) };
            var txtMa = new TextBox { Location = new Point(150, 20), Width = 300 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtMa);

            lbl = new Label { Text = "Họ tên:", Location = new Point(10, 60) };
            var txtHoTen = new TextBox { Location = new Point(150, 60), Width = 300 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtHoTen);

            lbl = new Label { Text = "CCCD:", Location = new Point(10, 100) };
            var txtCCCD = new TextBox { Location = new Point(150, 100), Width = 300 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtCCCD);

            lbl = new Label { Text = "Email:", Location = new Point(10, 140) };
            var txtEmail = new TextBox { Location = new Point(150, 140), Width = 300 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtEmail);

            lbl = new Label { Text = "Điện thoại:", Location = new Point(10, 180) };
            var txtPhone = new TextBox { Location = new Point(150, 180), Width = 300 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtPhone);

            lbl = new Label { Text = "Địa chỉ căn hộ:", Location = new Point(10, 220) };
            var txtDiaChi = new TextBox { Location = new Point(150, 220), Width = 300 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtDiaChi);

            var btnOK = new Button { Text = "Lưu", Location = new Point(150, 260), Width = 100 };
            var btnCancel = new Button { Text = "Hủy", Location = new Point(260, 260), Width = 100 };

            btnOK.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtMa.Text) || string.IsNullOrWhiteSpace(txtHoTen.Text) || string.IsNullOrWhiteSpace(txtCCCD.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo");
                    return;
                }

                try
                {
                    int result = DatabaseManager.AddCuDan(txtMa.Text, txtHoTen.Text, txtCCCD.Text,
                        txtEmail.Text, txtPhone.Text, txtDiaChi.Text);

                    if (result > 0)
                    {
                        MaterialMessageBox.Show("Thêm cư dân thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadResidentData(dgv);
                        form.Close();
                    }
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show("Lỗi thêm cư dân:\n" + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnCancel.Click += (s, e) => form.Close();
            form.Controls.Add(btnOK);
            form.Controls.Add(btnCancel);

            form.ShowDialog();
        }

        private void ShowEditResidentDialog(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MaterialMessageBox.Show("Vui lòng chọn cư dân cần sửa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dgv.SelectedRows[0];
            string maCuDan = row.Cells["MaCuDan"].Value?.ToString() ?? "";
            string hoTen = row.Cells["HoTen"].Value?.ToString() ?? "";
            string email = row.Cells["Email"].Value?.ToString() ?? "";
            string soDienThoai = row.Cells["SoDienThoai"].Value?.ToString() ?? "";
            string trangThai = row.Cells["TrangThai"].Value?.ToString() ?? "Đang cư trú";

            var form = new Form
            {
                Text = "Sửa cư dân",
                Width = 500,
                Height = 300,
                StartPosition = FormStartPosition.CenterParent
            };

            var lbl = new Label { Text = "Mã cư dân:", Location = new Point(10, 20) };
            var txtMa = new TextBox { Location = new Point(150, 20), Width = 300, Text = maCuDan, ReadOnly = true };
            form.Controls.Add(lbl);
            form.Controls.Add(txtMa);

            lbl = new Label { Text = "Họ tên:", Location = new Point(10, 60) };
            var txtHoTen = new TextBox { Location = new Point(150, 60), Width = 300, Text = hoTen };
            form.Controls.Add(lbl);
            form.Controls.Add(txtHoTen);

            lbl = new Label { Text = "Email:", Location = new Point(10, 100) };
            var txtEmail = new TextBox { Location = new Point(150, 100), Width = 300, Text = email };
            form.Controls.Add(lbl);
            form.Controls.Add(txtEmail);

            lbl = new Label { Text = "Điện thoại:", Location = new Point(10, 140) };
            var txtPhone = new TextBox { Location = new Point(150, 140), Width = 300, Text = soDienThoai };
            form.Controls.Add(lbl);
            form.Controls.Add(txtPhone);

            lbl = new Label { Text = "Trạng thái:", Location = new Point(10, 180) };
            var cmbTrangThai = new ComboBox { Location = new Point(150, 180), Width = 300, Text = trangThai };
            cmbTrangThai.Items.AddRange(new[] { "Đang cư trú", "Tạm trú", "Đã chuyển đi" });
            form.Controls.Add(lbl);
            form.Controls.Add(cmbTrangThai);

            var btnOK = new Button { Text = "Lưu", Location = new Point(150, 220), Width = 100 };
            var btnCancel = new Button { Text = "Hủy", Location = new Point(260, 220), Width = 100 };

            btnOK.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtHoTen.Text))
                {
                    MessageBox.Show("Vui lòng nhập họ tên!", "Thông báo");
                    return;
                }

                try
                {
                    int result = DatabaseManager.UpdateCuDan(maCuDan, txtHoTen.Text, txtEmail.Text,
                        txtPhone.Text, cmbTrangThai.Text);

                    if (result > 0)
                    {
                        MaterialMessageBox.Show("Cập nhật cư dân thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadResidentData(dgv);
                        form.Close();
                    }
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show("Lỗi cập nhật cư dân:\n" + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnCancel.Click += (s, e) => form.Close();
            form.Controls.Add(btnOK);
            form.Controls.Add(btnCancel);

            form.ShowDialog();
        }

        private void ShowDeleteResidentDialog(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MaterialMessageBox.Show("Vui lòng chọn cư dân cần xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dgv.SelectedRows[0];
            string maCuDan = row.Cells["MaCuDan"].Value?.ToString() ?? "";

            if (MaterialMessageBox.Show("Xác nhận xóa cư dân này?", "Xác nhận",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                return;

            try
            {
                int result = DatabaseManager.DeleteCuDan(maCuDan);
                if (result > 0)
                {
                    MaterialMessageBox.Show("Xóa cư dân thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadResidentData(dgv);
                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Lỗi xóa cư dân:\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadResidentData(DataGridView dgv)
        {
            try
            {
                dgv.DataSource = DatabaseManager.GetAllCuDan();
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Lỗi tải dữ liệu cư dân:\n" + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        //  PANEL CHO XE - CREATE VEHICLE PANEL
        // ============================================================
        private Panel CreateVehiclePanel()
        {
            var toolStrip = new ToolStrip();
            var btnAdd     = new ToolStripButton("➕ Thêm xe") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnEdit    = new ToolStripButton("✏️ Sửa")     { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnDel     = new ToolStripButton("🗑️ Xóa")     { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnRefresh = new ToolStripButton("🔄 Tải lại")  { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            toolStrip.Items.AddRange(new ToolStripItem[] { btnAdd, btnEdit, btnDel, btnRefresh });

            var dgv = new DataGridView();
            ConfigureGrid(dgv, readOnly: true);
            LoadVehicleData(dgv);

            btnRefresh.Click += (s, e) => LoadVehicleData(dgv);
            btnAdd.Click     += (s, e) => ShowAddVehicleDialog(dgv);
            btnEdit.Click    += (s, e) => ShowEditVehicleDialog(dgv);
            btnDel.Click     += (s, e) => ShowDeleteVehicleDialog(dgv);

            return MakeGridPanel(toolStrip, dgv);
        }

        private void ShowAddVehicleDialog(DataGridView dgv)
        {
            var form = new Form
            {
                Text = "Thêm xe",
                Width = 520,
                Height = 420,
                StartPosition = FormStartPosition.CenterParent
            };

            var lbl = new Label { Text = "Mã xe:", Location = new Point(10, 20) };
            var txtMa = new TextBox { Location = new Point(150, 20), Width = 320 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtMa);

            lbl = new Label { Text = "Biển số:", Location = new Point(10, 60) };
            var txtBienSo = new TextBox { Location = new Point(150, 60), Width = 320 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtBienSo);

            lbl = new Label { Text = "Hãng xe:", Location = new Point(10, 100) };
            var txtHang = new TextBox { Location = new Point(150, 100), Width = 320 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtHang);

            lbl = new Label { Text = "Dòng xe:", Location = new Point(10, 140) };
            var txtDong = new TextBox { Location = new Point(150, 140), Width = 320 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtDong);

            lbl = new Label { Text = "Màu xe:", Location = new Point(10, 180) };
            var txtMau = new TextBox { Location = new Point(150, 180), Width = 320 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtMau);

            lbl = new Label { Text = "Năm SX:", Location = new Point(10, 220) };
            var txtNam = new TextBox { Location = new Point(150, 220), Width = 320 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtNam);

            lbl = new Label { Text = "Mã loại xe:", Location = new Point(10, 260) };
            var txtMaLoai = new TextBox { Location = new Point(150, 260), Width = 320 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtMaLoai);

            lbl = new Label { Text = "Mã cư dân:", Location = new Point(10, 300) };
            var txtMaCuDan = new TextBox { Location = new Point(150, 300), Width = 320 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtMaCuDan);

            var btnOK = new Button { Text = "Lưu", Location = new Point(150, 340), Width = 100 };
            var btnCancel = new Button { Text = "Hủy", Location = new Point(260, 340), Width = 100 };

            btnOK.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtMa.Text) || string.IsNullOrWhiteSpace(txtBienSo.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã xe và biển số!", "Thông báo");
                    return;
                }

                if (!int.TryParse(txtNam.Text, out int nam))
                    nam = DateTime.Now.Year;

                try
                {
                    int result = DatabaseManager.AddXe(txtMa.Text, txtBienSo.Text, txtHang.Text,
                        txtDong.Text, txtMau.Text, "", "", nam, txtMaLoai.Text, txtMaCuDan.Text);

                    if (result > 0)
                    {
                        MaterialMessageBox.Show("Thêm xe thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadVehicleData(dgv);
                        form.Close();
                    }
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show("Lỗi thêm xe:\n" + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnCancel.Click += (s, e) => form.Close();
            form.Controls.Add(btnOK);
            form.Controls.Add(btnCancel);

            form.ShowDialog();
        }

        private void ShowEditVehicleDialog(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MaterialMessageBox.Show("Vui lòng chọn xe cần sửa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dgv.SelectedRows[0];
            string maXe      = row.Cells["MaXe"].Value?.ToString()      ?? "";
            string hangXe    = row.Cells["HangXe"].Value?.ToString()    ?? "";
            string tenDongXe = row.Cells["TenDongXe"].Value?.ToString() ?? "";
            string mauXe     = row.Cells["MauXe"].Value?.ToString()     ?? "";
            string maLoaiXe  = row.Cells["MaLoaiXe"].Value?.ToString()  ?? "";   // ← dùng MaLoaiXe
            string trangThai = row.Cells["TrangThai"].Value?.ToString()  ?? "Đang sử dụng";

            var form = new Form
            {
                Text = "Sửa xe",
                Width = 520,
                Height = 320,
                StartPosition = FormStartPosition.CenterParent
            };

            var lbl = new Label { Text = "Mã xe:", Location = new Point(10, 20) };
            var txtMa = new TextBox { Location = new Point(150, 20), Width = 320, Text = maXe, ReadOnly = true };
            form.Controls.Add(lbl);
            form.Controls.Add(txtMa);

            lbl = new Label { Text = "Hãng xe:", Location = new Point(10, 60) };
            var txtHang = new TextBox { Location = new Point(150, 60), Width = 320, Text = hangXe };
            form.Controls.Add(lbl);
            form.Controls.Add(txtHang);

            lbl = new Label { Text = "Dòng xe:", Location = new Point(10, 100) };
            var txtDong = new TextBox { Location = new Point(150, 100), Width = 320, Text = tenDongXe };
            form.Controls.Add(lbl);
            form.Controls.Add(txtDong);

            lbl = new Label { Text = "Màu xe:", Location = new Point(10, 140) };
            var txtMau = new TextBox { Location = new Point(150, 140), Width = 320, Text = mauXe };
            form.Controls.Add(lbl);
            form.Controls.Add(txtMau);

            lbl = new Label { Text = "Mã loại xe:", Location = new Point(10, 180) };
            var txtMaLoai = new TextBox { Location = new Point(150, 180), Width = 320, Text = maLoaiXe };
            form.Controls.Add(lbl);
            form.Controls.Add(txtMaLoai);

            lbl = new Label { Text = "Trạng thái:", Location = new Point(10, 220) };
            var cmbTrangThai = new ComboBox { Location = new Point(150, 220), Width = 320, Text = trangThai };
            cmbTrangThai.Items.AddRange(new[] { "Đang sử dụng", "Tạm dừng", "Không sử dụng" });
            form.Controls.Add(lbl);
            form.Controls.Add(cmbTrangThai);

            var btnOK = new Button { Text = "Lưu", Location = new Point(150, 260), Width = 100 };
            var btnCancel = new Button { Text = "Hủy", Location = new Point(260, 260), Width = 100 };

            btnOK.Click += (s, e) =>
            {
                try
                {
                    int result = DatabaseManager.UpdateXe(maXe, txtHang.Text, txtDong.Text,
                        txtMau.Text, txtMaLoai.Text, cmbTrangThai.Text);

                    if (result > 0)
                    {
                        MaterialMessageBox.Show("Cập nhật xe thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadVehicleData(dgv);
                        form.Close();
                    }
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show("Lỗi cập nhật xe:\n" + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnCancel.Click += (s, e) => form.Close();
            form.Controls.Add(btnOK);
            form.Controls.Add(btnCancel);

            form.ShowDialog();
        }

        private void ShowDeleteVehicleDialog(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MaterialMessageBox.Show("Vui lòng chọn xe cần xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dgv.SelectedRows[0];
            string maXe = row.Cells["MaXe"].Value?.ToString() ?? "";

            if (MaterialMessageBox.Show("Xác nhận xóa xe này?", "Xác nhận",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                return;

            try
            {
                int result = DatabaseManager.DeleteXe(maXe);
                if (result > 0)
                {
                    MaterialMessageBox.Show("Xóa xe thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadVehicleData(dgv);
                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Lỗi xóa xe:\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadVehicleData(DataGridView dgv)
        {
            try
            {
                dgv.DataSource = DatabaseManager.GetAllXe();
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Lỗi tải dữ liệu xe:\n" + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        //  PANEL CHO THẺ XE - CREATE CARD PANEL
        // ============================================================
        private Panel CreateCardPanel()
        {
            var toolStrip = new ToolStrip();
            var btnAdd     = new ToolStripButton("➕ Thêm") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnEdit    = new ToolStripButton("✏️ Sửa")  { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnDel     = new ToolStripButton("🗑️ Xóa")  { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnRefresh = new ToolStripButton("🔄 Tải lại") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            toolStrip.Items.AddRange(new ToolStripItem[] { btnAdd, btnEdit, btnDel, btnRefresh });

            var dgv = new DataGridView();
            ConfigureGrid(dgv, readOnly: true);
            LoadCardData(dgv);

            btnRefresh.Click += (s, e) => LoadCardData(dgv);
            btnAdd.Click     += (s, e) => ShowAddCardDialog(dgv);
            btnEdit.Click    += (s, e) => ShowEditCardDialog(dgv);
            btnDel.Click     += (s, e) => ShowDeleteCardDialog(dgv);

            return MakeGridPanel(toolStrip, dgv);
        }

        private void ShowAddCardDialog(DataGridView dgv)
        {
            var form = new Form
            {
                Text = "Thêm thẻ xe",
                Width = 520,
                Height = 320,
                StartPosition = FormStartPosition.CenterParent
            };

            var lbl = new Label { Text = "Mã thẻ:", Location = new Point(10, 20) };
            var txtMa = new TextBox { Location = new Point(150, 20), Width = 320 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtMa);

            lbl = new Label { Text = "Số thẻ:", Location = new Point(10, 60) };
            var txtSoThe = new TextBox { Location = new Point(150, 60), Width = 320 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtSoThe);

            lbl = new Label { Text = "Mã xe:", Location = new Point(10, 100) };
            var txtMaXe = new TextBox { Location = new Point(150, 100), Width = 320 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtMaXe);

            lbl = new Label { Text = "Loại thẻ:", Location = new Point(10, 140) };
            var txtLoaiThe = new TextBox { Location = new Point(150, 140), Width = 320 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtLoaiThe);

            lbl = new Label { Text = "Ngày hết hạn:", Location = new Point(10, 180) };
            var dtHetHan = new DateTimePicker { Location = new Point(150, 180), Width = 320, Format = DateTimePickerFormat.Short };
            form.Controls.Add(lbl);
            form.Controls.Add(dtHetHan);

            lbl = new Label { Text = "Tiền cọc:", Location = new Point(10, 220) };
            var txtTienCoc = new TextBox { Location = new Point(150, 220), Width = 320 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtTienCoc);

            var btnOK = new Button { Text = "Lưu", Location = new Point(150, 260), Width = 100 };
            var btnCancel = new Button { Text = "Hủy", Location = new Point(260, 260), Width = 100 };

            btnOK.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtMa.Text) || string.IsNullOrWhiteSpace(txtLoaiThe.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã thẻ và loại thẻ!", "Thông báo");
                    return;
                }

                if (!decimal.TryParse(txtTienCoc.Text, out decimal tienCoc))
                    tienCoc = 0;

                try
                {
                    int result = DatabaseManager.AddTheXe(txtMa.Text, txtSoThe.Text, txtMaXe.Text,
                        txtLoaiThe.Text, dtHetHan.Value.Date, tienCoc);

                    if (result > 0)
                    {
                        MaterialMessageBox.Show("Thêm thẻ xe thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCardData(dgv);
                        form.Close();
                    }
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show("Lỗi thêm thẻ xe:\n" + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnCancel.Click += (s, e) => form.Close();
            form.Controls.Add(btnOK);
            form.Controls.Add(btnCancel);

            form.ShowDialog();
        }

        private void ShowEditCardDialog(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MaterialMessageBox.Show("Vui lòng chọn thẻ xe cần sửa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dgv.SelectedRows[0];
            string maThe = row.Cells["MaThe"].Value?.ToString() ?? "";
            string maXe = row.Cells["MaXe"].Value?.ToString() ?? "";
            string trangThai = row.Cells["TrangThai"].Value?.ToString() ?? "Đang hoạt động";

            var form = new Form
            {
                Text = "Sửa thẻ xe",
                Width = 500,
                Height = 240,
                StartPosition = FormStartPosition.CenterParent
            };

            var lbl = new Label { Text = "Mã thẻ:", Location = new Point(10, 20) };
            var txtMa = new TextBox { Location = new Point(150, 20), Width = 300, Text = maThe, ReadOnly = true };
            form.Controls.Add(lbl);
            form.Controls.Add(txtMa);

            lbl = new Label { Text = "Mã xe:", Location = new Point(10, 60) };
            var txtMaXe = new TextBox { Location = new Point(150, 60), Width = 300, Text = maXe };
            form.Controls.Add(lbl);
            form.Controls.Add(txtMaXe);

            lbl = new Label { Text = "Ngày hết hạn:", Location = new Point(10, 100) };
            var dtHetHan = new DateTimePicker { Location = new Point(150, 100), Width = 300, Format = DateTimePickerFormat.Short };
            form.Controls.Add(lbl);
            form.Controls.Add(dtHetHan);

            lbl = new Label { Text = "Trạng thái:", Location = new Point(10, 140) };
            var cmbTrangThai = new ComboBox { Location = new Point(150, 140), Width = 300, Text = trangThai };
            cmbTrangThai.Items.AddRange(new[] { "Đang hoạt động", "Tạm dừng", "Hết hạn" });
            form.Controls.Add(lbl);
            form.Controls.Add(cmbTrangThai);

            var btnOK = new Button { Text = "Lưu", Location = new Point(150, 180), Width = 100 };
            var btnCancel = new Button { Text = "Hủy", Location = new Point(260, 180), Width = 100 };

            btnOK.Click += (s, e) =>
            {
                try
                {
                    int result = DatabaseManager.UpdateTheXe(maThe, txtMaXe.Text, dtHetHan.Value.Date, cmbTrangThai.Text);

                    if (result > 0)
                    {
                        MaterialMessageBox.Show("Cập nhật thẻ xe thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCardData(dgv);
                        form.Close();
                    }
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show("Lỗi cập nhật thẻ xe:\n" + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnCancel.Click += (s, e) => form.Close();
            form.Controls.Add(btnOK);
            form.Controls.Add(btnCancel);

            form.ShowDialog();
        }

        private void ShowDeleteCardDialog(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MaterialMessageBox.Show("Vui lòng chọn thẻ xe cần xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dgv.SelectedRows[0];
            string maThe = row.Cells["MaThe"].Value?.ToString() ?? "";

            if (MaterialMessageBox.Show("Xác nhận xóa thẻ xe này?", "Xác nhận",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                return;

            try
            {
                int result = DatabaseManager.DeleteTheXe(maThe);
                if (result > 0)
                {
                    MaterialMessageBox.Show("Xóa thẻ xe thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCardData(dgv);
                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Lỗi xóa thẻ xe:\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCardData(DataGridView dgv)
        {
            try
            {
                dgv.DataSource = DatabaseManager.GetAllTheXe();
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Lỗi tải dữ liệu thẻ xe:\n" + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        //  PANEL CHO VỊ TRÍ ĐỖ - CREATE PARKING SPOT PANEL
        // ============================================================
        private Panel CreateParkingSpotPanel()
        {
            var toolStrip = new ToolStrip();
            var btnAdd     = new ToolStripButton("➕ Thêm") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnEdit    = new ToolStripButton("✏️ Sửa")  { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnDel     = new ToolStripButton("🗑️ Xóa")  { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnRefresh = new ToolStripButton("🔄 Tải lại") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            toolStrip.Items.AddRange(new ToolStripItem[] { btnAdd, btnEdit, btnDel, btnRefresh });

            var dgv = new DataGridView();
            ConfigureGrid(dgv, readOnly: true);
            LoadParkingSpotData(dgv);

            btnRefresh.Click += (s, e) => LoadParkingSpotData(dgv);
            btnAdd.Click     += (s, e) => ShowAddParkingSpotDialog(dgv);
            btnEdit.Click    += (s, e) => ShowEditParkingSpotDialog(dgv);
            btnDel.Click     += (s, e) => ShowDeleteParkingSpotDialog(dgv);

            return MakeGridPanel(toolStrip, dgv);
        }

        private void ShowAddParkingSpotDialog(DataGridView dgv)
        {
            var form = new Form
            {
                Text = "Thêm vị trí đỗ",
                Width = 500,
                Height = 300,
                StartPosition = FormStartPosition.CenterParent
            };

            var lbl = new Label { Text = "Mã vị trí:", Location = new Point(10, 20) };
            var txtMa = new TextBox { Location = new Point(150, 20), Width = 300 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtMa);

            lbl = new Label { Text = "Mã khu:", Location = new Point(10, 60) };
            var txtMaKhu = new TextBox { Location = new Point(150, 60), Width = 300 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtMaKhu);

            lbl = new Label { Text = "Tên vị trí:", Location = new Point(10, 100) };
            var txtTen = new TextBox { Location = new Point(150, 100), Width = 300 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtTen);

            lbl = new Label { Text = "Loại vị trí:", Location = new Point(10, 140) };
            var txtLoai = new TextBox { Location = new Point(150, 140), Width = 300 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtLoai);

            lbl = new Label { Text = "Sức chứa:", Location = new Point(10, 180) };
            var txtSucChua = new TextBox { Location = new Point(150, 180), Width = 300 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtSucChua);

            var btnOK = new Button { Text = "Lưu", Location = new Point(150, 220), Width = 100 };
            var btnCancel = new Button { Text = "Hủy", Location = new Point(260, 220), Width = 100 };

            btnOK.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtMa.Text) || string.IsNullOrWhiteSpace(txtTen.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã và tên vị trí!", "Thông báo");
                    return;
                }

                if (!int.TryParse(txtSucChua.Text, out int sucChua))
                    sucChua = 1;

                try
                {
                    int result = DatabaseManager.AddViTriDo(txtMa.Text, txtMaKhu.Text, txtTen.Text, txtLoai.Text, sucChua);
                    if (result > 0)
                    {
                        MaterialMessageBox.Show("Thêm vị trí đỗ thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadParkingSpotData(dgv);
                        form.Close();
                    }
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show("Lỗi thêm vị trí đỗ:\n" + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnCancel.Click += (s, e) => form.Close();
            form.Controls.Add(btnOK);
            form.Controls.Add(btnCancel);

            form.ShowDialog();
        }

        private void ShowEditParkingSpotDialog(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MaterialMessageBox.Show("Vui lòng chọn vị trí cần sửa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dgv.SelectedRows[0];
            string maViTri = row.Cells["MaViTri"].Value?.ToString() ?? "";
            int trangThai = row.Cells["TrangThai"].Value != null && row.Cells["TrangThai"].Value.ToString() == "True" ? 1 : 0;

            var form = new Form
            {
                Text = "Sửa vị trí đỗ",
                Width = 400,
                Height = 200,
                StartPosition = FormStartPosition.CenterParent
            };

            var lbl = new Label { Text = "Mã vị trí:", Location = new Point(10, 20) };
            var txtMa = new TextBox { Location = new Point(120, 20), Width = 240, Text = maViTri, ReadOnly = true };
            form.Controls.Add(lbl);
            form.Controls.Add(txtMa);

            lbl = new Label { Text = "Trạng thái:", Location = new Point(10, 60) };
            var cmbTrangThai = new ComboBox { Location = new Point(120, 60), Width = 240 };
            cmbTrangThai.Items.AddRange(new[] { "0", "1" });
            cmbTrangThai.Text = trangThai.ToString();
            form.Controls.Add(lbl);
            form.Controls.Add(cmbTrangThai);

            var btnOK = new Button { Text = "Lưu", Location = new Point(120, 110), Width = 100 };
            var btnCancel = new Button { Text = "Hủy", Location = new Point(230, 110), Width = 100 };

            btnOK.Click += (s, e) =>
            {
                if (!int.TryParse(cmbTrangThai.Text, out int status))
                    status = 0;

                try
                {
                    int result = DatabaseManager.UpdateViTriDo(maViTri, status);
                    if (result > 0)
                    {
                        MaterialMessageBox.Show("Cập nhật vị trí đỗ thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadParkingSpotData(dgv);
                        form.Close();
                    }
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show("Lỗi cập nhật vị trí đỗ:\n" + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnCancel.Click += (s, e) => form.Close();
            form.Controls.Add(btnOK);
            form.Controls.Add(btnCancel);

            form.ShowDialog();
        }

        private void ShowDeleteParkingSpotDialog(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MaterialMessageBox.Show("Vui lòng chọn vị trí cần xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dgv.SelectedRows[0];
            string maViTri = row.Cells["MaViTri"].Value?.ToString() ?? "";

            if (MaterialMessageBox.Show("Xác nhận xóa vị trí đỗ này?", "Xác nhận",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                return;

            try
            {
                int result = DatabaseManager.DeleteViTriDo(maViTri);
                if (result > 0)
                {
                    MaterialMessageBox.Show("Xóa vị trí đỗ thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadParkingSpotData(dgv);
                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Lỗi xóa vị trí đỗ:\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadParkingSpotData(DataGridView dgv)
        {
            try
            {
                dgv.DataSource = DatabaseManager.GetAllViTriDo();
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Lỗi tải dữ liệu vị trí đỗ:\n" + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        //  PANEL CHO LOẠI XE - CREATE VEHICLE TYPE PANEL
        // ============================================================
        private Panel CreateVehicleTypePanel()
        {
            var toolStrip = new ToolStrip();
            var btnAdd     = new ToolStripButton("➕ Thêm") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnEdit    = new ToolStripButton("✏️ Sửa")  { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnDel     = new ToolStripButton("🗑️ Xóa")  { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnRefresh = new ToolStripButton("🔄 Tải lại") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            toolStrip.Items.AddRange(new ToolStripItem[] { btnAdd, btnEdit, btnDel, btnRefresh });

            var dgv = new DataGridView();
            ConfigureGrid(dgv, readOnly: true);
            LoadVehicleTypeData(dgv);

            btnRefresh.Click += (s, e) => LoadVehicleTypeData(dgv);
            btnAdd.Click     += (s, e) => ShowAddVehicleTypeDialog(dgv);
            btnEdit.Click    += (s, e) => ShowEditVehicleTypeDialog(dgv);
            btnDel.Click     += (s, e) => ShowDeleteVehicleTypeDialog(dgv);

            return MakeGridPanel(toolStrip, dgv);
        }

        private void ShowAddVehicleTypeDialog(DataGridView dgv)
        {
            var form = new Form
            {
                Text = "Thêm loại xe",
                Width = 480,
                Height = 280,
                StartPosition = FormStartPosition.CenterParent
            };

            var lbl = new Label { Text = "Mã loại xe:", Location = new Point(10, 20) };
            var txtMa = new TextBox { Location = new Point(140, 20), Width = 300 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtMa);

            lbl = new Label { Text = "Tên loại xe:", Location = new Point(10, 60) };
            var txtTen = new TextBox { Location = new Point(140, 60), Width = 300 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtTen);

            lbl = new Label { Text = "Giá tháng:", Location = new Point(10, 100) };
            var txtGiaThang = new TextBox { Location = new Point(140, 100), Width = 300 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtGiaThang);

            lbl = new Label { Text = "Giá ngày:", Location = new Point(10, 140) };
            var txtGiaNgay = new TextBox { Location = new Point(140, 140), Width = 300 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtGiaNgay);

            lbl = new Label { Text = "Mô tả:", Location = new Point(10, 180) };
            var txtMoTa = new TextBox { Location = new Point(140, 180), Width = 300 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtMoTa);

            var btnOK = new Button { Text = "Lưu", Location = new Point(140, 220), Width = 100 };
            var btnCancel = new Button { Text = "Hủy", Location = new Point(250, 220), Width = 100 };

            btnOK.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtMa.Text) || string.IsNullOrWhiteSpace(txtTen.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã và tên loại xe!", "Thông báo");
                    return;
                }

                if (!decimal.TryParse(txtGiaThang.Text, out decimal giaThang))
                    giaThang = 0;
                if (!decimal.TryParse(txtGiaNgay.Text, out decimal giaNgay))
                    giaNgay = 0;

                try
                {
                    int result = DatabaseManager.AddLoaiXe(txtMa.Text, txtTen.Text, giaThang, giaNgay, txtMoTa.Text);
                    if (result > 0)
                    {
                        MaterialMessageBox.Show("Thêm loại xe thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadVehicleTypeData(dgv);
                        form.Close();
                    }
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show("Lỗi thêm loại xe:\n" + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnCancel.Click += (s, e) => form.Close();
            form.Controls.Add(btnOK);
            form.Controls.Add(btnCancel);

            form.ShowDialog();
        }

        private void ShowEditVehicleTypeDialog(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MaterialMessageBox.Show("Vui lòng chọn loại xe cần sửa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dgv.SelectedRows[0];
            string maLoai = row.Cells["MaLoaiXe"].Value?.ToString() ?? "";
            string tenLoai = row.Cells["TenLoaiXe"].Value?.ToString() ?? "";
            decimal giaThang = Convert.ToDecimal(row.Cells["GiaTienThang"].Value ?? 0);
            decimal giaNgay = Convert.ToDecimal(row.Cells["GiaTienNgay"].Value ?? 0);
            string trangThai = row.Cells["TrangThai"].Value?.ToString() ?? "Đang hoạt động";

            var form = new Form
            {
                Text = "Sửa loại xe",
                Width = 480,
                Height = 260,
                StartPosition = FormStartPosition.CenterParent
            };

            var lbl = new Label { Text = "Mã loại xe:", Location = new Point(10, 20) };
            var txtMa = new TextBox { Location = new Point(140, 20), Width = 300, Text = maLoai, ReadOnly = true };
            form.Controls.Add(lbl);
            form.Controls.Add(txtMa);

            lbl = new Label { Text = "Tên loại xe:", Location = new Point(10, 60) };
            var txtTen = new TextBox { Location = new Point(140, 60), Width = 300, Text = tenLoai };
            form.Controls.Add(lbl);
            form.Controls.Add(txtTen);

            lbl = new Label { Text = "Giá tháng:", Location = new Point(10, 100) };
            var txtGiaThang = new TextBox { Location = new Point(140, 100), Width = 300, Text = giaThang.ToString() };
            form.Controls.Add(lbl);
            form.Controls.Add(txtGiaThang);

            lbl = new Label { Text = "Giá ngày:", Location = new Point(10, 140) };
            var txtGiaNgay = new TextBox { Location = new Point(140, 140), Width = 300, Text = giaNgay.ToString() };
            form.Controls.Add(lbl);
            form.Controls.Add(txtGiaNgay);

            lbl = new Label { Text = "Trạng thái:", Location = new Point(10, 180) };
            var cmbTrangThai = new ComboBox { Location = new Point(140, 180), Width = 300, Text = trangThai };
            cmbTrangThai.Items.AddRange(new[] { "Đang hoạt động", "Tạm dừng" });
            form.Controls.Add(lbl);
            form.Controls.Add(cmbTrangThai);

            var btnOK = new Button { Text = "Lưu", Location = new Point(140, 220), Width = 100 };
            var btnCancel = new Button { Text = "Hủy", Location = new Point(250, 220), Width = 100 };

            btnOK.Click += (s, e) =>
            {
                if (!decimal.TryParse(txtGiaThang.Text, out decimal newGiaThang))
                    newGiaThang = 0;
                if (!decimal.TryParse(txtGiaNgay.Text, out decimal newGiaNgay))
                    newGiaNgay = 0;

                try
                {
                    int result = DatabaseManager.UpdateLoaiXe(maLoai, txtTen.Text, newGiaThang, newGiaNgay, cmbTrangThai.Text);
                    if (result > 0)
                    {
                        MaterialMessageBox.Show("Cập nhật loại xe thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadVehicleTypeData(dgv);
                        form.Close();
                    }
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show("Lỗi cập nhật loại xe:\n" + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnCancel.Click += (s, e) => form.Close();
            form.Controls.Add(btnOK);
            form.Controls.Add(btnCancel);

            form.ShowDialog();
        }

        private void ShowDeleteVehicleTypeDialog(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MaterialMessageBox.Show("Vui lòng chọn loại xe cần xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dgv.SelectedRows[0];
            string maLoai = row.Cells["MaLoaiXe"].Value?.ToString() ?? "";

            if (MaterialMessageBox.Show("Xác nhận xóa loại xe này?", "Xác nhận",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                return;

            try
            {
                int result = DatabaseManager.DeleteLoaiXe(maLoai);
                if (result > 0)
                {
                    MaterialMessageBox.Show("Xóa loại xe thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadVehicleTypeData(dgv);
                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Lỗi xóa loại xe:\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadVehicleTypeData(DataGridView dgv)
        {
            try
            {
                dgv.DataSource = DatabaseManager.GetAllLoaiXe();
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Lỗi tải dữ liệu loại xe:\n" + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        //  PANEL CHO KHU VỰC - CREATE ZONE PANEL
        // ============================================================
        private Panel CreateZonePanel()
        {
            var toolStrip = new ToolStrip();
            var btnAdd     = new ToolStripButton("➕ Thêm") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnEdit    = new ToolStripButton("✏️ Sửa")  { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnDel     = new ToolStripButton("🗑️ Xóa")  { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnRefresh = new ToolStripButton("🔄 Tải lại") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            toolStrip.Items.AddRange(new ToolStripItem[] { btnAdd, btnEdit, btnDel, btnRefresh });

            var dgv = new DataGridView();
            ConfigureGrid(dgv, readOnly: true);
            LoadZoneData(dgv);

            btnRefresh.Click += (s, e) => LoadZoneData(dgv);
            btnAdd.Click     += (s, e) => ShowAddZoneDialog(dgv);
            btnEdit.Click    += (s, e) => ShowEditZoneDialog(dgv);
            btnDel.Click     += (s, e) => ShowDeleteZoneDialog(dgv);

            return MakeGridPanel(toolStrip, dgv);
        }

        private void ShowAddZoneDialog(DataGridView dgv)
        {
            var form = new Form
            {
                Text = "Thêm khu vực",
                Width = 500,
                Height = 300,
                StartPosition = FormStartPosition.CenterParent
            };

            var lbl = new Label { Text = "Mã khu:", Location = new Point(10, 20) };
            var txtMa = new TextBox { Location = new Point(150, 20), Width = 300 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtMa);

            lbl = new Label { Text = "Tên khu:", Location = new Point(10, 60) };
            var txtTen = new TextBox { Location = new Point(150, 60), Width = 300 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtTen);

            lbl = new Label { Text = "Tầng:", Location = new Point(10, 100) };
            var txtTang = new TextBox { Location = new Point(150, 100), Width = 300 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtTang);

            lbl = new Label { Text = "Sức chứa:", Location = new Point(10, 140) };
            var txtSucChua = new TextBox { Location = new Point(150, 140), Width = 300 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtSucChua);

            lbl = new Label { Text = "Mô tả:", Location = new Point(10, 180) };
            var txtMoTa = new TextBox { Location = new Point(150, 180), Width = 300 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtMoTa);

            var btnOK = new Button { Text = "Lưu", Location = new Point(150, 220), Width = 100 };
            var btnCancel = new Button { Text = "Hủy", Location = new Point(260, 220), Width = 100 };

            btnOK.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtMa.Text) || string.IsNullOrWhiteSpace(txtTen.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã và tên khu!", "Thông báo");
                    return;
                }

                if (!int.TryParse(txtTang.Text, out int tang))
                    tang = 0;
                if (!int.TryParse(txtSucChua.Text, out int sucChua))
                    sucChua = 0;

                try
                {
                    int result = DatabaseManager.AddKhuVuc(txtMa.Text, txtTen.Text, tang, sucChua, txtMoTa.Text);
                    if (result > 0)
                    {
                        MaterialMessageBox.Show("Thêm khu vực thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadZoneData(dgv);
                        form.Close();
                    }
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show("Lỗi thêm khu vực:\n" + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnCancel.Click += (s, e) => form.Close();
            form.Controls.Add(btnOK);
            form.Controls.Add(btnCancel);

            form.ShowDialog();
        }

        private void ShowEditZoneDialog(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MaterialMessageBox.Show("Vui lòng chọn khu vực cần sửa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dgv.SelectedRows[0];
            string maKhu = row.Cells["MaKhu"].Value?.ToString() ?? "";
            string tenKhu = row.Cells["TenKhu"].Value?.ToString() ?? "";
            int tang = Convert.ToInt32(row.Cells["Tang"].Value ?? 0);
            int sucChua = Convert.ToInt32(row.Cells["SucChuaToiDa"].Value ?? 0);
            string trangThai = row.Cells["TrangThai"].Value?.ToString() ?? "Đang hoạt động";

            var form = new Form
            {
                Text = "Sửa khu vực",
                Width = 500,
                Height = 280,
                StartPosition = FormStartPosition.CenterParent
            };

            var lbl = new Label { Text = "Mã khu:", Location = new Point(10, 20) };
            var txtMa = new TextBox { Location = new Point(150, 20), Width = 300, Text = maKhu, ReadOnly = true };
            form.Controls.Add(lbl);
            form.Controls.Add(txtMa);

            lbl = new Label { Text = "Tên khu:", Location = new Point(10, 60) };
            var txtTen = new TextBox { Location = new Point(150, 60), Width = 300, Text = tenKhu };
            form.Controls.Add(lbl);
            form.Controls.Add(txtTen);

            lbl = new Label { Text = "Tầng:", Location = new Point(10, 100) };
            var txtTang = new TextBox { Location = new Point(150, 100), Width = 300, Text = tang.ToString() };
            form.Controls.Add(lbl);
            form.Controls.Add(txtTang);

            lbl = new Label { Text = "Sức chứa:", Location = new Point(10, 140) };
            var txtSucChua = new TextBox { Location = new Point(150, 140), Width = 300, Text = sucChua.ToString() };
            form.Controls.Add(lbl);
            form.Controls.Add(txtSucChua);

            lbl = new Label { Text = "Trạng thái:", Location = new Point(10, 180) };
            var cmbTrangThai = new ComboBox { Location = new Point(150, 180), Width = 300, Text = trangThai };
            cmbTrangThai.Items.AddRange(new[] { "Đang hoạt động", "Tạm dừng" });
            form.Controls.Add(lbl);
            form.Controls.Add(cmbTrangThai);

            var btnOK = new Button { Text = "Lưu", Location = new Point(150, 220), Width = 100 };
            var btnCancel = new Button { Text = "Hủy", Location = new Point(260, 220), Width = 100 };

            btnOK.Click += (s, e) =>
            {
                if (!int.TryParse(txtTang.Text, out int newTang))
                    newTang = 0;
                if (!int.TryParse(txtSucChua.Text, out int newSucChua))
                    newSucChua = 0;

                try
                {
                    int result = DatabaseManager.UpdateKhuVuc(maKhu, txtTen.Text, newTang, newSucChua, cmbTrangThai.Text);
                    if (result > 0)
                    {
                        MaterialMessageBox.Show("Cập nhật khu vực thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadZoneData(dgv);
                        form.Close();
                    }
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show("Lỗi cập nhật khu vực:\n" + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnCancel.Click += (s, e) => form.Close();
            form.Controls.Add(btnOK);
            form.Controls.Add(btnCancel);

            form.ShowDialog();
        }

        private void ShowDeleteZoneDialog(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MaterialMessageBox.Show("Vui lòng chọn khu vực cần xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dgv.SelectedRows[0];
            string maKhu = row.Cells["MaKhu"].Value?.ToString() ?? "";

            if (MaterialMessageBox.Show("Xác nhận xóa khu vực này?", "Xác nhận",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                return;

            try
            {
                int result = DatabaseManager.DeleteKhuVuc(maKhu);
                if (result > 0)
                {
                    MaterialMessageBox.Show("Xóa khu vực thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadZoneData(dgv);
                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Lỗi xóa khu vực:\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadZoneData(DataGridView dgv)
        {
            try
            {
                dgv.DataSource = DatabaseManager.GetAllKhuVuc();
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Lỗi tải dữ liệu khu vực:\n" + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        //  PANEL CHO LƯỢT GỬI XE - CREATE PARKING SESSION PANEL
        // ============================================================
        private Panel CreateParkingSessionPanel()
        {
            var toolStrip = new ToolStrip();
            var btnAdd     = new ToolStripButton("➕ Check-in")      { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnEdit    = new ToolStripButton("✏️ Check-out/Sửa") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnDel     = new ToolStripButton("🗑️ Xóa")           { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnRefresh = new ToolStripButton("🔄 Tải lại")        { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            toolStrip.Items.AddRange(new ToolStripItem[] { btnAdd, btnEdit, btnDel, btnRefresh });

            var dgv = new DataGridView();
            ConfigureGrid(dgv, readOnly: true);
            LoadParkingSessionData(dgv);

            btnRefresh.Click += (s, e) => LoadParkingSessionData(dgv);
            btnAdd.Click     += (s, e) => ShowAddParkingSessionDialog(dgv);
            btnEdit.Click    += (s, e) => ShowEditParkingSessionDialog(dgv);
            btnDel.Click     += (s, e) => ShowDeleteParkingSessionDialog(dgv);

            return MakeGridPanel(toolStrip, dgv);
        }

        private void ShowAddParkingSessionDialog(DataGridView dgv)
        {
            var form = new Form
            {
                Text = "Check-in xe",
                Width = 500,
                Height = 280,
                StartPosition = FormStartPosition.CenterParent
            };

            var lbl = new Label { Text = "Mã lượt gửi:", Location = new Point(10, 20) };
            var txtMa = new TextBox { Location = new Point(150, 20), Width = 300 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtMa);

            lbl = new Label { Text = "Mã thẻ:", Location = new Point(10, 60) };
            var txtMaThe = new TextBox { Location = new Point(150, 60), Width = 300 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtMaThe);

            lbl = new Label { Text = "Mã vị trí:", Location = new Point(10, 100) };
            var txtMaViTri = new TextBox { Location = new Point(150, 100), Width = 300 };
            form.Controls.Add(lbl);
            form.Controls.Add(txtMaViTri);

            lbl = new Label { Text = "Thời gian vào:", Location = new Point(10, 140) };
            var dtVao = new DateTimePicker { Location = new Point(150, 140), Width = 300, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy HH:mm", ShowUpDown = true };
            form.Controls.Add(lbl);
            form.Controls.Add(dtVao);

            var btnOK = new Button { Text = "Lưu", Location = new Point(150, 200), Width = 100 };
            var btnCancel = new Button { Text = "Hủy", Location = new Point(260, 200), Width = 100 };

            btnOK.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtMa.Text) || string.IsNullOrWhiteSpace(txtMaThe.Text) || string.IsNullOrWhiteSpace(txtMaViTri.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo");
                    return;
                }
                try
                {
                    int result = DatabaseManager.AddLuotGuiXe(txtMa.Text, txtMaThe.Text, txtMaViTri.Text, dtVao.Value);
                    if (result > 0)
                    {
                        MaterialMessageBox.Show("Check-in thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadParkingSessionData(dgv);
                        form.Close();
                    }
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show("Lỗi check-in:\n" + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnCancel.Click += (s, e) => form.Close();
            form.Controls.Add(btnOK);
            form.Controls.Add(btnCancel);
            form.ShowDialog();
        }

        private void ShowEditParkingSessionDialog(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MaterialMessageBox.Show("Vui lòng chọn lượt gửi xe cần sửa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dgv.SelectedRows[0];
            string maLuotGui = row.Cells["MaLuotGui"].Value?.ToString() ?? "";
            string trangThai = row.Cells["TrangThaiLuotGui"].Value?.ToString() ?? "Đang gửi";

            var form = new Form
            {
                Text = "Check-out / Sửa lượt gửi xe",
                Width = 500,
                Height = 280,
                StartPosition = FormStartPosition.CenterParent
            };

            var lbl = new Label { Text = "Mã lượt gửi:", Location = new Point(10, 20) };
            var txtMa = new TextBox { Location = new Point(150, 20), Width = 300, Text = maLuotGui, ReadOnly = true };
            form.Controls.Add(lbl);
            form.Controls.Add(txtMa);

            lbl = new Label { Text = "Thời gian ra:", Location = new Point(10, 60) };
            var dtRa = new DateTimePicker { Location = new Point(150, 60), Width = 300, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy HH:mm", ShowUpDown = true, Value = DateTime.Now };
            form.Controls.Add(lbl);
            form.Controls.Add(dtRa);

            lbl = new Label { Text = "Trạng thái:", Location = new Point(10, 100) };
            var cmbTrangThai = new ComboBox { Location = new Point(150, 100), Width = 300, Text = trangThai };
            cmbTrangThai.Items.AddRange(new[] { "Đang gửi", "Đã lấy xe", "Vi phạm" });
            form.Controls.Add(lbl);
            form.Controls.Add(cmbTrangThai);

            lbl = new Label { Text = "Tổng tiền:", Location = new Point(10, 140) };
            var txtTongTien = new TextBox { Location = new Point(150, 140), Width = 300, Text = "0" };
            form.Controls.Add(lbl);
            form.Controls.Add(txtTongTien);

            var btnOK = new Button { Text = "Lưu", Location = new Point(150, 200), Width = 100 };
            var btnCancel = new Button { Text = "Hủy", Location = new Point(260, 200), Width = 100 };

            btnOK.Click += (s, e) =>
            {
                if (!decimal.TryParse(txtTongTien.Text, out decimal tongTien))
                    tongTien = 0;
                try
                {
                    int result = DatabaseManager.UpdateLuotGuiXe(maLuotGui, dtRa.Value, cmbTrangThai.Text, tongTien);
                    if (result > 0)
                    {
                        MaterialMessageBox.Show("Cập nhật lượt gửi xe thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadParkingSessionData(dgv);
                        form.Close();
                    }
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show("Lỗi cập nhật:\n" + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnCancel.Click += (s, e) => form.Close();
            form.Controls.Add(btnOK);
            form.Controls.Add(btnCancel);
            form.ShowDialog();
        }

        private void ShowDeleteParkingSessionDialog(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MaterialMessageBox.Show("Vui lòng chọn lượt gửi xe cần xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dgv.SelectedRows[0];
            string maLuotGui = row.Cells["MaLuotGui"].Value?.ToString() ?? "";

            if (MaterialMessageBox.Show("Xác nhận xóa lượt gửi xe này?", "Xác nhận",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                return;

            try
            {
                int result = DatabaseManager.DeleteLuotGuiXe(maLuotGui);
                if (result > 0)
                {
                    MaterialMessageBox.Show("Xóa lượt gửi xe thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadParkingSessionData(dgv);
                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Lỗi xóa lượt gửi xe:\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadParkingSessionData(DataGridView dgv)
        {
            try
            {
                dgv.DataSource = DatabaseManager.GetAllLuotGuiXe();
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Lỗi tải dữ liệu lượt gửi xe:\n" + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        //  PANEL CHO DASHBOARD - TRỰ VẤN + THỐNG KÊ
        // ============================================================
        private Panel CreateDashboardPanel()
        {
            var panel = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(10) };

            // ── Tab control để chia Dashboard thành 2 phần ─────────
            var tabCtrl = new TabControl { Dock = DockStyle.Fill };

            // ── Tab 1: Truy vấn ────────────────────────────────────
            var tabQuery = new TabPage("Truy vấn");
            var pnlQuery = CreateQueryPanelContent();
            tabQuery.Controls.Add(pnlQuery);
            tabCtrl.TabPages.Add(tabQuery);

            // ── Tab 2: Thống kê ────────────────────────────────────
            var tabStats = new TabPage("Thống kê");
            var pnlStats = CreateStatisticsPanelContent();
            tabStats.Controls.Add(pnlStats);
            tabCtrl.TabPages.Add(tabStats);

            panel.Controls.Add(tabCtrl);
            return panel;
        }

        // ============================================================
        //  NỘI DUNG TRUY VẤN
        // ============================================================
        private Panel CreateQueryPanelContent()
        {
            var panel = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(10) };

            var tabCtrl = new TabControl { Dock = DockStyle.Fill };

            tabCtrl.TabPages.Add(CreateQueryHistoryTab());
            tabCtrl.TabPages.Add(CreateQueryEmptySpotTab());
            tabCtrl.TabPages.Add(CreateQueryCardExpiryTab());
            tabCtrl.TabPages.Add(CreateQueryCurrentParkingTab());
            tabCtrl.TabPages.Add(CreateQueryResidentVehicleTab());
            tabCtrl.TabPages.Add(CreateQueryPaymentTab());
            tabCtrl.TabPages.Add(CreateQueryIncidentTab());

            panel.Controls.Add(tabCtrl);
            return panel;
        }

        private TabPage CreateQueryPaymentTab()
        {
            var tab = new TabPage("Thanh toán");

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
                Padding = new Padding(10)
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 130));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var formPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };

            var lblTitle = new Label
            {
                Text = "💳 Tra cứu lịch sử thanh toán",
                Location = new Point(0, 0),
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };

            var lblTuNgay = new MaterialLabel { Text = "Từ ngày:", Location = new Point(0, 38), AutoSize = true };
            var dtTuNgay = new MaterialDateTimePicker { Location = new Point(68, 35), Width = 180 };

            var lblDenNgay = new MaterialLabel { Text = "Đến ngày:", Location = new Point(262, 38), AutoSize = true };
            var dtDenNgay = new MaterialDateTimePicker { Location = new Point(335, 35), Width = 180 };

            var lblLoai = new MaterialLabel { Text = "Loại TT:", Location = new Point(528, 38), AutoSize = true };
            var cmbLoai = new ComboBox { Location = new Point(585, 38), Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbLoai.Items.AddRange(new[] { "Tất cả", "Ngày", "Tháng" });
            cmbLoai.SelectedIndex = 0;

            var lblPhuongThuc = new MaterialLabel { Text = "Phương thức:", Location = new Point(718, 38), AutoSize = true };
            var cmbPhuongThuc = new ComboBox { Location = new Point(813, 38), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbPhuongThuc.Items.AddRange(new[] { "Tất cả", "Tiền mặt", "Chuyển khoản" });
            cmbPhuongThuc.SelectedIndex = 0;

            var btnTraCuu = new MaterialButton { Text = "Tra cứu", Location = new Point(0, 85), Width = 120, Height = 36 };
            var lblCount = new MaterialLabel { Text = "Kết quả: 0 bản ghi", Location = new Point(135, 92), AutoSize = true, ForeColor = Color.Gray };

            formPanel.Controls.AddRange(new Control[] { lblTitle, lblTuNgay, dtTuNgay, lblDenNgay, dtDenNgay,
                lblLoai, cmbLoai, lblPhuongThuc, cmbPhuongThuc, btnTraCuu, lblCount });

            var dgv = CreateDockGrid();

            btnTraCuu.Click += (s, e) =>
            {
                try
                {
                    string loai = cmbLoai.SelectedItem?.ToString() == "Tất cả" ? null : cmbLoai.SelectedItem?.ToString();
                    string phuongThuc = cmbPhuongThuc.SelectedItem?.ToString() == "Tất cả" ? null : cmbPhuongThuc.SelectedItem?.ToString();
                    var data = DatabaseManager.GetLichSuThanhToan(dtTuNgay.Value.Date, dtDenNgay.Value.Date, loai, phuongThuc);
                    dgv.DataSource = data;
                    lblCount.Text = $"Kết quả: {data.Rows.Count} bản ghi";
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show("Lỗi tra cứu thanh toán:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            layout.Controls.Add(formPanel, 0, 0);
            layout.Controls.Add(dgv, 0, 1);
            tab.Controls.Add(layout);
            return tab;
        }

        private TabPage CreateQueryIncidentTab()
        {
            var tab = new TabPage("Sự cố");

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
                Padding = new Padding(10)
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 130));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var formPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };

            var lblTitle = new Label
            {
                Text = "⚠️ Tra cứu sự cố bãi xe",
                Location = new Point(0, 0),
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };

            var lblKeyword = new MaterialLabel { Text = "Nội dung:", Location = new Point(0, 38), AutoSize = true };
            var txtKeyword = new MaterialTextBox2 { Hint = "Tìm theo nội dung sự cố", Location = new Point(70, 35), Width = 240 };

            var lblTrangThai = new MaterialLabel { Text = "Trạng thái:", Location = new Point(325, 38), AutoSize = true };
            var cmbTrangThai = new ComboBox { Location = new Point(405, 38), Width = 160, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbTrangThai.Items.AddRange(new[] { "Tất cả", "Đang chờ", "Đã xử lý" });
            cmbTrangThai.SelectedIndex = 0;

            var lblTuNgay = new MaterialLabel { Text = "Từ ngày:", Location = new Point(580, 38), AutoSize = true };
            var dtTuNgay = new MaterialDateTimePicker { Location = new Point(645, 35), Width = 180 };

            var lblDenNgay = new MaterialLabel { Text = "Đến ngày:", Location = new Point(840, 38), AutoSize = true };
            var dtDenNgay = new MaterialDateTimePicker { Location = new Point(913, 35), Width = 180 };

            var btnTraCuu = new MaterialButton { Text = "Tra cứu", Location = new Point(0, 85), Width = 120, Height = 36 };
            var lblCount = new MaterialLabel { Text = "Kết quả: 0 sự cố", Location = new Point(135, 92), AutoSize = true, ForeColor = Color.Gray };

            formPanel.Controls.AddRange(new Control[] { lblTitle, lblKeyword, txtKeyword, lblTrangThai, cmbTrangThai,
                lblTuNgay, dtTuNgay, lblDenNgay, dtDenNgay, btnTraCuu, lblCount });

            var dgv = CreateDockGrid();

            btnTraCuu.Click += (s, e) =>
            {
                try
                {
                    string trangThai = cmbTrangThai.SelectedItem?.ToString() == "Tất cả" ? null : cmbTrangThai.SelectedItem?.ToString();
                    var data = DatabaseManager.GetSuCoNangCao(txtKeyword.Text, trangThai, dtTuNgay.Value.Date, dtDenNgay.Value.Date);
                    dgv.DataSource = data;
                    lblCount.Text = $"Kết quả: {data.Rows.Count} sự cố";
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show("Lỗi tra cứu sự cố:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            layout.Controls.Add(formPanel, 0, 0);
            layout.Controls.Add(dgv, 0, 1);
            tab.Controls.Add(layout);
            return tab;
        }

        // ============================================================
        //  NỘI DUNG THỐNG KÊ
        // ============================================================
        private Panel CreateStatisticsPanelContent()
        {
            var panel = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(10) };

            var tabCtrl = new TabControl { Dock = DockStyle.Fill };
            tabCtrl.TabPages.Add(CreateStatisticsRevenueTab());
            tabCtrl.TabPages.Add(CreateStatisticsIncidentTab());
            tabCtrl.TabPages.Add(CreateStatisticsOccupancyTab());
            tabCtrl.TabPages.Add(CreateStatisticsStaffTab());
            tabCtrl.TabPages.Add(CreateStatisticsVehicleTab());

            panel.Controls.Add(tabCtrl);

            return panel;
        }

        private TabPage CreateStatisticsRevenueTab()
        {
            var tab = new TabPage("Doanh thu");

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                ColumnCount = 1,
                Padding = new Padding(15)
            };
            layout.RowCount = 4;
            layout.RowStyles.Clear();
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));   // row 0: title
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 220));  // row 1: form + card
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 180));  // row 2: grid
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));   // row 3: chart

            // ── Title ──────────────────────────────────────────────
            var lblTitle = new Label
            {
                Text = "📊 Báo cáo doanh thu",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // ── Form + Card panel ──────────────────────────────────
            var middlePanel = new Panel { Dock = DockStyle.Fill };

            var lblTuNgay = new MaterialLabel { Text = "Từ ngày", Location = new Point(0, 5), AutoSize = true };
            var dtTuNgay = new MaterialDateTimePicker { Location = new Point(0, 30), Width = 220 };

            var lblDenNgay = new MaterialLabel { Text = "Đến ngày", Location = new Point(240, 5), AutoSize = true };
            var dtDenNgay = new MaterialDateTimePicker { Location = new Point(240, 30), Width = 220 };

            var btnThongKe = new MaterialButton { Text = "Thống kê", Location = new Point(480, 28), Width = 120, Height = 36 };

            var card = new MaterialCard { Location = new Point(0, 80), Width = 720, Height = 125 };

            var lblTongLuotGuiTitle = new MaterialLabel { Text = "Tổng lượt gửi:", Location = new Point(16, 16), AutoSize = true };
            var lblTongLuotGui = new MaterialLabel { Text = "0", Location = new Point(200, 16), AutoSize = true, Font = new Font("Segoe UI", 11, FontStyle.Bold) };

            var lblTongDoanhThuTitle = new MaterialLabel { Text = "Tổng doanh thu:", Location = new Point(16, 50), AutoSize = true };
            var lblTongDoanhThu = new MaterialLabel { Text = "0 ₫", Location = new Point(200, 50), AutoSize = true, Font = new Font("Segoe UI", 11, FontStyle.Bold), ForeColor = Color.DarkGreen };

            var lblDoanhThuTheNgayTitle = new MaterialLabel { Text = "Doanh thu thẻ ngày:", Location = new Point(16, 84), AutoSize = true };
            var lblDoanhThuTheNgay = new MaterialLabel { Text = "0 ₫", Location = new Point(200, 84), AutoSize = true, Font = new Font("Segoe UI", 11, FontStyle.Bold) };

            var lblLuotTheNgayTitle = new MaterialLabel { Text = "Lượt thẻ ngày:", Location = new Point(400, 16), AutoSize = true };
            var lblLuotTheNgay = new MaterialLabel { Text = "0", Location = new Point(540, 16), AutoSize = true, Font = new Font("Segoe UI", 11, FontStyle.Bold) };

            var lblLuotTheThangTitle = new MaterialLabel { Text = "Lượt thẻ tháng:", Location = new Point(400, 50), AutoSize = true };
            var lblLuotTheThang = new MaterialLabel { Text = "0", Location = new Point(540, 50), AutoSize = true, Font = new Font("Segoe UI", 11, FontStyle.Bold) };

            card.Controls.AddRange(new Control[] { lblTongLuotGuiTitle, lblTongLuotGui, lblTongDoanhThuTitle,
                lblTongDoanhThu, lblDoanhThuTheNgayTitle, lblDoanhThuTheNgay,
                lblLuotTheNgayTitle, lblLuotTheNgay, lblLuotTheThangTitle, lblLuotTheThang });

            btnThongKe.Click += (s, e) =>
                LoadDoanhThuData(dtTuNgay.Value, dtDenNgay.Value, lblTongLuotGui, lblTongDoanhThu, lblDoanhThuTheNgay);

            middlePanel.Controls.AddRange(new Control[] { lblTuNgay, dtTuNgay, lblDenNgay, dtDenNgay, btnThongKe, card });

            // ── Grid ──────────────────────────────────────────────
            var dgv = CreateDockGrid();

            btnThongKe.Click += (s2, e2) =>
            {
                try
                {
                    var detail = DatabaseManager.GetDoanhThuChiTiet(dtTuNgay.Value.Date, dtDenNgay.Value.Date);
                    dgv.DataSource = detail;
                }
                catch { /* ignore grid error, summary still shows */ }
            };

            layout.Controls.Add(lblTitle, 0, 0);
            layout.Controls.Add(middlePanel, 0, 1);
            layout.Controls.Add(dgv, 0, 2);

            // ── Biểu đồ cột doanh thu theo ngày ──────────────────
            var chartPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 8, 0, 0) };

            var lblChartTitle = new Label
            {
                Text = "📈 Biểu đồ doanh thu theo ngày",
                Location = new Point(0, 0),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            var chart = new LiveCharts.WinForms.CartesianChart
            {
                Location = new Point(0, 26),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
            };

            chartPanel.Resize += (s, e) =>
            {
                chart.Width = chartPanel.ClientSize.Width;
                chart.Height = chartPanel.ClientSize.Height - 26;
            };

            chartPanel.Controls.Add(lblChartTitle);
            chartPanel.Controls.Add(chart);

            btnThongKe.Click += (s, e) =>
            {
                try
                {
                    var dt = DatabaseManager.GetDoanhThuChiTiet(dtTuNgay.Value.Date, dtDenNgay.Value.Date);

                    // Nhóm theo ngày
                    var dict = new System.Collections.Generic.Dictionary<string, double>();
                    foreach (DataRow r in dt.Rows)
                    {
                        if (r["Giờ vào"] == DBNull.Value) continue;
                        string ngay = Convert.ToDateTime(r["Giờ vào"]).ToString("dd/MM");
                        double tien = r["Doanh thu (VNĐ)"] != DBNull.Value ? Convert.ToDouble(r["Doanh thu (VNĐ)"]) : 0;
                        if (dict.ContainsKey(ngay)) dict[ngay] += tien;
                        else dict[ngay] = tien;
                    }

                    var labels = new System.Collections.Generic.List<string>(dict.Keys);
                    var values = new ChartValues<double>();
                    foreach (var v in dict.Values) values.Add(v);

                    chart.Series = new SeriesCollection
        {
            new LiveCharts.Wpf.ColumnSeries
            {
                Title  = "Doanh thu (VNĐ)",
                Values = values,
                Fill   = new System.Windows.Media.SolidColorBrush(
                             System.Windows.Media.Color.FromRgb(30, 136, 229)),
                StrokeThickness = 0
            }
        };

                    chart.AxisX.Clear();
                    chart.AxisX.Add(new LiveCharts.Wpf.Axis
                    {
                        Labels = labels,
                        LabelsRotation = -30,
                        Separator = new LiveCharts.Wpf.Separator { IsEnabled = false }
                    });

                    chart.AxisY.Clear();
                    chart.AxisY.Add(new LiveCharts.Wpf.Axis
                    {
                        Title = "VNĐ",
                        LabelFormatter = val => val.ToString("N0")
                    });

                    chart.LegendLocation = LegendLocation.Top;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Lỗi vẽ biểu đồ: " + ex.Message);
                }
            };

            layout.Controls.Add(lblTitle, 0, 0);
            layout.Controls.Add(middlePanel, 0, 1);
            layout.Controls.Add(dgv, 0, 2);
            layout.Controls.Add(chartPanel, 0, 3);


            tab.Controls.Add(layout);
            return tab;
        }

        private TabPage CreateStatisticsIncidentTab()
        {
            var tab = new TabPage("Sự cố");

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                ColumnCount = 1,
                Padding = new Padding(15)
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 185));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var lblTitle = new Label
            {
                Text = "📋 Thống kê sự cố theo khu vực",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };

            var middlePanel = new Panel { Dock = DockStyle.Fill };

            var btnTai = new MaterialButton { Text = "Tải dữ liệu", Location = new Point(0, 0), Width = 120, Height = 36 };

            var card = new MaterialCard { Location = new Point(0, 48), Width = 720, Height = 128 };

            var lblTongSuCoTitle = new MaterialLabel { Text = "Tổng sự cố:", Location = new Point(16, 16), AutoSize = true };
            var lblTongSuCo = new MaterialLabel { Text = "0", Location = new Point(150, 16), AutoSize = true, Font = new Font("Segoe UI", 11, FontStyle.Bold) };

            var lblTongChiPhiTitle = new MaterialLabel { Text = "Tổng chi phí:", Location = new Point(16, 52), AutoSize = true };
            var lblTongChiPhi = new MaterialLabel { Text = "0 ₫", Location = new Point(150, 52), AutoSize = true, Font = new Font("Segoe UI", 11, FontStyle.Bold), ForeColor = Color.DarkRed };

            var lblChuaXuLyTitle = new MaterialLabel { Text = "Chưa xử lý:", Location = new Point(360, 16), AutoSize = true };
            var lblChuaXuLy = new MaterialLabel { Text = "0", Location = new Point(500, 16), AutoSize = true, Font = new Font("Segoe UI", 11, FontStyle.Bold), ForeColor = Color.OrangeRed };

            var lblDaXuLyTitle = new MaterialLabel { Text = "Đã xử lý:", Location = new Point(360, 52), AutoSize = true };
            var lblDaXuLy = new MaterialLabel { Text = "0", Location = new Point(500, 52), AutoSize = true, Font = new Font("Segoe UI", 11, FontStyle.Bold), ForeColor = Color.DarkGreen };

            var lblChiPhiTBTitle = new MaterialLabel { Text = "Chi phí TB/sự cố:", Location = new Point(16, 90), AutoSize = true };
            var lblChiPhiTB = new MaterialLabel { Text = "0 ₫", Location = new Point(150, 90), AutoSize = true, Font = new Font("Segoe UI", 11, FontStyle.Bold) };

            card.Controls.AddRange(new Control[] { lblTongSuCoTitle, lblTongSuCo, lblTongChiPhiTitle, lblTongChiPhi,
                lblChuaXuLyTitle, lblChuaXuLy, lblDaXuLyTitle, lblDaXuLy, lblChiPhiTBTitle, lblChiPhiTB });

            middlePanel.Controls.AddRange(new Control[] { btnTai, card });

            var dgv = CreateDockGrid();

            btnTai.Click += (s, e) =>
            {
                try
                {
                    var data = DatabaseManager.GetBaoCaoSuCo(null);
                    dgv.DataSource = data;

                    int tongSuCo = 0; decimal tongChiPhi = 0; int daXuLy = 0;
                    foreach (DataRow row in data.Rows)
                    {
                        if (data.Columns.Contains("TongSuCo") && row["TongSuCo"] != DBNull.Value)
                            tongSuCo += Convert.ToInt32(row["TongSuCo"]);
                        if (data.Columns.Contains("TongChiPhi") && row["TongChiPhi"] != DBNull.Value)
                            tongChiPhi += Convert.ToDecimal(row["TongChiPhi"]);
                        if (data.Columns.Contains("DaXuLy") && row["DaXuLy"] != DBNull.Value)
                            daXuLy += Convert.ToInt32(row["DaXuLy"]);
                    }
                    lblTongSuCo.Text = tongSuCo.ToString("N0");
                    lblTongChiPhi.Text = tongChiPhi.ToString("N0") + " ₫";
                    lblChuaXuLy.Text = (tongSuCo - daXuLy).ToString("N0");
                    lblDaXuLy.Text = daXuLy.ToString("N0");
                    lblChiPhiTB.Text = tongSuCo > 0 ? (tongChiPhi / tongSuCo).ToString("N0") + " ₫" : "0 ₫";
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show("Lỗi tải dữ liệu sự cố:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            layout.Controls.Add(lblTitle, 0, 0);
            layout.Controls.Add(middlePanel, 0, 1);
            layout.Controls.Add(dgv, 0, 2);
            tab.Controls.Add(layout);
            return tab;
        }

        private TabPage CreateStatisticsOccupancyTab()
        {
            var tab = new TabPage("Công suất");

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                ColumnCount = 1,
                Padding = new Padding(10)
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 175));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var lblTitle = new Label
            {
                Text = "🅿️ Thống kê công suất bãi đỗ theo khu vực",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };

            var middlePanel = new Panel { Dock = DockStyle.Fill };

            var btnTai = new MaterialButton { Text = "Thống kê", Location = new Point(0, 0), Width = 120, Height = 36 };

            var card = new MaterialCard { Location = new Point(0, 48), Width = 800, Height = 116 };

            var lblTongViTriTitle = new MaterialLabel { Text = "Tổng vị trí hiện có:", Location = new Point(16, 16), AutoSize = true };
            var lblTongViTri = new MaterialLabel { Text = "0", Location = new Point(150, 16), AutoSize = true, Font = new Font("Segoe UI", 11, FontStyle.Bold) };

            var lblDaDungTitle = new MaterialLabel { Text = "Đã dùng:", Location = new Point(16, 52), AutoSize = true };
            var lblDaDung = new MaterialLabel { Text = "0", Location = new Point(150, 52), AutoSize = true, Font = new Font("Segoe UI", 11, FontStyle.Bold), ForeColor = Color.DarkRed };

            var lblConTrongTitle = new MaterialLabel { Text = "Còn trống:", Location = new Point(280, 16), AutoSize = true };
            var lblConTrong = new MaterialLabel { Text = "0", Location = new Point(410, 16), AutoSize = true, Font = new Font("Segoe UI", 11, FontStyle.Bold), ForeColor = Color.DarkGreen };

            var lblTyLeTitle = new MaterialLabel { Text = "Tỷ lệ lấp đầy:", Location = new Point(280, 52), AutoSize = true };
            var lblTyLe = new MaterialLabel { Text = "0%", Location = new Point(410, 52), AutoSize = true, Font = new Font("Segoe UI", 11, FontStyle.Bold) };

            var lblSucChuaTitle = new MaterialLabel { Text = "Tổng sức chứa:", Location = new Point(560, 16), AutoSize = true };
            var lblSucChua = new MaterialLabel { Text = "0", Location = new Point(680, 16), AutoSize = true, Font = new Font("Segoe UI", 11, FontStyle.Bold) };

            card.Controls.AddRange(new Control[] { lblTongViTriTitle, lblTongViTri, lblDaDungTitle, lblDaDung,
                lblConTrongTitle, lblConTrong, lblTyLeTitle, lblTyLe, lblSucChuaTitle, lblSucChua });

            middlePanel.Controls.AddRange(new Control[] { btnTai, card });

            var dgv = CreateDockGrid();

            btnTai.Click += (s, e) =>
            {
                try
                {
                    var data = DatabaseManager.GetThongKeKhuVuc();
                    dgv.DataSource = data;

                    int tongViTri = 0, daDung = 0, sucChua = 0;
                    foreach (DataRow row in data.Rows)
                    {
                        if (data.Columns.Contains("Tổng vị trí") && row["Tổng vị trí"] != DBNull.Value)
                            tongViTri += Convert.ToInt32(row["Tổng vị trí"]);
                        if (data.Columns.Contains("Đang dùng") && row["Đang dùng"] != DBNull.Value)
                            daDung += Convert.ToInt32(row["Đang dùng"]);
                        if (data.Columns.Contains("Sức chứa tối đa") && row["Sức chứa tối đa"] != DBNull.Value)
                            sucChua += Convert.ToInt32(row["Sức chứa tối đa"]);
                    }
                    int conTrong = tongViTri - daDung;
                    decimal tyLe = tongViTri > 0 ? (decimal)daDung * 100m / tongViTri : 0m;

                    lblTongViTri.Text = tongViTri.ToString("N0");
                    lblDaDung.Text = daDung.ToString("N0");
                    lblConTrong.Text = conTrong.ToString("N0");
                    lblTyLe.Text = tyLe.ToString("N1") + "%";
                    lblSucChua.Text = sucChua.ToString("N0");
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show("Lỗi tải dữ liệu công suất bãi:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            layout.Controls.Add(lblTitle, 0, 0);
            layout.Controls.Add(middlePanel, 0, 1);
            layout.Controls.Add(dgv, 0, 2);
            tab.Controls.Add(layout);
            return tab;
        }

        private TabPage CreateStatisticsStaffTab()
        {
            var tab = new TabPage("Nhân viên");

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                ColumnCount = 1,
                Padding = new Padding(10)
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var lblTitle = new Label
            {
                Text = "👷 Thống kê hoạt động nhân viên",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };

            var middlePanel = new Panel { Dock = DockStyle.Fill };
            var lblCaLabel = new MaterialLabel { Text = "Lọc theo ca:", Location = new Point(0, 10), AutoSize = true };
            var cmbCa = new ComboBox { Location = new Point(100, 8), Width = 130, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbCa.Items.AddRange(new[] { "Tất cả", "Sáng", "Chiều", "Tối" });
            cmbCa.SelectedIndex = 0;
            var btnTai = new MaterialButton { Text = "Thống kê", Location = new Point(245, 5), Width = 120, Height = 36 };
            middlePanel.Controls.AddRange(new Control[] { lblCaLabel, cmbCa, btnTai });

            var dgv = CreateDockGrid();

            btnTai.Click += (s, e) =>
            {
                try
                {
                    string ca = cmbCa.SelectedItem?.ToString() == "Tất cả" ? null : cmbCa.SelectedItem?.ToString();
                    dgv.DataSource = DatabaseManager.GetThongKeNhanVien(ca);
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show("Lỗi thống kê nhân viên:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            layout.Controls.Add(lblTitle, 0, 0);
            layout.Controls.Add(middlePanel, 0, 1);
            layout.Controls.Add(dgv, 0, 2);
            tab.Controls.Add(layout);
            return tab;
        }

        private TabPage CreateStatisticsVehicleTab()
        {
            var tab = new TabPage("Loại xe");

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                ColumnCount = 1,
                Padding = new Padding(10)
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var lblTitle = new Label
            {
                Text = "🚘 Thống kê xe theo loại và hãng",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };

            var middlePanel = new Panel { Dock = DockStyle.Fill };
            var lblNhomLabel = new MaterialLabel { Text = "Nhóm theo:", Location = new Point(0, 10), AutoSize = true };
            var cmbNhom = new ComboBox { Location = new Point(90, 8), Width = 160, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbNhom.Items.AddRange(new[] { "Loại xe (Ô tô / Xe máy)", "Hãng xe", "Màu xe", "Năm sản xuất" });
            cmbNhom.SelectedIndex = 0;
            var btnTai = new MaterialButton { Text = "Thống kê", Location = new Point(265, 5), Width = 120, Height = 36 };
            middlePanel.Controls.AddRange(new Control[] { lblNhomLabel, cmbNhom, btnTai });

            var dgv = CreateDockGrid();

            btnTai.Click += (s, e) =>
            {
                try
                {
                    string nhom = cmbNhom.SelectedItem?.ToString() ?? "";
                    dgv.DataSource = DatabaseManager.GetThongKeXe(nhom);
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show("Lỗi thống kê xe:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            layout.Controls.Add(lblTitle, 0, 0);
            layout.Controls.Add(middlePanel, 0, 1);
            layout.Controls.Add(dgv, 0, 2);
            tab.Controls.Add(layout);
            return tab;
        }

        /// <summary>Tải biểu đồ cột doanh thu theo tháng</summary>
        private void LoadRevenueChart(LiveCharts.WinForms.CartesianChart chart)
        {
            try
            {
                var data = DatabaseManager.GetMonthlyRevenue(DateTime.Now.Year);

                // ── Tạo label tháng ───────────────────────────────────
                var labels = new[] { "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10", "T11", "T12" };

                // ── Khởi tạo mảng doanh thu ────────────────────────────
                double[] revenue = new double[12];
                for (int i = 0; i < 12; i++) revenue[i] = 0;

                // ── Điền dữ liệu từ DB ─────────────────────────────────
                foreach (DataRow row in data.Rows)
                {
                    int month = Convert.ToInt32(row["Thang"]) - 1;
                    revenue[month] = Convert.ToDouble(row["DoanhThu"]);
                }

                // ── Debug: In ra dữ liệu ───────────────────────────────
                decimal totalRevenue = DatabaseManager.GetTotalRevenue();
                decimal todayRevenue = DatabaseManager.GetTodayRevenue();
                System.Diagnostics.Debug.WriteLine($"[DEBUG] Total Revenue: {totalRevenue:N0}, Today: {todayRevenue:N0}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DEBUG ERROR] Failed to load revenue chart: {ex.Message}");
            }
        }

        /// <summary>Tải biểu đồ tròn loại xe</summary>
        private void LoadVehicleTypeChart(LiveCharts.WinForms.PieChart chart)
        {
            try
            {
                var data = DatabaseManager.GetVehicleTypeStatistics();

                string info = "[DEBUG] Vehicle Types Statistics:\n";
                foreach (DataRow row in data.Rows)
                {
                    string tenLoaiXe = row["TenLoaiXe"].ToString();
                    int soLuong = Convert.ToInt32(row["SoLuong"]);
                    info += $"{tenLoaiXe}: {soLuong} vehicles\n";
                }

                System.Diagnostics.Debug.WriteLine(info);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DEBUG ERROR] Failed to load vehicle type chart: {ex.Message}");
            }
        }

        // ============================================================
        //  PANEL CHO CƯ DÂN - THÔNG TIN CỦA TÔI
        // ============================================================
        private Panel CreateResidentInfoPanel()
        {
            var panel = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(10) };

            // ── Thông tin cú dân ───────────────────────────────────
            var lblInfo = new Label
            {
                Text = $"Mã cư dân: {_maUser}",
                Location = new Point(10, 10),
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };
            panel.Controls.Add(lblInfo);

            // ── Danh sách xe ───────────────────────────────────────
            var lblVehicles = new Label
            {
                Text = "Xe của tôi:",
                Location = new Point(10, 50),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            panel.Controls.Add(lblVehicles);

            var dgvVehicles = new DataGridView
            {
                Location = new Point(10, 80),
                Height = 200,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                AutoGenerateColumns = true,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                GridColor = Color.FromArgb(220, 220, 220),
                BorderStyle = BorderStyle.FixedSingle,
            };

            LoadResidentVehicles(dgvVehicles);
            panel.Controls.Add(dgvVehicles);

            // ── Lịch sử gửi xe ────────────────────────────────────
            var lblHistory = new Label
            {
                Text = "Lịch sử gửi xe:",
                Location = new Point(10, 300),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            panel.Controls.Add(lblHistory);

            var dgvHistory = new DataGridView
            {
                Location = new Point(10, 330),
                Height = 300,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                AutoGenerateColumns = true,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                GridColor = Color.FromArgb(220, 220, 220),
                BorderStyle = BorderStyle.FixedSingle,
            };

            LoadResidentHistory(dgvHistory);
            panel.Controls.Add(dgvHistory);

            // ── Nút tải lại ────────────────────────────────────────
            var btnRefresh = new Button
            {
                Text = "🔄 Tải lại",
                Location = new Point(10, 640),
                Width = 100,
                Height = 40
            };

            btnRefresh.Click += (s, e) =>
            {
                LoadResidentVehicles(dgvVehicles);
                LoadResidentHistory(dgvHistory);
            };

            panel.Controls.Add(btnRefresh);
            panel.Resize += (s, e) =>
            {
                int w = panel.ClientSize.Width - 20;
                if (w > 0)
                {
                    dgvVehicles.Width = w;
                    dgvHistory.Width = w;
                }
            };
            return panel;
        }

        /// <summary>Tải danh sách xe của cư dân</summary>
        private void LoadResidentVehicles(DataGridView dgv)
        {
            try
            {
                dgv.DataSource = DatabaseManager.GetXeByMaCuDan(_maUser);
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Lỗi tải danh sách xe:\n" + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Tải lịch sử gửi xe của cư dân</summary>
        private void LoadResidentHistory(DataGridView dgv)
        {
            try
            {
                dgv.DataSource = DatabaseManager.GetLuotGuiByMaCuDan(_maUser);
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show("Lỗi tải lịch sử gửi xe:\n" + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        //  HỖ TRỢ - HELPER METHODS
        // ============================================================

        /// <summary>Lấy tên vai trò từ mã</summary>
        private string GetRoleName(string maVaiTro)
        {
            return maVaiTro switch
            {
                "AD" => "Quản trị viên",
                "NV" => "Nhân viên",
                "CD" => "Cư dân",
                _ => "Không xác định"
            };
        }

        // ============================================================
        //  HELPER: Tạo label + control theo dòng trong form dialog
        // ============================================================
        private static (Label lbl, T ctrl) MakeRow<T>(Form f, string text, int y, T ctrl) where T : Control
        {
            var lbl = new Label { Text = text, Location = new Point(10, y + 3), Width = 135, AutoSize = false };
            ctrl.Location = new Point(150, y);
            ctrl.Width = 300;
            f.Controls.Add(lbl);
            f.Controls.Add(ctrl);
            return (lbl, ctrl);
        }

        private static Button MakeBtn(string text, int x, int y)
            => new Button { Text = text, Location = new Point(x, y), Width = 100 };

        private static string SafeStr(DataGridViewRow r, string col)
        {
            try { return r.Cells[col].Value?.ToString() ?? ""; } catch { return ""; }
        }

        private static void FkError(string msg)
            => MaterialMessageBox.Show(msg, "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        // ============================================================
        //  VAI TRÒ PANEL
        // ============================================================
        private Panel CreateVaiTroPanel()
        {
            var ts = new ToolStrip();
            var btnAdd = new ToolStripButton("➕ Thêm") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnEdit = new ToolStripButton("✏️ Sửa") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnDel = new ToolStripButton("🗑️ Xóa") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnRef = new ToolStripButton("🔄 Tải lại") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            ts.Items.AddRange(new ToolStripItem[] { btnAdd, btnEdit, btnDel, btnRef });
            var dgv = new DataGridView(); ConfigureGrid(dgv, true);
            void Load() { try { dgv.DataSource = DatabaseManager.GetAllVaiTro(); } catch (Exception ex) { MaterialMessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); } }
            Load();
            btnRef.Click += (s, e) => Load();
            btnAdd.Click += (s, e) =>
            {
                var f = new Form { Text = "Thêm vai trò", Width = 480, Height = 280, StartPosition = FormStartPosition.CenterParent };
                var (_, txtMa) = MakeRow(f, "Mã vai trò:", 20, new TextBox());
                var (_, txtTen) = MakeRow(f, "Tên vai trò:", 60, new TextBox());
                var (_, txtMoTa) = MakeRow(f, "Mô tả:", 100, new TextBox());
                var (_, cmbTT) = MakeRow(f, "Trạng thái:", 140, new ComboBox());
                cmbTT.Items.AddRange(new[] { "Đang hoạt động", "Ngừng hoạt động" }); cmbTT.SelectedIndex = 0;
                var btnOK = MakeBtn("Lưu", 150, 190); var btnC = MakeBtn("Hủy", 260, 190);
                f.Controls.AddRange(new Control[] { btnOK, btnC });
                btnOK.Click += (_, __) => {
                    if (string.IsNullOrWhiteSpace(txtMa.Text) || string.IsNullOrWhiteSpace(txtTen.Text)) { MessageBox.Show("Nhập đủ thông tin!"); return; }
                    try { DatabaseManager.AddVaiTro(txtMa.Text, txtTen.Text, txtMoTa.Text, cmbTT.Text); Load(); f.Close(); }
                    catch (Exception ex) { MaterialMessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                };
                btnC.Click += (_, __) => f.Close(); f.ShowDialog();
            };
            btnEdit.Click += (s, e) =>
            {
                if (dgv.SelectedRows.Count == 0) { MessageBox.Show("Chọn dòng cần sửa!"); return; }
                var row = dgv.SelectedRows[0];
                var f = new Form { Text = "Sửa vai trò", Width = 480, Height = 280, StartPosition = FormStartPosition.CenterParent };
                var (_, txtMa) = MakeRow(f, "Mã vai trò:", 20, new TextBox { Text = SafeStr(row, "MaVaiTro"), ReadOnly = true });
                var (_, txtTen) = MakeRow(f, "Tên vai trò:", 60, new TextBox { Text = SafeStr(row, "TenVaiTro") });
                var (_, txtMoTa) = MakeRow(f, "Mô tả:", 100, new TextBox { Text = SafeStr(row, "MoTa") });
                var (_, cmbTT) = MakeRow(f, "Trạng thái:", 140, new ComboBox());
                cmbTT.Items.AddRange(new[] { "Đang hoạt động", "Ngừng hoạt động" }); cmbTT.Text = SafeStr(row, "TrangThai");
                var btnOK = MakeBtn("Lưu", 150, 190); var btnC = MakeBtn("Hủy", 260, 190);
                f.Controls.AddRange(new Control[] { btnOK, btnC });
                btnOK.Click += (_, __) => {
                    try { DatabaseManager.UpdateVaiTro(txtMa.Text, txtTen.Text, txtMoTa.Text, cmbTT.Text); Load(); f.Close(); }
                    catch (Exception ex) { MaterialMessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                };
                btnC.Click += (_, __) => f.Close(); f.ShowDialog();
            };
            btnDel.Click += (s, e) =>
            {
                if (dgv.SelectedRows.Count == 0) { MessageBox.Show("Chọn dòng cần xóa!"); return; }
                var row = dgv.SelectedRows[0]; string ma = SafeStr(row, "MaVaiTro");
                if (MessageBox.Show($"Xóa vai trò '{ma}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                try { DatabaseManager.DeleteVaiTro(ma); Load(); }
                catch (Exception ex) { FkError(ex.Message); }
            };
            return MakeGridPanel(ts, dgv);
        }

        // ============================================================
        //  CĂN HỘ PANEL
        // ============================================================
        private Panel CreateCanHoPanel()
        {
            var ts = new ToolStrip();
            var btnAdd = new ToolStripButton("➕ Thêm") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnEdit = new ToolStripButton("✏️ Sửa") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnDel = new ToolStripButton("🗑️ Xóa") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnRef = new ToolStripButton("🔄 Tải lại") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            ts.Items.AddRange(new ToolStripItem[] { btnAdd, btnEdit, btnDel, btnRef });
            var dgv = new DataGridView(); ConfigureGrid(dgv, true);
            void Load() { try { dgv.DataSource = DatabaseManager.GetAllCanHo(); } catch (Exception ex) { MaterialMessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); } }
            Load();
            btnRef.Click += (s, e) => Load();
            btnAdd.Click += (s, e) =>
            {
                var f = new Form { Text = "Thêm căn hộ", Width = 480, Height = 340, StartPosition = FormStartPosition.CenterParent };
                var (_, txtMa) = MakeRow(f, "Mã căn hộ:", 20, new TextBox());
                var (_, txtSo) = MakeRow(f, "Số căn hộ:", 60, new TextBox());
                var (_, txtToa) = MakeRow(f, "Tòa nhà:", 100, new TextBox());
                var (_, numTang) = MakeRow(f, "Tầng:", 140, new NumericUpDown { Minimum = 0, Maximum = 100 });
                var (_, cmbTT) = MakeRow(f, "Trạng thái:", 180, new ComboBox());
                cmbTT.Items.AddRange(new[] { "Đã thuê", "Còn trống", "Đang sửa chữa" }); cmbTT.SelectedIndex = 1;
                var (_, txtGC) = MakeRow(f, "Ghi chú:", 220, new TextBox());
                var btnOK = MakeBtn("Lưu", 150, 265); var btnC = MakeBtn("Hủy", 260, 265);
                f.Controls.AddRange(new Control[] { btnOK, btnC });
                btnOK.Click += (_, __) => {
                    if (string.IsNullOrWhiteSpace(txtMa.Text) || string.IsNullOrWhiteSpace(txtSo.Text)) { MessageBox.Show("Nhập đủ thông tin!"); return; }
                    try { DatabaseManager.AddCanHo(txtMa.Text, txtSo.Text, txtToa.Text, (int)numTang.Value, cmbTT.Text, txtGC.Text); Load(); f.Close(); }
                    catch (Exception ex) { MaterialMessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                };
                btnC.Click += (_, __) => f.Close(); f.ShowDialog();
            };
            btnEdit.Click += (s, e) =>
            {
                if (dgv.SelectedRows.Count == 0) { MessageBox.Show("Chọn dòng cần sửa!"); return; }
                var row = dgv.SelectedRows[0];
                var f = new Form { Text = "Sửa căn hộ", Width = 480, Height = 340, StartPosition = FormStartPosition.CenterParent };
                var (_, txtMa) = MakeRow(f, "Mã căn hộ:", 20, new TextBox { Text = SafeStr(row, "MaCanHo"), ReadOnly = true });
                var (_, txtSo) = MakeRow(f, "Số căn hộ:", 60, new TextBox { Text = SafeStr(row, "SoCanHo") });
                var (_, txtToa) = MakeRow(f, "Tòa nhà:", 100, new TextBox { Text = SafeStr(row, "ToaNha") });
                var (_, numTang) = MakeRow(f, "Tầng:", 140, new NumericUpDown { Minimum = 0, Maximum = 100 });
                if (int.TryParse(SafeStr(row, "Tang"), out int tang)) numTang.Value = tang;
                var (_, cmbTT) = MakeRow(f, "Trạng thái:", 180, new ComboBox());
                cmbTT.Items.AddRange(new[] { "Đã thuê", "Còn trống", "Đang sửa chữa" }); cmbTT.Text = SafeStr(row, "TrangThai");
                var (_, txtGC) = MakeRow(f, "Ghi chú:", 220, new TextBox { Text = SafeStr(row, "GhiChu") });
                var btnOK = MakeBtn("Lưu", 150, 265); var btnC = MakeBtn("Hủy", 260, 265);
                f.Controls.AddRange(new Control[] { btnOK, btnC });
                btnOK.Click += (_, __) => {
                    try { DatabaseManager.UpdateCanHo(txtMa.Text, txtSo.Text, txtToa.Text, (int)numTang.Value, cmbTT.Text, txtGC.Text); Load(); f.Close(); }
                    catch (Exception ex) { MaterialMessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                };
                btnC.Click += (_, __) => f.Close(); f.ShowDialog();
            };
            btnDel.Click += (s, e) =>
            {
                if (dgv.SelectedRows.Count == 0) { MessageBox.Show("Chọn dòng cần xóa!"); return; }
                var row = dgv.SelectedRows[0]; string ma = SafeStr(row, "MaCanHo");
                if (MessageBox.Show($"Xóa căn hộ '{ma}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                try { DatabaseManager.DeleteCanHo(ma); Load(); }
                catch (Exception ex) { FkError(ex.Message); }
            };
            return MakeGridPanel(ts, dgv);
        }

        // ============================================================
        //  CƯ DÂN - CĂN HỘ PANEL
        // ============================================================
        private Panel CreateCuDanCanHoPanel()
        {
            var ts = new ToolStrip();
            var btnAdd = new ToolStripButton("➕ Thêm liên kết") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnEdit = new ToolStripButton("✏️ Sửa") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnDel = new ToolStripButton("🗑️ Xóa liên kết") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnRef = new ToolStripButton("🔄 Tải lại") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            ts.Items.AddRange(new ToolStripItem[] { btnAdd, btnEdit, btnDel, btnRef });
            var dgv = new DataGridView(); ConfigureGrid(dgv, true);
            void Load() { try { dgv.DataSource = DatabaseManager.GetAllCuDanCanHo(); } catch (Exception ex) { MaterialMessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); } }
            Load();
            btnRef.Click += (s, e) => Load();
            btnAdd.Click += (s, e) =>
            {
                var f = new Form { Text = "Liên kết Cư dân - Căn hộ", Width = 480, Height = 340, StartPosition = FormStartPosition.CenterParent };
                var (_, txtCuDan) = MakeRow(f, "Mã cư dân:", 20, new TextBox());
                var (_, txtCanHo) = MakeRow(f, "Mã căn hộ:", 60, new TextBox());
                var (_, cmbVaiTro) = MakeRow(f, "Vai trò:", 100, new ComboBox());
                cmbVaiTro.Items.AddRange(new[] { "Chủ căn hộ", "Thành viên", "Khách thuê" }); cmbVaiTro.SelectedIndex = 0;
                var (_, dtBD) = MakeRow(f, "Ngày bắt đầu:", 140, new DateTimePicker { Format = DateTimePickerFormat.Short });
                var (_, chkKT) = MakeRow(f, "Có ngày kết thúc:", 180, new CheckBox());
                var (_, dtKT) = MakeRow(f, "Ngày kết thúc:", 220, new DateTimePicker { Format = DateTimePickerFormat.Short, Enabled = false });
                chkKT.CheckedChanged += (_, __) => dtKT.Enabled = chkKT.Checked;
                var btnOK = MakeBtn("Lưu", 150, 265); var btnC = MakeBtn("Hủy", 260, 265);
                f.Controls.AddRange(new Control[] { btnOK, btnC });
                btnOK.Click += (_, __) => {
                    if (string.IsNullOrWhiteSpace(txtCuDan.Text) || string.IsNullOrWhiteSpace(txtCanHo.Text)) { MessageBox.Show("Nhập mã cư dân và mã căn hộ!"); return; }
                    try {
                        DateTime? kt = chkKT.Checked ? (DateTime?)dtKT.Value : null;
                        DatabaseManager.AddCuDanCanHo(txtCuDan.Text, txtCanHo.Text, cmbVaiTro.Text, dtBD.Value, kt);
                        Load(); f.Close();
                    }
                    catch (Exception ex) { MaterialMessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                };
                btnC.Click += (_, __) => f.Close(); f.ShowDialog();
            };
            btnEdit.Click += (s, e) =>
            {
                if (dgv.SelectedRows.Count == 0) { MessageBox.Show("Chọn dòng cần sửa!"); return; }
                var row = dgv.SelectedRows[0];
                string maCuDan = SafeStr(row, "MaCuDan"), maCanHo = SafeStr(row, "MaCanHo");
                var f = new Form { Text = "Sửa liên kết", Width = 480, Height = 260, StartPosition = FormStartPosition.CenterParent };
                MakeRow(f, "Mã cư dân:", 20, new TextBox { Text = maCuDan, ReadOnly = true });
                MakeRow(f, "Mã căn hộ:", 60, new TextBox { Text = maCanHo, ReadOnly = true });
                var (_, cmbVaiTro) = MakeRow(f, "Vai trò:", 100, new ComboBox());
                cmbVaiTro.Items.AddRange(new[] { "Chủ căn hộ", "Thành viên", "Khách thuê" }); cmbVaiTro.Text = SafeStr(row, "VaiTroCuDan");
                var (_, dtKT) = MakeRow(f, "Ngày kết thúc:", 140, new DateTimePicker { Format = DateTimePickerFormat.Short });
                var btnOK = MakeBtn("Lưu", 150, 185); var btnC = MakeBtn("Hủy", 260, 185);
                f.Controls.AddRange(new Control[] { btnOK, btnC });
                btnOK.Click += (_, __) => {
                    try { DatabaseManager.UpdateCuDanCanHo(maCuDan, maCanHo, cmbVaiTro.Text, dtKT.Value); Load(); f.Close(); }
                    catch (Exception ex) { MaterialMessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                };
                btnC.Click += (_, __) => f.Close(); f.ShowDialog();
            };
            btnDel.Click += (s, e) =>
            {
                if (dgv.SelectedRows.Count == 0) { MessageBox.Show("Chọn dòng cần xóa!"); return; }
                var row = dgv.SelectedRows[0];
                string maCuDan = SafeStr(row, "MaCuDan"), maCanHo = SafeStr(row, "MaCanHo");
                if (MessageBox.Show($"Xóa liên kết '{maCuDan}' - '{maCanHo}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                try { DatabaseManager.DeleteCuDanCanHo(maCuDan, maCanHo); Load(); }
                catch (Exception ex) { FkError(ex.Message); }
            };
            return MakeGridPanel(ts, dgv);
        }

        // ============================================================
        //  BẢNG GIÁ PANEL
        // ============================================================
        private Panel CreateBangGiaPanel()
        {
            var ts = new ToolStrip();
            var btnAdd = new ToolStripButton("➕ Thêm") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnEdit = new ToolStripButton("✏️ Sửa") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnDel = new ToolStripButton("🗑️ Xóa") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnRef = new ToolStripButton("🔄 Tải lại") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            ts.Items.AddRange(new ToolStripItem[] { btnAdd, btnEdit, btnDel, btnRef });
            var dgv = new DataGridView(); ConfigureGrid(dgv, true);
            void Load() { try { dgv.DataSource = DatabaseManager.GetAllBangGia(); } catch (Exception ex) { MaterialMessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); } }
            Load();
            btnRef.Click += (s, e) => Load();
            btnAdd.Click += (s, e) =>
            {
                var f = new Form { Text = "Thêm bảng giá", Width = 480, Height = 320, StartPosition = FormStartPosition.CenterParent };
                var (_, txtMa) = MakeRow(f, "Mã bảng giá:", 20, new TextBox());
                var (_, txtLoai) = MakeRow(f, "Mã loại xe:", 60, new TextBox());
                var (_, cmbTinhPhi) = MakeRow(f, "Loại tính phí:", 100, new ComboBox());
                cmbTinhPhi.Items.AddRange(new[] { "Theo ngày", "Theo tháng" }); cmbTinhPhi.SelectedIndex = 0;
                var (_, numDonGia) = MakeRow(f, "Đơn giá (VNĐ):", 140, new NumericUpDown { Minimum = 0, Maximum = 99999999, DecimalPlaces = 0, ThousandsSeparator = true });
                var (_, dtApDung) = MakeRow(f, "Ngày áp dụng:", 180, new DateTimePicker { Format = DateTimePickerFormat.Short, Value = DateTime.Today });
                var btnOK = MakeBtn("Lưu", 150, 240); var btnC = MakeBtn("Hủy", 260, 240);
                f.Controls.AddRange(new Control[] { btnOK, btnC });
                btnOK.Click += (_, __) => {
                    if (string.IsNullOrWhiteSpace(txtMa.Text) || string.IsNullOrWhiteSpace(txtLoai.Text)) { MessageBox.Show("Nhập đủ thông tin!"); return; }
                    try { DatabaseManager.AddBangGia(txtMa.Text, txtLoai.Text, cmbTinhPhi.Text, (decimal)numDonGia.Value, dtApDung.Value); Load(); f.Close(); }
                    catch (Exception ex) { MaterialMessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                };
                btnC.Click += (_, __) => f.Close(); f.ShowDialog();
            };
            btnEdit.Click += (s, e) =>
            {
                if (dgv.SelectedRows.Count == 0) { MessageBox.Show("Chọn dòng cần sửa!"); return; }
                var row = dgv.SelectedRows[0];
                var f = new Form { Text = "Sửa bảng giá", Width = 480, Height = 280, StartPosition = FormStartPosition.CenterParent };
                var (_, txtMa) = MakeRow(f, "Mã bảng giá:", 20, new TextBox { Text = SafeStr(row, "MaBangGia"), ReadOnly = true });
                var (_, txtLoai) = MakeRow(f, "Mã loại xe:", 60, new TextBox { Text = SafeStr(row, "MaLoaiXe") });
                var (_, cmbTinhPhi) = MakeRow(f, "Loại tính phí:", 100, new ComboBox());
                cmbTinhPhi.Items.AddRange(new[] { "Theo ngày", "Theo tháng" }); cmbTinhPhi.Text = SafeStr(row, "LoaiTinhPhi");
                var (_, numDonGia) = MakeRow(f, "Đơn giá (VNĐ):", 140, new NumericUpDown { Minimum = 0, Maximum = 99999999, DecimalPlaces = 0, ThousandsSeparator = true });
                if (decimal.TryParse(SafeStr(row, "DonGia"), out decimal dg)) numDonGia.Value = dg;
                var (_, cmbTT) = MakeRow(f, "Trạng thái:", 180, new ComboBox());
                cmbTT.Items.AddRange(new[] { "Đang hoạt động", "Ngừng áp dụng" }); cmbTT.Text = SafeStr(row, "TrangThai");
                var btnOK = MakeBtn("Lưu", 150, 225); var btnC = MakeBtn("Hủy", 260, 225);
                f.Controls.AddRange(new Control[] { btnOK, btnC });
                btnOK.Click += (_, __) => {
                    try { DatabaseManager.UpdateBangGia(txtMa.Text, txtLoai.Text, cmbTinhPhi.Text, (decimal)numDonGia.Value, cmbTT.Text); Load(); f.Close(); }
                    catch (Exception ex) { MaterialMessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                };
                btnC.Click += (_, __) => f.Close(); f.ShowDialog();
            };
            btnDel.Click += (s, e) =>
            {
                if (dgv.SelectedRows.Count == 0) { MessageBox.Show("Chọn dòng cần xóa!"); return; }
                string ma = SafeStr(dgv.SelectedRows[0], "MaBangGia");
                if (MessageBox.Show($"Xóa bảng giá '{ma}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                try { DatabaseManager.DeleteBangGia(ma); Load(); }
                catch (Exception ex) { FkError(ex.Message); }
            };
            return MakeGridPanel(ts, dgv);
        }

        // ============================================================
        //  SỰ CỐ PANEL
        // ============================================================
        private Panel CreateSuCoPanel()
        {
            var ts = new ToolStrip();
            var btnAdd = new ToolStripButton("➕ Báo sự cố") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnEdit = new ToolStripButton("✏️ Cập nhật") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnDel = new ToolStripButton("🗑️ Xóa") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnRef = new ToolStripButton("🔄 Tải lại") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            ts.Items.AddRange(new ToolStripItem[] { btnAdd, btnEdit, btnDel, btnRef });
            var dgv = new DataGridView(); ConfigureGrid(dgv, true);
            void Load() { try { dgv.DataSource = DatabaseManager.GetAllSuCo(); } catch (Exception ex) { MaterialMessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); } }
            Load();
            btnRef.Click += (s, e) => Load();
            btnAdd.Click += (s, e) =>
            {
                var f = new Form { Text = "Báo sự cố", Width = 480, Height = 260, StartPosition = FormStartPosition.CenterParent };
                var (_, txtLuotGui) = MakeRow(f, "Mã lượt gửi:", 20, new TextBox());
                var (_, txtNoi) = MakeRow(f, "Nội dung:", 60, new TextBox());
                var (_, numChiPhi) = MakeRow(f, "Chi phí (VNĐ):", 100, new NumericUpDown { Minimum = 0, Maximum = 999999999, DecimalPlaces = 0, ThousandsSeparator = true });
                var btnOK = MakeBtn("Lưu", 150, 150); var btnC = MakeBtn("Hủy", 260, 150);
                f.Controls.AddRange(new Control[] { btnOK, btnC });
                btnOK.Click += (_, __) => {
                    if (string.IsNullOrWhiteSpace(txtLuotGui.Text) || string.IsNullOrWhiteSpace(txtNoi.Text)) { MessageBox.Show("Nhập đủ thông tin!"); return; }
                    try { DatabaseManager.AddSuCo(txtLuotGui.Text, txtNoi.Text, (decimal)numChiPhi.Value); Load(); f.Close(); }
                    catch (Exception ex) { MaterialMessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                };
                btnC.Click += (_, __) => f.Close(); f.ShowDialog();
            };
            btnEdit.Click += (s, e) =>
            {
                if (dgv.SelectedRows.Count == 0) { MessageBox.Show("Chọn dòng cần cập nhật!"); return; }
                var row = dgv.SelectedRows[0];
                if (!int.TryParse(SafeStr(row, "MaSuCo"), out int maSuCo)) return;
                var f = new Form { Text = "Cập nhật sự cố", Width = 480, Height = 260, StartPosition = FormStartPosition.CenterParent };
                var (_, cmbTT) = MakeRow(f, "Trạng thái:", 20, new ComboBox());
                cmbTT.Items.AddRange(new[] { "Đang chờ xử lý", "Đang xử lý", "Đã xử lý xong" }); cmbTT.Text = SafeStr(row, "TrangThai");
                var (_, numChiPhi) = MakeRow(f, "Chi phí (VNĐ):", 60, new NumericUpDown { Minimum = 0, Maximum = 999999999, DecimalPlaces = 0, ThousandsSeparator = true });
                if (decimal.TryParse(SafeStr(row, "ChiPhi"), out decimal cp)) numChiPhi.Value = cp;
                var (_, chkXuLy) = MakeRow(f, "Đã xử lý hôm nay:", 100, new CheckBox());
                var btnOK = MakeBtn("Lưu", 150, 150); var btnC = MakeBtn("Hủy", 260, 150);
                f.Controls.AddRange(new Control[] { btnOK, btnC });
                btnOK.Click += (_, __) => {
                    try {
                        DateTime? ngayXuLy = chkXuLy.Checked ? (DateTime?)DateTime.Now : null;
                        DatabaseManager.UpdateSuCo(maSuCo, cmbTT.Text, (decimal)numChiPhi.Value, ngayXuLy);
                        Load(); f.Close();
                    }
                    catch (Exception ex) { MaterialMessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                };
                btnC.Click += (_, __) => f.Close(); f.ShowDialog();
            };
            btnDel.Click += (s, e) =>
            {
                if (dgv.SelectedRows.Count == 0) { MessageBox.Show("Chọn dòng cần xóa!"); return; }
                if (!int.TryParse(SafeStr(dgv.SelectedRows[0], "MaSuCo"), out int maSuCo)) return;
                if (MessageBox.Show($"Xóa sự cố #{maSuCo}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                try { DatabaseManager.DeleteSuCo(maSuCo); Load(); }
                catch (Exception ex) { FkError(ex.Message); }
            };
            return MakeGridPanel(ts, dgv);
        }

        // ============================================================
        //  THANH TOÁN PANEL
        // ============================================================
        private Panel CreateThanhToanPanel()
        {
            var ts = new ToolStrip();
            var btnAdd = new ToolStripButton("➕ Thêm") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnEdit = new ToolStripButton("✏️ Sửa trạng thái") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnDel = new ToolStripButton("🗑️ Xóa") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnRef = new ToolStripButton("🔄 Tải lại") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            ts.Items.AddRange(new ToolStripItem[] { btnAdd, btnEdit, btnDel, btnRef });
            var dgv = new DataGridView(); ConfigureGrid(dgv, true);
            void Load() { try { dgv.DataSource = DatabaseManager.GetAllThanhToan(); } catch (Exception ex) { MaterialMessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); } }
            Load();
            btnRef.Click += (s, e) => Load();
            btnAdd.Click += (s, e) =>
            {
                var f = new Form { Text = "Thêm thanh toán", Width = 500, Height = 340, StartPosition = FormStartPosition.CenterParent };
                var (_, txtMa) = MakeRow(f, "Mã thanh toán:", 20, new TextBox());
                var (_, cmbLoai) = MakeRow(f, "Loại thanh toán:", 60, new ComboBox());
                cmbLoai.Items.AddRange(new[] { "Ngày", "Tháng", "Sự cố", "Khác" }); cmbLoai.SelectedIndex = 0;
                var (_, numTien) = MakeRow(f, "Số tiền (VNĐ):", 100, new NumericUpDown { Minimum = 0, Maximum = 999999999, DecimalPlaces = 0, ThousandsSeparator = true });
                var (_, cmbPhuong) = MakeRow(f, "Phương thức:", 140, new ComboBox());
                cmbPhuong.Items.AddRange(new[] { "Tiền mặt", "Chuyển khoản", "Thẻ ngân hàng", "Ví điện tử" }); cmbPhuong.SelectedIndex = 0;
                var (_, txtNV) = MakeRow(f, "Mã nhân viên:", 180, new TextBox());
                var (_, cmbTT) = MakeRow(f, "Trạng thái:", 220, new ComboBox());
                cmbTT.Items.AddRange(new[] { "Thành công", "Đang xử lý", "Hoàn tiền" }); cmbTT.SelectedIndex = 0;
                var btnOK = MakeBtn("Lưu", 150, 270); var btnC = MakeBtn("Hủy", 260, 270);
                f.Controls.AddRange(new Control[] { btnOK, btnC });
                btnOK.Click += (_, __) => {
                    if (string.IsNullOrWhiteSpace(txtMa.Text)) { MessageBox.Show("Nhập mã thanh toán!"); return; }
                    try { DatabaseManager.AddThanhToan(txtMa.Text, cmbLoai.Text, (decimal)numTien.Value, cmbPhuong.Text, txtNV.Text, cmbTT.Text, ""); Load(); f.Close(); }
                    catch (Exception ex) { MaterialMessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                };
                btnC.Click += (_, __) => f.Close(); f.ShowDialog();
            };
            btnEdit.Click += (s, e) =>
            {
                if (dgv.SelectedRows.Count == 0) { MessageBox.Show("Chọn dòng cần sửa!"); return; }
                var row = dgv.SelectedRows[0]; string ma = SafeStr(row, "MaThanhToan");
                var f = new Form { Text = "Sửa trạng thái thanh toán", Width = 480, Height = 220, StartPosition = FormStartPosition.CenterParent };
                MakeRow(f, "Mã thanh toán:", 20, new TextBox { Text = ma, ReadOnly = true });
                var (_, cmbTT) = MakeRow(f, "Trạng thái:", 60, new ComboBox());
                cmbTT.Items.AddRange(new[] { "Thành công", "Đang xử lý", "Hoàn tiền" }); cmbTT.Text = SafeStr(row, "TrangThai");
                var (_, txtGC) = MakeRow(f, "Ghi chú:", 100, new TextBox { Text = SafeStr(row, "GhiChu") });
                var btnOK = MakeBtn("Lưu", 150, 145); var btnC = MakeBtn("Hủy", 260, 145);
                f.Controls.AddRange(new Control[] { btnOK, btnC });
                btnOK.Click += (_, __) => {
                    try { DatabaseManager.UpdateThanhToan(ma, cmbTT.Text, txtGC.Text); Load(); f.Close(); }
                    catch (Exception ex) { MaterialMessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                };
                btnC.Click += (_, __) => f.Close(); f.ShowDialog();
            };
            btnDel.Click += (s, e) =>
            {
                if (dgv.SelectedRows.Count == 0) { MessageBox.Show("Chọn dòng cần xóa!"); return; }
                string ma = SafeStr(dgv.SelectedRows[0], "MaThanhToan");
                if (MessageBox.Show($"Xóa thanh toán '{ma}'?\n(Sẽ xóa cả bản ghi con ThanhToanNgay/Thang liên quan)", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
                try { DatabaseManager.DeleteThanhToan(ma); Load(); }
                catch (Exception ex) { FkError(ex.Message); }
            };
            return MakeGridPanel(ts, dgv);
        }

        // ============================================================
        //  THANH TOÁN NGÀY PANEL
        // ============================================================
        private Panel CreateThanhToanNgayPanel()
        {
            var ts = new ToolStrip();
            var btnAdd = new ToolStripButton("➕ Thêm TT Ngày") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnDel = new ToolStripButton("🗑️ Xóa") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnRef = new ToolStripButton("🔄 Tải lại") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            ts.Items.AddRange(new ToolStripItem[] { btnAdd, btnDel, btnRef });
            var dgv = new DataGridView(); ConfigureGrid(dgv, true);
            void Load() { try { dgv.DataSource = DatabaseManager.GetAllThanhToanNgay(); } catch (Exception ex) { MaterialMessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); } }
            Load();
            btnRef.Click += (s, e) => Load();
            btnAdd.Click += (s, e) =>
            {
                var f = new Form { Text = "Thanh toán theo ngày", Width = 480, Height = 280, StartPosition = FormStartPosition.CenterParent };
                var (_, txtMaTT) = MakeRow(f, "Mã thanh toán:", 20, new TextBox());
                var (_, txtLuotGui) = MakeRow(f, "Mã lượt gửi:", 60, new TextBox());
                var (_, numTien) = MakeRow(f, "Số tiền (VNĐ):", 100, new NumericUpDown { Minimum = 0, Maximum = 999999999, DecimalPlaces = 0, ThousandsSeparator = true });
                var (_, cmbPhuong) = MakeRow(f, "Phương thức:", 140, new ComboBox());
                cmbPhuong.Items.AddRange(new[] { "Tiền mặt", "Chuyển khoản", "Thẻ ngân hàng" }); cmbPhuong.SelectedIndex = 0;
                var (_, txtNV) = MakeRow(f, "Mã nhân viên:", 180, new TextBox());
                var btnOK = MakeBtn("Lưu", 150, 220); var btnC = MakeBtn("Hủy", 260, 220);
                f.Controls.AddRange(new Control[] { btnOK, btnC });
                btnOK.Click += (_, __) => {
                    if (string.IsNullOrWhiteSpace(txtMaTT.Text) || string.IsNullOrWhiteSpace(txtLuotGui.Text)) { MessageBox.Show("Nhập đủ thông tin!"); return; }
                    try { DatabaseManager.AddThanhToanNgay(txtMaTT.Text, txtLuotGui.Text, (decimal)numTien.Value, cmbPhuong.Text, txtNV.Text); Load(); f.Close(); }
                    catch (Exception ex) { MaterialMessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                };
                btnC.Click += (_, __) => f.Close(); f.ShowDialog();
            };
            btnDel.Click += (s, e) =>
            {
                if (dgv.SelectedRows.Count == 0) { MessageBox.Show("Chọn dòng cần xóa!"); return; }
                string ma = SafeStr(dgv.SelectedRows[0], "MaThanhToan");
                if (MessageBox.Show($"Xóa thanh toán ngày '{ma}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
                try { DatabaseManager.DeleteThanhToan(ma); Load(); }
                catch (Exception ex) { FkError(ex.Message); }
            };
            return MakeGridPanel(ts, dgv);
        }

        // ============================================================
        //  THANH TOÁN THÁNG PANEL
        // ============================================================
        private Panel CreateThanhToanThangPanel()
        {
            var ts = new ToolStrip();
            var btnAdd = new ToolStripButton("➕ Thêm TT Tháng") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnDel = new ToolStripButton("🗑️ Xóa") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnRef = new ToolStripButton("🔄 Tải lại") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            ts.Items.AddRange(new ToolStripItem[] { btnAdd, btnDel, btnRef });
            var dgv = new DataGridView(); ConfigureGrid(dgv, true);
            void Load() { try { dgv.DataSource = DatabaseManager.GetAllThanhToanThang(); } catch (Exception ex) { MaterialMessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); } }
            Load();
            btnRef.Click += (s, e) => Load();
            btnAdd.Click += (s, e) =>
            {
                var f = new Form { Text = "Thanh toán theo tháng", Width = 500, Height = 340, StartPosition = FormStartPosition.CenterParent };
                var (_, txtMaTT) = MakeRow(f, "Mã thanh toán:", 20, new TextBox());
                var (_, txtMaThe) = MakeRow(f, "Mã thẻ xe:", 60, new TextBox());
                var (_, numTien) = MakeRow(f, "Số tiền (VNĐ):", 100, new NumericUpDown { Minimum = 0, Maximum = 999999999, DecimalPlaces = 0, ThousandsSeparator = true });
                var (_, cmbPhuong) = MakeRow(f, "Phương thức:", 140, new ComboBox());
                cmbPhuong.Items.AddRange(new[] { "Tiền mặt", "Chuyển khoản", "Thẻ ngân hàng" }); cmbPhuong.SelectedIndex = 0;
                var (_, txtNV) = MakeRow(f, "Mã nhân viên:", 180, new TextBox());
                var (_, dtTu) = MakeRow(f, "Từ ngày:", 220, new DateTimePicker { Format = DateTimePickerFormat.Short, Value = DateTime.Today });
                var (_, dtDen) = MakeRow(f, "Đến ngày:", 260, new DateTimePicker { Format = DateTimePickerFormat.Short, Value = DateTime.Today.AddMonths(1) });
                var btnOK = MakeBtn("Lưu", 150, 300); var btnC = MakeBtn("Hủy", 260, 300);
                f.Controls.AddRange(new Control[] { btnOK, btnC });
                btnOK.Click += (_, __) => {
                    if (string.IsNullOrWhiteSpace(txtMaTT.Text) || string.IsNullOrWhiteSpace(txtMaThe.Text)) { MessageBox.Show("Nhập đủ thông tin!"); return; }
                    try { DatabaseManager.AddThanhToanThang(txtMaTT.Text, txtMaThe.Text, (decimal)numTien.Value, cmbPhuong.Text, txtNV.Text, dtTu.Value, dtDen.Value); Load(); f.Close(); }
                    catch (Exception ex) { MaterialMessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                };
                btnC.Click += (_, __) => f.Close(); f.ShowDialog();
            };
            btnDel.Click += (s, e) =>
            {
                if (dgv.SelectedRows.Count == 0) { MessageBox.Show("Chọn dòng cần xóa!"); return; }
                string ma = SafeStr(dgv.SelectedRows[0], "MaThanhToan");
                if (MessageBox.Show($"Xóa thanh toán tháng '{ma}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
                try { DatabaseManager.DeleteThanhToan(ma); Load(); }
                catch (Exception ex) { FkError(ex.Message); }
            };
            return MakeGridPanel(ts, dgv);
        }

        // ============================================================
        //  LỊCH SỬ THẺ XE PANEL
        // ============================================================
        private Panel CreateLichSuTheXePanel()
        {
            var ts = new ToolStrip();
            var btnAdd = new ToolStripButton("➕ Ghi lịch sử") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnDel = new ToolStripButton("🗑️ Xóa") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnRef = new ToolStripButton("🔄 Tải lại") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            ts.Items.AddRange(new ToolStripItem[] { btnAdd, btnDel, btnRef });
            var dgv = new DataGridView(); ConfigureGrid(dgv, true);
            void Load() { try { dgv.DataSource = DatabaseManager.GetAllLichSuTheXe(); } catch (Exception ex) { MaterialMessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); } }
            Load();
            btnRef.Click += (s, e) => Load();
            btnAdd.Click += (s, e) =>
            {
                var f = new Form { Text = "Ghi lịch sử thẻ xe", Width = 480, Height = 260, StartPosition = FormStartPosition.CenterParent };
                var (_, txtMaThe) = MakeRow(f, "Mã thẻ:", 20, new TextBox());
                var (_, txtCu) = MakeRow(f, "Trạng thái cũ:", 60, new TextBox());
                var (_, txtMoi) = MakeRow(f, "Trạng thái mới:", 100, new TextBox());
                var (_, txtGC) = MakeRow(f, "Ghi chú:", 140, new TextBox());
                var btnOK = MakeBtn("Lưu", 150, 185); var btnC = MakeBtn("Hủy", 260, 185);
                f.Controls.AddRange(new Control[] { btnOK, btnC });
                btnOK.Click += (_, __) => {
                    if (string.IsNullOrWhiteSpace(txtMaThe.Text)) { MessageBox.Show("Nhập mã thẻ!"); return; }
                    try { DatabaseManager.AddLichSuTheXe(txtMaThe.Text, txtCu.Text, txtMoi.Text, txtGC.Text); Load(); f.Close(); }
                    catch (Exception ex) { MaterialMessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                };
                btnC.Click += (_, __) => f.Close(); f.ShowDialog();
            };
            btnDel.Click += (s, e) =>
            {
                if (dgv.SelectedRows.Count == 0) { MessageBox.Show("Chọn dòng cần xóa!"); return; }
                if (!int.TryParse(SafeStr(dgv.SelectedRows[0], "MaLichSu"), out int maLS)) return;
                if (MessageBox.Show($"Xóa bản ghi lịch sử #{maLS}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                try { DatabaseManager.DeleteLichSuTheXe(maLS); Load(); }
                catch (Exception ex) { FkError(ex.Message); }
            };
            return MakeGridPanel(ts, dgv);
        }

        // ============================================================
        //  LỊCH SỬ VỊ TRÍ ĐỖ PANEL
        // ============================================================
        private Panel CreateLichSuViTriDoPanel()
        {
            var ts = new ToolStrip();
            var btnAdd = new ToolStripButton("➕ Ghi lịch sử") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnDel = new ToolStripButton("🗑️ Xóa") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            var btnRef = new ToolStripButton("🔄 Tải lại") { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            ts.Items.AddRange(new ToolStripItem[] { btnAdd, btnDel, btnRef });
            var dgv = new DataGridView(); ConfigureGrid(dgv, true);
            void Load() { try { dgv.DataSource = DatabaseManager.GetAllLichSuViTriDo(); } catch (Exception ex) { MaterialMessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); } }
            Load();
            btnRef.Click += (s, e) => Load();
            btnAdd.Click += (s, e) =>
            {
                var f = new Form { Text = "Ghi lịch sử vị trí đỗ", Width = 480, Height = 300, StartPosition = FormStartPosition.CenterParent };
                var (_, txtViTri) = MakeRow(f, "Mã vị trí:", 20, new TextBox());
                var (_, txtMaThe) = MakeRow(f, "Mã thẻ:", 60, new TextBox());
                var (_, dtBD) = MakeRow(f, "Thời gian bắt đầu:", 100, new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy HH:mm", ShowUpDown = true });
                var (_, chkKT) = MakeRow(f, "Có thời gian kết thúc:", 140, new CheckBox());
                var (_, dtKT) = MakeRow(f, "Thời gian kết thúc:", 180, new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy HH:mm", ShowUpDown = true, Enabled = false });
                chkKT.CheckedChanged += (_, __) => dtKT.Enabled = chkKT.Checked;
                var (_, txtGC) = MakeRow(f, "Ghi chú:", 220, new TextBox());
                var btnOK = MakeBtn("Lưu", 150, 255); var btnC = MakeBtn("Hủy", 260, 255);
                f.Controls.AddRange(new Control[] { btnOK, btnC });
                f.Height = 320;
                btnOK.Click += (_, __) => {
                    if (string.IsNullOrWhiteSpace(txtViTri.Text) || string.IsNullOrWhiteSpace(txtMaThe.Text)) { MessageBox.Show("Nhập đủ thông tin!"); return; }
                    try {
                        DateTime? kt = chkKT.Checked ? (DateTime?)dtKT.Value : null;
                        DatabaseManager.AddLichSuViTriDo(txtViTri.Text, txtMaThe.Text, dtBD.Value, kt, txtGC.Text);
                        Load(); f.Close();
                    }
                    catch (Exception ex) { MaterialMessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                };
                btnC.Click += (_, __) => f.Close(); f.ShowDialog();
            };
            btnDel.Click += (s, e) =>
            {
                if (dgv.SelectedRows.Count == 0) { MessageBox.Show("Chọn dòng cần xóa!"); return; }
                if (!int.TryParse(SafeStr(dgv.SelectedRows[0], "MaLichSu"), out int maLS)) return;
                if (MessageBox.Show($"Xóa bản ghi lịch sử vị trí #{maLS}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                try { DatabaseManager.DeleteLichSuViTriDo(maLS); Load(); }
                catch (Exception ex) { FkError(ex.Message); }
            };
            return MakeGridPanel(ts, dgv);
        }
    }
}
