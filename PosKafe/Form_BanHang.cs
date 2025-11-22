using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using MetroFramework.Controls;
using MetroFramework;
using PosKafe.DAO;
using PosKafe.DTO;
using System.Drawing.Text;
using ListView = System.Windows.Forms.ListView;
using ListViewItem = System.Windows.Forms.ListViewItem;
using PosKafe.BUS;
using Button = System.Windows.Forms.Button;
using System.Net;
using System.Net.Mail;
using System.ComponentModel;

namespace PosKafe
{
    public partial class Form_BanHang : MetroForm
    {
        // === 1. KHAI BÁO BIẾN TOÀN CỤC (ĐỂ CÁC HÀM KHÁC ĐỀU NHÌN THẤY) ===
        private List<BillInfoDTO> currentBill = new List<BillInfoDTO>();

        // CÁC BIẾN NÀY LÀ CÁC ĐỐI TƯỢC CONTROL TRÊN FORM (ĐÃ ĐƯỢC DESIGNER TẠO RA)
        // DÙNG this.TÊN_THẬT CỦA CONTROL TRONG CODE ĐỂ TRÁNH LỖI CS0103

        public Form_BanHang()
        {
            InitializeComponent();

            // KHỞI TẠO CÁC CONTROL ĐÃ KHAI BÁO (ĐÂY LÀ CÁCH FIX LỖI NullReference)
            // CÁC HÀM NÀY PHẢI ĐƯỢC GỌI RA NGOÀI, KHÔNG ĐƯỢC NẰM TRONG HÀM KHÁC
            LoadNhomMon();

            // CODE METRO-HÓA FORM
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.None;
            this.ControlBox = false;
            this.Text = "";
            // Gắn sự kiện listview double click để xóa món
            this.lsvBill.MouseDoubleClick += LsvBill_DoubleClick;
            this.cbVAT.SelectedIndex = 0;
        }

        // --- 2. HÀM TẢI NHÓM MÓN ---
        private void LoadNhomMon()
        {
            // TẠM THỜI DÙNG TÊN flpNhomMon (BẠN PHẢI ĐẢM BẢO TÊN NÀY KHỚP VỚI DESIGNER)
            this.flpNhomMon.Controls.Clear();

            List<NhomSanPhamDTO> listNhom = MenuDAO.Instance.GetListNhomSanPham();

            foreach (NhomSanPhamDTO item in listNhom)
            {
                MetroTile btn = new MetroTile();
                btn.TileTextFontWeight = MetroTileTextWeight.Bold;
                btn.TextAlign = ContentAlignment.MiddleCenter;
                btn.Size = new Size(100, 100);
                btn.Text = item.TenNhom;
                btn.Tag = item;
                btn.Click += btnNhom_Click;

                this.flpNhomMon.Controls.Add(btn);
            }
        }

        // --- 3. HÀM TẢI MÓN ĂN ---
        private void LoadSanPham(int idNhom)
        {
            // TẠM THỜI DÙNG TÊN flpSanPham (BẠN PHẢI ĐẢM BẢO TÊN NÀY KHỚP VỚI DESIGNER)
            this.flpSanPham.Controls.Clear();

            List<SanPhamDTO> listFood = MenuDAO.Instance.GetListSanPhamByNhom(idNhom);

            foreach (SanPhamDTO item in listFood)
            {
                MetroTile btn = new MetroTile();
                btn.Size = new Size(130, 130);
                // chỉnh màu 
                btn.TextAlign = ContentAlignment.TopCenter;
                btn.Style = MetroFramework.MetroColorStyle.Orange;
                btn.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
                btn.Text = item.TenSanPham + "\n" + item.Gia.ToString("N0") + "đ";
                btn.Tag = item;
                btn.Click += btnMonAn_Click;

                this.flpSanPham.Controls.Add(btn);
            }
        }

        // --- 4. CÁC HÀM SỰ KIỆN VÀ LOGIC NGHIỆP VỤ ---

