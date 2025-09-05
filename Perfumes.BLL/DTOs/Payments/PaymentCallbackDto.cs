namespace Perfumes.BLL.DTOs.Payment
{
    public class PaymentCallbackDto
    {
        public string TransactionId { get; set; } = string.Empty;
        public string OrderNumber { get; set; } = string.Empty;
        public decimal AmountPaid { get; set; }
        public string Status { get; set; } = string.Empty; 
        public string PaymentMethod { get; set; } = string.Empty;
        public string Gateway { get; set; } = string.Empty;
    }
}
