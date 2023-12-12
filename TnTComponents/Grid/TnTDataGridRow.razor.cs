using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TnTComponents.Events;
using TnTComponents.Grid.Infrastructure;

namespace TnTComponents.Grid;
public partial class TnTDataGridRow<TGridItem> : IDisposable {

    [CascadingParameter]
    private TnTDataGridContext<TGridItem> _context { get; set; } = default!;

    public int RowIndex { get; internal set; } = -1;

    [Parameter]
    public TGridItem Item { get; set; } = default!;

    [Parameter]
    public bool ShowRowIndex { get; set; }

    protected override void OnInitialized() {
        base.OnInitialized();
        _context.RegisterRow(this);
    }

    protected override void OnAfterRender(bool firstRender) {
        base.OnAfterRender(firstRender);
        if(firstRender) {
            StateHasChanged();
        }
    }

    private async Task RowClicked(MouseEventArgs args) {
        await _context.RowClicked.InvokeAsync(new DataGridRowClickEventArgs(args, Item, RowIndex));
    }

    public void Dispose() {
        GC.SuppressFinalize(this);
        _context.RemoveRow(this);
    }
}