        private void btnNhom_Click(object sender, EventArgs e)
        {
            MetroTile btn = sender as MetroTile;
            NhomSanPhamDTO nhom = btn.Tag as NhomSanPhamDTO;
            int categoryID = nhom.ID;

            // LOGIC NGHIỆP VỤ: TẢI MÓN ĂN CỦA NHÓM NÀY
            LoadSanPham(categoryID);
        }

        private void btnMonAn_Click(object sender, EventArgs e)
        {
            MetroTile btn = sender as MetroTile;
            SanPhamDTO monAn = btn.Tag as SanPhamDTO;

            // LOGIC NGHIỆP VỤ: THÊM MÓN VÀO HÓA ĐƠN
            AddMonVaoHoaDon(monAn, 1);
        }

        private void AddMonVaoHoaDon(SanPhamDTO monAn, int soLuong)
        {
            bool daCoTrongBill = false;

            // LOGIC KIỂM TRA VÀ TĂNG SỐ LƯỢNG MÓN TRÙNG (GIỮ NGUYÊN)
            foreach (var item in currentBill)
            {
                if (item.ID_SanPham == monAn.ID)
                {
                    item.SoLuong += soLuong;
                    item.ThanhTien = item.SoLuong * item.DonGia;
                    daCoTrongBill = true;
                    break;
                }
            }
            if (!daCoTrongBill)
            {
                BillInfoDTO newInfo = new BillInfoDTO()
                {
                    ID_SanPham = monAn.ID,
                    TenSanPham = monAn.TenSanPham,
                    SoLuong = soLuong,
                    DonGia = monAn.Gia,
                    ThanhTien = soLuong * monAn.Gia
                };
                currentBill.Add(newInfo);
            }

            // CẬP NHẬT LẠI GIAO DIỆN VÀ TÍNH TỔNG TIỀN
            HienThiBillLenListView();
            TinhTongTien();
        }

        private void HienThiBillLenListView()
        {
            // LƯU Ý: lsvBill phải được thay thế bằng tên thật của control ListView của bạn
            // Ví dụ: this.listView1
            this.lsvBill.Items.Clear();

            foreach (var item in currentBill)
            {
                ListViewItem lvi = new ListViewItem(item.TenSanPham);
                lvi.SubItems.Add(item.SoLuong.ToString());
                lvi.SubItems.Add(item.DonGia.ToString("N0"));
                lvi.SubItems.Add(item.ThanhTien.ToString("N0"));

                lvi.Tag = item;
                this.lsvBill.Items.Add(lvi);
            }
        }

        private void TinhTongTien()
        {
            decimal tongTienHang = 0;
            foreach (var item in currentBill)
            {
                tongTienHang += item.ThanhTien;
            }
            int thueVAT = 0;
            if (!string.IsNullOrEmpty(cbVAT.Text))
            {
                int.TryParse(cbVAT.Text, out thueVAT);
            }

            decimal tienThue = tongTienHang * thueVAT / 100;

            //3. Tổng thanh toán
            decimal tongThanhToan = tongTienHang - tienThue;

            //4. Hiển thị
            //Có thể hiển thị chi tiết: Tiền hàng + Thuế = tổng
            this.lblTongTienHang.Text = string.Format("Hàng: {0:N0} + VAT{1}%: {2:N0}\nTC: {3:N0} đ", tongTienHang, thueVAT, tienThue, tongThanhToan);

            // Lưu biến toàn cục để lúc thanh toán còn dùng
            this.Tag = tongThanhToan;
        }

