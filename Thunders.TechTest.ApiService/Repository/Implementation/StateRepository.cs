using Microsoft.EntityFrameworkCore;
using Thunders.TechTest.ApiService.Entities;
using Thunders.TechTest.ApiService.Repository.Interfaces;

namespace Thunders.TechTest.ApiService.Repository.Implementation
{
    public class StateRepository : IStateRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public StateRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<State>> GetAllAsync()
        {
            return await _applicationDbContext.States
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<State> GetByIdAsync(int stateId)
        {
            return await _applicationDbContext.States
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == stateId);
        }
    }
}
