using Thunders.TechTest.ApiService.Entities;

namespace Thunders.TechTest.ApiService.Repository.Interfaces
{
    public interface IRegisterUseRepository
    {
        Task<int> AddRegisterUseAsync(RegisterUse registerUse);
        Task<RegisterUse> GetByIdAsync(int id);
    }
}
