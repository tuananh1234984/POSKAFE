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
    }
}
