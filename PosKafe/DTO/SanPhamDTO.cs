using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosKafe.DTO
{
    public class  SanPhamDTO
    {
        public int ID { get; set; }
        public string TenSanPham { get; set; }
        public int ID_Nhom { get; set; }
        public decimal Gia { get; set; }

        public SanPhamDTO(int id, string tenSanPham, int idNhom, decimal gia)
        {
            this.ID = id;
            this.TenSanPham = tenSanPham;
            this.ID_Nhom = idNhom;
            this.Gia = gia;
        }

        public SanPhamDTO(DataRow row)
        {
            this.ID = (int)row["ID"];
            this.TenSanPham = row["TenSanPham"].ToString();
            this.ID_Nhom = (int)row["ID_Nhom"];
            // luu y kieu du lieu float/double trong SQL
            this.Gia = Convert.ToDecimal(row["Gia"]);
        }
        public SanPhamDTO() { }
    }
}
