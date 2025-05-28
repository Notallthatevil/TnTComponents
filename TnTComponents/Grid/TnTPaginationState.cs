using TnTComponents.Grid.Infrastructure;

namespace TnTComponents.Grid;

/// <summary>
///     Delegate to notify when the current page index has changed.
/// </summary>
public delegate Task CurrentPageChangedAsync();

/// <summary>
///     Delegate to notify when the total item count has changed.
/// </summary>
public delegate Task TotalItemCountChangedAsync();

/// <summary>
///     Holds state to represent pagination in a <see cref="TnTDataGrid{TGridItem}" />.
/// </summary>
public class TnTPaginationState {

    /// <summary>
    ///     Event that is raised when the current page index changes, typically after a user navigates to a different page.
    /// </summary>
    public event CurrentPageChangedAsync? CurrentPageChangedAsync;

    /// <summary>
    ///     Event that is raised when the current page items change, typically after a data load operation.
    /// </summary>
    public event TotalItemCountChangedAsync? TotalItemCountChangedAsync;

    /// <summary>
    ///     Gets the current zero-based page index. To set it, call <see cref="SetCurrentPageIndexAsync(int)" />.
    /// </summary>
    public int CurrentPageIndex { get; private set; }

    /// <summary>
    ///     Gets or sets the number of items on each page.
    /// </summary>
    public int ItemsPerPage { get; set; } = 10;

    /// <summary>
    ///     Gets the zero-based index of the last page, if known. The value will be null until <see cref="TotalItemCount" /> is known.
    /// </summary>
    public int? LastPageIndex => (TotalItemCount - 1) / ItemsPerPage;

    /// <summary>
    ///     Gets the total number of items across all pages, if known. The value will be null until an associated <see cref="TnTDataGrid{TGridItem}" /> assigns a value after loading data.
    /// </summary>
    public int? TotalItemCount { get; private set; }

    /// <summary>
    ///     Sets the current page index, and notifies any associated <see cref="TnTDataGrid{TGridItem}" /> to fetch and render updated data.
    /// </summary>
    /// <param name="pageIndex">The new, zero-based page index.</param>
    /// <returns>A <see cref="Task" /> representing the completion of the operation.</returns>
    public Task SetCurrentPageIndexAsync(int pageIndex) {
        CurrentPageIndex = pageIndex;
        return CurrentPageChangedAsync?.Invoke() ?? Task.CompletedTask;
    }

    /// <summary>
    ///     Sets the total item count, and notifies any associated <see cref="TnTDataGrid{TGridItem}" /> to update the pagination UI.
    /// </summary>
    /// <param name="totalItemCount">The new count</param>
    public Task SetTotalItemCountAsync(int totalItemCount) {
        if (totalItemCount == TotalItemCount) {
            return Task.CompletedTask;
        }

        TotalItemCount = totalItemCount;

        if (CurrentPageIndex > 0 && CurrentPageIndex > LastPageIndex) {
            // If the number of items has reduced such that the current page index is no longer valid, move automatically to the final valid page index and trigger a further data load.
            SetCurrentPageIndexAsync(LastPageIndex.Value);
        }

        // Under normal circumstances, we just want any associated pagination UI to update
        return TotalItemCountChangedAsync?.Invoke() ?? Task.CompletedTask;
    }
}