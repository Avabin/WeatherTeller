using BuilderGenerator;

namespace WeatherTeller.Services.Core.WeatherApi.Models;

[BuilderFor(typeof(WeatherState))]
public partial class WeatherStateBuilder
{
    // WithLocation that accepts a Action<WeatherLocationBuilder> to build the WeatherLocation
    public WeatherStateBuilder WithLocation(Action<WeatherLocationBuilder> build) =>
        WithLocation(() =>
        {
            var locationBuilder = new WeatherLocationBuilder();
            build(locationBuilder);
            return locationBuilder.Build();
        });
    
    // WithLocation that accepts a WeatherLocationBuilder to build the WeatherLocation
    public WeatherStateBuilder WithLocation(WeatherLocationBuilder locationBuilder)
    {
        var location = locationBuilder.Build();
     
        return this.WithLocation(location);
    }
}