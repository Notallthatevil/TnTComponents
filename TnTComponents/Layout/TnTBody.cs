using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using TnTComponents.Core;
using TnTComponents.Layout;

namespace TnTComponents;

/// <summary>
/// Represents the body of a TnT layout component, typically used to contain the main content area.
/// </summary>
public class TnTBody : TnTLayoutComponent {
    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create(base.ElementClass!)
        .AddClass("tnt-body")
        .Build();

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