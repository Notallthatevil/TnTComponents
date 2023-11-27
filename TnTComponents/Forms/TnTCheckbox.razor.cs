using System.Diagnostics.CodeAnalysis;

namespace TnTComponents.Forms;

public partial class TnTCheckbox {
    protected virtual bool Switch { get; }

    protected override bool TryParseValueFromString(string? value, out bool result, [NotNullWhen(false)] out string? validationErrorMessage)
        => throw new NotSupportedException($"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");
}