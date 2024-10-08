using System.Reactive.Linq;
using System.Reactive.Subjects;
using DynamicData;
using MediatR;
using WeatherTeller.Services.Core.WeatherApi.Models;
using WeatherTeller.Services.Core.WeatherForecasts.Requests;
using WeatherTeller.ViewModels.WeatherForecast.ForecastDay;

namespace WeatherTeller.ViewModels.WeatherForecast;

internal class WeatherForecastService : IWeatherForecastService
{
    private readonly IMediator _mediator;
    private readonly IWeatherForecastDayViewModelFactory _weatherForecastDayViewModelFactory;
    private readonly IWeatherStateViewModelFactory _weatherStateViewModelFactory;

    private readonly ISubject<WeatherStateViewModel> _currentWeatherState = new ReplaySubject<WeatherStateViewModel>(1);

    public WeatherForecastService(IWeatherForecastDayViewModelFactory weatherForecastDayViewModelFactory,
        IWeatherStateViewModelFactory weatherStateViewModelFactory, IMediator mediator)
    {
        _weatherForecastDayViewModelFactory = weatherForecastDayViewModelFactory;
        _weatherStateViewModelFactory = weatherStateViewModelFactory;
        _mediator = mediator;
    }

    public SourceCache<WeatherForecastDayViewModel, DateOnly> WeatherForecast { get; } = new(x => x.Date);
    public IObservable<WeatherStateViewModel> CurrentWeatherState => _currentWeatherState.AsObservable();

    public IObservable<IChangeSet<WeatherForecastDayViewModel, DateOnly>> Connect() => WeatherForecast.Connect();

    public void Add(WeatherForecastDay forecastDay)
    {
        var vm = _weatherForecastDayViewModelFactory.Create(forecastDay);
        WeatherForecast.AddOrUpdate(vm);
    }

    public void AddRange(IEnumerable<WeatherForecastDay> forecastDays)
    {
        var vms = forecastDays.Select(_weatherForecastDayViewModelFactory.Create);
        WeatherForecast.AddOrUpdate(vms);
    }

    public async Task Refresh()
    {
        await _mediator.Send(new RefreshWeatherForecastCommand());
    }

    public void SetCurrentWeatherState(WeatherState state)
    {
        var vm = _weatherStateViewModelFactory.Create(state);
        _currentWeatherState.OnNext(vm);
    }
}