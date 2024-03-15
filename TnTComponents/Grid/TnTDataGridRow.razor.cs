using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TnTComponents.Core;
using TnTComponents.Grid.Infrastructure;

namespace TnTComponents.Grid;

[CascadingTypeParameter(nameof(TGridItem))]
public partial class TnTDataGridRow<TGridItem> : IHandleEvent {

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    public string? CssClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-clickable", OnClick.HasDelegate)
        .Build();

    public string? CssStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    /// <summary>
    /// Gets or sets the reference to the item that holds this row's values.
    /// </summary>
    [Parameter]
    public TGridItem? Item { get; set; }

    [Parameter]
    public EventCallback<TGridItem?> OnClick { get; set; }

    /// <summary>
    /// Gets or sets the index of this row. When FluentDataGrid is virtualized, this value is not used.
    /// </summary>
    [Parameter]
    public int? RowIndex { get; set; }

    internal string RowId { get; set; } = string.Empty;

    private readonly Dictionary<string, TnTDataGridCell<TGridItem>> cells = [];

    Task IHandleEvent.HandleEventAsync(EventCallbackWorkItem callback, object? arg) => callback.InvokeAsync(arg);

    private async Task RowClicked(MouseEventArgs args) {
        await OnClick.InvokeAsync(Item);
    }
}