using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosKafe.DTO
{
    public class AccountDTO
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public int Type { get; set; }

        public AccountDTO(DataRow row)
        {
            this.Username = row["TenDangNhap"].ToString();
            this.DisplayName = row["TenHienThi"].ToString();
            this.Password = row["Password"].ToString();
            this.Type = (int)row["LoaiTaiKhoan"];
        }
    }
}
