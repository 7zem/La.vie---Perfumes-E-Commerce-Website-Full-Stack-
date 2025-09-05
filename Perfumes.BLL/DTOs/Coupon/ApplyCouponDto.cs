namespace Perfumes.BLL.DTOs.Coupon
{
    public class ApplyCouponDto
    {
        public string Code { get; set; } = string.Empty;
        public decimal OrderAmount { get; set; }
    }
}
