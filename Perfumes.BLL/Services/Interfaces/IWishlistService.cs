using Perfumes.BLL.DTOs;
using Perfumes.BLL.DTOs.Dashboard;

namespace Perfumes.BLL.Services.Interfaces
{
    public interface IWishlistService
    {
        Task<List<WishlistItemDto>> GetUserItemsAsync(int userId);
        Task AddAsync(int userId, int productId);
        Task RemoveAsync(int userId, int productId);
        Task<bool> ExistsAsync(int userId, int productId);
        Task<int> CountAsync(int userId);
    }
}


