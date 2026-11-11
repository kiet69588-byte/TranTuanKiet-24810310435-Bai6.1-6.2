using System;
using System.Data;
using Bai6_1.DAL;
using Bai6_1.DTO;

namespace Bai6_1.BUS
{
    public class NhanVienBUS
    {
        private NhanVienDAL dal = new NhanVienDAL();

        public DataTable GetAll()
        {
            return dal.GetAll();
        }

        public DataTable Search(string keyword)
        {
            return dal.Search(keyword);
        }

        public bool Insert(NhanVienDTO nv)
        {
            if (string.IsNullOrWhiteSpace(nv.TenNV))
                throw new Exception("Tên nhân viên không được để trống!");

            return dal.Insert(nv);
        }

        public bool Update(NhanVienDTO nv)
        {
            if (nv.MaNV <= 0 ||
                string.IsNullOrWhiteSpace(nv.TenNV))
                throw new Exception("Thông tin nhân viên không hợp lệ!");

            return dal.Update(nv);
        }

        public bool Delete(int maNV)
        {
            return dal.Delete(maNV);
        }
    }
}