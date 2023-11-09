using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using TnTComponents.Infrastructure;

namespace TnTComponents.Grid;
public abstract partial class TnTColumnBase<TGridItem> {

    [Parameter]
    public bool Sortable { get; set; }

    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public RenderFragment<TnTColumnBase<TGridItem>>? HeaderTemplate { get; set; }

    [Parameter]
    public override string BaseCssClass { get; set; } = "tnt-grid-column-content";

    [Parameter]
    public virtual string HeaderClass { get; set; } = "tnt-grid-column-header";

    protected internal string ColumnContentClass => GetCssClass();

    protected internal virtual RenderFragment HeaderContent { get; } = default!;
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

    [CascadingParameter]
    internal TnTGridContext<TGridItem> Grid { get; set; } = default!;

    public TnTColumnBase() {
        HeaderContent = RenderHeaderContent;
    }

    public void Dispose() {
        Grid.RemoveColumn(this);
        GC.SuppressFinalize(this);
    }

    protected override void OnInitialized() {
        if (Grid is null) {
            throw new InvalidOperationException($"A column must be a child of {nameof(TnTGrid<TGridItem>)}");
        }

        Grid.AddColumn(this);
        base.OnInitialized();
    }
    protected internal abstract RenderFragment CellContent(TGridItem item);

}
