using Microsoft.AspNetCore.Mvc;
using Perfumes.BLL.DTOs.Order;
using Perfumes.BLL.DTOs.Payment;
using Perfumes.BLL.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Perfumes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IConfiguration _configuration;

        public OrderController(IOrderService orderService, IConfiguration configuration)
        {
            _orderService = orderService;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {
            var result = await _orderService.CreateOrderAsync(dto);
            return Ok(new { message = "Order created", redirect = result });
        }

        [HttpGet("id/{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            return order == null ? NotFound() : Ok(order);
        }

        [HttpGet("number/{orderNumber}")]
        public async Task<IActionResult> GetOrderByNumber(string orderNumber)
        {
            var order = await _orderService.GetOrderByNumberAsync(orderNumber);
            return order == null ? NotFound() : Ok(order);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUser(int userId)
        {
            var orders = await _orderService.GetOrdersByUserAsync(userId);
            return Ok(orders);
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetOrdersByStatus(string status)
        {
            var orders = await _orderService.GetOrdersByStatusAsync(status);
            return Ok(orders);
        }

        [HttpGet("date-range")]
        public async Task<IActionResult> GetOrdersByDateRange([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var orders = await _orderService.GetOrdersByDateRangeAsync(start, end);
            return Ok(orders);
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingOrders()
        {
            var orders = await _orderService.GetPendingOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("completed")]
        public async Task<IActionResult> GetCompletedOrders()
        {
            var orders = await _orderService.GetCompletedOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("total-sales")]
        public async Task<IActionResult> GetTotalSales([FromQuery] DateTime? start = null, [FromQuery] DateTime? end = null)
        {
            var total = await _orderService.GetTotalSalesAsync(start, end);
            return Ok(new { total });
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetOrdersCount([FromQuery] DateTime? start = null, [FromQuery] DateTime? end = null)
        {
            var count = await _orderService.GetOrdersCountAsync(start, end);
            return Ok(new { count });
        }

        [HttpPut("{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] string status)
        {
            var result = await _orderService.UpdateOrderStatusAsync(orderId, status);
            if (!result) return NotFound(new { message = "Order not found or update failed." });
            return Ok(new { message = "Order status updated." });
        }

        [HttpPut("{orderId}/shipping")]
        public async Task<IActionResult> UpdateShippingInfo(int orderId, [FromBody] ShippingInfoDto dto)
        {
            var result = await _orderService.UpdateShippingInfoAsync(orderId, dto);
            if (!result) return NotFound(new { message = "Shipping update failed." });
            return Ok(new { message = "Shipping information updated." });
        }

        [HttpPost("payment-callback")]
        public async Task<IActionResult> HandlePaymentCallback([FromBody] PaymentCallbackDto dto)
        {
            var result = await _orderService.HandlePaymentCallbackAsync(dto);
            if (!result) return BadRequest(new { message = "Callback processing failed." });
            return Ok(new { message = "Payment processed successfully." });
        }

        // GET api/order/payment-return?success=true&... (forwarded from Paymob redirection URL)
        [HttpGet("payment-return")]
        public IActionResult PaymentReturn()
        {
            var baseUrl = _configuration["FrontendBaseUrl"] ?? "http://localhost:4200";
            var query = HttpContext.Request.QueryString.Value ?? string.Empty;
            var redirect = $"{baseUrl}/store/payment-result{query}";
            return Redirect(redirect);
        }
    }
}
