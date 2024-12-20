using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using TnTComponents.Core;

namespace TnTComponents;

/// <summary>
///     Represents a skeleton component used for displaying loading placeholders.
/// </summary>
public class TnTSkeleton : TnTComponentBase {

    /// <summary>
    ///     Gets or sets a value indicating whether the skeleton should be animated.
    /// </summary>
    [Parameter]
    public bool Animated { get; set; } = true;

    /// <summary>
    ///     Gets or sets the appearance of the skeleton (e.g., Rectangle or Circle).
    /// </summary>
    [Parameter]
    public SkeletonAppearance Appearance { get; set; }

    /// <summary>
    ///     Gets or sets the background color of the skeleton.
    /// </summary>
    [Parameter]
    public TnTColor? BackgroundColor { get; set; } = TnTColor.SurfaceVariant;

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-skeleton")
        .AddBorderRadius(Appearance == SkeletonAppearance.Circle ? new TnTBorderRadius(10) : null)
        .AddClass("tnt-animated", Animated)
        .AddBackgroundColor(BackgroundColor)
        .Build();

    /// <inheritdoc />
    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddVariable("bg-color", $"var(--tnt-color-{BackgroundColor.ToCssClassName()})", Animated && BackgroundColor.HasValue && BackgroundColor.Value != TnTColor.None)
        .Build();

    /// <inheritdoc />
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