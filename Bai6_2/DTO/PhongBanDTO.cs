
namespace Bai6_2.DTO
{
    public class PhongBanDTO
    {
        public int MaPB { get; set; }
        public string TenPB { get; set; }
        public string Mota { get; set; }

        public override string ToString()
        {
            return TenPB;
        }
    }
}
