using NTComponents.Charts.Core.Scales;

namespace NTComponents.Charts.Core.Cartesian;

/// <summary>
/// Represents a linear axis in a Cartesian chart.
/// </summary>
public class LinearAxis : Axis {
   /// <summary>
   /// Gets or sets the scale used by this axis.
   /// </summary>
   public IScale Scale { get; set; } = new LinearScale();
}
