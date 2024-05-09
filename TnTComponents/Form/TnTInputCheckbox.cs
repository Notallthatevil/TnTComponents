using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using TnTComponents.Core;

namespace TnTComponents;

public class TnTInputCheckbox : TnTInputBase<bool> {

    [Parameter]
    public TnTColor CheckMarkColor { get; set; } = TnTColor.Surface;

    [Parameter]
    public TnTColor FillColor { get; set; } = TnTColor.Primary;

    public override string FormCssClass => CssClassBuilder.Create(base.FormCssClass).Build();

    public override string? FormCssStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddVariable("outline-color", $"var(--tnt-color-{OutlineColor.ToCssClassName()})")
        .AddVariable("fill-color", $"var(--tnt-color-{FillColor.ToCssClassName()})")
        .AddVariable("check-mark-color", $"var(--tnt-color-{CheckMarkColor.ToCssClassName()})")
        .Build();

    [Parameter]
    public TnTColor OutlineColor { get; set; } = TnTColor.Outline;

    public override InputType Type => InputType.Checkbox;

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