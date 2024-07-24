using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using TnTComponents.Ext;

namespace TnTComponents;

public class TnTInputCurrency : TnTInputBase<decimal?> {
    public override InputType Type => InputType.Currency;

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out decimal? result, [NotNullWhen(false)] out string? validationErrorMessage) {
        validationErrorMessage = null;
        if (value is not null) {
            if (decimal.TryParse(value, NumberStyles.Currency, CultureInfo.InvariantCulture, out var r)) {
                result = r;
                return true;
            }
            else if (value is null) {
                result = null;
                return true;
            }
            else {
                result = null;
                validationErrorMessage = $"Failed to parse {value} into a {typeof(decimal).Name}";
                return false;
            }
        }
        else {
            result = null;
            validationErrorMessage = null;
            return true;
        }
    }
}
