using System.Linq.Expressions;
using System.Reflection;

namespace LiveTest.Internal;

public static class LinqExtensions {

    public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> query, string name) {
        var propInfo = GetPropertyInfo(typeof(T), name);
        var expr = GetOrderExpression(typeof(T), propInfo);

        MethodInfo method = typeof(Enumerable).GetMethods().First(m => m.Name == "OrderBy" && m.GetParameters().Length == 2);
        MethodInfo genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
        return (IEnumerable<T>)genericMethod.Invoke(null, new object[] { query, expr.Compile() })!;
    }

    public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string name) {
        var propInfo = GetPropertyInfo(typeof(T), name);
        var expr = GetOrderExpression(typeof(T), propInfo);

        MethodInfo method = typeof(Queryable).GetMethods().First(m => m.Name == "OrderBy" && m.GetParameters().Length == 2);
        MethodInfo genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
        return (IQueryable<T>)genericMethod.Invoke(null, new object[] { query, expr })!;
    }

    public static IEnumerable<T> OrderByDescending<T>(this IEnumerable<T> query, string name) {
        var propInfo = GetPropertyInfo(typeof(T), name);
        var expr = GetOrderExpression(typeof(T), propInfo);

        MethodInfo method = typeof(Enumerable).GetMethods().First(m => m.Name == "OrderByDescending" && m.GetParameters().Length == 2);
        MethodInfo genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
        return (IEnumerable<T>)genericMethod.Invoke(null, new object[] { query, expr.Compile() })!;
    }

    public static IQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string name) {
        var propInfo = GetPropertyInfo(typeof(T), name);
        var expr = GetOrderExpression(typeof(T), propInfo);

        MethodInfo method = typeof(Queryable).GetMethods().First(m => m.Name == "OrderByDescending" && m.GetParameters().Length == 2);
        MethodInfo genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
        return (IQueryable<T>)genericMethod.Invoke(null, new object[] { query, expr })!;
    }

    private static LambdaExpression GetOrderExpression(Type objType, PropertyInfo pi) {
        var paramExpr = Expression.Parameter(objType);
        var propAccess = Expression.PropertyOrField(paramExpr, pi.Name);
        var expr = Expression.Lambda(propAccess, paramExpr);
        return expr;
    }

    private static PropertyInfo GetPropertyInfo(Type objType, string name) {
        var properties = objType.GetProperties();
        var matchedProperty = properties.FirstOrDefault(p => p.Name == name);
        if (matchedProperty == null)
            throw new ArgumentException("name");

        return matchedProperty;
    }
}