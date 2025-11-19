using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Security.AccessControl;
using PosKafe.DAO;

namespace PosKafe.BUS
{
    public class TaiKhoanBus
    {
        private static TaiKhoanBus instance;
        public static TaiKhoanBus Instance
        {
            get { if (instance == null) instance = new TaiKhoanBus(); return instance; }
            private set { instance = value; }
        }
        private TaiKhoanBus() { }

        // Hàm Kiểm tra đang nhập
        // Hàm này chỉ có nhiệm vụ gọi xuống TaiKhoanDao
        // Update nếu cần logic phức tạp (ví dụ: Kiểm tra mật khẩu mạnh/yếu)
        public bool KiemTraDangNhap(string tenDangNhap, string matkhau)
        {
            //1. Băm mật khẩu (dạng 123) mà người dùng nhập vào
            string matkhauDaBam = HashPassword(matkhau);
            //2. Gọi DAO và so sánh mật khẩu Đã Băm
            return TaiKhoanDao.Instance.KiemTraDangNhap(tenDangNhap, matkhauDaBam);
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {   
                //2. Băm mật khẩu (chuyển về mảng byte)
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                //3. Chuyển mảng byte về dạng chuỗi Hexadecimal (64 ký tự )
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++) {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
