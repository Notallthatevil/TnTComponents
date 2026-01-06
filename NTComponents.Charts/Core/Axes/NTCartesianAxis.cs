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
         float labelHeight = 18;
         float titleHeight = string.IsNullOrEmpty(Title) ? 0 : 20;
         float totalAxisHeight = labelHeight + titleHeight + 2;
         return new SKRect(renderArea.Left, renderArea.Top, renderArea.Right, renderArea.Bottom - totalAxisHeight);
      }
      else
      {
         float labelWidth = 35;
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
         Color = Chart.GetThemeColor(Chart.TextColor),
         IsAntialias = true
      };

      using var textFont = new SKFont
      {
         Size = 12
      };

      using var titlePaint = new SKPaint
      {
         Color = Chart.GetThemeColor(Chart.TextColor),
         IsAntialias = true
      };

      using var titleFont = new SKFont
      {
         Size = 16,
         Embolden = true
      };

      using var linePaint = new SKPaint
      {
         Color = Chart.GetThemeColor(TnTColor.Outline),
         StrokeWidth = 1,
         Style = SKPaintStyle.Stroke,
         IsAntialias = true
      };

      if (Direction == AxisDirection.X)
      {
         var yLine = plotArea.Bottom;
         canvas.DrawLine(plotArea.Left, yLine, plotArea.Right, yLine, linePaint);

         // Draw labels
         var allX = Chart.GetAllXValues();
         if (allX.Any())
         {
            for (int i = 0; i < allX.Count; i++)
            {
               double val = Chart.IsCategoricalX ? i : allX[i];
               float x = Chart.ScaleX(val, plotArea);
               var label = allX[i].ToString("0.#");

               SKTextAlign textAlign = SKTextAlign.Center;
               if (!Chart.IsCategoricalX)
               {
                  if (i == 0 && allX.Count > 1) textAlign = SKTextAlign.Left;
                  else if (i == allX.Count - 1 && allX.Count > 1) textAlign = SKTextAlign.Right;
               }

               canvas.DrawText(label, x, yLine + 14, textAlign, textFont, textPaint);
            }
         }

         if (!string.IsNullOrEmpty(Title))
         {
            canvas.DrawText(Title, plotArea.Left + plotArea.Width / 2, plotArea.Bottom + 34, SKTextAlign.Center, titleFont, titlePaint);
         }
      }
      else
      {
         var xLine = plotArea.Left;
         canvas.DrawLine(xLine, plotArea.Top, xLine, plotArea.Bottom, linePaint);

         // Draw labels
         int labelCount = 5;
         for (int i = 0; i < labelCount; i++)
         {
            float t = i / (float)(labelCount - 1);
            var val = yMinReal + t * (yMaxReal - yMinReal);
            var y = plotArea.Bottom - (float)((val - yMin) / (yMax - yMin)) * plotArea.Height;

            float yOffset = 5;
            if (i == 0) yOffset = 0;
            else if (i == labelCount - 1) yOffset = 10;

            canvas.DrawText(val.ToString("0.#"), xLine - 5, y + yOffset, SKTextAlign.Right, textFont, textPaint);
         }

         if (!string.IsNullOrEmpty(Title))
         {
            var xTitle = totalArea.Left + 8;
            var yTitle = plotArea.Top + plotArea.Height / 2;
            canvas.Save();
            canvas.RotateDegrees(-90, xTitle, yTitle);
            canvas.DrawText(Title, xTitle, yTitle, SKTextAlign.Center, titleFont, titlePaint);
            canvas.Restore();
         }
      }
   }
}
