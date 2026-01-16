using SkiaSharp;

namespace NTComponents.Charts.Core.Axes;

/// <summary>
///     Options for the Y axis of a cartesian chart.
/// </summary>
public class NTYAxisOptions : NTAxisOptions {

   private bool IsVertical<TData>(NTChart<TData> chart) where TData : class => chart.Orientation == NTChartOrientation.Vertical;

   /// <inheritdoc />
   internal override SKRect Measure<TData>(SKRect renderArea, NTChart<TData> chart) {
      if (!IsVertical(chart)) {
         float labelHeight = 18;
         float titleHeight = string.IsNullOrEmpty(Title) ? 0 : 20;
         var totalAxisHeight = labelHeight + titleHeight + 2;
         return new SKRect(renderArea.Left, renderArea.Top, renderArea.Right, renderArea.Bottom - totalAxisHeight);
      }
      else {
         float labelWidth = 35;
         float titleWidth = string.IsNullOrEmpty(Title) ? 0 : 25;
         var totalAxisWidth = labelWidth + titleWidth + 5;
         return new SKRect(renderArea.Left + totalAxisWidth, renderArea.Top, renderArea.Right, renderArea.Bottom);
      }
   }

   /// <inheritdoc />
   internal override void Render<TData>(SKCanvas canvas, SKRect plotArea, SKRect totalArea, NTChart<TData> chart) {
      var (yMinReal, yMaxReal) = chart.GetYRange(false);

      using var textPaint = new SKPaint {
         Color = chart.GetThemeColor(chart.TextColor),
         IsAntialias = true
      };

      using var textFont = new SKFont {
         Size = 12,
         Typeface = chart.DefaultTypeface
      };

      using var titlePaint = new SKPaint {
         Color = chart.GetThemeColor(chart.TextColor),
         IsAntialias = true
      };

      using var titleFont = new SKFont {
         Size = 16,
         Embolden = true,
         Typeface = chart.DefaultTypeface
      };

      using var linePaint = new SKPaint {
         Color = chart.GetThemeColor(TnTColor.Outline),
         StrokeWidth = 1,
         Style = SKPaintStyle.Stroke,
         IsAntialias = true
      };

      var isVertical = IsVertical(chart);

      if (!isVertical) {
         var yLine = plotArea.Bottom;
         canvas.DrawLine(plotArea.Left, yLine, plotArea.Right, yLine, linePaint);

         if (ValuesToShow != null) {
            for (var i = 0; i < ValuesToShow.Count; i++) {
               var val = ValuesToShow[i];
               var screenCoord = chart.ScaleY(val, plotArea);

               if (screenCoord < plotArea.Left - 1 || screenCoord > plotArea.Right + 1) continue;

               var label = val.ToString("0.#");

               SKTextAlign textAlign = SKTextAlign.Center;
               if (i == 0 && ValuesToShow.Count > 1) {
                  textAlign = SKTextAlign.Left;
               }
               else if (i == ValuesToShow.Count - 1 && ValuesToShow.Count > 1) {
                  textAlign = SKTextAlign.Right;
               }

               canvas.DrawText(label, screenCoord, yLine + 14, textAlign, textFont, textPaint);
            }
         }
         else if (Scale == NTAxisScale.Logarithmic) {
            var (min, max) = chart.GetYRange(true);
            min = Math.Max(0.000001, min);
            max = Math.Max(min * 1.1, max);

            var startLog = (int)Math.Floor(Math.Log10(min));
            var endLog = (int)Math.Ceiling(Math.Log10(max));

            for (int log = startLog; log <= endLog; log++) {
               double val = Math.Pow(10, log);
               if (val < min || val > max) continue;

               var screenCoord = chart.ScaleY(val, plotArea);
               if (screenCoord < plotArea.Left - 1 || screenCoord > plotArea.Right + 1) continue;

               canvas.DrawText(val.ToString("G"), screenCoord, yLine + 14, SKTextAlign.Center, textFont, textPaint);
            }
         }
         else if (chart.UseNiceNumbers) {
            var (niceMin, niceMax, spacing) = chart.CalculateNiceScaling(yMinReal, yMaxReal);
            int totalLabels = (int)Math.Round((niceMax - niceMin) / spacing) + 1;
            for (int i = 0; i < totalLabels; i++) {
               double val = niceMin + i * spacing;
               var screenCoord = chart.ScaleY(val, plotArea);

               if (screenCoord < plotArea.Left - 1 || screenCoord > plotArea.Right + 1) continue;

               SKTextAlign textAlign = SKTextAlign.Center;
               if (i == 0) {
                  textAlign = SKTextAlign.Left;
               }
               else if (i == totalLabels - 1) {
                  textAlign = SKTextAlign.Right;
               }

               canvas.DrawText(val.ToString("0.#"), screenCoord, yLine + 14, textAlign, textFont, textPaint);
            }
         }
         else {
            var labelCount = 5;
            for (var i = 0; i < labelCount; i++) {
               var t = i / (float)(labelCount - 1);
               var val = yMinReal + (t * (yMaxReal - yMinReal));
               var screenCoord = chart.ScaleY(val, plotArea);

               if (screenCoord < plotArea.Left - 1 || screenCoord > plotArea.Right + 1) continue;

               SKTextAlign textAlign = SKTextAlign.Center;
               if (i == 0) {
                  textAlign = SKTextAlign.Left;
               }
               else if (i == labelCount - 1) {
                  textAlign = SKTextAlign.Right;
               }

               canvas.DrawText(val.ToString("0.#"), screenCoord, yLine + 14, textAlign, textFont, textPaint);
            }
         }

         if (!string.IsNullOrEmpty(Title)) {
            canvas.DrawText(Title, plotArea.Left + (plotArea.Width / 2), plotArea.Bottom + 34, SKTextAlign.Center, titleFont, titlePaint);
         }
      }
      else {
         var xLine = plotArea.Left;
         canvas.DrawLine(xLine, plotArea.Top, xLine, plotArea.Bottom, linePaint);

         if (ValuesToShow != null && ValuesToShow.Any()) {
            for (var i = 0; i < ValuesToShow.Count; i++) {
               var val = ValuesToShow[i];
               var screenCoord = chart.ScaleY(val, plotArea);

               if (screenCoord < plotArea.Top - 1 || screenCoord > plotArea.Bottom + 1) continue;

               var yOffset = 5;
               if (i == 0) {
                  yOffset = 0;
               }
               else if (i == ValuesToShow.Count - 1) {
                  yOffset = 10;
               }

               canvas.DrawText(val.ToString("0.#"), xLine - 5, screenCoord + yOffset, SKTextAlign.Right, textFont, textPaint);
            }
         }
         else if (Scale == NTAxisScale.Logarithmic) {
            var (min, max) = chart.GetYRange(true);
            min = Math.Max(0.000001, min);
            max = Math.Max(min * 1.1, max);

            var startLog = (int)Math.Floor(Math.Log10(min));
            var endLog = (int)Math.Ceiling(Math.Log10(max));

            for (int log = startLog; log <= endLog; log++) {
               double val = Math.Pow(10, log);
               if (val < min || val > max) continue;

               var screenCoord = chart.ScaleY(val, plotArea);
               if (screenCoord < plotArea.Top - 1 || screenCoord > plotArea.Bottom + 1) continue;

               canvas.DrawText(val.ToString("G"), xLine - 5, screenCoord + 5, SKTextAlign.Right, textFont, textPaint);
            }
         }
         else if (chart.UseNiceNumbers) {
            var (niceMin, niceMax, spacing) = chart.CalculateNiceScaling(yMinReal, yMaxReal);
            int totalLabels = (int)Math.Round((niceMax - niceMin) / spacing) + 1;
            for (int i = 0; i < totalLabels; i++) {
               double val = niceMin + i * spacing;
               var screenCoord = chart.ScaleY(val, plotArea);

               if (screenCoord < plotArea.Top - 1 || screenCoord > plotArea.Bottom + 1) continue;

               float yOffset = 5;
               if (i == 0) {
                  yOffset = 0;
               }
               else if (i == totalLabels - 1) {
                  yOffset = 10;
               }

               canvas.DrawText(val.ToString("0.#"), xLine - 5, screenCoord + yOffset, SKTextAlign.Right, textFont, textPaint);
            }
         }
         else {
            var labelCount = 5;
            for (var i = 0; i < labelCount; i++) {
               var t = i / (float)(labelCount - 1);
               var val = yMinReal + (t * (yMaxReal - yMinReal));
               var screenCoord = chart.ScaleY(val, plotArea);

               if (screenCoord < plotArea.Top - 1 || screenCoord > plotArea.Bottom + 1) continue;

               float yOffset = 5;
               if (i == 0) {
                  yOffset = 0;
               }
               else if (i == labelCount - 1) {
                  yOffset = 10;
               }

               canvas.DrawText(val.ToString("0.#"), xLine - 5, screenCoord + yOffset, SKTextAlign.Right, textFont, textPaint);
            }
         }

         if (!string.IsNullOrEmpty(Title)) {
            canvas.Save();
            canvas.RotateDegrees(-90, xLine - 45, plotArea.Top + (plotArea.Height / 2));
            canvas.DrawText(Title, xLine - 45, plotArea.Top + (plotArea.Height / 2), SKTextAlign.Center, titleFont, titlePaint);
            canvas.Restore();
         }
      }
   }
}
