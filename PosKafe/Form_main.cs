using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using MetroFramework;


namespace PosKafe
{
    public partial class Form_main : MetroForm
    {
        private Form activeForm = null;
        public Form_main()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void FormLoad(Form formCon)
        {
            // Xóa tất cả các control hiện có khỏi panel
            panelContent.Controls.Clear();

            // Thiết lập và hiển thị Form con mới
            activeForm = formCon;
            formCon.TopLevel = false;
            formCon.FormBorderStyle = FormBorderStyle.None;
            formCon.Dock = DockStyle.Fill;

            panelContent.Controls.Add(formCon);
            formCon.BringToFront();
            formCon.Show();
        }

        private void tileQuanLyNhanSu_Click(object sender, EventArgs e)
        {
            FormLoad(new Form_NhanSu());
        }

        private void tileBanHang_Click(object sender, EventArgs e)
        {
            FormLoad(new Form_BanHang());
        }

        private void tileBaoCao_Click(object sender, EventArgs e)
        {
            FormLoad(new Form_BaoCao());
        }

        private void tileQuanLyMenu_Click(object sender, EventArgs e)
        {
            FormLoad(new Form_QuanLyMenu());
        }

        private void tileQuanLyKho_Click(object sender, EventArgs e)
        {
            FormLoad(new Form_QuanLyKho());
        }

        private void tileQuanLyKhuyenMai_Click(object sender, EventArgs e)
        {
            FormLoad(new Form_KhuyenMai());
        }

        private void tileCaiDat_Click(object sender, EventArgs e)
        {
            FormLoad(new Form_CaiDat());
        }
    }
}