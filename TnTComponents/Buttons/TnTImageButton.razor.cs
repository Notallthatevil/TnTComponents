using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTImageButton {

    [Parameter]
    public TnTColor Color { get; set; } = TnTColor.OnSurface;

    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddActionableBackgroundColor(TnTColor.Transparent)
        .AddForegroundColor(Color)
        .AddBorderRadius(new(10))
        .AddClass("tnt-button")
        .Build();

    public override string? ElementStyle => CssStyleBuilder.Create()
       .AddFromAdditionalAttributes(AdditionalAttributes)
       .Build();

    [Parameter, EditorRequired]
    public TnTIcon Icon { get; set; } = default!;

    public override string? JsModulePath => "./_content/TnTComponents/Buttons/TnTButton.razor.js";

    [Parameter]
    public string? Name { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    [Parameter]
    public ButtonType Type { get; set; }

    [Parameter]
    public bool StopPropagation { get; set; }

    protected override void OnInitialized() {
        base.OnInitialized();
        Name ??= ComponentIdentifier;
    }
}