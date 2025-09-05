namespace Perfumes.BLL.DTOs.Order
{
    public class OrderDetailsDto
    {
        public int OrderId { get; set; }
        public string? OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending";
        public string? PaymentMethod { get; set; }
        public string? Notes { get; set; }

        public string? Email { get; set; }  

        public List<OrderItemDto> Items { get; set; } = new();
        public ShippingInfoDto ShippingInfo { get; set; } = new();
    }
}
