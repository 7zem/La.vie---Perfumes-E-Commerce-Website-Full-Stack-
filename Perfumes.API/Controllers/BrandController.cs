using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Perfumes.BLL.DTOs.Brand;
using Perfumes.BLL.Services.Interfaces;

namespace Perfumes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorage;

        public BrandController(
            IBrandService brandService,
            IMapper mapper,
            IFileStorageService fileStorage)
        {
            _brandService = brandService;
            _mapper = mapper;
            _fileStorage = fileStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetAll()
        {
            var brands = await _brandService.GetAllAsync();
            return Ok(brands);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BrandDto>> GetById(int id)
        {
            var brand = await _brandService.GetByIdAsync(id);
            if (brand == null) return NotFound();

            return Ok(brand);
        }

        [HttpPost]
        public async Task<ActionResult<BrandDto>> CreateBrand([FromForm] CreateBrandDto dto)
        {
            try
            {
                // Handle logo upload if provided
                if (dto.Logo != null)
                {
                    var fileName = await _fileStorage.SaveImageAsync(dto.Logo, "brands");
                    dto.LogoUrl = $"/images/brands/{fileName}";
                }

                var brand = await _brandService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = brand.BrandId }, brand);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBrand(int id, [FromForm] UpdateBrandDto dto)
        {
            try
            {
                // Handle logo upload if provided
                if (dto.Logo != null)
                {
                    var fileName = await _fileStorage.SaveImageAsync(dto.Logo, "brands");
                    dto.LogoUrl = $"/images/brands/{fileName}";
                }

                await _brandService.UpdateAsync(id, dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _brandService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetActive()
        {
            var brands = await _brandService.GetActiveAsync();
            return Ok(brands);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<BrandDto>>> Search([FromQuery] string term)
        {
            var brands = await _brandService.SearchAsync(term);
            return Ok(brands);
        }

        [HttpGet("with-count")]
        public async Task<ActionResult<IEnumerable<BrandWithProductCountDto>>> GetWithProductCount()
        {
            var brands = await _brandService.GetWithProductCountAsync();
            return Ok(brands);
        }

        [HttpGet("{id}/products")]
        public async Task<ActionResult<BrandWithProductsDto>> GetWithProducts(int id)
        {
            var brand = await _brandService.GetWithProductsAsync(id);
            if (brand == null) return NotFound();
            return Ok(brand);
        }
    }
}
