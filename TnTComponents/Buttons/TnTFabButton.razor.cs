using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TnTComponents.Core;
using TnTComponents.Enum;

namespace TnTComponents;
public partial class TnTFabButton {
    [Parameter]
    public ButtonType Type { get; set; }

    [Parameter]
    public TnTColor BackgroundColor { get; set; }
    [Parameter]
    public TnTColor ForegroundColor { get; set; }

    [Parameter]
    public string? Name { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    public override string? Class => CssBuilder.Create()
        .AddClass("tnt-fab")
        .AddBackgroundColor(BackgroundColor)
        .AddForegroundColor(ForegroundColor)
        .Build();

    protected override void OnInitialized() {
        base.OnInitialized();
        Name ??= ComponentIdentifier;
    }
}
