using Perfumes.DAL.Entities;

namespace Perfumes.DAL.Repositories
{
    public interface IBrandRepository : IRepository<Brand>
    {
        Task<IEnumerable<Brand>> GetActiveBrandsAsync();
        Task<Brand?> GetBrandWithProductsAsync(int brandId);
        Task<IEnumerable<Brand>> GetBrandsWithProductCountAsync();
        Task<IEnumerable<Brand>> SearchBrandsAsync(string searchTerm);
    }
} 