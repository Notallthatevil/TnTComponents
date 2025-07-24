using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using TnTComponents.Core;

namespace TnTComponents.Grid;


public sealed class TnTGridSort<TGridItem> {
    private const string _expressionNotRepresentableMessage = "The supplied expression can't be represented as a property name for sorting. Only simple member expressions, such as @(x => x.SomeProperty), can be converted to property names.";


    private Dictionary<string, SortDirection> _sortDirections = new();
    private readonly List<(PropertyInfo Property, Type? CustomComparer)> _expressions = [];

    private bool _flipDirections;

    public bool FlipDirections {
        get => _flipDirections;
        set {
            if (value == _flipDirections) {
                return;
            }

            foreach (var (propertyName, direction) in _sortDirections) {
                _sortDirections[propertyName] = direction == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
            }
            _flipDirections = value;
        }
    }

    private TnTGridSort() { }

    public static TnTGridSort<TGridItem> ByAscending<U>(Expression<Func<TGridItem, U>> expression, IComparer<U>? comparer = null) {
        var gridSort = new TnTGridSort<TGridItem>();
        AddExpression(gridSort, expression, SortDirection.Ascending, comparer);
        return gridSort;
    }

    public static TnTGridSort<TGridItem> ByDescending<U>(Expression<Func<TGridItem, U>> expression, IComparer<U>? comparer = null) {
        var gridSort = new TnTGridSort<TGridItem>();
        AddExpression(gridSort, expression, SortDirection.Descending, comparer);
        return gridSort;
    }


    public TnTGridSort<TGridItem> ThenAscending<U>(Expression<Func<TGridItem, U>> expression, IComparer<U>? comparer = null) {
        AddExpression(this, expression, SortDirection.Ascending, comparer);
        return this;
    }

    public TnTGridSort<TGridItem> ThenDescending<U>(Expression<Func<TGridItem, U>> expression, IComparer<U>? comparer = null) {
        AddExpression(this, expression, SortDirection.Descending, comparer);
        return this;
    }

    private static void AddExpression<U>(TnTGridSort<TGridItem> gridSort, Expression<Func<TGridItem, U>> expression, SortDirection direction, IComparer<U>? comparer) {
        var body = expression.Body;
        if (body is not MemberExpression memberExpression) {
            throw new ArgumentException(_expressionNotRepresentableMessage, nameof(expression));
        }

        var property = memberExpression.Member as PropertyInfo ?? throw new InvalidOperationException(_expressionNotRepresentableMessage);
        gridSort._expressions.Add((property, comparer?.GetType()));
        if (!gridSort._sortDirections.TryAdd(property.Name, direction)) {
            throw new InvalidOperationException($"The property '{expression.Body}' is already added for sorting, properties can only be sorted by once.");
        }
    }

    public TnTGridSort<TGridItem> Append(TnTGridSort<TGridItem>? other) {
        var gridSort = new TnTGridSort<TGridItem>();
        gridSort._expressions.AddRange(_expressions);
        gridSort._flipDirections = _flipDirections;
        gridSort._sortDirections = new Dictionary<string, SortDirection>(_sortDirections);

        if (other is not null) {
            gridSort._expressions.AddRange(other._expressions);
            foreach (var (property, direction) in other._sortDirections) {
                if (!gridSort._sortDirections.TryAdd(property, direction)) {
                    throw new InvalidOperationException($"The property '{property}' is already added for sorting, properties can only be sorted by once.");
                }
            }
        }
        return gridSort;
    }

    public IOrderedQueryable<TGridItem> Apply(IQueryable<TGridItem> queryable) {
        var query = queryable;
        var param = Expression.Parameter(typeof(TGridItem), "x");
        var func = nameof(Queryable.OrderBy);
        var descending = nameof(Queryable.OrderByDescending);
        foreach (var (property, customComparer) in _expressions) {

            if (_sortDirections[property.Name] == SortDirection.Descending) {
                func = descending;
            }

            query = (IOrderedQueryable<TGridItem>)query.Provider.CreateQuery(Expression.Call(typeof(Queryable), func, [typeof(TGridItem), property.PropertyType], query.Expression, Expression.Lambda(Expression.PropertyOrField(param, property.Name), param)));

            func = nameof(Queryable.ThenBy);
            descending = nameof(Queryable.ThenByDescending);
        }

        return (IOrderedQueryable<TGridItem>)query;
    }

    internal IOrderedQueryable<TGridItem> Apply(IQueryable<TGridItem> queryable, bool ascending) {
        return Apply(queryable);
    }

    public IEnumerable<SortedProperty> ToPropertyList() => _sortDirections.Select(kv => new SortedProperty() { Direction = kv.Value, PropertyName = kv.Key });
}