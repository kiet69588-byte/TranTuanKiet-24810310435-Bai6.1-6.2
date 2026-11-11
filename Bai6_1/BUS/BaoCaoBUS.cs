using System.Data;
using Bai6_1.DAL;

namespace Bai6_1.BUS
{
    public class BaoCaoBUS
    {
        private BaoCaoDAL dal = new BaoCaoDAL();

        public decimal DoanhThuNgay()
        {
            return dal.DoanhThuNgay();
        }

        public decimal DoanhThuThang()
        {
            return dal.DoanhThuThang();
        }

        public DataTable Top5SanPham()
        {
            return dal.Top5SanPham();
        }
    }
}