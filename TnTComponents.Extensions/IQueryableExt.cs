using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Virtualization;

namespace TnTComponents.Extensions;

public static class IQueryableExt {

    public static IQueryable<T> Apply<T>(this IQueryable<T> query, TnTItemsProviderRequest request) {
        if (request.SortOnProperties?.Any() == true) {
            query = query.OrderBy(request.SortOnProperties);
        }

        if (request.StartIndex != 0) {
            query = query.Skip(request.StartIndex);
        }

        if (request.Count.HasValue) {
            query = query.Take(request.Count.Value);
        }

        return query;
    }

    /// <summary>
    /// Builds the Queryable functions using a TSource property name.
    /// </summary>
    private static IOrderedQueryable<T> CallOrderedQueryable<T>(this IQueryable<T> query, string methodName, string propertyName) {
        var param = Expression.Parameter(typeof(T), "x");

        var body = propertyName.Split('.').Aggregate<string, Expression>(param, Expression.PropertyOrField);

        return (IOrderedQueryable<T>)query.Provider.CreateQuery(Expression.Call(typeof(Queryable), methodName, [typeof(T), body.Type], query.Expression, Expression.Lambda(body, param)));
    }

    private static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string propertyName) {
        return CallOrderedQueryable(query, "OrderBy", propertyName);
    }

    private static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, IEnumerable<KeyValuePair<string, SortDirection>> sortOn) {
        if (!sortOn.Any()) {
            throw new InvalidOperationException("Attempted to sort on a query with an empty list of sorts");
        }
        IOrderedQueryable<T> result;

        var firstSort = sortOn.First();

        if (firstSort.Value == SortDirection.Descending) {
            result = query.OrderByDescending(firstSort.Key);
        }
        else {
            result = query.OrderBy(firstSort.Key);
        }

        foreach (var sort in sortOn.Skip(1)) {
            if (sort.Value == SortDirection.Descending) {
                result = result.ThenByDescending(sort.Key);
            }
            else {
                result = result.ThenBy(sort.Key);
            }
        }

        return result;
    }

    private static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string propertyName) {
        return CallOrderedQueryable(query, "OrderByDescending", propertyName);
    }

    private static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> query, string propertyName) {
        return CallOrderedQueryable(query, "ThenBy", propertyName);
    }

    private static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> query, string propertyName) {
        return CallOrderedQueryable(query, "ThenByDescending", propertyName);
    }
}