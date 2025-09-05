using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Perfumes.BLL.DTOs.Category;

namespace Perfumes.BLL.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(int id);
        Task<CategoryDto> CreateAsync(CreateCategoryDto category);
        Task UpdateAsync(int id, UpdateCategoryDto category);
        Task DeleteAsync(int id);
        Task<IEnumerable<CategoryDto>> GetActiveAsync();
        Task<IEnumerable<CategoryDto>> GetParentsAsync();
        Task<IEnumerable<CategoryDto>> GetSubCategoriesAsync(int parentId);
        Task<CategoryWithProductsDto?> GetWithProductsAsync(int id);
        Task<IEnumerable<CategoryTreeDto>> GetTreeAsync();
    }
}

