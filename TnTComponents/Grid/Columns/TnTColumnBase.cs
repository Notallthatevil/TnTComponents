using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using System.Reflection.PortableExecutable;
using TnTComponents.Core;
using TnTComponents.Grid.Infrastructure;

namespace TnTComponents.Grid.Columns;

/// <summary>
///     An abstract base class for columns in a <see cref="TnTDataGrid{TGridItem}" />.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
public abstract class TnTColumnBase<TGridItem> : TnTComponentBase {

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
    ///     The alignment of the header content. This is used to align the header content within the cell.
    /// </summary>
    [Parameter]
    public TextAlign HeaderAlignment { get; set; } = TnTComponents.TextAlign.Left;

    /// <summary>
    ///     Gets or sets an optional template for this column's header cell. If not specified, the default header template includes the <see cref="Title" /> along with any applicable sort indicators
    ///     and options buttons.
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
    ///     If specified, virtualized grids will use this template to render cells whose data has not yet been loaded.
    /// </summary>
    [Parameter]
    public RenderFragment<PlaceholderContext>? PlaceholderTemplate { get; set; }

    /// <summary>
    ///     Indicates whether the sort icon should be shown in the header cell.
    /// </summary>
    public bool ShowSortIcon { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the data should be sortable by this column. /// The default value may vary according to the column type (for example, a <see
    ///     cref="TnTTemplateColumn{TGridItem}" /> is sortable by default if any <see cref="TnTTemplateColumn{TGridItem}.SortBy" /> parameter is specified).
    /// </summary>
    [Parameter]
    public bool Sortable { get; set; }

    /// <summary>
    ///     Gets or sets the sorting rules for a column.
    /// </summary>
    public abstract TnTGridSort<TGridItem>? SortBy { get; set; }

    /// <summary>
    ///     The alignment of the cell content.
    /// </summary>
    [Parameter]
    public TextAlign? TextAlign { get; set; }

    /// <summary>
    ///     Gets or sets the title text for the column. This is rendered automatically if <see cref="HeaderCellItemTemplate" /> is not used.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Cascading parameter that provides access to the enclosing <see cref="TnTDataGrid{TGridItem}" /> instance.
    /// </summary>
    [CascadingParameter]
    internal TnTInternalGridContext<TGridItem> Context { get; private set; } = default!;

    internal RenderFragment RenderDefaultHeaderContent() {
#pragma warning disable IDE0046 // Convert to conditional expression
        if (HeaderCellItemTemplate is not null) {
            return HeaderCellItemTemplate(this);
        }
        else if (Sortable) {
            return new RenderFragment(b => {
                b.OpenElement(0, "div");
                b.AddAttribute(10, "class", CssClassBuilder.Create()
                    .AddClass("tnt-header-content")
                    .AddClass("tnt-sorted", ShowSortIcon)
                    .AddTextAlign(HeaderAlignment)
                    .AddClass("tnt-interactable")
                    .AddRipple()
                    .Build());

                b.AddAttribute(20, "onclick", EventCallback.Factory.Create(this, () => Context.Grid.SortByColumnAsync(this)));
                b.AddContent(30, Title);

                if (ShowSortIcon) {
                    b.OpenComponent<MaterialIcon>(40);
                    b.AddAttribute(50, "Icon", Context.Grid.SortByAscending ? MaterialIcon.ArrowDropUp : MaterialIcon.ArrowDropDown);
                    b.AddAttribute(60, "Class", "tnt-sort-icon");
                    b.CloseComponent();
                }

                b.OpenComponent<TnTRippleEffect>(70);
                b.CloseComponent();

                b.CloseElement();
            });
        }
#pragma warning restore IDE0046 // Convert to conditional expression
        else {
            return new RenderFragment(b => {
                b.OpenElement(0, "div");
                b.AddAttribute(10, "class", CssClassBuilder.Create()
                    .AddClass("tnt-header-content")
                    .AddTextAlign(HeaderAlignment)
                    .Build());
                b.AddContent(20, Title);
                b.CloseElement();
            });
        }
    }

    internal RenderFragment? RenderPlaceholderContent(PlaceholderContext placeholderContext) => PlaceholderTemplate is not null ? PlaceholderTemplate(placeholderContext) : null;

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

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder) => Context.Grid.AddColumn(this, InitialSortDirection, IsDefaultSortColumn);

    /// <summary>
    ///     Gets a value indicating whether this column should act as sortable if no value was set for the <see cref="TnTColumnBase{TGridItem}.Sortable" /> parameter. The default behavior is not to be
    ///     sortable unless <see cref="TnTColumnBase{TGridItem}.Sortable" /> is true. /// Derived components may override this to implement alternative default sortability rules.
    /// </summary>
    /// <returns>True if the column should be sortable by default, otherwise false.</returns>
    protected virtual bool IsSortableByDefault() => false;

}