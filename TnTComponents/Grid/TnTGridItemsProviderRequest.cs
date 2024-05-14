using Microsoft.AspNetCore.Components.QuickGrid;
using TnTComponents.Grid.Columns;
using TnTComponents.Virtualization;

namespace TnTComponents.Grid;

/// <summary>
/// Parameters for data to be supplied by a <see cref="TnTDataGrid{TGridItem}" />'s <see
/// cref="TnTDataGrid{TGridItem}.ItemsProvider" />.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
public readonly struct TnTGridItemsProviderRequest<TGridItem> {

    /// <summary>
    /// Gets or sets a token that indicates if the request should be cancelled.
    /// </summary>
    public CancellationToken CancellationToken { get; init; }

    /// <summary>
    /// If set, the maximum number of items to be supplied. If not set, the maximum number is unlimited.
    /// </summary>
    public int? Count { get; init; }

    /// <summary>
    /// Gets or sets thecurrent sort direction.
    ///
    /// Rather than inferring the sort rules manually, you should normally call either <see
    /// cref="ApplySorting(IQueryable{TGridItem})" /> or <see cref="GetSortByProperties" />, since
    /// they also account for <see cref="SortByColumn" /> and <see cref="SortByAscending" /> automatically.
    /// </summary>
    public bool SortByAscending { get; init; }

    /// <summary>
    /// Gets or sets which column represents the sort order.
    ///
    /// Rather than inferring the sort rules manually, you should normally call either <see
    /// cref="ApplySorting(IQueryable{TGridItem})" /> or <see cref="GetSortByProperties" />, since
    /// they also account for <see cref="SortByColumn" /> and <see cref="SortByAscending" /> automatically.
    /// </summary>
    public TnTColumnBase<TGridItem>? SortByColumn { get; init; }

    public IReadOnlyCollection<KeyValuePair<string, SortDirection>> SortOnProperties =>
            GetSortByProperties().Select(sp => new KeyValuePair<string, SortDirection>(sp.PropertyName, sp.Direction)).ToList();

    /// <summary>
    /// Gets or sets the zero-based index of the first item to be supplied.
    /// </summary>
    public int StartIndex { get; init; }

    internal TnTGridItemsProviderRequest(int startIndex, int? count, TnTColumnBase<TGridItem>? sortByColumn, bool sortByAscending,
        CancellationToken cancellationToken) {
        StartIndex = startIndex;
        Count = count;
        SortByColumn = sortByColumn;
        SortByAscending = sortByAscending;
        CancellationToken = cancellationToken;
    }

    public static implicit operator TnTGridItemsProviderRequest<TGridItem>(TnTItemsProviderRequest request) {
        return new TnTGridItemsProviderRequest<TGridItem> {
            Count = request.Count,
            SortByColumn = null,
            SortByAscending = request.SortOnProperties.Count() > 0 && request.SortOnProperties.First().Value == SortDirection.Ascending,
            StartIndex = request.StartIndex,
            CancellationToken = default
        };
    }

    public static implicit operator TnTItemsProviderRequest(TnTGridItemsProviderRequest<TGridItem> request) {
        return new TnTItemsProviderRequest {
            StartIndex = request.StartIndex,
            SortOnProperties = request.SortOnProperties,
            Count = request.Count
        };
    }

    /// <summary>
    /// Applies the request's sorting rules to the supplied <see cref="IQueryable{TGridItem}" />.
    /// </summary>
    /// <param name="source">An <see cref="IQueryable{TGridItem}" />.</param>
    /// <returns>
    /// A new <see cref="IQueryable{TGridItem}" /> representing the <paramref name="source" /> with
    /// sorting rules applied.
    /// </returns>
    public IQueryable<TGridItem> ApplySorting(IQueryable<TGridItem> source) =>
        SortByColumn?.SortBy?.Apply(source, SortByAscending) ?? source;

    /// <summary>
    /// Produces a collection of (property name, direction) pairs representing the sorting rules.
    /// </summary>
    /// <returns>A collection of (property name, direction) pairs representing the sorting rules</returns>
    public IReadOnlyCollection<SortedProperty> GetSortByProperties() =>
        SortByColumn?.SortBy?.ToPropertyList(SortByAscending) ?? Array.Empty<SortedProperty>();
}