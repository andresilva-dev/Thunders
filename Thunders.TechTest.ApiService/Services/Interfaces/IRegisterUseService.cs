using Thunders.TechTest.ApiService.Dto;

namespace Thunders.TechTest.ApiService.Services.Interfaces
{
    public interface IRegisterUseService
    {
        Task<int> AddRegisterUseAsync(RegisterUseDto dto);
    }
}
