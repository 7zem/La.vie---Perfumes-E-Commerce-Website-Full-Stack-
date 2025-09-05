using System.Linq.Expressions;
using Perfumes.BLL.DTOs.Product;

namespace Perfumes.BLL.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<ProductDto> CreateAsync(CreateProductDto product);
        Task UpdateAsync(int id, UpdateProductDto product);
        Task DeleteAsync(int id);
        Task<IEnumerable<ProductDto>> AddRangeAsync(IEnumerable<CreateProductDto> products);
        Task DeleteRangeAsync(IEnumerable<int> productIds);
        Task<IEnumerable<ProductDto>> FindAsync(Expression<Func<ProductDto, bool>> predicate);
        Task<ProductDto?> FirstOrDefaultAsync(Expression<Func<ProductDto, bool>> predicate);
        Task<bool> ExistsAsync(int id);
        Task<int> CountAsync();
        Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<ProductDto>> GetProductsByBrandAsync(int brandId);
        Task<IEnumerable<ProductDto>> GetProductsByGenderAsync(string gender);
        Task<IEnumerable<ProductDto>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<IEnumerable<ProductDto>> GetActiveProductsAsync();
        Task<ProductDto?> GetProductWithDetailsAsync(int productId);
        Task<IEnumerable<ProductWithReviewsDto>> GetProductsWithReviewsAsync(int take = 10);
        Task<IEnumerable<ProductDto>> GetFeaturedProductsAsync();
        Task<IEnumerable<ProductDto>> GetNewArrivalsAsync(int take = 10);
        Task<IEnumerable<ProductDto>> GetBestSellersAsync(int take = 10);
        Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm);
    }
}
