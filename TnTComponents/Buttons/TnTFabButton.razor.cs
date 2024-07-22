using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTFabButton {

    [Parameter]
    public TnTColor BackgroundColor { get; set; }

    [Parameter]
    public TnTBorderRadius? BorderRadius { get; set; } = new(10);

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-button")
        .AddClass("tnt-fab")
        .AddActionableBackgroundColor(BackgroundColor)
        .AddForegroundColor(ForegroundColor)
        .AddBorderRadius(BorderRadius)
        .Build();

    [Parameter]
    public bool Disabled { get; set; }

    public override string? ElementStyle => CssStyleBuilder.Create()
       .AddFromAdditionalAttributes(AdditionalAttributes)
       .Build();

    [Parameter]
    public TnTColor ForegroundColor { get; set; }

    public override string? JsModulePath => "./_content/TnTComponents/Buttons/TnTButton.razor.js";

    [Parameter]
    public string? Name { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    [Parameter]
    public ButtonType Type { get; set; }

    protected override void OnInitialized() {
        base.OnInitialized();
        Name ??= ComponentIdentifier;
    }
}