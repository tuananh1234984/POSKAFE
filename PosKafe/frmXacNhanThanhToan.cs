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
using System.Net;
using System.Net.Mail;
using PosKafe.DTO;

namespace PosKafe
{
    public partial class frmXacNhanThanhToan : MetroForm
    {
        public string EmailKhachHang { get; private set; }
        public frmXacNhanThanhToan(decimal tongTien)
        {
            InitializeComponent();
            // Hiển thị số tiền thật to để check
            this.lblTongTien.Text = tongTien.ToString("N0") + " VND";
            this.txtEmail.Focus();
        }

        private void frmXacNhanThanhToan_Load(object sender, EventArgs e)
        {

        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            // Lưu email khách hàng và đóng form
            EmailKhachHang = txtEmail.Text.Trim();

            // Trả về kết quả OK báo hiệu người dùng đồng ý thanh toán
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
