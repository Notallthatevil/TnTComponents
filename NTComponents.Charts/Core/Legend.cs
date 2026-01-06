namespace NTComponents.Charts.Core;

/// <summary>
/// Represents the legend configuration for a chart.
/// </summary>
public class Legend {
   /// <summary>
   /// Gets or sets a value indicating whether the legend is visible.
   /// </summary>
   public bool IsVisible { get; set; } = true;

   /// <summary>
   /// Gets or sets the position of the legend.
   /// </summary>
   public ChartPosition Position { get; set; } = ChartPosition.Bottom;

   /// <summary>
   /// Gets or sets the orientation of the legend items.
   /// </summary>
   public ChartOrientation Orientation { get; set; } = ChartOrientation.Horizontal;
}
