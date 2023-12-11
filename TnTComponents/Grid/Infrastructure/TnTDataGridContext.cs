using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Grid.Infrastructure;
internal sealed class TnTDataGridContext<TGridItem>(TnTDataGrid<TGridItem> grid) {

    public TnTDataGrid<TGridItem> Grid { get; } = grid;

    public Dictionary<string, TnTDataGridRow<TGridItem>> Rows { get; set; } = [];

    private int _rowIndex = 0;

    public void RegisterRow(TnTDataGridRow<TGridItem> row) {
        Rows.Add(row.ComponentIdentifier, row);
        if (!Grid.Virtualize) {
            row.RowIndex = _rowIndex++;
        }
    }

    public void RemoveRow(TnTDataGrid<TGridItem> row) {
        Rows.Remove(row.ComponentIdentifier);
    }
}

