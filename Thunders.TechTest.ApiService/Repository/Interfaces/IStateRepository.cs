using Thunders.TechTest.ApiService.Entities;

namespace Thunders.TechTest.ApiService.Repository.Interfaces
{
    public interface IStateRepository
    {
        Task<IEnumerable<State>> GetAllAsync();
        Task<State> GetByIdAsync(int stateId);
    }
}
