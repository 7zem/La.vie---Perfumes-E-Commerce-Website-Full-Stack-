using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Perfumes.DAL.Entities
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        [StringLength(50)]
        public string? SKU { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
        
        public int Stock { get; set; } = 0;
        
        [StringLength(255)]
        public string? ImageUrl { get; set; }
        
        public int? BrandId { get; set; }
        
        public int? CategoryId { get; set; }
        
        [Column(TypeName = "decimal(8,3)")]
        public decimal? Weight { get; set; }
        
        [StringLength(100)]
        public string? Dimensions { get; set; }
        
        [StringLength(500)]
        public string? FragranceNotes { get; set; }
        
        [StringLength(50)]
        public string? Concentration { get; set; }
        
        [StringLength(20)]
        public string? Volume { get; set; }
        
        [StringLength(20)]
        public string? Gender { get; set; }
        
        [StringLength(50)]
        public string? Season { get; set; }
        
        [StringLength(50)]
        public string? Longevity { get; set; }
        
        [StringLength(50)]
        public string? Sillage { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual Brand? Brand { get; set; }
        public virtual Category? Category { get; set; }
        public virtual ICollection<Cart> CartItems { get; set; }
        public virtual ICollection<Wishlist> WishlistItems { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual Inventory? Inventory { get; set; }
    }
} 