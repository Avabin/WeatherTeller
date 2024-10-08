using Microsoft.EntityFrameworkCore;
using WeatherTeller.Persistence.Settings;

namespace WeatherTeller.Persistence.EntityFramework.Settings;

internal class EntityFrameworkSettingsDataSource :
    EntityFrameworkDataSourceBase<SettingsEntity, Persistence.Settings.Settings, string>, ISettingsDataSource
{
    public EntityFrameworkSettingsDataSource(IDbContextFactory<ApplicationDbContext> dbContextFactory) : base(
        dbContextFactory)
    {
    }

    protected override Persistence.Settings.Settings ToPersistence(SettingsEntity entity) => entity.ToPersistence();

    protected override SettingsEntity ToEntity(Persistence.Settings.Settings model) => model.ToEntity();
}