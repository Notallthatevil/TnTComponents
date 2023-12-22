using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;

namespace TnTComponents.Layout;

public partial class TnTSideNavToggle {

    [Parameter]
    public bool Expanded { get; set; } = false;

    [Parameter]
    public bool AlwaysExpandOnLarge { get; set; } = true;

    [Parameter]
    public EventCallback<bool> ToggleCallback { get; set; }

    [Parameter]
    public string Icon { get; set; } = "menu";

    [Parameter]
    public override string? Class { get; set; } = "tnt-side-nav-toggle";

    protected override bool HasIsolatedJs => true;

    [DynamicDependency(nameof(Toggle))]
    public TnTSideNavToggle() : base() { }

    [JSInvokable]
    private async Task Toggle(bool expanded) {
        Expanded = expanded;
        await ToggleCallback.InvokeAsync(expanded);
    }

    public override string GetClass() => $"{base.GetClass()} {(AlwaysExpandOnLarge ? "expand-large" : string.Empty)} {(Expanded ? "expanded" : string.Empty)}";
}