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

namespace PosKafe
{
    public partial class Form_BaoCao : MetroForm
    {
        public Form_BaoCao()
        {
            InitializeComponent();
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.None;
            this.ControlBox = false;
            this.Text = "";
            
            LoadDateTimePicker();
        }

        //1. Thiết lập ngày mặc định (Từ đầu tháng đến hiện tại)
        void LoadDateTimePicker()
        {
            DateTime today = DateTime.Now;
            //Ngày đầu tháng: Ngày 1, tháng hiện tại, năm hiện tại
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            // Ngày cuối: Hôm nay
            dtpkToDate.Value = today;
        }

        private void Form_BaoCao_Load(object sender, EventArgs e)
        {

        }
        // 2. Sự kiện nút thống kê
        private void btnThongKe_Click(object sender, EventArgs e)
        {
            DateTime d1 = dtpkFromDate.Value;
            DateTime d2 = dtpkToDate.Value;

            // Load Tab 1: Doanh thu
            LoadListBillByDate(d1, d2);

            // Load Tab 2: Top bán chạy
            LoadTopBanChay(d1, d2);
        }

        void LoadListBillByDate(DateTime fromDate, DateTime toDate)
        {
            try
            {
                // Gọi Proceduce trong SQL
                string query = "EXEC USP_GetListBillByDate @fromDate , @toDate";

                // Thực thi và lấy về bảng dữ liệu (DataTable)
                DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { fromDate, toDate });

                // A. Dổ dữ liệu vào DataGridViewa (Bảng chi tiết hóa đơn)
                dtgvBill.DataSource = data;

                dtgvBill.Parent.BackColor = Color.White; // Hoặc màu trùng với Form
                dtgvBill.BackgroundColor = Color.White;
                dtgvBill.BorderStyle = System.Windows.Forms.BorderStyle.None;

                // 2. CẤU HÌNH CO GIÃN CỘT (QUAN TRỌNG)
                // Mã HĐ: Nhỏ thôi
                dtgvBill.Columns["Mã Hóa Đơn"].Width = 80;

                // Ngày bán & Người bán: Tự giãn theo nội dung chữ (Để không bị che chữ)
                dtgvBill.Columns["Ngày Bán"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dtgvBill.Columns["Người Bán"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                // Tổng tiền: Quan trọng nhất -> Cho nó giãn hết phần còn thừa (Fill)
                dtgvBill.Columns["Tổng Tiền"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                // 3. Căn chỉnh lề
                dtgvBill.Columns["Mã Hóa Đơn"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtgvBill.Columns["Ngày Bán"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dtgvBill.Columns["Người Bán"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtgvBill.Columns["Tổng Tiền"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                // 4. Định dạng số tiền và ngày tháng
                dtgvBill.Columns["Tổng Tiền"].DefaultCellStyle.Format = "N0";
                dtgvBill.Columns["Tổng Tiền"].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold); // In đậm tiền
                dtgvBill.Columns["Ngày Bán"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";

                // 5. Chỉnh Header (Tiêu đề cột) cho cao và thoáng hơn
                dtgvBill.ColumnHeadersHeight = 40;
                dtgvBill.RowTemplate.Height = 35;

                // B. Tính tổng doanh thu
                Drawchart(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi tải báo cáo: " + ex.Message);
            }
        }
        void LoadTopBanChay(DateTime fromDate, DateTime toDate)
        {
            try
            {
                // --- BƯỚC 1: KIỂM TRA TỪNG ÔNG MỘT ---
                if (dtgvBestSeller == null)
                {
                    MessageBox.Show("LỖI TÌM THẤY: Cái Bảng (DataGridView) đang bị Null.\nBạn hãy kiểm tra lại tên (Name) của nó xem có phải là 'dtgvBestSeller' không?");
                    return; // Dừng lại ngay
                }

                if (lblMonHotNhat == null)
                {
                    MessageBox.Show("LỖI TÌM THẤY: Cái Label (Chữ đỏ) đang bị Null.\nBạn hãy kiểm tra lại tên (Name) của nó xem có phải là 'lblMonHotNhat' không?");
                    return;
                }

                if (ChartBestSeller == null)
                {
                    MessageBox.Show("LỖI TÌM THẤY: Cái Biểu đồ (Chart) đang bị Null.\nBạn hãy kiểm tra lại tên (Name) của nó xem có phải là 'ChartBestSeller' không?");
                    return;
                }

                // --- BƯỚC 2: NẾU TẤT CẢ ĐỀU ỔN THÌ MỚI CHẠY CODE CHÍNH ---

                string query = "EXEC USP_GetBestSellers @fromDate , @toDate";
                DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { fromDate, toDate });
                dtgvBestSeller.DataSource = data;

                // Đổ bảng
                dtgvBestSeller.DataSource = data;
                dtgvBestSeller.Columns["TenSanPham"].HeaderText = "Tên Món";
                dtgvBestSeller.Columns["SoLuongBan"].HeaderText = "Số Lượng";
                dtgvBestSeller.BackgroundColor = Color.White;
                dtgvBestSeller.BorderStyle = System.Windows.Forms.BorderStyle.None;
                dtgvBestSeller.BackgroundColor = Color.White; // Nền trắng
                dtgvBestSeller.BorderStyle = System.Windows.Forms.BorderStyle.None; // Bỏ viền
                dtgvBestSeller.RowHeadersVisible = false; // Bỏ cột đầu dòng thừa thãi
                                                          // Đẩy chữ ra ngoài biểu đồ để đỡ rối


                // Đổ Label
                if (data.Rows.Count > 0)
                {
                    string topName = data.Rows[0]["TenSanPham"].ToString();
                    string topCount = data.Rows[0]["SoLuongBan"].ToString();
                    lblMonHotNhat.Text = $"🏆 QUÁN QUÂN: {topName.ToUpper()} ({topCount} ly)";
                }

                // Đổ Chart
                ChartBestSeller.Series.Clear();
                System.Windows.Forms.DataVisualization.Charting.Series series = new System.Windows.Forms.DataVisualization.Charting.Series("BestSellers");
                series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
                series["PieLabelStyle"] = "Outside";
                series.BorderWidth = 1;
                series.BorderColor = Color.White; // Tạo viền trắng giữa các miếng bánh cho đẹp

                // Cấu hình hiển thị trực tiếp
                series.IsValueShownAsLabel = true;
                series.Label = "#VALX (#VALY)";

                foreach (DataRow row in data.Rows)
                {
                    string tenMon = row["TenSanPham"].ToString();
                    int soLuong = Convert.ToInt32(row["SoLuongBan"]);
                    series.Points.AddXY(tenMon, soLuong);
                }
                ChartBestSeller.Series.Add(series);

                // Legend an toàn
                ChartBestSeller.Legends.Clear();
                ChartBestSeller.Legends.Add(new System.Windows.Forms.DataVisualization.Charting.Legend("Legend1"));
                ChartBestSeller.Legends[0].Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khác: " + ex.Message);
            }
        }

        void Drawchart(DataTable data)
        {
            // Xóa dữ liệu biểu đồ cũ để vẽ cột mới
            chartRevenue.Series.Clear();

            // Tạo một Series mới lên là "Doanh thu"
            // Lưu ý: Dùng đường dẫn đầy đủ để tránh lỗi thiếu thư viện
            System.Windows.Forms.DataVisualization.Charting.Series series = new System.Windows.Forms.DataVisualization.Charting.Series("Doanh thu");

            // Chọn kiểu biểu đồ là cột
            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;

            series.IsValueShownAsLabel = true;// hiện số tiền trên đầu cột
            series.LabelFormat = "{0:#,##0} VNĐ";// Format tiền Việt Nam

            // Duyệt qua từng dòng trong bảng dữ liệu để vẽ cột
            foreach (DataRow row in data.Rows)
            {
                // Lấy dữ liệu từ cột (Tên cột phải khớp với tên SQL Procedure)
                // Trong SQL mình đã đặt alias là [Ngày bán] và [Tổng tiền]
                DateTime date = (DateTime)row["Ngày Bán"];
                decimal money = (decimal)row["Tổng Tiền"];


                // Thêm điểm vào biểu đồ
                // Trục x: Ngày tháng (Format: dd/MM cho gọn)
                // Trục y: Tổng tiền
                series.Points.AddXY(date.ToString("dd/MM"), money);
            }
            // Thêm Series vào biểu đồ
            chartRevenue.Series.Add(series);


            // Cấu hình hiển thị cho đẹp
            chartRevenue.ChartAreas[0].AxisX.Title = "Thời gian"; // Tiêu đề trục X 
            chartRevenue.ChartAreas[0].AxisY.Title = "Doanh thu (VNĐ)"; // Tiêu đề trục Y
        }

        private void ToExcel(DataGridView dGV, string fileName)
        {
            string stOutput = "";
            // Export titles:
            string sHeaders = "";

            for (int j = 0; j < dGV.Columns.Count; j++)
                sHeaders = sHeaders.ToString() + Convert.ToString(dGV.Columns[j].HeaderText) + "\t";
            stOutput += sHeaders + "\r\n";


            // Export data.
            for (int i = 0; i < dGV.RowCount - 1; i++)
            {
                string stLine = "";
                for (int j = 0; j < dGV.Rows[i].Cells.Count; j++)
                    stLine = stLine.ToString() + Convert.ToString(dGV.Rows[i].Cells[j].Value) + "\t";
                stOutput += stLine + "\r\n";
            }

            // Encoding utf-16 để hỗ trợ Tiếng Việt không bị lỗi font
            byte[] output = System.Text.Encoding.Unicode.GetBytes(stOutput);
            System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Create);
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(fs);
            bw.Write(output, 0, output.Length);
            bw.Flush();
            bw.Close();
            fs.Close();
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            if (dtgvBill.Rows.Count > 0)
            {
                SaveFileDialog std = new SaveFileDialog();
                std.Filter = "Excel Documents (*.xls)|*.xls";
                std.FileName = "BaoCaoDoanhThu_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xls";

                if (std.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        ToExcel(dtgvBill, std.FileName);
                        MessageBox.Show("Xuất file thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        System.Diagnostics.Process.Start(std.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Xuất file thất bại! \n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                 MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
