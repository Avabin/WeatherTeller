using System.Collections.Immutable;
using System.Reactive.Linq;
using WeatherTeller.Services.Core.WeatherApi;
using WeatherTeller.Services.WeatherApiCom.Client.Interfaces;
using WeatherTeller.Services.WeatherApiCom.Extensions;

namespace WeatherTeller.Services.WeatherApiCom.Client;

internal class WeatherApiComWeatherApi(IWeatherApiComClient weatherApiComClient) : WeatherApiBase
{
    private readonly IWeatherApiComClient _weatherApiComClient = weatherApiComClient;

    public override async Task SetLocation(double latitude, double longitude) => 
        await _weatherApiComClient.SetLocation(latitude, longitude);

    public override async Task Refresh()
    {
        await _weatherApiComClient.Refresh();
        var location = await _weatherApiComClient.Current.Location.FirstAsync();
        var current = await _weatherApiComClient.Current.CurrentWeather.FirstAsync();
        var forecast = await _weatherApiComClient.Forecast.Forecast.FirstAsync();

        var forecastModel = forecast.ToCoreModel(location: location);
        var days = forecastModel.Days.Select(x => x with { State = x.State with { Location = location.Name } }).ToList();
        
        CurrentSubject.OnNext(current.ToCoreModel(location));
        DaysSubject.OnNext(days.ToImmutableList());
    }
}