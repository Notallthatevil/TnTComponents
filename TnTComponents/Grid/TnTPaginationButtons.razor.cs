using Microsoft.AspNetCore.Components;
using TnTComponents.Core;

namespace TnTComponents.Grid;

/// <summary>
///     Generic Pagination Buttons for <see cref="TnTPaginationState" />
/// </summary>
public partial class TnTPaginationButtons {

    /// <summary>
    ///     The background color for the current page button.
    /// </summary>
    [Parameter]
    public TnTColor ActiveBackgroundColor { get; set; } = TnTColor.Primary;

    /// <summary>
    ///     The text color for the current page button.
    /// </summary>
    [Parameter]
    public TnTColor ActiveTextColor { get; set; } = TnTColor.OnPrimary;

    /// <summary>
    ///     The background color for the pagination buttons.
    /// </summary>
    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.PrimaryContainer;

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-pagination-buttons")
        .Build();

    /// <inheritdoc />
    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    /// <summary>
    ///     The state for which to create pagination buttons.
    /// </summary>
    [Parameter, EditorRequired]
    public TnTPaginationState PaginationState { get; set; } = default!;

    /// <summary>
    ///     The text color for the pagination buttons.
    /// </summary>
    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnPrimaryContainer;

    private int _numberOfPages;

    /// <inheritdoc />
    protected override void OnParametersSet() {
        base.OnParametersSet();
        ArgumentNullException.ThrowIfNull(PaginationState, nameof(PaginationState));
        _numberOfPages = (PaginationState?.LastPageIndex ?? 0) + 1;
    }
}