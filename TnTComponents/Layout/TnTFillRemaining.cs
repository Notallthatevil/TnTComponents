using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using TnTComponents.Core;

namespace TnTComponents;
/// <summary>
/// A container that fills the remaining height of the screen.
/// </summary>
public partial class TnTFillRemaining : TnTComponentBase {
    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-fill-remaining")
        .Build();

    /// <inheritdoc />
    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    /// <summary>
    /// The content to be rendered inside the container.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }


    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "div");
        builder.AddMultipleAttributes(10, AdditionalAttributes);
        builder.AddAttribute(20, "class", ElementClass);
        builder.AddAttribute(30, "style", ElementStyle);
        builder.AddAttribute(40, "lang", ElementLang);
        builder.AddAttribute(50, "id", ElementId);
        builder.AddAttribute(60, "title", ElementTitle);
        builder.AddElementReferenceCapture(70, e => Element = e);
        builder.AddContent(80, ChildContent);
        builder.CloseElement();
    }
}
