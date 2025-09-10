using System.Diagnostics.CodeAnalysis;

namespace TnTComponents.Core;
/// <summary>
///     Defines the direction of sorting for a property.
/// </summary>
[ExcludeFromCodeCoverage]
public readonly record struct SortedProperty {
    /// <summary>
    ///     The name of the property to be sorted.
    /// </summary>
    public required string PropertyName { get; init; }
    /// <summary>
    ///     The direction of the sort.
    /// </summary>
    public required SortDirection Direction { get; init; }
}