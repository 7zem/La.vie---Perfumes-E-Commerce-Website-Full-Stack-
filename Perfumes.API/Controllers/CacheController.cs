using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perfumes.BLL.Services.Interfaces;

namespace Perfumes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class CacheController : ControllerBase
    {
        private readonly IRedisCacheService _cacheService;
        private readonly ILoggingService _loggingService;

        public CacheController(IRedisCacheService cacheService, ILoggingService loggingService)
        {
            _cacheService = cacheService;
            _loggingService = loggingService;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetCacheStats()
        {
            try
            {
                var databaseSize = await _cacheService.GetDatabaseSizeAsync();
                
                var stats = new
                {
                    DatabaseSize = databaseSize,
                    Timestamp = DateTime.UtcNow
                };

                return Ok(stats);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting cache stats: {ex.Message}", ex);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("keys")]
        public async Task<IActionResult> GetKeysByPattern([FromQuery] string pattern = "*")
        {
            try
            {
                var keys = await _cacheService.GetKeysByPatternAsync(pattern);
                return Ok(new { keys = keys.ToList() });
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting cache keys: {ex.Message}", ex);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("keys/{key}")]
        public async Task<IActionResult> GetKeyValue(string key)
        {
            try
            {
                var exists = await _cacheService.ExistsAsync(key);
                if (!exists)
                    return NotFound(new { message = "Key not found" });

                var value = await _cacheService.GetAsync<object>(key);
                var expiration = await _cacheService.GetExpirationAsync(key);

                var result = new
                {
                    Key = key,
                    Value = value,
                    Exists = exists,
                    Expiration = expiration
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting cache key value: {ex.Message}", ex);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("keys/{key}")]
        public async Task<IActionResult> RemoveKey(string key)
        {
            try
            {
                await _cacheService.RemoveAsync(key);
                return Ok(new { message = $"Key '{key}' removed successfully" });
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error removing cache key: {ex.Message}", ex);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("pattern/{pattern}")]
        public async Task<IActionResult> RemoveByPattern(string pattern)
        {
            try
            {
                await _cacheService.RemoveByPatternAsync(pattern);
                return Ok(new { message = $"Keys matching pattern '{pattern}' removed successfully" });
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error removing cache by pattern: {ex.Message}", ex);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("flush")]
        public async Task<IActionResult> FlushDatabase()
        {
            try
            {
                await _cacheService.FlushDatabaseAsync();
                return Ok(new { message = "Cache database flushed successfully" });
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error flushing cache database: {ex.Message}", ex);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("invalidate/products")]
        public async Task<IActionResult> InvalidateProductCache()
        {
            try
            {
                await _cacheService.RemoveByPatternAsync("products:*");
                return Ok(new { message = "Product cache invalidated successfully" });
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error invalidating product cache: {ex.Message}", ex);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("invalidate/categories")]
        public async Task<IActionResult> InvalidateCategoryCache()
        {
            try
            {
                await _cacheService.RemoveByPatternAsync("categories:*");
                return Ok(new { message = "Category cache invalidated successfully" });
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error invalidating category cache: {ex.Message}", ex);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("invalidate/brands")]
        public async Task<IActionResult> InvalidateBrandCache()
        {
            try
            {
                await _cacheService.RemoveByPatternAsync("brands:*");
                return Ok(new { message = "Brand cache invalidated successfully" });
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error invalidating brand cache: {ex.Message}", ex);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("invalidate/dashboard")]
        public async Task<IActionResult> InvalidateDashboardCache()
        {
            try
            {
                await _cacheService.RemoveByPatternAsync("dashboard:*");
                return Ok(new { message = "Dashboard cache invalidated successfully" });
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error invalidating dashboard cache: {ex.Message}", ex);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("invalidate/all")]
        public async Task<IActionResult> InvalidateAllCache()
        {
            try
            {
                await _cacheService.FlushDatabaseAsync();
                return Ok(new { message = "All cache invalidated successfully" });
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error invalidating all cache: {ex.Message}", ex);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("test")]
        public async Task<IActionResult> TestCache()
        {
            try
            {
                var testKey = "test:cache";
                var testValue = new { message = "Cache is working!", timestamp = DateTime.UtcNow };

                // Set test value
                await _cacheService.SetAsync(testKey, testValue, TimeSpan.FromMinutes(5));

                // Get test value
                var retrievedValue = await _cacheService.GetAsync<object>(testKey);

                // Check if exists
                var exists = await _cacheService.ExistsAsync(testKey);

                // Get expiration
                var expiration = await _cacheService.GetExpirationAsync(testKey);

                // Clean up
                await _cacheService.RemoveAsync(testKey);

                var result = new
                {
                    TestKey = testKey,
                    SetValue = testValue,
                    RetrievedValue = retrievedValue,
                    Exists = exists,
                    Expiration = expiration,
                    Status = "Cache test completed successfully"
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error testing cache: {ex.Message}", ex);
                return BadRequest(new { message = ex.Message });
            }
        }
    }
} 