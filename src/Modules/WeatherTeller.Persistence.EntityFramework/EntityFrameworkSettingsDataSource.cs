using System.Reactive;
using System.Reactive.Linq;
using Microsoft.EntityFrameworkCore;
using WeatherTeller.Persistence.EntityFramework.Settings;
using WeatherTeller.Persistence.Models;
using WeatherTeller.Persistence.Settings;

namespace WeatherTeller.Persistence.EntityFramework;

internal class EntityFrameworkSettingsDataSource : ISettingsDataSource
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
    private ApplicationDbContext DbContext() => _dbContextFactory.CreateDbContext();

    public EntityFrameworkSettingsDataSource(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public IAsyncEnumerable<Persistence.Settings.Settings> Where(Func<Persistence.Settings.Settings, bool>? predicate = null)
    {
        // get async stream of settings
        var asyncEnumerable = DbContext().Settings.AsAsyncEnumerable().Select(x => x.ToPersistence());
        
        return predicate is null ?
            // apply predicate if provided
            asyncEnumerable :
            // apply predicate if provided
            asyncEnumerable.Where(predicate);
    }

    public IObservable<Unit> Add(Persistence.Settings.Settings item) => 
        Observable.FromAsync(async token =>
        {
            var dbContext = DbContext();
            await dbContext.Settings.AddAsync(item.ToEntity(), token);
            await dbContext.SaveChangesAsync(token);
        });

    public IObservable<Unit> AddRange(IEnumerable<Persistence.Settings.Settings> items) => 
        Observable.FromAsync(async token =>
        {
            var dbContext = DbContext();
            await dbContext.Settings.AddRangeAsync(items.Select(x => x.ToEntity()), token);
            await dbContext.SaveChangesAsync(token);
        });

    public IObservable<Unit> UpdateOne(Id<string> id, Func<Persistence.Settings.Settings, Persistence.Settings.Settings> update) => 
        Observable.FromAsync(async token =>
        {
            var dbContext = DbContext();
            var idValue = id.Value;
            var item = await dbContext.Settings.AsQueryable().FirstOrDefaultAsync(x => x.Id == idValue, token);
            if (item is null)
            {
                return;
            }

            var updatedItem = update(item.ToPersistence()).ToEntity();
            dbContext.Settings.Update(updatedItem);
            await dbContext.SaveChangesAsync(token);
        });

    public IObservable<Unit> ReplaceOne(Id<string> id, Persistence.Settings.Settings item) =>
        Observable.FromAsync(async token =>
        {
            var dbContext = DbContext();
            var idValue = id.Value;
            var existingItem = await dbContext.Settings.FirstOrDefaultAsync(x => x.Id == idValue, token);
            if (existingItem is not null)
            {
                dbContext.Settings.Remove(existingItem);
            }

            await dbContext.Settings.AddAsync(item.ToEntity(), token);
            await dbContext.SaveChangesAsync(token);
        });

    public IObservable<Unit> UpdateMany(IEnumerable<Id<string>> ids, Func<Persistence.Settings.Settings, Persistence.Settings.Settings> update)
    {
        return Observable.FromAsync(async token =>
        {
            var dbContext = DbContext();
            var idValues = ids.Select(x => x.Value).ToList();
            var items = await dbContext.Settings.Where(x => idValues.Contains(x.Id))
                .Select(x => x.ToPersistence())
                .ToListAsync(token);
            foreach (var updatedItem in items.Select(update))
            {
                dbContext.Settings.Update(updatedItem.ToEntity());
            }

            await dbContext.SaveChangesAsync(token);
        });
    }

    public IObservable<Unit> RemoveOne(Id<string> id) => 
        Observable.FromAsync(async token =>
        {
            var dbContext = DbContext();
            var idValue = id.Value;
            var item = await dbContext.Settings.AsQueryable().FirstOrDefaultAsync(x => x.Id == idValue, token);
            if (item is not null)
            {
                dbContext.Settings.Remove(item);
                await dbContext.SaveChangesAsync(token);
            }
        });

    public IObservable<Unit> RemoveAll() => 
        Observable.FromAsync(async token =>
        {
            var dbContext = DbContext();
            dbContext.Settings.RemoveRange(await dbContext.Settings.ToListAsync(token));
            await dbContext.SaveChangesAsync(token);
        });

    public IObservable<bool> Contains(Id<string> id) => 
        Observable.FromAsync(async token =>
        {
            var dbContext = DbContext();
            var idValue = id.Value;
            return await dbContext.Settings.AnyAsync(x => x.Id == idValue, token);
        });

    public IObservable<Persistence.Settings.Settings?> GetById(Id<string> id) => 
        Observable.FromAsync(async token =>
        {
            var dbContext = DbContext();
            var idValue = id.Value;
            var settingsEntity = await dbContext.Settings.FirstOrDefaultAsync(x => x.Id == idValue, token);
            return settingsEntity?.ToPersistence();
        });
}