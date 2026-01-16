using Microsoft.AspNetCore.Components;
using NTComponents.Charts.Core.Axes;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NTComponents.Charts.Core.Series;

public abstract class NTCartesianSeries<TData> : NTBaseSeries<TData> where TData : class {

    /// <inheritdoc />
    public override ChartCoordinateSystem CoordinateSystem => ChartCoordinateSystem.Cartesian;

    [Parameter, EditorRequired]
    public Func<TData, double> XValueSelector { get; set; } = default!;

    [Parameter, EditorRequired]
    public Func<TData, double> YValueSelector { get; set; } = default!;

    /// <summary>
    ///     Gets or sets the X axis options.
    /// </summary>
    [Parameter]
    public NTXAxisOptions XAxis { get; set; } = new();

    /// <summary>
    ///    Gets or sets the Y axis options.
    /// </summary>
    [Parameter]
    public NTYAxisOptions YAxis { get; set; } = new();

    /// <inheritdoc />
    internal override List<string> GetTooltipLines(TData data) {
        var xValue = XValueSelector(data);
        var yValue = YValueSelector(data);
        return
        [
            $"{Title ?? "Series"}: {xValue:0.#}",
            string.Format(DataLabelFormat, yValue)
        ];
    }

    /// <inheritdoc />
    internal override SKRect Measure(SKRect renderArea, HashSet<object> measured) {
        if (!IsEffectivelyVisible) return renderArea;
        var rect = renderArea;
        if (XAxis != null && XAxis.Visible && measured.Add(XAxis)) {
            rect = XAxis.Measure(rect, Chart);
        }
        if (YAxis != null && YAxis.Visible && measured.Add(YAxis)) {
            rect = YAxis.Measure(rect, Chart);
        }
        return rect;
    }

    /// <inheritdoc />
    internal override void RenderAxes(SKCanvas canvas, SKRect plotArea, SKRect totalArea, HashSet<object> rendered) {
        if (!IsEffectivelyVisible) return;
        if (XAxis != null && XAxis.Visible && rendered.Add(XAxis)) {
            XAxis.Render(canvas, plotArea, totalArea, Chart);
        }
        if (YAxis != null && YAxis.Visible && rendered.Add(YAxis)) {
            YAxis.Render(canvas, plotArea, totalArea, Chart);
        }
    }

    /// <inheritdoc />
    internal override (double Min, double Max)? GetXRange() {
        if (Data == null || !Data.Any()) return null;
        var values = Data.Select(XValueSelector).ToList();
        var min = values.Min();
        var max = values.Max();
        if (XAxis != null && XAxis.Visible && XAxis.ValuesToShow != null && XAxis.ValuesToShow.Any()) {
            min = Math.Min(min, XAxis.ValuesToShow.Min());
            max = Math.Max(max, XAxis.ValuesToShow.Max());
        }
        return (min, max);
    }

    /// <inheritdoc />
    internal override (double Min, double Max)? GetYRange() {
        if (Data == null || !Data.Any()) return null;
        var min = double.MaxValue;
        var max = double.MinValue;

        if (this is NTBoxPlotSeries<TData> boxPlot) {
            foreach (var item in Data) {
                var values = boxPlot.BoxValueSelector(item);
                min = Math.Min(min, values.Min);
                max = Math.Max(max, values.Max);
                if (values.Outliers != null && values.Outliers.Any()) {
                    min = Math.Min(min, values.Outliers.Min());
                    max = Math.Max(max, values.Outliers.Max());
                }
            }
        }
        else {
            var values = Data.Select(item => YValueSelector(item) * VisibilityFactor).ToList();
            min = values.Min();
            max = values.Max();
        }

        if (YAxis != null && YAxis.Visible && YAxis.ValuesToShow != null && YAxis.ValuesToShow.Any()) {
            min = Math.Min(min, YAxis.ValuesToShow.Min());
            max = Math.Max(max, YAxis.ValuesToShow.Max());
        }

        return (min, max);
    }

    /// <inheritdoc />
    internal override void RegisterXValues(HashSet<double> values) {
        if (Data == null) return;
        foreach (var item in Data) {
            values.Add(XValueSelector(item));
        }
        if (XAxis != null && XAxis.Visible && XAxis.ValuesToShow != null) {
            foreach (var val in XAxis.ValuesToShow) values.Add(val);
        }
    }

    /// <inheritdoc />
    internal override void RegisterYValues(HashSet<double> values) {
        if (Data == null) return;
        foreach (var item in Data) {
            values.Add(YValueSelector(item));
        }
        if (YAxis != null && YAxis.Visible && YAxis.ValuesToShow != null) {
            foreach (var val in YAxis.ValuesToShow) values.Add(val);
        }
    }

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
    ///     Gets or sets the background color for data labels. If null, the series' color will be used.
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

        var color = overrideColor ?? (DataLabelColor.HasValue ? Chart.GetThemeColor(DataLabelColor.Value) : Chart.GetSeriesTextColor(this));
        var size = overrideFontSize ?? DataLabelSize;

        using var font = new SKFont {
            Size = size,
            Typeface = Chart.DefaultTypeface
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
            var bgColor = DataLabelBackgroundColor.HasValue
                ? Chart.GetThemeColor(DataLabelBackgroundColor.Value)
                : Chart.GetSeriesColor(this);

            using var bgPaint = new SKPaint {
                Color = bgColor,
                Style = SKPaintStyle.Fill,
                IsAntialias = true,
                ImageFilter = SKImageFilter.CreateDropShadow(2, 2, 4, 4, SKColors.Black.WithAlpha(80))
            };

            var bgRect = new SKRect(
                x - (textWidth / 2) - paddingX,
                drawY - textHeight - paddingY,
                x + (textWidth / 2) + paddingX + 2, // Added a little bit more padding on the right side
                drawY + paddingY);

            canvas.DrawRoundRect(bgRect, 6, 6, bgPaint);

            using var borderPaint = new SKPaint {
                Color = Chart.GetThemeColor(TnTColor.Outline),
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 1,
                IsAntialias = true
            };
            canvas.DrawRoundRect(bgRect, 6, 6, borderPaint);
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

        if (PointStyle == PointStyle.Outlined && strokeColor.HasValue) {
            paint.Color = strokeColor.Value;
            paint.Style = SKPaintStyle.Stroke;
            // Redraw with stroke color if explicitly requested? 
            // Actually, the base paint already did it if Style was Stroke.
        }
    }
}
