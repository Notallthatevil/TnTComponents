namespace NTComponents.Charts.Core;

/// <summary>
/// Represents the margins around a chart.
/// </summary>
public record struct ChartMargin(float Left, float Top, float Right, float Bottom) {
   /// <summary>
   /// Gets a margin with all sides set to zero.
   /// </summary>
   public static ChartMargin Empty => new(0, 0, 0, 0);

   /// <summary>
   /// Gets a margin with all sides set to the same value.
   /// </summary>
   /// <param name="all">The value for all sides.</param>
   /// <returns>A new <see cref="ChartMargin"/>.</returns>
   public static ChartMargin All(float all) => new(all, all, all, all);
}
