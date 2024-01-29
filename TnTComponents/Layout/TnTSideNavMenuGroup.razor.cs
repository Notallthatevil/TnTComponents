using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTSideNavMenuGroup {

    [CascadingParameter]
    private TnTSideNav _sideNav { get; set; } = default!;

    [Parameter]
    public TnTIcon? Icon { get; set; }

    public override string? Class => CssBuilder.Create()
        .SetDisabled(Disabled)
        .AddClass("tnt-expanded", Expand)
        .Build();

    [Parameter, EditorRequired]
    public string Title { get; set; } = default!;

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    protected override bool RunIsolatedJsScript => base.RunIsolatedJsScript;

    [Parameter]
    public bool Expand { get; set; }

    [Parameter]
    public EventCallback<bool> Expanded { get; set; }

    [Parameter]
    public bool Ripple { get; set; } = true;

    [DynamicDependency(nameof(Toggle))]
    public TnTSideNavMenuGroup() : base() { }

    [JSInvokable]
    public async Task Toggle(bool expanded) {
        Expand = expanded;
        await Expanded.InvokeAsync(expanded);
    }
}