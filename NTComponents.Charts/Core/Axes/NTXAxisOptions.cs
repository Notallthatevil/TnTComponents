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
    internal override void Render(SKCanvas canvas, SKRect plotArea, SKRect totalArea, IEnumerable<double> tickValues) {
        var yLine = plotArea.Bottom;
        canvas.DrawLine(plotArea.Left, yLine, plotArea.Right, yLine, _linePaint);

        var values = tickValues.ToList();
        for (var i = 0; i < values.Count; i++) {
            var val = values[i];
            var screenCoord = Chart.ScaleX(val, plotArea);

            if (screenCoord < plotArea.Left - 1 || screenCoord > plotArea.Right + 1) {
                continue;
            }

            var textAlign = SKTextAlign.Center;
            if (i == 0) {
                textAlign = SKTextAlign.Left;
            }
            else if (i == values.Count - 1) {
                textAlign = SKTextAlign.Right;
            }

            canvas.DrawText(FormatLabel(val), screenCoord, yLine + 14, textAlign, _textFont, _textPaint);
        }

        if (!string.IsNullOrEmpty(Title)) {
            canvas.DrawText(Title, plotArea.Left + (plotArea.Width / 2), plotArea.Bottom + 34, SKTextAlign.Center, _titleFont, _titlePaint);
        }
    }

    internal virtual string FormatLabel(double value) {
        var format = LabelFormat ?? "0.#";
        return value.ToString(format);
    }

}

