using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System.Diagnostics.CodeAnalysis;
using TnTComponents.Core;

namespace TnTComponents;

/// <summary>
///     Represents a checkbox input component.
/// </summary>
public class TnTInputCheckbox : TnTInputBase<bool> {
    public override InputType Type => InputType.Checkbox;

    protected override void RenderCustomContent(RenderTreeBuilder builder) {
        builder.OpenElement(0, "div");
        builder.AddAttribute(10, "class", "tnt-checkbox");
        builder.CloseElement();
    }

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out bool result, [NotNullWhen(false)] out string? validationErrorMessage) {
        throw new NotSupportedException();
    }
}