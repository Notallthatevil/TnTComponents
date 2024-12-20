using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using TnTComponents.Core;
using TnTComponents.Interfaces;

namespace TnTComponents;

/// <summary>
///     Represents a child tab component within a <see cref="TnTTabView" />.
/// </summary>
public class TnTTabChild : TnTComponentBase, ITnTInteractable, IDisposable {

    /// <summary>
    ///     Gets or sets the content to be rendered inside the tab.
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    /// <inheritdoc />
    [Parameter]
    public bool Disabled { get; set; }

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-tab-child")
        .AddDisabled(Disabled)
        .Build();

    /// <inheritdoc />
    [Parameter]
    public string? ElementName { get; set; }

    /// <inheritdoc />
    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    /// <inheritdoc />
    [Parameter]
    public bool EnableRipple { get; set; }

    /// <summary>
    ///     Gets or sets the icon to be displayed in the tab header.
    /// </summary>
    [Parameter]
    public TnTIcon? Icon { get; set; }

    /// <summary>
    ///     Gets or sets the label for the tab.
    /// </summary>
    [Parameter, EditorRequired]
    public string Label { get; set; } = default!;

    /// <inheritdoc />
    [Parameter]
    public TnTColor? OnTintColor { get; set; }

    /// <summary>
    ///     Gets or sets the template for the tab header.
    /// </summary>
    [Parameter]
    public RenderFragment? TabHeaderTemplate { get; set; }

    /// <inheritdoc />
    [Parameter]
    public TnTColor? TintColor { get; set; }

    /// <summary>
    ///     Gets or sets the parent <see cref="TnTTabView" /> context.
    /// </summary>
    [CascadingParameter]
    private TnTTabView _context { get; set; } = default!;

    /// <summary>
    ///     Disposes the tab child and removes it from the parent context.
    /// </summary>
    public void Dispose() {
        _context.RemoveTabChild(this);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "div");
        builder.AddMultipleAttributes(10, AdditionalAttributes);
        builder.AddAttribute(20, "class", ElementClass);
        builder.AddAttribute(30, "style", ElementStyle);
        builder.AddAttribute(40, "title", ElementTitle);
        builder.AddAttribute(50, "id", ElementId);
        builder.AddAttribute(60, "lang", ElementLang);
        builder.AddAttribute(70, "name", ElementName);
        builder.AddElementReferenceCapture(80, e => Element = e);
        builder.AddContent(90, ChildContent);
        builder.CloseElement();
    }

    /// <inheritdoc />
    protected override void OnInitialized() {
        base.OnInitialized();
        if (_context is null) {
            throw new InvalidOperationException($"A {nameof(TnTTabChild)} must be a child of {nameof(TnTTabView)}");
        }
        _context.AddTabChild(this);
    }
}
