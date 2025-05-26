namespace Thunders.TechTest.ApiService.Handlers.Messages
{
    public record GenerateReportRequestedMessage(
    string ReportType,
    Dictionary<string, string> Parameters,
    string RequestId);
}
