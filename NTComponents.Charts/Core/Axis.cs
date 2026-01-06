namespace NTComponents.Charts.Core;

/// <summary>
/// Represents a base axis for a chart.
/// </summary>
public abstract class Axis {
   /// <summary>
   /// Gets or sets the title of the axis.
   /// </summary>
   public string? Title { get; set; }

   /// <summary>
   /// Gets or sets a value indicating whether the axis is visible.
   /// </summary>
   public bool IsVisible { get; set; } = true;

   /// <summary>
   /// Gets or sets the position of the axis.
   /// </summary>
   public ChartPosition Position { get; set; }

   /// <summary>
   /// Gets or sets the scale type of the axis.
   /// </summary>
   public AxisScaleType ScaleType { get; set; } = AxisScaleType.Linear;

   /// <summary>
   /// Gets or sets whether to show grid lines for this axis.
   /// </summary>
   public bool ShowGridLines { get; set; } = true;
}
