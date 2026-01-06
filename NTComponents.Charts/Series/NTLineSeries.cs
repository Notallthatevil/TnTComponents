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
    ///     Gets or sets the width of the line.
    /// </summary>
    [Parameter]
    public float StrokeWidth { get; set; } = 2.0f;

    /// <summary>
    ///    Gets or sets the style of the line.
    /// </summary>
    [Parameter]
    public LineStyle LineStyle { get; set; } = LineStyle.Solid;

    /// <summary>
    ///    Gets or sets the interpolation type for the line.
    /// </summary>
    [Parameter]
    public LineInterpolation Interpolation { get; set; } = LineInterpolation.Straight;

    /// <inheritdoc />
    public override void Render(SKCanvas canvas, SKRect renderArea)
    {
        if (Data == null || !Data.Any())
        {
            return;
        }

        var (xMin, xMax) = Chart.GetXRange(true);
        var (yMin, yMax) = Chart.GetYRange(true);

        var xRange = Math.Max(0.0001, xMax - xMin);
        var yRange = Math.Max(0.0001, yMax - yMin);

        using var paint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = Chart.GetSeriesColor(this),
            StrokeWidth = StrokeWidth,
            IsAntialias = true,
            StrokeCap = SKStrokeCap.Round,
            StrokeJoin = SKStrokeJoin.Round
        };

        if (LineStyle == LineStyle.Dashed)
        {
            paint.PathEffect = SKPathEffect.CreateDash([10, 5], 0);
        }

        using var path = new SKPath();
        var dataList = Data.ToList();
        var points = new List<SKPoint>();
        var progress = GetAnimationProgress();
        var easedProgress = BackEase(progress);

        if (AnimationCurrentValues == null || AnimationCurrentValues.Length != dataList.Count)
        {
            AnimationCurrentValues = new double[dataList.Count];
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            var xValue = XValueSelector(dataList[i]);
            var targetYValue = YValueSelector(dataList[i]);

            var startYValue = (AnimationStartValues != null && i < AnimationStartValues.Length)
                ? AnimationStartValues[i]
                : yMin;

            var currentYValue = startYValue + (targetYValue - startYValue) * easedProgress;
            AnimationCurrentValues[i] = currentYValue;

            var x = renderArea.Left + (float)((xValue - xMin) / xRange) * renderArea.Width;
            var y = renderArea.Bottom - (float)((currentYValue - yMin) / yRange) * renderArea.Height;

            points.Add(new SKPoint(x, y));
        }

        if (LineStyle != LineStyle.None && points.Count > 1)
        {
            path.MoveTo(points[0]);

            if (Interpolation == LineInterpolation.Straight)
            {
                for (int i = 1; i < points.Count; i++)
                {
                    path.LineTo(points[i]);
                }
            }
            else if (Interpolation == LineInterpolation.Step)
            {
                for (int i = 1; i < points.Count; i++)
                {
                    path.LineTo(points[i].X, points[i - 1].Y);
                    path.LineTo(points[i]);
                }
            }
            else if (Interpolation == LineInterpolation.Curved)
            {
                for (int i = 0; i < points.Count - 1; i++)
                {
                    var p0 = points[Math.Max(i - 1, 0)];
                    var p1 = points[i];
                    var p2 = points[i + 1];
                    var p3 = points[Math.Min(i + 2, points.Count - 1)];

                    // Catmull-Rom to Cubic Bezier conversion
                    var cp1 = new SKPoint(p1.X + (p2.X - p0.X) / 6, p1.Y + (p2.Y - p0.Y) / 6);
                    var cp2 = new SKPoint(p2.X - (p3.X - p1.X) / 6, p2.Y - (p3.Y - p1.Y) / 6);

                    path.CubicTo(cp1, cp2, p2);
                }
            }
            else if (Interpolation == LineInterpolation.Smoothed)
            {
                for (int i = 1; i < points.Count; i++)
                {
                    path.LineTo(points[i]);
                }
                paint.PathEffect = LineStyle == LineStyle.Dashed
                    ? SKPathEffect.CreateCompose(paint.PathEffect, SKPathEffect.CreateCorner(10))
                    : SKPathEffect.CreateCorner(10);
            }

            canvas.DrawPath(path, paint);
        }

        if (PointStyle != PointStyle.None)
        {
            foreach (var point in points)
            {
                RenderPoint(canvas, point.X, point.Y, paint.Color);
            }
        }
    }
}
