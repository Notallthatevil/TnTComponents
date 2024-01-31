namespace TnTComponents.Core;

public struct TnTBorderRadius {
    public bool AllSame { get; }
    public int EndEnd { get; init; }
    public int EndStart { get; init; }
    public int StartEnd { get; init; }
    public int StartStart { get; init; }

    public TnTBorderRadius(int radius) {
        StartStart = radius;
        StartEnd = radius;
        EndStart = radius;
        EndEnd = radius;
        AllSame = true;
    }

    public TnTBorderRadius() {
    }
}