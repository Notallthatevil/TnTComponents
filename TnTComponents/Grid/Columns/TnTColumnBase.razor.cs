using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using System.Diagnostics;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;
using TnTComponents.Core;
using TnTComponents.Grid.Infrastructure;

namespace TnTComponents.Grid.Columns;

[CascadingTypeParameter(nameof(TGridItem))]
[DebuggerDisplay("Title = {Title}, Order = {Order}, Sortable = {Sortable}, IsSortedOn = {IsSortedOn}")]
public abstract partial class TnTColumnBase<TGridItem> {
    public int ColumnId { get; internal set; } = -1;

    [Parameter]
    public RenderFragment<TnTColumnBase<TGridItem>>? HeaderCellItemTemplate { get; set; }

    [Parameter]
    public int Order { get; set; }

    [Parameter]
    public bool Sortable { get; set; }

    [CascadingParameter]
    internal TnTInternalGridContext<TGridItem> Context { get; set; } = default!;

    public void Dispose() {
        Context?.RegisterColumn(this);
        GC.SuppressFinalize(this);
    }

    protected override void OnInitialized() {
        base.OnInitialized();
        ArgumentNullException.ThrowIfNull(Context, nameof(Context));
        Context.RegisterColumn(this);
    }

    [Parameter]
    public TextAlign HeaderAlignment { get; set; } = TnTComponents.TextAlign.Left;

    internal bool NewColumn { get; set; }

    [Parameter]
    public bool EnableRipple { get; set; } = true;

    public SortDirection? IsSortedOn => Context?.ColumnIsSortedOn(this);

    public abstract RenderFragment RenderCellContent(TGridItem gridItem);

    [Parameter]
    public SortDirection InitialSortDirection { get; set; }

    [Parameter]
    public bool IsDefaultSortColumn { get; set; }

    // /// <summary> /// If specified, virtualized grids will use this template to render cells whose data has not yet been loaded. /// </summary> [Parameter] public
    // RenderFragment<PlaceholderContext>? PlaceholderTemplate { get; set; }
    public abstract TnTGridSort<TGridItem>? SortBy { get; set; }

    [Parameter]
    public TextAlign? TextAlign { get; set; }

    private int? _sortIndex => Context.GetSortIndex(this);

    ///<summary>
    ///     Gets or sets the title text for the column. This is rendered automatically if <see cref="HeaderCellItemTemplate" /> is not used. ///
    ///</summary>
    [Parameter]
    public string? Title { get; set; }

    private string _headerClass => CssClassBuilder.Create()
        .AddClass("tnt-header-content")
        .AddTextAlign(HeaderAlignment)
        .AddClass("tnt-interactable", Sortable)
        .AddClass("tnt-column-header-sorted-on", IsSortedOn.HasValue)
        .Build();

    public async Task SortAsync() {
        Context.SortByColumn(this);
        await Context.RefreshAsync();
    }

    protected override void OnParametersSet() {
        base.OnParametersSet();
        if (NewColumn) {
            if (Sortable && IsDefaultSortColumn) {
                Context.SortByColumn(this);
            }
            NewColumn = false;
        }

    }
    // internal RenderFragment? RenderPlaceholderContent(PlaceholderContext placeholderContext) => PlaceholderTemplate is not null ? PlaceholderTemplate(placeholderContext) : null;

    // ///
    // <summary>
    //     /// Overridden by derived components to provide rendering logic for the column's cells. ///
    // </summary>
    // ///
    // <param name="builder">The current <see cref="RenderTreeBuilder" />.</param>
    // ///
    // <param name="item">   The data for the row being rendered.</param>
    // protected internal abstract void CellContent(RenderTreeBuilder builder, TGridItem item);

    // ///
    // <summary>
    //     /// Overridden by derived components to provide the raw content for the column's cells. ///
    // </summary>
    // ///
    // <param name="item">The data for the row being rendered.</param>
    // protected internal virtual string? RawCellContent(TGridItem item) =&gt; null;

    // ///
    // <summary>
    //     /// Gets a value indicating whether this column should act as sortable if no value was set for the <see cref="TnTColumnBase{TGridItem}.Sortable" /> parameter. The default behavior is not to
    //     be /// sortable unless <see cref="TnTColumnBase{TGridItem}.Sortable" /> is true. /// Derived components may override this to implement alternative default sortability rules. ///
    // </summary>
    // ///
    // <returns>True if the column should be sortable by default, otherwise false.</returns>
    // protected virtual bool IsSortableByDefault() =&gt; false;
}