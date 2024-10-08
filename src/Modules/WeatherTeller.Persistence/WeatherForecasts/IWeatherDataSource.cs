namespace WeatherTeller.Persistence.WeatherForecasts;

internal interface IWeatherDataSource : IDataSource<WeatherForecastSnapshot, ulong>
{
}