using System.Diagnostics.CodeAnalysis;

namespace TnTComponents.Forms;

public class TnTCheckbox : TnTInputBase<bool> {
    protected override bool BindValue { get => CurrentValue; set => CurrentValue = value; }

    protected override string InputType => "checkbox";

    protected override bool TryParseValueFromString(string? value, out bool result, [NotNullWhen(false)] out string? validationErrorMessage)
        => throw new NotSupportedException($"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");
}