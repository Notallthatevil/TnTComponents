using Microsoft.AspNetCore.Components;
using NTComponents.Charts.Core.Series;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NTComponents.Charts;

/// <summary>
///     Represents a line series in a cartesian chart.
/// </summary>
public class NTLineSeries<TData> : NTCartesianSeries<TData> where TData : class
{

    /// <summary>
    ///     Gets or sets the color of the line.
    /// </summary>
    [Parameter]
    public TnTColor Color { get; set; } = TnTColor.Primary;

    /// <summary>
    ///     Gets or sets the width of the line.
    /// </summary>
    [Parameter]
    public float StrokeWidth { get; set; } = 2.0f;

    /// <inheritdoc />
    public override void Render(SKCanvas canvas, SKRect renderArea)
    {
        if (Data == null || !Data.Any())
        {
            return;
        }

        var (xMin, xMax) = Chart.GetXRange();
        var (yMin, yMax) = Chart.GetYRange();

        var xRange = Math.Max(0.0001, xMax - xMin);
        var yRange = Math.Max(0.0001, yMax - yMin);

        using var paint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.RoyalBlue, // Default for now until ThemeHelper is ready
            StrokeWidth = StrokeWidth,
            IsAntialias = true,
            StrokeCap = SKStrokeCap.Round,
            StrokeJoin = SKStrokeJoin.Round
        };

        using var path = new SKPath();
        var dataList = Data.ToList();
        var points = new List<SKPoint>();

        for (int i = 0; i < dataList.Count; i++)
        {
            var xValue = XValueSelector(dataList[i]);
            var yValue = YValueSelector(dataList[i]);

            var x = renderArea.Left + (float)((xValue - xMin) / xRange) * renderArea.Width;
            var y = renderArea.Bottom - (float)((yValue - yMin) / yRange) * renderArea.Height;

            var point = new SKPoint(x, y);
            points.Add(point);

            if (i == 0)
            {
                path.MoveTo(point);
            }
            else
            {
                path.LineTo(point);
            }
        }

        canvas.DrawPath(path, paint);

        if (PointStyle != PointStyle.None)
        {
            foreach (var point in points)
            {
                RenderPoint(canvas, point.X, point.Y, paint.Color);
            }
        }
    }
}
