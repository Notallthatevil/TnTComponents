using Microsoft.AspNetCore.Components;
using System.Text;
using TnTComponents.Infrastructure;

namespace TnTComponents.Grid;

public abstract partial class TnTColumnBase<TGridItem> {

    public TnTColumnBase() {
        HeaderContent = RenderHeaderContent;
    }

    [Parameter]
    public override string BaseCssClass { get; set; } = "tnt-grid-column-content";

    [Parameter]
    public virtual string HeaderClass { get; set; } = "tnt-grid-column-header";

    [Parameter]
    public RenderFragment<TnTColumnBase<TGridItem>>? HeaderTemplate { get; set; }

    [Parameter]
    public bool Sortable { get; set; }

    [Parameter]
    public string? Title { get; set; }

    [CascadingParameter]
    internal TnTGridContext<TGridItem> Grid { get; set; } = default!;

    protected internal string ColumnContentClass => GetCssClass();

    protected internal string ColumnHeaderClass {
        get {
            var strBuilder = new StringBuilder(HeaderClass);
            if (Disabled) {
                strBuilder.Append(' ').Append("disabled");
            }

            if (Active && !Disabled) {
                strBuilder.Append(' ').Append("active");
            }

            return strBuilder.ToString();
        }
    }

    protected internal virtual RenderFragment HeaderContent { get; } = default!;

    public void Dispose() {
        Grid.RemoveColumn(this);
        GC.SuppressFinalize(this);
    }

    protected internal abstract RenderFragment CellContent(TGridItem item);

    protected override void OnInitialized() {
        if (Grid is null) {
            throw new InvalidOperationException($"A column must be a child of {nameof(TnTGrid<TGridItem>)}");
        }

        Grid.AddColumn(this);
        base.OnInitialized();
    }
}