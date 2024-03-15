namespace TnTComponents.Grid;

/// <summary>
/// Holds data being supplied to a <see cref="TnTDataGrid{TGridItem}" />'s <see
/// cref="TnTDataGrid{TGridItem}.ItemsProvider" />.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
public readonly struct TnTGridItemsProviderResult<TGridItem>() {

    public TnTGridItemsProviderResult(IReadOnlyCollection<TGridItem> items, int totalCount) : this() {
        Items = items;
        TotalItemCount = totalCount;
    }

    /// <summary>
    /// Gets or sets the items being supplied.
    /// </summary>
    public IReadOnlyCollection<TGridItem> Items { get; init; } = [];

    /// <summary>
    /// Gets or sets the total number of items that may be displayed in the grid. This normally
    /// means the total number of items in the underlying data source after applying any filtering
    /// that is in effect.
    ///
    /// If the grid is paginated, this should include all pages. If the grid is virtualized, this
    /// should include the entire scroll range.
    /// </summary>
    public int TotalItemCount { get; init; }
}