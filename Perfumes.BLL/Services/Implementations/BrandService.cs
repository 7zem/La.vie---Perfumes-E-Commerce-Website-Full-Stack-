using AutoMapper;
using Perfumes.BLL.DTOs.Brand;
using Perfumes.BLL.Services.Interfaces;
using Perfumes.DAL.Exceptions;
using Perfumes.DAL.UnitOfWork;

namespace Perfumes.BLL.Services.Implementations
{
    public class BrandService : IBrandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BrandService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BrandDto>> GetAllAsync()
        {
            var brands = await _unitOfWork.Brands.GetAllAsync();
            return _mapper.Map<IEnumerable<BrandDto>>(brands);
        }

        public async Task<IEnumerable<BrandDto>> GetActiveAsync()
        {
            var brands = await _unitOfWork.Brands.GetActiveBrandsAsync();
            return _mapper.Map<IEnumerable<BrandDto>>(brands);
        }

        public async Task<BrandDto?> GetByIdAsync(int id)
        {
            var brand = await _unitOfWork.Brands.GetByIdAsync(id);
            return _mapper.Map<BrandDto>(brand);
        }

        public async Task<BrandWithProductsDto?> GetWithProductsAsync(int brandId)
        {
            var brand = await _unitOfWork.Brands.GetBrandWithProductsAsync(brandId);
            return _mapper.Map<BrandWithProductsDto>(brand);
        }

        public async Task<IEnumerable<BrandWithProductCountDto>> GetWithProductCountAsync()
        {
            var brands = await _unitOfWork.Brands.GetBrandsWithProductCountAsync();
            return _mapper.Map<IEnumerable<BrandWithProductCountDto>>(brands);
        }

        public async Task<IEnumerable<BrandDto>> SearchAsync(string term)
        {
            var brands = await _unitOfWork.Brands.SearchBrandsAsync(term);
            return _mapper.Map<IEnumerable<BrandDto>>(brands);
        }

        public async Task<BrandDto> CreateAsync(CreateBrandDto brandDto)
        {
            var brand = _mapper.Map<Perfumes.DAL.Entities.Brand>(brandDto);
            var created = await _unitOfWork.Brands.AddAsync(brand);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<BrandDto>(created);
        }

        public async Task UpdateAsync(int id, UpdateBrandDto brandDto)
        {
            var existing = await _unitOfWork.Brands.GetByIdAsync(id);
            if (existing == null)
                throw new EntityNotFoundException(nameof(Perfumes.DAL.Entities.Brand), id);

            _mapper.Map(brandDto, existing);
            await _unitOfWork.Brands.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var brand = await _unitOfWork.Brands.GetByIdAsync(id);
            if (brand == null)
                throw new EntityNotFoundException(nameof(Perfumes.DAL.Entities.Brand), id);

            await _unitOfWork.Brands.DeleteAsync(brand);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
