using System.Collections.Immutable;
using System.Reactive.Linq;
using WeatherTeller.Services.Core.WeatherApi;
using WeatherTeller.Services.WeatherApiCom.Extensions;
using WeatherTeller.Services.WeatherApiCom.Interfaces;

namespace WeatherTeller.Services.WeatherApiCom;

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
        
        CurrentSubject.OnNext(current.ToCoreModel(location));
        TomorrowSubject.OnNext(forecastModel.Days[0]);
        DaysSubject.OnNext(forecastModel.Days[1..].ToImmutableList());
    }
}