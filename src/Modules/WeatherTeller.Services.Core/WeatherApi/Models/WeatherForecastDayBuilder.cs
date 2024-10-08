using BuilderGenerator;

namespace WeatherTeller.Services.Core.WeatherApi.Models;

[BuilderFor(typeof(WeatherForecastDay))]
public partial class WeatherForecastDayBuilder
{
    // WithWeatherState that accepts an Action<WeatherStateBuilder> to build the WeatherState
    public WeatherForecastDayBuilder WithState(Action<WeatherStateBuilder> build) =>
        WithState(() =>
        {
            var stateBuilder = new WeatherStateBuilder();
            build(stateBuilder);
            return stateBuilder.Build();
        });

    // WithWeatherState that accepts a WeatherStateBuilder to build the WeatherState
    public WeatherForecastDayBuilder WithState(WeatherStateBuilder stateBuilder) => WithState(stateBuilder.Build);

    // WithDate that accepts a DateTime to build the DateOnly
    public WeatherForecastDayBuilder WithDate(DateTime date) => WithDate(() => DateOnly.FromDateTime(date));

    // WithDate that accepts a DateTimeOffset to build the DateOnly
    public WeatherForecastDayBuilder WithDate(DateTimeOffset date) => WithDate(() => DateOnly.FromDateTime(date.Date));
}