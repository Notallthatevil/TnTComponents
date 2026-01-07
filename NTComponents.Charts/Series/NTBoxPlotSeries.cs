using Microsoft.AspNetCore.Components;
using NTComponents.Charts.Core.Series;
using SkiaSharp;

namespace NTComponents.Charts;

/// <summary>
///     Represents a box plot series in a cartesian chart.
/// </summary>
/// <typeparam name="TData">The type of the data.</typeparam>
public class NTBoxPlotSeries<TData> : NTCartesianSeries<TData> where TData : class
{
   [Parameter, EditorRequired]
   public Func<TData, BoxPlotValues> BoxValueSelector { get; set; } = default!;

   /// <summary>
   ///    Gets or sets the width of the box as a fraction of the available space (0.0 to 1.0).
   /// </summary>
   [Parameter]
   public float BoxWidthRatio { get; set; } = 0.6f;

   /// <summary>
   ///    Gets or sets the width of the whiskers as a fraction of the box width (0.0 to 1.0).
   /// </summary>
   [Parameter]
   public float WhiskerWidthRatio { get; set; } = 0.5f;

   public override void Render(SKCanvas canvas, SKRect renderArea)
   {
      if (Data == null || !Data.Any()) return;

      var dataList = Data.ToList();
      var (xMin, xMax) = Chart.GetXRange(true);
      var (yMin, yMax) = Chart.GetYRange(true);

      var isHovered = Chart.HoveredSeries == this;
      var hasHover = Chart.HoveredSeries != null;
      var baseColor = Chart.GetSeriesColor(this);
      var visibilityFactor = VisibilityFactor;

      var progress = GetAnimationProgress();
      var easedProgress = progress; // Linear for now

      // Determine available width for each categorical item
      var allX = Chart.GetAllXValues();
      float itemWidth = renderArea.Width / Math.Max(1, allX.Count);
      float boxWidth = itemWidth * BoxWidthRatio;

      for (int i = 0; i < dataList.Count; i++)
      {
         var item = dataList[i];
         var xVal = Chart.GetScaledXValue(XValueSelector(item));
         var boxValues = BoxValueSelector(item);

         float cx = Chart.ScaleX(xVal, renderArea);

         // Animation: scale values from median or 0
         double animMin = boxValues.Median + (boxValues.Min - boxValues.Median) * easedProgress;
         double animQ1 = boxValues.Median + (boxValues.Q1 - boxValues.Median) * easedProgress;
         double animQ3 = boxValues.Median + (boxValues.Q3 - boxValues.Median) * easedProgress;
         double animMax = boxValues.Median + (boxValues.Max - boxValues.Median) * easedProgress;
         double animMedian = boxValues.Median;

         float yMinPos = Chart.ScaleY(animMin * visibilityFactor, renderArea);
         float yQ1Pos = Chart.ScaleY(animQ1 * visibilityFactor, renderArea);
         float yMedianPos = Chart.ScaleY(animMedian * visibilityFactor, renderArea);
         float yQ3Pos = Chart.ScaleY(animQ3 * visibilityFactor, renderArea);
         float yMaxPos = Chart.ScaleY(animMax * visibilityFactor, renderArea);

         var isPointHovered = Chart.HoveredSeries == this && Chart.HoveredPointIndex == i;
         var color = (hasHover && !isHovered) ? baseColor.WithAlpha((byte)(baseColor.Alpha * 0.15f)) : baseColor;
         if (isPointHovered) color = baseColor.WithAlpha(255);

         using var strokePaint = new SKPaint
         {
            Style = SKPaintStyle.Stroke,
            Color = color,
            StrokeWidth = 2,
            IsAntialias = true
         };

         using var fillPaint = new SKPaint
         {
            Style = SKPaintStyle.Fill,
            Color = color.WithAlpha((byte)(color.Alpha * 0.3f)),
            IsAntialias = true
         };

         // Draw Box
         var boxRect = new SKRect(cx - boxWidth / 2, yQ3Pos, cx + boxWidth / 2, yQ1Pos);
         canvas.DrawRect(boxRect, fillPaint);
         canvas.DrawRect(boxRect, strokePaint);

         // Draw Median
         canvas.DrawLine(cx - boxWidth / 2, yMedianPos, cx + boxWidth / 2, yMedianPos, strokePaint);

         // Draw Whiskers
         canvas.DrawLine(cx, yQ3Pos, cx, yMaxPos, strokePaint);
         canvas.DrawLine(cx, yQ1Pos, cx, yMinPos, strokePaint);

         float whiskerWidth = boxWidth * WhiskerWidthRatio;
         canvas.DrawLine(cx - whiskerWidth / 2, yMaxPos, cx + whiskerWidth / 2, yMaxPos, strokePaint);
         canvas.DrawLine(cx - whiskerWidth / 2, yMinPos, cx + whiskerWidth / 2, yMinPos, strokePaint);

         // Outliers
         if (boxValues.Outliers != null)
         {
            foreach (var outlier in boxValues.Outliers)
            {
               float oy = Chart.ScaleY(outlier * visibilityFactor, renderArea);
               RenderPoint(canvas, cx, oy, color);
            }
         }
      }
   }

   public override (int Index, TData? Data)? HitTest(SKPoint point, SKRect renderArea)
   {
      if (Data == null || !Data.Any()) return null;
      var dataList = Data.ToList();
      var allX = Chart.GetAllXValues();
      float itemWidth = renderArea.Width / Math.Max(1, allX.Count);
      float boxWidth = itemWidth * BoxWidthRatio;

      for (int i = 0; i < dataList.Count; i++)
      {
         var item = dataList[i];
         var xVal = Chart.GetScaledXValue(XValueSelector(item));
         var boxValues = BoxValueSelector(item);
         float cx = Chart.ScaleX(xVal, renderArea);

         float yMinPos = Chart.ScaleY(boxValues.Min, renderArea);
         float yMaxPos = Chart.ScaleY(boxValues.Max, renderArea);

         var hitRect = new SKRect(cx - boxWidth / 2, Math.Min(yMinPos, yMaxPos), cx + boxWidth / 2, Math.Max(yMinPos, yMaxPos));
         if (hitRect.Contains(point)) return (i, item);
      }

      return null;
   }
}
