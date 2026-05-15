using System;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;

namespace QL_BaiDoXe
{
    public partial class AdminForm : MaterialForm
    {
        private readonly string _hoTen;
        private readonly string _maVaiTro;

        public AdminForm(string hoTen, string maVaiTro)
        {
            _hoTen    = hoTen;
            _maVaiTro = maVaiTro;

            InitializeComponent();

            MaterialSkinManager.Instance.AddFormToManage(this);

            lblWelcome.Text = $"Xin chào, Quản trị viên: {_hoTen}";
        }
    }
}
