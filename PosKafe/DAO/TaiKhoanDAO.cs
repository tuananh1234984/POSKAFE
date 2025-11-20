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

        // =============================================================================
        // 2. Các hàm quản lý (thêm mới)
        // =============================================================================
        // 2.1 Lấy danh sách tài khoản (đê đổ lên DataGridView)
        public DataTable GetListAccount()
        {
            // Chỉ lấy Tên đăng nhập, Tên hiển thị và loại TK (không lấy mật khẩu để bảo mật )
            string query = "SELECT TenDangNhap, TenHienThi, LoaiTaiKhoan FROM [dbo].[TaiKhoan]";
            return DataProvider.Instance.ExecuteQuery(query);
        }
        // 2.2. Thêm tài khoản mới
        // Mặc định mật khẩu khi tạo mới sẽ là "1"
        public bool InsertAccount(string name, string displayName, int type)
        {
            string query = string.Format("INSERT dbo.TaiKhoan (TenDangNhap, TenHienThi, LoaiTaiKhoan, MatKhau) VALUE (N'{0}', N'{1}', N'{2}', N'1')", name, displayName, type);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        // 2.3 Sửa tài khoản
        // Lưu ý: Không sửa được TenDangNhap (vì là khóa chính), chỉ sửa tên hiển thị và loại 
        public bool UpdateAccount(string name, string displayName, int type)
        {
            string query = string.Format("UPDATE dbo.TaiKhoan SET TenHienThi = N'{1}', LoaiTaiKhoan = {2} WHERE TenDangNhap = N'{0}'", name, displayName, type);

            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        //2.4. Xóa mật khẩu
        public bool DeleteAccount(string name)
        {
            // Lưu ý: Nếu tài khoản này đã lập hóa đơn, xóa sẽ bị lỗi ( do ràng buộc khóa ngoại )
            // Cần dùng Try-catch bên form để bắt lỗi này
            string query = string.Format("DELETE dbo.TaiKhoan WHERE TenDangNhap = N'{0}'", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        // 2.5 Đặt lại mật khẩu (Reset Password )
        // Đưa mật khẩu về mặc định là "1"
        public bool ResetPassword(string name)
        {
            string query = string.Format("UPDATE dbo.TaiKhoan SET MatKhau = N'1' WHERE TenDangNhap = n'{0}'", name);
            int result  = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
    }
}