using System.Collections.Immutable;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using MediatR;
using Microsoft.Extensions.Logging;
using WeatherTeller.Services.Core.WeatherApi.Models;
using WeatherTeller.Services.Core.WeatherForecasts.Requests;

namespace WeatherTeller.Services.Core.WeatherApi;

public abstract class WeatherApiBase : IWeatherApi
{
    private readonly ILogger<WeatherApiBase> _logger;
    private readonly IMediator _mediator;

    protected WeatherApiBase(ILogger<WeatherApiBase> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public IObservable<WeatherForecastDay> Current => DaysSubject
        .Select(x => x[0])
        .Where(x => x.IsToday);
    
    protected ReplaySubject<ImmutableList<WeatherForecastDay>> DaysSubject { get; } = new(1);
    public IObservable<ImmutableList<WeatherForecastDay>> Days => DaysSubject.AsObservable();

    public abstract Task SetLocation(double latitude, double longitude);

    public abstract Task Refresh();

    protected async Task HandleResult(WeatherForecast forecast)
    {
        var days = forecast.Days.ToImmutableList();
        await PersistForecast(forecast);
        
        _logger.LogDebug("Publishing days forecast to Subject");
        DaysSubject.OnNext(days);
    }
    
    
    private async Task PersistForecast(WeatherForecast forecast)
    {
        _logger.LogDebug("Persisting forecast");
        var id = await _mediator.Send(new PersistWeatherForecastCommand(forecast));
        _logger.LogDebug("Forecast persisted with id {Id}", id);
    }
}