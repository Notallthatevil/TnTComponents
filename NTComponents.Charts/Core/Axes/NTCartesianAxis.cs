using Microsoft.AspNetCore.Components;
using SkiaSharp;

namespace NTComponents.Charts.Core.Axes;

/// <summary>
///     An axis for cartesian charts.
/// </summary>
public class NTCartesianAxis<TData> : NTAxis<TData> where TData : class {

    private bool IsVertical => Chart.Orientation == NTChartOrientation.Vertical 
        ? Direction == AxisDirection.Y 
        : Direction == AxisDirection.X;

    /// <inheritdoc />
    public override SKRect Measure(SKRect renderArea) {
        if (!IsVertical) {
            float labelHeight = 18;
            float titleHeight = string.IsNullOrEmpty(Title) ? 0 : 20;
            var totalAxisHeight = labelHeight + titleHeight + 2;
            return new SKRect(renderArea.Left, renderArea.Top, renderArea.Right, renderArea.Bottom - totalAxisHeight);
        }
        else {
            float labelWidth = 35;
            float titleWidth = string.IsNullOrEmpty(Title) ? 0 : 25;
            var totalAxisWidth = labelWidth + titleWidth + 5;
            return new SKRect(renderArea.Left + totalAxisWidth, renderArea.Top, renderArea.Right, renderArea.Bottom);
        }
    }

