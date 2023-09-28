using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Forms {
    public class TnTNumeric<TNumberType> : TnTFormField<TNumberType> where TNumberType : struct, IComparable, IComparable<TNumberType>, IConvertible, IEquatable<TNumberType>, IFormattable {
        protected override string InputType => "text";

        private MethodInfo _tryParse = default!;
        protected override void OnInitialized() {
            var type = typeof(TNumberType);
            _tryParse = type.GetMethod("TryParse", new[] { typeof(string), typeof(TNumberType).MakeByRefType() }) ?? throw new InvalidOperationException($"{typeof(TNumberType).Name} does not contain a method \"TryParse\"");
            base.OnInitialized();
        }


        protected override async Task OnChange(ChangeEventArgs e) {
            var parameters = new object?[] { e?.Value?.ToString() ?? "0", null };

            if (((bool?)_tryParse.Invoke(null, parameters)) ?? false) {
                await ValueChanged.InvokeAsync((TNumberType)parameters[1]!);
            }
        }
    }
}
