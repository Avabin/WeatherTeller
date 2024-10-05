using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: InternalsVisibleTo("WeatherTeller.Tests")]
[assembly: InternalsVisibleTo("WeatherTeller")]

namespace WeatherTeller.Infrastructure;

public class ServiceLocator
{
    internal static IServiceProvider? Instance { get; set; }

    public static T? Get<T>() => Instance!.GetService<T>();


    public static object? Get(Type type)
    {
        var logger = Instance!.GetRequiredService<ILogger<ServiceLocator>>();
        try
        {
            var service = Instance!.GetService(type);
            if (service is null) logger.LogError("Failed to get service {ServiceType}", type);

            return service;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to get service {ServiceType}", type);
            throw;
        }
    }

    public static T GetRequired<T>() where T : notnull => Instance!.GetRequiredService<T>();
}