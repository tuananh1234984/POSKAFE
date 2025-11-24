using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Net.Http; // Dùng để gọi Web
using Newtonsoft.Json; // Dùng để dịch JSON 
using System.Threading.Tasks;// Dùng cho xử lý bất đồng bộ (Async)
using System.Windows.Forms;
using MetroFramework.Forms;
using MetroFramework;
using PosKafe.DTO;
using PosKafe.DAO;
using System.Drawing.Printing;

namespace PosKafe
{
    public partial class Form_CaiDat : MetroForm
    {
        private AccountDTO loginAccount; // Lưu người đang đăng nhập

        public Form_CaiDat(AccountDTO acc)
        {
            InitializeComponent();
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.None;
            this.ControlBox = false;
            this.Text = "";
            this.loginAccount = acc;

            // Gán StyleManager cho Form để đổi màu đồng bộ (Bọc try-catch để an toàn)
            try
            {
                if (this.metroStyleManager1 != null)
                {
                    this.StyleManager = this.metroStyleManager1;
                }
            }
            catch { }

            // Load dữ liệu (LoadLogs sẽ được gọi an toàn hơn)
            LoadStoreInfo();
            LoadSettings();
            LoadLogs();
        }

        // =================================================================================
        // TAB 1: THÔNG TIN QUÁN (LOGO, BANK, ĐỊA CHỈ)
        // =================================================================================
        void LoadStoreInfo()
        {
            try
            {
                DataRow row = StoreDAO.Instance.GetStoreInfo();
                if (row != null)
                {
                    // Thông tin chung
                    if (txtTenQuan != null) txtTenQuan.Text = row["TenQuan"].ToString();
                    if (txtDiaChi != null) txtDiaChi.Text = row["DiaChi"].ToString();
                    if (txtSDT != null) txtSDT.Text = row["SDT"].ToString();
                    if (txtLoiChao != null) txtLoiChao.Text = row["LoiChao"].ToString();
                    if (txtWifi != null) txtWifi.Text = row["WifiInfo"] != DBNull.Value ? row["WifiInfo"].ToString() : ""; // Sửa tên cột cho khớp DB (WifiInfo hay Wifi)
                    if (txtMaSoThue != null) txtMaSoThue.Text = row["MaSoThue"].ToString();

                    // Thông tin ngân hàng
                    if (txtTenNganHang != null) txtTenNganHang.Text = row["NganHang"].ToString(); // Sửa tên cột NganHang cho khớp DB
                    if (txtSoTaiKhoan != null) txtSoTaiKhoan.Text = row["SoTaiKhoan"].ToString();
                    if (txtChuTaiKhoan != null) txtChuTaiKhoan.Text = row["ChuTaiKhoan"].ToString();

                    // Load Ảnh Logo
                    string logoName = row["LogoPath"].ToString();
                    string fullPath = Application.StartupPath + "\\Images\\" + logoName;
                    string saveBankCode = row["NganHang"].ToString();
                    txtSoTaiKhoan.Text = row["SoTaiKhoan"].ToString();

                    if (!string.IsNullOrEmpty(logoName) && File.Exists(fullPath) && ptbLogo != null)
                    {
                        ptbLogo.Image = Image.FromFile(fullPath);
                        ptbLogo.Tag = logoName; // Lưu tên file vào Tag để sử dụng
                    }

                    // Kiểm tra cbNganHang có tồn tại và có items không trước khi gán
                    if (cbNganHang != null && cbNganHang.Items.Count > 0)
                    {
                        cbNganHang.SelectedValue = saveBankCode;
                    }
                    HienThiQRMau();
                }
            }
            catch (Exception ex)
            {
                // Console.WriteLine("Lỗi LoadStoreInfo: " + ex.Message); // Debug
            }
        }

        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ptbLogo.Image = Image.FromFile(ofd.FileName);
                ptbLogo.Tag = "New!" + ofd.FileName;// Đánh dấu là ảnh mới chọn
            }
        }

        private void btnLuuThongTin_Click(object sender, EventArgs e)
        {
            if (loginAccount.Type != 1)
            {
                MessageBox.Show(this, "Bạn không có quyền sửa thông tin hệ thống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //1. Xử lý copy ảnh logo vào thư mục phần mềm (nếu chọn ảnh mới)
            string logoToSave = "";
            if (ptbLogo.Tag != null && ptbLogo.Tag.ToString().StartsWith("New!"))
            {
                string soucreFile = ptbLogo.Tag.ToString().Split('!')[1]; // Sửa split char thành '!' cho khớp với tag gán ở trên
                string fileName = "Logo_" + DateTime.Now.Ticks + Path.GetExtension(soucreFile); // Đặt tên file unique
                string destFolder = Application.StartupPath + "\\Images\\";

                if (!Directory.Exists(destFolder)) Directory.CreateDirectory(destFolder);

                File.Copy(soucreFile, destFolder + fileName, true);
                logoToSave = fileName;
            }
            else
            {
                // Nếu không đổi ảnh thì giữ nguyên ảnh cũ (hoặc lấy từ tag nếu tag không bắt đầu bằng New!)
                if (ptbLogo.Tag != null)
                    logoToSave = ptbLogo.Tag.ToString().Replace("New!", "");
                else
                    logoToSave = "";
            }

            //2. Gọi DAO để lưu xuống database
            string bankCode = "";
            if (cbNganHang.SelectedValue != null) bankCode = cbNganHang.SelectedValue.ToString();

            if (StoreDAO.Instance.UpdateStoreInfo(txtTenQuan.Text,
                txtDiaChi.Text,
                txtSDT.Text,
                txtLoiChao.Text,
                txtWifi.Text,
                txtMaSoThue.Text,
                bankCode, // Lưu mã ngân hàng từ ComboBox
                txtSoTaiKhoan.Text,
                txtChuTaiKhoan.Text,
                logoToSave))
            {
                MessageBox.Show(this, "Lưu thông tin quán thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LogDAO.Instance.WriteLog(loginAccount.Username, "Cập nhật thông tin quán");
            }
            else
            {
                MessageBox.Show(this, "Lưu thông tin quán thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =================================================================================
        // TAB 2: CẤU HÌNH MÁY IN & GIAO DIỆN (SETTINGS)
        // =================================================================================
        void LoadSettings()
        {
            cbMayin.Items.Clear(); // Sửa tên biến cbMayin -> cbMayIn cho chuẩn CamelCase (nếu design đặt là cbMayIn)
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                cbMayin.Items.Add(printer);
            }

            // Load thông số in
            cbMayin.SelectedItem = Properties.Settings.Default.PrinterName;
            cbKhoGiay.SelectedItem = Properties.Settings.Default.PaperSize;
            chkTuDongIn.Checked = Properties.Settings.Default.AutoPrint;
            chkHienPreview.Checked = Properties.Settings.Default.ShowPreview;
            chkMoNganKeo.Checked = Properties.Settings.Default.OpenDrawer;

            // Load Giao diện (theme)
            cbTheme.SelectedItem = Properties.Settings.Default.AppTheme;
            cbStyle.SelectedItem = Properties.Settings.Default.AppStyle;
        }

        private void btnLuuCauHinh_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.PrinterName = cbMayin.Text;
            Properties.Settings.Default.PrintCopies = (int)nudSoLien.Value;
            Properties.Settings.Default.PaperSize = cbKhoGiay.Text;
            Properties.Settings.Default.AutoPrint = chkTuDongIn.Checked;
            Properties.Settings.Default.ShowPreview = chkHienPreview.Checked;
            Properties.Settings.Default.OpenDrawer = chkMoNganKeo.Checked;

            Properties.Settings.Default.AppTheme = cbTheme.Text;
            Properties.Settings.Default.AppStyle = cbStyle.Text;

            Properties.Settings.Default.Save();

            //Áp dụng màu sắc ngay lập tức cho Form hiện tại
            try
            {
                if (metroStyleManager1 != null)
                {
                    metroStyleManager1.Theme = (MetroThemeStyle)Enum.Parse(typeof(MetroThemeStyle), cbTheme.Text);
                    metroStyleManager1.Style = (MetroColorStyle)Enum.Parse(typeof(MetroColorStyle), cbStyle.Text);
                    this.Refresh();
                }
            }
            catch
            {
                // Ignore error
            }
            MessageBox.Show(this, "Đã lưu cấu hình thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // =================================================================================
        // TAB 3: SAO LƯU DỮ LIỆU  (BACKUP)
        // =================================================================================

        private void btnChonThuMuc_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtDuongDanBackup.Text = fbd.SelectedPath;
            }
        }

        private void btnSaoLuu_Click(object sender, EventArgs e)
        {
            string folderPath = txtDuongDanBackup.Text;
            if (string.IsNullOrEmpty(folderPath))
            {
                MessageBox.Show(this, "Vui lòng chọn thư mục để lưu file sao lưu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string dbName = "quanlyquanCafe"; // Tên Database
            string fileName = "PosKafe_backup_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".bak";
            string fullPath = folderPath + "\\" + fileName;

            try
            {
                // Câu lệnh SQL Backup
                string query = "BACKUP DATABASE [" + dbName + "] TO DISK = '" + fullPath + "'";
                DataProvider.Instance.ExecuteQuery(query);

                MessageBox.Show("Sao lưu dữ liệu thành công!\n\nFile: " + fileName, "Tuyệt vời", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //Ghi log
                LogDAO.Instance.WriteLog(loginAccount.Username, "Thực hiện sao lưu dữ liệu");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sao lưu thất bại. Vui lòng chọn ổ đĩa khác (Ví dụ ổ D, E).\nLỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =================================================================================
        // TAB 4: NHẬT KÝ HOẠT ĐỘNG (LOGS) - ĐÃ SỬA ĐỂ TRÁNH CRASH
        // =================================================================================
        void LoadLogs()
        {
            try
            {
                // 1. Kiểm tra Control dtgvLog có tồn tại không
                if (dtgvLog == null) return;

                // 2. Lấy dữ liệu
                DataTable data = LogDAO.Instance.GetLogs();

                // 3. Kiểm tra dữ liệu
                if (data == null) return;

                dtgvLog.DataSource = data;

                // 4. Cấu hình giao diện (Bọc try-catch để lỡ sai tên cột không bị crash)
                try
                {
                    dtgvLog.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    if (dtgvLog.Columns.Contains("ID"))
                        dtgvLog.Columns["ID"].Width = 50;

                    if (dtgvLog.Columns.Contains("ThoiGian"))
                    {
                        dtgvLog.Columns["ThoiGian"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
                        dtgvLog.Columns["ThoiGian"].Width = 150;
                    }
                }
                catch { }
            }
            catch (Exception ex)
            {
                // Console.WriteLine(ex.Message);
            }
        }

        // =================================================================================
        // API VIETQR
        // =================================================================================
        public class VietQRBank
        {
            public int id { get; set; }
            public string name { get; set; }
            public string code { get; set; }
            public string bin { get; set; }
            public string shortName { get; set; }

            //Tạo thuộc tính hiển thị cho đẹp trên combobox
            public string DisplayName
            {
                get { return shortName + " - " + name; }
            }
        }

        public class VietQRResponse
        {
            public string code { get; set; }
            public string desc { get; set; }
            public List<VietQRBank> data { get; set; }
        }

        private async Task LoadBankFromAPI()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Gọi API của VietQR
                    string url = "https://api.vietqr.io/v2/banks";
                    var response = await client.GetAsync(url);

                    // Đọc nội dung trả về
                    string jsonContent = await response.Content.ReadAsStringAsync();

                    VietQRResponse apiResponse = JsonConvert.DeserializeObject<VietQRResponse>(jsonContent);

                    if (apiResponse != null && apiResponse.code == "00")
                    {
                        //Đổ dữ liệu vào comboBox
                        if (cbNganHang != null) // Kiểm tra null
                        {
                            cbNganHang.DataSource = apiResponse.data;
                            cbNganHang.DisplayMember = "DisplayName";
                            cbNganHang.ValueMember = "code";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Fallback hoặc thông báo lỗi nhẹ
                // MessageBox.Show("Không tải được danh sách ngân hàng: " + ex.Message);
            }
        }

        private async void Form_CaiDat_Load(object sender, EventArgs e)
        {
            await LoadBankFromAPI();
            // Gọi lại LoadStoreInfo để chọn đúng ngân hàng sau khi API load xong
            LoadStoreInfo();
        }

        private void btnRefreshLog_Click_1(object sender, EventArgs e)
        {
            LoadLogs();
        }
        private void HienThiQRMau()
        {
            // 1. Lấy thông tin từ giao diện
            if (cbNganHang.SelectedValue == null || string.IsNullOrEmpty(txtSoTaiKhoan.Text))
            {
                return; // Chưa nhập đủ thì thôi
            }

            string bankID = cbNganHang.SelectedValue.ToString(); // Lấy mã (VD: MB, VCB)
            string accountNo = txtSoTaiKhoan.Text.Trim().Replace(" ", ""); // Lấy số tài khoản
            string template = "compact2"; // Giao diện mẫu (compact2 là gọn đẹp nhất)

            // 2. Tạo link API VietQR
            // Cấu trúc: https://img.vietqr.io/image/<BANK_ID>-<ACCOUNT_NO>-<TEMPLATE>.jpg
            string url = $"https://img.vietqr.io/image/{bankID}-{accountNo}-{template}.jpg";

            // 3. Load ảnh từ Internet vào PictureBox
            try
            {
                ptbQR.Load(url); // Hàm Load này tự động tải ảnh từ URL
                ptbQR.SizeMode = PictureBoxSizeMode.Zoom; // Co giãn ảnh cho vừa khung
            }
            catch
            {
                // Nếu lỗi (mất mạng, sai số TK...) thì kệ nó hoặc hiện ảnh lỗi
            }
        }

        private void cbNganHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            HienThiQRMau();
        }

        private void txtSoTaiKhoan_Leave(object sender, EventArgs e)
        {
            HienThiQRMau();
        }
    }
}