        private void LsvBill_DoubleClick(object sender, MouseEventArgs e)
        {
            //1. Kiểm tra xem có mục nào được chọn không
            if (this.lsvBill.SelectedItems.Count <= 0)
            {
                MetroMessageBox.Show(this, "Vui lòng chọn món cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //2. Lấy thông tin từ dòng đang chọn
            ListViewItem selectedItem = this.lsvBill.SelectedItems[0];

            BillInfoDTO infoCanXoa = selectedItem.Tag as BillInfoDTO;

            if (infoCanXoa != null)
            {
                XoaMonKhoiBill(infoCanXoa.ID_SanPham);
            }
        }

        private void XoaMonKhoiBill(int idSanPham)
        {
            // Dùng vóng lặp For ngược để tránh lỗi khi xóa phần tử trong list
            for (int i = currentBill.Count - 1; i >= 0; i--)
            {
                if (currentBill[i].ID_SanPham == idSanPham)
                {
                    currentBill.RemoveAt(i);
                    break;
                }
            }
            HienThiBillLenListView();
            TinhTongTien();
        }


        // --- CÁC HÀM GỐC KHÁC CỦA BẠN (GIỮ NGUYÊN) ---

        private void Form_BanHang_Load(object sender, EventArgs e) { /*....*/ }
        private void btnLuu_Click(object sender, EventArgs e)
        {
            //1 .Kiểm tra nếu hóa đơn trống thì không lưu
            if (currentBill.Count == 0) return;

            //2. (Tùy chọn) Hiện dialog hỏi tên gợi nhớ để dễ tìm
            string tenGhiNho = "Đơn lúc " + DateTime.Now.ToString("HH:mm");

            //3. Tạo đối tượng lưu trữ
            SavedBill billTreo = new SavedBill();
            billTreo.ID = listsavedBills.Count;
            billTreo.TenGhiNho = tenGhiNho;
            //Quan trọng; Phải clone list ra list mới, nếu không nó sẽ bị tham chiếu
            billTreo.SavedCart = new List<BillInfoDTO>(currentBill);

            //4. Tạo nút hiển thị lên FlowLayoutPanel (flpOrderTabs)
            decimal tongTien = 0;
            foreach (var i in currentBill) tongTien += i.ThanhTien;

            System.Windows.Forms.Button btnWin = new System.Windows.Forms.Button();
            btnWin.Text = tenGhiNho + "\n" + tongTien.ToString("N0");
            btnWin.Size = new System.Drawing.Size(100, 60);
            btnWin.BackColor = Color.Orange;
            btnWin.Tag = billTreo;
            btnWin.Click += BtnTreo_Click;

            billTreo.BtnRepresent = btnWin;

            //5. Thêm vào danh sách và giao diện
            listsavedBills.Add(billTreo);
            this.flpOrderTabs.Controls.Add(btnWin);

            //6. Quan trọng: Reset màn hình chính để bán đơn mới
            currentBill.Clear();
            HienThiBillLenListView();
            TinhTongTien();

            MetroMessageBox.Show(this, "Đã lưu đơn hàng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnTreo_Click(object sender, EventArgs e)
        {
            //1. Kiểm tra xem màn hình chính có đang lở dở đơn nào không?
            if (currentBill.Count > 0)
            {
                MetroMessageBox.Show(this, "Vui lòng thanh toán hoặc lưu đơn hiện tại trước khi mở đơn khi mở đơn khác!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            System.Windows.Forms.Button btnClicked = sender as System.Windows.Forms.Button;
            SavedBill billDuocChon = btnClicked.Tag as SavedBill;

            currentBill = new List<BillInfoDTO>(billDuocChon.SavedCart);

            //3. Xóa đơn treo đó đi (vì đã lấy xuống xử lý rồi)
            listsavedBills.Remove(billDuocChon);
            this.flpOrderTabs.Controls.Remove(btnClicked);

            //4. Hiển thị hóa đơn lên màn hình
            HienThiBillLenListView();
            TinhTongTien();

        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            if (currentBill.Count > 0)
            {
                // Yêu cầu xác nhận
                DialogResult result = MetroMessageBox.Show(this, "Bạn có chắc chắn muốn hủy đơn hàng này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    currentBill.Clear();
                    HienThiBillLenListView();
                    TinhTongTien();
                    MetroMessageBox.Show(this, "Đã hủy đơn hàng!", "Thông Báo", MessageBoxButtons.OK);
                }
            }
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (currentBill.Count == 0) return;

            //1. Tính lại tổng tiền (bao gồm cả VAT nếu có)
            decimal tongTienHang = 0;
            foreach (var item in currentBill) tongTienHang += item.ThanhTien;

            //Xử lý VAT từ combobox
            int thueVAT = 0;
            int.TryParse(cbVAT.Text, out thueVAT);
            decimal tongThanhToan = tongTienHang + (tongTienHang * thueVAT / 100);

            //2. Hiện form xác nhận (POPUP)
            using (frmXacNhanThanhToan frm = new frmXacNhanThanhToan(tongThanhToan))
            {
                // Hiện form lên và chờ người dùng bấm nút
                var result = frm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    //3. Nếu người dùng bấm xác nhận -> Tiền hành thanh toán

                    // Lấy email khách vừa nhập từ form con
                    string emailKhachHang = frm.EmailKhachHang;

                    // Gọi hàm xử lý giao dịch (truyền thêm email vào nếu cần xử lý bên trong)
                    XuLyGiaoDichThanhCong(1, emailKhachHang);

                }
                else
                {
                    // Người dùng bấm Hủy hoặc tắt form -> không làm gì cả
                }
            }

        }

        private void XuLyGiaoDichThanhCong(int trangThai, string emailKhach = "")
        {
            if (currentBill.Count == 0)
            {
                MetroMessageBox.Show(this, "Hóa đơn đang trống", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Tính tổng tiền cuối cùng
            decimal tongTien = 0;
            foreach (var item in currentBill)
            {
                tongTien += item.ThanhTien;
            }

            //Lấy thông tin nhân viên đang đăng nhập
            //Sửa: Bạn cần phải tạo một lớp Session để lưu thông tin đăng nhập
            string tenNV = "admin"; // Tạm thời dùng 'admin', sau này cần sửa

            // Gọi BUS xử lý
            int idHoaDon = BillBUS.Instance.XuLyGiaoDich(tenNV, tongTien, currentBill, trangThai);

            if (idHoaDon > 0 && trangThai == 1)
            {
                if (!string.IsNullOrEmpty(emailKhach))
                {
                    int thueVAT = 0;
                    int.TryParse(cbVAT.Text, out thueVAT);
                    //Chạy bất đồng bộ (Task) để gửi mail không bị đơ phần mềm
                    Task.Run(() =>
                    {
                        try
                        {
                            string htmlBill = TaoHoaDonHTML("Quý khách", currentBill, thueVAT);
                            GuiMailHoaDon(emailKhach, htmlBill);
                        }
                        catch { /* Ignore lỗi gửi mail */ }
                    });
                    MetroMessageBox.Show(this, "Đã thanh toán và gửi hóa đơn điện tử!", "Thông Công");
                }
                else
                {
                    MetroMessageBox.Show(this, "Thanh toán thành công (Không gửi mail)", "Thành công");
                }
                DialogResult hoiIn = MessageBox.Show("Bạn có muốn in hóa đơn không?", "In Ấn", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (hoiIn == DialogResult.Yes)
                {
                    // Tạo mới đối tượng Preview mỗi lần bấm để tránh lỗi lưu cache cũ
                    PrintPreviewDialog preview = new PrintPreviewDialog();

                    // Gắn hóa đơn vào để nó biết cần vẽ cái gì
                    preview.Document = pdocHoaDon;

                    // Phóng to cửa sổ cho dễ nhìn
                    ((Form)preview).WindowState = FormWindowState.Maximized;

                    // Hiện bảng Xem trước (Màu xám)
                    preview.ShowDialog();
                }
                pdocHoaDon.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("K80", 300, 2000);

                //2. Reset màn hình
                currentBill.Clear();
                HienThiBillLenListView();
                TinhTongTien();
            }
            else
            {
                MetroMessageBox.Show(this, "Lỗi xảy ra trong quá trình lưu trữ giao dịch.", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public class SavedBill
        {
            public int ID { get; set; }
            public string TenGhiNho { get; set; }// VD: "Khách bàn 2", "Anh áo đỏ"
            public List<BillInfoDTO> SavedCart { get; set; }
            public Button BtnRepresent { get; set; }
        }

        private List<SavedBill> listsavedBills = new List<SavedBill>();

        private void cbVAT_SelectedIndexChanged(object sender, EventArgs e)
        {
            TinhTongTien();
        }

        private string TaoHoaDonHTML(string tenKhach, List<BillInfoDTO> listMon, int phanTramVAT)
        {
            StringBuilder sb = new StringBuilder();

            // Header
            sb.Append("<div style='font-family: Arial, sans-serif;'>"); // Thêm font cho đẹp
            sb.Append("<h2 style='color: #d9534f;'>HÓA ĐƠN ĐIỆN TỬ - POS KAFE</h2>");
            sb.Append($"<p>Kính gửi: <b>{tenKhach}</b></p>");
            sb.Append($"<p>Thời gian: {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}</p>");

            // Bắt đầu tạo bảng
            sb.Append("<table border='1' cellpadding='5' cellspacing='0' style='border-collapse: collapse; width: 100%; border-color: #ddd;'>");

            // Tiêu đề cột
            sb.Append("<tr style='background-color: #f2f2f2;'>");
            sb.Append("<th>Tên món</th> <th style='width:50px'>SL</th> <th>Đơn giá</th> <th>Thành tiền</th>");
            sb.Append("</tr>");

            // Biến tính toán lại tiền hàng (để đảm bảo chính xác)
            decimal tongTienHang = 0;

            foreach (var item in listMon)
            {
                sb.Append("<tr>");
                sb.Append($"<td>{item.TenSanPham}</td>");
                sb.Append($"<td style='text-align: center;'>{item.SoLuong}</td>");
                sb.Append($"<td style='text-align: right;'>{item.DonGia:N0}</td>");
                sb.Append($"<td style='text-align: right;'>{item.ThanhTien:N0}</td>");
                sb.Append("</tr>");

                tongTienHang += item.ThanhTien; // Cộng dồn tiền hàng
            }

            // --- PHẦN QUAN TRỌNG: VẼ THÊM DÒNG VAT ---

            decimal tienThue = tongTienHang * phanTramVAT / 100;
            decimal tongCong = tongTienHang + tienThue;

            // 1. Dòng Tạm tính (Cộng tiền hàng)
            sb.Append("<tr>");
            sb.Append("<td colspan='3' style='text-align: right; font-weight: bold;'>Tạm tính:</td>");
            sb.Append($"<td style='text-align: right;'>{tongTienHang:N0}</td>");
            sb.Append("</tr>");

            // 2. Dòng Thuế VAT (Nếu có thuế > 0 thì mới hiện)
            if (phanTramVAT > 0)
            {
                sb.Append("<tr>");
                sb.Append($"<td colspan='3' style='text-align: right;'>Thuế VAT ({phanTramVAT}%):</td>");
                sb.Append($"<td style='text-align: right;'>{tienThue:N0}</td>");
                sb.Append("</tr>");
            }

            // 3. Dòng TỔNG CỘNG TO ĐÙNG
            sb.Append("<tr>");
            sb.Append("<td colspan='3' style='text-align: right; font-weight: bold; color: red; font-size: 18px;'>TỔNG THANH TOÁN:</td>");
            sb.Append($"<td style='text-align: right; font-weight: bold; color: red; font-size: 18px;'>{tongCong:N0} VNĐ</td>");
            sb.Append("</tr>");

            sb.Append("</table>");
            sb.Append("<p style='margin-top: 20px;'><i>Cảm ơn quý khách đã sử dụng dịch vụ! Hẹn gặp lại.</i></p>");
            sb.Append("</div>");

            return sb.ToString();
        }
        private void GuiMailHoaDon(string emailKhach, string noiDungHTML)
        {
            try
            {
                var fromAddress = new MailAddress("tuananhnb439@gmail.com", "POS KAFE System");
                var toAddress = new MailAddress(emailKhach);
                const string fromPassword = "xlnz odue oiee cehm"; // App Password

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = "Hóa đơn thanh toán POS KAFE - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                    Body = noiDungHTML,
                    IsBodyHtml = true // Quan trọng: Bật cái này mới hiện bảng đẹp được
                })
                {
                    smtp.Send(message);
                }
                MetroMessageBox.Show(this, "Đã gửi hóa đơn điện tử thành công!", "Email", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MetroMessageBox.Show(this, "Gửi email thất bại. Vui lòng kiểm tra lại địa chỉ email.\n\nChi tiết lỗi: " + ex.Message, "Lỗi gửi email", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void XulyXoaMon()
        {
            if (this.lsvBill.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = this.lsvBill.SelectedItems[0];
                BillInfoDTO info = selectedItem.Tag as BillInfoDTO;
                XoaMonKhoiBill(info.ID_SanPham);
            }
            else
            {
                MetroMessageBox.Show(this, "Vui lòng chọn món cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnXoaMon_Click(object sender, EventArgs e)
        {
            XulyXoaMon();
        }

        private void xóaMónToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            XulyXoaMon();
        }

        private void XulyGiamMon()
        {
            //1. Kiểm tra xem có món chưa
            if (this.lsvBill.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = this.lsvBill.SelectedItems[0];
                BillInfoDTO info = selectedItem.Tag as BillInfoDTO;

                if (info.SoLuong > 1)
                {
                    //Giảm số lượng trực tiếp lên đối tượng
                    info.SoLuong--;

                    //2. Tính lại thành tiền của món ăn
                    info.ThanhTien = info.SoLuong * info.DonGia;

                    //3. Cập nhật lại giao diện
                    HienThiBillLenListView();
                    TinhTongTien();
                }
                else
                {
                    //Trường hợp B: Số lượng = 1 -> hỏi xóa
                    DialogResult result = MessageBox.Show("Số lượng món hiện tại là 1. Bạn có muốn xóa món này khỏi hóa đơn không?", "Xác nhận xóa món", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        XoaMonKhoiBill(info.ID_SanPham);
                    }
                }
            }
            else
            {
                MetroMessageBox.Show(this, "Vui lòng chọn món cần giảm số lượng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btnGiamMon_Click(object sender, EventArgs e)
        {
            XulyGiamMon();
        }

        private void giảmSốLượngToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            XulyGiamMon();
        }

        private void pdocHoaDon_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics graphics = e.Graphics;

            // Font chữ
            Font fontTitle = new Font("Courier New", 14, FontStyle.Bold); // Tiêu đề to
            Font fontBold = new Font("Courier New", 9, FontStyle.Bold);   // Chữ đậm
            Font fontRegular = new Font("Courier New", 9, FontStyle.Regular); // Chữ thường

            // Cấu hình lề
            float yPos = 10;
            int leftMargin = 10;
            int rightMargin = 280; // Chiều rộng khổ in ~280-290 là đẹp

            // Căn lề
            StringFormat centerFormat = new StringFormat() { Alignment = StringAlignment.Center };
            StringFormat rightFormat = new StringFormat() { Alignment = StringAlignment.Far };

            // --- 1. HEADER ---
            // Tên quán
            graphics.DrawString("POS KAFE", fontTitle, Brushes.Black, new RectangleF(0, yPos, 300, 25), centerFormat);
            yPos += 25;

            // Địa chỉ
            graphics.DrawString("ĐC: 123 Đường ABC, Quận X", fontRegular, Brushes.Black, new RectangleF(0, yPos, 300, 20), centerFormat);
            yPos += 15;

            // SĐT
            graphics.DrawString("SĐT: 0909.123.456", fontRegular, Brushes.Black, new RectangleF(0, yPos, 300, 20), centerFormat);
            yPos += 15;

            // Đường kẻ
            graphics.DrawString("---------------------------------------", fontRegular, Brushes.Black, leftMargin, yPos);
            yPos += 15;

            // Tiêu đề hóa đơn
            graphics.DrawString("HÓA ĐƠN THANH TOÁN", fontBold, Brushes.Black, new RectangleF(0, yPos, 300, 20), centerFormat);
            yPos += 20;

            // Ngày giờ & Nhân viên
            graphics.DrawString("Ngày: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), fontRegular, Brushes.Black, leftMargin, yPos);
            yPos += 15;
            graphics.DrawString("Nhân viên: Admin", fontRegular, Brushes.Black, leftMargin, yPos);
            yPos += 15;

            // Đường kẻ
            graphics.DrawString("---------------------------------------", fontRegular, Brushes.Black, leftMargin, yPos);
            yPos += 15;

            // --- 2. DANH SÁCH MÓN ---
            // Tiêu đề cột

            decimal tongTienGoc = 0;

            graphics.DrawString("Tên món", fontBold, Brushes.Black, leftMargin, yPos);
            graphics.DrawString("SL", fontBold, Brushes.Black, 190, yPos); // Dời SL ra 190
            graphics.DrawString("T.Tiền", fontBold, Brushes.Black, rightMargin, yPos, rightFormat);
            yPos += 20;

            // --- SỬA LẠI VÒNG LẶP IN MÓN ---
            foreach (var item in currentBill)
            {
                // 1. CỘT TÊN MÓN (Quan trọng nhất)
                // Ta quy định tên món chỉ được nằm trong độ rộng 170 đơn vị.
                // Nếu tên dài quá, nó sẽ tự động xuống dòng trong cái khung này.
                RectangleF rectTenMon = new RectangleF(leftMargin, yPos, 170, 40);
                graphics.DrawString(item.TenSanPham, fontRegular, Brushes.Black, rectTenMon);

                // 2. CỘT SỐ LƯỢNG (Canh ở vị trí 190)
                // Căn giữa cho đẹp (Hoặc căn trái tùy ý)
                StringFormat centerSL = new StringFormat() { Alignment = StringAlignment.Center };
                graphics.DrawString(item.SoLuong.ToString(), fontRegular, Brushes.Black, 200, yPos, centerSL);

                // 3. CỘT THÀNH TIỀN (Căn phải ở lề phải)
                graphics.DrawString(item.ThanhTien.ToString("N0"), fontRegular, Brushes.Black, rightMargin, yPos, rightFormat);

                // 4. TÍNH TOÁN XUỐNG DÒNG THÔNG MINH
                // Nếu tên món dài quá (> 25 ký tự) thì nó chiếm 2 dòng -> Phải cộng yPos nhiều hơn
                if (item.TenSanPham.Length > 25)
                {
                    yPos += 30; // Xuống dòng rộng hơn
                }
                else
                {
                    yPos += 20; // Xuống dòng bình thường
                }

                tongTienGoc += item.ThanhTien;
            }

                graphics.DrawString("---------------------------------------", fontRegular, Brushes.Black, leftMargin, yPos);
            yPos += 15;

            // --- 3. TỔNG KẾT TIỀN ---

            // Lấy VAT từ giao diện
            int thueVAT = 0;
            int.TryParse(cbVAT.Text, out thueVAT);
            decimal tienThue = tongTienGoc * thueVAT / 100;
            decimal tongCong = tongTienGoc + tienThue;

            // Tạm tính
            graphics.DrawString("Tạm tính:", fontRegular, Brushes.Black, 100, yPos);
            graphics.DrawString(tongTienGoc.ToString("N0"), fontBold, Brushes.Black, rightMargin, yPos, rightFormat);
            yPos += 15;

            // VAT (Nếu có)
            if (thueVAT > 0)
            {
                graphics.DrawString($"Thuế VAT ({thueVAT}%):", fontRegular, Brushes.Black, 100, yPos);
                graphics.DrawString(tienThue.ToString("N0"), fontBold, Brushes.Black, rightMargin, yPos, rightFormat);
                yPos += 15;
            }

            // Tổng cộng (To và Đậm)
            yPos += 5;
            graphics.DrawString("TỔNG CỘNG:", fontBold, Brushes.Black, leftMargin, yPos);
            graphics.DrawString(tongCong.ToString("N0") + " đ", new Font("Courier New", 12, FontStyle.Bold), Brushes.Black, rightMargin, yPos - 2, rightFormat);

            yPos += 30;

            // --- 4. FOOTER ---
            graphics.DrawString("Cảm ơn quý khách & Hẹn gặp lại!", new Font("Courier New", 8, FontStyle.Italic), Brushes.Black, new RectangleF(0, yPos, 300, 20), centerFormat);
            yPos += 15;
            graphics.DrawString("Wifi: PosKafe / Pass: 12345678", new Font("Courier New", 8, FontStyle.Regular), Brushes.Black, new RectangleF(0, yPos, 300, 20), centerFormat);
        }
    }
}