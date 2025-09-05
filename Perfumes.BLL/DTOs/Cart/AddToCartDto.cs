namespace Perfumes.BLL.DTOs.Cart
{
    public class AddToCartDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
        public int? UserId { get; set; }
        public string? VisitorId { get; set; }
    }
}
