using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTInputChip {

    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.Primary;

    [Parameter]
    public TnTBorderRadius? BorderRadius { get; set; } = new TnTBorderRadius(2);

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public EventCallback<MouseEventArgs> ClosedCallback { get; set; }

    [Parameter]
    public TnTIcon CloseIcon { get; set; } = MaterialIcon.Close;

    public override string? CssClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-input-chip")
        .AddClass("tnt-chip")
        .AddBorderRadius(BorderRadius)
        .Build();

    public override string? CssStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddVariable("bg-color", $"var(--tnt-color-{BackgroundColor.ToCssClassName()})")
        .AddVariable("text-color", $"var(--tnt-color-{TextColor.ToCssClassName()})")
        .Build();

    [Parameter]
    public TnTIcon? StartIcon { get; set; }

    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnPrimary;

    [Parameter]
    public bool Value { get; set; }

    //protected override string? JsModulePath => "./_content/TnTComponents/TnTChip.razor.js";

    protected override void OnParametersSet() {
        base.OnParametersSet();
        if (StartIcon is not null) {
            StartIcon.AdditionalClass = "tnt-start";
        }
        if (CloseIcon is not null) {
            CloseIcon.AdditionalClass = "tnt-end";
        }
    }

    private async Task CloseClicked(MouseEventArgs args) {
        await ClosedCallback.InvokeAsync(args);
    }
}