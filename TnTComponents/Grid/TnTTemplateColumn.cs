using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Grid;

namespace TnTComponents;
public class TnTTemplateColumn<TItem> : TnTColumnBase<TItem> {
    public override bool Sortable { get; set; } = false;

    [Parameter]
    public RenderFragment? HeaderTemplate { get; set; }

    [Parameter, EditorRequired]
    public RenderFragment<TItem> RowTemplate { get; set; } = default!;

    public override void RenderCellContent(RenderTreeBuilder builder, TItem item) =>builder.AddContent(0, RowTemplate(item));

    public override IOrderedQueryable<TItem> SortOn(IQueryable<TItem> items, bool descending) {
        throw new NotImplementedException();
    }
}
