namespace Perfumes.DAL.DTOs
{
    public class ShippingInfoDto
    {
        public int ShippingId { get; set; }
        public int OrderId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? ShippingMethod { get; set; }
        public string? TrackingNumber { get; set; }
    }
} 