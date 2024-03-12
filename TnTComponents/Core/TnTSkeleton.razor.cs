using Microsoft.AspNetCore.Components;
using TnTComponents.Core;

namespace TnTComponents;
public partial class TnTSkeleton {
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public SkeletonAppearance Appearance { get; set; }

    [Parameter]
    public bool Animated { get; set; } = true;

    [Parameter]
    public TnTColor? BackgroundColor { get; set; } = TnTColor.SurfaceVariant;

    public ElementReference Element { get; set; }

    public string CssClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-skeleton")
        .AddBorderRadius(Appearance == SkeletonAppearance.Circle ? new TnTBorderRadius(10) : null)
        .AddClass("tnt-animated", Animated)
        .AddBackgroundColor(BackgroundColor)
        .Build();

    public string? CssStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddVariable("bg-color", $"var(--tnt-color-{BackgroundColor.ToCssClassName()})", Animated && BackgroundColor.HasValue && BackgroundColor.Value != TnTColor.None)
        .Build();
}
