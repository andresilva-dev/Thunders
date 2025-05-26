namespace Thunders.TechTest.ApiService.Services.Interfaces
{
    public interface IReportProcessorService
    {
        Task ProcessReportAsync(string reportType, Dictionary<string, string> parameters, string requestId);
    }
}
