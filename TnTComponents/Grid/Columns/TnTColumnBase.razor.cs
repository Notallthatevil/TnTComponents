using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using TnTComponents.Core;
using TnTComponents.Grid.Infrastructure;

namespace TnTComponents.Grid.Columns;

/// <summary>
///     An abstract base class for columns in a <see cref="TnTDataGrid{TGridItem}" />.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
public abstract partial class TnTColumnBase<TGridItem> {

    ///// <summary>
    /////     If specified, indicates that this column has this associated options UI. A button to
    /////     display this UI will be included in the header cell by default. /// If <see
    /////     cref="HeaderCellItemTemplate" /> is used, it is left up to that template to render any
    /////     relevant "show options" UI and invoke the grid's <see
    /////     cref="TnTDataGrid{TGridItem}.ShowColumnOptionsAsync(TnTColumnBase{TGridItem})" />).
    ///// </summary>
    //[Parameter]
    //public RenderFragment? ColumnOptions { get; set; }

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddTextAlign(TextAlign)
        .Build();

    /// <inheritdoc />
    public override string? ElementStyle => CssStyleBuilder.Create()
            .AddFromAdditionalAttributes(AdditionalAttributes)
            .Build();

    /// <summary>
    ///     Gets a reference to the enclosing <see cref="TnTDataGrid{TGridItem}" />.
    /// </summary>
    public TnTDataGrid<TGridItem> Grid => InternalGridContext.Grid;

    /// <summary>
    /// The alignment of the header content. This is used to align the header content within the cell.
    /// </summary>
    [Parameter]
    public TextAlign HeaderAlignment { get; set; } = TnTComponents.TextAlign.Center;

    /// <summary>
    ///     Gets or sets an optional template for this column's header cell. If not specified, the
    ///     default header template includes the <see cref="Title" /> along with any applicable sort
    ///     indicators and options buttons.
    /// </summary>
    [Parameter]
    public RenderFragment<TnTColumnBase<TGridItem>>? HeaderCellItemTemplate { get; set; }

    /// <summary>
    ///     Gets or sets the initial sort direction. if <see cref="IsDefaultSortColumn" /> is true.
    /// </summary>
    [Parameter]
    public SortDirection InitialSortDirection { get; set; } = default;

    /// <summary>
    ///     Gets or sets a value indicating whether this column should be sorted by default.
    /// </summary>
    [Parameter]
    public bool IsDefaultSortColumn { get; set; } = false;

    /// <summary>
    ///     If specified, virtualized grids will use this template to render cells whose data has
    ///     not yet been loaded.
    /// </summary>
    [Parameter]
    public RenderFragment<PlaceholderContext>? PlaceholderTemplate { get; set; }

    /// <summary>
    /// Indicates whether the sort icon should be shown in the header cell. 
    /// </summary>
    public bool ShowSortIcon { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the data should be sortable by this column. ///
    ///     The default value may vary according to the column type (for example, a <see
    ///     cref="TnTTemplateColumn{TGridItem}" /> is sortable by default if any <see
    ///     cref="TnTTemplateColumn{TGridItem}.SortBy" /> parameter is specified).
    /// </summary>
    [Parameter]
    public bool Sortable { get; set; }

    /// <summary>
    ///     Gets or sets the sorting rules for a column.
    /// </summary>
    public abstract TnTGridSort<TGridItem>? SortBy { get; set; }

    /// <summary>
    /// The alignment of the cell content. 
    /// </summary>
    [Parameter]
    public TextAlign? TextAlign { get; set; }

    /// <summary>
    ///     Gets or sets the title text for the column. This is rendered automatically if <see
    ///     cref="HeaderCellItemTemplate" /> is not used.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    [CascadingParameter]
    internal TnTInternalGridContext<TGridItem> InternalGridContext { get; set; } = default!;

    /// <summary>
    ///     Gets or sets a <see cref="RenderFragment" /> that will be rendered for this column's
    ///     header cell. This allows derived components to change the header output. However,
    ///     derived components are then responsible for using <see cref="HeaderCellItemTemplate" />
    ///     within that new output if they want to continue respecting that option.
    /// </summary>
    protected internal RenderFragment HeaderContent { get; protected set; }

    /// <summary>
    ///     Constructs an instance of <see cref="TnTColumnBase{TGridItem}" />.
    /// </summary>
    public TnTColumnBase() {
        HeaderContent = RenderDefaultHeaderContent;
    }

    /// <summary>
    ///     Overridden by derived components to provide rendering logic for the column's cells.
    /// </summary>
    /// <param name="builder">The current <see cref="RenderTreeBuilder" />.</param>
    /// <param name="item">   The data for the row being rendered.</param>
    protected internal abstract void CellContent(RenderTreeBuilder builder, TGridItem item);

    /// <summary>
    ///     Overridden by derived components to provide the raw content for the column's cells.
    /// </summary>
    /// <param name="item">The data for the row being rendered.</param>
    protected internal virtual string? RawCellContent(TGridItem item) => null;

    /// <summary>
    ///     Gets a value indicating whether this column should act as sortable if no value was set
    ///     for the <see cref="TnTColumnBase{TGridItem}.Sortable" /> parameter. The default behavior is
    ///     not to be sortable unless <see cref="TnTColumnBase{TGridItem}.Sortable" /> is true. ///
    ///     Derived components may override this to implement alternative default sortability rules.
    /// </summary>
    /// <returns>True if the column should be sortable by default, otherwise false.</returns>
    protected virtual bool IsSortableByDefault() => false;
}