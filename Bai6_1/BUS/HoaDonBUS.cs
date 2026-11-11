using System;
using System.Data;
using Bai6_1.DAL;
using Bai6_1.DTO;

namespace Bai6_1.BUS
{
    public class HoaDonBUS
    {
        private HoaDonDAL dal = new HoaDonDAL();

        public int ThanhToan(HoaDonDTO hd)
        {
            if (hd.MaNV <= 0)
                throw new Exception("Vui lòng chọn nhân viên!");

            if (hd.ChiTiet == null || hd.ChiTiet.Count == 0)
                throw new Exception("Giỏ hàng đang trống!");

            return dal.ThanhToan(hd);
        }

        public DataTable GetAll()
        {
            return dal.GetAll();
        }
    }
}