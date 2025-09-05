using Perfumes.BLL.DTOs.Dashboard;

namespace Perfumes.BLL.Services.Interfaces
{
    public interface IDashboardService
    {
        // Admin Dashboard
        Task<AdminDashboardDto> GetAdminDashboardAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<SalesAnalyticsDto> GetSalesAnalyticsAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<CustomerAnalyticsDto> GetCustomerAnalyticsAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<ProductAnalyticsDto> GetProductAnalyticsAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<OrderAnalyticsDto> GetOrderAnalyticsAsync(DateTime? startDate = null, DateTime? endDate = null);
        
        // User Dashboard
        Task<UserDashboardDto> GetUserDashboardAsync(int userId);
        Task<UserOrdersDto> GetUserOrdersAsync(int userId);
        Task<UserWishlistDto> GetUserWishlistAsync(int userId);
        Task<UserReviewsDto> GetUserReviewsAsync(int userId);
        
        // Reports
        Task<SalesReportDto> GetSalesReportAsync(DateTime startDate, DateTime endDate);
        Task<InventoryReportDto> GetInventoryReportAsync();
        Task<CustomerReportDto> GetCustomerReportAsync(DateTime? startDate = null, DateTime? endDate = null);
    }
} 