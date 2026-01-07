namespace NTComponents.Charts.Core.Series;

/// <summary>
///     Represents the values for a box plot point.
/// </summary>
/// <param name="Min">The minimum value.</param>
/// <param name="Q1">The first quartile.</param>
/// <param name="Median">The median value.</param>
/// <param name="Q3">The third quartile.</param>
/// <param name="Max">The maximum value.</param>
/// <param name="Outliers">Optional list of outlier values.</param>
public record BoxPlotValues(double Min, double Q1, double Median, double Q3, double Max, double[]? Outliers = null);
