using Microsoft.AspNetCore.Components;

namespace TnTComponents.Grid.Infrastructure;

/// <summary>
///     Represents a cell in the body of a data grid for a specific grid item type.
/// </summary>
/// <typeparam name="TGridItem">The type of the data item displayed in the cell.</typeparam>
[CascadingTypeParameter(nameof(TGridItem))]
public partial class TnTDataGridBodyCell<TGridItem> {

    /// <summary>
    ///     The data item associated with this cell.
    /// </summary>
    [Parameter, EditorRequired]
    public TGridItem Item { get; set; }
}