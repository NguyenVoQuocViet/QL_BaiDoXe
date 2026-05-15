using System;
using System.Windows.Forms;
using MaterialSkin;

namespace QL_BaiDoXe
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // ── Khởi tạo MaterialSkin toàn cục ──────────────────────
            // (Instance được LoginForm dùng lại, không cần khởi tạo lần 2)
            var skinManager = MaterialSkinManager.Instance;
            skinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            skinManager.ColorScheme = new ColorScheme(
                Primary.Blue700,
                Primary.Blue900,
                Primary.Blue500,
                Accent.LightBlue200,
                TextShade.WHITE
            );

            // ── Chạy ứng dụng bắt đầu từ LoginForm ─────────────────
            Application.Run(new LoginForm());
        }
    }
}