using Rebus.Bus;
using Thunders.TechTest.ApiService.Dto;
using Thunders.TechTest.ApiService.Handlers.Messages;
using Thunders.TechTest.ApiService.Services.Interfaces;

namespace Thunders.TechTest.ApiService.Services.Implementation
{
    public class ReportService : IReportService
    {
        private readonly ICacheService _cache;
        private readonly IBus _messageSender;

        public ReportService(ICacheService cache, IBus messageSender)
        {
            _cache = cache;
            _messageSender = messageSender;
        }

        public async Task<string> ExecuteAsync(string reportType, Dictionary<string, string> parameters)
        {
            var requestId = Guid.NewGuid();

            parameters ??= [];

            var message = new GenerateReportRequestedMessage(reportType, parameters, requestId.ToString());
            
            await _messageSender.SendLocal(message);

            await _cache.SetAsync(requestId.ToString(), new
            {
                Report = reportType
            });

           return requestId.ToString();
        }

        public async Task<object> GetResult(ResultReportDto dto)
        {
            var resultKey = $"report:result:{dto.RequestReportId}";
            var result = await _cache.GetAsync<object>(resultKey);

            return result;
        }
    }
}
