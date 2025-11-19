using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosKafe.DTO
{
    public class NhomSanPhamDTO
    {
        public int ID { get; set; }
        public string TenNhom { get; set; }

        public NhomSanPhamDTO(int id, string tenNhom)
        {
            this.ID = id;
            this.TenNhom = tenNhom;
        }

        // Hàm này giúp chuyển 1 dòng dữ liệu từ SQL thành Object C#
        public NhomSanPhamDTO(DataRow row)
        {
            this.ID = (int)row["ID"];
            this.TenNhom = row["TenNhom"].ToString();
        }
    }
}
