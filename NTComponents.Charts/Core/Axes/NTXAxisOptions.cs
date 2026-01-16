using SkiaSharp;

namespace NTComponents.Charts.Core.Axes;

/// <summary>
///     Options for the X axis of a cartesian chart.
/// </summary>
public class NTXAxisOptions : NTAxisOptions {

    /// <inheritdoc />
    internal override SKRect Measure<TData>(SKRect renderArea, NTChart<TData> chart) {
        float labelHeight = 18;
        float titleHeight = string.IsNullOrEmpty(Title) ? 0 : 20;
        var totalAxisHeight = labelHeight + titleHeight + 2;
        return new SKRect(renderArea.Left, renderArea.Top, renderArea.Right, renderArea.Bottom - totalAxisHeight);
    }

    /// <inheritdoc />
    internal override void Render<TData>(SKCanvas canvas, SKRect plotArea, SKRect totalArea, NTChart<TData> chart) {
        var (xMinReal, xMaxReal) = chart.GetXRange(false);

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

        var yLine = plotArea.Bottom;
        canvas.DrawLine(plotArea.Left, yLine, plotArea.Right, yLine, linePaint);

        if (Scale == NTAxisScale.Logarithmic) {
            var (min, max) = chart.GetXRange(true);
            min = Math.Max(0.000001, min);
            max = Math.Max(min * 1.1, max);

            var startLog = (int)Math.Floor(Math.Log10(min));
            var endLog = (int)Math.Ceiling(Math.Log10(max));

            for (var log = startLog; log <= endLog; log++) {
                var val = Math.Pow(10, log);
                if (val < min || val > max) {
                    continue;
                }

                var screenCoord = chart.ScaleX(val, plotArea);
                if (screenCoord < plotArea.Left - 1 || screenCoord > plotArea.Right + 1) {
                    continue;
                }

                canvas.DrawText(val.ToString("G"), screenCoord, yLine + 14, SKTextAlign.Center, textFont, textPaint);
            }
        }
        else if (chart.UseNiceNumbers) {
            var (niceMin, niceMax, spacing) = chart.CalculateNiceScaling(xMinReal, xMaxReal);
            var totalLabels = (int)Math.Round((niceMax - niceMin) / spacing) + 1;
            for (var i = 0; i < totalLabels; i++) {
                var val = niceMin + (i * spacing);
                var screenCoord = chart.ScaleX(val, plotArea);

                if (screenCoord < plotArea.Left - 1 || screenCoord > plotArea.Right + 1) {
                    continue;
                }

                var textAlign = SKTextAlign.Center;
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
                var val = xMinReal + (t * (xMaxReal - xMinReal));
                var screenCoord = chart.ScaleX(val, plotArea);

                if (screenCoord < plotArea.Left - 1 || screenCoord > plotArea.Right + 1) {
                    continue;
                }

                var textAlign = SKTextAlign.Center;
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
}
