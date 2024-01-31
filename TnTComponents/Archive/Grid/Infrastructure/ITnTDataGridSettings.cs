//using Microsoft.AspNetCore.Components;
//using System.Linq.Expressions;
//
//using TnTComponents.Enum;
//using TnTComponents.Events;

//namespace TnTComponents.Archive.Grid.Infrastructure;

///// <summary>
///// List of settings that apply to TnTDataGrids. Wrapped in an interface to facilitate easy passing
///// of settings around.
///// </summary>
///// <typeparam name="TGridItem">The type of the grid item.</typeparam>
//internal interface ITnTDataGridSettings<TGridItem> : ITnTComponentBase {
//    /// <summary>
//    /// Gets the appearance of the table.
//    /// </summary>
//    DataGridAppearance Appearance { get; }

// ///
// <summary>
// /// Gets the cell content container's css class. ///
// </summary>
// string CellContentContainerClass { get; }

// ///
// <summary>
// /// Gets the container's css class. ///
// </summary>
// string ContainerClass { get; }

// /// <summary> /// Optional value indicating the default sort option /// </summary>
// Expression<Func<TGridItem, object>>? DefaultSort { get; }

// ///
// <summary>
// /// The height of the table ///
// </summary>
// double? Height { get; }

// ///
// <summary>
// /// Gets the type of icons to use ///
// </summary>
// IconType IconType { get; }

// /// <summary> /// Gets or sets the items. /// </summary> IQueryable<TGridItem>? Items { get; set; }

// TnTItemsProvider<TGridItem>? ItemsProvider { get; }

// ///
// <summary>
// /// Gets the maximum height of the table. ///
// </summary>
// double? MaxHeight { get; }

// ///
// <summary>
// /// The name of the table ///
// </summary>
// string Name { get; }

// /// <summary> /// Gets the row clicked callback. /// </summary>
// EventCallback<DataGridRowClickEventArgs> RowClickedCallback { get; }

// ///
// <summary>
// /// Gets a value indicating whether to show row indexes ///
// </summary>
// bool ShowRowIndex { get; }

//    /// <summary>
//    /// Gets a value indicating whether this <see cref="ITnTDataGridSettings{TGridItem}" /> is virtualized.
//    /// </summary>
//    bool Virtualize { get; }
//}