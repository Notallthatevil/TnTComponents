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

        var points = GetPoints(renderArea, xMin, xMax, yMin, yMax);

        var isHovered = Chart.HoveredSeries == this;
        var hasHover = Chart.HoveredSeries != null;
        var color = Chart.GetSeriesColor(this);

        if (hasHover && !isHovered)
        {
            color = color.WithAlpha((byte)(color.Alpha * 0.15f));
        }

        using var paint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = color,
            StrokeWidth = StrokeWidth,
            IsAntialias = true,
            StrokeCap = SKStrokeCap.Round,
            StrokeJoin = SKStrokeJoin.Round
        };

        if (LineStyle == LineStyle.Dashed)
        {
            paint.PathEffect = SKPathEffect.CreateDash([10, 5], 0);
        }

        if (LineStyle != LineStyle.None && points.Count > 1)
        {
            using var path = BuildPath(points);
            canvas.DrawPath(path, paint);
        }

        if (PointStyle != PointStyle.None || ShowDataLabels)
        {
            var dataList = Data.ToList();
            for (int i = 0; i < points.Count; i++)
            {
                var point = points[i];
                var isPointHovered = Chart.HoveredSeries == this && Chart.HoveredPointIndex == i;
                var pointColor = paint.Color;
                var pointSize = PointSize;

                if (isPointHovered)
                {
                    pointColor = pointColor.WithAlpha(255);
                    pointSize *= 1.5f;
                }

                if (PointStyle != PointStyle.None)
                {
                    RenderPoint(canvas, point.X, point.Y, pointColor);
                }

                if (ShowDataLabels || isPointHovered)
                {
                    RenderDataLabel(canvas, point.X, point.Y, YValueSelector(dataList[i]), pointColor);
                }
            }
        }
    }

    private List<SKPoint> GetPoints(SKRect renderArea, double xMin, double xMax, double yMin, double yMax)
    {
        var dataList = Data.ToList();
        var points = new List<SKPoint>();
        var progress = GetAnimationProgress();
        var easedProgress = BackEase(progress);
        var xRange = Math.Max(0.0001, xMax - xMin);
        var yRange = Math.Max(0.0001, yMax - yMin);

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
        return points;
    }

    private SKPath BuildPath(List<SKPoint> points)
    {
        var path = new SKPath();
        if (points.Count < 2) return path;

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
                path.LineTo(points[i - 1].X, points[i].Y);
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

                var cp1 = new SKPoint(p1.X + (p2.X - p0.X) / 6, p1.Y + (p2.Y - p0.Y) / 6);
                var cp2 = new SKPoint(p2.X - (p3.X - p1.X) / 6, p2.Y - (p3.Y - p1.Y) / 6);

                path.CubicTo(cp1, cp2, p2);
            }
        }
        else if (Interpolation == LineInterpolation.Smoothed)
        {
            int n = points.Count;
            if (n > 2)
            {
                float[] slopes = new float[n - 1];
                for (int i = 0; i < n - 1; i++)
                {
                    slopes[i] = (points[i + 1].Y - points[i].Y) / (points[i + 1].X - points[i].X + 1e-6f);
                }

                float[] tangents = new float[n];
                for (int i = 1; i < n - 1; i++)
                {
                    if (Math.Sign(slopes[i - 1]) != Math.Sign(slopes[i]))
                    {
                        tangents[i] = 0;
                    }
                    else
                    {
                        float dxPrev = points[i].X - points[i - 1].X;
                        float dxCurr = points[i + 1].X - points[i].X;
                        float w1 = 2 * dxCurr + dxPrev;
                        float w2 = dxCurr + 2 * dxPrev;

                        if (Math.Abs(slopes[i - 1]) > 1e-6f && Math.Abs(slopes[i]) > 1e-6f)
                        {
                            tangents[i] = (w1 + w2) / (w1 / slopes[i - 1] + w2 / slopes[i]);
                        }
                        else
                        {
                            tangents[i] = 0;
                        }
                    }
                }
                tangents[0] = slopes[0];
                tangents[n - 1] = slopes[n - 2];

                for (int i = 0; i < n - 1; i++)
                {
                    float xSpan = (points[i + 1].X - points[i].X) / 3f;
                    var cp1 = new SKPoint(points[i].X + xSpan, points[i].Y + tangents[i] * xSpan);
                    var cp2 = new SKPoint(points[i + 1].X - xSpan, points[i + 1].Y - tangents[i + 1] * xSpan);
                    path.CubicTo(cp1, cp2, points[i + 1]);
                }
            }
            else
            {
                path.LineTo(points[1]);
            }
        }

        return path;
    }

    /// <inheritdoc />
    public override (int Index, TData? Data)? HitTest(SKPoint point, SKRect renderArea)
    {
        if (Data == null || !Data.Any()) return null;

        var (xMin, xMax) = Chart.GetXRange(true);
        var (yMin, yMax) = Chart.GetYRange(true);
        var points = GetPoints(renderArea, xMin, xMax, yMin, yMax);
        var dataList = Data.ToList();

        for (int i = 0; i < points.Count; i++)
        {
            var p = points[i];
            var distance = Math.Sqrt(Math.Pow(p.X - point.X, 2) + Math.Pow(p.Y - point.Y, 2));
            if (distance < PointSize + 5)
            {
                return (i, dataList[i]);
            }
        }

        if (points.Count > 1 && LineStyle != LineStyle.None)
        {
            using var path = BuildPath(points);
            using var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = StrokeWidth + 10, // Wider tolerance for hovering
                StrokeCap = SKStrokeCap.Round,
                StrokeJoin = SKStrokeJoin.Round
            };

            using var strokePath = new SKPath();
            paint.GetFillPath(path, strokePath);

            if (strokePath.Contains(point.X, point.Y))
            {
                return (-1, null);
            }
        }

        return null;
    }

    private float DistanceToSegment(SKPoint p, SKPoint v, SKPoint w)
    {
        float l2 = (v.X - w.X) * (v.X - w.X) + (v.Y - w.Y) * (v.Y - w.Y);
        if (l2 == 0) return (float)Math.Sqrt((p.X - v.X) * (p.X - v.X) + (p.Y - v.Y) * (p.Y - v.Y));
        float t = ((p.X - v.X) * (w.X - v.X) + (p.Y - v.Y) * (w.Y - v.Y)) / l2;
        t = Math.Max(0, Math.Min(1, t));
        SKPoint projection = new SKPoint(v.X + t * (w.X - v.X), v.Y + t * (w.Y - v.Y));
        return (float)Math.Sqrt((p.X - projection.X) * (p.X - projection.X) + (p.Y - projection.Y) * (p.Y - projection.Y));
    }
}
