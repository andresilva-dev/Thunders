using Thunders.TechTest.ApiService.Entities;

namespace Thunders.TechTest.ApiService.Repository.Interfaces
{
    public interface IRegisterUseRepository
    {
        Task AddRegisterUseAsync(RegisterUse registerUse);
        Task DeleteAsync(int id);
    }
}
