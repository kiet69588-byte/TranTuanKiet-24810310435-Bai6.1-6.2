using System.Data;
using Microsoft.Data.SqlClient;
using Bai6_1.DTO;

namespace Bai6_1.DAL
{
    public class SanPhamDAL
    {
        public DataTable GetAll()
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string sql = "SELECT * FROM SanPham ORDER BY MaSP";

                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }

        public DataTable Search(string keyword)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string sql = @"SELECT * FROM SanPham
                    WHERE MaSP LIKE @TuKhoa
                    OR TenSP LIKE @TuKhoa";

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

        public DataTable GetByMaSP(string maSP)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string sql = "SELECT * FROM SanPham WHERE MaSP = @MaSP";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@MaSP", SqlDbType.VarChar, 20)
                        .Value = maSP;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    return dt;
                }
            }
        }

        public bool Insert(SanPhamDTO sp)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string sql = @"INSERT INTO SanPham
                    (MaSP, TenSP, GiaBan, SoLuongTon)
                    VALUES (@MaSP, @TenSP, @GiaBan, @SoLuongTon)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@MaSP", SqlDbType.VarChar, 20)
                        .Value = sp.MaSP;

                    cmd.Parameters.Add("@TenSP", SqlDbType.NVarChar, 150)
                        .Value = sp.TenSP;

                    SqlParameter gia = cmd.Parameters.Add(
                        "@GiaBan", SqlDbType.Decimal);
                    gia.Precision = 18;
                    gia.Scale = 2;
                    gia.Value = sp.GiaBan;

                    cmd.Parameters.Add("@SoLuongTon", SqlDbType.Int)
                        .Value = sp.SoLuongTon;

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Update(SanPhamDTO sp)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string sql = @"UPDATE SanPham SET
                    TenSP = @TenSP,
                    GiaBan = @GiaBan,
                    SoLuongTon = @SoLuongTon
                    WHERE MaSP = @MaSP";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@MaSP", SqlDbType.VarChar, 20)
                        .Value = sp.MaSP;

                    cmd.Parameters.Add("@TenSP", SqlDbType.NVarChar, 150)
                        .Value = sp.TenSP;

                    SqlParameter gia = cmd.Parameters.Add(
                        "@GiaBan", SqlDbType.Decimal);
                    gia.Precision = 18;
                    gia.Scale = 2;
                    gia.Value = sp.GiaBan;

                    cmd.Parameters.Add("@SoLuongTon", SqlDbType.Int)
                        .Value = sp.SoLuongTon;

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Delete(string maSP)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string sql = "DELETE FROM SanPham WHERE MaSP = @MaSP";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@MaSP", SqlDbType.VarChar, 20)
                        .Value = maSP;

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}