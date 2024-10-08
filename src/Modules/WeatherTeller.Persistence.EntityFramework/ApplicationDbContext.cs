using Microsoft.EntityFrameworkCore;
using WeatherTeller.Persistence.EntityFramework.Settings;
using WeatherTeller.Persistence.EntityFramework.WeatherForecasts;

namespace WeatherTeller.Persistence.EntityFramework;

internal class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<SettingsEntity> Settings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SettingsEntity>().HasKey(x => x.Id);
        modelBuilder.Entity<SettingsEntity>().OwnsOne(x => x.Location);
        modelBuilder.Entity<SettingsEntity>().Property(x => x.ApiKey).IsRequired(false);
        
        
        modelBuilder.Entity<WeatherForecastEntity>().HasKey(x => x.Id);
        modelBuilder.Entity<WeatherForecastEntity>().OwnsMany(x => x.Days, x =>
        {
            x.HasKey(y => y.Id);
            x.OwnsOne(y => y.State);
            x.Property(y => y.Date);
        });
        modelBuilder.Entity<WeatherForecastEntity>().Property(x => x.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<WeatherForecastEntity>().OwnsOne(x => x.Location, x =>
        {
            x.Property(y => y.City);
            x.Property(y => y.Country);
            x.Property(y => y.Latitude);
            x.Property(y => y.Longitude);
        });

        
        base.OnModelCreating(modelBuilder);
    }
}