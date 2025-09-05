using Microsoft.EntityFrameworkCore;
using Perfumes.DAL.Entities;

namespace Perfumes.DAL.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(PerfumesDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
        {
            return await _dbSet
                .Where(c => c.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetParentCategoriesAsync()
        {
            return await _dbSet
                .Where(c => c.IsActive && c.ParentCategoryId == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentId)
        {
            return await _dbSet
                .Where(c => c.IsActive && c.ParentCategoryId == parentId)
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryWithProductsAsync(int categoryId)
        {
            return await _dbSet
                .Include(c => c.Products.Where(p => p.IsActive))
                .ThenInclude(p => p.Brand)
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId && c.IsActive);
        }

        public async Task<IEnumerable<Category>> GetCategoryTreeAsync()
        {
            return await _dbSet
                .Include(c => c.SubCategories.Where(sc => sc.IsActive))
                .Where(c => c.IsActive && c.ParentCategoryId == null)
                .ToListAsync();
        }
    }
} 