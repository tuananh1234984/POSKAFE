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
    public partial class Form_KhuyenMai : MetroForm
    {
        private bool isThem = false; // Biến cờ để phân biệt Thêm hay Sửa
        public Form_KhuyenMai()
        {
            InitializeComponent();
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.None;
            this.ControlBox = false;
            this.Text = "";
        }

        private void Form_KhuyenMai_Load(object sender, EventArgs e)
        {
            //1. Khi form mở lên: khóa nhập liệu, chỉ cho xem danh sách
            SetTrangThai(true);
            LoadDataGrid();
        }

        private void SetTrangThai(bool xemChiTiet)
        {
            // Nếu đang xem: Khóa ô nhập, hiện nút Thêm/Sửa/Xóa
            // Nếu đang thêm/sửa: Mở ô nhập, hiện nút Lưu/Hủy

            txtMaKM.Enabled = !xemChiTiet;
            txtTenKM.Enabled = !xemChiTiet;
            numGiaTri.Enabled = !xemChiTiet;
            dtpNgayBatDau.Enabled = !xemChiTiet;
            dtpNgayKetThuc.Enabled = !xemChiTiet;

            // Các nút chức năng
            btnThem.Enabled = xemChiTiet;
            btnSua.Enabled = xemChiTiet;
            btnXoa.Enabled = xemChiTiet;

            btnLuu.Enabled = !xemChiTiet;
            btnHuy.Enabled = !xemChiTiet;
        }

        // Hàm xóa trắng ô nhập để chuẩn bị nhập mới

        private void ResetInput()
        {
            txtMaKM.Clear();
            txtTenKM.Clear();
            numGiaTri.Value = 0;
            dtpNgayBatDau.Value = DateTime.Now;
            dtpNgayKetThuc.Value = DateTime.Now;
            chkTrangThai.Checked = true;
        }
        // --- các sự kiện nút bấm ---
        private void btnThem_Click(object sender, EventArgs e)
        {
            isThem = true; // Đánh dấu là đang thêm mới
            SetTrangThai(false); // Mở ô nhập
            ResetInput(); // Xóa trắng ô nhập
            txtMaKM.Focus(); // Đặt con trỏ vào ô Mã KM
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaKM.Text == "")
            {
                MessageBox.Show("Vui lòng chọn dòng cần sửa trên lưới!", "Thông báo");
                return;
            }

            isThem = false; // Đánh dấu là đang sửa
            SetTrangThai(false); // Mở khóa nhập liệu
            txtMaKM.Enabled = false; // Không cho sửa Mã (Primary Key)
            txtTenKM.Focus();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaKM.Text))
            {
                MessageBox.Show("Chưa nhập mã km!", "Lỗi");
                txtMaKM.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtTenKM.Text))
            {
                MessageBox.Show("Chưa nhập tên KM!", "Lỗi");
                txtTenKM.Focus();
                return;
            }
            if (dtpNgayKetThuc.Value < dtpNgayBatDau.Value)
            {
                MessageBox.Show("Ngày kết thúc phải sau ngày bắt đầu!", "Lỗi");
                dtpNgayKetThuc.Focus();
                return;
            }
            // 2. Thực hiện Lưu xuống CSDL (SQL)
            if (isThem)
            {
                MessageBox.Show("đã thêm mới thành công! (Demo)");
            }
            else
            {
                // Gọi hàm update SQL ở đây
                MessageBox.Show("đã cập nhật thành công! (Demo)");
            }

            //3. Quay về trạng thái ban đầu
            SetTrangThai(true);
            LoadDataGrid();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            SetTrangThai(true);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn xóa KM này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                // Gọi hàm DELETE SQL ở đây
                MessageBox.Show("Đã xóa thành công!");
                LoadDataGrid();
                ResetInput();
            }
        }

        private void LoadDataGrid()
        {
            // Tạo bảng ảo để test giao diện
            DataTable dt = new DataTable();
            dt.Columns.Add("MaKM");
            dt.Columns.Add("TenKM");
            dt.Columns.Add("GiaTri");

            dt.Rows.Add("SUMMER2025", "Chào hè rực rỡ", "10%");
            dt.Rows.Add("TEST01", "Khai trương quán", "50,000");

            dgvKhuyenMai.DataSource = dt;
        }

        private void dgvKhuyenMai_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Tránh click vào header
            {
                DataGridViewRow row = dgvKhuyenMai.Rows[e.RowIndex];

                txtMaKM.Text = row.Cells["MaKM"].Value.ToString();
                txtTenKM.Text = row.Cells["TenKM"].Value.ToString();
                numDonToiThieu.Value = row.Cells["GiaTri"].Value != DBNull.Value ? Convert.ToDecimal(row.Cells["GiaTri"].Value) : 0;
                numGiaTri.Value = row.Cells["GiaTri"].Value != DBNull.Value ? Convert.ToDecimal(row.Cells["GiaTri"].Value) : 0;
                // Thêm các trường khác tương tự...
            }
        }
    }
}
