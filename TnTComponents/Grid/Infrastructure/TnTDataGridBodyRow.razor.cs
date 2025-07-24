using Microsoft.AspNetCore.Components;
using TnTComponents.Core;

namespace TnTComponents.Grid.Infrastructure;
public partial class TnTDataGridBodyRow<TGridItem> {
    [Parameter, EditorRequired]
    public TGridItem Item { get; set; }

    private string? _elementClass => CssClassBuilder.Create()
        .AddClass("tnt-data-grid-body-row")
        .AddClass("tnt-interactable", Context.RowClickCallback.HasDelegate)
        .AddClass("tnt-stripped", Context.DataGridAppearance.HasFlag(DataGridAppearance.Stripped))
        .Build();
}
