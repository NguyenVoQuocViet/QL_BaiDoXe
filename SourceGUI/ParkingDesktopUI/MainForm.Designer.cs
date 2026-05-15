namespace QL_BaiDoXe
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TabControl materialTabControl1;
        private System.Windows.Forms.Panel pnlTabBar;          
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Button btnLogout;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.materialTabControl1 = new System.Windows.Forms.TabControl();
            this.pnlTabBar           = new System.Windows.Forms.Panel();
            this.lblWelcome          = new System.Windows.Forms.Label();

            this.SuspendLayout();

            // ── lblWelcome ─────────────────────────────────────────
            this.lblWelcome.AutoSize  = false;
            this.lblWelcome.Dock      = System.Windows.Forms.DockStyle.Top;
            this.lblWelcome.Font      = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.Location  = new System.Drawing.Point(0, 60);
            this.lblWelcome.Name      = "lblWelcome";
            this.lblWelcome.Padding   = new System.Windows.Forms.Padding(16, 8, 0, 8);
            this.lblWelcome.Size      = new System.Drawing.Size(1400, 40);
            this.lblWelcome.TabIndex  = 0;
            this.lblWelcome.Text      = "Xin chào...";
            this.lblWelcome.BackColor = System.Drawing.Color.FromArgb(30, 136, 229);
            this.lblWelcome.ForeColor = System.Drawing.Color.White;

            // ── materialTabControl1 (standard, Multiline=false → scrollable) ─
            this.materialTabControl1.Dock          = System.Windows.Forms.DockStyle.Fill;
            this.materialTabControl1.Location      = new System.Drawing.Point(0, 140);
            this.materialTabControl1.Name          = "materialTabControl1";
            this.materialTabControl1.SelectedIndex = 0;
            this.materialTabControl1.TabIndex      = 2;
            this.materialTabControl1.Multiline     = false;   // single row → arrows appear automatically
            this.materialTabControl1.SizeMode      = System.Windows.Forms.TabSizeMode.Fixed;
            this.materialTabControl1.ItemSize      = new System.Drawing.Size(130, 36);
            this.materialTabControl1.Font          = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.materialTabControl1.Appearance    = System.Windows.Forms.TabAppearance.Normal;

            // ── MainForm ──────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(1400, 760);
            this.Controls.Add(this.materialTabControl1);
            this.Name = "MainForm";
            this.Text = "Quản lý Bãi Đỗ Xe";
            this.Load += new System.EventHandler(this.MainForm_Load);

            // ── pnlHeader ──────────────────────────────────────────
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 44;
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(30, 136, 229);

            // ── btnLogout ──────────────────────────────────────────
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnLogout.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnLogout.Width = 130;
            this.btnLogout.Text = "⏻  Đăng xuất";
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            
            this.btnLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(229, 57, 53);
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);

            // Chuyển lblWelcome vào pnlHeader, xóa dòng Controls.Add(this.lblWelcome) cũ
            this.lblWelcome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWelcome.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.pnlHeader.Controls.Add(this.lblWelcome);
            this.pnlHeader.Controls.Add(this.btnLogout);
            this.Controls.Add(this.pnlHeader);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
