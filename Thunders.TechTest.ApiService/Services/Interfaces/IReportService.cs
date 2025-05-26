using Thunders.TechTest.ApiService.Dto;

namespace Thunders.TechTest.ApiService.Services.Interfaces
{
    public interface IReportService
    {
        Task<string> ExecuteAsync(string reportType, Dictionary<string, string> parameters);
        Task<object> GetResult(ResultReportDto dto);
    }
}
