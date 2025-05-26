using Thunders.TechTest.ApiService.Entities;

namespace Thunders.TechTest.ApiService.Repository.Interfaces
{
    public interface ICityRepository
    {
        Task<IEnumerable<City>> GetAllAsync();
        Task<City> GetByIdAsync(int id);
        Task<int> AddAsync(City city);
        Task UpdateAsync(int id, City city);
        Task DeleteAsync(int id);
    }
}
