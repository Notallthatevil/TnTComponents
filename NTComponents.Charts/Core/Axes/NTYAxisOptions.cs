using SkiaSharp;

namespace NTComponents.Charts.Core.Axes;

/// <summary>
///     Options for the Y axis of a cartesian chart.
/// </summary>
public class NTYAxisOptions : NTAxisOptions {

    /// <inheritdoc />
    internal override SKRect Measure<TData>(SKRect renderArea, NTChart<TData> chart) {
        float labelWidth = 35;
        float titleWidth = string.IsNullOrEmpty(Title) ? 0 : 25;
        var totalAxisWidth = labelWidth + titleWidth + 5;
        return new SKRect(renderArea.Left + totalAxisWidth, renderArea.Top, renderArea.Right, renderArea.Bottom);
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


        var xLine = plotArea.Left;
        canvas.DrawLine(xLine, plotArea.Top, xLine, plotArea.Bottom, linePaint);

        if (Scale == NTAxisScale.Logarithmic) {
            var (min, max) = chart.GetYRange(true);
            min = Math.Max(0.000001, min);
            max = Math.Max(min * 1.1, max);

            var startLog = (int)Math.Floor(Math.Log10(min));
            var endLog = (int)Math.Ceiling(Math.Log10(max));

            for (var log = startLog; log <= endLog; log++) {
                var val = Math.Pow(10, log);
                if (val < min || val > max) {
                    continue;
                }

                var screenCoord = chart.ScaleY(val, plotArea);
                if (screenCoord < plotArea.Top - 1 || screenCoord > plotArea.Bottom + 1) {
                    continue;
                }

                canvas.DrawText(val.ToString("G"), xLine - 5, screenCoord + 5, SKTextAlign.Right, textFont, textPaint);
            }
        }
        else if (chart.UseNiceNumbers) {
            var (niceMin, niceMax, spacing) = chart.CalculateNiceScaling(yMinReal, yMaxReal);
            var totalLabels = (int)Math.Round((niceMax - niceMin) / spacing) + 1;
            for (var i = 0; i < totalLabels; i++) {
                var val = niceMin + (i * spacing);
                var screenCoord = chart.ScaleY(val, plotArea);

                if (screenCoord < plotArea.Top - 1 || screenCoord > plotArea.Bottom + 1) {
                    continue;
                }

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

                if (screenCoord < plotArea.Top - 1 || screenCoord > plotArea.Bottom + 1) {
                    continue;
                }

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
