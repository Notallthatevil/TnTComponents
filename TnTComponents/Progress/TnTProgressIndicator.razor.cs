using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Text;
using TnTComponents.Core;

namespace TnTComponents;

/// <summary>
///     A component that represents a progress indicator.
/// </summary>
public partial class TnTProgressIndicator{

    /// <summary>
    ///     Gets or sets the appearance of the progress indicator.
    /// </summary>
    [Parameter]
    public ProgressAppearance Appearance { get; set; }

    /// <summary>
    ///     Gets or sets the child content to be rendered inside the progress indicator.
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-progress-linear", Appearance == ProgressAppearance.Linear)
        .AddSize(Size)
        .Build();

    /// <inheritdoc />
    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddVariable("progress-color", $"var(--tnt-color-{ProgressColor.ToCssClassName()})")
        .AddStyle("background", $"conic-gradient(from 0deg, currentColor {360 * (Value.GetValueOrDefault() / Max)}deg, transparent {360 * (Value.GetValueOrDefault() / Max)}deg);", Value is not null)
        .Build();

    /// <summary>
    ///     Gets or sets the maximum value of the progress indicator.
    /// </summary>
    [Parameter]
    public double Max { get; set; } = 100.0;

    /// <summary>
    ///     Gets or sets the color of the progress indicator.
    /// </summary>
    [Parameter]
    public TnTColor ProgressColor { get; set; } = TnTColor.Primary;

    /// <summary>
    ///     Gets or sets a value indicating whether the progress indicator is visible.
    /// </summary>
    [Parameter]
    public bool Show { get; set; } = true;

    /// <summary>
    ///     Gets or sets the size of the progress indicator.
    /// </summary>
    [Parameter]
    public Size Size { get; set; } = Size.Medium;

    /// <summary>
    ///     Gets or sets the current value of the progress indicator.
    /// </summary>
    [Parameter]
    public double? Value { get; set; }
}

/// <summary>
///     Specifies the appearance of the progress indicator.
/// </summary>
public enum ProgressAppearance {

    /// <summary>
    ///     A ring-shaped progress indicator.
    /// </summary>
    Ring,

    /// <summary>
    ///     A linear progress indicator.
    /// </summary>
    Linear
}