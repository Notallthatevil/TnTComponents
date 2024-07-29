using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Text;
using TnTComponents.Core;

namespace TnTComponents;

public class TnTProgressIndicator : TnTComponentBase {

    [Parameter]
    public ProgressAppearance Appearance { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-progress-linear", Appearance == ProgressAppearance.Linear)
        .AddSize(Size)
        .Build();

    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddVariable("progress-color", $"var(--tnt-color-{ProgressColor.ToCssClassName()})")
        .AddStyle("background", $"conic-gradient(from 0deg, currentColor {360 * (Value.GetValueOrDefault() / Max)}deg, transparent {360 * (Value.GetValueOrDefault() / Max)}deg);", Value is not null)
        .Build();

    [Parameter]
    public double Max { get; set; } = 100.0;

    [Parameter]
    public TnTColor ProgressColor { get; set; } = TnTColor.Primary;

    [Parameter]
    public Size Size { get; set; } = Size.Default;

    [Parameter]
    public double? Value { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
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