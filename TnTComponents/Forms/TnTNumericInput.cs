using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace TnTComponents.Forms;

public class TnTNumericInput<TInputType> : TnTInputBase<TInputType> where TInputType : struct,
    IComparable,
    IConvertible,
    ISpanFormattable,
    IComparable<TInputType>,
    IEquatable<TInputType>,
    IMinMaxValue<TInputType>,
    ISignedNumber<TInputType>,
    IUtf8SpanFormattable,
    IAdditionOperators<TInputType, TInputType, TInputType>,
    ISubtractionOperators<TInputType, TInputType, TInputType>,
    IMultiplyOperators<TInputType, TInputType, TInputType>,
    IDivisionOperators<TInputType, TInputType, TInputType> {
    protected override TInputType BindValue { get => CurrentValue; set => CurrentValue = value; }

    protected override string InputType => "number";

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TInputType result, [NotNullWhen(false)] out string? validationErrorMessage) {
        if (BindConverter.TryConvertTo<TInputType>(value, CultureInfo.InvariantCulture, out result)) {
            validationErrorMessage = null;
            return true;
        }

        validationErrorMessage = string.Format(CultureInfo.InvariantCulture, "The {0} field must be a number.", base.DisplayName ?? base.FieldIdentifier.FieldName);
        return false;
    }
}