using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTSideNavToggle {

    [Parameter]
    public bool AlwaysExpandOnLarge { get; set; } = true;

    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.Transparent;

    [Parameter]
    public TnTBorderRadius? BorderRadius { get; set; } = new(10);

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    public override string? CssClass => CssClassBuilder.Create()
        .AddActionableBackgroundColor(BackgroundColor)
        .AddForegroundColor(TextColor)
        .AddRipple(Ripple)
        .AddBorderRadius(BorderRadius)
        .Build();

    [Parameter]
    public bool Expanded { get; set; } = false;

    [Parameter]
    public bool Ripple { get; set; } = true;

    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnSurface;

    [Parameter]
    public EventCallback<bool> ToggleCallback { get; set; }

    protected override bool RunIsolatedJsScript => true;

    [DynamicDependency(nameof(Toggle))]
    public TnTSideNavToggle() : base() { }

    [JSInvokable]
    private async Task Toggle(bool expanded) {
        Expanded = expanded;
        await ToggleCallback.InvokeAsync(expanded);
    }
}