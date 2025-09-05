using AutoMapper;
using Perfumes.BLL.DTOs.Product;
using Perfumes.BLL.Services.Interfaces;
using Perfumes.DAL.UnitOfWork;
using System.Linq.Expressions;

namespace Perfumes.BLL.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICachingService _cachingService;
        private readonly ILoggingService _loggingService;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, ICachingService cachingService, ILoggingService loggingService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cachingService = cachingService;
            _loggingService = loggingService;
        }

        // CRUD
        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            // v2 key to invalidate old cached shape that missed Brand/Category includes
            const string cacheKey = "products_all_v2";
            return await _cachingService.GetOrSetAsync(cacheKey, async () =>
            {
                _loggingService.LogInformation("Fetching all products from database");
                var products = await _unitOfWork.Products.GetAllAsync();
                return _mapper.Map<IEnumerable<ProductDto>>(products);
            }, TimeSpan.FromMinutes(10));
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            return await _cachingService.GetOrSetAsync($"product_{id}", async () =>
            {
                _loggingService.LogInformation($"Fetching product with ID: {id}");
                var product = await _unitOfWork.Products.GetByIdAsync(id);
                return _mapper.Map<ProductDto>(product);
            }, TimeSpan.FromMinutes(5));
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto productDto)
        {
            try
            {
                _loggingService.LogInformation($"Creating new product: {productDto.Name}");
                var product = _mapper.Map<Perfumes.DAL.Entities.Product>(productDto);
                var created = await _unitOfWork.Products.AddAsync(product);
                await _unitOfWork.SaveChangesAsync();

                // Clear cache
                await _cachingService.RemoveAsync("products_all");
                await _cachingService.RemoveAsync("products_all_v2");
                await _cachingService.RemoveAsync("products_active");
                await _cachingService.RemoveAsync("products_featured");

                _loggingService.LogInformation($"Product created successfully with ID: {created.ProductId}");
                return _mapper.Map<ProductDto>(created);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error creating product: {ex.Message}", ex);
                throw;
            }
        }

        public async Task UpdateAsync(int id, UpdateProductDto productDto)
        {
            try
            {
                _loggingService.LogInformation($"Updating product with ID: {id}");
                var existingProduct = await _unitOfWork.Products.GetByIdAsync(id);
                if (existingProduct == null)
                {
                    _loggingService.LogWarning($"Product with ID {id} not found for update");
                    throw new Exception($"Product with ID {id} not found.");
                }

                _mapper.Map(productDto, existingProduct);
                await _unitOfWork.Products.UpdateAsync(existingProduct);
                await _unitOfWork.SaveChangesAsync();

                // Clear cache
                await _cachingService.RemoveAsync($"product_{id}");
                await _cachingService.RemoveAsync("products_all");
                await _cachingService.RemoveAsync("products_all_v2");
                await _cachingService.RemoveAsync("products_active");

                _loggingService.LogInformation($"Product with ID {id} updated successfully");
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error updating product with ID {id}: {ex.Message}", ex);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                _loggingService.LogInformation($"Deleting product with ID: {id}");
                var product = await _unitOfWork.Products.GetByIdAsync(id);
                if (product != null)
                {
                    await _unitOfWork.Products.DeleteAsync(product);
                    await _unitOfWork.SaveChangesAsync();

                    // Clear cache
                    await _cachingService.RemoveAsync($"product_{id}");
                    await _cachingService.RemoveAsync("products_all");
                    await _cachingService.RemoveAsync("products_all_v2");
                    await _cachingService.RemoveAsync("products_active");

                    _loggingService.LogInformation($"Product with ID {id} deleted successfully");
                }
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error deleting product with ID {id}: {ex.Message}", ex);
                throw;
            }
        }

        // Bulk
        public async Task<IEnumerable<ProductDto>> AddRangeAsync(IEnumerable<CreateProductDto> productDtos)
        {
            try
            {
                _loggingService.LogInformation($"Adding {productDtos.Count()} products in bulk");
                var products = _mapper.Map<IEnumerable<Perfumes.DAL.Entities.Product>>(productDtos);
                var result = await _unitOfWork.Products.AddRangeAsync(products);
                await _unitOfWork.SaveChangesAsync();

                // Clear cache
                await _cachingService.RemoveAsync("products_all");
                await _cachingService.RemoveAsync("products_active");

                _loggingService.LogInformation($"Successfully added {result.Count()} products");
                return _mapper.Map<IEnumerable<ProductDto>>(result);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error adding products in bulk: {ex.Message}", ex);
                throw;
            }
        }

        public async Task DeleteRangeAsync(IEnumerable<int> productIds)
        {
            try
            {
                _loggingService.LogInformation($"Deleting {productIds.Count()} products");
                var products = await _unitOfWork.Products.FindAsync(p => productIds.Contains(p.ProductId));
                await _unitOfWork.Products.DeleteRangeAsync(products);
                await _unitOfWork.SaveChangesAsync();

                // Clear cache
                await _cachingService.RemoveAsync("products_all");
                await _cachingService.RemoveAsync("products_active");

                _loggingService.LogInformation($"Successfully deleted {products.Count()} products");
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error deleting products in bulk: {ex.Message}", ex);
                throw;
            }
        }

        // Queries with conditions
        public async Task<IEnumerable<ProductDto>> FindAsync(Expression<Func<ProductDto, bool>> predicate)
        {
            // Note: This is a simplified implementation. In practice, you might need to translate
            // the predicate from DTO to Entity or implement a more sophisticated query system
            var products = await _unitOfWork.Products.GetAllAsync();
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            return productDtos.Where(predicate.Compile());
        }

        public async Task<ProductDto?> FirstOrDefaultAsync(Expression<Func<ProductDto, bool>> predicate)
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            return productDtos.FirstOrDefault(predicate.Compile());
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Products.ExistsAsync(p => p.ProductId == id);
        }

        public async Task<int> CountAsync()
        {
            return await _unitOfWork.Products.CountAsync();
        }

        // Specialized Queries
        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _cachingService.GetOrSetAsync($"products_category_{categoryId}", async () =>
            {
                var products = await _unitOfWork.Products.GetProductsByCategoryAsync(categoryId);
                return _mapper.Map<IEnumerable<ProductDto>>(products);
            }, TimeSpan.FromMinutes(5));
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByBrandAsync(int brandId)
        {
            return await _cachingService.GetOrSetAsync($"products_brand_{brandId}", async () =>
            {
                var products = await _unitOfWork.Products.GetProductsByBrandAsync(brandId);
                return _mapper.Map<IEnumerable<ProductDto>>(products);
            }, TimeSpan.FromMinutes(5));
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByGenderAsync(string gender)
        {
            return await _cachingService.GetOrSetAsync($"products_gender_{gender}", async () =>
            {
                var products = await _unitOfWork.Products.GetProductsByGenderAsync(gender);
                return _mapper.Map<IEnumerable<ProductDto>>(products);
            }, TimeSpan.FromMinutes(5));
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _cachingService.GetOrSetAsync($"products_price_{minPrice}_{maxPrice}", async () =>
            {
                var products = await _unitOfWork.Products.GetProductsByPriceRangeAsync(minPrice, maxPrice);
                return _mapper.Map<IEnumerable<ProductDto>>(products);
            }, TimeSpan.FromMinutes(5));
        }

        public async Task<IEnumerable<ProductDto>> GetActiveProductsAsync()
        {
            return await _cachingService.GetOrSetAsync("products_active", async () =>
            {
                var products = await _unitOfWork.Products.GetActiveProductsAsync();
                return _mapper.Map<IEnumerable<ProductDto>>(products);
            }, TimeSpan.FromMinutes(10));
        }

        public async Task<ProductDto?> GetProductWithDetailsAsync(int productId)
        {
            return await _cachingService.GetOrSetAsync($"product_details_{productId}", async () =>
            {
                var product = await _unitOfWork.Products.GetProductWithDetailsAsync(productId);
                return _mapper.Map<ProductDto>(product);
            }, TimeSpan.FromMinutes(5));
        }

        public async Task<IEnumerable<ProductWithReviewsDto>> GetProductsWithReviewsAsync(int take = 10)
        {
            return await _cachingService.GetOrSetAsync($"products_reviews_{take}", async () =>
            {
                var products = await _unitOfWork.Products.GetProductsWithReviewsAsync(take);
                return _mapper.Map<IEnumerable<ProductWithReviewsDto>>(products);
            }, TimeSpan.FromMinutes(5));
        }

        public async Task<IEnumerable<ProductDto>> GetFeaturedProductsAsync()
        {
            return await _cachingService.GetOrSetAsync("products_featured", async () =>
            {
                var products = await _unitOfWork.Products.GetFeaturedProductsAsync();
                return _mapper.Map<IEnumerable<ProductDto>>(products);
            }, TimeSpan.FromMinutes(15));
        }

        public async Task<IEnumerable<ProductDto>> GetNewArrivalsAsync(int take = 10)
        {
            return await _cachingService.GetOrSetAsync($"products_new_{take}", async () =>
            {
                var products = await _unitOfWork.Products.GetNewArrivalsAsync(take);
                return _mapper.Map<IEnumerable<ProductDto>>(products);
            }, TimeSpan.FromMinutes(10));
        }

        public async Task<IEnumerable<ProductDto>> GetBestSellersAsync(int take = 10)
        {
            return await _cachingService.GetOrSetAsync($"products_bestsellers_{take}", async () =>
            {
                var products = await _unitOfWork.Products.GetBestSellersAsync(take);
                return _mapper.Map<IEnumerable<ProductDto>>(products);
            }, TimeSpan.FromMinutes(10));
        }

        public async Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm)
        {
            return await _cachingService.GetOrSetAsync($"products_search_{searchTerm}", async () =>
            {
                var products = await _unitOfWork.Products.SearchProductsAsync(searchTerm);
                return _mapper.Map<IEnumerable<ProductDto>>(products);
            }, TimeSpan.FromMinutes(5));
        }
    }
}
