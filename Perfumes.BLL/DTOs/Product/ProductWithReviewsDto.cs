using Perfumes.DAL.DTOs;

namespace Perfumes.BLL.DTOs.Product
{
    public class ProductWithReviewsDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public List<ReviewDto> Reviews { get; set; } = new();
    }
}
