using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace TnTComponents.Forms;
public partial class TnTCheckbox {

    [Parameter]
    public override string BaseCssClass { get; set; } = "tnt-form-field-checkbox";

    //protected override void OnChange(ChangeEventArgs e) {
        //throw new NotImplementedException();
    //}

    protected override bool TryParseValueFromString(string? value, out bool result, [NotNullWhen(false)] out string? validationErrorMessage)
        => throw new NotSupportedException($"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");

    protected void Toggle() {
        if (!Disabled) {
            CurrentValue = !CurrentValue;
        }
    }
}
