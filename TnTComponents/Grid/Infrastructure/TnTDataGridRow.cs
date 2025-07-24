using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TnTComponents.Core;
using TnTComponents.Grid.Columns;
using TnTComponents.Grid.Infrastructure;

namespace TnTComponents.Grid.Infrastructure;

/// <summary>
///     Represents a row within a TnTDataGrid component that displays content for a specific grid item. This component defines the layout and behavior of individual rows in the grid.
/// </summary>
/// <typeparam name="TGridItem">The type of data item displayed in the row.</typeparam>
[CascadingTypeParameter(nameof(TGridItem))]
public abstract partial class TnTDataGridRow<TGridItem> : ComponentBase {

    [CascadingParameter]
    internal TnTInternalGridContext<TGridItem> Context { get; set; } = default!;

    protected override void OnParametersSet() {
        base.OnParametersSet();
        ArgumentNullException.ThrowIfNull(Context, nameof(Context));
    }

    public async Task RefreshAsync() {
        await InvokeAsync(StateHasChanged);
    }
    ///// <summary>
    /////     Additional attributes to apply to the row element. These attributes are captured from unmatched attributes during component instantiation.
    ///// </summary>
    //[Parameter(CaptureUnmatchedValues = true)]
    //public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    ///// <summary>
    /////     The content to be rendered inside the component. This content can include static text, HTML, or other Blazor components.
    ///// </summary>
    //[Parameter]
    //public RenderFragment? ChildContent { get; set; }

    ///// <summary>
    /////     Gets the CSS class string generated for this row. Classes are built from the additional attributes provided to the component and includes "tnt-clickable" when the OnClick event has a delegate.
    ///// </summary>
    //public string? ElementClass => CssClassBuilder.Create()
    //    .AddFromAdditionalAttributes(AdditionalAttributes)
    //    .AddClass("tnt-clickable", OnClick.HasDelegate)
    //    .Build();

    ///// <summary>
    /////     Gets the CSS style string generated for this row. Styles are built from the additional attributes provided to the component.
    ///// </summary>
    //public string? ElementStyle => CssStyleBuilder.Create()
    //    .AddFromAdditionalAttributes(AdditionalAttributes)
    //    .Build();

    ///// <summary>
    /////     The reference to the item that holds this row's values. This provides access to the specific data item in the grid that this row represents.
    ///// </summary>
    //[Parameter, EditorRequired]
    //public TGridItem? Item { get; set; }

    ///// <summary>
    /////     The event callback that is invoked when the row is clicked. The callback receives the Item as its parameter.
    ///// </summary>
    //[Parameter]
    //public EventCallback<TGridItem?> OnClick { get; set; }

    ///// <summary>
    /////     Handles the click event on the row and invokes the OnClick callback with the current Item.
    ///// </summary>
    ///// <returns>A task representing the asynchronous operation.</returns>
    //private Task RowClicked() => OnClick.InvokeAsync(Item);
}