using Microsoft.AspNetCore.Components;
using NTComponents.Charts.Core;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTComponents.Charts.Core.Series;

public abstract class NTCartesianSeries<TData> : NTBaseSeries<TData> where TData : class
{

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

    protected double[]? AnimationStartValues { get; set; }
    protected double[]? AnimationCurrentValues { get; set; }

    /// <inheritdoc />
    protected override void OnDataChanged()
    {
        if (AnimationCurrentValues != null)
        {
            AnimationStartValues = AnimationCurrentValues;
        }
        AnimationCurrentValues = null;
        base.OnDataChanged();
    }

    protected void RenderDataLabel(SKCanvas canvas, float x, float y, double value)
    {
        if (!ShowDataLabels)
            return;

        using var paint = new SKPaint
        {
            Color = Chart.GetThemeColor(DataLabelColor ?? Chart.TextColor),
            IsAntialias = true,
            TextSize = DataLabelSize,
            TextAlign = SKTextAlign.Center
        };

        var text = string.Format(DataLabelFormat, value);
        canvas.DrawText(text, x, y - (PointSize / 2 + 5), paint);
    }

    protected void RenderPoint(SKCanvas canvas, float x, float y, SKColor color)
    {
        if (PointStyle == PointStyle.None)
            return;

        var shape = PointShape ?? (PointShape)(Chart.GetSeriesIndex(this) % Enum.GetValues<PointShape>().Length);

        using var paint = new SKPaint
        {
            Color = color,
            IsAntialias = true,
            Style = PointStyle == PointStyle.Filled ? SKPaintStyle.Fill : SKPaintStyle.Stroke,
            StrokeWidth = 2
        };

        var halfSize = PointSize / 2;

        switch (shape)
        {
            case Series.PointShape.Circle:
                canvas.DrawCircle(x, y, halfSize, paint);
                break;
            case Series.PointShape.Square:
                canvas.DrawRect(x - halfSize, y - halfSize, PointSize, PointSize, paint);
                break;
            case Series.PointShape.Triangle:
                using (var path = new SKPath())
                {
                    path.MoveTo(x, y - halfSize);
                    path.LineTo(x + halfSize, y + halfSize);
                    path.LineTo(x - halfSize, y + halfSize);
                    path.Close();
                    canvas.DrawPath(path, paint);
                }
                break;
            case Series.PointShape.Diamond:
                using (var path = new SKPath())
                {
                    path.MoveTo(x, y - halfSize);
                    path.LineTo(x + halfSize, y);
                    path.LineTo(x, y + halfSize);
                    path.LineTo(x - halfSize, y);
                    path.Close();
                    canvas.DrawPath(path, paint);
                }
                break;
        }
    }
}
