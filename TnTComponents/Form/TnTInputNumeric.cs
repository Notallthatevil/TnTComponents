using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents;
public class TnTInputNumeric<TNumericType> : TnTInputBase<TNumericType> where TNumericType : INumberBase<TNumericType>? {
    public override InputType Type => InputType.Number;

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TNumericType result, [NotNullWhen(false)] out string? validationErrorMessage) {
        validationErrorMessage = null;
        if (TNumericType.TryParse(value, CultureInfo.InvariantCulture, out result)) {
            return true;
        }
        else if (value is null) {
            result = default;
            return true;
        }
        else {
            validationErrorMessage = $"Failed to parse {value} into a {typeof(TNumericType).Name}";
            return false;
        }
    }
}

