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
using PosKafe.DAO;
using PosKafe.DTO;
namespace PosKafe
{
    public partial class Form_NhanSu : MetroForm
    {
        BindingSource accountList = new BindingSource();
        public Form_NhanSu()
        {
            InitializeComponent();
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.None;
            this.ControlBox = false;
            this.Text = "";

            dtgvAccount.DataSource = accountList;
            LoadAccount();
        }
        void LoadAccount()
        {
            accountList.DataSource = TaiKhoanDao.Instance.GetListAccount();
            dtgvAccount.Columns["TenDangNhap"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dtgvAccount.Columns["TenHienThi"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dtgvAccount.Columns["LoaiTaiKhoan"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // Header tiếng việt
            dtgvAccount.Columns["TenDangNhap"].HeaderText = "Tên đăng nhập";
            dtgvAccount.Columns["TenHienThi"].HeaderText = "Tên hiển thị";
            dtgvAccount.Columns["LoaiTaiKhoan"].HeaderText = "Loại TK (1:Admin)";
        }

        private void dtgvAccount_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == 0)
            {
                DataGridViewRow row = dtgvAccount.Rows[e.RowIndex];

                txtUsername.Text = row.Cells["TenDangNhap"].Value.ToString();
                txtDisplayName.Text = row.Cells["TenHienThi"].Value.ToString();

                // Xử lý loại tài khoản (Map số 1 -> Quản lý, 0 -> Nhân viên)
                int type = (int)row.Cells["LoaiTaiKhoan"].Value;
                if (type == 1) cbRole.Text = "Quản Lý";
                else cbRole.Text = "Nhân viên";
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string user = txtUsername.Text;
            string display = txtDisplayName.Text;
            int type = (cbRole.Text == "Quản lý") ? 1 : 0;

            if (TaiKhoanDao.Instance.InsertAccount(user, display, type))
            {
                MessageBox.Show("Thêm tài khoản thành công! Mật khẩu mặc định là: 1");
                LoadAccount();
            }else
            {
                MessageBox.Show("Thêm thất bại (Có thể trùng tên đăng nhập)");
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            string user = txtUsername.Text;
            string display = txtDisplayName.Text;
            int type = (cbRole.Text == "Quản lý") ? 1 : 0;

            if (TaiKhoanDao.Instance.UpdateAccount(user, display, type))
            {
                MessageBox.Show("Cập nhật thành công");
                LoadAccount();
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string user = txtUsername.Text;

            if (user == "admin") return;
            if (TaiKhoanDao.Instance.DeleteAccount(user)){
                MessageBox.Show("Xóa thành công");
            }
            else MessageBox.Show("Xóa thất bại");
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            string user = txtUsername.Text;
            if (TaiKhoanDao.Instance.ResetPassword(user))
            {
                MessageBox.Show("Đã đặt lại mật khẩu về mặc định; 1");
            }
        }
    }
}
