using System.Data;
using Microsoft.Data.SqlClient;
using Bai6_2.DTO;

namespace Bai6_2.DAL
{
    public class NhanVienDAL
    {


        public DataTable GetAll()
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string sql = @"
                    SELECT NV.MaNV, NV.HoTen, NV.NgaySinh,
                           NV.CCCD, NV.MaPB, PB.TenPB,
                           NV.LuongCoBan, NV.HeSoLuong
                    FROM NhanVien NV
                    INNER JOIN PhongBan PB ON NV.MaPB = PB.MaPB";

                using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable Search(string keyword)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string sql = @"
                    SELECT NV.MaNV, NV.HoTen, NV.NgaySinh,
                           NV.CCCD, NV.MaPB, PB.TenPB,
                           NV.LuongCoBan, NV.HeSoLuong
                    FROM NhanVien NV
                    INNER JOIN PhongBan PB ON NV.MaPB = PB.MaPB
                    WHERE NV.HoTen LIKE @TuKhoa
                       OR NV.CCCD LIKE @TuKhoa
                       OR PB.TenPB LIKE @TuKhoa";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@TuKhoa", SqlDbType.NVarChar)
                        .Value = "%" + keyword + "%";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public void Add(NhanVienDTO nv)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string sql = @"
                    INSERT INTO NhanVien
                    (HoTen, NgaySinh, CCCD, MaPB, LuongCoBan, HeSoLuong)
                    VALUES
                    (@HoTen, @NgaySinh, @CCCD, @MaPB, @LuongCoBan, @HeSoLuong)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    AddParameters(cmd, nv);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(NhanVienDTO nv)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string sql = @"
                    UPDATE NhanVien
                    SET HoTen = @HoTen,
                        NgaySinh = @NgaySinh,
                        CCCD = @CCCD,
                        MaPB = @MaPB,
                        LuongCoBan = @LuongCoBan,
                        HeSoLuong = @HeSoLuong
                    WHERE MaNV = @MaNV";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    AddParameters(cmd, nv);
                    cmd.Parameters.Add("@MaNV", SqlDbType.Int).Value = nv.MaNV;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int maNV)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string sql = "DELETE FROM NhanVien WHERE MaNV = @MaNV";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@MaNV", SqlDbType.Int).Value = maNV;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void AddParameters(SqlCommand cmd, NhanVienDTO nv)
        {
            cmd.Parameters.Add("@HoTen", SqlDbType.NVarChar, 100)
                .Value = nv.HoTen;

            cmd.Parameters.Add("@NgaySinh", SqlDbType.DateTime)
                .Value = nv.NgaySinh.HasValue
                    ? (object)nv.NgaySinh.Value
                    : DBNull.Value;

            cmd.Parameters.Add("@CCCD", SqlDbType.VarChar, 20)
                .Value = string.IsNullOrWhiteSpace(nv.CCCD)
                    ? (object)DBNull.Value
                    : nv.CCCD;

            cmd.Parameters.Add("@MaPB", SqlDbType.Int)
                .Value = nv.MaPB;

            SqlParameter luong = cmd.Parameters.Add(
                "@LuongCoBan", SqlDbType.Decimal);

            luong.Precision = 18;
            luong.Scale = 2;
            luong.Value = nv.LuongCoBan;

            cmd.Parameters.Add("@HeSoLuong", SqlDbType.Float)
                .Value = nv.HeSoLuong;
        }
    }
}