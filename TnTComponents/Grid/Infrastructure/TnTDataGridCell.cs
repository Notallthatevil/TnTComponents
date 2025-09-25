using Microsoft.AspNetCore.Components;
using TnTComponents.Core;
using TnTComponents.Grid.Columns;

namespace TnTComponents.Grid.Infrastructure;

/// <summary>
///     Base class for all data grid cell components in TnTComponents. Provides common logic for cell refresh and column association.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
[CascadingTypeParameter(nameof(TGridItem))]
public abstract class TnTDataGridCell<TGridItem> : ComponentBase {

    /// <summary>
    ///     The column definition associated with this cell.
    /// </summary>
    [Parameter, EditorRequired]
    public TnTColumnBase<TGridItem> Column { get; set; } = default!;

    /// <summary>
    ///     The base CSS class applied to the root element of this component. <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/class" />
    /// </summary>
    protected virtual string? ElementClass { get; }

    /// <summary>
    ///     Additional inline CSS styles applied to the root element for this component. <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/style" />
    /// </summary>
    protected virtual string? ElementStyle { get; }

    /// <summary>
    ///     Refreshes the cell by invoking <see cref="ComponentBase.StateHasChanged" /> asynchronously.
    /// </summary>
    public Task RefreshAsync() => InvokeAsync(StateHasChanged);

    /// <inheritdoc />
    protected override void OnParametersSet() {
        base.OnParametersSet();
        ArgumentNullException.ThrowIfNull(Column, nameof(Column));
    }
}