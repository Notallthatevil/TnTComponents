namespace TnTComponents.Core;

/// <summary>
///     Represents the border radius for a component, allowing for different radii on each corner.
/// </summary>
public readonly struct TnTBorderRadius {

    /// <summary>
    ///     Gets a <see cref="TnTBorderRadius" /> with a full radius of 10 for all corners.
    /// </summary>
    public static TnTBorderRadius Full => new(10);

    /// <summary>
    ///     Gets a <see cref="TnTBorderRadius" /> with a half radius of 5 for all corners.
    /// </summary>
    public static TnTBorderRadius Half => new(5);

    /// <summary>
    ///     Gets a <see cref="TnTBorderRadius" /> with no radius for all corners.
    /// </summary>
    public static TnTBorderRadius None => new(0);

    /// <summary>
    ///     Gets a value indicating whether all corners have the same radius.
    /// </summary>
    public bool AllSame => StartStart == StartEnd && StartStart == EndStart && StartStart == EndEnd;

    /// <summary>
    ///     Gets the radius for the end-end corner.
    /// </summary>
    public int EndEnd { get; init; }

    /// <summary>
    ///     Gets the radius for the end-start corner.
    /// </summary>
    public int EndStart { get; init; }

    /// <summary>
    ///     Gets the radius for the start-end corner.
    /// </summary>
    public int StartEnd { get; init; }

    /// <summary>
    ///     Gets the radius for the start-start corner.
    /// </summary>
    public int StartStart { get; init; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TnTBorderRadius" /> struct with the same radius for all corners.
    /// </summary>
    /// <param name="radius">The radius for all corners.</param>
    public TnTBorderRadius(int radius) {
        StartStart = radius;
        StartEnd = radius;
        EndStart = radius;
        EndEnd = radius;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TnTBorderRadius" /> struct with default values.
    /// </summary>
    public TnTBorderRadius() {
    }
}