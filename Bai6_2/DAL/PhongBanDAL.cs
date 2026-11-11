
using System.Data;
using Microsoft.Data.SqlClient;
using Bai6_2.DTO;

namespace Bai6_2.DAL
{
    public class PhongBanDAL
    {
        public DataTable GetAll()
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string sql = "SELECT * FROM PhongBan";

                using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }
    }
}
