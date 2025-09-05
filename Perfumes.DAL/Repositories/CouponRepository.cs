using Microsoft.EntityFrameworkCore;
using Perfumes.DAL.Entities;

namespace Perfumes.DAL.Repositories
{
    public class CouponRepository : Repository<Coupon>, ICouponRepository
    {
        public CouponRepository(PerfumesDbContext context) : base(context)
        {
        }

        public async Task<Coupon?> GetCouponByCodeAsync(string code)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.Code == code && c.IsActive);
        }

        public async Task<IEnumerable<Coupon>> GetActiveCouponsAsync()
        {
            return await _dbSet
                .Where(c => c.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Coupon>> GetValidCouponsAsync()
        {
            var now = DateTime.UtcNow;
            return await _dbSet
                .Where(c => c.IsActive && 
                           (!c.ValidFrom.HasValue || c.ValidFrom <= now) &&
                           (!c.ValidUntil.HasValue || c.ValidUntil >= now))
                .ToListAsync();
        }

        public async Task<bool> IsCouponValidAsync(string code, decimal orderAmount)
        {
            var coupon = await GetCouponByCodeAsync(code);
            if (coupon == null) return false;

            var now = DateTime.UtcNow;
            
            // Check if coupon is active and within valid date range
            if (!coupon.IsActive || 
                (coupon.ValidFrom.HasValue && coupon.ValidFrom > now) ||
                (coupon.ValidUntil.HasValue && coupon.ValidUntil < now))
                return false;

            // Check minimum order amount
            if (coupon.MinOrderAmount.HasValue && orderAmount < coupon.MinOrderAmount.Value)
                return false;

            // Check usage limit
            if (coupon.MaxUses.HasValue && coupon.UsedCount >= coupon.MaxUses.Value)
                return false;

            return true;
        }

        public async Task<bool> CanUseCouponAsync(string code)
        {
            var coupon = await GetCouponByCodeAsync(code);
            if (coupon == null) return false;

            var now = DateTime.UtcNow;
            
            return coupon.IsActive && 
                   (!coupon.ValidFrom.HasValue || coupon.ValidFrom <= now) &&
                   (!coupon.ValidUntil.HasValue || coupon.ValidUntil >= now) &&
                   (!coupon.MaxUses.HasValue || coupon.UsedCount < coupon.MaxUses.Value);
        }

        public async Task<IEnumerable<Coupon>> GetExpiredCouponsAsync()
        {
            var now = DateTime.UtcNow;
            return await _dbSet
                .Where(c => c.IsActive && 
                           c.ValidUntil.HasValue && 
                           c.ValidUntil < now)
                .ToListAsync();
        }

        public async Task<IEnumerable<Coupon>> GetCouponsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(c => c.CreatedAt >= startDate && c.CreatedAt <= endDate)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }
    }
} 