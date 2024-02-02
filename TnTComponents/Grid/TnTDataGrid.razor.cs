using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using TnTComponents.Core;
using TnTComponents.Grid;

namespace TnTComponents;

[CascadingTypeParameter(nameof(TItem))]
public partial class TnTDataGrid<TItem> {
    [Parameter]
    public IQueryable<TItem> Items { get; set; } = default!;

    [Parameter]
    public ItemsProviderDelegate<TItem>? ItemsProvider { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public DataGridAppearance Appearance { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public string? ComponentStyle { get; set; }

    [Parameter]
    public EventCallback<TItem> RowClickedCallback { get; set; }

    [Parameter]
    public int ContainerHeightPx { get; set; } = 500;

    [Parameter]
    public TnTBorderRadius? BorderRadius { get; set; } = new(2);

    private TnTColumnBase<TItem>? _lastSortColumn;
    private bool _descending = true;

    public string Class => CssClassBuilder.Create()
        .AddClass("tnt-datagrid")
        .AddClass("tnt-compact", Appearance.HasFlag(DataGridAppearance.Compact))
        .AddClass("tnt-interactive", _interactive && RowClickedCallback.HasDelegate)
        .SetAlternative(Appearance.HasFlag(DataGridAppearance.Stripped))
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddBorderRadius(BorderRadius)
        .Build();

    private readonly List<TnTColumnBase<TItem>> _columns = [];

    private bool _interactive;

    protected override void OnAfterRender(bool firstRender) {
        base.OnAfterRender(firstRender);
        if (firstRender) {
            _interactive = true;
            StateHasChanged();
        }
    }

    internal void AddColumn(TnTColumnBase<TItem> column) {
        _columns.Add(column);
    }

    internal void RemoveColumn(TnTColumnBase<TItem> column) {
        _columns.Remove(column);
    }

    private string GetStyle() {
        return CssStyleBuilder.Create()
            .AddFromAdditionalAttributes(AdditionalAttributes)
            .AddStyle(ComponentStyle, string.Empty)
            .AddStyle("height", ContainerHeightPx + "px")
            .Build();
    }

    private async Task SortOn(TnTColumnBase<TItem> column) {
        if (_lastSortColumn == column) {
            _descending = !_descending;
        }
        else {
            _lastSortColumn = column;
            _descending = false;
        }
    }

    private IQueryable<TItem> GetSorted() {
        return _lastSortColumn?.SortOn(Items, _descending) ?? Items;
    }
}
