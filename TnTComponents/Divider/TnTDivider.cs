using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using TnTComponents.Core;

namespace TnTComponents;

public class TnTDivider : TnTComponentBase {

    [Parameter]
    public TnTColor? Color { get; set; } = TnTColor.OutlineVariant;

    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass($"tnt-divider-{Orientation.ToString().ToLower()}")
        .Build();

    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddVariable("divider-color", $"var(--tnt-color-{Color.ToCssClassName()})", Color.HasValue)
        .Build();

    public Orientation Orientation { get; set; } = Orientation.Horizontal;

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "div");
        builder.AddMultipleAttributes(10, AdditionalAttributes);
        builder.AddAttribute(20, "class", ElementClass);
        builder.AddAttribute(30, "style", ElementStyle);
        builder.AddAttribute(40, "title", ElementTitle);
        builder.AddAttribute(50, "lang", ElementLang);
        builder.AddAttribute(60, "id", ElementId);
        builder.CloseElement();
    }
}