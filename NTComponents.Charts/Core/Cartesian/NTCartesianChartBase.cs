using Microsoft.AspNetCore.Components;
using SkiaSharp;

namespace NTComponents.Charts.Core.Cartesian;

/// <summary>
/// A base class for charts that use a Cartesian coordinate system (X and Y axes).
/// </summary>
/// <typeparam name="TPoint">The type of data points in the series.</typeparam>
public abstract class NTCartesianChartBase<TPoint> : NTChartBase
{
   /// <summary>
   /// Gets or sets the X-axis configuration.
   /// </summary>
   [Parameter]
   public Axis XAxis { get; set; } = new LinearAxis { Position = ChartPosition.Bottom };

   /// <summary>
   /// Gets or sets the Y-axis configuration.
   /// </summary>
   [Parameter]
   public Axis YAxis { get; set; } = new LinearAxis { Position = ChartPosition.Left };

   /// <summary>
   /// Gets or sets the data series for the chart.
   /// </summary>
   [Parameter]
   public List<ChartSeries<TPoint>> Series { get; set; } = [];

   /// <summary>
   /// Gets the area available for plotting data, after margins and axes are accounted for.
   /// </summary>
   protected SKRect PlotArea { get; private set; }

   protected override void Render(SKCanvas canvas, SKImageInfo info)
   {
      CalculateLayout(info);

      DrawAxes(canvas, info);

      canvas.Save();
      canvas.ClipRect(PlotArea);
      DrawSeries(canvas, info);
      canvas.Restore();

      DrawLegend(canvas, info);
   }

   /// <summary>
   /// Calculates the layout of the chart components.
   /// </summary>
   /// <param name="info">The image info.</param>
   protected virtual void CalculateLayout(SKImageInfo info)
   {
      // Simple initial implementation: use margins and fixed axis sizes
      float axisSize = 40; // Default axis width/height

      float left = Margin.Left + (YAxis.Position == ChartPosition.Left ? axisSize : 0);
      float right = info.Width - Margin.Right - (YAxis.Position == ChartPosition.Right ? axisSize : 0);
      float top = Margin.Top + (XAxis.Position == ChartPosition.Top ? axisSize : 0);
      float bottom = info.Height - Margin.Bottom - (XAxis.Position == ChartPosition.Bottom ? axisSize : 0);

      PlotArea = new SKRect(left, top, right, bottom);
   }

   /// <summary>
   /// Draws the axes and gridlines.
   /// </summary>
   protected virtual void DrawAxes(SKCanvas canvas, SKImageInfo info)
   {
      // Future: use PlotArea to draw axes and gridlines
   }

   /// <summary>
   /// Draws the data series.
   /// </summary>
   protected abstract void DrawSeries(SKCanvas canvas, SKImageInfo info);

   /// <summary>
   /// Draws the legend.
   /// </summary>
   protected virtual void DrawLegend(SKCanvas canvas, SKImageInfo info)
   {
      if (!Legend.IsVisible) return;
      // Future: Legend rendering logic
   }
}
