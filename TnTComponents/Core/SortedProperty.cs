using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Core;
public readonly record struct SortedProperty {
    /// <summary>
    ///     Gets or initializes the name of the property to be sorted.
    /// </summary>
    public required string PropertyName { get; init; }
    /// <summary>
    ///     Gets or initializes the direction of the sort.
    /// </summary>
    public required SortDirection Direction { get; init; }
}