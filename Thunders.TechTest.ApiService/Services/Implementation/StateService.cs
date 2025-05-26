using Thunders.TechTest.ApiService.Entities;
using Thunders.TechTest.ApiService.Repository.Interfaces;
using Thunders.TechTest.ApiService.Services.Interfaces;

namespace Thunders.TechTest.ApiService.Services.Implementation
{
    public class StateService : IStateService
    {
        private readonly IStateRepository _stateRepository;

        public StateService(IStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }

        public async Task<IEnumerable<State>> GetAllAsync()
        {
            return await _stateRepository.GetAllAsync();
        }
    }
}
