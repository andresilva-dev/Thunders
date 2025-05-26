using Thunders.TechTest.ApiService.Dto;
using Thunders.TechTest.ApiService.Entities;

namespace Thunders.TechTest.ApiService.Repository.Interfaces
{
    public interface ITollStationRepository
    {
        Task<IEnumerable<TollStation>> GetAllAsync();
        Task<TollStation> GetByIdAsync(int id);
        Task<int> AddAsync(TollStation tollStation);
        Task UpdateAsync(int id, TollStation tollStation);
        Task DeleteAsync(int id);
    }
}
