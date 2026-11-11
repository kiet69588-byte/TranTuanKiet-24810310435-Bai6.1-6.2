using System.Data;
using Bai6_2.DAL;
using Bai6_2.DTO;

namespace Bai6_2.BUS
{
    public class NhanVienBUS
    {
        private readonly NhanVienDAL dal = new NhanVienDAL();

        public DataTable GetAll()
        {
            return dal.GetAll();
        }

        public DataTable Search(string keyword)
        {
            return dal.Search(keyword);
        }

        public void Add(NhanVienDTO nv)
        {
            if (string.IsNullOrWhiteSpace(nv.HoTen))
                throw new Exception("Họ tên không được để trống.");

            if (nv.LuongCoBan <= 0)
                throw new Exception("Lương cơ bản phải lớn hơn 0.");

            if (nv.HeSoLuong <= 0)
                throw new Exception("Hệ số lương phải lớn hơn 0.");

            dal.Add(nv);
        }

        public void Update(NhanVienDTO nv)
        {
            if (nv.MaNV <= 0)
                throw new Exception("Mã nhân viên không hợp lệ.");

            if (string.IsNullOrWhiteSpace(nv.HoTen))
                throw new Exception("Họ tên không được để trống.");

            if (nv.LuongCoBan <= 0)
                throw new Exception("Lương cơ bản phải lớn hơn 0.");

            if (nv.HeSoLuong <= 0)
                throw new Exception("Hệ số lương phải lớn hơn 0.");

            dal.Update(nv);
        }

        public void Delete(int maNV)
        {
            dal.Delete(maNV);
        }
    }
}