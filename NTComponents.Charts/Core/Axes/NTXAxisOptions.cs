using SkiaSharp;
using System.Runtime.InteropServices;

namespace NTComponents.Charts.Core.Axes;

/// <summary>
///     Options for the X axis of a cartesian chart.
/// </summary>
public class NTXAxisOptions<TChartData> : NTAxisOptions<TChartData> where TChartData : class {

    private SKFont _textFont = default!;
    private SKPaint _titlePaint = default!;
    private SKFont _titleFont = default!;
    private SKPaint _linePaint = default!;
    private SKPaint _textPaint = default!;

    public NTXAxisOptions(NTChart<TChartData> chart) : base(chart) {
        Refresh();
    }

    public NTXAxisOptions() : base() { }

    private void DisposeSKResources() {
        _textFont?.Dispose();
        _titlePaint?.Dispose();
        _titleFont?.Dispose();
        _linePaint?.Dispose();
        _textPaint?.Dispose();
    }
    public override void Dispose() {
        DisposeSKResources();
    }

    public override void Refresh() {
        if (Chart == null) return;
        DisposeSKResources();
        _textFont = new SKFont {
            Size = 12,
            Typeface = Chart.DefaultTypeface
        };

        _titlePaint = new SKPaint {
            Color = Chart.GetThemeColor(Chart.TextColor),
            IsAntialias = true
        };

        _titleFont = new SKFont {
            Size = 16,
            Embolden = true,
            Typeface = Chart.DefaultTypeface
        };

        _linePaint = new SKPaint {
            Color = Chart.GetThemeColor(TnTColor.Outline),
            StrokeWidth = 1,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true
        };
        _textPaint = new SKPaint {
            Color = Chart.GetThemeColor(Chart.TextColor),
            IsAntialias = true
        };
    }
    /// <inheritdoc />
    internal override SKRect Measure(SKRect renderArea) {
        float labelHeight = 18;
        float titleHeight = string.IsNullOrEmpty(Title) ? 0 : 20;
        var totalAxisHeight = labelHeight + titleHeight + 2;
        return new SKRect(renderArea.Left, renderArea.Top, renderArea.Right, renderArea.Bottom - totalAxisHeight);
    }

    /// <inheritdoc />
    internal override void Render(SKCanvas canvas, SKRect plotArea, SKRect totalArea) {
        var (xMinReal, xMaxReal) = Chart.GetXRange(false);

        var yLine = plotArea.Bottom;
        canvas.DrawLine(plotArea.Left, yLine, plotArea.Right, yLine, _linePaint);

        if (Scale == NTAxisScale.Logarithmic) {
            var (min, max) = Chart.GetXRange(true);
            min = Math.Max(0.000001, min);
            max = Math.Max(min * 1.1, max);

            var startLog = (int)Math.Floor(Math.Log10(min));
            var endLog = (int)Math.Ceiling(Math.Log10(max));

            for (var log = startLog; log <= endLog; log++) {
                var val = Math.Pow(10, log);
                if (val < min || val > max) {
                    continue;
                }

                var screenCoord = Chart.ScaleX(val, plotArea);
                if (screenCoord < plotArea.Left - 1 || screenCoord > plotArea.Right + 1) {
                    continue;
                }

                canvas.DrawText(FormatLabel(val, Chart), screenCoord, yLine + 14, SKTextAlign.Center, _textFont, _textPaint);
            }
        }
        else if (Chart.UseNiceNumbers) {
            var (niceMin, niceMax, spacing) = Chart.CalculateNiceScaling(xMinReal, xMaxReal);
            var totalLabels = (int)Math.Round((niceMax - niceMin) / spacing) + 1;
            for (var i = 0; i < totalLabels; i++) {
                var val = niceMin + (i * spacing);
                var screenCoord = Chart.ScaleX(val, plotArea);

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

                canvas.DrawText(FormatLabel(val, Chart), screenCoord, yLine + 14, textAlign, _textFont, _textPaint);
            }
        }
        else {
            var labelCount = 5;
            for (var i = 0; i < labelCount; i++) {
                var t = i / (float)(labelCount - 1);
                var val = xMinReal + (t * (xMaxReal - xMinReal));
                var screenCoord = Chart.ScaleX(val, plotArea);

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

                canvas.DrawText(FormatLabel(val, Chart), screenCoord, yLine + 14, textAlign, _textFont, _textPaint);
            }
        }

        if (!string.IsNullOrEmpty(Title)) {
            canvas.DrawText(Title, plotArea.Left + (plotArea.Width / 2), plotArea.Bottom + 34, SKTextAlign.Center, _titleFont, _titlePaint);
        }
    }

    internal virtual string FormatLabel<TData>(double value, NTChart<TData> chart) where TData : class {
        return chart.GetXLabel(value);
    }

}

