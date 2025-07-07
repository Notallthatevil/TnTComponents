using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using TnTComponents.Core;
using TnTComponents.Interfaces;

namespace TnTComponents;

/// <summary>
///     A component that represents a row
/// </summary>
public partial class TnTRow  {

    /// <inheritdoc />
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-row")
        .Build();

    /// <inheritdoc />
    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();
}