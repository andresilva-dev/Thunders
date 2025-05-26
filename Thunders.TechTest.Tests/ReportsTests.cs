using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Json;
using Thunders.TechTest.ApiService.Dto;
using Thunders.TechTest.ApiService.Entities;
using Thunders.TechTest.ApiService.Enumerators;
using Thunders.TechTest.ApiService.Handlers.Messages;
using Thunders.TechTest.ApiService.Handlers;
using Thunders.TechTest.ApiService.Services.Implementation;
using IdentityModel.Client;

namespace Thunders.TechTest.Tests
{
    public class ReportsTests : BaseTest, IClassFixture<WebApplicationFactory<Program>>
    {
        public ReportsTests(WebApplicationFactory<Program> factory) : base(factory, "TestReportsDb")
        {
            SeedDataAsync().GetAwaiter().GetResult();
        }

        private async Task SeedDataAsync()
        {
            _dbContext.States.RemoveRange(_dbContext.States);
            _dbContext.Cities.RemoveRange(_dbContext.Cities);
            _dbContext.TollStations.RemoveRange(_dbContext.TollStations);
            _dbContext.RegistersUse.RemoveRange(_dbContext.RegistersUse);

            var state = new State { Id = 1, Name = "São Paulo", Uf = "SP" };
            _dbContext.States.Add(state);

            var city = new City { Id = 1, Name = "Campinas", StateId = 1 };
            _dbContext.Cities.Add(city);

            var tollStation1 = new TollStation { Id = 1, Name = "Toll 1", CityId = 1 };
            var tollStation2 = new TollStation { Id = 2, Name = "Toll 2", CityId = 1 };
            var tollStation3 = new TollStation { Id = 3, Name = "Toll 3", CityId = 1 };
            _dbContext.TollStations.AddRange(tollStation1, tollStation2, tollStation3);

            await _dbContext.SaveChangesAsync();

            var now = DateTime.UtcNow;

            var registerUses = new List<RegisterUse>
            {
                new RegisterUse { AmountPaid = 5, UsedAt = now.AddMinutes(-15), TollStationId = 1, VehicleType = VehicleType.Car },
                new RegisterUse { AmountPaid = 7, UsedAt = now.AddMinutes(-30), TollStationId = 1, VehicleType = VehicleType.Truck },
                new RegisterUse { AmountPaid = 4, UsedAt = now.AddMinutes(-45), TollStationId = 2, VehicleType = VehicleType.Motorcycle },
                new RegisterUse { AmountPaid = 6, UsedAt = now.AddMinutes(-60), TollStationId = 3, VehicleType = VehicleType.Car },
                new RegisterUse { AmountPaid = 8, UsedAt = now.AddMinutes(-75), TollStationId = 3, VehicleType = VehicleType.Truck }
            };

            _dbContext.RegistersUse.AddRange(registerUses);

            await _dbContext.SaveChangesAsync();

            foreach (var register in _dbContext.RegistersUse)
            {
                await _cacheService.SetAsync(register.Id.ToString(), register);
            }
        }

        [Fact(DisplayName = "POST /api/reports/generate/total-hour-city should return requestId and report result")]
        public async Task GenerateTotalHourCityReport_ShouldReturnRequestId()
        {
            var response = await _client.PostAsync("/api/reports/generate/total-hour-city", null);
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<JObject>(content);

            var requestId = json["requestId"]?.ToString();
            Assert.False(string.IsNullOrWhiteSpace(requestId));

            var parameters = new Dictionary<string, string>
            {
                { "cityId", "1" }
            };

            ExecuteHandler("totalPerHourPerCity", requestId, parameters);

            Thread.Sleep(1000);

            var resultResponse = await _client.PostAsJsonAsync("/api/reports/result", new ResultReportDto { RequestReportId = requestId });

            Assert.Equal(HttpStatusCode.OK, resultResponse.StatusCode);

            var resultContent = await resultResponse.Content.ReadAsStringAsync();
            Assert.False(string.IsNullOrWhiteSpace(resultContent));
        }


        [Fact(DisplayName = "POST /api/reports/generate/top-stations-month should return requestId and report result")]
        public async Task GenerateTopStationsReport_ShouldReturnRequestId()
        {
            var dto = new TopStationsReportRequestDto
            {
                QtdToProcess = 2,
                Month = DateTime.UtcNow.Month,
                Year = DateTime.UtcNow.Year
            };

            var response = await _client.PostAsJsonAsync("/api/reports/generate/top-stations-month", dto);
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<JObject>(content);

            var requestId = json["requestId"]?.ToString();
            Assert.False(string.IsNullOrWhiteSpace(requestId));

            var parameters = new Dictionary<string, string>
            {
                { "top", dto.QtdToProcess.ToString() },
                { "month", dto.Month.ToString() },
                { "year", dto.Year.ToString() }
            };

            ExecuteHandler("topStationsByMonth", requestId, parameters);

            Thread.Sleep(1000);

            var resultResponse = await _client.PostAsJsonAsync("/api/reports/result", new ResultReportDto { RequestReportId = requestId });

            Assert.Equal(HttpStatusCode.OK, resultResponse.StatusCode);

            var resultContent = await resultResponse.Content.ReadAsStringAsync();
            Assert.False(string.IsNullOrWhiteSpace(resultContent));
        }


        [Fact(DisplayName = "POST /api/reports/generate/vehicle-types-by-station should return requestId and report result")]
        public async Task GenerateVehicleTypesByStationReport_ShouldReturnRequestId()
        {
            var dto = new VehicleTypesByStationRequestDto
            {
                StationId = 3
            };

            var response = await _client.PostAsJsonAsync("/api/reports/generate/vehicle-types-by-station", dto);
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<JObject>(content);

            var requestId = json["requestId"]?.ToString();
            Assert.False(string.IsNullOrWhiteSpace(requestId));

            var parameters = new Dictionary<string, string>
            {
                { "stationId", "1" }
            };

            ExecuteHandler("vehiclesByStation", requestId, parameters);

            Thread.Sleep(1000);

            var resultResponse = await _client.PostAsJsonAsync("/api/reports/result", new ResultReportDto { RequestReportId = requestId });

            Assert.Equal(HttpStatusCode.OK, resultResponse.StatusCode);

            var resultContent = await resultResponse.Content.ReadAsStringAsync();
            Assert.False(string.IsNullOrWhiteSpace(resultContent));
        }

        private async void ExecuteHandler(string reportType,string requestId, Dictionary<string, string> parameters)
        {
            var reportProcessorService = new ReportProcessorService(_cacheService);

            var handler = new GenerateReportHandler(reportProcessorService);

            var message = new GenerateReportRequestedMessage(reportType, parameters, requestId.ToString());

            await handler.Handle(message);

        }
    }
}
