using Microsoft.AspNetCore.Components;
using TnTComponents.Core;
using TnTComponents.Layout;

namespace TnTComponents;

/// <summary>
///     The header content to be rendered.
/// </summary>
public partial class TnTHeader {

    /// <inheritdoc />
    [Parameter]
    public override TnTColor BackgroundColor { get; set; } = TnTColor.Surface;

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create(base.ElementClass!)
        .AddClass("tnt-header")
        .AddBackgroundColor(BackgroundColor)
        .AddForegroundColor(TextColor)
        .Build();

    /// <inheritdoc />
    [Parameter]
    public override TnTColor TextColor { get; set; } = TnTColor.OnSurface;

}