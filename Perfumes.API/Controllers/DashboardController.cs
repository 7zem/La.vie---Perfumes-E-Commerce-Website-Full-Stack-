using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perfumes.BLL.DTOs.Dashboard;
using Perfumes.BLL.Services.Interfaces;

namespace Perfumes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // Admin Dashboard Endpoints
        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAdminDashboard([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var dashboard = await _dashboardService.GetAdminDashboardAsync(startDate, endDate);
                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("admin/sales-analytics")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSalesAnalytics([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var analytics = await _dashboardService.GetSalesAnalyticsAsync(startDate, endDate);
                return Ok(analytics);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("admin/customer-analytics")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCustomerAnalytics([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var analytics = await _dashboardService.GetCustomerAnalyticsAsync(startDate, endDate);
                return Ok(analytics);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("admin/product-analytics")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetProductAnalytics([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var analytics = await _dashboardService.GetProductAnalyticsAsync(startDate, endDate);
                return Ok(analytics);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("admin/order-analytics")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOrderAnalytics([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var analytics = await _dashboardService.GetOrderAnalyticsAsync(startDate, endDate);
                return Ok(analytics);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Quick Statistics for Admin
        [HttpGet("admin/quick-stats")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetQuickStats()
        {
            try
            {
                var dashboard = await _dashboardService.GetAdminDashboardAsync();
                var quickStats = new
                {
                    TotalRevenue = dashboard.TotalRevenue,
                    TotalOrders = dashboard.TotalOrders,
                    TotalCustomers = dashboard.TotalCustomers,
                    TotalProducts = dashboard.TotalProducts,
                    PendingOrders = dashboard.PendingOrders,
                    LowStockProducts = dashboard.LowStockProducts,
                    AverageOrderValue = dashboard.AverageOrderValue
                };
                return Ok(quickStats);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // User Dashboard Endpoints
        [HttpGet("user")]
        public async Task<IActionResult> GetUserDashboard()
        {
            try
            {
                var userId = GetCurrentUserId();
                var dashboard = await _dashboardService.GetUserDashboardAsync(userId);
                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("user/orders")]
        public async Task<IActionResult> GetUserOrders()
        {
            try
            {
                var userId = GetCurrentUserId();
                var orders = await _dashboardService.GetUserOrdersAsync(userId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("user/wishlist")]
        public async Task<IActionResult> GetUserWishlist()
        {
            try
            {
                var userId = GetCurrentUserId();
                var wishlist = await _dashboardService.GetUserWishlistAsync(userId);
                return Ok(wishlist);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("user/reviews")]
        public async Task<IActionResult> GetUserReviews()
        {
            try
            {
                var userId = GetCurrentUserId();
                var reviews = await _dashboardService.GetUserReviewsAsync(userId);
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Quick Statistics for User
        [HttpGet("user/quick-stats")]
        public async Task<IActionResult> GetUserQuickStats()
        {
            try
            {
                var userId = GetCurrentUserId();
                var dashboard = await _dashboardService.GetUserDashboardAsync(userId);
                var quickStats = new
                {
                    TotalOrders = dashboard.TotalOrders,
                    TotalSpent = dashboard.TotalSpent,
                    WishlistItems = dashboard.WishlistItems,
                    TotalReviews = dashboard.TotalReviews,
                    AverageRating = dashboard.AverageRating
                };
                return Ok(quickStats);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Reports Endpoints
        [HttpGet("admin/reports/sales")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSalesReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var report = await _dashboardService.GetSalesReportAsync(startDate, endDate);
                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("admin/reports/inventory")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetInventoryReport()
        {
            try
            {
                var report = await _dashboardService.GetInventoryReportAsync();
                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("admin/reports/customers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCustomerReport([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var report = await _dashboardService.GetCustomerReportAsync(startDate, endDate);
                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Chart Data Endpoints
        [HttpGet("admin/charts/revenue")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRevenueChart([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var analytics = await _dashboardService.GetSalesAnalyticsAsync(startDate, endDate);
                return Ok(analytics.SalesTrend);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("admin/charts/orders")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOrdersChart([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var analytics = await _dashboardService.GetOrderAnalyticsAsync(startDate, endDate);
                return Ok(analytics.OrderTrend);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("admin/charts/customers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCustomersChart([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var analytics = await _dashboardService.GetCustomerAnalyticsAsync(startDate, endDate);
                return Ok(analytics.CustomerGrowth);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("admin/charts/products")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetProductsChart([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var analytics = await _dashboardService.GetProductAnalyticsAsync(startDate, endDate);
                return Ok(analytics.ProductPerformance);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Recent Activity Endpoints
        [HttpGet("admin/recent-orders")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRecentOrders()
        {
            try
            {
                var dashboard = await _dashboardService.GetAdminDashboardAsync();
                return Ok(dashboard.RecentOrders);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("admin/recent-customers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRecentCustomers()
        {
            try
            {
                var dashboard = await _dashboardService.GetAdminDashboardAsync();
                return Ok(dashboard.RecentCustomers);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("admin/top-products")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTopProducts()
        {
            try
            {
                var dashboard = await _dashboardService.GetAdminDashboardAsync();
                return Ok(dashboard.TopProducts);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // User Recent Activity
        [HttpGet("user/recent-orders")]
        public async Task<IActionResult> GetUserRecentOrders()
        {
            try
            {
                var userId = GetCurrentUserId();
                var dashboard = await _dashboardService.GetUserDashboardAsync(userId);
                return Ok(dashboard.RecentOrders);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("user/recent-reviews")]
        public async Task<IActionResult> GetUserRecentReviews()
        {
            try
            {
                var userId = GetCurrentUserId();
                var dashboard = await _dashboardService.GetUserDashboardAsync(userId);
                return Ok(dashboard.RecentReviews);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("user/wishlist-items")]
        public async Task<IActionResult> GetUserWishlistItems()
        {
            try
            {
                var userId = GetCurrentUserId();
                var dashboard = await _dashboardService.GetUserDashboardAsync(userId);
                return Ok(dashboard.WishlistItems);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                return userId;
            throw new UnauthorizedAccessException("User not authenticated");
        }
    }
} 