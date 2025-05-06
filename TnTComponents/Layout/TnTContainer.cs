using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using TnTComponents.Core;
using TnTComponents.Interfaces;

namespace TnTComponents;

/// <summary>
///     Represents a container component.
/// </summary>
public class TnTContainer : TnTComponentBase, ITnTComponentBase, ITnTFlexBox {

    /// <inheritdoc />
    [Parameter]
    public AlignContent? AlignContent { get; set; }

    /// <inheritdoc />
    [Parameter]
    public AlignItems? AlignItems { get; set; }

    /// <summary>
    ///     The content to be rendered inside the container.
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    /// <inheritdoc />
    [Parameter]
    public LayoutDirection? Direction { get; set; }

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddFlexBox(this)
        .AddClass("tnt-container")
        .Build();

    /// <inheritdoc />
    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    /// <inheritdoc />
    [Parameter]
    public JustifyContent? JustifyContent { get; set; }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "div");
        builder.AddMultipleAttributes(10, AdditionalAttributes);
        builder.AddAttribute(20, "class", ElementClass);
        builder.AddAttribute(30, "style", ElementStyle);
        builder.AddAttribute(40, "autofocus", AutoFocus);
        builder.AddAttribute(50, "lang", ElementLang);
        builder.AddAttribute(60, "title", ElementTitle);
        builder.AddAttribute(70, "id", ElementId);
        builder.AddElementReferenceCapture(80, e => Element = e);
        builder.AddContent(90, ChildContent);
        builder.CloseElement();
    }
}