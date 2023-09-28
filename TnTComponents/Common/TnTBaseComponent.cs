using Microsoft.AspNetCore.Components;
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
        public IReadOnlyDictionary<string, object>? Attributes { get; set; }


        protected virtual string GetClass() {
            var classResult = BaseCssClass;

            if (Attributes?.TryGetValue("class", out var result) ?? false) {
                    return classResult + " " + string.Join(' ', result);
            }
            return classResult;
        }
    }
}
