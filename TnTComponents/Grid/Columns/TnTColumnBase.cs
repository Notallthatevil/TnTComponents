using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Common;
using TnTComponents.Grid.Infrastructure;

namespace TnTComponents.Grid.Columns;
public abstract class TnTColumnBase<TGridItem> : TnTComponentBase {

    [CascadingParameter]
    internal TnTDataGridContext<TGridItem> Context { get; set; } = default!;

    [Parameter]
    public RenderFragment<PlaceholderContext>? PlaceholderTemplate { get; set; }

    [Parameter]
    public RenderFragment<TnTColumnBase<TGridItem>>? HeaderTemplate { get; set; }

    [Parameter]
    public virtual bool Sortable { get; set; }

    [Parameter]
    public virtual Expression<Func<TGridItem, object>>? SortFunction { get; set; }

    [Parameter]
    public bool DefaultSort { get; set; }


    [Parameter]
    public string? ColumnHeader { get; set; }


    protected override void OnParametersSet() {
        if (Sortable && SortFunction is null) {
            throw new InvalidOperationException($"When setting {nameof(Sortable)} to {bool.TrueString}, a {nameof(SortFunction)} must be provided!");
        }
        base.OnParametersSet();

    }
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        Context.AddColumn(this);
    }

    internal RenderFragment RenderHeaderContent() {
        if(HeaderTemplate is not null) {
            return HeaderTemplate(this);
        }
        else {
            return new RenderFragment(builder => {
                builder.AddContent(0, ColumnHeader);
            });
        }
    }

    internal abstract void RenderCellContent(RenderTreeBuilder __builder, TGridItem item);
}