    /// <inheritdoc />
    public override void Render(SKCanvas canvas, SKRect plotArea, SKRect totalArea) {
        var (xMin, xMax) = Chart.GetXRange(true);
        var (yMin, yMax) = Chart.GetYRange(true);
        var (xMinReal, xMaxReal) = Chart.GetXRange(false);
        var (yMinReal, yMaxReal) = Chart.GetYRange(false);

        using var textPaint = new SKPaint {
            Color = Chart.GetThemeColor(Chart.TextColor),
            IsAntialias = true
        };

        using var textFont = new SKFont {
            Size = 12,
            Typeface = Chart.DefaultTypeface
        };

        using var titlePaint = new SKPaint {
            Color = Chart.GetThemeColor(Chart.TextColor),
            IsAntialias = true
        };

        using var titleFont = new SKFont {
            Size = 16,
            Embolden = true,
            Typeface = Chart.DefaultTypeface
        };

        using var linePaint = new SKPaint {
            Color = Chart.GetThemeColor(TnTColor.Outline),
            StrokeWidth = 1,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true
        };

        if (!IsVertical) {
            var yLine = plotArea.Bottom;
            canvas.DrawLine(plotArea.Left, yLine, plotArea.Right, yLine, linePaint);

            // Draw labels
            var isCategorical = Direction == AxisDirection.X ? Chart.IsCategoricalX : Chart.IsCategoricalY;
            var minReal = Direction == AxisDirection.X ? xMinReal : yMinReal;
            var maxReal = Direction == AxisDirection.X ? xMaxReal : yMaxReal;

            if (ValuesToShow != null || isCategorical) {
                var allValues = ValuesToShow ?? (Direction == AxisDirection.X ? Chart.GetAllXValues() : Chart.GetAllYValues());
                if (allValues.Any()) {
                    for (var i = 0; i < allValues.Count; i++) {
                        var val = allValues[i];
                        var scaledVal = Direction == AxisDirection.X ? Chart.GetScaledXValue(val) : Chart.GetScaledYValue(val);
                        var screenCoord = Direction == AxisDirection.X ? Chart.ScaleX(scaledVal, plotArea) : Chart.ScaleY(scaledVal, plotArea);

                        // Only draw if within reasonable bounds of the plot area
                        if (screenCoord < plotArea.Left - 1 || screenCoord > plotArea.Right + 1) continue;

                        var label = val.ToString("0.#");

                        SKTextAlign textAlign = SKTextAlign.Center;
                        if (!isCategorical) {
                            if (i == 0 && allValues.Count > 1) {
                                textAlign = SKTextAlign.Left;
                            }
                            else if (i == allValues.Count - 1 && allValues.Count > 1) {
                                textAlign = SKTextAlign.Right;
                            }
                        }

                        canvas.DrawText(label, screenCoord, yLine + 14, textAlign, textFont, textPaint);
                    }
                }
            }
            else if (Chart.UseNiceNumbers) {
                var (niceMin, niceMax, spacing) = Chart.CalculateNiceScaling(minReal, maxReal);
                int totalLabels = (int)Math.Round((niceMax - niceMin) / spacing) + 1;
                for (int i = 0; i < totalLabels; i++) {
                    double val = niceMin + i * spacing;
                    var screenCoord = Direction == AxisDirection.X ? Chart.ScaleX(val, plotArea) : Chart.ScaleY(val, plotArea);

                    // Only draw if within reasonable bounds of the plot area
                    if (screenCoord < plotArea.Left - 1 || screenCoord > plotArea.Right + 1) continue;

                    SKTextAlign textAlign = SKTextAlign.Center;
                    if (i == 0) {
                        textAlign = SKTextAlign.Left;
                    }
                    else if (i == totalLabels - 1) {
                        textAlign = SKTextAlign.Right;
                    }

                    canvas.DrawText(val.ToString("0.#"), screenCoord, yLine + 14, textAlign, textFont, textPaint);
                }
            }
            else {
                var labelCount = 5;
                for (var i = 0; i < labelCount; i++) {
                    var t = i / (float)(labelCount - 1);
                    var val = minReal + (t * (maxReal - minReal));
                    var screenCoord = Direction == AxisDirection.X ? Chart.ScaleX(val, plotArea) : Chart.ScaleY(val, plotArea);

                    // Only draw if within reasonable bounds of the plot area
                    if (screenCoord < plotArea.Left - 1 || screenCoord > plotArea.Right + 1) continue;

                    SKTextAlign textAlign = SKTextAlign.Center;
                    if (i == 0) {
                        textAlign = SKTextAlign.Left;
                    }
                    else if (i == labelCount - 1) {
                        textAlign = SKTextAlign.Right;
                    }

                    canvas.DrawText(val.ToString("0.#"), screenCoord, yLine + 14, textAlign, textFont, textPaint);
                }
            }

            if (!string.IsNullOrEmpty(Title)) {
                canvas.DrawText(Title, plotArea.Left + (plotArea.Width / 2), plotArea.Bottom + 34, SKTextAlign.Center, titleFont, titlePaint);
            }
        }
        else {
            var xLine = plotArea.Left;
            canvas.DrawLine(xLine, plotArea.Top, xLine, plotArea.Bottom, linePaint);

            // Draw labels
            var isCategorical = Direction == AxisDirection.X ? Chart.IsCategoricalX : Chart.IsCategoricalY;
            var minReal = Direction == AxisDirection.X ? xMinReal : yMinReal;
            var maxReal = Direction == AxisDirection.X ? xMaxReal : yMaxReal;

            if (ValuesToShow != null || isCategorical) {
                var allValues = ValuesToShow ?? (Direction == AxisDirection.X ? Chart.GetAllXValues() : Chart.GetAllYValues());
                if (allValues.Any()) {
                    for (var i = 0; i < allValues.Count; i++) {
                        var val = allValues[i];
                        var scaledVal = Direction == AxisDirection.X ? Chart.GetScaledXValue(val) : Chart.GetScaledYValue(val);
                        var screenCoord = Direction == AxisDirection.X ? Chart.ScaleX(scaledVal, plotArea) : Chart.ScaleY(scaledVal, plotArea);

                        // Only draw if within reasonable bounds of the plot area
                        if (screenCoord < plotArea.Top - 1 || screenCoord > plotArea.Bottom + 1) continue;

                        float yOffset = 5;
                        if (i == 0 && !isCategorical) {
                            yOffset = 0;
                        }
                        else if (i == allValues.Count - 1 && !isCategorical) {
                            yOffset = 10;
                        }

                        canvas.DrawText(val.ToString("0.#"), xLine - 5, screenCoord + yOffset, SKTextAlign.Right, textFont, textPaint);
                    }
                }
            }
            else if (Chart.UseNiceNumbers) {
                var (niceMin, niceMax, spacing) = Chart.CalculateNiceScaling(minReal, maxReal);
                int totalLabels = (int)Math.Round((niceMax - niceMin) / spacing) + 1;
                for (int i = 0; i < totalLabels; i++) {
                    double val = niceMin + i * spacing;
                    var screenCoord = Direction == AxisDirection.X ? Chart.ScaleX(val, plotArea) : Chart.ScaleY(val, plotArea);

                    // Only draw if within reasonable bounds of the plot area
                    if (screenCoord < plotArea.Top - 1 || screenCoord > plotArea.Bottom + 1) continue;

                    float yOffset = 5;
                    if (i == 0) {
                        yOffset = 0;
                    }
                    else if (i == totalLabels - 1) {
                        yOffset = 10;
                    }

                    canvas.DrawText(val.ToString("0.#"), xLine - 5, screenCoord + yOffset, SKTextAlign.Right, textFont, textPaint);
                }
            }
            else {
                var labelCount = 5;
                for (var i = 0; i < labelCount; i++) {
                    var t = i / (float)(labelCount - 1);
                    var val = minReal + (t * (maxReal - minReal));
                    var screenCoord = Direction == AxisDirection.X ? Chart.ScaleX(val, plotArea) : Chart.ScaleY(val, plotArea);

                    // Only draw if within reasonable bounds of the plot area
                    if (screenCoord < plotArea.Top - 1 || screenCoord > plotArea.Bottom + 1) continue;

                    float yOffset = 5;
                    if (i == 0) {
                        yOffset = 0;
                    }
                    else if (i == labelCount - 1) {
                        yOffset = 10;
                    }

                    canvas.DrawText(val.ToString("0.#"), xLine - 5, screenCoord + yOffset, SKTextAlign.Right, textFont, textPaint);
                }
            }

            if (!string.IsNullOrEmpty(Title)) {
                canvas.Save();
                canvas.RotateDegrees(-90, xLine - 45, plotArea.Top + (plotArea.Height / 2));
                canvas.DrawText(Title, xLine - 45, plotArea.Top + (plotArea.Height / 2), SKTextAlign.Center, titleFont, titlePaint);
                canvas.Restore();
            }
        }
    }
}
