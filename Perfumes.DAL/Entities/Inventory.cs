using System.ComponentModel.DataAnnotations;

namespace Perfumes.DAL.Entities
{
    public class Inventory
    {
        [Key]
        public int InventoryId { get; set; }
        
        public int ProductId { get; set; }
        
        public int Quantity { get; set; } = 0;
        
        public int ReservedQuantity { get; set; } = 0;
        
        [StringLength(100)]
        public string? Location { get; set; }
        
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual Product Product { get; set; }
    }
} 