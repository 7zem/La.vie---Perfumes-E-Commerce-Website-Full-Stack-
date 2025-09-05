using System.ComponentModel.DataAnnotations;

namespace Perfumes.BLL.DTOs.Product
{
    public class CreateProductDto
    {
        [Required]
        [StringLength(200, ErrorMessage = "Product name cannot exceed 200 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string? Description { get; set; }

        [StringLength(50, ErrorMessage = "SKU cannot exceed 50 characters")]
        public string? SKU { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative")]
        public int Stock { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Brand ID must be valid")]
        public int? BrandId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Category ID must be valid")]
        public int? CategoryId { get; set; }

        [Range(0.001, double.MaxValue, ErrorMessage = "Weight must be greater than 0")]
        public decimal? Weight { get; set; }

        [StringLength(100, ErrorMessage = "Dimensions cannot exceed 100 characters")]
        public string? Dimensions { get; set; }

        [StringLength(500, ErrorMessage = "Fragrance notes cannot exceed 500 characters")]
        public string? FragranceNotes { get; set; }

        [StringLength(50, ErrorMessage = "Concentration cannot exceed 50 characters")]
        public string? Concentration { get; set; }

        [StringLength(20, ErrorMessage = "Volume cannot exceed 20 characters")]
        public string? Volume { get; set; }

        [StringLength(20, ErrorMessage = "Gender cannot exceed 20 characters")]
        public string? Gender { get; set; }

        [StringLength(50, ErrorMessage = "Season cannot exceed 50 characters")]
        public string? Season { get; set; }

        [StringLength(50, ErrorMessage = "Longevity cannot exceed 50 characters")]
        public string? Longevity { get; set; }

        [StringLength(50, ErrorMessage = "Sillage cannot exceed 50 characters")]
        public string? Sillage { get; set; }

        public bool IsActive { get; set; } = true;
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
    }
}
