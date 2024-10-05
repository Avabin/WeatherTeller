using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using WeatherTeller.Persistence.LiteDb.Settings;
using WeatherTeller.Persistence.Settings;

namespace WeatherTeller.Persistence.LiteDb;

public static class Services
{
    public static void AddLiteDb(this IServiceCollection services, string dbPath)
    {
        services.AddSingleton(_ => CreateDb(dbPath));
        services.AddSingleton<ISettingsDataSource, SettingsLiteDbDataSource>();
    }

    private static ILiteDatabase CreateDb(string dbPath)
    {
        var db = new LiteDatabase(dbPath);
        return db;
    }
}