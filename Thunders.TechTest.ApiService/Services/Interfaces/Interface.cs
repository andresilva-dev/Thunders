namespace Thunders.TechTest.ApiService.Services.Interfaces
{
    public interface ICacheService
    {
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
        Task UpdateAsync<T>(string key, T value, TimeSpan? expiration = null);
        Task<T?> GetAsync<T>(string key);
        Task RemoveAsync<T>(string key);
        Task<List<T>> GetAllAsync<T>();
    }
}
