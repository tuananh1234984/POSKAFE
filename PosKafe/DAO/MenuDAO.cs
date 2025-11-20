using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using PosKafe.DTO;

namespace PosKafe.DAO
{
    public class MenuDAO
    {
        private static MenuDAO instance;

        public static MenuDAO Instance
        {
            get { if (instance == null) instance = new MenuDAO(); return instance; }
            private set { instance = value; }
        }

        private MenuDAO()
        {
            // 1. Lấy danh sách Nhóm (Categories)
        }
        // 1. Lấy danh sách Nhóm (Categories)
        public List<NhomSanPhamDTO> GetListNhomSanPham(){
            List<NhomSanPhamDTO> list = new List<NhomSanPhamDTO>();
            string query = "SELECT * FROM NhomSanPham";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                NhomSanPhamDTO category = new NhomSanPhamDTO(item);
                list.Add(category);
            }
            return list;
        }

        public List<SanPhamDTO> GetListSanPhamByNhom(int idNhom)
        {
            List<SanPhamDTO> list = new List<SanPhamDTO>();
            // Lấy tất cả sản phẩm theo nhóm đó
            string query = "SELECT * FROM SanPham WHERE ID_Nhom = " + idNhom;

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                SanPhamDTO food = new SanPhamDTO(item);
                list.Add(food);
            }
            return list;
        }

        //3. Lấy tất cả sản phẩm (Cho Form Quản lý Menu)
        public List<SanPhamDTO> GetListFood()
        {
            List<SanPhamDTO> list = new List<SanPhamDTO>();
            // Lấy hết (kể cả món ngừng bán) để quản lý
            string query = "SELECT * FROM SanPham";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                SanPhamDTO food = new SanPhamDTO(item);
                list.Add(food);
            }
            return list;
        }

        public List<SanPhamDTO> SearchFoodByName(string name)
        {
            List<SanPhamDTO> list = new List<SanPhamDTO>();
            // Tìm kiếm gần đúng (LIKE)
            string query = string.Format("SELECT * FROM SanPham WHERE TenSanPham LIKE N'%{0}%'", name);

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                SanPhamDTO food = new SanPhamDTO(item);
                list.Add(food);
            }
            return list;
        }

        // =============================================================================================
        // Phần 2: Các Hàm cập nhật dữ liệu (write)
        // =============================================================================================

        //5. Thêm món mới
        public bool InsertFood(string name, int idcategory, float price, decimal GiaVon, string dvt, int trangThai) 
        {
            // Nếu chưa nhập DVT thì mặc định là 'ly'
            if (string.IsNullOrEmpty(dvt)) dvt = "Ly";

            // Lưu ý: string.Format giúp chèn biến vào câu SQL
            string query = string.Format("INSERT dbo.SanPham (TenSanPham, ID_Nhom, Gia, GiaVon, DonViTinh, TrangThai) VALUES (N'{0}', {1}, {2}, {3}, N'{4}', {5})",
                name, idcategory, price, GiaVon, dvt, trangThai);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        //6. Sửa món (update)
        public bool UpdateFood(int idfood, string name, int idCategory, float price, decimal GiaVon, string dvt, int trangThai)
        {
            string query = string.Format("UPDATE dbo.SanPham SET TenSanPham = N'{0}', ID_Nhom = {1}, Gia = {2}, GiaVon = {3}, DonViTinh = N'{4}', TrangThai = {5} WHERE ID = {6}",
                name, idCategory, price, GiaVon, dvt, trangThai, idfood);

            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        //7. Xóa món (delete)
        public bool DeleteFood(int idfood)
        {
            // Nếu món đã bán ( có trong BillInfo) thì SQL sẽ báo lỗi Foreign Key
            // Nên dùng Try-Catch ở bên trong Form để bắt lỗi này
            string query = string.Format("DELETE dbo.SanPham WHERE ID = {0}", idfood);

            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
    }
}
