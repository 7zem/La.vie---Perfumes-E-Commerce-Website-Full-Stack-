using Microsoft.AspNetCore.Mvc;
using Perfumes.BLL.DTOs.Coupon;
using Perfumes.BLL.Services.Interfaces;

namespace Perfumes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponController : ControllerBase
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCoupon([FromBody] CreateCouponDto dto)
        {
            var result = await _couponService.CreateCouponAsync(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCoupon(int id, [FromBody] UpdateCouponDto dto)
        {
            var result = await _couponService.UpdateCouponAsync(id, dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCoupon(int id)
        {
            var deleted = await _couponService.DeleteCouponAsync(id);
            return deleted ? Ok(new { message = "Coupon deleted." }) : NotFound(new { message = "Coupon not found." });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _couponService.GetAllCouponsAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _couponService.GetCouponByIdAsync(id);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpGet("code/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var result = await _couponService.GetCouponByCodeAsync(code);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var result = await _couponService.GetActiveCouponsAsync();
            return Ok(result);
        }

        [HttpGet("valid")]
        public async Task<IActionResult> GetValid()
        {
            var result = await _couponService.GetValidCouponsAsync();
            return Ok(result);
        }

        [HttpGet("expired")]
        public async Task<IActionResult> GetExpired()
        {
            var result = await _couponService.GetExpiredCouponsAsync();
            return Ok(result);
        }

        [HttpGet("range")]
        public async Task<IActionResult> GetByDateRange([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var result = await _couponService.GetCouponsByDateRangeAsync(start, end);
            return Ok(result);
        }

        [HttpGet("can-use/{code}")]
        public async Task<IActionResult> CanUse(string code)
        {
            var result = await _couponService.CanUseCouponAsync(code);
            return Ok(new { canUse = result });
        }

        [HttpPost("apply")]
        public async Task<IActionResult> ApplyCoupon([FromBody] ApplyCouponDto dto)
        {
            var result = await _couponService.ApplyCouponAsync(dto);
            return Ok(result);
        }
    }
}
