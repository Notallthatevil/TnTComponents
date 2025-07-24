using Microsoft.AspNetCore.Components;
using TnTComponents.Core;
using TnTComponents.Grid.Columns;

namespace TnTComponents.Grid.Infrastructure;

[CascadingTypeParameter(nameof(TGridItem))]
public abstract class TnTDataGridCell<TGridItem> : ComponentBase {
    [Parameter, EditorRequired]
    public TnTColumnBase<TGridItem> Column { get; set; } = default!;

    protected override void OnParametersSet() {
        base.OnParametersSet();
        ArgumentNullException.ThrowIfNull(Column, nameof(Column));
    }

    public async Task RefreshAsync() {
        await InvokeAsync(StateHasChanged);
    }
}