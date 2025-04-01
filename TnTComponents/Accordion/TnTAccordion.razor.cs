using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TnTComponents.Core;
using System.Diagnostics.CodeAnalysis;

namespace TnTComponents;

[method: DynamicDependency(nameof(SetAsOpened))]
[method: DynamicDependency(nameof(SetAsClosed))]
public partial class TnTAccordion() {

    /// <summary>
    /// The content for this accordion container. Should contain one or more <see cref="TnTAccordionChild"/> components.
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    /// <summary>
    /// The body color of the content.
    /// </summary>
    [Parameter]
    public TnTColor ContentBodyColor { get; set; } = TnTColor.SurfaceVariant;

    /// <summary>
    /// The text color of the content.
    /// </summary>
    [Parameter]
    public TnTColor ContentTextColor { get; set; } = TnTColor.OnSurfaceVariant;

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create()
            .AddFromAdditionalAttributes(AdditionalAttributes)
            .AddClass("tnt-accordion")
            .AddClass("tnt-limit-one-expanded", LimitToOneExpanded)
            .Build();

    /// <inheritdoc />
    public override string? ElementStyle => CssStyleBuilder.Create()
            .AddFromAdditionalAttributes(AdditionalAttributes)
            .Build();

    /// <summary>
    /// The body color of the header.
    /// </summary>
    [Parameter]
    public TnTColor HeaderBodyColor { get; set; } = TnTColor.PrimaryContainer;

    /// <summary>
    /// The text color of the header.
    /// </summary>
    [Parameter]
    public TnTColor HeaderTextColor { get; set; } = TnTColor.OnPrimaryContainer;

    /// <summary>
    /// The color of the ripple effect when the accordion is interacted with.
    /// </summary>
    [Parameter]
    public TnTColor HeaderTintColor { get; set; } = TnTColor.SurfaceTint;

    /// <inheritdoc />
    public override string? JsModulePath => "./_content/TnTComponents/Accordion/TnTAccordion.razor.js";

    /// <summary>
    /// When set, limits only one child to be expanded at a time, closing the others automatically.
    /// </summary>
    [Parameter]
    public bool LimitToOneExpanded { get; set; }

    /// <summary>
    /// Used internally to determine if the accordion is allowed to be open by default.
    /// </summary>
    internal bool AllowOpenByDefault => _parentAccordion is null;

    /// <summary>
    /// Used internally to track whether an expanded child has been found.
    /// </summary>
    internal bool FoundExpanded;

    /// <summary>
    /// Gets or sets the parent accordion.
    /// </summary>
    [CascadingParameter]
    private TnTAccordion? _parentAccordion { get; set; }

    private readonly Dictionary<int, TnTAccordionChild> _children = new();
    private static int _elementId = 0;

    /// <summary>
    /// Closes all child accordion items asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task CloseAllAsync() {
        foreach (var (_, child) in _children) {
            await child.CloseAsync();
        }
    }

    /// <summary>
    /// Registers a child accordion item.
    /// </summary>
    /// <param name="child">The child accordion item to register.</param>
    public void RegisterChild(TnTAccordionChild child) {
        if (child is not null) {
            if (child._elementId == int.MinValue) {
                child._elementId = _elementId;
                Interlocked.Increment(ref _elementId);
            }
            _children.TryAdd(child._elementId, child);
            StateHasChanged();
        }
    }

    /// <summary>
    /// Removes a child accordion item.
    /// </summary>
    /// <param name="child">The child accordion item to remove.</param>
    public void RemoveChild(TnTAccordionChild child) {
        if (child is not null) {
            _children.Remove(child._elementId);
            StateHasChanged();
        }
    }

    /// <summary>
    /// Sets the specified child accordion item as opened.
    /// </summary>
    /// <param name="elementId">The element ID of the child accordion item to open.</param>
    [JSInvokable]
    public async Task SetAsOpened(int elementId) {
        if (_children.TryGetValue(elementId, out var child) && child?._open == false) {
            if (LimitToOneExpanded) {
                await CloseAllAsync();
            }
            child._open = true;
            await child.OnOpenCallback.InvokeAsync();
        }
        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Sets the specified child accordion item as closed.
    /// </summary>
    /// <param name="elementId">The element ID of the child accordion item to close.</param>
    [JSInvokable]
    public async Task SetAsClosed(int elementId) {
        if (_children.TryGetValue(elementId, out var child) && child?._open == true) {
            if (LimitToOneExpanded) {
                await CloseAllAsync();
            }
            child._open = false;
            await child.OnCloseCallback.InvokeAsync();
        }
        await InvokeAsync(StateHasChanged);
    }
}
