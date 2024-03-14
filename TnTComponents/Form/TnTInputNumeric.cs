using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents;
public class TnTInputNumeric<TNumericType> : TnTInputBase<TNumericType> {
    public override InputType Type => InputType.Number;


    protected override void OnInitialized() {
        base.OnInitialized();
        Type type = Nullable.GetUnderlyingType(typeof(TNumericType)) ?? typeof(TNumericType);
        if (type != typeof(int) && type != typeof(long) && type != typeof(short) && type != typeof(float) && type != typeof(double) && type != typeof(decimal)) {
            throw new InvalidOperationException($"The type '{type}' is not a supported numeric type.");
        }

    }

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TNumericType result, [NotNullWhen(false)] out string? validationErrorMessage) {
        if (BindConverter.TryConvertTo<TNumericType>(value, CultureInfo.InvariantCulture, out result)) {
            validationErrorMessage = null;
            return true;
        }

        validationErrorMessage = string.Format(CultureInfo.InvariantCulture, "The {0} field must be a number.", base.DisplayName ?? base.FieldIdentifier.FieldName);
        return false;
    }
}

