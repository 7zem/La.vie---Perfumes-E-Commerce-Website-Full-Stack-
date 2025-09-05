using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perfumes.BLL.Services.Interfaces;

namespace Perfumes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WishlistController(IWishlistService wishlistService) : ControllerBase
    {
        private readonly IWishlistService _wishlistService = wishlistService;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = GetCurrentUserId();
            var items = await _wishlistService.GetUserItemsAsync(userId);
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromQuery] int productId)
        {
            var userId = GetCurrentUserId();
            await _wishlistService.AddAsync(userId, productId);
            return Ok(new { message = "Added to wishlist" });
        }

        [HttpDelete]
        public async Task<IActionResult> Remove([FromQuery] int productId)
        {
            var userId = GetCurrentUserId();
            await _wishlistService.RemoveAsync(userId, productId);
            return Ok(new { message = "Removed from wishlist" });
        }

        [HttpGet("exists")]
        public async Task<IActionResult> Exists([FromQuery] int productId)
        {
            var userId = GetCurrentUserId();
            var exists = await _wishlistService.ExistsAsync(userId, productId);
            return Ok(new { exists });
        }

        [HttpGet("count")]
        public async Task<IActionResult> Count()
        {
            var userId = GetCurrentUserId();
            var count = await _wishlistService.CountAsync(userId);
            return Ok(new { count });
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value
                ?? User.FindFirst("nameid")?.Value
                ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            if (userIdClaim != null && int.TryParse(userIdClaim, out int userId))
                return userId;
            throw new UnauthorizedAccessException("User not authenticated");
        }
    }
}


