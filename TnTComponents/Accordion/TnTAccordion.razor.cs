using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TnTComponents.Core;

namespace TnTComponents;

/// <summary>
///     Represents an accordion component that can contain multiple child items.
/// </summary>
public partial class TnTAccordion {

    /// <summary>
    ///     Gets or sets the content to be rendered inside the accordion.
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    /// <summary>
    ///     Gets or sets the body color of the accordion content.
    /// </summary>
    [Parameter]
    public TnTColor ContentBodyColor { get; set; } = TnTColor.SurfaceVariant;

    /// <summary>
    ///     Gets or sets the text color of the accordion content.
    /// </summary>
    [Parameter]
    public TnTColor ContentTextColor { get; set; } = TnTColor.OnSurfaceVariant;

    public override string? ElementClass => CssClassBuilder.Create()
            .AddFromAdditionalAttributes(AdditionalAttributes)
            .AddClass("tnt-accordion")
            .AddClass("tnt-limit-one-expanded", LimitToOneExpanded)
            .Build();

    public override string? ElementStyle => CssStyleBuilder.Create()
            .AddFromAdditionalAttributes(AdditionalAttributes)
            .Build();

    /// <summary>
    ///     Gets or sets the body color of the accordion header.
    /// </summary>
    [Parameter]
    public TnTColor HeaderBodyColor { get; set; } = TnTColor.PrimaryContainer;

    /// <summary>
    ///     Gets or sets the text color of the accordion header.
    /// </summary>
    [Parameter]
    public TnTColor HeaderTextColor { get; set; } = TnTColor.OnPrimaryContainer;

    /// <summary>
    ///     Gets or sets the tint color of the accordion header.
    /// </summary>
    [Parameter]
    public TnTColor HeaderTintColor { get; set; } = TnTColor.SurfaceTint;

    public override string? JsModulePath => "./_content/TnTComponents/Accordion/TnTAccordion.razor.js";

    /// <summary>
    ///     Gets or sets a value indicating whether only one child can be expanded at a time.
    /// </summary>
    [Parameter]
    public bool LimitToOneExpanded { get; set; }

    /// <summary>
    ///     Gets a value indicating whether the accordion can be opened by default.
    /// </summary>
    internal bool AllowOpenByDefault => _parentAccordion is null;

    /// <summary>
    ///     Gets or sets a value indicating whether an expanded child has been found.
    /// </summary>
    internal bool FoundExpanded { get; set; }

    /// <summary>
    ///     Gets or sets the parent accordion if this accordion is nested inside another accordion.
    /// </summary>
    [CascadingParameter]
    private TnTAccordion? _parentAccordion { get; set; }

    private readonly List<TnTAccordionChild> _children = [];

    /// <summary>
    ///     Closes all child accordion items asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task CloseAllAsync() {
        foreach (var child in _children) {
            await child.CloseAsync();
        }
    }

    /// <summary>
    ///     Registers a child accordion item.
    /// </summary>
    /// <param name="child">The child accordion item to register.</param>
    public void RegisterChild(TnTAccordionChild child) {
        if (child is not null) {
            _children.Add(child);
            StateHasChanged();
        }
    }

    /// <summary>
    ///     Removes a child accordion item.
    /// </summary>
    /// <param name="child">The child accordion item to remove.</param>
    public void RemoveChild(TnTAccordionChild child) {
        if (child is not null) {
            _children.Remove(child);
            StateHasChanged();
        }
    }
}