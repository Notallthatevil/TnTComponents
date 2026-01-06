namespace NTComponents.Charts.Core;

/// <summary>
/// A base implementation for a chart data series containing points of type <typeparamref name="TPoint"/>.
/// </summary>
/// <typeparam name="TPoint">The type of data points in this series.</typeparam>
public abstract class ChartSeries<TPoint> : IChartSeries {
   /// <inheritdoc />
   public string Name { get; set; } = string.Empty;

   /// <inheritdoc />
   public bool IsVisible { get; set; } = true;

   /// <inheritdoc />
   public string? Color { get; set; }

   /// <summary>
   /// Gets or sets the collection of data points in this series.
   /// </summary>
   public IEnumerable<TPoint> Points { get; set; } = [];
}
