using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PosKafe.DTO;
using System.Data;
using System.Security.RightsManagement;

namespace PosKafe.DAO
{
    public class BillDAO
    {
        private static BillDAO instance;
        public static BillDAO Instance
        {
            get { if (instance == null) instance = new BillDAO(); return instance; }
            private set { instance = value; }
        }

        private BillDAO() { }

        public int InsertBill(string tenNV, decimal tongTien, int trangThai)
        {
            string query = "INSERT INTO dbo.HoaDon (TenDangNhapNV, NgayLapHoaDon, TongTien, TrangThai) VALUES ( @tenNV , GETDATE() , @tongTien , @trangThai ); SELECT SCOPE_IDENTITY();";
            object[] parameters = new object[] { tenNV, tongTien, trangThai };
            // ExecuteScalar is used to retrieve a single value (the new bill ID)
            object result = DataProvider.Instance.ExecuteScalar(query, parameters);
            // Safely convert the result to an integer
            return result != null ? Convert.ToInt32(result) : -1;
        }

        public void InsertBillInfo(int idHoaDon, int idSanPham, int soLuong)
        {
            string query = "EXECUTE USP_InsertBillInfo @idHoaDon , @idSanPham , @soLuong;";
            DataProvider.Instance.ExecuteNonQuery(query, new object[] { idHoaDon, idSanPham, soLuong });
        }
    }
}
