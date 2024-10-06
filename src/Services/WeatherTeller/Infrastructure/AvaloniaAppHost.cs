using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherTeller.Essentials;
using WeatherTeller.Services;
using WeatherTeller.Services.WeatherApiCom;
using WeatherTeller.Services.WeatherApiCom.Models;
using WeatherTeller.ViewModels;

namespace WeatherTeller.Infrastructure;

public class AvaloniaAppHost : AppHost
{
    protected override void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
    {
        base.ConfigureServices(ctx, services);
        var weatherApiCom = ctx.Configuration.GetSection(WeatherApiComClientOptions.SectionName);

        services.AddWeatherApiCom(weatherApiCom).AddServices().AddViewModelsMediatR();

        services.AddEssentials();
    }
}