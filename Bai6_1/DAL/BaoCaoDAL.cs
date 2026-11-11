using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Bai6_1.DAL
{
    public class BaoCaoDAL
    {
        // Tổng doanh thu trong ngày
        public decimal DoanhThuNgay()
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string sql = @"
                    SELECT ISNULL(SUM(TongTien), 0)
                    FROM HoaDon
                    WHERE NgayLap >= CAST(GETDATE() AS DATE)
                    AND NgayLap < DATEADD(DAY, 1, CAST(GETDATE() AS DATE))";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    return Convert.ToDecimal(cmd.ExecuteScalar());
                }
            }
        }

        // Tổng doanh thu trong tháng
        public decimal DoanhThuThang()
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string sql = @"
                    SELECT ISNULL(SUM(TongTien), 0)
                    FROM HoaDon
                    WHERE YEAR(NgayLap) = YEAR(GETDATE())
                    AND MONTH(NgayLap) = MONTH(GETDATE())";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    return Convert.ToDecimal(cmd.ExecuteScalar());
                }
            }
        }

        // Top 5 sản phẩm bán chạy nhất
        public DataTable Top5SanPham()
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string sql = @"
                    SELECT TOP 5
                        SP.MaSP,
                        SP.TenSP,
                        SUM(CT.SoLuong) AS TongSoLuong,
                        SUM(CT.SoLuong * CT.DonGia) AS DoanhThu
                    FROM ChiTietHoaDon CT
                    INNER JOIN SanPham SP ON CT.MaSP=SP.MaSP
                    GROUP BY SP.MaSP, SP.TenSP
                    ORDER BY SUM(CT.SoLuong) DESC";

                conn.Open();

                DataTable dt = new DataTable();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                }

                return dt;
            }
        }
    }
}