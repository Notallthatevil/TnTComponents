using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Common;
using TnTComponents.Enum;

namespace TnTComponents.Forms {
    public class TnTFormField<TInputType> : TnTBaseComponent {
        [CascadingParameter]
        public TnTForm? ParentForm { get; set; }

        [Parameter]
        public string Label { get; set; } = default!;

        [Parameter]
        public string Placeholder { get; set; } = " ";

        [Parameter]
        public bool Disable { get; set; } = false;

        [Parameter]
        public TInputType? Value { get; set; }

        [Parameter]
        public string? Format { get; set; }

        [Parameter]
        public EventCallback<TInputType> ValueChanged { get; set; }

        [Parameter]
        public FormType FormType { get; set; }

        protected bool Active;


        protected override void OnInitialized() {
            if (ParentForm is not null) {
                FormType = ParentForm.FormType;
                Theme = ParentForm.Theme;
            }
        }

        protected string GetFromType() {
            switch (FormType) {
                case FormType.Outlined: return "outlined";
                case FormType.Filled: return "filled";
                default:
                    return string.Empty;
            }
        }
    }
}
