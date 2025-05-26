using Thunders.TechTest.ApiService.Services.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Thunders.TechTest.ApiService.Services.Implementation
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _db;

        public CacheService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            try
            {
                var redisKey = $"{typeof(T).Name}:{key}";
                var json = JsonSerializer.Serialize(value);
                await _db.StringSetAsync(redisKey, json, expiration);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in the try of set value in the cache: {ex.Message}");
            }
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                var redisKey = $"{typeof(T).Name}:{key}";
                var value = await _db.StringGetAsync(redisKey);
                return value.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(value!);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in the try of get value in the cache: {ex.Message}");
            }

            return default;
        }

        public async Task RemoveAsync<T>(string key)
        {
            try
            {
                var redisKey = $"{typeof(T).Name}:{key}";
                await _db.KeyDeleteAsync(redisKey);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in the try of remove value in the cache: {ex.Message}");
            }
        }

        public async Task<List<T>> GetAllAsync<T>()
        {
            var result = new List<T>();

            try
            {
                var pattern = $"{typeof(T).Name}:*";
                var server = _db.Multiplexer.GetServer(_db.Multiplexer.GetEndPoints().First());
                var keys = server.Keys(pattern: pattern);

                foreach (var key in keys)
                {
                    var value = await _db.StringGetAsync(key);
                    if (!value.IsNullOrEmpty)
                    {
                        var item = JsonSerializer.Deserialize<T>(value!);
                        if (item is not null)
                            result.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in the try of get values in the cache: {ex.Message}");
            }

            return result;
        }

        public async Task UpdateAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var redisKey = $"{typeof(T).Name}:{key}";

            var exists = await _db.KeyExistsAsync(redisKey);
            if (!exists)
            {
                throw new KeyNotFoundException($"Key '{redisKey}' not found in Redis.");
            }

            var json = JsonSerializer.Serialize(value);
            await _db.StringSetAsync(redisKey, json, expiration);
        }
    }
}
