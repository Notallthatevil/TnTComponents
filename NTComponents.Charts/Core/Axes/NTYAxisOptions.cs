using SkiaSharp;
using System.Runtime.InteropServices;

namespace NTComponents.Charts.Core.Axes;

/// <summary>
///     Options for the Y axis of a cartesian chart.
/// </summary>
public class NTYAxisOptions<TChartData> : NTAxisOptions<TChartData> where TChartData : class {

    private SKFont _textFont = default!;
    private SKPaint _titlePaint = default!;
    private SKFont _titleFont = default!;
    private SKPaint _linePaint = default!;
    private SKPaint _textPaint = default!;

    public NTYAxisOptions(NTChart<TChartData> chart) : base(chart) {
        Refresh();
    }

    public NTYAxisOptions() : base() { }

    private void DisposeSKResources() {
        _textFont?.Dispose();
        _textPaint?.Dispose();
        _titlePaint?.Dispose();
        _titleFont?.Dispose();
        _linePaint?.Dispose();
    }
    public override void Dispose() {
        DisposeSKResources();
    }

    public override void Refresh() {
        if (Chart == null) return;
        DisposeSKResources();
        _textPaint = new SKPaint {
            Color = Chart.GetThemeColor(Chart.TextColor),
            IsAntialias = true
        };

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
    }


    /// <inheritdoc />
    internal override SKRect Measure(SKRect renderArea) {
        float labelWidth = 35;
        float titleWidth = string.IsNullOrEmpty(Title) ? 0 : 25;
        var totalAxisWidth = labelWidth + titleWidth + 5;
        return new SKRect(renderArea.Left + totalAxisWidth, renderArea.Top, renderArea.Right, renderArea.Bottom);
    }

    /// <inheritdoc />
    internal override void Render(SKCanvas canvas, SKRect plotArea, SKRect totalArea) {
        var (yMinReal, yMaxReal) = Chart.GetYRange(false);



        var xLine = plotArea.Left;
        canvas.DrawLine(xLine, plotArea.Top, xLine, plotArea.Bottom, _linePaint);

        if (Scale == NTAxisScale.Logarithmic) {
            var (min, max) = Chart.GetYRange(true);
            min = Math.Max(0.000001, min);
            max = Math.Max(min * 1.1, max);

            var startLog = (int)Math.Floor(Math.Log10(min));
            var endLog = (int)Math.Ceiling(Math.Log10(max));

            for (var log = startLog; log <= endLog; log++) {
                var val = Math.Pow(10, log);
                if (val < min || val > max) {
                    continue;
                }

                var screenCoord = Chart.ScaleY(val, plotArea);
                if (screenCoord < plotArea.Top - 1 || screenCoord > plotArea.Bottom + 1) {
                    continue;
                }

                canvas.DrawText(FormatLabel(val, Chart), xLine - 5, screenCoord + 5, SKTextAlign.Right, _textFont, _textPaint);
            }
        }
        else if (Chart.UseNiceNumbers) {
            var (niceMin, niceMax, spacing) = Chart.CalculateNiceScaling(yMinReal, yMaxReal);
            var totalLabels = (int)Math.Round((niceMax - niceMin) / spacing) + 1;
            for (var i = 0; i < totalLabels; i++) {
                var val = niceMin + (i * spacing);
                var screenCoord = Chart.ScaleY(val, plotArea);

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

                canvas.DrawText(FormatLabel(val, Chart), xLine - 5, screenCoord + yOffset, SKTextAlign.Right, _textFont, _textPaint);
            }
        }
        else {
            var labelCount = 5;
            for (var i = 0; i < labelCount; i++) {
                var t = i / (float)(labelCount - 1);
                var val = yMinReal + (t * (yMaxReal - yMinReal));
                var screenCoord = Chart.ScaleY(val, plotArea);

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

                canvas.DrawText(FormatLabel(val, Chart), xLine - 5, screenCoord + yOffset, SKTextAlign.Right, _textFont, _textPaint);
            }
        }

        if (!string.IsNullOrEmpty(Title)) {
            canvas.Save();
            canvas.RotateDegrees(-90, xLine - 45, plotArea.Top + (plotArea.Height / 2));
            canvas.DrawText(Title, xLine - 45, plotArea.Top + (plotArea.Height / 2), SKTextAlign.Center, _titleFont, _titlePaint);
            canvas.Restore();
        }
    }

    internal virtual string FormatLabel<TData>(double value, NTChart<TData> chart) where TData : class {
        return chart.GetYLabel(value);
    }
}
