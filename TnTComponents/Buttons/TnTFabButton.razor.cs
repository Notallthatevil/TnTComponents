using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTFabButton {

    [Parameter]
    public TnTColor BackgroundColor { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    public override string? CssClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-button")
        .AddClass("tnt-fab")
        .AddActionableBackgroundColor(BackgroundColor)
        .AddForegroundColor(ForegroundColor)
        .AddBorderRadius(BorderRadius)
        .Build();

    public override string? CssStyle => CssStyleBuilder.Create()
       .AddFromAdditionalAttributes(AdditionalAttributes)
       .Build();

    [Parameter]
    public TnTBorderRadius? BorderRadius { get; set; } = new(10);

    [Parameter]
    public TnTColor ForegroundColor { get; set; }

    [Parameter]
    public string? Name { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    [Parameter]
    public ButtonType Type { get; set; }
    public override string? JsModulePath => "./_content/TnTComponents/Buttons/TnTButton.razor.js";

    protected override void OnInitialized() {
        base.OnInitialized();
        Name ??= ComponentIdentifier;
    }
}