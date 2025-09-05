using Perfumes.BLL.DTOs.Order;
using Perfumes.BLL.DTOs.Payment;

namespace Perfumes.BLL.Services.Interfaces
{
    public interface IOrderService
    {
        Task<string> CreateOrderAsync(CreateOrderDto dto);

        Task<OrderDetailsDto?> GetOrderByIdAsync(int orderId);

        Task<OrderDetailsDto?> GetOrderByNumberAsync(string orderNumber);

        Task<IEnumerable<OrderSummaryDto>> GetOrdersByUserAsync(int userId);

        Task<IEnumerable<OrderSummaryDto>> GetOrdersByStatusAsync(string status);

        Task<IEnumerable<OrderSummaryDto>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);

        Task<decimal> GetTotalSalesAsync(DateTime? start = null, DateTime? end = null);
        Task<int> GetOrdersCountAsync(DateTime? start = null, DateTime? end = null);

        Task<IEnumerable<OrderSummaryDto>> GetPendingOrdersAsync();
        Task<IEnumerable<OrderSummaryDto>> GetCompletedOrdersAsync();
        Task<bool> UpdateOrderStatusAsync(int orderId, string newStatus);
        Task<bool> UpdateShippingInfoAsync(int orderId, ShippingInfoDto dto);
        Task<bool> HandlePaymentCallbackAsync(PaymentCallbackDto dto);
    }
}
