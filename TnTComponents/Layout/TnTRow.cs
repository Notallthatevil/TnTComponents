using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using TnTComponents.Core;
using TnTComponents.Interfaces;

namespace TnTComponents;

public class TnTRow : TnTComponentBase, ITnTComponentBase, ITnTFlexBox {

    [Parameter]
    public AlignContent? AlignContent { get; set; }

    [Parameter]
    public AlignItems? AlignItems { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-row")
        .AddFlexBox(this)
        .Build();

    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    [Parameter]
    public LayoutDirection? Direction { get; set; }

    [Parameter]
    public JustifyContent? JustifyContent { get; set; }

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