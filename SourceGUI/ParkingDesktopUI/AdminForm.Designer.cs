namespace QL_BaiDoXe
{
    partial class AdminForm
    {
        private System.ComponentModel.IContainer components = null;

        private MaterialSkin.Controls.MaterialLabel  lblWelcome;
        private MaterialSkin.Controls.MaterialLabel  lblStub;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblWelcome = new MaterialSkin.Controls.MaterialLabel();
            this.lblStub    = new MaterialSkin.Controls.MaterialLabel();

            this.SuspendLayout();

            // Form
            this.Text          = "Admin – Quản lý Bãi Đỗ Xe";
            this.ClientSize    = new System.Drawing.Size(1024, 680);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            // lblWelcome
            this.lblWelcome.AutoSize  = true;
            this.lblWelcome.Depth     = 0;
            this.lblWelcome.Font      = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.Location  = new System.Drawing.Point(30, 80);
            this.lblWelcome.MouseState = MaterialSkin.MouseState.HOVER;
            this.lblWelcome.Text      = "Xin chào, Quản trị viên";

            // lblStub
            this.lblStub.AutoSize  = true;
            this.lblStub.Depth     = 0;
            this.lblStub.Font      = new System.Drawing.Font("Roboto", 10F);
            this.lblStub.ForeColor = System.Drawing.Color.Gray;
            this.lblStub.Location  = new System.Drawing.Point(30, 120);
            this.lblStub.MouseState = MaterialSkin.MouseState.HOVER;
            this.lblStub.Text      = "[ AdminForm – stub. Thêm TabControl / MenuStrip tại đây ]";

            this.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.lblWelcome,
                this.lblStub,
            });

            this.ResumeLayout(false);
        }
    }
}
