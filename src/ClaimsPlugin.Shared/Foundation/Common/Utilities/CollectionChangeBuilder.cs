using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;
using Microsoft.Extensions.Localization;
namespace ClaimsPlugin.Shared.Foundation.Common.Utilities;
public class CollectionChangeBuilder<TSource, TCompare>
{
    private readonly HashSet<TCompare>? _hashedCompares;
    private readonly IReadOnlyCollection<TSource> _source;
    public CollectionChangeBuilder(
        IReadOnlyCollection<TSource> source,
        IEnumerable<TCompare> compare
    )
    {
        _hashedCompares = compare.Distinct().ToHashSet();
        _source = source;
    }
    public void ApplyChanges<T>(
        Func<TSource, T> sourceKeySelector,
        Func<TCompare, T?> compareKeySelector,
        Action<TSource> deleteAction,
        Action<TCompare> addAction,
        Action<TSource, TCompare> updateAction,
        out List<TCompare> notFoundList
    )
        where T : struct
    {
        DeleteOperation(sourceKeySelector, compareKeySelector, deleteAction);
        UpdateOperation(sourceKeySelector, compareKeySelector, updateAction, out notFoundList);
        AddOperation(compareKeySelector, addAction);
    }
    public string[]? ApplyChanges<T>(
        Func<TSource, T> sourceKeySelector,
        Func<TCompare, T?> compareKeySelector,
        Action<TSource> deleteAction,
        Action<TCompare> addAction,
        Func<TSource, TCompare, string[]?> updateAction,
        out List<TCompare> notFoundList
    )
        where T : struct
    {
        DeleteOperation(sourceKeySelector, compareKeySelector, deleteAction);
        string[]? updateResult = UpdateOperation(
            sourceKeySelector,
            compareKeySelector,
            updateAction,
            out notFoundList
        );
        if (updateResult.IsNotNullOrEmpty())
        {
            return updateResult;
        }
        AddOperation(compareKeySelector, addAction);
        return null;
    }
    public string[]? ApplyChanges<T>(
        Func<TSource, T> sourceKeySelector,
        Func<TCompare, T?> compareKeySelector,
        Action<TSource> deleteAction,
        Action<TCompare> addAction,
        Func<TSource, TCompare, LocalizedString[]?> updateAction,
        out List<TCompare> notFoundList
    )
        where T : struct
    {
        DeleteOperation(sourceKeySelector, compareKeySelector, deleteAction);
        string[]? updateResult = UpdateOperation(
            sourceKeySelector,
            compareKeySelector,
            updateAction,
            out notFoundList
        );
        if (updateResult.IsNotNullOrEmpty())
        {
            return updateResult.Select(x => x.ToString()).ToArray();
        }
        AddOperation(compareKeySelector, addAction);
        return null;
    }
    public void ApplyChanges<T>(
        Func<TSource, T> sourceKeySelector,
        Func<TCompare, T> compareKeySelector,
        Action<TSource> deleteAction,
        Action<TCompare> addAction,
        Action<TSource, TCompare> updateAction
    )
    {
        DeleteOperation(sourceKeySelector, compareKeySelector, deleteAction);
        UpdateOperation(sourceKeySelector, compareKeySelector, updateAction);
        AddOperation(sourceKeySelector, compareKeySelector, addAction);
    }
    public void ApplyChanges<T>(
        Func<TSource, T> sourceKeySelector,
        Func<TCompare, T> compareKeySelector,
        Action<TSource> deleteAction,
        Action<TCompare> addAction
    )
    {
        DeleteOperation(sourceKeySelector, compareKeySelector, deleteAction);
        AddOperation(sourceKeySelector, compareKeySelector, addAction);
    }
    private void DeleteOperation<T>(
        Func<TSource, T> sourceKeySelector,
        Func<TCompare, T?> compareKeySelector,
        Action<TSource> action
    )
        where T : struct
    {
        if (_hashedCompares.IsNull())
        {
            return;
        }
        foreach (
            TSource source in _source
                .Where(
                    x =>
                        _hashedCompares
                            .Where(k => compareKeySelector(k).IsNotNull())
                            .All(k => !sourceKeySelector(x).Equals(compareKeySelector(k)))
                )
                .ToList()
        )
        {
            action(source);
        }
    }
    private void DeleteOperation<T>(
        Func<TSource, T> sourceKeySelector,
        Func<TCompare, T> compareKeySelector,
        Action<TSource> action
    )
    {
        if (_hashedCompares.IsNull())
        {
            return;
        }
        foreach (
            TSource source in _source
                .Where(
                    x =>
                        _hashedCompares
                            .Where(k => compareKeySelector(k).IsNotNull())
                            .All(k => !sourceKeySelector(x).Equals(compareKeySelector(k)))
                )
                .ToList()
        )
        {
            action(source);
        }
    }
    private void UpdateOperation<T>(
        Func<TSource, T> sourceKeySelector,
        Func<TCompare, T?> compareKeySelector,
        Action<TSource, TCompare> action,
        out List<TCompare> notFoundList
    )
        where T : struct
    {
        notFoundList = new List<TCompare>();
        if (_hashedCompares.IsNull())
        {
            return;
        }
        notFoundList = _hashedCompares
            .Where(
                x =>
                    compareKeySelector(x) is not null
                    && _source.All(k => !sourceKeySelector(k).Equals(compareKeySelector(x)))
            )
            .ToList();
        if (!notFoundList.Any())
        {
            foreach (
                TSource source in _source.Where(
                    x =>
                        _hashedCompares.Any(
                            k =>
                                compareKeySelector(k) is not null
                                && sourceKeySelector(x).Equals(compareKeySelector(k))
                        )
                )
            )
            {
                TCompare compare = _hashedCompares.First(
                    x => compareKeySelector(x).Equals(sourceKeySelector(source))
                );
                action(source, compare);
            }
        }
    }
    private string[]? UpdateOperation<T>(
        Func<TSource, T> sourceKeySelector,
        Func<TCompare, T?> compareKeySelector,
        Func<TSource, TCompare, string[]?> action,
        out List<TCompare> notFoundList
    )
        where T : struct
    {
        notFoundList = new List<TCompare>();
        if (_hashedCompares.IsNull())
        {
            return null;
        }
        notFoundList = _hashedCompares
            .Where(
                x =>
                    compareKeySelector(x) is not null
                    && _source.All(k => !sourceKeySelector(k).Equals(compareKeySelector(x)))
            )
            .ToList();
        if (!notFoundList.Any())
        {
            foreach (
                TSource source in _source.Where(
                    x =>
                        _hashedCompares.Any(
                            k =>
                                compareKeySelector(k) is not null
                                && sourceKeySelector(x).Equals(compareKeySelector(k))
                        )
                )
            )
            {
                TCompare compare = _hashedCompares.First(
                    x => compareKeySelector(x).Equals(sourceKeySelector(source))
                );
                string[]? result = action(source, compare);
                if (result.IsNotNullOrEmpty())
                {
                    return result;
                }
            }
        }
        return null;
    }
    private string[]? UpdateOperation<T>(
        Func<TSource, T> sourceKeySelector,
        Func<TCompare, T?> compareKeySelector,
        Func<TSource, TCompare, LocalizedString[]?> action,
        out List<TCompare> notFoundList
    )
        where T : struct
    {
        notFoundList = new List<TCompare>();
        if (_hashedCompares.IsNull())
        {
            return null;
        }
        notFoundList = _hashedCompares
            .Where(
                x =>
                    compareKeySelector(x) is not null
                    && _source.All(k => !sourceKeySelector(k).Equals(compareKeySelector(x)))
            )
            .ToList();
        if (!notFoundList.Any())
        {
            foreach (
                TSource source in _source.Where(
                    x =>
                        _hashedCompares.Any(
                            k =>
                                compareKeySelector(k) is not null
                                && sourceKeySelector(x).Equals(compareKeySelector(k))
                        )
                )
            )
            {
                TCompare compare = _hashedCompares.First(
                    x => compareKeySelector(x).Equals(sourceKeySelector(source))
                );
                string[]? result = action(source, compare)?.Select(x => x.ToString()).ToArray();
                if (result.IsNotNullOrEmpty())
                {
                    return result;
                }
            }
        }
        return null;
    }
    private void UpdateOperation<T>(
        Func<TSource, T> sourceKeySelector,
        Func<TCompare, T> compareKeySelector,
        Action<TSource, TCompare> action
    )
    {
        if (_hashedCompares.IsNull())
        {
            return;
        }
        foreach (
            TSource source in _source.Where(
                x => _hashedCompares.Any(k => sourceKeySelector(x).Equals(compareKeySelector(k)))
            )
        )
        {
            TCompare compare = _hashedCompares.First(
                x => compareKeySelector(x).Equals(sourceKeySelector(source))
            );
            action(source, compare);
        }
    }
    private void AddOperation<T>(Func<TCompare, T?> compareKeySelector, Action<TCompare> action)
    {
        if (_hashedCompares.IsNull())
        {
            return;
        }
        foreach (TCompare compare in _hashedCompares.Where(x => compareKeySelector(x) is null))
        {
            action(compare);
        }
    }
    private void AddOperation<T>(
        Func<TSource, T> sourceKeySelector,
        Func<TCompare, T> compareKeySelector,
        Action<TCompare> action
    )
    {
        if (_hashedCompares.IsNull())
        {
            return;
        }
        foreach (
            TCompare compare in _hashedCompares
                .Where(
                    x =>
                        _source
                            .Where(k => sourceKeySelector(k).IsNotNull())
                            .All(k => !compareKeySelector(x).Equals(sourceKeySelector(k)))
                )
                .ToList()
        )
        {
            action(compare);
        }
    }
}
