using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using TnTComponents.Core;
using TnTComponents.Layout;

namespace TnTComponents;

public class TnTBody : TnTLayoutComponent {
    public override string? ElementClass => CssClassBuilder.Create(base.ElementClass!)
        .AddClass("tnt-body")
        .Build();

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "div");
        builder.AddMultipleAttributes(10, AdditionalAttributes);
        builder.AddAttribute(20, "class", ElementClass);
        builder.AddAttribute(30, "style", ElementStyle);
        builder.AddAttribute(40, "lang", ElementLang);
        builder.AddAttribute(50, "id", ElementId);
        builder.AddAttribute(60, "title", ElementTitle);
        builder.AddAttribute(61, "onscroll", "TnTComponents.bodyScrollListener(event)");
        builder.AddElementReferenceCapture(70, e => Element = e);
        builder.AddContent(80, ChildContent);
        builder.CloseElement();
    }
}