using System.ComponentModel.DataAnnotations;

namespace Perfumes.BLL.DTOs.Coupon
{
    public class CreateCouponDto
    {
        [Required]
        [StringLength(50)]
        public string Code { get; set; } = string.Empty;

        public decimal? DiscountPercent { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? MinOrderAmount { get; set; }
        public int? MaxUses { get; set; }

        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidUntil { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
