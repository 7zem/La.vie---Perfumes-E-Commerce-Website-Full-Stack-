namespace Perfumes.BLL.Services.Interfaces
{
    public interface IRedisCacheService
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
        Task RemoveAsync(string key);
        Task<bool> ExistsAsync(string key);
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null);
        Task RemoveByPatternAsync(string pattern);
        Task<long> IncrementAsync(string key, long value = 1);
        Task<double> IncrementAsync(string key, double value = 1);
        Task<bool> SetExpirationAsync(string key, TimeSpan expiration);
        Task<TimeSpan?> GetExpirationAsync(string key);
        Task<IEnumerable<string>> GetKeysByPatternAsync(string pattern);
        Task<long> GetDatabaseSizeAsync();
        Task FlushDatabaseAsync();
    }
} 