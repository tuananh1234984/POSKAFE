using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace PosKafe.DAO
{
    public class TaiKhoanDao
    {
        private static TaiKhoanDao instance;
        public static TaiKhoanDao Instance
        {
            get { if (instance == null) instance = new TaiKhoanDao(); return instance; }
            private set { TaiKhoanDao.instance = value; }
        }

        private TaiKhoanDao() { }

        // -- Hàm --
        // Hàm kiểm tra đăng nhập
        // Trả về true nếu đang nhập thành công hoặc false nếu đăng nhập thất bại
        public bool KiemTraDangNhap(string tenDangNhap, string matKhau)
        {
            // 1. Hàm kiểm tra đăng nhập 
            // Áp dụng kĩ thuật Chống SQL Injection 
            string query = "SELECT * FROM TaiKhoan WHERE TenDangNhap = @username AND MatKhau = @password";

            // 2. Thực thi query
            // Gọi DataProvider.Instance.ExecuteQuery
            // truyền 2 tham số vào:
            // - Câu query 
            // - Mảng object chứa giá trị của các tham số (theo đúng thứ tự)
            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] { tenDangNhap, matKhau });
            // 3. Kiểm tra kết quả trả về
            // Nếu có ít nhất 1 hàng (row) trong DataTable thì đăng nhập thành công
            // Ngược lại đăng nhập thất bại
            return result.Rows.Count > 0;
        }
    }
}
