using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using TnTComponents.Core;

namespace TnTComponents;

public class TnTDivider : ComponentBase {

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public TnTColor? Color { get; set; } = TnTColor.OutlineVariant;

    public string? ElementClass => CssClassBuilder.Create()
            .AddClass($"tnt-divider-{Orientation.ToString().ToLower()}")
        .Build();

    public string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddVariable("divider-color", $"var(--tnt-color-{Color.ToCssClassName()})", Color.HasValue)
        .Build();

    public Orientation Orientation { get; set; } = Orientation.Horizontal;

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "div");
        builder.AddMultipleAttributes(10, AdditionalAttributes);
        builder.AddAttribute(20, "class", ElementClass);
        builder.AddAttribute(30, "style", ElementStyle);
        builder.CloseElement();
    }
}