using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using System.Linq.Expressions;
using TnTComponents.Common;
using TnTComponents.Grid.Infrastructure;

namespace TnTComponents.Grid.Columns;

public abstract class TnTColumnBase<TGridItem> : TnTComponentBase {

    [Parameter]
    public string? ColumnHeader { get; set; }

    [Parameter]
    public bool DefaultSort { get; set; }

    [Parameter]
    public RenderFragment<TnTColumnBase<TGridItem>>? HeaderTemplate { get; set; }

    [Parameter]
    public RenderFragment<PlaceholderContext>? PlaceholderTemplate { get; set; }

    [Parameter]
    public virtual bool Sortable { get; set; }

    [Parameter]
    public virtual Expression<Func<TGridItem, object>>? SortFunction { get; set; }

    [CascadingParameter]
    internal TnTDataGridContext<TGridItem> Context { get; set; } = default!;

    internal abstract void RenderCellContent(RenderTreeBuilder __builder, TGridItem item);

    internal RenderFragment RenderHeaderContent() {
        if (HeaderTemplate is not null) {
            return HeaderTemplate(this);
        }
        else {
            return new RenderFragment(builder => {
                builder.AddContent(0, ColumnHeader);
            });
        }
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        Context.AddColumn(this);
    }

    protected override void OnParametersSet() {
        if (Sortable && SortFunction is null) {
            throw new InvalidOperationException($"When setting {nameof(Sortable)} to {bool.TrueString}, a {nameof(SortFunction)} must be provided!");
        }
        base.OnParametersSet();
    }
}