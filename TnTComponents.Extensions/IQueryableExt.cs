using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Virtualization;

namespace TnTComponents.Extensions;

/// <summary>
///     Extension methods for <see cref="IQueryable{T}" /> to add additional functionality for sorting, filtering and paging operations.
/// </summary>
public static class IQueryableExt {

    /// <summary>
    ///     Applies sorting, paging, and filtering to an <see cref="IQueryable{T}" /> based on the provided request parameters.
    /// </summary>
    /// <typeparam name="T">The type of elements in the query.</typeparam>
    /// <param name="query">  The source query to apply operations to.</param>
    /// <param name="request">The request containing sorting and paging parameters.</param>
    /// <returns>A filtered and sorted <see cref="IQueryable{T}" /> with paging applied.</returns>
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
    ///     Builds the Queryable functions using a TSource property name.
    /// </summary>
    /// <typeparam name="T">The type of elements in the query.</typeparam>
    /// <param name="query">       The source query.</param>
    /// <param name="methodName">  The name of the method to call on the Queryable type.</param>
    /// <param name="propertyName">The name of the property to sort by. Can be a nested property using dot notation.</param>
    /// <returns>An <see cref="IOrderedQueryable{T}" /> sorted according to the specified method and property.</returns>
    private static IOrderedQueryable<T> CallOrderedQueryable<T>(this IQueryable<T> query, string methodName, string propertyName) {
        var param = Expression.Parameter(typeof(T), "x");

        var body = propertyName.Split('.').Aggregate<string, Expression>(param, Expression.PropertyOrField);

        return (IOrderedQueryable<T>)query.Provider.CreateQuery(Expression.Call(typeof(Queryable), methodName, new Type[] { typeof(T), body.Type }, query.Expression, Expression.Lambda(body, param)));
    }

    /// <summary>
    ///     Orders the elements of a sequence in ascending order according to a specified property name.
    /// </summary>
    /// <typeparam name="T">The type of elements in the query.</typeparam>
    /// <param name="query">       The source query.</param>
    /// <param name="propertyName">The name of the property to sort by.</param>
    /// <returns>An <see cref="IOrderedQueryable{T}" /> whose elements are sorted in ascending order.</returns>
    private static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string propertyName) {
        return CallOrderedQueryable(query, "OrderBy", propertyName);
    }

    /// <summary>
    ///     Orders the elements of a sequence according to multiple properties and sort directions.
    /// </summary>
    /// <typeparam name="T">The type of elements in the query.</typeparam>
    /// <param name="query"> The source query.</param>
    /// <param name="sortOn">A collection of key-value pairs where the key is the property name and the value is the sort direction.</param>
    /// <returns>An <see cref="IOrderedQueryable{T}" /> whose elements are sorted according to the specified properties and directions.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the sortOn collection is empty.</exception>
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

    /// <summary>
    ///     Orders the elements of a sequence in descending order according to a specified property name.
    /// </summary>
    /// <typeparam name="T">The type of elements in the query.</typeparam>
    /// <param name="query">       The source query.</param>
    /// <param name="propertyName">The name of the property to sort by.</param>
    /// <returns>An <see cref="IOrderedQueryable{T}" /> whose elements are sorted in descending order.</returns>
    private static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string propertyName) {
        return CallOrderedQueryable(query, "OrderByDescending", propertyName);
    }

    /// <summary>
    ///     Performs a subsequent ordering of the elements in a sequence in ascending order according to a specified property name.
    /// </summary>
    /// <typeparam name="T">The type of elements in the query.</typeparam>
    /// <param name="query">       The source query.</param>
    /// <param name="propertyName">The name of the property to sort by.</param>
    /// <returns>An <see cref="IOrderedQueryable{T}" /> whose elements are sorted according to an additional key.</returns>
    private static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> query, string propertyName) {
        return CallOrderedQueryable(query, "ThenBy", propertyName);
    }

    /// <summary>
    ///     Performs a subsequent ordering of the elements in a sequence in descending order according to a specified property name.
    /// </summary>
    /// <typeparam name="T">The type of elements in the query.</typeparam>
    /// <param name="query">       The source query.</param>
    /// <param name="propertyName">The name of the property to sort by.</param>
    /// <returns>An <see cref="IOrderedQueryable{T}" /> whose elements are sorted in descending order according to an additional key.</returns>
    private static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> query, string propertyName) {
        return CallOrderedQueryable(query, "ThenByDescending", propertyName);
    }
}