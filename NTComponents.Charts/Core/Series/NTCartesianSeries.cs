using Microsoft.AspNetCore.Components;
using SkiaSharp;

namespace NTComponents.Charts.Core.Series;

public abstract class NTCartesianSeries<TData> : NTBaseSeries<TData> where TData : class {

    /// <inheritdoc />
    public override ChartCoordinateSystem CoordinateSystem => ChartCoordinateSystem.Cartesian;

    [Parameter, EditorRequired]
    public Func<TData, double> XValueSelector { get; set; } = default!;

    [Parameter, EditorRequired]
    public Func<TData, double> YValueSelector { get; set; } = default!;

    /// <summary>
    ///    Gets or sets the style of the data points.
    /// </summary>
    [Parameter]
    public PointStyle PointStyle { get; set; } = PointStyle.Filled;

    /// <summary>
    ///     Gets or sets the shape of the data points. If null, a shape will be assigned based on the series index.
    /// </summary>
    [Parameter]
    public PointShape? PointShape { get; set; }

    /// <summary>
    ///    Gets or sets the size of the data points.
    /// </summary>
    [Parameter]
    public float PointSize { get; set; } = 8.0f;

    /// <summary>
    ///     Gets or sets whether to show data labels for each point.
    /// </summary>
    [Parameter]
    public bool ShowDataLabels { get; set; }

    /// <summary>
    ///     Gets or sets the format for the data labels.
    /// </summary>
    [Parameter]
    public string DataLabelFormat { get; set; } = "{0:0.#}";

    /// <summary>
    ///     Gets or sets the size of the data labels.
    /// </summary>
    [Parameter]
    public float DataLabelSize { get; set; } = 12.0f;

    /// <summary>
    ///     Gets or sets the color of the data labels. If null, the chart's text color will be used.
    /// </summary>
    [Parameter]
    public TnTColor? DataLabelColor { get; set; }

    /// <summary>
    ///     Gets or sets whether to show a background for data labels.
    /// </summary>
    [Parameter]
    public bool ShowDataLabelBackground { get; set; } = true;

    /// <summary>
    ///     Gets or sets the background color for data labels. If null, the chart's background color will be used.
    /// </summary>
    [Parameter]
    public TnTColor? DataLabelBackgroundColor { get; set; }

    protected double[]? AnimationStartValues { get; set; }
    protected double[]? AnimationCurrentValues { get; set; }

    /// <inheritdoc />
    protected override void OnDataChanged() {
        if (AnimationCurrentValues != null) {
            AnimationStartValues = AnimationCurrentValues;
        }
        AnimationCurrentValues = null;
        base.OnDataChanged();
    }

    protected void RenderDataLabel(SKCanvas canvas, float x, float y, double value, SKRect renderArea, SKColor? overrideColor = null, float? overrideFontSize = null) {
        if (overrideColor == null && !ShowDataLabels) {
            return;
        }

        var color = overrideColor ?? Chart.GetThemeColor(DataLabelColor ?? Chart.TextColor);
        var size = overrideFontSize ?? DataLabelSize;

        using var font = new SKFont {
            Size = size
        };

        using var paint = new SKPaint {
            Color = color,
            IsAntialias = true
        };

        var text = string.Format(DataLabelFormat, value);
        var textWidth = font.MeasureText(text);
        var textHeight = font.Size;
        
        var paddingX = 6f;
        var paddingY = 2f;
        var drawY = y - ((size / 2) + 5);

        // Check if label would be cut off at the top
        if (drawY - textHeight < renderArea.Top) {
            // Shift label below the point if it would be cut off
            drawY = y + ((size / 2) + textHeight + 5);
        }

        if (ShowDataLabelBackground) {
            var bgColor = Chart.GetThemeColor(DataLabelBackgroundColor ?? Chart.BackgroundColor).WithAlpha(230);
            using var bgPaint = new SKPaint {
                Color = bgColor,
                Style = SKPaintStyle.Fill,
                IsAntialias = true
            };

            var bgRect = new SKRect(
                x - (textWidth / 2) - paddingX,
                drawY - textHeight - paddingY,
                x + (textWidth / 2) + paddingX,
                drawY + paddingY);

            canvas.DrawRoundRect(bgRect, 4, 4, bgPaint);

            using var borderPaint = new SKPaint {
                Color = Chart.GetThemeColor(TnTColor.OutlineVariant),
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 1,
                IsAntialias = true
            };
            canvas.DrawRoundRect(bgRect, 4, 4, borderPaint);
        }

        canvas.DrawText(text, x, drawY, SKTextAlign.Center, font, paint);
    }

    protected void RenderPoint(SKCanvas canvas, float x, float y, SKColor color, float? pointSize = null, PointShape? pointShape = null, SKColor? strokeColor = null) {
        if (PointStyle == PointStyle.None) {
            return;
        }

        var size = pointSize ?? PointSize;
        var shape = pointShape ?? PointShape ?? (PointShape)(Chart.GetSeriesIndex(this) % Enum.GetValues<PointShape>().Length);

        using var paint = new SKPaint {
            Color = color,
            IsAntialias = true,
            Style = PointStyle == PointStyle.Filled ? SKPaintStyle.Fill : SKPaintStyle.Stroke,
            StrokeWidth = 2
        };

        var halfSize = size / 2;

        switch (shape) {
            case Series.PointShape.Circle:
                canvas.DrawCircle(x, y, halfSize, paint);
                break;
            case Series.PointShape.Square:
                canvas.DrawRect(x - halfSize, y - halfSize, size, size, paint);
                break;
            case Series.PointShape.Triangle:
                using (var path = new SKPath()) {
                    path.MoveTo(x, y - halfSize);
                    path.LineTo(x + halfSize, y + halfSize);
                    path.LineTo(x - halfSize, y + halfSize);
                    path.Close();
                    canvas.DrawPath(path, paint);
                }
                break;
            case Series.PointShape.Diamond:
                using (var path = new SKPath()) {
                    path.MoveTo(x, y - halfSize);
                    path.LineTo(x + halfSize, y);
                    path.LineTo(x, y + halfSize);
                    path.LineTo(x - halfSize, y);
                    path.Close();
                    canvas.DrawPath(path, paint);
                }
                break;
        }

        if (PointStyle == PointStyle.Outlined && strokeColor.HasValue)
        {
            paint.Color = strokeColor.Value;
            paint.Style = SKPaintStyle.Stroke;
            // Redraw with stroke color if explicitly requested? 
            // Actually, the base paint already did it if Style was Stroke.
        }
    }
}
