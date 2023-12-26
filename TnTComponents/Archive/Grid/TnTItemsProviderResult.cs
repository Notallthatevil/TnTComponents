namespace TnTComponents.Archive.Grid;

public class TnTItemsProviderResult<TGridItem> {
    public ICollection<TGridItem> Items { get; set; }
    public int Total { get; set; }
}