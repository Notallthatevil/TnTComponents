using Microsoft.AspNetCore.Components;
using TnTComponents.Grid.Columns;

namespace TnTComponents.Grid.Infrastructure;

internal sealed class TnTInternalGridContext<TGridItem>(TnTDataGrid<TGridItem> _grid) {
    public IEnumerable<TnTColumnBase<TGridItem>> Columns => _columns.Values.OrderBy(c => c.Order).ThenBy(c => c.ColumnId);
    public DataGridAppearance DataGridAppearance => _grid.DataGridAppearance;

    public IEnumerable<TGridItem> Items { get; set; } = [];
    public EventCallback<TGridItem> RowClickCallback => _grid.OnRowClicked;
    public TnTDataGrid<TGridItem> Grid => _grid;
    public int TotalRowCount { get; set; }

    public TnTPaginationState? PaginationState => _grid.Pagination;

    private static int _nextColumnId;
    private readonly Dictionary<int, TnTColumnBase<TGridItem>> _columns = [];
    private readonly List<TnTColumnBase<TGridItem>> _sortByColumns = [];
    private readonly Dictionary<int, SortDirection> _sortingDirections = [];

    public TnTGridSort<TGridItem>? SortBy { get; private set; }

    public Func<TGridItem, object> ItemKey => _grid.ItemKey;

    public SortDirection? ColumnIsSortedOn(TnTColumnBase<TGridItem> column) => _sortingDirections.TryGetValue(column.ColumnId, out var direction) ? direction : null;


    public void RegisterColumn(TnTColumnBase<TGridItem> column) {
        if (column.ColumnId < 0) {
            column.ColumnId = Interlocked.Increment(ref _nextColumnId);
        }
        column.NewColumn = _columns.TryAdd(column.ColumnId, column);
    }

    public int? GetSortIndex(TnTColumnBase<TGridItem> column) => _sortByColumns.IndexOf(column) switch {
        -1 => null,
        var index => index + 1 // +1 because the first column is always the default sort column
    };

    public void RemoveColumn(TnTColumnBase<TGridItem> column) => _columns.Remove(column.ColumnId);

    public void SortByColumn(TnTColumnBase<TGridItem> column) {
        if (_sortingDirections.TryGetValue(column.ColumnId, out var direction)) {
            if (direction == column.InitialSortDirection) {
                var newDirection = direction == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
                _sortingDirections[column.ColumnId] = newDirection;
                if (column.SortBy is not null) {
                    column.SortBy.FlipDirections = true;
                }
            }
            else {
                _sortingDirections.Remove(column.ColumnId);
                _sortByColumns.Remove(column);
                if (column.SortBy is not null) {
                    column.SortBy.FlipDirections = false;
                }
            }
        }
        else {
            _sortingDirections[column.ColumnId] = column.InitialSortDirection;
            _sortByColumns.Add(column);
        }

        if (_sortByColumns.Count == 0) {
            SortBy = null;
        }
        else {
            SortBy = _sortByColumns[0].SortBy;
            foreach (var c in _sortByColumns.Skip(1)) {
                SortBy = SortBy?.Append(c.SortBy);
            }
        }
        UpdateItems();
    }

    private void UpdateItems() {
        if (_grid.Items is not null) {

            var items = _grid.Items;
            if (SortBy is not null) {
                items = SortBy.Apply(items);
            }
            if (PaginationState is not null) {
                items = PaginationState.Apply(items);
            }
            Items = items;
        }
        else {
            Items = _grid.ProvidedItems ?? [];
        }
        //_grid.Refresh();
    }

    public Task RefreshAsync() => _grid.RefreshDataGridAsync();


}