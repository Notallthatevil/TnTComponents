using Microsoft.AspNetCore.Components;
using TnTComponents.Core;
using TnTComponents.Layout;

namespace TnTComponents;

/// <summary>
///     The footer content to be rendered.
/// </summary>
public partial class TnTFooter {

    /// <inheritdoc />
    [Parameter]
    public override TnTColor BackgroundColor { get; set; } = TnTColor.SurfaceVariant;

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create(base.ElementClass!)
        .AddClass("tnt-footer")
        .AddBackgroundColor(BackgroundColor)
        .AddForegroundColor(TextColor)
        .AddElevation(Elevation)
        .Build();

    /// <inheritdoc />
    [Parameter]
    public override int Elevation { get; set; } = 2;

    /// <inheritdoc />
    [Parameter]
    public override TnTColor TextColor { get; set; } = TnTColor.OnSurfaceVariant;
}