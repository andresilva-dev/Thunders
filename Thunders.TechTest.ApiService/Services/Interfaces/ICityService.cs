using Thunders.TechTest.ApiService.Dto;
using Thunders.TechTest.ApiService.Entities;

namespace Thunders.TechTest.ApiService.Services.Interfaces
{
    public interface ICityService
    {
        Task<IEnumerable<City>> GetAllAsync();
        Task<City> GetByIdAsync(int id);
        Task<int> AddAsync(CityDto cityDto);
        Task UpdateAsync(int id, CityDto cityDto);
        Task DeleteAsync(int id);
    }
}
