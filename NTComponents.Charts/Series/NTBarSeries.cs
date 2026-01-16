using Microsoft.AspNetCore.Components;
using NTComponents.Charts.Core;
using NTComponents.Charts.Core.Series;
using SkiaSharp;

namespace NTComponents.Charts;

/// <summary>
///     Represents a bar series in a cartesian chart.
/// </summary>
/// <typeparam name="TData">The type of the data.</typeparam>
public class NTBarSeries<TData> : NTCartesianSeries<TData> where TData : class {
    /// <summary>
    ///     Gets or sets the corner radius for the bars.
    /// </summary>
    [Parameter]
    public float CornerRadius { get; set; } = 0.0f;

    /// <inheritdoc />
    public override void Render(SKCanvas canvas, SKRect renderArea) {
        if (Data == null || !Data.Any()) {
            return;
        }

        var (xMin, xMax) = Chart.GetXRange(true);
        var (yMin, yMax) = Chart.GetYRange(true);
        var yBase = Math.Max(yMin, 0); // Start bars from 0 if possible

        var barRects = GetBarRects(renderArea, xMin, xMax, yMin, yMax, yBase);

        var isHovered = Chart.HoveredSeries == this;
        var color = Chart.GetSeriesColor(this);
        var myVisibilityFactor = VisibilityFactor;
        var hoverFactor = HoverFactor;

        color = color.WithAlpha((byte)(color.Alpha * hoverFactor * myVisibilityFactor));

        using var paint = new SKPaint {
            Color = color,
            IsAntialias = true,
            Style = SKPaintStyle.Fill
        };

        var dataList = Data.ToList();
        for (var i = 0; i < barRects.Count; i++) {
            var item = dataList[i];
            var args = new NTDataPointRenderArgs<TData> {
                Data = item,
                Index = i,
                Color = color,
                GetThemeColor = Chart.GetThemeColor
            };
            OnDataPointRender?.Invoke(args);

            var rect = barRects[i];
            var isPointHovered = isHovered && Chart.HoveredPointIndex == i;
            var pointColor = args.Color ?? color;

            if (isPointHovered) {
                // Highlight hovered bar
                using var highlightPaint = new SKPaint {
                    Color = pointColor.WithAlpha(255),
                    IsAntialias = true,
                    Style = SKPaintStyle.Fill
                };
                DrawBar(canvas, rect, highlightPaint);
            }
            else {
                using var barPaint = new SKPaint {
                    Color = pointColor,
                    IsAntialias = true,
                    Style = SKPaintStyle.Fill
                };
                DrawBar(canvas, rect, barPaint);
            }

            if (ShowDataLabels || isPointHovered) {
                var labelColor = args.DataLabelColor;
                var labelSize = args.DataLabelSize ?? DataLabelSize;
                
                if (Chart.Orientation == NTChartOrientation.Vertical) {
                    RenderDataLabel(canvas, rect.MidX, rect.Top - 5, YValueSelector(dataList[i]), renderArea, labelColor, labelSize);
                }
                else {
                    // For horizontal bars, we want the label to the right of the bar
                    // RenderDataLabel is designed for vertical positioning, so we'll do it manually here or enhance it.
                    // Let's enhance NTCartesianSeries.RenderDataLabel to support alignment.
                    RenderDataLabel(canvas, rect.Right + 5, rect.MidY + (labelSize / 2), YValueSelector(dataList[i]), renderArea, labelColor, labelSize);
                }
            }
        }
    }

    private void DrawBar(SKCanvas canvas, SKRect rect, SKPaint paint) {
        if (CornerRadius > 0) {
            canvas.DrawRoundRect(rect, CornerRadius, CornerRadius, paint);
        }
        else {
            canvas.DrawRect(rect, paint);
        }
    }

    private List<SKRect> GetBarRects(SKRect renderArea, double xMin, double xMax, double yMin, double yMax, double yBase) {
        var dataList = Data.ToList();
        var rects = new List<SKRect>();
        var xRange = (float)Math.Max(0.0001, xMax - xMin);

        var progress = GetAnimationProgress();
        var easedProgress = BackEase(progress);

        // Multi-series info for side-by-side layout with animation weights
        var barSeriesWeightTotal = Math.Max(0.001f, Chart.GetBarSeriesTotalWeight());
        var barSeriesOffsetWeight = Chart.GetBarSeriesOffsetWeight(this);
        var myVisibilityFactor = VisibilityFactor;

        // Calculate available width for each categorical slot
        float slotWidth;
        var plotWidth = Chart.Orientation == NTChartOrientation.Vertical ? renderArea.Width : renderArea.Height;

        slotWidth = dataList.Count > 1
            ? (plotWidth / xRange)
            : plotWidth * 0.1f;

        // Group width is 80% of the slot to allow spacing between categories
        var groupWidth = slotWidth * 0.8f;

        // Each bar's width is proportional to its weight
        var barWidth = groupWidth / barSeriesWeightTotal * myVisibilityFactor;

        if (AnimationCurrentValues == null || AnimationCurrentValues.Length != dataList.Count) {
            AnimationCurrentValues = new double[dataList.Count];
        }

        for (var i = 0; i < dataList.Count; i++) {
            var originalX = XValueSelector(dataList[i]);
            var xValue = originalX;

            // We use VisibilityFactor^2 for the height to ensure it shrinks to zero 
            // even as the axis range (which uses linear VisibilityFactor) also shrinks.
            var targetYValue = YValueSelector(dataList[i]) * myVisibilityFactor * myVisibilityFactor;

            var startYValue = (AnimationStartValues != null && i < AnimationStartValues.Length)
                ? AnimationStartValues[i]
                : yBase;

            var currentYValue = startYValue + ((targetYValue - startYValue) * easedProgress);
            AnimationCurrentValues[i] = currentYValue;

            var centerPos = Chart.ScaleX(xValue, renderArea);

            // Calculate the start position for this series' bar within the group
            // The group is centered at centerPos
            var groupStart = centerPos - (groupWidth / 2);
            var barPos = groupStart + (barSeriesOffsetWeight * (groupWidth / barSeriesWeightTotal)) + (barWidth / 2);

            var topPos = Chart.ScaleY(currentYValue, renderArea);
            var bottomPos = Chart.ScaleY(yBase, renderArea);

            if (Chart.Orientation == NTChartOrientation.Vertical) {
                rects.Add(new SKRect(barPos - (barWidth / 2), Math.Min(topPos, bottomPos), barPos + (barWidth / 2), Math.Max(topPos, bottomPos)));
            }
            else {
                // Horizontal: barPos is Y coordinate, topPos/bottomPos are X coordinates
                rects.Add(new SKRect(Math.Min(topPos, bottomPos), barPos - (barWidth / 2), Math.Max(topPos, bottomPos), barPos + (barWidth / 2)));
            }
        }

        return rects;
    }

    /// <inheritdoc />
    public override (int Index, TData? Data)? HitTest(SKPoint point, SKRect renderArea) {
        if (Data == null || !Data.Any()) {
            return null;
        }

        var (xMin, xMax) = Chart.GetXRange(true);
        var (yMin, yMax) = Chart.GetYRange(true);
        var yBase = Math.Max(yMin, 0);

        var rects = GetBarRects(renderArea, xMin, xMax, yMin, yMax, yBase);
        var dataList = Data.ToList();

        for (var i = 0; i < rects.Count; i++) {
            if (rects[i].Contains(point)) {
                return (i, dataList[i]);
            }
        }

        return null;
    }
}
