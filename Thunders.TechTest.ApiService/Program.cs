using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Thunders.TechTest.ApiService;
using Thunders.TechTest.ApiService.Dto;
using Thunders.TechTest.ApiService.Repository;
using Thunders.TechTest.ApiService.Services.Implementation;
using Thunders.TechTest.ApiService.Services.Interfaces;
using Thunders.TechTest.ApiService.Validators;
using Thunders.TechTest.OutOfBox.Database;
using Thunders.TechTest.ApiService.Repository.Interfaces;
using Thunders.TechTest.ApiService.Repository.Implementation;
using StackExchange.Redis;
using Rebus.Config;
using Rebus.Bus;
using Thunders.TechTest.ApiService.Handlers;

public class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddServiceDefaults();
        builder.Services.AddControllers();

        var features = Features.BindFromConfiguration(builder.Configuration);

        builder.Services.AddProblemDetails();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Thunders TechTest API",
                Version = "v1"
            });
        });

        //if (features.UseMessageBroker)
        //{
        //    builder.Services.AddBus(builder.Configuration, new SubscriptionBuilder());
        //}

        if (features.UseEntityFramework)
        {
            builder.Services.AddSqlServerDbContext<ApplicationDbContext>(builder.Configuration);
        }

        builder.Services.AddScoped<IValidator<RegisterUseDto>, RegisterUseDtoValidator>();
        builder.Services.AddScoped<IValidator<CityDto>, CityDtoValidator>();
        builder.Services.AddScoped<IValidator<TollStationDto>, TollStationDtoValidator>();
        builder.Services.AddScoped<IValidator<TopStationsReportRequestDto>, TopStationsReportRequestDtoValidator>();
        builder.Services.AddScoped<IValidator<VehicleTypesByStationRequestDto>, VehicleTypesByStationRequestDtoValidator>();
        builder.Services.AddScoped<IValidator<ResultReportDto>, ResultReportDtoValidator>();

        builder.Services.AddScoped<IRegisterUseService, RegisterUseService>();
        builder.Services.AddScoped<IStateService, StateService>();
        builder.Services.AddScoped<ICityService, CityService>();
        builder.Services.AddScoped<ITollStationService, TollStationService>();
        builder.Services.AddScoped<ICacheService, CacheService>();
        builder.Services.AddScoped<IReportService, ReportService>();
        builder.Services.AddScoped<IReportProcessorService, ReportProcessorService>();

        builder.Services.AddScoped<IRegisterUseRepository, RegisterUseRepository>();
        builder.Services.AddScoped<IStateRepository, StateRepository>();
        builder.Services.AddScoped<ICityRepository, CityRepository>();
        builder.Services.AddScoped<ITollStationRepository, TollStationRepository>();

        builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var configuration = builder.Configuration.GetConnectionString("cache");
            return ConnectionMultiplexer.Connect(configuration);
        });

        builder.Services.AutoRegisterHandlersFromAssemblyOf<GenerateReportHandler>();

        if (!builder.Environment.IsEnvironment("Test"))
        {
            var rabbitMqUrl = builder.Configuration.GetConnectionString("RabbitMq");

            builder.Services.AddRebus(c => c
                    .Transport(t =>
                    {
                        t.UseRabbitMq(builder.Configuration.GetConnectionString("RabbitMq"), "Thunders.TechTest");
                    }));
        }

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            applicationDbContext.ApplyMigrations();

            //var migrator = applicationDbContext.GetService<IMigrator>();
            //migrator.Migrate("0");

            var services = scope.ServiceProvider;
            var cacheService = services.GetRequiredService<ICacheService>();

            var registers = await applicationDbContext.RegistersUse
                .Include(r => r.TollStation)
                .ThenInclude(t => t.City)
                .ThenInclude(c => c.State)
                .ToListAsync();

            foreach (var register in registers)
            {
                await cacheService.SetAsync(register.Id.ToString(), register);
            }
        }

        //using (var scope = app.Services.CreateScope())
        //{
        //    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        //    var migrationsAssembly = dbContext.GetService<IMigrationsAssembly>();

        //    var migrationFolderPath = Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\Migrations");

        //    var allMigrations = migrationsAssembly.Migrations.Keys;

        //    foreach (var migrationName in allMigrations)
        //    {
        //        var migrationFile = Directory
        //            .EnumerateFiles(migrationFolderPath, $"{migrationName}*.cs", SearchOption.AllDirectories)
        //            .ToList();

        //        foreach (var file in migrationFile)
        //        {
        //            File.Delete(file);
        //            Console.WriteLine($"Migration file deleted: {file}");
        //        }
        //    }
        //}
        app.Services.GetRequiredService<IBus>();

        app.UseExceptionHandler();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Thunders TechTest API V1");
            c.RoutePrefix = string.Empty; 
        });

        app.MapDefaultEndpoints();
        app.MapControllers();

        app.Run();
    }
}