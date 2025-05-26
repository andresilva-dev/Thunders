using Thunders.TechTest.ApiService.Services.Interfaces;

namespace Thunders.TechTest.Tests.FakeServices
{
    public class FakeCacheService : ICacheService
    {
        private readonly Dictionary<string, object> _cache = new();

        public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            _cache[key] = value;
            return Task.CompletedTask;
        }

        public Task UpdateAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            if (_cache.ContainsKey(key))
            {
                _cache[key] = value;
            }
            return Task.CompletedTask;
        }

        public Task<T?> GetAsync<T>(string key)
        {
            if (_cache.TryGetValue(key, out var entry))
            {
                return Task.FromResult((T?)entry);
            }

            return Task.FromResult<T?>(default);
        }

        public Task RemoveAsync<T>(string key)
        {
            _cache.Remove(key);
            return Task.CompletedTask;
        }

        public Task<List<T>> GetAllAsync<T>()
        {
            var list = _cache.Values
                .Where(v => v is T )
                .Select(v => (T)v)
                .ToList();

            return Task.FromResult(list);
        }
    }
}
