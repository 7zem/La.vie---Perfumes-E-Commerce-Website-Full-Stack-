namespace Perfumes.BLL.DTOs.Product
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? SKU { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? ImageUrl { get; set; }
        public int? BrandId { get; set; }
        public string? BrandName { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public decimal? Weight { get; set; }
        public string? Dimensions { get; set; }
        public string? FragranceNotes { get; set; }
        public string? Concentration { get; set; }
        public string? Volume { get; set; }
        public string? Gender { get; set; }
        public string? Season { get; set; }
        public string? Longevity { get; set; }
        public string? Sillage { get; set; }
        public bool IsActive { get; set; }
    }
}
