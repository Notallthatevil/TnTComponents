using Microsoft.AspNetCore.Components;
using NTComponents.Charts.Core.Series;
using NTComponents.Charts.Core;
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
      float plotWidth = Chart.Orientation == NTChartOrientation.Vertical ? renderArea.Width : renderArea.Height;
      float itemWidth = plotWidth / Math.Max(1, allX.Count);
      float boxWidth = itemWidth * BoxWidthRatio;

      for (int i = 0; i < dataList.Count; i++)
      {
         var item = dataList[i];
         
         var args = new NTDataPointRenderArgs<TData>
         {
            Data = item,
            Index = i,
            Color = baseColor,
            GetThemeColor = Chart.GetThemeColor
         };
         OnDataPointRender?.Invoke(args);

         var xVal = Chart.GetScaledXValue(XValueSelector(item));
         var boxValues = BoxValueSelector(item);

         float centerPos = Chart.ScaleX(xVal, renderArea);

         // Animation: scale values from median or 0
         double animMin = boxValues.Median + (boxValues.Min - boxValues.Median) * easedProgress;
         double animQ1 = boxValues.Median + (boxValues.Q1 - boxValues.Median) * easedProgress;
         double animQ3 = boxValues.Median + (boxValues.Q3 - boxValues.Median) * easedProgress;
         double animMax = boxValues.Median + (boxValues.Max - boxValues.Median) * easedProgress;
         double animMedian = boxValues.Median;

         float minPos = Chart.ScaleY(animMin * visibilityFactor, renderArea);
         float q1Pos = Chart.ScaleY(animQ1 * visibilityFactor, renderArea);
         float medianPos = Chart.ScaleY(animMedian * visibilityFactor, renderArea);
         float q3Pos = Chart.ScaleY(animQ3 * visibilityFactor, renderArea);
         float maxPos = Chart.ScaleY(animMax * visibilityFactor, renderArea);

         var isPointHovered = Chart.HoveredSeries == this && Chart.HoveredPointIndex == i;
         var hoverFactor = HoverFactor;
         var currentColor = args.Color ?? baseColor;
         var color = (isPointHovered) ? currentColor : currentColor.WithAlpha((byte)(currentColor.Alpha * hoverFactor));

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

         if (Chart.Orientation == NTChartOrientation.Vertical) {
            // Draw Box
            var boxRect = new SKRect(centerPos - boxWidth / 2, q3Pos, centerPos + boxWidth / 2, q1Pos);
            canvas.DrawRect(boxRect, fillPaint);
            canvas.DrawRect(boxRect, strokePaint);

            // Draw Median
            canvas.DrawLine(centerPos - boxWidth / 2, medianPos, centerPos + boxWidth / 2, medianPos, strokePaint);

            // Draw Whiskers
            canvas.DrawLine(centerPos, q3Pos, centerPos, maxPos, strokePaint);
            canvas.DrawLine(centerPos, q1Pos, centerPos, minPos, strokePaint);

            float whiskerWidth = boxWidth * WhiskerWidthRatio;
            canvas.DrawLine(centerPos - whiskerWidth / 2, maxPos, centerPos + whiskerWidth / 2, maxPos, strokePaint);
            canvas.DrawLine(centerPos - whiskerWidth / 2, minPos, centerPos + whiskerWidth / 2, minPos, strokePaint);

            // Outliers
            if (boxValues.Outliers != null)
            {
               foreach (var outlier in boxValues.Outliers)
               {
                  float oy = Chart.ScaleY(outlier * visibilityFactor, renderArea);
                  RenderPoint(canvas, centerPos, oy, color);
               }
            }
         }
         else {
            // Horizontal: centerPos is Y coordinate, Pos variables are X coordinates
            var boxRect = new SKRect(Math.Min(q1Pos, q3Pos), centerPos - boxWidth / 2, Math.Max(q1Pos, q3Pos), centerPos + boxWidth / 2);
            canvas.DrawRect(boxRect, fillPaint);
            canvas.DrawRect(boxRect, strokePaint);

            // Draw Median
            canvas.DrawLine(medianPos, centerPos - boxWidth / 2, medianPos, centerPos + boxWidth / 2, strokePaint);

            // Draw Whiskers
            canvas.DrawLine(q3Pos, centerPos, maxPos, centerPos, strokePaint);
            canvas.DrawLine(q1Pos, centerPos, minPos, centerPos, strokePaint);

            float whiskerWidth = boxWidth * WhiskerWidthRatio;
            canvas.DrawLine(maxPos, centerPos - whiskerWidth / 2, maxPos, centerPos + whiskerWidth / 2, strokePaint);
            canvas.DrawLine(minPos, centerPos - whiskerWidth / 2, minPos, centerPos + whiskerWidth / 2, strokePaint);

            // Outliers
            if (boxValues.Outliers != null)
            {
               foreach (var outlier in boxValues.Outliers)
               {
                  float ox = Chart.ScaleY(outlier * visibilityFactor, renderArea);
                  RenderPoint(canvas, ox, centerPos, color);
               }
            }
         }
      }
   }

   public override (int Index, TData? Data)? HitTest(SKPoint point, SKRect renderArea)
   {
      if (Data == null || !Data.Any()) return null;
      var dataList = Data.ToList();
      var allX = Chart.GetAllXValues();
      float plotWidth = Chart.Orientation == NTChartOrientation.Vertical ? renderArea.Width : renderArea.Height;
      float itemWidth = plotWidth / Math.Max(1, allX.Count);
      float boxWidth = itemWidth * BoxWidthRatio;

      for (int i = 0; i < dataList.Count; i++)
      {
         var item = dataList[i];
         var xVal = Chart.GetScaledXValue(XValueSelector(item));
         var boxValues = BoxValueSelector(item);
         float centerPos = Chart.ScaleX(xVal, renderArea);

         float minPos = Chart.ScaleY(boxValues.Min, renderArea);
         float maxPos = Chart.ScaleY(boxValues.Max, renderArea);

         SKRect hitRect;
         if (Chart.Orientation == NTChartOrientation.Vertical) {
            hitRect = new SKRect(centerPos - boxWidth / 2, Math.Min(minPos, maxPos), centerPos + boxWidth / 2, Math.Max(minPos, maxPos));
         }
         else {
            hitRect = new SKRect(Math.Min(minPos, maxPos), centerPos - boxWidth / 2, Math.Max(minPos, maxPos), centerPos + boxWidth / 2);
         }
         
         if (hitRect.Contains(point)) return (i, item);
      }

      return null;
   }
}
