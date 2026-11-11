using System.Data;
using Microsoft.Data.SqlClient;
using Bai6_1.DTO;

namespace Bai6_1.DAL
{
    public class NhanVienDAL
    {
        public DataTable GetAll()
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT * FROM NhanVien ORDER BY MaNV", conn);

                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }

        public DataTable Search(string keyword)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string sql = @"SELECT * FROM NhanVien
                    WHERE TenNV LIKE @TuKhoa
                    OR SoDienThoai LIKE @TuKhoa";

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

        public bool Insert(NhanVienDTO nv)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string sql = @"INSERT INTO NhanVien
                    (TenNV, SoDienThoai, ChucVu)
                    VALUES (@TenNV, @SDT, @ChucVu)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@TenNV", SqlDbType.NVarChar, 100)
                        .Value = nv.TenNV;

                    cmd.Parameters.Add("@SDT", SqlDbType.VarChar, 15)
                        .Value = string.IsNullOrWhiteSpace(nv.SoDienThoai)
                        ? DBNull.Value : nv.SoDienThoai;

                    cmd.Parameters.Add("@ChucVu", SqlDbType.NVarChar, 50)
                        .Value = string.IsNullOrWhiteSpace(nv.ChucVu)
                        ? DBNull.Value : nv.ChucVu;

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Update(NhanVienDTO nv)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string sql = @"UPDATE NhanVien SET
                    TenNV = @TenNV,
                    SoDienThoai = @SDT,
                    ChucVu = @ChucVu
                    WHERE MaNV = @MaNV";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@MaNV", SqlDbType.Int)
                        .Value = nv.MaNV;

                    cmd.Parameters.Add("@TenNV", SqlDbType.NVarChar, 100)
                        .Value = nv.TenNV;

                    cmd.Parameters.Add("@SDT", SqlDbType.VarChar, 15)
                        .Value = string.IsNullOrWhiteSpace(nv.SoDienThoai)
                        ? DBNull.Value : nv.SoDienThoai;

                    cmd.Parameters.Add("@ChucVu", SqlDbType.NVarChar, 50)
                        .Value = string.IsNullOrWhiteSpace(nv.ChucVu)
                        ? DBNull.Value : nv.ChucVu;

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Delete(int maNV)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string sql = "DELETE FROM NhanVien WHERE MaNV = @MaNV";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@MaNV", SqlDbType.Int)
                        .Value = maNV;

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}