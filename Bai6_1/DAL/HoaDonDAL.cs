using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Bai6_1.DTO;

namespace Bai6_1.DAL
{
    public class HoaDonDAL
    {
        public int ThanhToan(HoaDonDTO hd)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        if (hd.ChiTiet == null || hd.ChiTiet.Count == 0)
                            throw new Exception("Giỏ hàng đang trống!");

                        // Kiểm tra nhân viên
                        string sqlNV =
                            "SELECT COUNT(*) FROM NhanVien WHERE MaNV=@MaNV";

                        using (SqlCommand cmd =
                            new SqlCommand(sqlNV, conn, trans))
                        {
                            cmd.Parameters.Add("@MaNV", SqlDbType.Int)
                                .Value = hd.MaNV;

                            if (Convert.ToInt32(cmd.ExecuteScalar()) == 0)
                                throw new Exception("Nhân viên không tồn tại!");
                        }

                        // Kiểm tra khách hàng
                        if (hd.MaKH.HasValue)
                        {
                            string sqlKH =
                                "SELECT COUNT(*) FROM KhachHang WHERE MaKH=@MaKH";

                            using (SqlCommand cmd =
                                new SqlCommand(sqlKH, conn, trans))
                            {
                                cmd.Parameters.Add("@MaKH", SqlDbType.Int)
                                    .Value = hd.MaKH.Value;

                                if (Convert.ToInt32(cmd.ExecuteScalar()) == 0)
                                    throw new Exception("Khách hàng không tồn tại!");
                            }
                        }

                        decimal tongTien = 0;

                        // Kiểm tra tồn kho và lấy giá bán từ CSDL
                        foreach (ChiTietHoaDonDTO ct in hd.ChiTiet)
                        {
                            if (ct.SoLuong <= 0)
                                throw new Exception("Số lượng không hợp lệ!");

                            string sqlCheck = @"
                                SELECT GiaBan, SoLuongTon
                                FROM SanPham WITH (UPDLOCK, HOLDLOCK)
                                WHERE MaSP=@MaSP";

                            using (SqlCommand cmd =
                                new SqlCommand(sqlCheck, conn, trans))
                            {
                                cmd.Parameters.Add("@MaSP", SqlDbType.VarChar, 20)
                                    .Value = ct.MaSP;

                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    if (!reader.Read())
                                        throw new Exception(
                                            "Không tìm thấy sản phẩm " + ct.MaSP);

                                    ct.DonGia = reader.GetDecimal(0);

                                    int tonKho = reader.GetInt32(1);

                                    if (tonKho < ct.SoLuong)
                                        throw new Exception(
                                            "Sản phẩm " + ct.MaSP +
                                            " không đủ số lượng tồn kho!");
                                }
                            }

                            tongTien += ct.DonGia * ct.SoLuong;
                        }

                        // Tạo hóa đơn
                        string sqlHD = @"
                            INSERT INTO HoaDon
                                (NgayLap, MaNV, MaKH, TongTien)
                            VALUES
                                (GETDATE(), @MaNV, @MaKH, @TongTien);

                            SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        int maHD;

                        using (SqlCommand cmd =
                            new SqlCommand(sqlHD, conn, trans))
                        {
                            cmd.Parameters.Add("@MaNV", SqlDbType.Int)
                                .Value = hd.MaNV;

                            cmd.Parameters.Add("@MaKH", SqlDbType.Int)
                                .Value = hd.MaKH.HasValue
                                ? hd.MaKH.Value : DBNull.Value;

                            SqlParameter p = cmd.Parameters.Add(
                                "@TongTien", SqlDbType.Decimal);

                            p.Precision = 18;
                            p.Scale = 2;
                            p.Value = tongTien;

                            maHD = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        // Thêm chi tiết hóa đơn và trừ kho
                        foreach (ChiTietHoaDonDTO ct in hd.ChiTiet)
                        {
                            string sqlCT = @"
                                INSERT INTO ChiTietHoaDon
                                    (MaHD, MaSP, SoLuong, DonGia)
                                VALUES
                                    (@MaHD, @MaSP, @SoLuong, @DonGia)";

                            using (SqlCommand cmd =
                                new SqlCommand(sqlCT, conn, trans))
                            {
                                cmd.Parameters.Add("@MaHD", SqlDbType.Int)
                                    .Value = maHD;

                                cmd.Parameters.Add("@MaSP", SqlDbType.VarChar, 20)
                                    .Value = ct.MaSP;

                                cmd.Parameters.Add("@SoLuong", SqlDbType.Int)
                                    .Value = ct.SoLuong;

                                SqlParameter p = cmd.Parameters.Add(
                                    "@DonGia", SqlDbType.Decimal);

                                p.Precision = 18;
                                p.Scale = 2;
                                p.Value = ct.DonGia;

                                cmd.ExecuteNonQuery();
                            }

                            string sqlTruKho = @"
                                UPDATE SanPham
                                SET SoLuongTon = SoLuongTon - @SoLuong
                                WHERE MaSP = @MaSP
                                AND SoLuongTon >= @SoLuong";

                            using (SqlCommand cmd =
                                new SqlCommand(sqlTruKho, conn, trans))
                            {
                                cmd.Parameters.Add("@SoLuong", SqlDbType.Int)
                                    .Value = ct.SoLuong;

                                cmd.Parameters.Add("@MaSP", SqlDbType.VarChar, 20)
                                    .Value = ct.MaSP;

                                if (cmd.ExecuteNonQuery() != 1)
                                    throw new Exception(
                                        "Không đủ tồn kho sản phẩm " + ct.MaSP);
                            }
                        }

                        // Cộng điểm tích lũy
                        if (hd.MaKH.HasValue)
                        {
                            string sqlDiem = @"
                                UPDATE KhachHang
                                SET DiemTichLuy = DiemTichLuy
                                    + CAST(@TongTien / 100000 AS INT)
                                WHERE MaKH = @MaKH";

                            using (SqlCommand cmd =
                                new SqlCommand(sqlDiem, conn, trans))
                            {
                                SqlParameter p = cmd.Parameters.Add(
                                    "@TongTien", SqlDbType.Decimal);

                                p.Precision = 18;
                                p.Scale = 2;
                                p.Value = tongTien;

                                cmd.Parameters.Add("@MaKH", SqlDbType.Int)
                                    .Value = hd.MaKH.Value;

                                cmd.ExecuteNonQuery();
                            }
                        }

                        // Xác nhận giao dịch
                        trans.Commit();

                        return maHD;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        public DataTable GetAll()
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string sql = @"
                    SELECT HD.MaHD, HD.NgayLap,
                           NV.TenNV, KH.TenKH, HD.TongTien
                    FROM HoaDon HD
                    INNER JOIN NhanVien NV ON HD.MaNV=NV.MaNV
                    LEFT JOIN KhachHang KH ON HD.MaKH=KH.MaKH
                    ORDER BY HD.MaHD DESC";

                SqlDataAdapter da = new SqlDataAdapter(sql, conn);

                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }
    }
}