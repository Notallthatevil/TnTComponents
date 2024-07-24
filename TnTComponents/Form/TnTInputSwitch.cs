using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using TnTComponents.Core;

namespace TnTComponents;

public class TnTInputSwitch : TnTInputBase<bool> {

    public override InputType Type => InputType.Checkbox;

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out bool result, [NotNullWhen(false)] out string? validationErrorMessage) {
        throw new NotSupportedException();
    }
}