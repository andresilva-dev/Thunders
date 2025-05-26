using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;
using Thunders.TechTest.ApiService.Dto;
using Thunders.TechTest.ApiService.Entities;

namespace Thunders.TechTest.Tests
{
    public class CityTests : BaseTest, IClassFixture<WebApplicationFactory<Program>>
    {
        public CityTests(WebApplicationFactory<Program> factory) : base(factory, "TestCityDb")
        {
            SeedDataAsync().GetAwaiter().GetResult();
        }

        private async Task SeedDataAsync()
        {
            _dbContext.States.RemoveRange(_dbContext.States);
            _dbContext.Cities.RemoveRange(_dbContext.Cities);

            var state = new State { Id = 1, Name = "São Paulo", Uf = "SP" };
            _dbContext.States.Add(state);

            await _dbContext.SaveChangesAsync();

            var city = new City { Id = 1,StateId = 1, Name= "Sorocaba" };
            _dbContext.Cities.Add(city);

            var city2 = new City { Id = 2, StateId = 1, Name = "Santos" };
            _dbContext.Cities.Add(city2);

            await _dbContext.SaveChangesAsync();
        }

        [Fact(DisplayName = "POST /api/city should create city")]
        public async Task CreateCity_ShouldReturnCreated()
        {
            var city = new CityDto { Name = "Campinas", StateId = 1 };

            var response = await _client.PostAsJsonAsync("/api/city", city);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "GET /api/city should return list")]
        public async Task GetCities_ShouldReturnList()
        {
            var response = await _client.GetAsync("/api/city");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<dynamic>>(content);

            Assert.True(list.Count == 2);
        }

        [Fact(DisplayName = "GET /api/city/{id} should return single city")]
        public async Task GetCityById_ShouldReturnCity()
        {
            var response = await _client.GetAsync($"/api/city/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var item = JsonConvert.DeserializeObject<JObject>(content);

            var value = (int)item["id"];
            Assert.True(value == 1);
        }

        [Fact(DisplayName = "PUT /api/city/{id} should update city")]
        public async Task UpdateCity_ShouldSucceed()
        {
            var updateDto = new CityDto { Name = "Jundiaí Atualizado", StateId = 1 };

            var response = await _client.PutAsJsonAsync($"/api/city/1", updateDto);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "DELETE /api/city/{id} should remove city")]
        public async Task DeleteCity_ShouldSucceed()
        {
            var response = await _client.DeleteAsync($"/api/city/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
