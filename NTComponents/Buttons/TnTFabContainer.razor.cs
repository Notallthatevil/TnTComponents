namespace NTComponents;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using NTComponents.Core;

/// <summary>
/// A container for floating action buttons that manages their layout and expansion.
/// </summary>
public partial class TnTFabContainer : TnTComponentBase {

    /// <summary>
    /// The direction to layout the FABs.
    /// </summary>
    [Parameter]
    public LayoutDirection Direction { get; set; } = LayoutDirection.Vertical;

    /// <summary>
    /// The corner of the screen where the container is positioned.
    /// </summary>
    [Parameter]
    public Corner Position { get; set; } = Corner.BottomRight;

    /// <summary>
    /// The expansion mode for the container.
    /// </summary>
    [Parameter]
    public FabExpandMode ExpandMode { get; set; } = FabExpandMode.Never;

    /// <summary>
    /// The content to be rendered as the toggle button.
    /// </summary>
    [Parameter]
    public RenderFragment? ToggleContent { get; set; }

    /// <summary>
    /// The content to be rendered inside the container.
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-fab-container")
        .AddClass($"tnt-position-{Position.ToString().ToLower()}")
        .AddClass($"tnt-direction-{Direction.ToString().ToLower()}")
        .AddClass("tnt-expandable", ExpandMode != FabExpandMode.Never)
        .AddClass("tnt-expand-always", ExpandMode == FabExpandMode.Always)
        .AddClass("tnt-expand-small", ExpandMode == FabExpandMode.SmallScreens)
        .Build();

    /// <inheritdoc />
    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();
}
