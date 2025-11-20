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

        public decimal GiaVon { get; set; }

        public string DonViTinh { get; set; }

        public bool TrangThai { get; set; }

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

            // Kiểm tra null để tránh lỗi khi đọc dữ liệu
            if (row["GiaVon"].ToString() != "") this.GiaVon = Convert.ToDecimal(row["GiaVon"]);
            else this.GiaVon = 0;

            this.DonViTinh = row["DonViTinh"].ToString();

            if (row["TrangThai"].ToString() != "") this.TrangThai = (bool)row["TrangThai"];
            else this.TrangThai = true;
        }
        public SanPhamDTO() { }
    }
}
