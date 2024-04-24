using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using TnTComponents.Core;

namespace TnTComponents;

public class TnTDivider : ComponentBase {

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    public string? CssClass => CssClassBuilder.Create()
        .AddClass($"tnt-divider-{Orientation.ToString().ToLower()}")
        .Build();

    public Orientation Orientation { get; set; } = Orientation.Horizontal;

    [Parameter]
    public TnTColor? Color { get; set; } = TnTColor.OutlineVariant;

    public string? CssStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddVariable("divider-color", $"var(--tnt-color-{Color.ToCssClassName()})", Color.HasValue)
        .Build();

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "div");
        builder.AddMultipleAttributes(10, AdditionalAttributes);
        builder.AddAttribute(20, "class", CssClass);
        builder.AddAttribute(30, "style", CssStyle);
        builder.CloseElement();
    }
}