using Microsoft.AspNetCore.Components;
using TnTComponents.Core;
using TnTComponents.Layout;

namespace TnTComponents;

/// <summary>
///     The footer content to be rendered.
/// </summary>
public class TnTFooter : TnTLayoutComponent {

    /// <inheritdoc />
    [Parameter]
    public override TnTColor BackgroundColor { get; set; } = TnTColor.SurfaceContainer;

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create(base.ElementClass!)
        .AddClass("tnt-footer")
        .Build();

    /// <inheritdoc />
    [Parameter]
    public override int Elevation { get; set; } = 2;

    /// <inheritdoc />
    [Parameter]
    public override TnTColor TextColor { get; set; } = TnTColor.OnSurface;
}