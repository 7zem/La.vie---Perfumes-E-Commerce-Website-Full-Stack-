using Perfumes.BLL.DTOs.Brand;

namespace Perfumes.BLL.Services.Interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandDto>> GetAllAsync();
        Task<IEnumerable<BrandDto>> GetActiveAsync();
        Task<BrandDto?> GetByIdAsync(int id);
        Task<BrandWithProductsDto?> GetWithProductsAsync(int brandId);
        Task<IEnumerable<BrandWithProductCountDto>> GetWithProductCountAsync();
        Task<IEnumerable<BrandDto>> SearchAsync(string term);
        Task<BrandDto> CreateAsync(CreateBrandDto brand);
        Task UpdateAsync(int id, UpdateBrandDto brand);
        Task DeleteAsync(int id);
    }
}
