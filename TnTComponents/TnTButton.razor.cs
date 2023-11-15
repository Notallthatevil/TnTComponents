using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using TnTComponents.Enum;

namespace TnTComponents;
public partial class TnTButton {
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public string Text { get; set; } = default!;

    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    [Parameter]
    public ButtonType ButtonType { get; set; }

    [Parameter]
    public override string BaseCssClass { get; set; } = "tnt-btn";

    protected override void OnInitialized() {
        if(AdditionalAttributes is null) {
            AdditionalAttributes = new Dictionary<string, object>() {
                { "type", "button" }
            };
        }

        if(!AdditionalAttributes.ContainsKey("type")) {
            AdditionalAttributes = new Dictionary<string, object>(AdditionalAttributes) {
                { "type", "button" }
            };
        }
        base.OnInitialized();
    }

    protected override string GetCssClass() {
        return $"{base.GetCssClass()} {GetButtonType()}";
    }

    private void ButtonClicked(MouseEventArgs args) {
        if(!Disabled) {
            OnClick.InvokeAsync(args);
        }
    }

    private string GetButtonType() {
        return ButtonType switch {
            ButtonType.Flat => "flat",
            ButtonType.Filled => "filled",
            ButtonType.Outlined => "outlined",
            _ => string.Empty,
        };
    }
}
