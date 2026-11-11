
using System.Data;
using Bai6_2.DAL;

namespace Bai6_2.BUS
{
    public class PhongBanBUS
    {
        private PhongBanDAL dal = new PhongBanDAL();

        public DataTable GetAll()
        {
            return dal.GetAll();
        }
    }
}
