using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Common {
    public abstract class TnTBaseComponent : ComponentBase, ITnTComponent {

        public virtual string BaseCssClass { get; set; } = string.Empty;

        [Parameter]
        public virtual string Theme { get; set; } = "default";

        [Parameter]
        public virtual bool Disabled { get; set; } = false;

        [Parameter]
        public bool Active { get; set; } = false;

        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }


        public virtual string GetCssClass() {
            var strBuilder = new StringBuilder(BaseCssClass);

            if (AdditionalAttributes?.TryGetValue("class", out var result) ?? false) {
                strBuilder.Append(' ').Append(string.Join(' ', result));
            }

            if(Disabled) {
                strBuilder.Append(' ').Append("disabled");
            }

            if(Active && !Disabled) {
                strBuilder.Append(' ').Append("active");
            }

            return strBuilder.ToString();
        }

        protected virtual string GetCustomStyle() {
            if (AdditionalAttributes?.TryGetValue("style", out var result) ?? false) {
                return string.Join(' ', result);
            }
            return string.Empty;
        }
    }
}
