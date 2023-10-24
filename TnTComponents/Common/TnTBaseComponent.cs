using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Common {
    public abstract class TnTBaseComponent : ComponentBase {

        public virtual string BaseCssClass { get; set; } = string.Empty;

        [Parameter]
        public virtual string Theme { get; set; } = "default";

        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }


        protected virtual string GetCssClass() {
            var classResult = BaseCssClass;

            if (AdditionalAttributes?.TryGetValue("class", out var result) ?? false) {
                return classResult + " " + string.Join(' ', result);
            }
            return classResult;
        }

        protected virtual string GetCustomStyle() {
            if (AdditionalAttributes?.TryGetValue("style", out var result) ?? false) {
                return string.Join(' ', result);
            }
            return string.Empty;
        }
    }
}
