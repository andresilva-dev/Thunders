namespace Thunders.TechTest.ApiService.Entities.ReportResults
{
    public class TotalPerHourPerCityResult
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public DateTime Date { get; set; }
        public int Hour { get; set; }
        public decimal Total { get; set; }
    }
}
