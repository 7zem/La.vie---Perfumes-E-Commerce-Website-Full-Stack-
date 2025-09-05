using Perfumes.DAL.Entities;

namespace Perfumes.DAL.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();
        Task<IEnumerable<Category>> GetParentCategoriesAsync();
        Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentId);
        Task<Category?> GetCategoryWithProductsAsync(int categoryId);
        Task<IEnumerable<Category>> GetCategoryTreeAsync();
    }
} 