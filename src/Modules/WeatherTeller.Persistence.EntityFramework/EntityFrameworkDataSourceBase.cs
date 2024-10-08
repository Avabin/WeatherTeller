using System.Reactive;
using System.Reactive.Linq;
using Microsoft.EntityFrameworkCore;
using WeatherTeller.Persistence.Models;

namespace WeatherTeller.Persistence.EntityFramework;

internal abstract class EntityFrameworkDataSourceBase<TEntity, T, TId> : IDataSource<T, TId> where TId : IComparable<TId> where T : IIdentifiable<TId> where TEntity : class, IHasId<TId>
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
    private ApplicationDbContext DbContext() => _dbContextFactory.CreateDbContext();
    
    protected abstract T ToPersistence(TEntity entity);
    protected abstract TEntity ToEntity(T model);

    public EntityFrameworkDataSourceBase(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public IAsyncEnumerable<T> Where(Func<T, bool>? predicate = null)
    {
        // get async stream of settings
        var asyncEnumerable = DbContext().Set<TEntity>().AsAsyncEnumerable().Select(ToPersistence);
        
        return predicate is null ?
            // apply predicate if provided
            asyncEnumerable :
            // apply predicate if provided
            asyncEnumerable.Where(predicate);
    }

    public IObservable<TId> Add(T item) =>
        Observable.FromAsync(async token =>
        {
            var dbContext = DbContext();
            var entry = await dbContext.Set<TEntity>().AddAsync(ToEntity(item), token);
            await dbContext.SaveChangesAsync(token);
            var id = entry.Entity.Id;
            return id;
        });

    public IObservable<TId[]> AddRange(IEnumerable<T> items) =>
        Observable.FromAsync(async token =>
        {
            var dbContext = DbContext();
            await dbContext.Set<TEntity>().AddRangeAsync(items.Select(ToEntity), token);
            await dbContext.SaveChangesAsync(token);
            var entries = dbContext.ChangeTracker.Entries<TEntity>().ToList();
            var filteredEntries = entries.Where(x => x.State == EntityState.Added).ToList();
            var ids = filteredEntries.Select(x => x.Entity.Id).ToArray();
            return ids;
        });

    public IObservable<Unit> UpdateOne(Id<TId> id, Func<T, T> update) =>
        Observable.FromAsync(async token =>
        {
            var dbContext = DbContext();
            var set = dbContext.Set<TEntity>();
            var idValue = id.Value;
            var item = await set.FirstOrDefaultAsync(x => x.Id.Equals(idValue), token);
            if (item is null)
            {
                return;
            }

            var persistence = ToPersistence(item);
            var updatedItem = update(persistence);
            var updatedEntity = ToEntity(updatedItem);
            set.Update(updatedEntity);
            await dbContext.SaveChangesAsync(token);
        });

    public IObservable<Unit> ReplaceOne(Id<TId> id, T item) =>
        Observable.FromAsync(async token =>
        {
            var dbContext = DbContext();
            var set = dbContext.Set<TEntity>();
            var idValue = id.Value;
            var existingItem = await set.FirstOrDefaultAsync(x => x.Id.Equals(idValue), token);
            if (existingItem is not null)
            {
                set.Remove(existingItem);
                await dbContext.SaveChangesAsync(token);
            }

            await set.AddAsync(ToEntity(item), token);
            await dbContext.SaveChangesAsync(token);
        });

    public IObservable<Unit> UpdateMany(IEnumerable<Id<TId>> ids, Func<T, T> update) =>
        Observable.FromAsync(async token =>
        {
            var dbContext = DbContext();
            var set = dbContext.Set<TEntity>();
            var idValues = ids.Select(x => x.Value).ToArray();
            var items = await set.Where(x => idValues.Contains(x.Id)).ToListAsync(token);
            foreach (var item in items)
            {
                var persistence = ToPersistence(item);
                var updatedItem = update(persistence);
                var updatedEntity = ToEntity(updatedItem);
                set.Update(updatedEntity);
            }

            await dbContext.SaveChangesAsync(token);
        });

    public IObservable<Unit> RemoveOne(Id<TId> id) =>
        Observable.FromAsync(async token =>
        {
            var dbContext = DbContext();
            var set = dbContext.Set<TEntity>();
            var idValue = id.Value;
            var item = await set.FirstOrDefaultAsync(x => x.Id.Equals(idValue), token);
            if (item is not null)
            {
                set.Remove(item);
                await dbContext.SaveChangesAsync(token);
            }
        });

    public IObservable<Unit> RemoveAll() =>
        Observable.FromAsync(async token =>
        {
            var dbContext = DbContext();
            var set = dbContext.Set<TEntity>();
            set.RemoveRange(await set.ToListAsync(token));
            await dbContext.SaveChangesAsync(token);
        });

    public IObservable<bool> Contains(Id<TId> id) =>
        Observable.FromAsync(async token =>
        {
            var dbContext = DbContext();
            var set = dbContext.Set<TEntity>();
            var idValue = id.Value;
            return await set.AnyAsync(x => x.Id.Equals(idValue), token);
        });

    public IObservable<T?> GetById(Id<TId> id) =>
        Observable.FromAsync(async token =>
        {
            var dbContext = DbContext();
            var set = dbContext.Set<TEntity>();
            var idValue = id.Value;
            var item = await set.FirstOrDefaultAsync(x => x.Id.Equals(idValue), token);
            return item is null ? default : ToPersistence(item);
        });
}