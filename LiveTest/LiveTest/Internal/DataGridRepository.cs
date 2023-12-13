using LiveTest.Client.Repositories;
using LiveTest.Client.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using TnTComponents.Grid;

namespace LiveTest.Internal;
public class DataGridRepository : IDataGridRepository {

    private static IQueryable<DataGridItem>? _items;
    private static object _locker = new object();
    public async Task<TnTItemsProviderResult<DataGridItem>> Get(TnTItemsProviderRequest? request) {
        lock (_locker) {
            if (_items == null) {
                var list = new List<DataGridItem>();
                foreach (var i in Enumerable.Range(1, 100)) {
                    list.Add(new DataGridItem() {
                        Column1 = Random.Shared.Next(1, 1000),
                        Column2 = Guid.NewGuid().ToString(),
                        Column3 = new DateTime(Random.Shared.Next(1, 10000), Random.Shared.Next(1, 13), Random.Shared.Next(1, 29))
                    });
                }
                _items = list.AsQueryable();
            }
        }

        var count = _items.Count();
        var items = _items;
        if (request is not null) {
            if (!string.IsNullOrWhiteSpace(request.OrderBy)) {
                if (request.Descending) {
                    items = items.OrderByDescending(request.OrderBy);
                }
                else {
                    items = items.OrderBy(request.OrderBy);
                }
            }

            items = items.Skip(request.Offset * request.Count).Take(request.Count);
        }

        return new TnTItemsProviderResult<DataGridItem>() {
            Items = items.ToList(),
            Total = count
        };
    }
}

public static class LinqExtensions {
    private static PropertyInfo GetPropertyInfo(Type objType, string name) {
        var properties = objType.GetProperties();
        var matchedProperty = properties.FirstOrDefault(p => p.Name == name);
        if (matchedProperty == null)
            throw new ArgumentException("name");

        return matchedProperty;
    }
    private static LambdaExpression GetOrderExpression(Type objType, PropertyInfo pi) {
        var paramExpr = Expression.Parameter(objType);
        var propAccess = Expression.PropertyOrField(paramExpr, pi.Name);
        var expr = Expression.Lambda(propAccess, paramExpr);
        return expr;
    }

    public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> query, string name) {
        var propInfo = GetPropertyInfo(typeof(T), name);
        var expr = GetOrderExpression(typeof(T), propInfo);

        var method = typeof(Enumerable).GetMethods().FirstOrDefault(m => m.Name == "OrderBy" && m.GetParameters().Length == 2);
        var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
        return (IEnumerable<T>)genericMethod.Invoke(null, new object[] { query, expr.Compile() });
    }

    public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string name) {
        var propInfo = GetPropertyInfo(typeof(T), name);
        var expr = GetOrderExpression(typeof(T), propInfo);

        var method = typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "OrderBy" && m.GetParameters().Length == 2);
        var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
        return (IQueryable<T>)genericMethod.Invoke(null, new object[] { query, expr });
    }

    public static IEnumerable<T> OrderByDescending<T>(this IEnumerable<T> query, string name) {
        var propInfo = GetPropertyInfo(typeof(T), name);
        var expr = GetOrderExpression(typeof(T), propInfo);

        var method = typeof(Enumerable).GetMethods().FirstOrDefault(m => m.Name == "OrderByDescending" && m.GetParameters().Length == 2);
        var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
        return (IEnumerable<T>)genericMethod.Invoke(null, new object[] { query, expr.Compile() });
    }

    public static IQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string name) {
        var propInfo = GetPropertyInfo(typeof(T), name);
        var expr = GetOrderExpression(typeof(T), propInfo);

        var method = typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "OrderByDescending" && m.GetParameters().Length == 2);
        var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
        return (IQueryable<T>)genericMethod.Invoke(null, new object[] { query, expr });
    }
}
