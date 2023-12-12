using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json.Serialization;
using TnTComponents.Common;
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
    [Parameter]
    public IconType IconType { get; set; }

    [Parameter]
    public string Name { get; set; } = default!;

    [Parameter]
    public Expression<Func<TGridItem, object>>? DefaultSort { get; set; }


    private readonly TnTDataGridContext<TGridItem> _context;

    private TnTColumnBase<TGridItem>? _sortedOn;

    public TnTDataGrid() {
        _context = new(this);
    }

    protected override void OnParametersSet() {
        base.OnParametersSet();
        if (string.IsNullOrWhiteSpace(Name)) {
            throw new InvalidOperationException($"{nameof(Name)} must be provided!");
        }


        _context.RowClicked = RowClickedCallback;
        _context.DataGridName = Name;
    }
}
