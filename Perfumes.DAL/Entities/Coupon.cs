using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Perfumes.DAL.Entities
{
    public class Coupon
    {
        [Key]
        public int CouponId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Code { get; set; }
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal? DiscountPercent { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal? DiscountAmount { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal? MinOrderAmount { get; set; }
        
        public int? MaxUses { get; set; }
        
        public int UsedCount { get; set; } = 0;
        
        public DateTime? ValidFrom { get; set; }
        
        public DateTime? ValidUntil { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual ICollection<Order> Orders { get; set; }
    }
} 