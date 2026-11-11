
using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Bai6_2.DAL
{
    public class ChamCongDAL
    {
        public DataTable LoadChamCong(string thangNam)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string sql = @"
                    SELECT MaCong, MaNV, ThangNam,
                           SoNgayCong, SoGioOT, DaChot
                    FROM BangChamCong
                    WHERE ThangNam = @ThangNam";

                using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
                {
                    adapter.SelectCommand.Parameters.Add("@ThangNam",
                        SqlDbType.VarChar, 7).Value = thangNam;

                    DataTable dt = new DataTable();

                    adapter.FillSchema(dt, SchemaType.Source);
                    adapter.Fill(dt);

                    // Thêm cột tên nhân viên để hiển thị
                    dt.Columns.Add("HoTen", typeof(string));

                    string sqlNV = @"
                        SELECT MaNV, HoTen
                        FROM NhanVien
                        ORDER BY MaNV";

                    using (SqlCommand cmd = new SqlCommand(sqlNV, conn))
                    {
                        if (conn.State != ConnectionState.Open)
                            conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int maNV = reader.GetInt32(0);
                                string hoTen = reader.GetString(1);

                                DataRow[] rows = dt.Select("MaNV = " + maNV);

                                if (rows.Length > 0)
                                {
                                    rows[0]["HoTen"] = hoTen;
                                }
                                else
                                {
                                    DataRow row = dt.NewRow();

                                    row["MaNV"] = maNV;
                                    row["ThangNam"] = thangNam;
                                    row["SoNgayCong"] = 0;
                                    row["SoGioOT"] = 0;
                                    row["DaChot"] = false;
                                    row["HoTen"] = hoTen;

                                    dt.Rows.Add(row);
                                }
                            }
                        }
                    }

                    return dt;
                }
            }
        }

        // Lưu chấm công bằng Disconnected Model
        public void SaveChamCong(DataTable dt, string thangNam)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();

                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        // Kiểm tra tháng đã chốt hay chưa
                        string checkSql = @"
                            SELECT COUNT(*)
                            FROM BangChamCong
                            WHERE ThangNam = @ThangNam
                            AND DaChot = 1";

                        using (SqlCommand checkCmd = new SqlCommand(
                            checkSql, conn, tran))
                        {
                            checkCmd.Parameters.Add("@ThangNam",
                                SqlDbType.VarChar, 7).Value = thangNam;

                            if ((int)checkCmd.ExecuteScalar() > 0)
                                throw new Exception("Tháng này đã chốt lương!");
                        }

                        string sql = @"
                            SELECT MaCong, MaNV, ThangNam,
                                   SoNgayCong, SoGioOT, DaChot
                            FROM BangChamCong
                            WHERE ThangNam = @ThangNam";

                        using (SqlDataAdapter adapter =
                            new SqlDataAdapter(sql, conn))
                        {
                            adapter.SelectCommand.Parameters.Add("@ThangNam",
                                SqlDbType.VarChar, 7).Value = thangNam;

                            adapter.SelectCommand.Transaction = tran;

                            SqlCommandBuilder builder =
                                new SqlCommandBuilder(adapter);

                            adapter.UpdateCommand = builder.GetUpdateCommand();
                            adapter.InsertCommand = builder.GetInsertCommand();
                            adapter.DeleteCommand = builder.GetDeleteCommand();

                            // Chỉ đồng bộ dữ liệu chưa chốt
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row.RowState == DataRowState.Added)
                                {
                                    row["ThangNam"] = thangNam;
                                    row["DaChot"] = false;
                                }

                                if (row.RowState != DataRowState.Deleted)
                                {
                                    double ngay = Convert.ToDouble(row["SoNgayCong"]);
                                    double ot = Convert.ToDouble(row["SoGioOT"]);

                                    if (ngay < 0 || ngay > 31 || ot < 0)
                                        throw new Exception("Dữ liệu chấm công không hợp lệ!");
                                }
                            }

                            // Cột HoTen không thuộc bảng BangChamCong
                            // nên SqlCommandBuilder không cập nhật cột này.
                            adapter.Update(dt);
                        }

                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }

        // Kiểm tra tháng đã chốt
        public bool IsDaChot(string thangNam)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string sql = @"
                    SELECT COUNT(*)
                    FROM BangChamCong
                    WHERE ThangNam = @ThangNam AND DaChot = 1";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@ThangNam",
                        SqlDbType.VarChar, 7).Value = thangNam;

                    conn.Open();
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }

        // Chốt lương bằng SqlTransaction
        public void ChotLuong(string thangNam)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();

                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        string checkSql = @"
                            SELECT COUNT(*)
                            FROM BangChamCong
                            WHERE ThangNam = @ThangNam";

                        int soLuong;

                        using (SqlCommand cmd = new SqlCommand(
                            checkSql, conn, tran))
                        {
                            cmd.Parameters.Add("@ThangNam",
                                SqlDbType.VarChar, 7).Value = thangNam;

                            soLuong = (int)cmd.ExecuteScalar();
                        }

                        if (soLuong == 0)
                            throw new Exception("Chưa có dữ liệu chấm công!");

                        string sql = @"
                            SELECT COUNT(*)
                            FROM BangChamCong
                            WHERE ThangNam = @ThangNam
                            AND DaChot = 1";

                        using (SqlCommand cmd = new SqlCommand(
                            sql, conn, tran))
                        {
                            cmd.Parameters.Add("@ThangNam",
                                SqlDbType.VarChar, 7).Value = thangNam;

                            if ((int)cmd.ExecuteScalar() > 0)
                                throw new Exception("Tháng này đã được chốt lương!");
                        }

                        // Tính lương và ghi nhận bảng lương
                        string salarySql = @"
                            INSERT INTO BangLuong(MaNV, ThangNam, ThucLinh)
                            SELECT
                                NV.MaNV,
                                @ThangNam,
                                CAST(
                                    (NV.LuongCoBan * NV.HeSoLuong / 22.0)
                                    * BC.SoNgayCong
                                    + (BC.SoGioOT * 50000)
                                AS DECIMAL(18,2))
                            FROM BangChamCong BC
                            INNER JOIN NhanVien NV
                                ON BC.MaNV = NV.MaNV
                            WHERE BC.ThangNam = @ThangNam
                            AND BC.DaChot = 0";

                        using (SqlCommand cmd = new SqlCommand(
                            salarySql, conn, tran))
                        {
                            cmd.Parameters.Add("@ThangNam",
                                SqlDbType.VarChar, 7).Value = thangNam;

                            cmd.ExecuteNonQuery();
                        }

                        // Khóa dữ liệu chấm công
                        string lockSql = @"
                            UPDATE BangChamCong
                            SET DaChot = 1
                            WHERE ThangNam = @ThangNam";

                        using (SqlCommand cmd = new SqlCommand(
                            lockSql, conn, tran))
                        {
                            cmd.Parameters.Add("@ThangNam",
                                SqlDbType.VarChar, 7).Value = thangNam;

                            cmd.ExecuteNonQuery();
                        }

                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }

        // Lấy bảng lương
        public DataTable GetBangLuong(string thangNam)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string sql = @"
                    SELECT BL.MaLuong, NV.MaNV, NV.HoTen,
                           BL.ThangNam, BL.ThucLinh
                    FROM BangLuong BL
                    INNER JOIN NhanVien NV ON BL.MaNV = NV.MaNV
                    WHERE BL.ThangNam = @ThangNam
                    ORDER BY NV.MaNV";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@ThangNam",
                        SqlDbType.VarChar, 7).Value = thangNam;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }
    }
}
