using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using TnTComponents.Core;
using TnTComponents.Layout;

namespace TnTComponents;

/// <summary>
///     Represents a side navigation component in the TnT layout system.
/// </summary>
public partial class TnTSideNav {

    /// <inheritdoc />
    [Parameter]
    public override TnTColor BackgroundColor { get; set; } = TnTColor.SurfaceContainer;

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create(base.ElementClass!)
        .AddClass("tnt-side-nav")
        .AddClass("tnt-side-nav-hide-on-large", HideOnLargeScreens)
        .AddBackgroundColor(BackgroundColor)
        .AddForegroundColor(TextColor)
        .Build();

    /// <summary>
    ///     Indicates whether the side navigation should be hidden on large screens.
    /// </summary>
    [Parameter]
    public bool HideOnLargeScreens { get; set; }

    /// <inheritdoc />
    [Parameter]
    public override TnTColor TextColor { get; set; } = TnTColor.OnSurfaceVariant;
}