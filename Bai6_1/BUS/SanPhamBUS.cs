using System;
using System.Data;
using Bai6_1.DAL;
using Bai6_1.DTO;

namespace Bai6_1.BUS
{
    public class SanPhamBUS
    {
        private SanPhamDAL dal = new SanPhamDAL();

        public DataTable GetAll()
        {
            return dal.GetAll();
        }

        public DataTable Search(string keyword)
        {
            return dal.Search(keyword);
        }

        public DataTable GetByMaSP(string maSP)
        {
            return dal.GetByMaSP(maSP);
        }

        public bool Insert(SanPhamDTO sp)
        {
            if (string.IsNullOrWhiteSpace(sp.MaSP))
                throw new Exception("Mã sản phẩm không được để trống!");

            if (string.IsNullOrWhiteSpace(sp.TenSP))
                throw new Exception("Tên sản phẩm không được để trống!");

            if (sp.GiaBan <= 0)
                throw new Exception("Giá bán phải lớn hơn 0!");

            if (sp.SoLuongTon < 0)
                throw new Exception("Số lượng tồn không được âm!");

            return dal.Insert(sp);
        }

        public bool Update(SanPhamDTO sp)
        {
            if (string.IsNullOrWhiteSpace(sp.MaSP) ||
                string.IsNullOrWhiteSpace(sp.TenSP))
                throw new Exception("Thông tin sản phẩm không hợp lệ!");

            if (sp.GiaBan <= 0 || sp.SoLuongTon < 0)
                throw new Exception("Giá bán hoặc số lượng tồn không hợp lệ!");

            return dal.Update(sp);
        }

        public bool Delete(string maSP)
        {
            return dal.Delete(maSP);
        }
    }
}