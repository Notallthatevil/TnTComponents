using Microsoft.AspNetCore.Components;
using TnTComponents.Infrastructure;

namespace TnTComponents.Grid;
public partial class TnTGrid<TGridItem> {

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    private TnTGridContext _gridContext = default!;

    protected override void OnInitialized() {
        _gridContext = new TnTGridContext(this);
    }

    public void Refresh() {
        StateHasChanged();
    }


    public string GetColumnHeaderClass(ITnTGridColumn column) {
        return column.Class;
    }
}
