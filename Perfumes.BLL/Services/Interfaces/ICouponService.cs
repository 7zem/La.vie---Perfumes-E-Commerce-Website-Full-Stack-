using Perfumes.BLL.DTOs.Coupon;

namespace Perfumes.BLL.Services.Interfaces
{
    public interface ICouponService
    {
        Task<CouponDto> CreateCouponAsync(CreateCouponDto dto);
        Task<CouponDto> UpdateCouponAsync(int id, UpdateCouponDto dto);
        Task<bool> DeleteCouponAsync(int id);

        Task<CouponDto?> GetCouponByIdAsync(int id);
        Task<CouponDto?> GetCouponByCodeAsync(string code);
        Task<IEnumerable<CouponDto>> GetAllCouponsAsync();

        Task<IEnumerable<CouponDto>> GetActiveCouponsAsync();
        Task<IEnumerable<CouponDto>> GetValidCouponsAsync();
        Task<IEnumerable<CouponDto>> GetExpiredCouponsAsync();
        Task<IEnumerable<CouponDto>> GetCouponsByDateRangeAsync(DateTime start, DateTime end);

        Task<bool> CanUseCouponAsync(string code);
        Task<CouponResultDto> ApplyCouponAsync(ApplyCouponDto dto);
    }
}
