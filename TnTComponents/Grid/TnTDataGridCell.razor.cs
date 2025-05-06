using Microsoft.AspNetCore.Components;
using TnTComponents.Core;

namespace TnTComponents.Grid;

/// <summary>
///     Represents a cell within a TnTDataGrid component that displays content for a specific grid item. This component is used to define the layout and appearance of individual cells in the grid.
/// </summary>
/// <typeparam name="TGridItem">The type of data item displayed in the grid.</typeparam>
public partial class TnTDataGridCell<TGridItem> {

    /// <summary>
    ///     Gets or sets additional attributes to apply to the cell element. These attributes are captured from unmatched attributes during component instantiation.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    /// <summary>
    ///     Gets or sets the content to be rendered inside the cell component. This content can include static text, HTML, or other Blazor components.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    ///     Gets the CSS class string generated for this cell. Classes are built from the additional attributes provided to the component.
    /// </summary>
    public string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    /// <summary>
    ///     Gets the CSS style string generated for this cell. Styles are built from the additional attributes provided to the component.
    /// </summary>
    public string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    /// <summary>
    ///     Gets or sets the reference to the data item that holds this cell's values. This provides access to the specific data item in the grid that this cell represents.
    /// </summary>
    [Parameter]
    public TGridItem? Item { get; set; }

    /// <summary>
    ///     Gets or sets the unique identifier for this cell within the grid. Used internally for tracking and referencing the cell.
    /// </summary>
    internal string CellId { get; set; } = string.Empty;
}