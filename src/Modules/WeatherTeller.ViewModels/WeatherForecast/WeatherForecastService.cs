using System.Reactive.Linq;
using System.Reactive.Subjects;
using DynamicData;
using WeatherTeller.Services.Core.WeatherApi.Models;
using WeatherTeller.ViewModels.WeatherForecast.ForecastDay;

namespace WeatherTeller.ViewModels.WeatherForecast;

internal class WeatherForecastService : IWeatherForecastService
{
    private readonly IWeatherForecastDayViewModelFactory _weatherForecastDayViewModelFactory;
    private readonly IWeatherStateViewModelFactory _weatherStateViewModelFactory;
    public SourceCache<WeatherForecastDayViewModel, DateTimeOffset> WeatherForecast { get; } = new(x => x.Date);
    
    private ISubject<WeatherStateViewModel> _currentWeatherState = new ReplaySubject<WeatherStateViewModel>(1);
    public IObservable<WeatherStateViewModel> CurrentWeatherState => _currentWeatherState.AsObservable();

    public WeatherForecastService(IWeatherForecastDayViewModelFactory weatherForecastDayViewModelFactory, IWeatherStateViewModelFactory weatherStateViewModelFactory)
    {
        _weatherForecastDayViewModelFactory = weatherForecastDayViewModelFactory;
        _weatherStateViewModelFactory = weatherStateViewModelFactory;
    }

    public IObservable<IChangeSet<WeatherForecastDayViewModel, DateTimeOffset>> Connect() => WeatherForecast.Connect();
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

    public void SetCurrentWeatherState(WeatherState state)
    {
        var vm = _weatherStateViewModelFactory.Create(state);
        _currentWeatherState.OnNext(vm);
    }
}