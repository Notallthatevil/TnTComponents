using Microsoft.AspNetCore.Components;
using TnTComponents.Core;
using TnTComponents.Layout;

namespace TnTComponents;

public class TnTHeader : TnTLayoutComponent {
    public override string? ElementClass => CssClassBuilder.Create(base.ElementClass!)
        .AddClass("tnt-header")
        .Build();

    [Parameter]
    public override TnTColor BackgroundColor { get; set; } = TnTColor.SurfaceContainerLowest;
    [Parameter]
    public override TnTColor TextColor { get; set; } = TnTColor.OnSurface;

}