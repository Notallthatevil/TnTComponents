using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;

namespace TnTComponents.Layout;

public partial class TnTSideNavMenuGroup {

    [CascadingParameter]
    private TnTSideNav _sideNav { get; set; } = default!;

    [Parameter]
    public string? Icon { get; set; }

    [Parameter]
    public override string? Class { get; set; } = "tnt-side-nav-menu-group";

    [Parameter, EditorRequired]
    public string Title { get; set; } = default!;

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    protected override bool HasIsolatedJs => true;

    [Parameter]
    public bool Expand { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public EventCallback<bool> Expanded { get; set; }

    [DynamicDependency(nameof(Toggle))]
    public TnTSideNavMenuGroup() : base() { }

    [JSInvokable]
    public async Task Toggle(bool expanded) {
        Expand = expanded;
        await Expanded.InvokeAsync(expanded);
    }

    public override string GetClass() => $"{base.GetClass()} {(Expand ? "expanded" : string.Empty)} {(Disabled ? "disabled" : string.Empty)}";
}