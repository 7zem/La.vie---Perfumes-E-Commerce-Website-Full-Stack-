namespace Perfumes.BLL.DTOs.Order
{
    public class OrderSummaryDto
    {
        public int OrderId { get; set; }
        public string? OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending";
        public string? PaymentMethod { get; set; }
    }
}
