using System.ComponentModel.DataAnnotations;

namespace Perfumes.DAL.Entities
{
    public class Wishlist
    {
        [Key]
        public int WishlistId { get; set; }
        
        public int UserId { get; set; }
        
        public int ProductId { get; set; }
        
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual User User { get; set; }
        public virtual Product Product { get; set; }
    }
} 