using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using TnTComponents.Grid;
using TnTComponents.Grid.Columns;

namespace TnTComponents;

/// <summary>
///     Represents a <see cref="TnTDataGrid{TGridItem}" /> column whose cells render a supplied template.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
public class TnTTemplateColumn<TGridItem> : TnTColumnBase<TGridItem> {

    /// <summary>
    ///     Gets or sets the content to be rendered for each row in the table.
    /// </summary>
    [Parameter]
    public RenderFragment<TGridItem> ChildContent { get; set; } = EmptyChildContent;

    /// <inheritdoc />
    [Parameter]
    public override TnTGridSort<TGridItem>? SortBy { get; set; }

    private static readonly RenderFragment<TGridItem> EmptyChildContent = _ => builder => { };

    /// <inheritdoc />
    protected internal override void CellContent(RenderTreeBuilder builder, TGridItem item)
        => builder.AddContent(0, ChildContent(item));

    protected internal override string? RawCellContent(TGridItem item) {
        return "Not implemented";
    }

    /// <inheritdoc />
    protected override bool IsSortableByDefault() => SortBy is not null;
}