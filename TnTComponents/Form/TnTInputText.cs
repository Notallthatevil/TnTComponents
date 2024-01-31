using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using TnTComponents.Enum;

namespace TnTComponents;

public class TnTInputText : TnTInputBase<string?> {

    [Parameter]
    public TextInputType InputType { get; set; } = TextInputType.Text;

    public override InputType Type => InputType.ToInputType();

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out string? result, [NotNullWhen(false)] out string? validationErrorMessage) {
        result = value;
        validationErrorMessage = null;
        return true;
    }
}