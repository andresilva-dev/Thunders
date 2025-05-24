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

        public async Task AddRegisterUseAsync(RegisterUse registerUse)
        {
            _applicationDbContext.RegistersUse.Add(registerUse);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var registerUse = await _applicationDbContext.RegistersUse.FindAsync(id);

            if (registerUse == null)
                throw new KeyNotFoundException($"RegisterUse with ID {id} not found.");

            _applicationDbContext.RegistersUse.Remove(registerUse);
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
