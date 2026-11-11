
using System.Data;
using Bai6_2.DAL;

namespace Bai6_2.BUS
{
    public class ChamCongBUS
    {
        private ChamCongDAL dal = new ChamCongDAL();

        public DataTable LoadChamCong(string thangNam)
        {
            return dal.LoadChamCong(thangNam);
        }

        public void SaveChamCong(DataTable dt, string thangNam)
        {
            dal.SaveChamCong(dt, thangNam);
        }

        public void ChotLuong(string thangNam)
        {
            dal.ChotLuong(thangNam);
        }

        public bool IsDaChot(string thangNam)
        {
            return dal.IsDaChot(thangNam);
        }

        public DataTable GetBangLuong(string thangNam)
        {
            return dal.GetBangLuong(thangNam);
        }
    }
}
