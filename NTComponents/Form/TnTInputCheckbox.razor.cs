using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System.Diagnostics.CodeAnalysis;
using NTComponents.Core;

namespace NTComponents;

/// <summary>
///     Represents a checkbox input component.
/// </summary>
public partial class TnTInputCheckbox {
    /// <inheritdoc />
    public override InputType Type => InputType.Checkbox;

    /// <inheritdoc />
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out bool result, [NotNullWhen(false)] out string? validationErrorMessage) => throw new NotSupportedException();
}