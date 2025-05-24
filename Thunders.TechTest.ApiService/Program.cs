using FluentValidation;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Thunders.TechTest.ApiService;
using Thunders.TechTest.ApiService.Dto;
using Thunders.TechTest.ApiService.Repository;
using Thunders.TechTest.ApiService.Services.Implementation;
using Thunders.TechTest.ApiService.Services.Interfaces;
using Thunders.TechTest.ApiService.Validators;
using Thunders.TechTest.OutOfBox.Database;
using Thunders.TechTest.OutOfBox.Queues;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Thunders.TechTest.ApiService.Repository.Interfaces;
using Thunders.TechTest.ApiService.Repository.Implementation;

internal class Program
{
    private static void Main(string[] args)
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

        if (features.UseMessageBroker)
        {
            builder.Services.AddBus(builder.Configuration, new SubscriptionBuilder());
        }

        if (features.UseEntityFramework)
        {
            builder.Services.AddSqlServerDbContext<ApplicationDbContext>(builder.Configuration);
        }

        builder.Services.AddScoped<IValidator<RegisterUseDto>, RegisterUseDtoValidator>();
        builder.Services.AddScoped<IValidator<CityDto>, CityDtoValidator>();
        builder.Services.AddScoped<IValidator<TollStationDto>, TollStationDtoValidator>();

        builder.Services.AddScoped<IRegisterUseService, RegisterUseService>();
        builder.Services.AddScoped<IStateService, StateService>();
        builder.Services.AddScoped<ICityService, CityService>();
        builder.Services.AddScoped<ITollStationService, TollStationService>();

        builder.Services.AddScoped<IRegisterUseRepository, RegisterUseRepository>();
        builder.Services.AddScoped<IStateRepository, StateRepository>();
        builder.Services.AddScoped<ICityRepository, CityRepository>();
        builder.Services.AddScoped<ITollStationRepository, TollStationRepository>();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            applicationDbContext.ApplyMigrations();

            //var migrator = applicationDbContext.GetService<IMigrator>();
            //migrator.Migrate("0");
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