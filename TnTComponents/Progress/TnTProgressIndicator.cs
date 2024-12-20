using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Text;
using TnTComponents.Core;

namespace TnTComponents;

/// <summary>
///     A component that represents a progress indicator.
/// </summary>
public class TnTProgressIndicator : TnTComponentBase {

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
    public Size Size { get; set; } = Size.Default;

    /// <summary>
    ///     Gets or sets the current value of the progress indicator.
    /// </summary>
    [Parameter]
    public double? Value { get; set; }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        if (Show) {
            builder.OpenElement(0, "progress");
            builder.AddMultipleAttributes(10, AdditionalAttributes);
            builder.AddAttribute(20, "class", ElementClass);
            builder.AddAttribute(30, "max", Max);
            builder.AddAttribute(40, "value", Value);
            builder.AddAttribute(50, "style", ElementStyle);
            builder.AddAttribute(60, "autofocus", AutoFocus);
            builder.AddAttribute(70, "lang", ElementLang);
            builder.AddAttribute(80, "title", ElementTitle);
            builder.AddAttribute(90, "id", ElementId);
            builder.AddElementReferenceCapture(100, e => Element = e);
            builder.AddContent(110, ChildContent);
            builder.CloseElement();
        }
    }
}