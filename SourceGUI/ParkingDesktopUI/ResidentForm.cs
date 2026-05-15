using System;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;

namespace QL_BaiDoXe
{
    public partial class ResidentForm : MaterialForm
    {
        private readonly string _hoTen;

        public ResidentForm(string hoTen)
        {
            _hoTen = hoTen;

            InitializeComponent();

            MaterialSkinManager.Instance.AddFormToManage(this);

            lblWelcome.Text = $"Xin chào, Cư dân: {_hoTen}";
        }
    }
}
