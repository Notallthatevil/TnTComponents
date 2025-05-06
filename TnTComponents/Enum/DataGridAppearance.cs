namespace TnTComponents;

/// <summary>
///     Specifies the appearance options for a DataGrid.
/// </summary>
[Flags]
public enum DataGridAppearance {

    /// <summary>
    ///     Default appearance with standard padding and no row striping.
    /// </summary>
    Default = 0,

    /// <summary>
    ///     Alternating rows have different background colors to improve readability.
    /// </summary>
    Stripped = 1,

    /// <summary>
    ///     Reduces padding and margins for a more condensed layout with more data visible.
    /// </summary>
    Compact = 2,
}