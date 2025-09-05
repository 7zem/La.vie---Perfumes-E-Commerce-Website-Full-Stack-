using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Perfumes.BLL.DTOs.Category;
using Perfumes.BLL.Services.Interfaces;

namespace Perfumes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
        {
            var created = await _categoryService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.CategoryId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDto dto)
        {
            await _categoryService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetActive()
        {
            var categories = await _categoryService.GetActiveAsync();
            return Ok(categories);
        }

        [HttpGet("parents")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetParents()
        {
            var categories = await _categoryService.GetParentsAsync();
            return Ok(categories);
        }

        [HttpGet("{parentId}/subcategories")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetSubCategories(int parentId)
        {
            var categories = await _categoryService.GetSubCategoriesAsync(parentId);
            return Ok(categories);
        }

        [HttpGet("{id}/products")]
        public async Task<ActionResult<CategoryWithProductsDto>> GetWithProducts(int id)
        {
            var category = await _categoryService.GetWithProductsAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpGet("tree")]
        public async Task<ActionResult<IEnumerable<CategoryTreeDto>>> GetTree()
        {
            var tree = await _categoryService.GetTreeAsync();
            return Ok(tree);
        }
    }
}
