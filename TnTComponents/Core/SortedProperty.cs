using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Core;
public readonly struct SortedProperty {
    public required string PropertyName { get; init; }
    public required SortDirection Direction { get; init; }
}

