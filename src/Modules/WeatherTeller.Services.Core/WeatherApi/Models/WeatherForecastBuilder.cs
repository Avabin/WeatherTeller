using BuilderGenerator;

namespace WeatherTeller.Services.Core.WeatherApi.Models;

[BuilderFor(typeof(WeatherForecast))]
public partial class WeatherForecastBuilder
{
    // WithDays that accepts a Action<WeatherForecastDayBuilder> to build the WeatherForecastDay
    public WeatherForecastBuilder WithDays(Action<WeatherForecastDayBuilder> build) =>
        WithDays(() =>
        {
            var dayBuilder = new WeatherForecastDayBuilder();
            build(dayBuilder);
            return [dayBuilder.Build()];
        });

    // WithDays that accepts a WeatherForecastDayBuilder to build the WeatherForecastDay
    public WeatherForecastBuilder WithDays(WeatherForecastDayBuilder dayBuilder) =>
        WithDays(() => [dayBuilder.Build()]);

    // WithDays that accepts a IEnumerable<WeatherForecastDay> to build the WeatherForecastDay
    public WeatherForecastBuilder WithDays(IEnumerable<WeatherForecastDay> days) => WithDays(days.ToList);

    // WithLocation that accepts a Action<WeatherLocationBuilder> to build the WeatherLocation
    public WeatherForecastBuilder WithLocation(Action<WeatherLocationBuilder> build) =>
        WithLocation(() =>
        {
            var locationBuilder = new WeatherLocationBuilder();
            build(locationBuilder);
            return locationBuilder.Build();
        });

    // WithLocation that accepts a WeatherLocationBuilder to build the WeatherLocation
    public WeatherForecastBuilder WithLocation(WeatherLocationBuilder locationBuilder) =>
        WithLocation(locationBuilder.Build);
}