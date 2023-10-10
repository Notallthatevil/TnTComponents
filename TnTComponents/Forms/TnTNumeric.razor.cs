using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq.Expressions;
using System.Numerics;

namespace TnTComponents.Forms;
public partial class TnTNumeric<TNumberType> where TNumberType : unmanaged,
    IComparable<TNumberType>,
    IMinMaxValue<TNumberType>,
    IAdditionOperators<TNumberType, TNumberType, TNumberType>,
    ISubtractionOperators<TNumberType, TNumberType, TNumberType> {

    [Parameter]
    public TNumberType AdjustmentAmount { get; set; } = DefaultAdjustmentValue();

    [Parameter]
    public TNumberType MaxValue { get; set; } = TNumberType.MaxValue;
    [Parameter]
    public TNumberType MinValue { get; set; } = TNumberType.MinValue;

    private static TNumberType DefaultAdjustmentValue() {
        TNumberType t = default;
        return t switch {
            byte => (TNumberType)(object)Convert.ToByte(1),
            sbyte => (TNumberType)(object)Convert.ToSByte(1),
            short => (TNumberType)(object)Convert.ToInt16(1),
            ushort => (TNumberType)(object)Convert.ToUInt16(1),
            int => (TNumberType)(object)1,
            uint => (TNumberType)(object)1U,
            long => (TNumberType)(object)1L,
            ulong => (TNumberType)(object)1UL,
            float => (TNumberType)(object)0.1F,
            double => (TNumberType)(object)0.1,
            decimal => (TNumberType)(object)0.1M,
            _ => throw new InvalidOperationException($"Unsupported type {t.GetType()}"),
        };
    }

    private bool _inAdjustment;

    //protected override void OnChange(ChangeEventArgs e) {
        //CurrentValueAsString = e?.Value?.ToString();
    //}

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TNumberType result, [NotNullWhen(false)] out string? validationErrorMessage) {
        if (BindConverter.TryConvertTo<TNumberType>(value, CultureInfo.InvariantCulture, out result)) {
            if (result.CompareTo(MaxValue) > 0 || result.CompareTo(MinValue) < 0) {
                validationErrorMessage = $"The value must be between {MinValue} and {MaxValue} inclusively.";
                return false;
            }
            validationErrorMessage = null;
            return true;
        }
        else {
            validationErrorMessage = string.Format(CultureInfo.InvariantCulture, $"The {DisplayName ?? FieldIdentifier.FieldName} field must be a number.");
            return false;
        }
    }

    protected override string? FormatValueAsString(TNumberType value) {
        // Avoiding a cast to IFormattable to avoid boxing.
        return value switch {

            byte @byte => BindConverter.FormatValue(@byte, CultureInfo.InvariantCulture)?.ToString(),
            sbyte @sbyte => BindConverter.FormatValue(@sbyte, CultureInfo.InvariantCulture)?.ToString(),
            short @short => BindConverter.FormatValue(@short, CultureInfo.InvariantCulture),
            ushort @ushort => BindConverter.FormatValue(@ushort, CultureInfo.InvariantCulture)?.ToString(),
            int @int => BindConverter.FormatValue(@int, CultureInfo.InvariantCulture),
            uint @uint => BindConverter.FormatValue(@uint, CultureInfo.InvariantCulture)?.ToString(),
            long @long => BindConverter.FormatValue(@long, CultureInfo.InvariantCulture),
            ulong @ulong => BindConverter.FormatValue(@ulong, CultureInfo.InvariantCulture)?.ToString(),
            float @float => BindConverter.FormatValue(@float, CultureInfo.InvariantCulture),
            double @double => BindConverter.FormatValue(@double, CultureInfo.InvariantCulture),
            decimal @decimal => BindConverter.FormatValue(@decimal, CultureInfo.InvariantCulture),
            _ => throw new InvalidOperationException($"Unsupported type {value.GetType()}"),
        };
    }

    private async Task Increment() {
        _inAdjustment = true;
        while (_inAdjustment) {
            CurrentValue += AdjustmentAmount;
            if (CurrentValue.CompareTo(MaxValue) > 0) {
                CurrentValue = MinValue;
            }
            await Task.Delay(100);
            StateHasChanged();
        }
    }
    private async Task Decrement() {
        _inAdjustment = true;
        while (_inAdjustment) {
            CurrentValue -= AdjustmentAmount;
            if (CurrentValue.CompareTo(MinValue) < 0) {
                CurrentValue = MaxValue;
            }

            await Task.Delay(100);
            StateHasChanged();
        }
    }
}
