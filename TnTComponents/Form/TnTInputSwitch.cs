using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using TnTComponents.Core;

namespace TnTComponents;

public class TnTInputSwitch : TnTInputBase<bool> {

    [Parameter]
    public TnTColor CheckedFillColor { get; set; } = TnTColor.Primary;

    [Parameter]
    public TnTColor CheckedKnobColor { get; set; } = TnTColor.OnPrimary;

    //public override string FormCssClass => CssClassBuilder.Create(base.FormCssClass).AddClass("tnt-alternative").Build();

    //public override string? FormCssStyle => CssStyleBuilder.Create()
    //    .AddFromAdditionalAttributes(AdditionalAttributes)
    //    .AddVariable("outline-color", $"var(--tnt-color-{OutlineColor.ToCssClassName()})")
    //    .AddVariable("unchecked-fill-color", $"var(--tnt-color-{UncheckedFillColor.ToCssClassName()})")
    //    .AddVariable("unchecked-knob-color", $"var(--tnt-color-{UncheckedKnobColor.ToCssClassName()})")
    //    .AddVariable("checked-fill-color", $"var(--tnt-color-{CheckedFillColor.ToCssClassName()})")
    //    .AddVariable("checked-knob-color", $"var(--tnt-color-{CheckedKnobColor.ToCssClassName()})")
    //    .Build();

    [Parameter]
    public TnTColor OutlineColor { get; set; } = TnTColor.Outline;

    public override InputType Type => InputType.Checkbox;

    [Parameter]
    public TnTColor UncheckedFillColor { get; set; } = TnTColor.SurfaceVariant;

    [Parameter]
    public TnTColor UncheckedKnobColor { get; set; } = TnTColor.OnSurfaceVariant;

    protected override void OnInitialized() {
        base.OnInitialized();
        if (Label is null) {
            throw new InvalidOperationException($"{GetType().Name} must be a child of {nameof(TnTLabel)}");
        }
    }

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out bool result, [NotNullWhen(false)] out string? validationErrorMessage) {
        throw new NotSupportedException();
    }
}