namespace NTComponents.Charts.Core.Scales;

/// <summary>
/// A linear scale implementation.
/// </summary>
public class LinearScale : IScale {
   /// <inheritdoc />
   public (double Min, double Max) Domain { get; set; } = (0, 100);

   /// <inheritdoc />
   public float Scale(double value, float rangeStart, float rangeEnd) {
      if (Math.Abs(Domain.Max - Domain.Min) < double.Epsilon) return rangeStart;

      var normalized = (float)((value - Domain.Min) / (Domain.Max - Domain.Min));
      return rangeStart + normalized * (rangeEnd - rangeStart);
   }
}
