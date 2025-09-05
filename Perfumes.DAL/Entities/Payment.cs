using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Perfumes.DAL.Entities
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        
        public int OrderId { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal AmountPaid { get; set; }
        
        [StringLength(50)]
        public string? Status { get; set; }
        
        [StringLength(50)]
        public string? PaymentMethod { get; set; }
        
        [StringLength(100)]
        public string? TransactionId { get; set; }
        
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        
        [StringLength(50)]
        public string? Gateway { get; set; }

        // Navigation Properties
        public virtual Order Order { get; set; }
    }
} 