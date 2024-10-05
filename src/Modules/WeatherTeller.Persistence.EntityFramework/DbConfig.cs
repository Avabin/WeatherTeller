using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WeatherTeller.Persistence.EntityFramework;

public static class DbConfig
{
    public static void EnsureDbCreated(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
        var dbContext = dbContextFactory.CreateDbContext();
        dbContext.Database.EnsureCreated();
    }
}