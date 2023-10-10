using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics.CodeAnalysis;

namespace TnTComponents.Forms;
public partial class TnTTextBox {
    protected override void OnChange(ChangeEventArgs e) {
        CurrentValueAsString = (string?)(e?.Value);
    }

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out string? result, [NotNullWhen(false)] out string? validationErrorMessage) {
        result = value;
        validationErrorMessage = null;
        return true;
    }
}
