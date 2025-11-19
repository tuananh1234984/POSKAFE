using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PosKafe // (Tên project của bạn)
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 1. Tạo Form Login
            Form_login frmLogin = new Form_login();

            // 2. Hiển thị Form Login (dạng ShowDialog) và chờ kết quả
            //    Hàm Main() sẽ "tạm dừng" ở đây
            if (frmLogin.ShowDialog() == DialogResult.OK)
            {
                // 3. NẾU Form Login trả về "OK" (đăng nhập thành công)
                //    THÌ mới chạy Form Main
                Application.Run(new Form_main()); // (Bạn phải tạo Form_Main trước)
            }

            // 4. NẾU Form Login trả về kết quả khác (Cancel, Thoát)
            //    thì hàm Main() sẽ kết thúc và ứng dụng tự tắt
        }
    }
}