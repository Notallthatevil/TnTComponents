using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using TnTComponents.Ext;

namespace TnTComponents;

public partial class TnTInputCurrency : TnTInputBase<decimal?> {
    public override InputType Type => InputType.Currency;

    [Parameter]
    public string CultureCode { get; set; } = "en-US";

    [Parameter]
    public string CurrencyCode { get; set; } = "USD";

    private string? _value {
        get {
            return CurrentValue?.ToString("C");
        }
        set {
            if (decimal.TryParse(value?.TrimStart('$'), NumberStyles.Currency, CultureInfo.GetCultureInfo(CultureCode), out var r)) {
                CurrentValue = r;
            }
            else {
                CurrentValue = null;
            }
        }
    }

    protected override void OnParametersSet() {
        base.OnParametersSet();

        var dict = AdditionalAttributes is not null ? new Dictionary<string, object>(AdditionalAttributes) : [];
        dict["onkeydown"] = "TnTComponents.enforceCurrencyFormat(event)";
        dict["onkeyup"] = "TnTComponents.formatToCurrency(event)";
        dict["name"] = ElementName!;
        dict["cultureCode"] = CultureCode;
        dict["currencyCode"] = CurrencyCode;

        AdditionalAttributes = dict;
    }


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

    private async Task BindAfterFunc(string? _) {
        if (BindAfter.HasDelegate) {
            await BindAfter.InvokeAsync(Value);
        }
    }

}