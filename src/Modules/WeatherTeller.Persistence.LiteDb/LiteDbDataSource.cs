using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using LiteDB;
using Microsoft.Extensions.Logging;
using WeatherTeller.Persistence.Models;

namespace WeatherTeller.Persistence.LiteDb;

internal class LiteDbDataSource<T, TId>(ILiteDatabase liteDatabase, ILogger<LiteDbDataSource<T, TId>> logger)
    : IDataSource<T, TId> where T : IIdentifiable<TId>, IComparable<T> where TId : IComparable<TId>
{
    private readonly ILogger<LiteDbDataSource<T, TId>> _logger = logger;
    private readonly IScheduler _scheduler = ThreadPoolScheduler.Instance;
    private ILiteCollection<T> Collection => liteDatabase.GetCollection<T>();

    public IAsyncEnumerable<T> Where(Func<T, bool>? predicate = null)
    {
        var observable = Observable.Create<T>((observer, cancellationToken) =>
        {
            foreach (var entity in WhereSync(predicate))
            {
                _logger.LogTrace("Emitting entity {entity}", entity);
                if (cancellationToken.IsCancellationRequested)
                {
                    _logger.LogDebug("Cancellation requested");
                    break;
                }

                if (entity == null)
                {
                    _logger.LogWarning("Entity is null");
                    continue;
                }

                observer.OnNext(entity);
            }

            observer.OnCompleted();
            return Task.CompletedTask;
        });

        return observable.ToAsyncEnumerable();
    }

    public IObservable<TId> Add(T item) => Observable.Start(() =>
    {
        _logger.LogTrace("Adding item {@item}", item);
        Collection.Insert(item);
        return item.Id.Value;
    }, _scheduler);

    public IObservable<TId[]> AddRange(IEnumerable<T> items) =>
        Observable.Start(() =>
        {
            var identifiables = items.ToArray();
            var itemCount = identifiables.Length;
            _logger.LogTrace("Adding items {ItemsCount}", itemCount);
            Collection.InsertBulk(identifiables);
            return identifiables.Select(x => x.Id.Value).ToArray();
        }, _scheduler);

    public IObservable<Unit> UpdateOne(Id<TId> id, Func<T, T> update) =>
        Observable.Start(() =>
        {
            UpdateOneSync(id, update);
            return Unit.Default;
        }, _scheduler);

    public IObservable<Unit> ReplaceOne(Id<TId> id, T item) =>
        Observable.Start(() =>
        {
            ReplaceOneSync(id, item);
            return Unit.Default;
        }, _scheduler);

    public IObservable<Unit> UpdateMany(IEnumerable<Id<TId>> ids, Func<T, T> update) =>
        Observable.Start(() =>
        {
            UpdateManySync(ids, update);
            return Unit.Default;
        }, _scheduler);

    public IObservable<Unit> RemoveOne(Id<TId> id) => Observable.Start(() =>
    {
        RemoveOneSync(id);
        return Unit.Default;
    }, _scheduler);

    public IObservable<Unit> RemoveMany(Func<T, bool> predicate) => Observable.Start(() =>
    {
        RemoveManySync(predicate);
        return Unit.Default;
    }, _scheduler);

    public IObservable<Unit> RemoveAll() => Observable.Start(() =>
    {
        RemoveAllSync();
        return Unit.Default;
    }, _scheduler);

    public IObservable<bool> Contains(Id<TId> id) => Observable.Start(() => ContainsSync(id), _scheduler);

    public IObservable<T?> GetById(Id<TId> id) => Observable.Start(() => GetByIdSync(id), _scheduler);

    private IEnumerable<T?> WhereSync(Func<T, bool>? predicate = null)
    {
        _logger.LogTrace("Querying collection");
        var query = Collection.Query();
        if (predicate != null) query = query.Where(x => predicate(x));

        return query.ToEnumerable();
    }

    private void ReplaceOneSync(Id<TId> id, T item)
    {
        // update
        UpdateOneSync(id, _ => item);
    }

    private void UpdateOneSync(Id<TId> id, Func<T, T> update)
    {
        _logger.LogTrace("Updating item with id {id}", id);
        var entity = GetByIdSync(id);
        if (entity == null) return;

        var updatedEntity = update(entity);
        Collection.Update(updatedEntity);
    }

    private void UpdateManySync(IEnumerable<Id<TId>> ids, Func<T, T> update)
    {
        _logger.LogTrace("Updating items with ids {@ids}", ids);
        foreach (var id in ids)
        {
            _logger.LogTrace("Updating item with id {id}", id);
            UpdateOneSync(id, update);
        }
    }

    private void RemoveManySync(Func<T, bool> predicate)
    {
        _logger.LogTrace("Removing items with predicate");
        Collection.DeleteMany(x => predicate(x));
    }

    private void RemoveOneSync(Id<TId> id)
    {
        var expression = new BsonValue(id.Value);
        _logger.LogTrace("Removing item with id {Expression}", expression);
        Collection.Delete(expression);
    }

    private void RemoveAllSync()
    {
        _logger.LogTrace("Removing all items");
        Collection.DeleteAll();
    }

    private bool ContainsSync(Id<TId> id)
    {
        BsonExpression expression = $"Id = {id.Value}";
        BsonExpression alternativeExpression = $"_id = {id.Value}";
        _logger.LogTrace("Checking if item with id {Expression} | {AlternativeExpression} exists", expression,
            alternativeExpression);
        return Collection.Exists(expression) || Collection.Exists(alternativeExpression);
    }

    private T? GetByIdSync(Id<TId> id)
    {
        BsonExpression expression = $"Id = {id.Value}";
        BsonExpression alternativeExpression = $"_id = {id.Value}";
        _logger.LogTrace("Getting item with id {Expression} | {AlternativeExpression}", expression,
            alternativeExpression);
        return Collection.FindOne(expression) ?? Collection.FindOne(alternativeExpression);
    }
}