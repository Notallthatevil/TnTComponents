using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTImageButton {

    [Parameter]
    public TnTColor Color { get; set; } = TnTColor.OnSurface;

    public override string? CssClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddActionableBackgroundColor(TnTColor.Transparent)
        .AddForegroundColor(Color)
        .AddBorderRadius(new(10))
        .Build();

    public override string? CssStyle => CssStyleBuilder.Create()
       .AddFromAdditionalAttributes(AdditionalAttributes)
       .Build();

    [Parameter, EditorRequired]
    public TnTIcon Icon { get; set; } = default!;

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