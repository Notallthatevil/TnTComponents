using TnTComponents.Grid;

namespace TnTComponents.Infrastructure;

internal class TnTGridContext<TGridItem>(TnTGrid<TGridItem> tntGrid) {
    internal TnTGrid<TGridItem> Grid { get; } = tntGrid;

    internal IEnumerable<TnTColumnBase<TGridItem>> Columns => _columns;

    private List<TnTColumnBase<TGridItem>> _columns = [];

    public void AddColumn(TnTColumnBase<TGridItem> column) {
        _columns.Add(column);
        Grid.Refresh();
    }

    public void RemoveColumn(TnTColumnBase<TGridItem> column) {
        _columns.Remove(column);
        Grid.Refresh();
    }
}