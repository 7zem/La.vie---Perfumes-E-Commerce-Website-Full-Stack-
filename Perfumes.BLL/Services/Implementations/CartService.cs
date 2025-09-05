using AutoMapper;
using Perfumes.BLL.DTOs.Cart;
using Perfumes.BLL.Services.Interfaces;
using Perfumes.DAL.UnitOfWork;

namespace Perfumes.BLL.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CartItemDto>> GetCartItemsAsync(int? userId, string? visitorId)
        {
            var items = await _unitOfWork.Cart.FindAsync(c => userId != null && c.UserId == userId);

            // Ensure product info (name, image, price) is populated even if navigation is not eagerly loaded
            var result = new List<CartItemDto>();
            foreach (var cartItem in items)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(cartItem.ProductId);
                result.Add(new CartItemDto
                {
                    CartId = cartItem.CartId,
                    ProductId = cartItem.ProductId,
                    ProductName = product?.Name ?? string.Empty,
                    ProductImageUrl = product?.ImageUrl ?? string.Empty,
                    Price = product?.Price ?? 0,
                    Quantity = cartItem.Quantity,
                    AddedAt = cartItem.AddedAt
                });
            }

            return result;
        }

        public async Task AddItemAsync(AddToCartDto dto)
        {
            var existing = await _unitOfWork.Cart.FirstOrDefaultAsync(c =>
                c.ProductId == dto.ProductId && dto.UserId != null && c.UserId == dto.UserId);

            if (existing != null)
            {
                existing.Quantity += dto.Quantity;
                await _unitOfWork.Cart.UpdateAsync(existing);
            }
            else
            {
                var cartItem = new Perfumes.DAL.Entities.Cart
                {
                    UserId = dto.UserId,
                    VisitorId = null,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                };

                await _unitOfWork.Cart.AddAsync(cartItem);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateQuantityAsync(int cartId, int quantity)
        {
            var item = await _unitOfWork.Cart.GetByIdAsync(cartId);
            if (item == null) throw new Exception("Item not found");

            item.Quantity = quantity > 0 ? quantity : 1;
            await _unitOfWork.Cart.UpdateAsync(item);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task IncreaseQuantityAsync(int cartId)
        {
            var item = await _unitOfWork.Cart.GetByIdAsync(cartId);
            if (item == null) throw new Exception("Item not found");

            item.Quantity += 1;
            await _unitOfWork.Cart.UpdateAsync(item);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DecreaseQuantityAsync(int cartId)
        {
            var item = await _unitOfWork.Cart.GetByIdAsync(cartId);
            if (item == null) throw new Exception("Item not found");

            item.Quantity -= 1;

            if (item.Quantity <= 0)
                await _unitOfWork.Cart.DeleteAsync(item);
            else
                await _unitOfWork.Cart.UpdateAsync(item);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveItemAsync(int cartId)
        {
            var item = await _unitOfWork.Cart.GetByIdAsync(cartId);
            if (item == null) throw new Exception("Item not found");

            await _unitOfWork.Cart.DeleteAsync(item);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ClearCartAsync(int? userId, string? visitorId)
        {
            var items = await _unitOfWork.Cart.FindAsync(c => userId != null && c.UserId == userId);

            await _unitOfWork.Cart.DeleteRangeAsync(items);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> GetCartCountAsync(int? userId, string? visitorId)
        {
            return await _unitOfWork.Cart.CountAsync(c => userId != null && c.UserId == userId);
        }

        public async Task<bool> ExistsAsync(int? userId, string? visitorId, int productId)
        {
            return await _unitOfWork.Cart.ExistsAsync(c =>
                c.ProductId == productId && (userId != null && c.UserId == userId));
        }
    }
}
