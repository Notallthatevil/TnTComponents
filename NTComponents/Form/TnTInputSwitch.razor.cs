using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using NTComponents.Core;

namespace NTComponents;

/// <summary>
///     Represents a switch input component.
/// </summary>
public partial class TnTInputSwitch {

    /// <inheritdoc />
    public override InputType Type => InputType.Checkbox;

    /// <inheritdoc />
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out bool result, [NotNullWhen(false)] out string? validationErrorMessage) => throw new NotSupportedException();
}