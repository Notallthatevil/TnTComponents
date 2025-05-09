namespace TnTComponents.Virtualization;

/// <summary>
///     Contains the result of a data request from a <see cref="TnTItemsProviderRequest" />.
/// </summary>
/// <typeparam name="TItem"></typeparam>
public readonly struct TnTItemsProviderResult<TItem>() {

    /// <summary>
    ///     Gets or sets the items being supplied.
    /// </summary>
    public IReadOnlyCollection<TItem> Items { get; init; } = [];

    /// <summary>
    ///     Gets or sets the total number of items that may be displayed in the grid. This normally means the total number of items in the underlying data source after applying any filtering that is
    ///     in effect. /// If the grid is paginated, this should include all pages. If the grid is virtualized, this should include the entire scroll range.
    /// </summary>
    public int TotalItemCount { get; init; }

    /// <summary>
    ///     Creates a new instance of <see cref="TnTItemsProviderResult{TItem}" />.
    /// </summary>
    public TnTItemsProviderResult(IReadOnlyCollection<TItem> items, int totalCount) : this() {
        Items = items;
        TotalItemCount = totalCount;
    }
}