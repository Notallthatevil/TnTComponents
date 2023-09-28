using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Forms {
    public partial class TnTNumeric<TNumberType> where TNumberType : unmanaged, IConvertible {
        [Parameter]
        public TNumberType AdjustmentAmount { get; set; } = AdjustmentDefaultValue();

        protected override string InputType => "text";

        private MethodInfo _tryParse = default!;

        private delegate bool TryParseDelegate(string str, out TNumberType result);
        private delegate TNumberType IncrementDelegate(TNumberType currentValue, TNumberType incrementAmount);
        private delegate TNumberType DecrementDelegate(TNumberType currentValue, TNumberType decrementAmount);

        private TryParseDelegate _tryParseHandler = default!;
        private IncrementDelegate _incrementHandler = default!;
        private DecrementDelegate _decrementHandler = default!;

        protected override void OnInitialized() {
            var type = typeof(TNumberType);
            _tryParse = type.GetMethod("TryParse", [typeof(string), typeof(TNumberType).MakeByRefType()]) ?? throw new InvalidOperationException($"{typeof(TNumberType).Name} does not contain a method \"TryParse\"");

            _tryParseHandler = (TnTNumeric<TNumberType>.TryParseDelegate)Delegate.CreateDelegate(typeof(TryParseDelegate), _tryParse);

            var param1 = Expression.Parameter(typeof(TNumberType), "lhs");
            var param2 = Expression.Parameter(typeof(TNumberType), "rhs");

            _incrementHandler = Expression.Lambda<IncrementDelegate>(Expression.AddChecked(param1, param2), param1, param2).Compile();
            _decrementHandler = Expression.Lambda<DecrementDelegate>(Expression.SubtractChecked(param1, param2), param1, param2).Compile();

            base.OnInitialized();
        }

        protected override async Task OnChange(ChangeEventArgs e) {
            var parameters = new object?[] { e?.Value?.ToString() ?? "0", null };

            if (_tryParseHandler(e?.Value?.ToString() ?? "0", out var result)) {
                await ValueChanged.InvokeAsync(result);
            }
        }

        private async Task Increment() {
            Value = _incrementHandler(Value, AdjustmentAmount);
            await ValueChanged.InvokeAsync(Value);
        }

        private async Task Decrement() {
            Value = _decrementHandler(Value, AdjustmentAmount);
            await ValueChanged.InvokeAsync(Value);
        }

        private static TNumberType AdjustmentDefaultValue() => new TNumberType().GetTypeCode() switch {
            TypeCode.Char => (TNumberType)(object)Convert.ToChar(1),
            TypeCode.SByte => (TNumberType)(object)Convert.ToSByte(1),
            TypeCode.Byte => (TNumberType)(object)Convert.ToByte(1),
            TypeCode.Int16 => (TNumberType)(object)Convert.ToInt16(1),
            TypeCode.UInt16 => (TNumberType)(object)Convert.ToUInt16(1),
            TypeCode.Int32 => (TNumberType)(object)Convert.ToInt32(1),
            TypeCode.UInt32 => (TNumberType)(object)Convert.ToUInt32(1),
            TypeCode.Int64 => (TNumberType)(object)Convert.ToInt64(1),
            TypeCode.UInt64 => (TNumberType)(object)Convert.ToUInt64(1),
            TypeCode.Single => (TNumberType)(object)Convert.ToSingle(1),
            TypeCode.Double => (TNumberType)(object)Convert.ToDouble(1),
            TypeCode.Decimal => (TNumberType)(object)Convert.ToDecimal(1),
            _ => throw new ArgumentException("The provided type is invalid.", nameof(TNumberType)),
        };

        private static bool AdjustmentAmountIsZero(TNumberType adjustmentAmount) => new TNumberType().GetTypeCode() switch {
            TypeCode.Char => adjustmentAmount.ToChar(CultureInfo.CurrentCulture) == Convert.ToChar(0),
            TypeCode.SByte => adjustmentAmount.ToSByte(CultureInfo.CurrentCulture) == Convert.ToSByte(0),
            TypeCode.Byte => adjustmentAmount.ToByte(CultureInfo.CurrentCulture) == Convert.ToByte(0),
            TypeCode.Int16 => adjustmentAmount.ToInt16(CultureInfo.CurrentCulture) == Convert.ToInt16(0),
            TypeCode.UInt16 =>adjustmentAmount.ToUInt16(CultureInfo.CurrentCulture) == Convert.ToUInt16(0),
            TypeCode.Int32 => adjustmentAmount.ToInt32(CultureInfo.CurrentCulture) == Convert.ToInt32(0),
            TypeCode.UInt32 =>adjustmentAmount.ToUInt32(CultureInfo.CurrentCulture) == Convert.ToUInt32(0),
            TypeCode.Int64 => adjustmentAmount.ToInt64(CultureInfo.CurrentCulture) == Convert.ToInt64(0),
            TypeCode.UInt64 =>adjustmentAmount.ToUInt64(CultureInfo.CurrentCulture) == Convert.ToUInt64(0),
            TypeCode.Single =>adjustmentAmount.ToSingle(CultureInfo.CurrentCulture) == Convert.ToSingle(0),
            TypeCode.Double => adjustmentAmount.ToDouble(CultureInfo.CurrentCulture) == Convert.ToDouble(0),
            TypeCode.Decimal => adjustmentAmount.ToDecimal(CultureInfo.CurrentCulture) == Convert.ToDecimal(0),
            _ => false
        };
    }
}
