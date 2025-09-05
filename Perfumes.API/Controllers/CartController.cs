using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perfumes.BLL.DTOs.Cart;
using Perfumes.BLL.Services.Interfaces;

namespace Perfumes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCartItems()
        {
            var userId = GetCurrentUserId();
            var result = await _cartService.GetCartItemsAsync(userId, null);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto dto)
        {
            dto.UserId = GetCurrentUserId();
            dto.VisitorId = null;
            await _cartService.AddItemAsync(dto);
            return Ok(new { message = "Item added to cart." });
        }

        [HttpPut("{cartId}/quantity")]
        public async Task<IActionResult> UpdateQuantity(int cartId, [FromQuery] int quantity)
        {
            await _cartService.UpdateQuantityAsync(cartId, quantity);
            return Ok(new { message = "Quantity updated." });
        }

        [HttpPut("{cartId}/increase")]
        public async Task<IActionResult> IncreaseQuantity(int cartId)
        {
            await _cartService.IncreaseQuantityAsync(cartId);
            return Ok(new { message = "Quantity increased." });
        }

        [HttpPut("{cartId}/decrease")]
        public async Task<IActionResult> DecreaseQuantity(int cartId)
        {
            await _cartService.DecreaseQuantityAsync(cartId);
            return Ok(new { message = "Quantity decreased." });
        }

        [HttpDelete("{cartId}")]
        public async Task<IActionResult> RemoveItem(int cartId)
        {
            await _cartService.RemoveItemAsync(cartId);
            return Ok(new { message = "Item removed from cart." });
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = GetCurrentUserId();
            await _cartService.ClearCartAsync(userId, null);
            return Ok(new { message = "Cart cleared." });
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetCartCount()
        {
            var userId = GetCurrentUserId();
            var count = await _cartService.GetCartCountAsync(userId, null);
            return Ok(new { count });
        }

        [HttpGet("exists")]
        public async Task<IActionResult> ProductExists([FromQuery] int productId)
        {
            var userId = GetCurrentUserId();
            var exists = await _cartService.ExistsAsync(userId, null, productId);
            return Ok(new { exists });
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
