using Microsoft.Extensions.DependencyInjection;

namespace WeatherTeller.ViewModels;

public static class Services
{
    // AddViewModelsMediatR
    public static IServiceCollection AddViewModelsMediatR(this IServiceCollection services)
    {
        services.AddMediatR(o => o.RegisterServicesFromAssemblyContaining<ViewModelsModule>());
        return services;
    }
}