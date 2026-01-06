using NTComponents.Charts.Core;
using NTComponents.Charts.Core.Series;
using SkiaSharp;
using System.Linq;

namespace NTComponents.Charts.Core.Axes;

/// <summary>
///     An axis for cartesian charts.
/// </summary>
public class NTCartesianAxis<TData> : NTAxis<TData> where TData : class
{

   /// <inheritdoc />
   public override SKRect Measure(SKRect renderArea)
   {
      if (Direction == AxisDirection.X)
      {
         float labelHeight = 20;
         float titleHeight = string.IsNullOrEmpty(Title) ? 0 : 25;
         float totalAxisHeight = labelHeight + titleHeight + 5;
         return new SKRect(renderArea.Left, renderArea.Top, renderArea.Right, renderArea.Bottom - totalAxisHeight);
      }
      else
      {
         float labelWidth = 40;
         float titleWidth = string.IsNullOrEmpty(Title) ? 0 : 25;
         float totalAxisWidth = labelWidth + titleWidth + 5;
         return new SKRect(renderArea.Left + totalAxisWidth, renderArea.Top, renderArea.Right, renderArea.Bottom);
      }
   }

   /// <inheritdoc />
   public override void Render(SKCanvas canvas, SKRect plotArea, SKRect totalArea)
   {
      var (xMin, xMax) = Chart.GetXRange(true);
      var (yMin, yMax) = Chart.GetYRange(true);
      var (xMinReal, xMaxReal) = Chart.GetXRange(false);
      var (yMinReal, yMaxReal) = Chart.GetYRange(false);

      using var textPaint = new SKPaint
      {
         Color = SKColors.Black,
         TextSize = 12,
         IsAntialias = true,
         TextAlign = SKTextAlign.Center
      };

      using var titlePaint = new SKPaint
      {
         Color = SKColors.Black,
         TextSize = 16,
         IsAntialias = true,
         TextAlign = SKTextAlign.Center,
         FakeBoldText = true
      };

      using var linePaint = new SKPaint
      {
         Color = SKColors.Gray,
         StrokeWidth = 1,
         Style = SKPaintStyle.Stroke,
         IsAntialias = true
      };

      if (Direction == AxisDirection.X)
      {
         var yLine = plotArea.Bottom;
         canvas.DrawLine(plotArea.Left, yLine, plotArea.Right, yLine, linePaint);

         // Draw labels
         int labelCount = 5;
         for (int i = 0; i < labelCount; i++)
         {
            float t = i / (float)(labelCount - 1);
            var val = xMinReal + t * (xMaxReal - xMinReal);
            var x = plotArea.Left + (float)((val - xMin) / (xMax - xMin)) * plotArea.Width;

            if (i == 0) textPaint.TextAlign = SKTextAlign.Left;
            else if (i == labelCount - 1) textPaint.TextAlign = SKTextAlign.Right;
            else textPaint.TextAlign = SKTextAlign.Center;

            canvas.DrawText(val.ToString("0.#"), x, yLine + 15, textPaint);
         }

         if (!string.IsNullOrEmpty(Title))
         {
            canvas.DrawText(Title, plotArea.Left + plotArea.Width / 2, totalArea.Bottom - 5, titlePaint);
         }
      }
      else
      {
         var xLine = plotArea.Left;
         canvas.DrawLine(xLine, plotArea.Top, xLine, plotArea.Bottom, linePaint);

         // Draw labels
         int labelCount = 5;
         textPaint.TextAlign = SKTextAlign.Right;
         for (int i = 0; i < labelCount; i++)
         {
            float t = i / (float)(labelCount - 1);
            var val = yMinReal + t * (yMaxReal - yMinReal);
            var y = plotArea.Bottom - (float)((val - yMin) / (yMax - yMin)) * plotArea.Height;

            float yOffset = 5;
            if (i == 0) yOffset = 0;
            else if (i == labelCount - 1) yOffset = 10;

            canvas.DrawText(val.ToString("0.#"), xLine - 5, y + yOffset, textPaint);
         }

         if (!string.IsNullOrEmpty(Title))
         {
            var xTitle = totalArea.Left + 15;
            var yTitle = plotArea.Top + plotArea.Height / 2;
            canvas.Save();
            canvas.RotateDegrees(-90, xTitle, yTitle);
            canvas.DrawText(Title, xTitle, yTitle, titlePaint);
            canvas.Restore();
         }
      }
   }
}
