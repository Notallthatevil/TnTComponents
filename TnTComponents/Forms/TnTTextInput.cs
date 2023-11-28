using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using TnTComponents.Enum;

namespace TnTComponents.Forms;

public class TnTTextInput : TnTInputBase<string?> {

    [Parameter]
    public TextInputType Type { get; set; }

    protected override string? BindValue { get => CurrentValueAsString; set => CurrentValueAsString = value; }

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out string result, [NotNullWhen(false)] out string? validationErrorMessage) {
        result = value;
        validationErrorMessage = null;
#pragma warning disable CS8762 // Parameter must have a non-null value when exiting in some condition.
        return true;
#pragma warning restore CS8762 // Parameter must have a non-null value when exiting in some condition.
    }

    protected override string GetInputType() => Type switch {
        TextInputType.Text => "text",
        TextInputType.Email => "email",
        TextInputType.Password => "password",
        TextInputType.Tel => "tel",
        TextInputType.Url => "url",
        _ => throw new NotImplementedException()
    };
}