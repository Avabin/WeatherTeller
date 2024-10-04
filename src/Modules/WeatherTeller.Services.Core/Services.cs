using Microsoft.Extensions.DependencyInjection;

namespace WeatherTeller.Services.Core
{
    public static class Services
    {
        public static IServiceCollection AddWeatherApiCore(this IServiceCollection services)
        {
            return services;
        }
    }
}