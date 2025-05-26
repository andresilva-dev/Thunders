using Rebus.Handlers;
using Thunders.TechTest.ApiService.Handlers.Messages;
using Thunders.TechTest.ApiService.Services.Interfaces;

namespace Thunders.TechTest.ApiService.Handlers
{
    public class GenerateReportHandler : IHandleMessages<GenerateReportRequestedMessage>
    {
        private readonly IReportProcessorService _reportProcessor;

        public GenerateReportHandler(IReportProcessorService reportProcessor)
        {
            _reportProcessor = reportProcessor;
        }

        public async Task Handle(GenerateReportRequestedMessage message)
        {
            await _reportProcessor.ProcessReportAsync(message.ReportType, message.Parameters, message.RequestId.ToString());
        }
    }

}
