using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TnTComponents.Core;
using TnTComponents.Enum;

namespace TnTComponents;
public partial class TnTImageButton {
    [Parameter]
    public string? Name { get; set; }

    [Parameter]
    public ButtonType Type { get; set; }

    [Parameter]
    public MaterialIconAppearance Appearance { get; set; }

    [Parameter]
    public MaterialIconSize Size { get; set; }


    [Parameter, EditorRequired]
    public MaterialIcons Icon { get; set; } = default!;

    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }


    [Parameter]
    public TnTColor Color { get; set; } = TnTColor.OnSurface;


    public override string? Class => CssBuilder.Create()
        .AddBackgroundColor(TnTColor.Transparent)
        .AddForegroundColor(Color)
        .AddBorderRadius(new(10))
        .Build();

    protected override void OnInitialized() {
        base.OnInitialized();
        Name ??= ComponentIdentifier;
    }
}
