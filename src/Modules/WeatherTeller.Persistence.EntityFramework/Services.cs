using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherTeller.Persistence.Settings;

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

        return services;
    }
}