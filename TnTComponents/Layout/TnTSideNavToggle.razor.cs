using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTSideNavToggle {

    [Parameter]
    public bool Expanded { get; set; } = false;

    [Parameter]
    public bool AlwaysExpandOnLarge { get; set; } = true;

    [Parameter]
    public EventCallback<bool> ToggleCallback { get; set; }

    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.Transparent;

    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnSurface;

    [Parameter]
    public bool Ripple { get; set; } = true;

    [Parameter]
    public TnTBorderRadius? BorderRadius { get; set; } = new(10);

    public override string? Class => CssBuilder.Create()
        .AddActionableBackgroundColor(BackgroundColor)
        .AddForegroundColor(TextColor)
        .AddRipple(Ripple)
        .AddBorderRadius(BorderRadius)
        .Build();

    protected override bool RunIsolatedJsScript => true;

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [DynamicDependency(nameof(Toggle))]
    public TnTSideNavToggle() : base() { }



    [JSInvokable]
    private async Task Toggle(bool expanded) {
        Expanded = expanded;
        await ToggleCallback.InvokeAsync(expanded);
    }
}