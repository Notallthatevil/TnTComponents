using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TnTComponents.Core;
using TnTComponents.Grid.Columns;
using TnTComponents.Grid.Infrastructure;

namespace TnTComponents.Grid.Infrastructure;

/// <summary>
///     Base class for all data grid row components in TnTComponents. Provides access to the grid context and common row logic.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
[CascadingTypeParameter(nameof(TGridItem))]
public abstract partial class TnTDataGridRow<TGridItem> : ComponentBase {

    /// <summary>
    ///     The internal grid context for this row, cascaded from the parent grid.
    /// </summary>
    [CascadingParameter]
    internal TnTInternalGridContext<TGridItem> Context { get; set; } = default!;

    /// <summary>
    ///     Refreshes the row by invoking a re-render.
    /// </summary>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public virtual async Task RefreshAsync() {
        await InvokeAsync(StateHasChanged);
    }

    /// <inheritdoc />
    protected override void OnParametersSet() {
        base.OnParametersSet();
        ArgumentNullException.ThrowIfNull(Context, nameof(Context));
    }
}