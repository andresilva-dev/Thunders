using Thunders.TechTest.ApiService.Entities.ReportResults;
using Thunders.TechTest.ApiService.Entities;
using Thunders.TechTest.ApiService.Services.Interfaces;

namespace Thunders.TechTest.ApiService.Services.Implementation
{
    public class ReportProcessorService : IReportProcessorService
    {
        private readonly ICacheService _cache;

        public ReportProcessorService(ICacheService cache)
        {
            _cache = cache;
        }

        public async Task ProcessReportAsync(string reportType, Dictionary<string, string> parameters, string requestId)
        {
            object result = null;

            result = reportType switch
            {
                "totalPerHourPerCity" => await GetTotalPerHourPerCity(),
                "topStationsByMonth" => await GetTopStationsByMonth(
                    int.Parse(parameters["top"]),
                    int.Parse(parameters["year"]),
                    int.Parse(parameters["month"])),
                "vehiclesByStation" => await GetVehicleTypesByStation(
                    int.Parse(parameters["stationId"])),
                _ => throw new ArgumentException("Unknown report type")
            };

            var resultKey = $"report:result:{requestId}";
            await _cache.SetAsync(resultKey, result);
        }

        private async Task<object> GetTotalPerHourPerCity()
        {
            var registers = await _cache.GetAllAsync<RegisterUse>();

            return registers
                .GroupBy(r => new
                {
                    r.TollStation.City.Id,
                    CityName = r.TollStation.City.Name,
                    StateName = r.TollStation.City.State.Name,
                    r.UsedAt.Date,
                    r.UsedAt.Hour
                })
                .Select(g => new TotalPerHourPerCityResult
                {
                    CityId = g.Key.Id,
                    CityName = g.Key.CityName,
                    StateName = g.Key.StateName,
                    Date = g.Key.Date,
                    Hour = g.Key.Hour,
                    Total = g.Sum(r => r.AmountPaid)
                })
                .OrderBy(r => r.Date)
                .ThenBy(r => r.CityName)
                .ThenBy(r => r.Hour)
                .ToList();
        }

        public async Task<object> GetTopStationsByMonth(int top, int year, int month)
        {
            var registers = await _cache.GetAllAsync<RegisterUse>();

            return registers
                .Where(r => r.UsedAt.Year == year && r.UsedAt.Month == month)
                .GroupBy(r => new
                {
                    r.TollStationId,
                    r.TollStation.Name,
                    CityName = r.TollStation.City.Name,
                    StateName = r.TollStation.City.State.Name
                })
                .Select(g => new TopStationResult
                {
                    StationId = g.Key.TollStationId,
                    StationName = g.Key.Name,
                    CityName = g.Key.CityName,
                    StateName = g.Key.StateName,
                    Total = g.Sum(r => r.AmountPaid)
                })
                .OrderByDescending(r => r.Total)
                .Take(top)
                .ToList();
        }

        public async Task<object> GetVehicleTypesByStation(int stationId)
        {
            var registers = await _cache.GetAllAsync<RegisterUse>();

            return registers
                .Where(r => r.TollStationId == stationId)
                .GroupBy(r => r.VehicleType)
                .Select(g => new VehicleTypesByStationResult
                {
                    VehicleType = g.Key,
                    Count = g.Count()
                })
                .ToList();
        }
    }
}
