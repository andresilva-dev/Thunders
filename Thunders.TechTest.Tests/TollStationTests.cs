using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Thunders.TechTest.ApiService.Dto;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Thunders.TechTest.ApiService.Entities;

namespace Thunders.TechTest.Tests
{
    public class TollStationTests : BaseTest, IClassFixture<WebApplicationFactory<Program>>
    {
        public TollStationTests(WebApplicationFactory<Program> factory) : base(factory, "TestTollStationDb")
        {
            SeedDataAsync().GetAwaiter().GetResult();
        }

        private async Task SeedDataAsync()
        {
            _dbContext.TollStations.RemoveRange(_dbContext.TollStations);
            _dbContext.Cities.RemoveRange(_dbContext.Cities);
            _dbContext.States.RemoveRange(_dbContext.States);

            var state = new State { Id = 1, Name = "São Paulo", Uf = "SP" };
            _dbContext.States.Add(state);

            var city = new City { Id = 1, Name = "Campinas", StateId = 1 };
            _dbContext.Cities.Add(city);

            var tollStation = new TollStation { Id = 1, Name = "Pedágio Leste", CityId = 1 };
            _dbContext.TollStations.Add(tollStation);

            await _dbContext.SaveChangesAsync();
        }

        [Fact(DisplayName = "POST /api/tollstation should create toll station")]
        public async Task CreateTollStation_ShouldReturnOk()
        {
            var dto = new TollStationDto
            {
                Name = "Pedágio Norte",
                CityId = 1
            };

            var response = await _client.PostAsJsonAsync("/api/tollstation", dto);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<JObject>(content);
            var id = (int)json["id"];
            Assert.True(id > 0);
        }

        [Fact(DisplayName = "GET /api/tollstation should return list")]
        public async Task GetTollStations_ShouldReturnList()
        {
            var response = await _client.GetAsync("/api/tollstation");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<dynamic>>(content);

            Assert.NotNull(list);
            Assert.True(list.Count > 0);
        }

        [Fact(DisplayName = "GET /api/tollstation/{id} should return item")]
        public async Task GetTollStationById_ShouldReturnItem()
        {
            var response = await _client.GetAsync("/api/tollstation/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var item = JsonConvert.DeserializeObject<JObject>(content);
            var id = (int)item["id"];
            Assert.Equal(1, id);
        }

        [Fact(DisplayName = "PUT /api/tollstation/{id} should update item")]
        public async Task UpdateTollStation_ShouldSucceed()
        {
            var dto = new TollStationDto
            {
                Name = "Pedágio Atualizado",
                CityId = 1
            };

            var response = await _client.PutAsJsonAsync("/api/tollstation/1", dto);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
