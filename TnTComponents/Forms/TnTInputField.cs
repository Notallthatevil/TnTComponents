using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Common;
using TnTComponents.Enum;

namespace TnTComponents.Forms;
public abstract class TnTInputField<TInputType> : InputBase<TInputType>, ITnTFormField {
    [Parameter]
    public virtual string BaseCssClass { get; set; } = ITnTFormField.DefaultBaseCssClass;
    [Parameter]
    public bool Disabled { get; set; }
    [Parameter]
    public FormType FormType { get; set; }

    [Parameter]
    public IconType IconType { get; set; }

    [Parameter]
    public string? Icon { get; set; }


    [Parameter, EditorRequired]
    public required string Label { get; set; }

    [CascadingParameter]
    public TnTForm? ParentForm { get; set; }
    [Parameter]
    public string? Placeholder { get; set; }
    [Parameter]
    public string Theme { get; set; } = TnTConstants.DefaultTheme;

    [Parameter]
    public Func<TInputType>? ValidationExpression { get; set; }

    protected bool Active { get; private set; }

    protected string? InputMessage { get; private set; }

    protected override void OnParametersSet() {
        this.MatchParentFormIfExists();
        base.OnParametersSet();
    }

    protected abstract void OnChange(ChangeEventArgs e);

    protected virtual void OnFocusIn(FocusEventArgs e) {
        Active = true;
    }
    protected virtual void OnFocusOut(FocusEventArgs e) {
        Active = false;
    }

    protected virtual string GetCssClass() {
        var strBuilder = new StringBuilder(BaseCssClass)
            .Append(' ').Append(CssClass);

        if (Active) {
            strBuilder.Append(' ').Append("active");
        }

        if (Disabled) {
            strBuilder.Append(' ').Append("disabled");
        }

        switch (FormType) {
            case FormType.Underlined:
                break;
            case FormType.Filled:
                strBuilder.Append(' ').Append("filled");
                break;
            case FormType.Outlined:
                strBuilder.Append(' ').Append("outlined");
                break;
            default:
                throw new NotImplementedException();
        }

        return strBuilder.ToString();
    }

    protected virtual string GetLabelCssClass() {
        var strBuilder = new StringBuilder(ITnTFormField.InputFieldLabelCssClass);

        if (Active) {
            strBuilder.Append(' ').Append("active");
        }

        if (Disabled) {
            strBuilder.Append(' ').Append("disabled");
        }

        if (!string.IsNullOrWhiteSpace(Placeholder) || !string.IsNullOrWhiteSpace(Icon) || !string.IsNullOrWhiteSpace(CurrentValueAsString)) {
            strBuilder.Append(' ').Append("dont-inline");
        }

        return strBuilder.ToString();
    }

    protected virtual string GetValidationMessageCssClass() {
        var strBuilder = new StringBuilder(ITnTFormField.InputFieldMessageCssClass);

        if(Disabled) {
            strBuilder.Append(' ').Append("disabled");
        }

        return strBuilder.ToString();
    }
}
