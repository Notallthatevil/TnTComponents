using Microsoft.AspNetCore.Components;
using NTComponents.Core;

namespace NTComponents;

/// <summary>
///     Represents a tab view component.
/// </summary>
public partial class TnTTabView {

    /// <summary>
    ///     Gets or sets the color of the active indicator.
    /// </summary>
    [Parameter]
    public TnTColor ActiveIndicatorColor { get; set; } = TnTColor.Primary;

    /// <summary>
    ///     Gets or sets the appearance of the tab view.
    /// </summary>
    [Parameter]
    public TabViewAppearance Appearance { get; set; } = TabViewAppearance.Primary;

    /// <summary>
    ///     Gets or sets the child content to be rendered inside the tab view.
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-tab-view")
        .AddClass("tnt-tab-view-secondary", Appearance == TabViewAppearance.Secondary)
        .Build();

    /// <inheritdoc />
    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddVariable("tnt-tab-view-active-indicator-color", ActiveIndicatorColor)
        .Build();

    /// <summary>
    ///     Gets or sets the background color of the tab header.
    /// </summary>
    [Parameter]
    public TnTColor HeaderBackgroundColor { get; set; } = TnTColor.Surface;

    /// <summary>
    ///     Gets or sets the text color of the tab header.
    /// </summary>
    [Parameter]
    public TnTColor HeaderTextColor { get; set; } = TnTColor.OnSurface;

    /// <summary>
    ///     The tint color for the tab header, used for ripple effects and other visual enhancements.
    /// </summary>
    [Parameter]
    public TnTColor HeaderTintColor { get; set; } = TnTColor.SurfaceTint;

    /// <inheritdoc />
    public override string? JsModulePath => "./_content/NTComponents/TabView/TnTTabView.razor.js";

    private readonly List<TnTTabChild> _tabChildren = [];

    /// <summary>
    ///     Adds a child tab to the tab view.
    /// </summary>
    /// <param name="tabChild">The child tab to add.</param>
    public void AddTabChild(TnTTabChild tabChild) => _tabChildren.Add(tabChild);

    /// <summary>
    ///     Removes a child tab from the tab view.
    /// </summary>
    /// <param name="tabChild">The child to remove</param>
    public void RemoveTabChild(TnTTabChild tabChild) => _tabChildren.Remove(tabChild);
}

/// <summary>
///     Specifies the appearance of a tab view.
/// </summary>
public enum TabViewAppearance {

    /// <summary>
    ///     The primary appearance of the tab view.
    /// </summary>
    Primary,

    /// <summary>
    ///     The secondary appearance of the tab view.
    /// </summary>
    Secondary
}