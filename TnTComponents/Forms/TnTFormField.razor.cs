using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Common;
using TnTComponents.Enum;

namespace TnTComponents.Forms {
    public abstract partial class TnTFormField<TInputType> : TnTIconComponent {

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
        public EventCallback<TInputType?> ValueChanged { get; set; }

        [Parameter]
        public string? HelperText { get; set; }

        [Parameter]
        public FormType FormType { get; set; }

        [Parameter]
        public override string BaseCssClass { get; set; } = "tnt-input-field";

        protected bool Active;

        protected abstract string InputType { get; }

        protected override void OnInitialized() {
            if (ParentForm is not null) {
                FormType = ParentForm.FormType;
                Theme = ParentForm.Theme;
            }
            base.OnInitialized();
        }

        protected string GetFromType() =>
             FormType switch {
                 FormType.Outlined => "outlined",
                 FormType.Filled => "filled",
                 _ => string.Empty,
             };



        protected abstract Task OnChange(ChangeEventArgs e);

        protected virtual RenderFragment GetAdditionalMarkup() {
            return default!;
        }

        protected override string GetCssClass() {
            return $"{base.GetCssClass()} {GetFromType()} {(Disable ? "disabled" : string.Empty)} {(Active ? "active" : string.Empty)}";
        }

        protected virtual string GetTheme() {
            return ParentForm?.Theme ?? Theme;
        }
    }
}
