using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosKafe.DAO
{
    public class DataProvider
    {
        //1. Chuỗi kết nối (Sửa lại cho đúng với máy của bạn)
        // Lấy Server name từ SSMS
        private static string str_ConnecLaptop = @"Data Source=DESKTOP-O1141BR;Initial Catalog=quanlyquanCafe;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
        private static string str_Desktop = @"Data Source=192.168.1.207,1433;Initial Catalog=quanlyquanCafe;User Id=sa;Password=tuananh_1098;TrustServerCertificate=True;";
        private string connectionString = str_ConnecLaptop;

        //2. Singleton Pattern (để gọi DataProvider dễ dàng hơn)
        // Đảm bảo chỉ có 1 instance được thể hiện của Provider
        private static DataProvider instance;
        public static DataProvider Instance
        {
            get { if (instance == null) instance = new DataProvider(); return instance; }
            private set { DataProvider.instance = value; }
        }
 
        private DataProvider() { } //Hàm dựng Private

        //3. Hàm thực thi query (SELECT) - trả về DataTable
        // (Sử dụng Parameterized Query để tránh SQL Injection)
        public DataTable ExecuteQuery(string query, object[] parameter = null)
        {
            DataTable data = new DataTable();
            using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);

                //Nếu có tham số (parameter)
                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            string paramName = System.Text.RegularExpressions.Regex.Match(item, @"@\w+").Value;
                            if (!string.IsNullOrEmpty(paramName) && i < parameter.Length)
                            {
                                command.Parameters.AddWithValue(paramName, parameter[i]);
                                i++;
                            }
                        }
                    }
                }
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(data);
                connection.Close();
            }
            return data;
        }
        public int ExecuteNonQuery(string query, object[] parameter = null )
        {
            int data = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);

                if (parameter != null) {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            string paramName = System.Text.RegularExpressions.Regex.Match(item, @"@\w+").Value;
                            if (!string.IsNullOrEmpty(paramName) && i < parameter.Length)
                            {
                                command.Parameters.AddWithValue(paramName, parameter[i]);
                                i++;
                            }
                        }
                    }
                }
                data = command.ExecuteNonQuery();
                connection.Close();
            }
            return data;
        }
        public object ExecuteScalar(string query, object[] parameter = null)
        {
            object data = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            string paramName = System.Text.RegularExpressions.Regex.Match(item, @"@\w+").Value;
                            if (!string.IsNullOrEmpty(paramName) && i < parameter.Length)
                            {
                                command.Parameters.AddWithValue(paramName, parameter[i]);
                                i++;
                            }
                        }
                    }
                }
                data = command.ExecuteScalar();
                connection.Close();
            }
            return data;
        }
    }
}
