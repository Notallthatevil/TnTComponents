using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using TnTComponents.Core;
using TnTComponents.Grid.Columns;

namespace TnTComponents.Grid.Infrastructure;

/// <summary>
///     Represents a header cell in the TnT data grid for the specified grid item type.
/// </summary>
/// <typeparam name="TGridItem">The type of the grid item associated with this header cell.</typeparam>
[CascadingTypeParameter(nameof(TGridItem))]
public partial class TnTDataGridHeaderCell<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields)] TGridItem> {

    /// <inheritdoc />
    protected override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(Column.AdditionalAttributes)
        .AddClass("tnt-column-header-cell")
        .Build();

    /// <inheritdoc />
    protected override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(Column.AdditionalAttributes)
        .Add(Column.Width.HasValue ? $"width:{Column.Width.Value}px;min-width:{Column.Width.Value}px" : null!)
        .Add(Column.MaxWidth.HasValue ? $"max-width:{Column.MaxWidth.Value}px;word-wrap:break-word" : null!)
        .Build();
}