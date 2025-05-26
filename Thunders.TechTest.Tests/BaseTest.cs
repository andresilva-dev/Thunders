using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rebus.Bus;
using StackExchange.Redis;
using Thunders.TechTest.ApiService.Repository;
using Thunders.TechTest.ApiService.Services.Interfaces;
using Thunders.TechTest.Tests.FakeServices;

namespace Thunders.TechTest.Tests
{
    public abstract class BaseTest
    {
        protected readonly HttpClient _client;
        protected readonly ApplicationDbContext _dbContext;
        protected readonly FakeCacheService _cacheService;
        private readonly WebApplicationFactory<Program> _webAppFactory;

        protected BaseTest(WebApplicationFactory<Program> factory, string dbName)
        {
            var webAppFactory = factory.WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Test");
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    if (descriptor != null) services.Remove(descriptor);

                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase(dbName);
                    });

                    services.RemoveAll(typeof(IConnectionMultiplexer));
                    services.AddSingleton<FakeCacheService>();
                    services.AddSingleton<ICacheService>(sp => sp.GetRequiredService<FakeCacheService>());

                    var busDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IBus));
                    if (busDescriptor != null)
                        services.Remove(busDescriptor);

                    var fakeBus = new FakeBus();
                    services.AddSingleton<IBus>(fakeBus);
                    services.AddSingleton(fakeBus);
                });
            });

            _client = webAppFactory.CreateClient();
            _dbContext = webAppFactory.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _cacheService = webAppFactory.Services.CreateScope().ServiceProvider.GetRequiredService<FakeCacheService>();
        }
    }
}
