using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;
using Thunders.TechTest.ApiService.Dto;
using Thunders.TechTest.ApiService.Entities;
using Thunders.TechTest.ApiService.Enumerators;

namespace Thunders.TechTest.Tests
{
    public class RegisterUseTests : BaseTest, IClassFixture<WebApplicationFactory<Program>>
    {
        public RegisterUseTests(WebApplicationFactory<Program> factory) : base(factory, "TestRegisterUseDb")
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

            var city = new City { Id = 1, Name = "Campinas", StateId = state.Id };
            _dbContext.Cities.Add(city);

            var tollStation = new TollStation { Id = 1, Name = "Toll A", CityId = city.Id };
            _dbContext.TollStations.Add(tollStation);

            await _dbContext.SaveChangesAsync();
        }

        [Fact(DisplayName = "POST /api/registerUse should create registerUse")]
        public async Task CreateRegisterUse_ShouldReturnCreated()
        {
            var registerUseDto = new RegisterUseDto
            {
                AmountPaid = 5,
                UsedAt = DateTime.UtcNow,
                TollStationId = 1,
                VehicleType = VehicleType.Motorcycle
            };

            var response = await _client.PostAsJsonAsync("/api/registerUse", registerUseDto);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<JObject>(content);

            Assert.True((int)json["id"] > 0);
        }
    }
}
