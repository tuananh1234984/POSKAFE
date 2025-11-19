using PosKafe.DAO;
using PosKafe.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosKafe.BUS
{
    public class BillBUS {
        private static BillBUS instance;
        public static BillBUS Instance
        {
            get { if (instance == null) instance = new BillBUS(); return instance; }
            private set { instance = value; }
        }
        private BillBUS() { }

        // ---Hàm nghiệp vụ chính (Thanh toán & Lưu Tạm) ---
        //trangThai: 1(Đã thanh toán) hoặc 0 (Lưu tạm)
        public int XuLyGiaoDich(string tenNV, decimal tongTien, List<BillInfoDTO> billInfos, int trangThai)
        {
            // 1. Insert a new bill and get its ID
            int idHoaDon = BillDAO.Instance.InsertBill(tenNV, tongTien, trangThai);

            // 2. If the bill was inserted successfully, insert the bill details
            if (idHoaDon > 0)
            {
                foreach (var info in billInfos)
                {
                    BillDAO.Instance.InsertBillInfo(idHoaDon, info.ID_SanPham, info.SoLuong);
                }
            }

            return idHoaDon;
        }
    }
}
