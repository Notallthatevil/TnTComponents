using Microsoft.AspNetCore.Components;
using TnTComponents.Core;

namespace TnTComponents.Grid.Infrastructure;

/// <summary>
///     Represents a row in the data grid body for a specific grid item.
/// </summary>
/// <typeparam name="TGridItem">The type of the grid item displayed in this row.</typeparam>
public partial class TnTDataGridBodyRow<TGridItem> {

    /// <summary>
    ///     Additional attributes to be applied to the row element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    /// <summary>
    ///     The grid item associated with this row.
    /// </summary>
    [Parameter, EditorRequired]
    public TGridItem Item { get; set; }

    /// <summary>
    ///     Gets the CSS class string for the row element, including state-based classes.
    /// </summary>
    private string? _elementClass => CssClassBuilder.Create()
        .AddClass("tnt-data-grid-body-row")
        .AddClass("tnt-interactable", Context.RowClickCallback.HasDelegate)
        .AddClass("tnt-stripped", Context.DataGridAppearance.HasFlag(DataGridAppearance.Stripped))
        .Build();
}