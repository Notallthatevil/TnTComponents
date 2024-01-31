using Microsoft.AspNetCore.Components;
using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTAccordion {

    [Parameter, EditorRequired]
    public string Title { get; set; } = default!;

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.Surface;

    [Parameter]
    public TnTColor BodyBackgroundColor { get; set; } = TnTColor.SurfaceVariant;

    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnSurface;

    [Parameter]
    public TnTColor BodyTextColor { get; set; } = TnTColor.OnSurfaceVariant;

    [Parameter]
    public bool Ripple { get; set; } = true;

    [Parameter]
    public int Elevation { get; set; } = 1;

    public override string? Class => CssBuilder.Create()
        .AddRipple(Ripple)
        .AddElevation(Elevation)
        .AddActionableBackgroundColor(BackgroundColor)
        .AddForegroundColor(TextColor)
        .Build();
}