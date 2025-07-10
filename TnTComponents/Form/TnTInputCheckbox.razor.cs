using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System.Diagnostics.CodeAnalysis;
using TnTComponents.Core;

namespace TnTComponents;

/// <summary>
///     Represents a checkbox input component.
/// </summary>
public partial class TnTInputCheckbox {
    /// <inheritdoc />
    public override InputType Type => InputType.Checkbox;

    /// <inheritdoc />
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out bool result, [NotNullWhen(false)] out string? validationErrorMessage) => throw new NotSupportedException();
}