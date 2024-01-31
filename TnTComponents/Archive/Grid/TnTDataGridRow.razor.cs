//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Components.Web;
//using TnTComponents.Archive.Grid.Infrastructure;
//using TnTComponents.Events;

//namespace TnTComponents.Archive.Grid;

//public partial class TnTDataGridRow<TGridItem> : IDisposable {
//    [Parameter]
//    public TGridItem Item { get; set; } = default!;

// public int RowIndex { get; internal set; } = -1;

// [Parameter] public bool ShowRowIndex { get; set; }

// [CascadingParameter] private TnTDataGridContext<TGridItem> _context { get; set; } = default!;

// public void Dispose() { GC.SuppressFinalize(this); _context.RemoveRow(this); }

// protected override void OnInitialized() { base.OnInitialized(); _context.RegisterRow(this); }

//    private async Task RowClicked(MouseEventArgs args) {
//        await _context.RowClicked.InvokeAsync(new DataGridRowClickEventArgs(args, Item, RowIndex));
//    }
//}