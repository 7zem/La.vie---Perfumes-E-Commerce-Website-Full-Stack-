using Microsoft.EntityFrameworkCore;
using Perfumes.DAL.Entities;

namespace Perfumes.DAL.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<Product> IncludeProductDetails(this IQueryable<Product> query)
        {
            return query
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Reviews.Where(r => r.IsApproved))
                .Include(p => p.Inventory);
        }

        public static IQueryable<Order> IncludeOrderDetails(this IQueryable<Order> query)
        {
            return query
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ThenInclude(p => p.Brand)
                .Include(o => o.ShippingInfo)
                .Include(o => o.Payment)
                .Include(o => o.Coupon);
        }

        public static IQueryable<User> IncludeUserDetails(this IQueryable<User> query)
        {
            return query
                .Include(u => u.CartItems)
                .ThenInclude(c => c.Product)
                .Include(u => u.WishlistItems)
                .ThenInclude(w => w.Product)
                .Include(u => u.Orders)
                .Include(u => u.Reviews);
        }

        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int page, int pageSize)
        {
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public static async Task<(IEnumerable<T> Items, int TotalCount)> PaginateWithCountAsync<T>(
            this IQueryable<T> query, int page, int pageSize)
        {
            var totalCount = await query.CountAsync();
            var items = await query.Paginate(page, pageSize).ToListAsync();
            
            return (items, totalCount);
        }
    }
} 