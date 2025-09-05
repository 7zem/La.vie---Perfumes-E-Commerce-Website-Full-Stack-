using Microsoft.EntityFrameworkCore;
using Perfumes.DAL.Entities;

namespace Perfumes.DAL.Repositories
{
    public class BrandRepository : Repository<Brand>, IBrandRepository
    {
        public BrandRepository(PerfumesDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Brand>> GetActiveBrandsAsync()
        {
            return await _dbSet
                .Where(b => b.IsActive)
                .ToListAsync();
        }

        public async Task<Brand?> GetBrandWithProductsAsync(int brandId)
        {
            return await _dbSet
                .Include(b => b.Products.Where(p => p.IsActive))
                .ThenInclude(p => p.Category)
                .FirstOrDefaultAsync(b => b.BrandId == brandId && b.IsActive);
        }

        public async Task<IEnumerable<Brand>> GetBrandsWithProductCountAsync()
        {
            return await _dbSet
                .Include(b => b.Products.Where(p => p.IsActive))
                .Where(b => b.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Brand>> SearchBrandsAsync(string searchTerm)
        {
            return await _dbSet
                .Where(b => b.IsActive && 
                           (b.Name.Contains(searchTerm) || 
                            b.Description!.Contains(searchTerm)))
                .ToListAsync();
        }
    }
} 