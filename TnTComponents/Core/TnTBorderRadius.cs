using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Core;
public struct TnTBorderRadius {
    public int StartStart { get; init; }
    public int StartEnd { get; init; }
    public int EndStart { get; init; }
    public int EndEnd { get; init; }
    public bool AllSame { get; }

    public TnTBorderRadius(int radius) {
        StartStart = radius;
        StartEnd = radius;
        EndStart = radius;
        EndEnd = radius;
        AllSame = true;
    }

    public TnTBorderRadius() { }
}

