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
        .AddClass("tnt-fab")
        .AddActionableBackgroundColor(BackgroundColor)
        .AddForegroundColor(ForegroundColor)
        .Build();

    [Parameter]
    public TnTColor ForegroundColor { get; set; }

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