using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherTeller.Persistence.EntityFramework.Settings;
using WeatherTeller.Persistence.EntityFramework.WeatherForecasts;
using WeatherTeller.Persistence.Settings;
using WeatherTeller.Persistence.WeatherForecasts;

namespace WeatherTeller.Persistence.EntityFramework;

public static class Services
{
    public static IServiceCollection AddWeatherTellerSqlite(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("Sqlite");
        services.AddDbContextFactory<ApplicationDbContext>(o => o.UseSqlite(connectionString));

        services.AddTransient<ISettingsDataSource, EntityFrameworkSettingsDataSource>();

        return services;
    }
    
    // overload that requires only the db file path
    public static IServiceCollection AddWeatherTellerSqlite(this IServiceCollection services, string dbFilePath)
    {
        services.AddDbContextFactory<ApplicationDbContext>(options => options.UseSqlite($"Data Source={dbFilePath}"));

        services.AddTransient<ISettingsDataSource, EntityFrameworkSettingsDataSource>();
        services.AddTransient<IWeatherDataSource, EntityFrameworkWeatherForecastDataSource>();
        

        return services;
    }
    
    public static IServiceCollection AddWeatherTellerInMem(this IServiceCollection services)
    {
        services.AddDbContextFactory<ApplicationDbContext>(options => options.UseInMemoryDatabase("WeatherTeller"));

        services.AddTransient<ISettingsDataSource, EntityFrameworkSettingsDataSource>();
        services.AddTransient<IWeatherDataSource, EntityFrameworkWeatherForecastDataSource>();
        

        return services;
    }
}