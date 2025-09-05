using Microsoft.EntityFrameworkCore;
using Perfumes.DAL.Entities;

namespace Perfumes.DAL.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(PerfumesDbContext context) : base(context)
        {
        }

        // Ensure Brand and Category are loaded when fetching all products
        public override async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _dbSet
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId && p.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByBrandAsync(int brandId)
        {
            return await _dbSet
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Where(p => p.BrandId == brandId && p.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
        {
            return await _dbSet
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Where(p => p.IsActive && 
                           (p.Name.Contains(searchTerm) || 
                            p.Description!.Contains(searchTerm) ||
                            p.Brand!.Name.Contains(searchTerm)))
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByGenderAsync(string gender)
        {
            return await _dbSet
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Where(p => p.IsActive && p.Gender == gender)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _dbSet
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Where(p => p.IsActive && p.Price >= minPrice && p.Price <= maxPrice)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetActiveProductsAsync()
        {
            return await _dbSet
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Where(p => p.IsActive)
                .ToListAsync();
        }

        public async Task<Product?> GetProductWithDetailsAsync(int productId)
        {
            return await _dbSet
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Reviews.Where(r => r.IsApproved))
                .Include(p => p.Inventory)
                .FirstOrDefaultAsync(p => p.ProductId == productId && p.IsActive);
        }

        public async Task<IEnumerable<Product>> GetProductsWithReviewsAsync(int take = 10)
        {
            return await _dbSet
                .Include(p => p.Brand)
                .Include(p => p.Reviews.Where(r => r.IsApproved))
                .Where(p => p.IsActive && p.Reviews.Any())
                .Take(take)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetFeaturedProductsAsync()
        {
            // يمكن تحسين هذا لاستخدام خوارزمية أكثر تعقيداً
            return await _dbSet
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.Reviews.Count)
                .Take(8)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetNewArrivalsAsync(int take = 10)
        {
            return await _dbSet
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.CreatedAt)
                .Take(take)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetBestSellersAsync(int take = 10)
        {
            // يمكن تحسين هذا لاستخدام بيانات المبيعات الفعلية
            return await _dbSet
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.Reviews.Average(r => r.Rating))
                .Take(take)
                .ToListAsync();
        }
    }
} 