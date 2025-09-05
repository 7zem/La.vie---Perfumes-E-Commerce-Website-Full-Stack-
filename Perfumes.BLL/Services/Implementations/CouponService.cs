using AutoMapper;
using Perfumes.BLL.DTOs.Coupon;
using Perfumes.BLL.Services.Interfaces;
using Perfumes.DAL.UnitOfWork;

namespace Perfumes.BLL.Services.Implementations
{
    public class CouponService : ICouponService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CouponService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CouponDto> CreateCouponAsync(CreateCouponDto dto)
        {
            var coupon = _mapper.Map<Perfumes.DAL.Entities.Coupon>(dto);
            var created = await _unitOfWork.Coupons.AddAsync(coupon);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CouponDto>(created);
        }

        public async Task<CouponDto> UpdateCouponAsync(int id, UpdateCouponDto dto)
        {
            var coupon = await _unitOfWork.Coupons.GetByIdAsync(id);
            if (coupon == null) throw new Exception("Coupon not found");

            _mapper.Map(dto, coupon);
            await _unitOfWork.Coupons.UpdateAsync(coupon);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CouponDto>(coupon);
        }

        public async Task<bool> DeleteCouponAsync(int id)
        {
            var coupon = await _unitOfWork.Coupons.GetByIdAsync(id);
            if (coupon == null) return false;

            await _unitOfWork.Coupons.DeleteAsync(coupon);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<CouponDto?> GetCouponByIdAsync(int id)
        {
            var coupon = await _unitOfWork.Coupons.GetByIdAsync(id);
            return coupon == null ? null : _mapper.Map<CouponDto>(coupon);
        }

        public async Task<CouponDto?> GetCouponByCodeAsync(string code)
        {
            var coupon = await _unitOfWork.Coupons.GetCouponByCodeAsync(code);
            return coupon == null ? null : _mapper.Map<CouponDto>(coupon);
        }

        public async Task<IEnumerable<CouponDto>> GetAllCouponsAsync()
        {
            var coupons = await _unitOfWork.Coupons.GetAllAsync();
            return _mapper.Map<IEnumerable<CouponDto>>(coupons);
        }

        public async Task<IEnumerable<CouponDto>> GetActiveCouponsAsync()
        {
            var coupons = await _unitOfWork.Coupons.GetActiveCouponsAsync();
            return _mapper.Map<IEnumerable<CouponDto>>(coupons);
        }

        public async Task<IEnumerable<CouponDto>> GetValidCouponsAsync()
        {
            var coupons = await _unitOfWork.Coupons.GetValidCouponsAsync();
            return _mapper.Map<IEnumerable<CouponDto>>(coupons);
        }

        public async Task<IEnumerable<CouponDto>> GetExpiredCouponsAsync()
        {
            var coupons = await _unitOfWork.Coupons.GetExpiredCouponsAsync();
            return _mapper.Map<IEnumerable<CouponDto>>(coupons);
        }

        public async Task<IEnumerable<CouponDto>> GetCouponsByDateRangeAsync(DateTime start, DateTime end)
        {
            var coupons = await _unitOfWork.Coupons.GetCouponsByDateRangeAsync(start, end);
            return _mapper.Map<IEnumerable<CouponDto>>(coupons);
        }

        public async Task<bool> CanUseCouponAsync(string code)
        {
            return await _unitOfWork.Coupons.CanUseCouponAsync(code);
        }

        public async Task<CouponResultDto> ApplyCouponAsync(ApplyCouponDto dto)
        {
            var coupon = await _unitOfWork.Coupons.GetCouponByCodeAsync(dto.Code);
            var result = new CouponResultDto();

            if (coupon == null)
            {
                result.IsValid = false;
                result.Message = "Coupon not found";
                return result;
            }

            if (!coupon.IsActive)
            {
                result.IsValid = false;
                result.Message = "Coupon is not active";
                return result;
            }

            if (coupon.ValidUntil.HasValue && coupon.ValidUntil < DateTime.UtcNow)
            {
                result.IsValid = false;
                result.Message = "Coupon has expired";
                return result;
            }

            if (coupon.MinOrderAmount.HasValue && dto.OrderAmount < coupon.MinOrderAmount.Value)
            {
                result.IsValid = false;
                result.Message = $"Minimum order amount is {coupon.MinOrderAmount.Value}";
                return result;
            }

            if (coupon.MaxUses.HasValue && coupon.UsedCount >= coupon.MaxUses.Value)
            {
                result.IsValid = false;
                result.Message = "Coupon usage limit exceeded";
                return result;
            }

            result.IsValid = true;
            result.CouponId = coupon.CouponId;
            result.Code = coupon.Code;
            result.IsPercentage = coupon.DiscountPercent.HasValue;
            result.DiscountValue = coupon.DiscountAmount ?? coupon.DiscountPercent ?? 0;
            result.Message = "Coupon applied successfully";

            return result;
        }
    }
}
