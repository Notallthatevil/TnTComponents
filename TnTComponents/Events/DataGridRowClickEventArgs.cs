using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Events;
public class DataGridRowClickEventArgs : MouseEventArgs {
    public object? Item { get; set; }
    public int RowIndex { get; set; }

    public DataGridRowClickEventArgs(MouseEventArgs args, object? item, int rowIndex) {
        foreach (var property in typeof(MouseEventArgs).GetProperties()) {
            typeof(DataGridRowClickEventArgs).GetProperty(property.Name)?.SetValue(this, property.GetValue(args));
        }

        Item = item;
        RowIndex = rowIndex;
    }
}

