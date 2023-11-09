using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TnTComponents.Common;
using TnTComponents.Common.Ext;
using TnTComponents.Infrastructure;

namespace TnTComponents.Grid;

[CascadingTypeParameter(nameof(TGridItem))]
public partial class TnTGrid<TGridItem> {

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public IEnumerable<TGridItem> Items { get; set; } = default!;

    [Parameter]
    public Func<TGridItem, object> ItemKey { get; set; } = i => i!;

    [Parameter]
    public override string BaseCssClass { get; set; } = "tnt-grid";

    [Parameter]
    public bool IncludeIndexColumn { get; set; }

    [Parameter]
    public bool Resizable { get; set; }

    [Inject]
    private IJSRuntime _jsRuntime { get; set; } = default!;

    private readonly TnTGridContext<TGridItem> _gridContext = default!;
    private readonly RenderFragment _renderTableRows = default!;
    private ElementReference _tableReference;
    private ElementOffset _tableOffset;

    public TnTGrid() {
        _gridContext = new TnTGridContext<TGridItem>(this);
        _renderTableRows = RenderTableRows;
        _tableOffset.OffsetHeight = 0.0;
    }

    public void Refresh() {
        StateHasChanged();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        await _jsRuntime.InvokeVoidAsync("TnTComponents.createResizableColumns", _tableReference);
        await base.OnAfterRenderAsync(firstRender);
    }
}
