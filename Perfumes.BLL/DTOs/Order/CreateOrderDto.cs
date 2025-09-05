using Perfumes.DAL.DTOs;

namespace Perfumes.BLL.DTOs.Order
{
    public class CreateOrderDto
    {
        public int? UserId { get; set; }  
        public string? VisitorId { get; set; }  

        public List<CreateOrderItemDto> Items { get; set; } = new();

        public ShippingInfoDto ShippingInfo { get; set; }

        public string? PaymentMethod { get; set; }  
        public string? CouponCode { get; set; }

        public string? Notes { get; set; }
    }
}
