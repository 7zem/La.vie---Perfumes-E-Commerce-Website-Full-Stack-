namespace Perfumes.BLL.DTOs.Product
{
    public class ProductSummaryDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public string? BrandName { get; set; }
    }
}
