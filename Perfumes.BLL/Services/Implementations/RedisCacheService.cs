using Microsoft.Extensions.Options;
using Perfumes.BLL.Configuration;
using Perfumes.BLL.Services.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Perfumes.BLL.Services.Implementations
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _database;
        private readonly RedisSettings _settings;
        private readonly ILoggingService _loggingService;

        public RedisCacheService(
            IConnectionMultiplexer redis,
            IOptions<RedisSettings> settings,
            ILoggingService loggingService)
        {
            _redis = redis;
            _database = redis.GetDatabase();
            _settings = settings.Value;
            _loggingService = loggingService;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                var prefixedKey = GetPrefixedKey(key);
                var value = await _database.StringGetAsync(prefixedKey);
                
                if (value.IsNull)
                    return default;

                return JsonSerializer.Deserialize<T>(value!);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting from Redis cache: {ex.Message}", ex);
                return default;
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            try
            {
                var prefixedKey = GetPrefixedKey(key);
                var serializedValue = JsonSerializer.Serialize(value);
                
                if (expiration.HasValue)
                    await _database.StringSetAsync(prefixedKey, serializedValue, expiration.Value);
                else
                    await _database.StringSetAsync(prefixedKey, serializedValue);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error setting Redis cache: {ex.Message}", ex);
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                var prefixedKey = GetPrefixedKey(key);
                await _database.KeyDeleteAsync(prefixedKey);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error removing from Redis cache: {ex.Message}", ex);
            }
        }

        public async Task<bool> ExistsAsync(string key)
        {
            try
            {
                var prefixedKey = GetPrefixedKey(key);
                return await _database.KeyExistsAsync(prefixedKey);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error checking Redis cache existence: {ex.Message}", ex);
                return false;
            }
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
        {
            try
            {
                var cachedValue = await GetAsync<T>(key);
                if (cachedValue != null)
                    return cachedValue;

                var value = await factory();
                await SetAsync(key, value, expiration);
                return value;
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error in GetOrSet Redis cache: {ex.Message}", ex);
                return await factory();
            }
        }

        public async Task RemoveByPatternAsync(string pattern)
        {
            try
            {
                var prefixedPattern = GetPrefixedKey(pattern);
                var server = _redis.GetServer(_redis.GetEndPoints().First());
                var keys = server.Keys(pattern: prefixedPattern);
                
                foreach (var key in keys)
                {
                    await _database.KeyDeleteAsync(key);
                }
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error removing by pattern from Redis cache: {ex.Message}", ex);
            }
        }

        public async Task<long> IncrementAsync(string key, long value = 1)
        {
            try
            {
                var prefixedKey = GetPrefixedKey(key);
                return await _database.StringIncrementAsync(prefixedKey, value);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error incrementing Redis cache: {ex.Message}", ex);
                return 0;
            }
        }

        public async Task<double> IncrementAsync(string key, double value = 1)
        {
            try
            {
                var prefixedKey = GetPrefixedKey(key);
                return await _database.StringIncrementAsync(prefixedKey, value);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error incrementing Redis cache: {ex.Message}", ex);
                return 0;
            }
        }

        public async Task<bool> SetExpirationAsync(string key, TimeSpan expiration)
        {
            try
            {
                var prefixedKey = GetPrefixedKey(key);
                return await _database.KeyExpireAsync(prefixedKey, expiration);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error setting expiration in Redis cache: {ex.Message}", ex);
                return false;
            }
        }

        public async Task<TimeSpan?> GetExpirationAsync(string key)
        {
            try
            {
                var prefixedKey = GetPrefixedKey(key);
                return await _database.KeyTimeToLiveAsync(prefixedKey);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting expiration from Redis cache: {ex.Message}", ex);
                return null;
            }
        }

        public Task<IEnumerable<string>> GetKeysByPatternAsync(string pattern)
        {
            try
            {
                var prefixedPattern = GetPrefixedKey(pattern);
                var server = _redis.GetServer(_redis.GetEndPoints().First());
                var keys = server.Keys(pattern: prefixedPattern);
                
                return Task.FromResult(keys.Select(k => k.ToString().Replace(_settings.InstanceName, "")));
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting keys by pattern from Redis cache: {ex.Message}", ex);
                return Task.FromResult(Enumerable.Empty<string>());
            }
        }

        public Task<long> GetDatabaseSizeAsync()
        {
            try
            {
                var server = _redis.GetServer(_redis.GetEndPoints().First());
                return Task.FromResult(server.DatabaseSize());
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting database size from Redis: {ex.Message}", ex);
                return Task.FromResult(0L);
            }
        }

        public async Task FlushDatabaseAsync()
        {
            try
            {
                await _database.ExecuteAsync("FLUSHDB");
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error flushing Redis database: {ex.Message}", ex);
            }
        }

        private string GetPrefixedKey(string key)
        {
            return $"{_settings.InstanceName}{key}";
        }
    }
} 