using System.ComponentModel.DataAnnotations;

namespace Perfumes.DAL.Entities
{
    public class ShippingInfo
    {
        [Key]
        public int ShippingId { get; set; }
        
        public int OrderId { get; set; }
        
        [StringLength(100)]
        public string? FirstName { get; set; }
        
        [StringLength(100)]
        public string? LastName { get; set; }
        
        [StringLength(255)]
        public string? Address { get; set; }
        
        [StringLength(100)]
        public string? City { get; set; }
        
        [StringLength(100)]
        public string? State { get; set; }
        
        [StringLength(20)]
        public string? PostalCode { get; set; }
        
        [StringLength(100)]
        public string? Country { get; set; }
        
        [StringLength(20)]
        public string? PhoneNumber { get; set; }
        
        [StringLength(255)]
        [EmailAddress]
        public string? Email { get; set; }
        
        [StringLength(50)]
        public string? ShippingMethod { get; set; }
        
        [StringLength(100)]
        public string? TrackingNumber { get; set; }

        // Navigation Properties
        public virtual Order Order { get; set; }
    }
} 