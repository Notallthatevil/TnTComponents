using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public string? ColumnHeader { get; set; }

    private string id = TnTComponentIdentifier.NewId();

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        Context.Grid.AddColumn(this);
    }

    internal void RenderHeaderContent(RenderTreeBuilder __builder) {
        if (HeaderTemplate is not null) {
            __builder.AddContent(0, HeaderTemplate(this));
        }
        else {
            __builder.AddContent(0, ColumnHeader);
        }
    }

    internal abstract void RenderCellContent(RenderTreeBuilder __builder, TGridItem item);
}

