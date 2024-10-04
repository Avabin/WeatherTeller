using System.Collections.Immutable;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using WeatherTeller.Services.Core.WeatherApi.Models;

namespace WeatherTeller.Services.Core.WeatherApi
{
    public abstract class WeatherApiBase : IWeatherApi
    {
        protected ReplaySubject<WeatherState> CurrentSubject { get; } = new(1);
        public IObservable<WeatherState> Current => CurrentSubject.AsObservable();
    
        protected ReplaySubject<WeatherForecastDay> TomorrowSubject { get; } = new(1);
        public IObservable<WeatherForecastDay> Tomorrow => TomorrowSubject.AsObservable();
    
        protected ReplaySubject<ImmutableList<WeatherForecastDay>> DaysSubject { get; } = new(1);
        public IObservable<ImmutableList<WeatherForecastDay>> Days => DaysSubject.AsObservable();

        public abstract Task SetLocation(double latitude, double longitude);

        public abstract Task Refresh();
    }
}