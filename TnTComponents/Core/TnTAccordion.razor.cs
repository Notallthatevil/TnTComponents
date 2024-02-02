using Microsoft.AspNetCore.Components;
using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTAccordion {

    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.Surface;

    [Parameter]
    public TnTColor BodyBackgroundColor { get; set; } = TnTColor.SurfaceVariant;

    [Parameter]
    public TnTColor BodyTextColor { get; set; } = TnTColor.OnSurfaceVariant;

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    public override string? CssClass => CssClassBuilder.Create()
        .AddRipple(Ripple)
        .AddElevation(Elevation)
        .AddActionableBackgroundColor(BackgroundColor)
        .AddForegroundColor(TextColor)
        .Build();

    [Parameter]
    public int Elevation { get; set; } = 1;

    [Parameter]
    public bool Ripple { get; set; } = true;

    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnSurface;

    [Parameter, EditorRequired]
    public string Title { get; set; } = default!;

    protected override string? JsModulePath => "./_content/TnTComponents/Core/TnTAccordion.razor.js";

    protected override bool RunIsolatedJsScript => true;
}