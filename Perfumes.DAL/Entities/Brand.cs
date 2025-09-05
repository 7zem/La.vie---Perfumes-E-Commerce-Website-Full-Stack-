using System.ComponentModel.DataAnnotations;

namespace Perfumes.DAL.Entities
{
    public class Brand
    {
        [Key]
        public int BrandId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        [StringLength(255)]
        public string? LogoUrl { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual ICollection<Product> Products { get; set; }
    }
} 