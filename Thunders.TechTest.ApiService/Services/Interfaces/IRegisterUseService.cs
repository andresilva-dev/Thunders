using Thunders.TechTest.ApiService.Dto;

namespace Thunders.TechTest.ApiService.Services.Interfaces
{
    public interface IRegisterUseService
    {
        Task AddRegisterUseAsync(RegisterUseDto dto);
        Task GetTopTollStationsByMonthAsync(int month, int year, int top);
        Task GetTotalAmountPerHourByCityAsync();
        Task GetQtdVehicleTypesByTollStationAsync(string tollStationName);
    }
}
