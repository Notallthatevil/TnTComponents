using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Text;
using System.Text.Json.Serialization;
using TnTComponents.Enum;
using TnTComponents.Events;
using TnTComponents.Grid.Columns;
using TnTComponents.Grid.Infrastructure;

namespace TnTComponents.Grid;

[CascadingTypeParameter(nameof(TGridItem))]
public partial class TnTDataGrid<TGridItem> {

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public IQueryable<TGridItem>? Items { get; set; }

    [Parameter]
    public EventCallback<DataGridRowClickEventArgs> RowClickedCallback { get; set; }
    [Parameter]
    public bool Virtualize { get; set; }
    [Parameter]
    public override string? Class { get; set; } = "tnt-data-grid";

    [Parameter]
    public string ContainerClass { get; set; } = "tnt-data-grid-container";

    [Parameter]
    public string CellContentContainerClass { get; set; } = "tnt-data-cell-content-container";

    [Parameter]
    public double? MaxHeight { get; set; }
    [Parameter]
    public double? Height { get; set; }

    [Parameter]
    public bool ShowRowIndex { get; set; }

    [Parameter]
    public DataGridAppearance Appearance { get; set; }

    private readonly List<TnTColumnBase<TGridItem>> _columns = [];
    private readonly TnTDataGridContext<TGridItem> _context;

    private RenderFragment _renderHeaderContent;
    private RenderFragment _renderRowContent;

    public TnTDataGrid() {
        _context = new(this);
        _renderHeaderContent = RenderHeaderContent;
        _renderRowContent = RenderRowContent;
    }

    public override string GetClass() {
        var strBuilder = new StringBuilder(base.GetClass());

        if (Appearance == DataGridAppearance.Default) {
            return strBuilder.ToString();
        }

        if ((Appearance & DataGridAppearance.Stripped) != DataGridAppearance.Default) {
            strBuilder.Append(' ').Append("stripped");
        }

        if ((Appearance & DataGridAppearance.Compat) != DataGridAppearance.Default) {
            strBuilder.Append(' ').Append("compact");
        }
        return strBuilder.ToString();
    }

    protected override void OnParametersSet() {
        base.OnParametersSet();
    }

    internal void AddColumn(TnTColumnBase<TGridItem> column) {
        _columns.Add(column);
    }

    private string? GetContainerStyle() {
        var strBuilder = new StringBuilder();
        if (MaxHeight.HasValue) {
            strBuilder.Append("max-height:");
            if (MaxHeight.Value <= 1) {
                strBuilder.Append(MaxHeight * 100).Append('%');
            }
            else {
                strBuilder.Append(MaxHeight).Append("px");
            }
            strBuilder.Append(';');
        }
        if (Height.HasValue) {
            strBuilder.Append("height:");
            if (Height.Value <= 1) {
                strBuilder.Append(Height * 100).Append('%');
            }
            else {
                strBuilder.Append(Height).Append("px");
            }
            strBuilder.Append(';');
        }
        return strBuilder.Length > 0 ? strBuilder.ToString() : null;
    }

    private async Task RowClicked(MouseEventArgs args, object? item, int rowIndex) {
        await RowClickedCallback.InvokeAsync(new DataGridRowClickEventArgs(args, item, rowIndex));
        StateHasChanged();
    }

}
