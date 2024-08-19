using Microsoft.AspNetCore.Components;
using TnTComponents.Core;
using TnTComponents.Layout;

namespace TnTComponents;

public class TnTFooter : TnTLayoutComponent {
    public override string? ElementClass => CssClassBuilder.Create(base.ElementClass!)
        .AddClass("tnt-footer")
        .Build();

    [Parameter]
    public override TnTColor BackgroundColor { get; set; } = TnTColor.SurfaceContainer;
    [Parameter]
    public override TnTColor TextColor { get; set; } = TnTColor.OnSurface;

    [Parameter]
    public override int Elevation { get; set; } = 2;
}