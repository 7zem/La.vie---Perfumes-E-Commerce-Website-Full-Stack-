using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Perfumes.BLL.Services.Interfaces;
using Perfumes.BLL.DTOs.Product;

namespace Perfumes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorage;

        public ProductController(IProductService productService, IMapper mapper, IFileStorageService fileStorage)
        {
            _productService = productService;
            _mapper = mapper;
            _fileStorage = fileStorage;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromForm] CreateProductDto dto)
        {
            try
            {
                // Handle image upload if provided
                if (dto.Image != null)
                {
                    var fileName = await _fileStorage.SaveImageAsync(dto.Image, "products");
                    dto.ImageUrl = $"/images/products/{fileName}";
                }

                var product = await _productService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = product.ProductId }, product);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromForm] UpdateProductDto dto)
        {
            try
            {
                // Handle image upload if provided
                if (dto.Image != null)
                {
                    var fileName = await _fileStorage.SaveImageAsync(dto.Image, "products");
                    dto.ImageUrl = $"/images/products/{fileName}";
                }

                await _productService.UpdateAsync(id, dto);
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
            await _productService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("{id}/reviews")]
        public async Task<IActionResult> GetWithReviews(int id)
        {
            var productWithReviews = await _productService.GetProductWithDetailsAsync(id);
            if (productWithReviews == null) return NotFound();
            return Ok(productWithReviews);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var products = await _productService.GetProductsByCategoryAsync(categoryId);
            return Ok(products);
        }

        [HttpGet("brand/{brandId}")]
        public async Task<IActionResult> GetByBrand(int brandId)
        {
            var products = await _productService.GetProductsByBrandAsync(brandId);
            return Ok(products);
        }

        [HttpGet("gender/{gender}")]
        public async Task<IActionResult> GetByGender(string gender)
        {
            var products = await _productService.GetProductsByGenderAsync(gender);
            return Ok(products);
        }

        [HttpGet("price")]
        public async Task<IActionResult> GetByPrice([FromQuery] decimal min, [FromQuery] decimal max)
        {
            var products = await _productService.GetProductsByPriceRangeAsync(min, max);
            return Ok(products);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var products = await _productService.GetActiveProductsAsync();
            return Ok(products);
        }

        [HttpGet("featured")]
        public async Task<IActionResult> GetFeatured()
        {
            var products = await _productService.GetFeaturedProductsAsync();
            return Ok(products);
        }

        [HttpGet("new")]
        public async Task<IActionResult> GetNewArrivals()
        {
            var products = await _productService.GetNewArrivalsAsync();
            return Ok(products);
        }

        [HttpGet("best")]
        public async Task<IActionResult> GetBestSellers()
        {
            var products = await _productService.GetBestSellersAsync();
            return Ok(products);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string term)
        {
            var products = await _productService.SearchProductsAsync(term);
            return Ok(products);
        }
    }
}