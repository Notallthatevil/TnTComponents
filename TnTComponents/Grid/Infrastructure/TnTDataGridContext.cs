using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Events;
using TnTComponents.Grid.Columns;

namespace TnTComponents.Grid.Infrastructure;
internal sealed class TnTDataGridContext<TGridItem>(TnTDataGrid<TGridItem> grid) {

    internal delegate void ColumnsCollected();

    public TnTDataGrid<TGridItem> Grid { get; } = grid;

    public Dictionary<string, TnTDataGridRow<TGridItem>> Rows { get; set; } = [];

    public List<TnTColumnBase<TGridItem>> Columns = [];

    public EventCallback<DataGridRowClickEventArgs> RowClicked { get; set; }

    public string DataGridName { get; set; } = default!;

    private int _rowIndex = 0;

    public void RegisterRow(TnTDataGridRow<TGridItem> row) {
        Rows.Add(row.ComponentIdentifier, row);
        if (!Grid.Virtualize) {
            row.RowIndex = _rowIndex++;
        }
    }

    public void RemoveRow(TnTDataGridRow<TGridItem> row) {
        Rows.Remove(row.ComponentIdentifier);
    }

    internal void AddColumn(TnTColumnBase<TGridItem> column) {
        Columns.Add(column);
    }
}

