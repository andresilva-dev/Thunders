using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

namespace Thunders.TechTest.Tests
{
    public class StateTests : BaseTest, IClassFixture<WebApplicationFactory<Program>>
    {
        public StateTests(WebApplicationFactory<Program> factory) : base(factory, "TestStateDb")
        {
            SeedDataAsync().GetAwaiter().GetResult();
        }

        private async Task SeedDataAsync()
        {
            _dbContext.States.RemoveRange(_dbContext.States);

            _dbContext.States.AddRange(
                new ApiService.Entities.State { Id = 1, Name = "São Paulo", Uf = "SP" },
                new ApiService.Entities.State { Id = 2, Name = "Minas Gerais", Uf = "MG" },
                new ApiService.Entities.State { Id = 3, Name = "Rio de Janeiro", Uf = "RJ" },
                new ApiService.Entities.State { Id = 4, Name = "Bahia", Uf = "BA" },
                new ApiService.Entities.State { Id = 5, Name = "Paraná", Uf = "PR" }
            );

            await _dbContext.SaveChangesAsync();
        }

        [Fact(DisplayName = "GET /api/state should return list")]
        public async Task GetStates_ShouldReturnList()
        {
            var response = await _client.GetAsync("/api/state");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<dynamic>>(content);

            Assert.NotNull(list);
            Assert.Equal(5, list.Count);
        }
    }
}
