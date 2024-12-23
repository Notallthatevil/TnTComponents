using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace TnTComponents;

/// <summary>
///     Represents a text input component with various text input types.
/// </summary>
public class TnTInputText : TnTInputBase<string?> {

    /// <inheritdoc />
    [Parameter]
    public TextInputType InputType { get; set; } = TextInputType.Text;

    /// <inheritdoc />
    public override InputType Type => InputType.ToInputType();

    /// <inheritdoc />
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out string? result, [NotNullWhen(false)] out string? validationErrorMessage) {
        result = value;
        validationErrorMessage = null;
        return true;
    }
}