using SkiaSharp;

namespace NTComponents.Charts.Core;

/// <summary>
/// Defines a component that can perform hit-testing.
/// </summary>
public interface IHitTestable {
   /// <summary>
   /// Checks if a point hits this component.
   /// </summary>
   /// <param name="point">The point to test.</param>
   /// <returns>True if the point hits, false otherwise.</returns>
   bool HitTest(SKPoint point);
}
