using System.Reactive.Linq;
using MediatR;
using Microsoft.Extensions.Logging;
using WeatherTeller.Services.Core.WeatherApi;
using WeatherTeller.Services.WeatherApiCom.Client.Interfaces;
using WeatherTeller.Services.WeatherApiCom.Extensions;

namespace WeatherTeller.Services.WeatherApiCom.Client;

internal class WeatherApiComWeatherApi(
    IWeatherApiComClient weatherApiComClient,
    IMediator mediator,
    ILogger<WeatherApiComWeatherApi> logger
) : WeatherApiBase(logger, mediator)
{
    private readonly ILogger<WeatherApiComWeatherApi> _logger = logger;
    private readonly IMediator _mediator = mediator;
    private readonly IWeatherApiComClient _weatherApiComClient = weatherApiComClient;

    public override async Task SetLocation(double latitude, double longitude) =>
        await _weatherApiComClient.SetLocation(latitude, longitude);

    public override async Task Refresh()
    {
        _logger.LogInformation("Refreshing weather data from WeatherApiCom");
        await _weatherApiComClient.Refresh();
        var location = await _weatherApiComClient.Current.Location.FirstAsync();
        var forecast = await _weatherApiComClient.Forecast.Forecast.FirstAsync();

        var forecastModel = forecast.ToCoreModel(location);

        await HandleResult(forecastModel);
    }
}