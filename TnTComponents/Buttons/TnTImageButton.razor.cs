using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTImageButton {

    public override string? Class => CssBuilder.Create()
        .AddActionableBackgroundColor(TnTColor.Transparent)
        .AddForegroundColor(Color)
        .AddBorderRadius(new(10))
        .Build();

    [Parameter]
    public TnTColor Color { get; set; } = TnTColor.OnSurface;

    [Parameter, EditorRequired]
    public TnTIcon Icon { get; set; } = default!;

    [Parameter]
    public string? Name { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> OnClickCallback { get; set; }

    [Parameter]
    public ButtonType Type { get; set; }

    protected override void OnInitialized() {
        base.OnInitialized();
        Name ??= ComponentIdentifier;
    }
}