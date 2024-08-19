using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using TnTComponents.Core;

namespace TnTComponents;

public class TnTSkeleton : TnTComponentBase {

    [Parameter]
    public bool Animated { get; set; } = true;

    [Parameter]
    public SkeletonAppearance Appearance { get; set; }

    [Parameter]
    public TnTColor? BackgroundColor { get; set; } = TnTColor.SurfaceVariant;

    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-skeleton")
        .AddBorderRadius(Appearance == SkeletonAppearance.Circle ? new TnTBorderRadius(10) : null)
        .AddClass("tnt-animated", Animated)
        .AddBackgroundColor(BackgroundColor)
        .Build();

    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddVariable("bg-color", $"var(--tnt-color-{BackgroundColor.ToCssClassName()})", Animated && BackgroundColor.HasValue && BackgroundColor.Value != TnTColor.None)
        .Build();

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "div");
        builder.AddMultipleAttributes(10, AdditionalAttributes);
        builder.AddAttribute(20, "class", ElementClass);
        builder.AddAttribute(30, "style", ElementStyle);
        builder.AddAttribute(40, "lang", ElementLang);
        builder.AddAttribute(50, "title", ElementTitle);
        builder.AddAttribute(60, "id", ElementId);
        builder.AddElementReferenceCapture(70, e => Element = e);
        builder.CloseElement();


    }
}