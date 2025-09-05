using Perfumes.DAL.Entities;

namespace Perfumes.DAL.Repositories
{
    public interface ICouponRepository : IRepository<Coupon>
    {
        Task<Coupon?> GetCouponByCodeAsync(string code);
        Task<IEnumerable<Coupon>> GetActiveCouponsAsync();
        Task<IEnumerable<Coupon>> GetValidCouponsAsync();
        Task<bool> IsCouponValidAsync(string code, decimal orderAmount);
        Task<bool> CanUseCouponAsync(string code);
        Task<IEnumerable<Coupon>> GetExpiredCouponsAsync();
        Task<IEnumerable<Coupon>> GetCouponsByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
} 