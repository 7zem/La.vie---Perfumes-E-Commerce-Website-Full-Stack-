using System.ComponentModel.DataAnnotations;

namespace Perfumes.DAL.Entities
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        
        public int? UserId { get; set; }

        [StringLength(100)]
        public string? VisitorId { get; set; }  // Id for guests
        public int ProductId { get; set; }
        
        public int Quantity { get; set; } = 1;
        
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual User? User { get; set; }
        public virtual Product Product { get; set; }
    }
} 