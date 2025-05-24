using Thunders.TechTest.ApiService.Entities;

namespace Thunders.TechTest.ApiService.Services.Interfaces
{
    public interface IStateService
    {
        Task<IEnumerable<State>> GetAllAsync();
    }
}
