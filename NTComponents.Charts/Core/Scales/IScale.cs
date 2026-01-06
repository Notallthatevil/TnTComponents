namespace NTComponents.Charts.Core.Scales;

/// <summary>
/// Defines a scale that maps data values to canvas coordinates.
/// </summary>
public interface IScale {
   /// <summary>
   /// Scales a data value to a coordinate within the specified range.
   /// </summary>
   /// <param name="value">The data value to scale.</param>
   /// <param name="rangeStart">The start coordinate of the range.</param>
   /// <param name="rangeEnd">The end coordinate of the range.</param>
   /// <returns>The scaled coordinate.</returns>
   float Scale(double value, float rangeStart, float rangeEnd);

   /// <summary>
   /// Gets or sets the data domain (min and max data values).
   /// </summary>
   (double Min, double Max) Domain { get; set; }
}
