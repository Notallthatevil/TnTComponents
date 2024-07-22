using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using TnTComponents.Core;
using TnTComponents.Interfaces;

namespace TnTComponents;

public class TnTRow : ComponentBase, ITnTComponentBase, ITnTFlexBox {

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public AlignContent? AlignContent { get; set; }

    [Parameter]
    public AlignItems? AlignItems { get; set; }

    [Parameter]
    public bool? AutoFocus { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    public string? CssClass => CssClassBuilder.Create()
        .AddClass("tnt-row")
        .AddFlexBox(this)
        .Build();

    public string? CssStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    [Parameter]
    public LayoutDirection? Direction { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    public ElementReference Element { get; }

    [Parameter]
    public string? Id { get; set; }

    [Parameter]
    public JustifyContent? JustifyContent { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "div");
        builder.AddMultipleAttributes(10, AdditionalAttributes);
        builder.AddAttribute(20, "class", CssClass);
        builder.AddAttribute(30, "style", CssStyle);
        builder.AddContent(40, ChildContent);
        builder.CloseElement();
    }
}