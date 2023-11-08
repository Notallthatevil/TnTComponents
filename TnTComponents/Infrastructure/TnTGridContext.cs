namespace TnTComponents.Infrastructure;
internal class TnTGridContext(ITnTGrid tntGrid) {

    internal ITnTGrid Grid { get; } = tntGrid;

    internal IEnumerable<ITnTGridColumn> Columns => _columns;

    private List<ITnTGridColumn> _columns = [];


    public void AddColumn(ITnTGridColumn column) {
        _columns.Add(column);
        Grid.Refresh();
    }

    public void RemoveColumn(ITnTGridColumn column) {
        _columns.Remove(column);
        Grid.Refresh();
    }
}
