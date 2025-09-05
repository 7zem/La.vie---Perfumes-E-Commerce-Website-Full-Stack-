namespace Perfumes.BLL.DTOs.Dashboard
{
    public class SalesReportDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public decimal AverageOrderValue { get; set; }
        public List<DailySalesDto> DailySales { get; set; } = new();
        public List<SalesByProductDto> SalesByProduct { get; set; } = new();
        public List<SalesByCategoryDto> SalesByCategory { get; set; } = new();
        public List<SalesByCustomerDto> SalesByCustomer { get; set; } = new();
    }

    public class InventoryReportDto
    {
        public int TotalProducts { get; set; }
        public int InStockProducts { get; set; }
        public int LowStockProducts { get; set; }
        public int OutOfStockProducts { get; set; }
        public decimal TotalInventoryValue { get; set; }
        public List<LowStockProductDto> LowStockProductsList { get; set; } = new();
        public List<OutOfStockProductDto> OutOfStockProductsList { get; set; } = new();
        public List<InventoryByCategoryDto> InventoryByCategory { get; set; } = new();
    }

    public class CustomerReportDto
    {
        public int TotalCustomers { get; set; }
        public int NewCustomers { get; set; }
        public int ActiveCustomers { get; set; }
        public int InactiveCustomers { get; set; }
        public decimal AverageCustomerValue { get; set; }
        public List<CustomerSegmentDto> CustomerSegments { get; set; } = new();
        public List<TopCustomerDto> TopCustomers { get; set; } = new();
        public List<CustomerActivityDto> CustomerActivity { get; set; } = new();
    }

    public class DailySalesDto
    {
        public DateTime Date { get; set; }
        public decimal Revenue { get; set; }
        public int OrderCount { get; set; }
        public int CustomerCount { get; set; }
    }

    public class SalesByProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public int QuantitySold { get; set; }
        public decimal Revenue { get; set; }
        public decimal AverageRating { get; set; }
    }

    public class SalesByCustomerDto
    {
        public int UserId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int OrderCount { get; set; }
        public decimal TotalSpent { get; set; }
        public DateTime LastOrderDate { get; set; }
    }

    public class OutOfStockProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime LastRestocked { get; set; }
    }

    public class InventoryByCategoryDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public int ProductCount { get; set; }
        public int InStockCount { get; set; }
        public int LowStockCount { get; set; }
        public int OutOfStockCount { get; set; }
        public decimal TotalValue { get; set; }
    }

    public class TopCustomerDto
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int OrderCount { get; set; }
        public decimal TotalSpent { get; set; }
        public decimal AverageOrderValue { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastOrderDate { get; set; }
    }

    public class CustomerActivityDto
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime LastLoginDate { get; set; }
        public DateTime LastOrderDate { get; set; }
        public int DaysSinceLastOrder { get; set; }
        public string ActivityStatus { get; set; } = string.Empty;
    }

    public class UserOrdersDto
    {
        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
        public decimal AverageOrderValue { get; set; }
        public List<RecentOrderDto> Orders { get; set; } = new();
    }

    public class UserWishlistDto
    {
        public int TotalItems { get; set; }
        public List<WishlistItemDto> Items { get; set; } = new();
    }

    public class UserReviewsDto
    {
        public int TotalReviews { get; set; }
        public decimal AverageRating { get; set; }
        public List<UserReviewDto> Reviews { get; set; } = new();
    }
} 