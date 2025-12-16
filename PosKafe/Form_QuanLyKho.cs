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
    public partial class Form_QuanLyKho : MetroForm
    {
        private bool isThemNL = false; // Biến cờ để phân biệt Thêm hay Sửa
        private DataTable dtGioHangNhap;
        public Form_QuanLyKho()
        {
            InitializeComponent();
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.None;
            this.ControlBox = false;
            this.Text = "";
        }

        private void Form_QuanLyKho_Load(object sender, EventArgs e)
        {
            // 1. Cấu hình Tab 1 (Danh sách nguyên liệu)
            SetTrangThaiTab1(true); // Khóa ô nhập
            LoadDataNguyenLieu();   // Load dữ liệu lên Grid

            // 2. Cấu hình Tab 2 (Nhập kho)
            KhoiTaoGioHangNhap();   // Tạo cấu trúc bảng tạm
            LoadComboNguyenLieu();  // Load danh sách NL vào ComboBox để chọn
        }

        // =================================================================================
        // PHẦN 1: LOGIC TAB 1 - QUẢN LÝ NGUYÊN LIỆU (Thêm/Sửa thông tin)
        // =================================================================================

        private void SetTrangThaiTab1(bool xemChiTiet)
        {
            // Logic: Đang xem thì khóa nhập, đang sửa thì mở nhập
            txtMaNL.Enabled = !xemChiTiet;
            txtTenNL.Enabled = !xemChiTiet;
            cboDVT.Enabled = !xemChiTiet;
            numDonGia.Enabled = !xemChiTiet;
            numDinhMuc.Enabled = !xemChiTiet;

            // Riêng Tồn kho luôn luôn khóa (chỉ tăng khi Nhập kho ở Tab 2)
            txtTonKho.Enabled = false;

            btnLuuPhieu.Enabled = !xemChiTiet;
        }

        private void ClearInputTab1()
        {
            txtMaNL.Clear();
            txtTenNL.Clear();
            cboDVT.SelectedIndex = -1;
            numDonGia.Value = 0;
            numDinhMuc.Value = 0;
            txtTonKho.Text = "0";
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            isThemNL = true;
            SetTrangThaiTab1(false);
            ClearInputTab1();
            txtMaNL.Focus();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaNL.Text))
            {
                MessageBox.Show("Vui lòng chọn nguyên liệu cần sửa!", "Thông báo");
                return;
            }

            isThemNL = false;
            SetTrangThaiTab1(false);
            txtMaNL.Enabled = false; // Khóa mã, không cho sửa mã
            txtTenNL.Focus();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Validate dữ liệu
            if (string.IsNullOrWhiteSpace(txtMaNL.Text) || string.IsNullOrWhiteSpace(txtTenNL.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã và Tên nguyên liệu!", "Cảnh báo");
                return;
            }

            // --- GỬI SINH VIÊN ĐOẠN NÀY ĐỂ VIẾT SQL ---
            if (isThemNL)
            {
                // SQL: INSERT INTO NguyenLieu (MaNL, TenNL, ...) VALUES (...)
                MessageBox.Show("Đã thêm nguyên liệu mới thành công! (Demo)");
            }
            else
            {
                // SQL: UPDATE NguyenLieu SET TenNL = ..., DonGia = ... WHERE MaNL = ...
                MessageBox.Show("Đã cập nhật thông tin thành công! (Demo)");
            }

            SetTrangThaiTab1(true);
            LoadDataNguyenLieu(); // Load lại lưới
            LoadComboNguyenLieu(); // Update lại ComboBox bên Tab 2 luôn
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            SetTrangThaiTab1(true);
        }

        private void dgvNguyenLieu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvNguyenLieu.Rows[e.RowIndex];
                txtMaNL.Text = row.Cells["MaNL"].Value.ToString();
                txtTenNL.Text = row.Cells["TenNL"].Value.ToString();
                cboDVT.Text = row.Cells["DonViTinh"].Value.ToString();
                numDonGia.Value = Convert.ToDecimal(row.Cells["DonGiaNhap"].Value);
                numDinhMuc.Value = Convert.ToDecimal(row.Cells["DinhMucToiThieu"].Value);
                txtTonKho.Text = row.Cells["SoLuongTon"].Value.ToString();
            }
        }

        // =================================================================================
        // PHẦN 2: LOGIC TAB 2 - NHẬP KHO (Tạo phiếu nhập)
        // =================================================================================

        private void KhoiTaoGioHangNhap()
        {
            // Tạo bảng tạm để chứa các món user chọn nhập
            dtGioHangNhap = new DataTable();
            dtGioHangNhap.Columns.Add("MaNL");
            dtGioHangNhap.Columns.Add("TenNL");
            dtGioHangNhap.Columns.Add("SoLuong", typeof(decimal));
            dtGioHangNhap.Columns.Add("DonGia", typeof(decimal));
            dtGioHangNhap.Columns.Add("ThanhTien", typeof(decimal));

            dgvChiTietNhap.DataSource = dtGioHangNhap;

            // Format cột tiền tệ cho đẹp
            dgvChiTietNhap.Columns["DonGia"].DefaultCellStyle.Format = "N0";
            dgvChiTietNhap.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
        }

        private void btnThemDong_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra đầu vào
            if (cboChonNL.SelectedIndex == -1)
            {
                MessageBox.Show("Chưa chọn nguyên liệu!", "Lỗi");
                return;
            }
            if (numSoLuongNhap.Value <= 0)
            {
                MessageBox.Show("Số lượng nhập phải lớn hơn 0!", "Lỗi");
                return;
            }

            // 2. Lấy thông tin từ giao diện
            string maNL = cboChonNL.SelectedValue.ToString();
            string tenNL = cboChonNL.Text;
            decimal soLuong = numSoLuongNhap.Value;
            decimal donGia = numGiaNhap.Value;
            decimal thanhTien = soLuong * donGia;

            // 3. Kiểm tra xem mã này đã có trong lưới chưa
            // Nếu có rồi thì cộng dồn số lượng, chưa có thì thêm dòng mới
            DataRow existingRow = dtGioHangNhap.AsEnumerable()
                                    .FirstOrDefault(r => r.Field<string>("MaNL") == maNL);

            if (existingRow != null)
            {
                // Đã tồn tại -> Cộng dồn
                decimal slCu = existingRow.Field<decimal>("SoLuong");
                decimal tienCu = existingRow.Field<decimal>("ThanhTien");
                existingRow["SoLuong"] = slCu + soLuong;
                existingRow["ThanhTien"] = tienCu + thanhTien;
            }
            else
            {
                // Chưa tồn tại -> Thêm dòng mới
                dtGioHangNhap.Rows.Add(maNL, tenNL, soLuong, donGia, thanhTien);
            }

            // 4. Tính lại tổng tiền cả phiếu
            CapNhatTongTien();
        }

        private void CapNhatTongTien()
        {
            decimal tongTien = 0;
            if (dtGioHangNhap.Rows.Count > 0)
            {
                // Tính tổng cột Thành Tiền
                tongTien = Convert.ToDecimal(dtGioHangNhap.Compute("SUM(ThanhTien)", string.Empty));
            }
            lblTongTien.Text = string.Format("{0:N0} VND", tongTien);
        }

        private void btnLuuPhieu_Click(object sender, EventArgs e)
        {
            if (dtGioHangNhap.Rows.Count == 0)
            {
                MessageBox.Show("Phiếu nhập đang trống!", "Cảnh báo");
                return;
            }

            if (MessageBox.Show("Xác nhận nhập kho? Kho sẽ được cập nhật ngay lập tức.", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                // ================================================================
                // QUAN TRỌNG: GỬI ĐOẠN NÀY CHO SINH VIÊN CODE SQL
                // ================================================================
                // Quy trình Transaction:
                // B1: Insert vào bảng [PhieuNhapKho] -> Lấy ID vừa tạo (VD: MaPN = 101)
                // B2: Duyệt vòng lặp dtGioHangNhap:
                //     - Insert vào bảng [ChiTietPhieuNhap] với MaPN = 101
                //     - Update bảng [NguyenLieu]: SoLuongTon = SoLuongTon + SoLuongNhap
                // ================================================================

                MessageBox.Show("Đã nhập kho thành công! Tồn kho đã được cập nhật.", "Thành công");

                // Reset giao diện để nhập phiếu mới
                dtGioHangNhap.Clear();
                CapNhatTongTien();
                numSoLuongNhap.Value = 1;
                cboChonNL.SelectedIndex = -1;

                // Load lại Tab 1 để thấy tồn kho thay đổi
                LoadDataNguyenLieu();
            }
        }

        private void btnXoaDong_Click(object sender, EventArgs e)
        {
            // Xóa dòng đang chọn trong lưới nhập hàng (nếu lỡ nhập sai)
            if (dgvChiTietNhap.CurrentRow != null)
            {
                dgvChiTietNhap.Rows.RemoveAt(dgvChiTietNhap.CurrentRow.Index);
                CapNhatTongTien();
            }
        }

        // =================================================================================
        // PHẦN 3: DỮ LIỆU GIẢ LẬP (MOCK DATA) - XÓA KHI CÓ SQL THẬT
        // =================================================================================

        private void LoadDataNguyenLieu()
        {
            // Tạo bảng ảo giống hệt cấu trúc SQL bác đã tạo
            DataTable dt = new DataTable();
            dt.Columns.Add("MaNL");
            dt.Columns.Add("TenNL");
            dt.Columns.Add("DonViTinh");
            dt.Columns.Add("SoLuongTon", typeof(decimal));
            dt.Columns.Add("DonGiaNhap", typeof(decimal));
            dt.Columns.Add("DinhMucToiThieu", typeof(decimal));

            // Thêm vài dòng dữ liệu mẫu
            dt.Rows.Add("CF01", "Cafe hạt Robusta", "Kg", 10.5, 150000, 2);
            dt.Rows.Add("SUA01", "Sữa đặc Ngôi Sao", "Lon", 50, 22000, 10);
            dt.Rows.Add("DUONG", "Đường cát", "Kg", 20, 18000, 5);

            dgvNguyenLieu.DataSource = dt;
        }

        private void LoadComboNguyenLieu()
        {
            // Lấy dữ liệu từ Grid Tab 1 đổ vào Combo Tab 2
            // Trong thực tế sẽ Query: SELECT MaNL, TenNL FROM NguyenLieu

            DataTable dtCombo = new DataTable();
            dtCombo.Columns.Add("MaNL");
            dtCombo.Columns.Add("TenNL");

            dtCombo.Rows.Add("CF01", "Cafe hạt Robusta");
            dtCombo.Rows.Add("SUA01", "Sữa đặc Ngôi Sao");
            dtCombo.Rows.Add("DUONG", "Đường cát");

            cboChonNL.DataSource = dtCombo;
            cboChonNL.DisplayMember = "TenNL"; // Hiển thị tên
            cboChonNL.ValueMember = "MaNL";    // Giá trị ngầm là Mã
            cboChonNL.SelectedIndex = -1;      // Chưa chọn gì
        }

        // Sự kiện khi chọn nguyên liệu ở Tab 2 -> Tự động điền giá nhập gợi ý (Pro UX)
        private void cboChonNL_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboChonNL.SelectedValue != null)
            {
                // Ở đây demo nên mình gán cứng, thực tế phải query lấy giá nhập gần nhất
                string ma = cboChonNL.SelectedValue.ToString();
                if (ma == "CF01") numGiaNhap.Value = 150000;
                else if (ma == "SUA01") numGiaNhap.Value = 22000;
                else numGiaNhap.Value = 18000;
            }
        }
    }
}
