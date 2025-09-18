using Microsoft.AspNetCore.Components;
using TnTComponents.Core;
using TnTComponents.Grid.Columns;

namespace TnTComponents.Grid.Infrastructure;

/// <summary>
///     Represents a header cell in the TnT data grid for the specified grid item type.
/// </summary>
/// <typeparam name="TGridItem">The type of the grid item associated with this header cell.</typeparam>
[CascadingTypeParameter(nameof(TGridItem))]
public partial class TnTDataGridHeaderCell<TGridItem> {
    protected override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(Column.AdditionalAttributes)
        .AddClass("tnt-column-header-cell")
        .Build();

    protected override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(Column.AdditionalAttributes)
        .AddStyle("width", $"{Column.Width!.Value}px", Column.Width.HasValue)
        .AddStyle("min-width", $"{Column.Width!.Value}px", Column.Width.HasValue)
        .Build();
}