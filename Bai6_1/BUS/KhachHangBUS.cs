using System;
using System.Data;
using Bai6_1.DAL;
using Bai6_1.DTO;

namespace Bai6_1.BUS
{
    public class KhachHangBUS
    {
        private KhachHangDAL dal = new KhachHangDAL();

        public DataTable GetAll()
        {
            return dal.GetAll();
        }

        public DataTable Search(string keyword)
        {
            return dal.Search(keyword);
        }

        public bool Insert(KhachHangDTO kh)
        {
            if (string.IsNullOrWhiteSpace(kh.TenKH))
                throw new Exception("Tên khách hàng không được để trống!");

            if (kh.DiemTichLuy < 0)
                throw new Exception("Điểm tích lũy không hợp lệ!");

            return dal.Insert(kh);
        }

        public bool Update(KhachHangDTO kh)
        {
            if (kh.MaKH <= 0 ||
                string.IsNullOrWhiteSpace(kh.TenKH))
                throw new Exception("Thông tin khách hàng không hợp lệ!");

            if (kh.DiemTichLuy < 0)
                throw new Exception("Điểm tích lũy không được âm!");

            return dal.Update(kh);
        }

        public bool Delete(int maKH)
        {
            return dal.Delete(maKH);
        }
    }
}