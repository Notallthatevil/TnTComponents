using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TnTComponents.Enum;

namespace TnTComponents;

public partial class TnTButton {

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    [Parameter, EditorRequired]
    public string Title { get; set; } = default!;

    [Parameter]
    public bool? AutoFocus { get; set; }

    [Parameter]
    public string? FormId { get; set; }

    [Parameter]
    public ButtonType Type { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public override string? Class { get; set; } = "tnt-button";

    [Parameter]
    public IconType IconType { get; set; }

    [Parameter]
    public string? StartIcon { get; set; }

    [Parameter]
    public string? EndIcon { get; set; }

    [Parameter]
    public ButtonAppearance Appearance { get; set; }

    [CascadingParameter]
    private TnTSegmentedButton? _segmentedButton { get; set; }

    public override string GetClass() {
        return base.GetClass() + " " + Appearance.ToString().ToLower();
    }

    private Task OnClickHandler(MouseEventArgs args) {
        if (!Disabled) {
            if (_segmentedButton is not null) {
                _segmentedButton.ActiveObject = this;
            }

            if (OnClick.HasDelegate) {
                return OnClick.InvokeAsync(args);
            }
        }
        return Task.CompletedTask;
    }

    private RenderFragment RenderScript() {
        var scriptContent = new MarkupString($"window.addEventListener('load', (e) => {{document.getElementById('{Id}').addEventListener('click', TnTComponents.ripple); }});");
        return new RenderFragment(b => {
            b.OpenElement(0, "script");
            b.AddAttribute(10, "type", "text/javascript");
            b.AddContent(20, scriptContent);
            b.CloseElement();
        });
    }
}