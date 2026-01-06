namespace NTComponents.Charts.Core;

/// <summary>
/// Defines the base requirements for a chart data series.
/// </summary>
public interface IChartSeries {
    /// <summary>
    /// Gets or sets the name of the series.
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the series is visible.
    /// </summary>
    bool IsVisible { get; set; }

    /// <summary>
    /// Gets or sets the color of the series. If null, a theme color will be used.
    /// </summary>
    string? Color { get; set; }
}
