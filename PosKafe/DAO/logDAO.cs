using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosKafe.DAO
{
    public class LogDAO
    {
        private static LogDAO instance;
        public static LogDAO Instance
        {
            get { if (instance == null) instance = new LogDAO(); return instance; }
            private set { instance = value; }
        }

        private LogDAO() { }

        // Hàm ghi nhật ký (Sửa tên thành WriteLog)
        public void WriteLog(string user, string action)
        {
            // Sửa lỗi thiếu dấu đóng ngoặc ) ở cuối câu lệnh
            string query = string.Format("INSERT INTO NhatKyHoatDong (NguoiThucHien, HanhDong) VALUES (N'{0}', N'{1}')", user, action);
            DataProvider.Instance.ExecuteNonQuery(query);
        }

        // Hàm lấy nhật ký để xem
        public DataTable GetLogs()
        {
            return DataProvider.Instance.ExecuteQuery("SELECT TOP 100 * FROM NhatKyHoatDong ORDER BY ThoiGian DESC");
        }
    }
}