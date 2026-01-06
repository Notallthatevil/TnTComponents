using Microsoft.AspNetCore.Components;
using SkiaSharp;

namespace NTComponents.Charts.Core.Polar;

/// <summary>
/// A base class for charts that use a Polar coordinate system.
/// </summary>
/// <typeparam name="TPoint">The type of data points in the series.</typeparam>
public abstract class PolarChartBase<TPoint> : NTChartBase {
   /// <summary>
   /// Gets or sets the data series for the chart.
   /// </summary>
   [Parameter]
   public List<ChartSeries<TPoint>> Series { get; set; } = [];

   /// <summary>
   /// Gets the center point of the polar chart.
   /// </summary>
   protected SKPoint Center { get; private set; }

   /// <summary>
   /// Gets the maximum radius of the polar chart.
   /// </summary>
   protected float Radius { get; private set; }

   protected override void Render(SKCanvas canvas, SKImageInfo info) {
      CalculateLayout(info);
      DrawGrid(canvas, info);
      DrawSeries(canvas, info);
   }

   /// <summary>
   /// Calculates the layout of the polar chart components.
   /// </summary>
   /// <param name="info">The image info.</param>
   protected virtual void CalculateLayout(SKImageInfo info) {
      var usableWidth = info.Width - Margin.Left - Margin.Right;
      var usableHeight = info.Height - Margin.Top - Margin.Bottom;

      Center = new SKPoint(Margin.Left + usableWidth / 2, Margin.Top + usableHeight / 2);
      Radius = Math.Min(usableWidth, usableHeight) / 2;
   }

   /// <summary>
   /// Draws the polar grid (circles and radial lines).
   /// </summary>
   protected virtual void DrawGrid(SKCanvas canvas, SKImageInfo info) {
      // Implementation for drawing polar grid
   }

   /// <summary>
   /// Draws the data series.
   /// </summary>
   protected abstract void DrawSeries(SKCanvas canvas, SKImageInfo info);
}
