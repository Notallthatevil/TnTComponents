using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics.CodeAnalysis;

namespace TnTComponents.Events;

/// <summary>
///     Provides data for the row click event in a data grid component. Extends <see cref="MouseEventArgs" /> with additional information about the clicked row.
/// </summary>
[ExcludeFromCodeCoverage]
public class DataGridRowClickEventArgs : MouseEventArgs {

    /// <summary>
    ///     Gets or sets the data item associated with the clicked row.
    /// </summary>
    public object? Item { get; set; }

    /// <summary>
    ///     Gets or sets the zero-based index of the clicked row.
    /// </summary>
    public int RowIndex { get; set; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="DataGridRowClickEventArgs" /> class.
    /// </summary>
    /// <param name="args">    The original mouse event arguments.</param>
    /// <param name="item">    The data item associated with the clicked row.</param>
    /// <param name="rowIndex">The zero-based index of the clicked row.</param>
    /// <remarks>This constructor copies all properties from the original <see cref="MouseEventArgs" /> and adds the row-specific information.</remarks>
    public DataGridRowClickEventArgs(MouseEventArgs args, object? item, int rowIndex) {
        foreach (var property in typeof(MouseEventArgs).GetProperties()) {
            typeof(DataGridRowClickEventArgs).GetProperty(property.Name)?.SetValue(this, property.GetValue(args));
        }

        Item = item;
        RowIndex = rowIndex;
    }
}