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
    public partial class Form_QuanLyMenu : MetroForm
    {
        BindingSource foodlist = new BindingSource();
        public Form_QuanLyMenu()
        {
            InitializeComponent();
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.None;
            this.ControlBox = false;
            this.Text = "";

            nmGiaBan.Maximum = 1000000000;
            nmGiaVon.Maximum = 1000000000;

            nmGiaBan.Minimum = 0;
            nmGiaVon.Minimum = 0;
            dtgvFood.DataSource = foodlist;

            LoadListFood();
            LoadCategory();

        }

        // --- 1. Các hàm load dữ liệu ---
        void LoadListFood()
        {
            foodlist.DataSource = MenuDAO.Instance.GetListFood();

            dtgvFood.Columns["TenSanPham"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            // 2. Các cột còn lại tự co vừa khít nội dung (AllCells) hoặc Tiêu đề (ColumnHeader)
            dtgvFood.Columns["ID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dtgvFood.Columns["Gia"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dtgvFood.Columns["GiaVon"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dtgvFood.Columns["DonViTinh"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            // 3. Căn giữa tiêu đề và nội dung cho đẹp (Optional)
            dtgvFood.Columns["ID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtgvFood.Columns["Gia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; // Tiền thì căn phải
            dtgvFood.Columns["GiaVon"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dtgvFood.Columns["ID"].HeaderText = "Mã món";
            dtgvFood.Columns["TenSanPham"].HeaderText = "Tên món";
            dtgvFood.Columns["Gia"].HeaderText = "Giá bán";
            dtgvFood.Columns["GiaVon"].HeaderText = "Giá vốn";
            dtgvFood.Columns["DonViTinh"].HeaderText = "Đơn vị tính";
            dtgvFood.Columns["TrangThai"].HeaderText = "Đang bán";


            if (dtgvFood.Columns["Gia"] != null)
                dtgvFood.Columns["Gia"].DefaultCellStyle.Format = "N0"; //Định dạng số với dấu phân cách hàng nghìn
            if (dtgvFood.Columns["GiaVon"] != null)
                dtgvFood.Columns["GiaVon"].DefaultCellStyle.Format = "N0"; //Định dạng số với dấu phân cách hàng nghìn

            //Ẩn cột ID nhóm nếu thấy rườm rà
            if (dtgvFood.Columns["ID_Nhom"] != null)
                dtgvFood.Columns["ID_Nhom"].Visible = false;
        }

        void LoadCategory()
        {
            List<NhomSanPhamDTO> listCategory = MenuDAO.Instance.GetListNhomSanPham();
            cbDanhMuc.DataSource = listCategory;
            cbDanhMuc.DisplayMember = "TenNhom";
            cbDanhMuc.ValueMember = "ID";

        }

        // --- 2. Sự kiện click vào bảng (đổ dữ liệu ngược lên ô nhận) ---

        private void Form_QuanLyMenu_Load(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void chBoxNgungBan_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void dtgvFood_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == 0)
            {
                DataGridViewRow row = dtgvFood.Rows[e.RowIndex];

                // Đổ dữ liệu text
                txtMaMonID.Text = row.Cells["ID"].Value.ToString();
                txtTenMon.Text = row.Cells["TenSanPham"].Value.ToString();

                // Đổ dữ liệu số (numericupdown)
                nmGiaBan.Value = Convert.ToDecimal(row.Cells["Gia"].Value);

                // Kiểm tra null cho Giá Vốn (phòng khi DB cũ bị null)
                if (row.Cells["GiaVon"].Value != DBNull.Value)
                    nmGiaVon.Value = Convert.ToDecimal(row.Cells["GiaVon"].Value);
                else
                    nmGiaVon.Value = 0;

                // Xử lý CheckBox Trạng thái
                try
                {
                    chboxTrangThai.Checked = Convert.ToBoolean(row.Cells["TrangThai"].Value);
                }catch(Exception ex)
                {
                    chboxTrangThai.Checked = true;
                }

                // Xử lý ComboBox (Chọn đúng danh mục của món đó)
                int idNhom = Convert.ToInt32(row.Cells["ID_Nhom"].Value);
                cbDanhMuc.SelectedValue = idNhom;
            }
        }
        // --- 3. Các nút chức năng (CRUD) ---

        // Nút Thêm món
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string name = txtTenMon.Text;
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Vui lòng nhập tên món!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //2. lấy giá trị
            int idCategory = (int) cbDanhMuc.SelectedValue;
            float price = (float)nmGiaBan.Value;
            decimal GiaVon = nmGiaVon.Value;
            int trangThai = chboxTrangThai.Checked ? 1 : 0;
            string dvt = "Ly"; // Mặc định ĐVT là 'Ly'

            //3. Gọi món
            if (MenuDAO.Instance.InsertFood(name, idCategory, price, GiaVon, dvt, trangThai))
            {
                MessageBox.Show("Thêm món thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadListFood();
                btnShow_Click(null, null); // Reset ô nhập
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm món!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Nút Làm mới (reset ô nhập để thêm mới)
        private void btnShow_Click(object sender, EventArgs e)
        {
            txtMaMonID.Text = "";
            txtTenMon.Text = "";
            nmGiaBan.Value = 0;
            nmGiaVon.Value = 0;
            chboxTrangThai.Checked = true;
            cbDanhMuc.SelectedIndex = 0;
            txtTenMon.Focus();// Đưa chuột vào ô tên
            LoadListFood();// Load lại danh sách gốc
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaMonID.Text))
            {
                MessageBox.Show("Vui lòng chọn món để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int id = Convert.ToInt32(txtMaMonID.Text);
            string name = txtTenMon.Text;
            int idCategory = (int)cbDanhMuc.SelectedValue;
            float price = (float)nmGiaBan.Value;
            decimal GiaVon = nmGiaVon.Value;
            int trangThai = chboxTrangThai.Checked ? 1 : 0;
            string dvt = "Ly"; // Mặc định ĐVT là 'Ly'

            if (MenuDAO.Instance.UpdateFood(id, name, idCategory, price, GiaVon, dvt, trangThai))
            {
                MessageBox.Show("Sửa món thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadListFood();
                btnShow_Click(null, null); // Reset ô nhập
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa món!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaMonID.Text))
            {
                MessageBox.Show("Vui lòng chọn món để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(txtMaMonID.Text);
            try
            {
                if (MenuDAO.Instance.DeleteFood(id))
                {
                    MessageBox.Show("Xóa món thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadListFood();
                    btnShow_Click(null, null); // Reset ô nhập
                }
                else
                {
                    MessageBox.Show("Có lỗi khi xóa món!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }catch (Exception ex)
            {
                MessageBox.Show("Không thể xóa món này vì có liên quan đến dữ liệu khác.\nChi tiết lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text;
            List<SanPhamDTO> searchList = MenuDAO.Instance.SearchFoodByName(keyword);

            foodlist.DataSource = searchList;
        }
    }
}
