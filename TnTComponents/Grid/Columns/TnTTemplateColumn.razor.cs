using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using TnTComponents.Grid;
using TnTComponents.Grid.Columns;

namespace TnTComponents;

/// <summary>
///     Represents a <see cref="TnTDataGrid{TGridItem}" /> column whose cells render a supplied template.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
[CascadingTypeParameter(nameof(TGridItem))]
public partial class TnTTemplateColumn<TGridItem> {

    [Parameter, EditorRequired]
    public RenderFragment<TGridItem> ChildContent { get; set; } = default!;

    /// <inheritdoc />
    [Parameter]
    public override TnTGridSort<TGridItem>? SortBy { get; set; }

    ///// <inheritdoc />
    //protected internal override void CellContent(RenderTreeBuilder builder, TGridItem item)
    //    => builder.AddContent(0, ChildContent(item));

    ///// <inheritdoc />
    //protected internal override string? RawCellContent(TGridItem item) => item?.ToString();

    ///// <inheritdoc />
    //protected override bool IsSortableByDefault() => SortBy is not null;
    public override string? ElementClass { get; }
    public override string? ElementStyle { get; }
}