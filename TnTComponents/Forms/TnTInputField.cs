using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Text;
using TnTComponents.Common;
using TnTComponents.Common.Ext;
using TnTComponents.Enum;

namespace TnTComponents.Forms;

public abstract class TnTInputField<TInputType> : InputBase<TInputType>, ITnTFormField {

    [Parameter]
    public virtual string BaseCssClass { get; set; } = ITnTFormField.DefaultBaseCssClass;

    [Parameter]
    public virtual bool Disabled { get; set; }

    [Parameter]
    public FormType FormType { get; set; }

    [Parameter]
    public IconType IconType { get; set; }

    [Parameter]
    public string? Icon { get; set; }

    [Parameter, EditorRequired]
    public string Label { get; set; }

    [CascadingParameter]
    public TnTForm? ParentForm { get; set; }

    [Parameter]
    public string? Placeholder { get; set; }

    [Parameter]
    public string Theme { get; set; } = TnTConstants.DefaultTheme;

    [Parameter]
    public bool ShowValidation { get; set; } = true;

    protected bool Active { get; private set; }

    protected ElementReference InputElement { get; set; }

    [Inject]
    protected IJSRuntime JSRuntime { get; set; } = default!;

    protected bool Interactive { get; private set; }

    protected override void OnParametersSet() {
        this.MatchParentFormIfExists();
        base.OnParametersSet();
    }

    //protected abstract void OnChange(ChangeEventArgs e);

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

    protected virtual async Task OnFocusInAsync(FocusEventArgs e) {
        Active = true;
    }

    protected virtual async Task OnFocusOutAsync(FocusEventArgs e) {
        Active = false;
    }

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

    protected override void OnAfterRender(bool firstRender) {
        if (firstRender) {
            Interactive = true;
            StateHasChanged();
        }

        base.OnAfterRender(firstRender);
    }

    protected virtual string GetCssClass() {
        var strBuilder = new StringBuilder(BaseCssClass)
            .Append(' ').Append(CssClass);

        if (Active && !Disabled) {
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

        if (!Interactive || !string.IsNullOrWhiteSpace(Placeholder) || !string.IsNullOrWhiteSpace(Icon) || !string.IsNullOrWhiteSpace(CurrentValueAsString)) {
            strBuilder.Append(' ').Append("dont-inline");
        }

        return strBuilder.ToString();
    }

    protected virtual string GetValidationMessageCssClass() {
        var strBuilder = new StringBuilder(ITnTFormField.InputFieldMessageCssClass);

        if (Disabled) {
            strBuilder.Append(' ').Append("disabled");
        }

        return strBuilder.ToString();
    }

    protected async Task SetInputFocus() {
        await JSRuntime.SetElementFocus(InputElement);
    }

    protected async Task RemoveInputFocus() {
        await JSRuntime.RemoveElementFocus(InputElement);
    }
}