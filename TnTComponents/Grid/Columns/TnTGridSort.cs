using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using TnTComponents.Core;

namespace TnTComponents.Grid;

/// <summary>
///     Represents sorting rules for a grid of <typeparamref name="TGridItem" /> items. Allows specifying multiple properties and directions for sorting, and supports custom comparers.
/// </summary>
/// <typeparam name="TGridItem">The type of items in the grid.</typeparam>
public sealed class TnTGridSort<TGridItem> {

    /// <summary>
    ///     Whether the sort directions should be flipped for all properties. When set, all sort directions are reversed.
    /// </summary>
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

    private const string _expressionNotRepresentableMessage = "The supplied expression can't be represented as a property name for sorting. Only simple member expressions, such as @(x => x.SomeProperty), can be converted to property names.";

    /// <summary>
    ///     Stores the list of property expressions and their associated custom comparer types.
    /// </summary>
    private readonly List<(PropertyInfo Property, Type? CustomComparer)> _expressions = [];

    /// <summary>
    ///     Indicates whether the sort directions are currently flipped.
    /// </summary>
    private bool _flipDirections;

    /// <summary>
    ///     Stores the sort direction for each property name.
    /// </summary>
    private Dictionary<string, SortDirection> _sortDirections = new();

    /// <summary>
    ///     Initializes a new instance of the <see cref="TnTGridSort{TGridItem}" /> class. Private to enforce use of static factory methods.
    /// </summary>
    private TnTGridSort() {
    }

    /// <summary>
    ///     Creates a new <see cref="TnTGridSort{TGridItem}" /> instance that sorts by the specified property in ascending order.
    /// </summary>
    /// <typeparam name="U">The type of the property to sort by.</typeparam>
    /// <param name="expression">An expression selecting the property to sort by.</param>
    /// <param name="comparer">  An optional custom comparer for the property.</param>
    /// <returns>A new <see cref="TnTGridSort{TGridItem}" /> instance.</returns>
    public static TnTGridSort<TGridItem> ByAscending<U>(Expression<Func<TGridItem, U>> expression, IComparer<U>? comparer = null) {
        var gridSort = new TnTGridSort<TGridItem>();
        AddExpression(gridSort, expression, SortDirection.Ascending, comparer);
        return gridSort;
    }

    /// <summary>
    ///     Creates a new <see cref="TnTGridSort{TGridItem}" /> instance that sorts by the specified property in descending order.
    /// </summary>
    /// <typeparam name="U">The type of the property to sort by.</typeparam>
    /// <param name="expression">An expression selecting the property to sort by.</param>
    /// <param name="comparer">  An optional custom comparer for the property.</param>
    /// <returns>A new <see cref="TnTGridSort{TGridItem}" /> instance.</returns>
    public static TnTGridSort<TGridItem> ByDescending<U>(Expression<Func<TGridItem, U>> expression, IComparer<U>? comparer = null) {
        var gridSort = new TnTGridSort<TGridItem>();
        AddExpression(gridSort, expression, SortDirection.Descending, comparer);
        return gridSort;
    }

    /// <summary>
    ///     Appends the sorting rules from another <see cref="TnTGridSort{TGridItem}" /> instance to this one. Throws if a property is already present.
    /// </summary>
    /// <param name="other">The other <see cref="TnTGridSort{TGridItem}" /> to append.</param>
    /// <returns>A new <see cref="TnTGridSort{TGridItem}" /> with combined sorting rules.</returns>
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

    /// <summary>
    ///     Applies the sorting rules to the supplied <see cref="IQueryable{TGridItem}" />.
    /// </summary>
    /// <param name="queryable">The queryable source to sort.</param>
    /// <returns>An <see cref="IOrderedQueryable{TGridItem}" /> with sorting applied.</returns>
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

    /// <summary>
    ///     Adds an additional property to sort by in ascending order.
    /// </summary>
    /// <typeparam name="U">The type of the property to sort by.</typeparam>
    /// <param name="expression">An expression selecting the property to sort by.</param>
    /// <param name="comparer">  An optional custom comparer for the property.</param>
    /// <returns>This <see cref="TnTGridSort{TGridItem}" /> instance.</returns>
    public TnTGridSort<TGridItem> ThenAscending<U>(Expression<Func<TGridItem, U>> expression, IComparer<U>? comparer = null) {
        AddExpression(this, expression, SortDirection.Ascending, comparer);
        return this;
    }

    /// <summary>
    ///     Adds an additional property to sort by in descending order.
    /// </summary>
    /// <typeparam name="U">The type of the property to sort by.</typeparam>
    /// <param name="expression">An expression selecting the property to sort by.</param>
    /// <param name="comparer">  An optional custom comparer for the property.</param>
    /// <returns>This <see cref="TnTGridSort{TGridItem}" /> instance.</returns>
    public TnTGridSort<TGridItem> ThenDescending<U>(Expression<Func<TGridItem, U>> expression, IComparer<U>? comparer = null) {
        AddExpression(this, expression, SortDirection.Descending, comparer);
        return this;
    }

    /// <summary>
    ///     Produces a collection of <see cref="SortedProperty" /> representing the sorting rules.
    /// </summary>
    /// <returns>A collection of <see cref="SortedProperty" /> with property names and directions.</returns>
    public IEnumerable<SortedProperty> ToPropertyList() => _sortDirections.Select(kv => new SortedProperty() { Direction = kv.Value, PropertyName = kv.Key });

    /// <summary>
    ///     Applies the sorting rules to the supplied <see cref="IQueryable{TGridItem}" />. The <paramref name="ascending" /> parameter is ignored; sorting is determined by the rules.
    /// </summary>
    /// <param name="queryable">The queryable source to sort.</param>
    /// <param name="ascending">Ignored. Sorting is determined by the rules.</param>
    /// <returns>An <see cref="IOrderedQueryable{TGridItem}" /> with sorting applied.</returns>
    internal IOrderedQueryable<TGridItem> Apply(IQueryable<TGridItem> queryable, bool ascending) {
        return Apply(queryable);
    }

    /// <summary>
    ///     Adds a property and its sort direction to the grid sort instance. Throws if the expression is not a simple member expression or if the property is already present.
    /// </summary>
    /// <typeparam name="U">The type of the property to sort by.</typeparam>
    /// <param name="gridSort">  The grid sort instance to modify.</param>
    /// <param name="expression">An expression selecting the property to sort by.</param>
    /// <param name="direction"> The direction to sort by.</param>
    /// <param name="comparer">  An optional custom comparer for the property.</param>
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
}