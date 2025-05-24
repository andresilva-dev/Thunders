using Thunders.TechTest.ApiService.Dto;
using Thunders.TechTest.ApiService.Entities;

namespace Thunders.TechTest.ApiService.Services.Interfaces
{
    public interface ITollStationService
    {
        Task<IEnumerable<TollStation>> GetAllAsync();
        Task<TollStation> GetByIdAsync(int id);
        Task<int> AddAsync(TollStationDto tollStationDto);
        Task UpdateAsync(int id, TollStationDto tollStationDto);
    }
}
