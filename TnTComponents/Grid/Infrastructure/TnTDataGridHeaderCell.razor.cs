using Microsoft.AspNetCore.Components;
using TnTComponents.Core;
using TnTComponents.Grid.Columns;

namespace TnTComponents.Grid.Infrastructure;

/// <summary>
///     Represents a header cell in the TnT data grid for the specified grid item type.
/// </summary>
/// <typeparam name="TGridItem">The type of the grid item associated with this header cell.</typeparam>
[CascadingTypeParameter(nameof(TGridItem))]public partial class TnTDataGridHeaderCell<TGridItem>;