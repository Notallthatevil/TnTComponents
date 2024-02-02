using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTSideNavMenuGroup {

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    public override string? CssClass => CssClassBuilder.Create()
        .SetDisabled(Disabled)
        .AddClass("tnt-expanded", Expand)
        .Build();

    [Parameter]
    public bool Expand { get; set; }

    [Parameter]
    public EventCallback<bool> Expanded { get; set; }

    [Parameter]
    public TnTIcon? Icon { get; set; }

    [Parameter]
    public bool Ripple { get; set; } = true;

    [Parameter, EditorRequired]
    public string Title { get; set; } = default!;

    protected override bool RunIsolatedJsScript => false;

    protected override string? JsModulePath => "./_content/TnTComponents/Layout/TnTSideNavMenuGroup.razor.js";

    [DynamicDependency(nameof(Toggle))]
    public TnTSideNavMenuGroup() : base() { }

    [JSInvokable]
    public async Task Toggle(bool expanded) {
        Expand = expanded;
        await Expanded.InvokeAsync(expanded);
    }
}