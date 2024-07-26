using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using TnTComponents.Core;
using TnTComponents.Layout;

namespace TnTComponents;

public class TnTSideNav : TnTLayoutComponent {
    [Parameter]
    public bool HideOnLargeScreens { get; set; }
    public override string? ElementClass => CssClassBuilder.Create(base.ElementClass!)
        .AddClass("tnt-side-nav")
        .AddClass("tnt-side-nav-hide-on-large", HideOnLargeScreens)
        .Build();

    [Parameter]
    public override TnTColor BackgroundColor { get; set; } = TnTColor.SurfaceVariant;

    [Parameter]
    public override TnTColor TextColor { get; set; } = TnTColor.OnSurfaceVariant;

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenComponent<CascadingValue<TnTSideNav>>(0);
        builder.AddAttribute(10, nameof(CascadingValue<TnTSideNav>.Value), this);
        builder.AddAttribute(20, nameof(CascadingValue<TnTSideNav>.IsFixed), true);
        builder.AddAttribute(30, nameof(CascadingValue<TnTSideNav>.ChildContent), new RenderFragment(base.BuildRenderTree));
        builder.CloseComponent();
    }
}