using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Grid;

public abstract class TnTColumnBase<TItem> : ComponentBase, IDisposable {
    [CascadingParameter]
    private TnTDataGrid<TItem> _dataGrid { get; set; } = default!;

    [Parameter]
    public string? Title { get; set; }

    public abstract bool Sortable { get; set; }

    protected override void OnInitialized() {
        base.OnInitialized();
        _dataGrid.AddColumn(this);
    }

    public void Dispose() {
        _dataGrid.RemoveColumn(this);
        GC.SuppressFinalize(this);
    }

    public abstract IOrderedQueryable<TItem> SortOn(IQueryable<TItem> items, bool descending);

    public virtual void RenderHeaderContent(RenderTreeBuilder builder) => builder.AddContent(0, Title);
    public abstract void RenderCellContent(RenderTreeBuilder builder, TItem item);

}

