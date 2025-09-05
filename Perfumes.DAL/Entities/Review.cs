using System.ComponentModel.DataAnnotations;

namespace Perfumes.DAL.Entities
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        
        public int UserId { get; set; }
        
        public int ProductId { get; set; }
        
        [Range(1, 5)]
        public int Rating { get; set; }
        
        [StringLength(1000)]
        public string? Comment { get; set; }
        
        public bool IsApproved { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual User User { get; set; }
        public virtual Product Product { get; set; }
    }
} 