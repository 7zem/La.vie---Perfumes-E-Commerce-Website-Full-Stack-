using Perfumes.BLL.DTOs.Cart;

namespace Perfumes.BLL.Services.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<CartItemDto>> GetCartItemsAsync(int? userId, string? visitorId);
        Task AddItemAsync(AddToCartDto dto);
        Task UpdateQuantityAsync(int cartId, int quantity);
        Task IncreaseQuantityAsync(int cartId);
        Task DecreaseQuantityAsync(int cartId);
        Task RemoveItemAsync(int cartId);
        Task ClearCartAsync(int? userId, string? visitorId);
        Task<int> GetCartCountAsync(int? userId, string? visitorId);
        Task<bool> ExistsAsync(int? userId, string? visitorId, int productId);
    }
}
