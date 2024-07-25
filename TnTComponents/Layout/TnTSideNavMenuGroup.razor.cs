using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTSideNavMenuGroup {

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public string? Name { get; set; }

    public override string? ElementClass => CssClassBuilder.Create()
        .AddDisabled(Disabled)
        .AddClass("tnt-expanded", Expand)
        .Build();

    [Parameter]
    public bool Disabled { get; set; }

    public override string? ElementStyle => CssStyleBuilder.Create()
       .AddFromAdditionalAttributes(AdditionalAttributes)
       .Build();

    public bool EnableRipple { get; set; }
    public TnTColor? TintColor { get; set; }
    public TnTColor? OnTintColor { get; set; }

    [Parameter]
    public bool Expand { get; set; }

    [Parameter]
    public EventCallback<bool> Expanded { get; set; }

    [Parameter]
    public TnTIcon? Icon { get; set; }

    public override string? JsModulePath => "./_content/TnTComponents/Layout/TnTSideNavMenuGroup.razor.js";

    [Parameter]
    public bool Ripple { get; set; } = true;

    [Parameter, EditorRequired]
    public string Title { get; set; } = default!;

    [DynamicDependency(nameof(Toggle))]
    public TnTSideNavMenuGroup() : base() { }

    [JSInvokable]
    public async Task Toggle(bool expanded) {
        Expand = expanded;
        await Expanded.InvokeAsync(expanded);
    }
}