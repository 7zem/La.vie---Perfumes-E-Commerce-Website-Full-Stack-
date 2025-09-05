namespace Perfumes.BLL.DTOs.Dashboard
{
    public class AdminDashboardDto
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalProducts { get; set; }
        public decimal AverageOrderValue { get; set; }
        public int PendingOrders { get; set; }
        public int LowStockProducts { get; set; }
        public decimal MonthlyGrowth { get; set; }
        
        // Recent Activity
        public List<RecentOrderDto> RecentOrders { get; set; } = new();
        public List<RecentCustomerDto> RecentCustomers { get; set; } = new();
        public List<TopProductDto> TopProducts { get; set; } = new();
        
        // Charts Data
        public List<ChartDataDto> RevenueChart { get; set; } = new();
        public List<ChartDataDto> OrdersChart { get; set; } = new();
        public List<ChartDataDto> CustomersChart { get; set; } = new();
    }

    public class SalesAnalyticsDto
    {
        public decimal TotalSales { get; set; }
        public decimal MonthlySales { get; set; }
        public decimal WeeklySales { get; set; }
        public decimal DailySales { get; set; }
        public decimal SalesGrowth { get; set; }
        public List<ChartDataDto> SalesTrend { get; set; } = new();
        public List<TopProductDto> BestSellers { get; set; } = new();
        public List<SalesByCategoryDto> SalesByCategory { get; set; } = new();
    }

    public class CustomerAnalyticsDto
    {
        public int TotalCustomers { get; set; }
        public int NewCustomers { get; set; }
        public int ActiveCustomers { get; set; }
        public decimal CustomerRetentionRate { get; set; }
        public decimal AverageCustomerValue { get; set; }
        public List<ChartDataDto> CustomerGrowth { get; set; } = new();
        public List<CustomerSegmentDto> CustomerSegments { get; set; } = new();
    }

    public class ProductAnalyticsDto
    {
        public int TotalProducts { get; set; }
        public int ActiveProducts { get; set; }
        public int LowStockProducts { get; set; }
        public int OutOfStockProducts { get; set; }
        public decimal AverageProductRating { get; set; }
        public List<TopProductDto> TopProducts { get; set; } = new();
        public List<LowStockProductDto> LowStockProductsList { get; set; } = new();
        public List<ChartDataDto> ProductPerformance { get; set; } = new();
    }

    public class OrderAnalyticsDto
    {
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int CompletedOrders { get; set; }
        public int CancelledOrders { get; set; }
        public decimal AverageOrderValue { get; set; }
        public decimal OrderCompletionRate { get; set; }
        public List<ChartDataDto> OrderTrend { get; set; } = new();
        public List<OrderStatusDto> OrdersByStatus { get; set; } = new();
    }

    public class UserDashboardDto
    {
        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
        public int WishlistItems { get; set; }
        public int TotalReviews { get; set; }
        public decimal AverageRating { get; set; }
        public List<RecentOrderDto> RecentOrders { get; set; } = new();
        public List<WishlistItemDto> WishlistItemsList { get; set; } = new();
        public List<UserReviewDto> RecentReviews { get; set; } = new();
    }

    public class ChartDataDto
    {
        public string Label { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public string Color { get; set; } = string.Empty;
    }

    public class RecentOrderDto
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
    }

    public class RecentCustomerDto
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
        public int TotalOrders { get; set; }
    }

    public class TopProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int SalesCount { get; set; }
        public decimal Revenue { get; set; }
        public decimal Rating { get; set; }
    }

    public class SalesByCategoryDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public int OrderCount { get; set; }
        public decimal Percentage { get; set; }
    }

    public class CustomerSegmentDto
    {
        public string Segment { get; set; } = string.Empty;
        public int CustomerCount { get; set; }
        public decimal AverageValue { get; set; }
        public decimal Percentage { get; set; }
    }

    public class LowStockProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CurrentStock { get; set; }
        public int MinStockLevel { get; set; }
    }

    public class OrderStatusDto
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal Percentage { get; set; }
    }

    public class WishlistItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime AddedDate { get; set; }
    }

    public class UserReviewDto
    {
        public int ReviewId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime ReviewDate { get; set; }
    }
} 