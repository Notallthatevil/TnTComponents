using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;

namespace TnTComponents;

public partial class TnTSideNavToggle {

    [Parameter]
    public bool Expanded { get; set; } = false;

    [Parameter]
    public bool AlwaysExpandOnLarge { get; set; } = true;

    [Parameter]
    public EventCallback<bool> ToggleCallback { get; set; }

    [Parameter]
    public string Icon { get; set; } = "menu";

    public override string? Class => null;

    protected override bool RunIsolatedJsScript => true;

    [DynamicDependency(nameof(Toggle))]
    public TnTSideNavToggle() : base() { }

    [JSInvokable]
    private async Task Toggle(bool expanded) {
        Expanded = expanded;
        await ToggleCallback.InvokeAsync(expanded);
    }
}