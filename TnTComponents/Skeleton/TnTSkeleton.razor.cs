using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using TnTComponents.Core;

namespace TnTComponents;

/// <summary>
///     Represents a skeleton component used for displaying loading placeholders.
/// </summary>
public partial class TnTSkeleton {

    /// <summary>
    ///     If provided, the skeleton will animate its background color.
    /// </summary>
    [Parameter]
    public TnTColor? AnimatedColor { get; set; } = TnTColor.OnPrimaryContainer;

    /// <summary>
    ///     Gets or sets the appearance of the skeleton (e.g., Rectangle or Circle).
    /// </summary>
    [Parameter]
    public SkeletonAppearance Appearance { get; set; }

    /// <summary>
    ///     Gets or sets the background color of the skeleton.
    /// </summary>
    [Parameter]
    public TnTColor? BackgroundColor { get; set; } = TnTColor.PrimaryContainer;

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-skeleton")
        .AddClass("tnt-skeleton-round", Appearance == SkeletonAppearance.Round)
        .AddClass("tnt-animated", AnimatedColor.HasValue)
        .AddBackgroundColor(BackgroundColor)
        .Build();

    /// <inheritdoc />
    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddVariable("tnt-skeleton-bg-color", $"var(--tnt-color-{BackgroundColor.ToCssClassName()})")
        .AddVariable("tnt-skeleton-shimmer-color", $"var(--tnt-color-{AnimatedColor.ToCssClassName()})", AnimatedColor.HasValue)
        .Build();
}

/// <summary>
///     Specifies the appearance of a skeleton component.
/// </summary>
public enum SkeletonAppearance {

    /// <summary>
    ///     A rectangular skeleton appearance.
    /// </summary>
    Square,

    /// <summary>
    ///     A circular skeleton appearance.
    /// </summary>
    Round
}