using System.Data;
using Microsoft.Data.SqlClient;
using Bai6_1.DTO;

namespace Bai6_1.DAL
{
    public class KhachHangDAL
    {
        public DataTable GetAll()
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT * FROM KhachHang ORDER BY MaKH", conn);

                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }

        public DataTable Search(string keyword)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string sql = @"SELECT * FROM KhachHang
                    WHERE TenKH LIKE @TuKhoa
                    OR DienThoai LIKE @TuKhoa";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@TuKhoa", SqlDbType.NVarChar, 200)
                        .Value = "%" + keyword + "%";

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    return dt;
                }
            }
        }

        public bool Insert(KhachHangDTO kh)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string sql = @"INSERT INTO KhachHang
                    (TenKH, DienThoai, DiemTichLuy)
                    VALUES (@TenKH, @DienThoai, @Diem)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@TenKH", SqlDbType.NVarChar, 100)
                        .Value = kh.TenKH;

                    cmd.Parameters.Add("@DienThoai", SqlDbType.VarChar, 15)
                        .Value = string.IsNullOrWhiteSpace(kh.DienThoai)
                        ? DBNull.Value : kh.DienThoai;

                    cmd.Parameters.Add("@Diem", SqlDbType.Int)
                        .Value = kh.DiemTichLuy;

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Update(KhachHangDTO kh)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string sql = @"UPDATE KhachHang SET
                    TenKH = @TenKH,
                    DienThoai = @DienThoai,
                    DiemTichLuy = @Diem
                    WHERE MaKH = @MaKH";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@MaKH", SqlDbType.Int)
                        .Value = kh.MaKH;

                    cmd.Parameters.Add("@TenKH", SqlDbType.NVarChar, 100)
                        .Value = kh.TenKH;

                    cmd.Parameters.Add("@DienThoai", SqlDbType.VarChar, 15)
                        .Value = string.IsNullOrWhiteSpace(kh.DienThoai)
                        ? DBNull.Value : kh.DienThoai;

                    cmd.Parameters.Add("@Diem", SqlDbType.Int)
                        .Value = kh.DiemTichLuy;

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Delete(int maKH)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string sql = "DELETE FROM KhachHang WHERE MaKH = @MaKH";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@MaKH", SqlDbType.Int)
                        .Value = maKH;

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}