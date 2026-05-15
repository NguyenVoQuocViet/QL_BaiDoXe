using System;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;

namespace QL_BaiDoXe
{
    public partial class StaffForm : MaterialForm
    {
        private readonly string _hoTen;
        private readonly string _maVaiTro;

        public StaffForm(string hoTen, string maVaiTro)
        {
            _hoTen    = hoTen;
            _maVaiTro = maVaiTro;

            InitializeComponent();

            MaterialSkinManager.Instance.AddFormToManage(this);

            lblWelcome.Text = $"Xin chào, Nhân viên: {_hoTen}";
        }
    }
}
