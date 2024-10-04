using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using LiteDB;
using Microsoft.Extensions.Logging;
using WeatherTeller.Persistence.Models;

namespace WeatherTeller.Persistence.LiteDb;

internal class LiteDbDataSource<T, TId>(ILiteDatabase liteDatabase, ILogger<LiteDbDataSource<T, TId>> logger)
    : IDataSource<T, TId> where T : IIdentifiable<TId>, IComparable<T> where TId : IComparable<TId>
{
    private readonly ILogger<LiteDbDataSource<T, TId>> _logger = logger;
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

    public IObservable<Unit> Add(T item) => Observable.Start(() =>
    {
        _logger.LogTrace("Adding item {@item}", item);
        Collection.Insert(item);
        return Unit.Default;
    });

    private IEnumerable<T?> WhereSync(Func<T, bool>? predicate = null)
    {
        _logger.LogTrace("Querying collection");
        var query = Collection.Query();
        if (predicate != null)
        {
            query = query.Where(x => predicate(x));
        }

        return query.ToEnumerable();
    }

    public IObservable<Unit> AddRange(IEnumerable<T> items) =>
        Observable.Start(() =>
        {
            _logger.LogTrace("Adding items {@items}", items);
            Collection.InsertBulk(items);
            return Unit.Default;
        });

    public IObservable<Unit> UpdateOne(Id<TId> id, Func<T, T> update) =>
        Observable.Start(() =>
        {
            UpdateOneSync(id, update);
            return Unit.Default;
        });

    public IObservable<Unit> ReplaceOne(Id<TId> id, T item) =>
        Observable.Start(() =>
        {
            ReplaceOneSync(id, item);
            return Unit.Default;
        });

    private void ReplaceOneSync(Id<TId> id, T item)
    {
        // update
        UpdateOneSync(id, _ => item);
    }

    private void UpdateOneSync(Id<TId> id, Func<T, T> update)
    {
        _logger.LogTrace("Updating item with id {id}", id);
        var entity = GetByIdSync(id);
        if (entity == null)
        {
            return;
        }

        var updatedEntity = update(entity);
        Collection.Update(updatedEntity);
    }

    public IObservable<Unit> UpdateMany(IEnumerable<Id<TId>> ids, Func<T, T> update) =>
        Observable.Start(() =>
        {
            UpdateManySync(ids, update);
            return Unit.Default;
        });

    private void UpdateManySync(IEnumerable<Id<TId>> ids, Func<T, T> update)
    {
        _logger.LogTrace("Updating items with ids {@ids}", ids);
        foreach (var id in ids)
        {
            _logger.LogTrace("Updating item with id {id}", id);
            UpdateOneSync(id, update);
        }
    }

    public IObservable<Unit> RemoveOne(Id<TId> id) => Observable.Start(() =>
    {
        RemoveOneSync(id);
        return Unit.Default;
    });

    private void RemoveOneSync(Id<TId> id)
    {
        var expression = new BsonValue(id.Value);
        _logger.LogTrace("Removing item with id {Expression}", expression);
        Collection.Delete(expression);
    }

    public IObservable<Unit> RemoveAll() => Observable.Start(() =>
    {
        RemoveAllSync();
        return Unit.Default;
    });
    private void RemoveAllSync()
    {
        _logger.LogTrace("Removing all items");
        Collection.DeleteAll();
    }

    public IObservable<bool> Contains(Id<TId> id) => Observable.Start(() => ContainsSync(id));

    private bool ContainsSync(Id<TId> id)
    {
        BsonExpression expression = $"Id = {id.Value}";
        BsonExpression alternativeExpression = $"_id = {id.Value}";
        _logger.LogTrace("Checking if item with id {Expression} | {AlternativeExpression} exists", expression, alternativeExpression);
        return Collection.Exists(expression) || Collection.Exists(alternativeExpression);
    }

    public IObservable<T?> GetById(Id<TId> id) => Observable.Start(() => GetByIdSync(id));

    private T? GetByIdSync(Id<TId> id)
    {
        BsonExpression expression = $"Id = {id.Value}";
        BsonExpression alternativeExpression = $"_id = {id.Value}";
        _logger.LogTrace("Getting item with id {Expression} | {AlternativeExpression}", expression, alternativeExpression);
        return Collection.FindOne(expression) ?? Collection.FindOne(alternativeExpression);
    }
}