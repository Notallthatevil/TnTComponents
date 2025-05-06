using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TnTComponents.Core;
using TnTComponents.Grid.Infrastructure;

namespace TnTComponents.Grid;

/// <summary>
///     Represents a row within a TnTDataGrid component that displays content for a specific grid item. This component defines the layout and behavior of individual rows in the grid.
/// </summary>
/// <typeparam name="TGridItem">The type of data item displayed in the row.</typeparam>
[CascadingTypeParameter(nameof(TGridItem))]
public partial class TnTDataGridRow<TGridItem> : IHandleEvent {

    /// <summary>
    ///     Additional attributes to apply to the row element. These attributes are captured from unmatched attributes during component instantiation.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    /// <summary>
    ///     The content to be rendered inside the component. This content can include static text, HTML, or other Blazor components.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    ///     Gets the CSS class string generated for this row. Classes are built from the additional attributes provided to the component and includes "tnt-clickable" when the OnClick event has a delegate.
    /// </summary>
    public string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-clickable", OnClick.HasDelegate)
        .Build();

    /// <summary>
    ///     Gets the CSS style string generated for this row. Styles are built from the additional attributes provided to the component.
    /// </summary>
    public string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    /// <summary>
    ///     The reference to the item that holds this row's values. This provides access to the specific data item in the grid that this row represents.
    /// </summary>
    [Parameter]
    public TGridItem? Item { get; set; }

    /// <summary>
    ///     The event callback that is invoked when the row is clicked. The callback receives the Item as its parameter.
    /// </summary>
    [Parameter]
    public EventCallback<TGridItem?> OnClick { get; set; }

    /// <summary>
    ///     The index of this row. When FluentDataGrid is virtualized, this value is not used.
    /// </summary>
    [Parameter]
    public int? RowIndex { get; set; }

    /// <summary>
    ///     The unique identifier for this row within the grid. Used internally for tracking and referencing the row.
    /// </summary>
    internal string RowId { get; set; } = string.Empty;

    /// <summary>
    ///     Dictionary of cells contained within this row, keyed by their cell ID.
    /// </summary>
    private readonly Dictionary<string, TnTDataGridCell<TGridItem>> cells = [];

    /// <summary>
    ///     Implementation of IHandleEvent.HandleEventAsync that invokes the callback with the provided argument. This enables more efficient event handling within the component.
    /// </summary>
    /// <param name="callback">The event callback to invoke.</param>
    /// <param name="arg">     The argument to pass to the callback.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task IHandleEvent.HandleEventAsync(EventCallbackWorkItem callback, object? arg) => callback.InvokeAsync(arg);

    /// <summary>
    ///     Handles the click event on the row and invokes the OnClick callback with the current Item.
    /// </summary>
    /// <param name="args">The mouse event arguments.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task RowClicked(MouseEventArgs args) {
        await OnClick.InvokeAsync(Item);
    }
}