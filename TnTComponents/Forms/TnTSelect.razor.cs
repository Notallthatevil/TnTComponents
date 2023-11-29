using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace TnTComponents.Forms;

[CascadingTypeParameter(nameof(TOptionType))]
public partial class TnTSelect<TOptionType> {
    protected override TOptionType? BindValue { get => CurrentValue; set => CurrentValue = value; }
    protected override string InputType => throw new NotImplementedException();

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TOptionType? result, [NotNullWhen(false)] out string? validationErrorMessage) {
        try {
            if (typeof(TOptionType) == typeof(bool)) {
                if (TryConvertToBool(value, out result)) {
                    validationErrorMessage = null;
                    return true;
                }
            }
            else if (typeof(TOptionType) == typeof(bool?)) {
                if (TryConvertToNullableBool(value, out result)) {
                    validationErrorMessage = null;
                    return true;
                }
            }
            else if (BindConverter.TryConvertTo(value, CultureInfo.CurrentCulture, out TOptionType? value2)) {
                result = value2;
                validationErrorMessage = null;
                return true;
            }

            result = default;
            validationErrorMessage = "The " + (DisplayName ?? FieldIdentifier.FieldName) + " field is not valid.";
            return false;
        }
        catch (InvalidOperationException innerException) {
            throw new InvalidOperationException($"{GetType()} does not support the type '{typeof(TOptionType)}'.", innerException);
        }
    }

    private static bool TryConvertToBool<TValue>(string? value, out TValue? result) {
        if (bool.TryParse(value, out var result2)) {
            result = (TValue)(object)result2;
            return true;
        }

        result = default;
        return false;
    }

    private static bool TryConvertToNullableBool<TValue>(string? value, out TValue? result) {
        if (string.IsNullOrEmpty(value)) {
            result = default;
            return true;
        }

        return TryConvertToBool(value, out result);
    }
}
