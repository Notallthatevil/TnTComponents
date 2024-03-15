namespace TnTComponents.Grid;

/// <summary>
/// A callback that provides data for a <see cref="TnTDataGrid{TGridItem}" />.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
/// <param name="request">Parameters describing the data being requested.</param>
/// <returns>
/// A <see cref="T:ValueTask{TnTGridItemsProviderResult{TResult}}" /> that gives the data to be displayed.
/// </returns>
public delegate ValueTask<TnTGridItemsProviderResult<TGridItem>> TnTGridItemsProvider<TGridItem>(TnTGridItemsProviderRequest<TGridItem> request);