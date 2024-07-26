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
        builder.AddAttribute(30, nameof(CascadingValue<TnTSideNav>.ChildContent), new RenderFragment(b => {
            b.OpenElement(0, "div");
            b.AddAttribute(10, "style", "display:none");
            b.AddAttribute(20, "class", "tnt-side-nav-toggle-indicator");
            b.AddAttribute(30, "data-permanent");
            b.OpenElement(40, "div");
            b.AddAttribute(50, "class", "tnt-toggle-indicator");
            b.CloseElement();
            b.CloseElement();
            b.AddContent(60, new RenderFragment(base.BuildRenderTree));
        }));
        builder.CloseComponent();
    }
}