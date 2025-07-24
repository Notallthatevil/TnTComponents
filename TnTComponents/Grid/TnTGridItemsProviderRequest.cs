using TnTComponents.Core;
using TnTComponents.Grid.Columns;
using TnTComponents.Virtualization;

namespace TnTComponents.Grid;

/// <summary>
///     Parameters for data to be supplied by a <see cref="TnTDataGrid{TGridItem}" />'s 
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
public readonly struct TnTGridItemsProviderRequest<TGridItem> {

    /// <summary>
    ///     Gets or sets a token that indicates if the request should be cancelled.
    /// </summary>
    public CancellationToken CancellationToken { get; init; }

    /// <summary>
    ///     If set, the maximum number of items to be supplied. If not set, the maximum number is unlimited.
    /// </summary>
    public int? Count { get; init; }

    /// <summary>
    ///     Gets or sets which column represents the sort order. Rather than inferring the sort rules manually, you should normally call either <see cref="ApplySorting(IQueryable{TGridItem})" /> or
    ///     <see cref="GetSortByProperties" />, since they also account for <see cref="SortByColumn" /> and <see cref="SortByAscending" /> automatically.
    /// </summary>
    public TnTGridSort<TGridItem>? Sort { get; init; }

    /// <summary>
    ///     Gets or sets the zero-based index of the first item to be supplied.
    /// </summary>
    public int StartIndex { get; init; }

    internal TnTGridItemsProviderRequest(int startIndex, int? count, TnTGridSort<TGridItem>? sort, CancellationToken cancellationToken) {
        StartIndex = startIndex;
        Count = count;
        Sort = sort;
        CancellationToken = cancellationToken;
    }

    /// <summary>
    ///     Implicitly converts a <see cref="TnTItemsProviderRequest" /> to a <see cref="TnTGridItemsProviderRequest{TGridItem}" />.
    /// </summary>
    /// <param name="request">The <see cref="TnTItemsProviderRequest" /> to convert from</param>
    public static implicit operator TnTGridItemsProviderRequest<TGridItem>(TnTItemsProviderRequest request) {
        return new TnTGridItemsProviderRequest<TGridItem> {
            Count = request.Count,
            Sort = null,
            StartIndex = request.StartIndex,
            CancellationToken = default
        };
    }

    /// <summary>
    ///     Implicitly converts a <see cref="TnTGridItemsProviderRequest{TGridItem}" /> to a <see cref="TnTItemsProviderRequest" />.
    /// </summary>
    public static implicit operator TnTItemsProviderRequest(TnTGridItemsProviderRequest<TGridItem> request) {
        return new TnTItemsProviderRequest {
            StartIndex = request.StartIndex,
            SortOnProperties = [.. request.GetSortByProperties().Select(sp => new KeyValuePair<string, SortDirection>(sp.PropertyName, sp.Direction))],
            Count = request.Count
        };
    }

    /// <summary>
    ///     Applies the request's sorting rules to the supplied <see cref="IQueryable{TGridItem}" />.
    /// </summary>
    /// <param name="source">An <see cref="IQueryable{TGridItem}" />.</param>
    /// <returns>A new <see cref="IQueryable{TGridItem}" /> representing the <paramref name="source" /> with sorting rules applied.</returns>
    public IQueryable<TGridItem> ApplySorting(IQueryable<TGridItem> source) => Sort?.Apply(source) ?? source;

    /// <summary>
    ///     Produces a collection of (property name, direction) pairs representing the sorting rules.
    /// </summary>
    /// <returns>A collection of (property name, direction) pairs representing the sorting rules</returns>
    public IEnumerable<SortedProperty> GetSortByProperties() => Sort?.ToPropertyList() ?? [];
}