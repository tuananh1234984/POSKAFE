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
using PosKafe.BUS;
using PosKafe.DTO;
using PosKafe.DAO;

namespace PosKafe
{
    public partial class Form_login : MetroForm
    {
        public Form_login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            // 1. Kiểm tra nhập liệu
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MetroMessageBox.Show(this, "Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // 2. Gọi BUS để kiểm tra 
            // Gọi hàm KiemTraDangNhap từ TaiKhoanBus
            try
            {
                if (TaiKhoanDao.Instance.KiemTraDangNhap(username, password)){
                    AccountDTO loginAccount = TaiKhoanDao.Instance.GetAccountByUserName(username);

                    Form_main f = new Form_main(loginAccount);

                    this.Hide();
                    f.ShowDialog();

                    this.Show();
                    txtPassword.Clear();
                }
                else
                {
                    MetroMessageBox.Show(this, "Tên đăng nhập hoặc mật khẩu không đúng!", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtPassword.Focus();

                }
            }
            catch (Exception ex){
                MetroMessageBox.Show(this, "Đã xảy ra lỗi trong quá trình đăng nhập.\nChi tiết lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void Form_login_Load(object sender, EventArgs e)
        {

        }
    }
}
