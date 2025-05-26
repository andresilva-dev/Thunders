using Microsoft.EntityFrameworkCore;
using Thunders.TechTest.ApiService.Entities;
using Thunders.TechTest.ApiService.Repository.Interfaces;

namespace Thunders.TechTest.ApiService.Repository.Implementation
{
    public class RegisterUseRepository : IRegisterUseRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public RegisterUseRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<int> AddRegisterUseAsync(RegisterUse registerUse)
        {
            _applicationDbContext.RegistersUse.Add(registerUse);
            return await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<RegisterUse> GetByIdAsync(int id)
        {
            return await _applicationDbContext.RegistersUse
                .Include(r => r.TollStation)
                .ThenInclude(t => t.City)
                .ThenInclude(c => c.State)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}
