namespace TnTComponents.Grid.Infrastructure;

// The grid cascades this so that descendant columns can talk back to it. It's an internal type so
// that it doesn't show up by mistake in unrelated components.
internal sealed class TnTInternalGridContext<TGridItem>(TnTDataGrid<TGridItem> grid) {
    public EventCallbackSubscribable<object?> ColumnsFirstCollected { get; } = new();
    public TnTDataGrid<TGridItem> Grid { get; } = grid;
    public Dictionary<string, TnTDataGridRow<TGridItem>> Rows { get; set; } = [];
    private int _cellId = 0;
    private int _index = 0;
    private int _rowId = 0;

    public int GetNextCellId() {
        Interlocked.Increment(ref _cellId);
        return _cellId;
    }

    public int GetNextRowId() {
        Interlocked.Increment(ref _rowId);
        return _rowId;
    }

    internal void Register(TnTDataGridRow<TGridItem> row) {
        Rows.Add(row.RowId, row);
        if (!Grid.Virtualize) {
#pragma warning disable BL0005 // Component parameter should not be set outside of its component.
            row.RowIndex = _index++;
#pragma warning restore BL0005 // Component parameter should not be set outside of its component.
        }
    }

    internal void ResetRowIndexes(int start) => _index = start;

    internal void Unregister(TnTDataGridRow<TGridItem> row) => Rows.Remove(row.RowId);
}