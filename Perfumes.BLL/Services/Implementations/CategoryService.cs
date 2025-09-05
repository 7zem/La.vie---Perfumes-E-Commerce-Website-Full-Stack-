using AutoMapper;
using Perfumes.BLL.DTOs.Category;
using Perfumes.BLL.Services.Interfaces;
using Perfumes.DAL.Exceptions;
using Perfumes.DAL.UnitOfWork;

namespace Perfumes.BLL.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto categoryDto)
        {
            var existing = await _unitOfWork.Categories
                .FirstOrDefaultAsync(c => c.Name == categoryDto.Name);

            if (existing != null)
                throw new DuplicateEntityException("Category", "Name", categoryDto.Name);

            var category = _mapper.Map<Perfumes.DAL.Entities.Category>(categoryDto);
            var created = await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CategoryDto>(created);
        }

        public async Task UpdateAsync(int id, UpdateCategoryDto categoryDto)
        {
            var existing = await _unitOfWork.Categories.GetByIdAsync(id);
            if (existing == null)
                throw new EntityNotFoundException("Category", id);

            _mapper.Map(categoryDto, existing);
            await _unitOfWork.Categories.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _unitOfWork.Categories.GetByIdAsync(id);
            if (existing == null)
                throw new EntityNotFoundException("Category", id);

            await _unitOfWork.Categories.DeleteAsync(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<CategoryDto>> GetActiveAsync()
        {
            var categories = await _unitOfWork.Categories.GetActiveCategoriesAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<IEnumerable<CategoryDto>> GetParentsAsync()
        {
            var categories = await _unitOfWork.Categories.GetParentCategoriesAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<IEnumerable<CategoryDto>> GetSubCategoriesAsync(int parentId)
        {
            var categories = await _unitOfWork.Categories.GetSubCategoriesAsync(parentId);
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryWithProductsDto?> GetWithProductsAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetCategoryWithProductsAsync(id);
            return _mapper.Map<CategoryWithProductsDto>(category);
        }

        public async Task<IEnumerable<CategoryTreeDto>> GetTreeAsync()
        {
            var categories = await _unitOfWork.Categories.GetCategoryTreeAsync();
            return _mapper.Map<IEnumerable<CategoryTreeDto>>(categories);
        }
    }
}
