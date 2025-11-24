using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosKafe.DAO
{
    public class StoreDAO
    {
        private static StoreDAO instance;
        public static StoreDAO Instance { 
            get { if (instance == null) instance = new StoreDAO(); return instance; }
            private set { StoreDAO.instance = value; }
        }
        private StoreDAO()
        {
            // để trống
        }

        public DataRow GetStoreInfo()
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM ThongTinQuan");
            if (data.Rows.Count > 0) return data.Rows[0];
            return null; 
        }

        public bool UpdateStoreInfo(string TenQuan, string diachi, string sdt, string loichao, string wifi, string MaSoThue, string TenNganHang, string SoTaiKhoan, string ChuTaiKhoan, string logoPath)
        {
            string query = string.Format("UPDATE ThongTinQuan SET TenQuan = N'{0}', DiaChi = N'{1}', SDT = N'{2}', LoiChao = N'{3}', WifiInfo = N'{4}', MaSoThue = N'{5}', NganHang = N'{6}', SoTaiKhoan = N'{7}', ChuTaiKhoan = N'{8}', LogoPath = N'{9}' WHERE ID = 1", TenQuan, diachi, sdt, loichao, wifi, MaSoThue, TenNganHang, SoTaiKhoan, ChuTaiKhoan, logoPath);
            return DataProvider.Instance.ExecuteNonQuery(query) > 0;
        }
    }
}
