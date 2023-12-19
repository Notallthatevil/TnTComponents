using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Grid;
public record class TnTItemsProviderRequest {
    public required int Count { get; set; } = int.MaxValue;
    public required int Offset { get; set; } = 0;
    public string? OrderBy { get; set; }
    public bool Descending { get; set; }
}

