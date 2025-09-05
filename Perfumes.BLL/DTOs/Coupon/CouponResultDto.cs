namespace Perfumes.BLL.DTOs.Coupon
{
    public class CouponResultDto
    {
        public bool IsValid { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? CouponId { get; set; }
        public string Code { get; set; } = string.Empty;
        public decimal DiscountValue { get; set; }
        public bool IsPercentage { get; set; }
    }
}
