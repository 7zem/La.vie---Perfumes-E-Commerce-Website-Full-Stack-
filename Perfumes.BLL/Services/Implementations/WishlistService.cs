using Perfumes.BLL.DTOs.Dashboard;
using Perfumes.BLL.Services.Interfaces;
using Perfumes.DAL.Entities;
using Perfumes.DAL.UnitOfWork;

namespace Perfumes.BLL.Services.Implementations
{
    public class WishlistService(IUnitOfWork unitOfWork) : IWishlistService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<List<WishlistItemDto>> GetUserItemsAsync(int userId)
        {
            var items = await _unitOfWork.Wishlist.FindAsync(w => w.UserId == userId);
            var result = new List<WishlistItemDto>();
            foreach (var w in items)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(w.ProductId);
                result.Add(new WishlistItemDto
                {
                    ProductId = w.ProductId,
                    ProductName = product?.Name ?? string.Empty,
                    Price = product?.Price ?? 0,
                    ImageUrl = product?.ImageUrl ?? string.Empty,
                    AddedDate = w.AddedAt
                });
            }
            return result;
        }

        public async Task AddAsync(int userId, int productId)
        {
            var exists = await _unitOfWork.Wishlist.ExistsAsync(w => w.UserId == userId && w.ProductId == productId);
            if (exists) return;
            await _unitOfWork.Wishlist.AddAsync(new Wishlist
            {
                UserId = userId,
                ProductId = productId
            });
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveAsync(int userId, int productId)
        {
            var item = await _unitOfWork.Wishlist.FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);
            if (item == null) return;
            await _unitOfWork.Wishlist.DeleteAsync(item);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int userId, int productId)
        {
            return await _unitOfWork.Wishlist.ExistsAsync(w => w.UserId == userId && w.ProductId == productId);
        }

        public async Task<int> CountAsync(int userId)
        {
            return await _unitOfWork.Wishlist.CountAsync(w => w.UserId == userId);
        }
    }
}


