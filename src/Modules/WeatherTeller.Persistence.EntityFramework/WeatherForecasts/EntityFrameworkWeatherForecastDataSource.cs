using Microsoft.EntityFrameworkCore;
using WeatherTeller.Persistence.WeatherForecasts;

namespace WeatherTeller.Persistence.EntityFramework.WeatherForecasts;

internal class EntityFrameworkWeatherForecastDataSource : EntityFrameworkDataSourceBase<WeatherForecastEntity, WeatherForecastSnapshot, ulong>, IWeatherDataSource
{
    public EntityFrameworkWeatherForecastDataSource(IDbContextFactory<ApplicationDbContext> dbContextFactory) : base(dbContextFactory)
    {
    }

    protected override WeatherForecastSnapshot ToPersistence(WeatherForecastEntity entity) =>
        entity.ToPersistence();

    protected override WeatherForecastEntity ToEntity(WeatherForecastSnapshot model) =>
        model.ToEntity();
}