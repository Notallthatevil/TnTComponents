using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using TnTComponents.Core;

namespace TnTComponents.Grid.Infrastructure;

/// <summary>
///     Represents the header row for a data grid component, providing column headers and supporting generic grid item types.
/// </summary>
/// <typeparam name="TGridItem">The type of the grid item displayed in the data grid.</typeparam>
[CascadingTypeParameter(nameof(TGridItem))]
public partial class TnTDataGridHeaderRow<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields)] TGridItem>;