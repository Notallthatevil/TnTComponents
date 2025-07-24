using Microsoft.AspNetCore.Components;
using TnTComponents.Core;

namespace TnTComponents.Grid.Infrastructure;

[CascadingTypeParameter(nameof(TGridItem))]
public partial class TnTDataGridBody<TGridItem> {
    [CascadingParameter]
    internal TnTInternalGridContext<TGridItem> Context { get; set; } = default!;

    public async virtual Task RefreshAsync() {
        await InvokeAsync(StateHasChanged);
    }
}
