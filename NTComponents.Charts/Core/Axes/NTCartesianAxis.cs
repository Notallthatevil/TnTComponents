using Microsoft.AspNetCore.Components;
using SkiaSharp;

namespace NTComponents.Charts.Core.Axes;

/// <summary>
///     An axis for cartesian charts.
/// </summary>
public class NTCartesianAxis<TData> : NTAxis<TData> where TData : class {

    /// <inheritdoc />
    public override SKRect Measure(SKRect renderArea) {
        if (Direction == AxisDirection.X) {
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
            Size = 12
        };

        using var titlePaint = new SKPaint {
            Color = Chart.GetThemeColor(Chart.TextColor),
            IsAntialias = true
        };

        using var titleFont = new SKFont {
            Size = 16,
            Embolden = true
        };

        using var linePaint = new SKPaint {
            Color = Chart.GetThemeColor(TnTColor.Outline),
            StrokeWidth = 1,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true
        };

        if (Direction == AxisDirection.X) {
            var yLine = plotArea.Bottom;
            canvas.DrawLine(plotArea.Left, yLine, plotArea.Right, yLine, linePaint);

            // Draw labels
            if (ValuesToShow != null || Chart.IsCategoricalX) {
                var allX = ValuesToShow ?? Chart.GetAllXValues();
                if (allX.Any()) {
                    for (var i = 0; i < allX.Count; i++) {
                        var val = allX[i];
                        var x = Chart.ScaleX(Chart.GetScaledXValue(val), plotArea);

                        // Only draw if within reasonable bounds of the plot area
                        if (x < plotArea.Left - 1 || x > plotArea.Right + 1) continue;

                        var label = val.ToString("0.#");

                        SKTextAlign textAlign = SKTextAlign.Center;
                        if (!Chart.IsCategoricalX) {
                            if (i == 0 && allX.Count > 1) {
                                textAlign = SKTextAlign.Left;
                            }
                            else if (i == allX.Count - 1 && allX.Count > 1) {
                                textAlign = SKTextAlign.Right;
                            }
                        }

                        canvas.DrawText(label, x, yLine + 14, textAlign, textFont, textPaint);
                    }
                }
            }
            else if (Chart.UseNiceNumbers) {
                var (niceMin, niceMax, spacing) = Chart.CalculateNiceScaling(xMinReal, xMaxReal);
                int totalLabels = (int)Math.Round((niceMax - niceMin) / spacing) + 1;
                for (int i = 0; i < totalLabels; i++) {
                    double val = niceMin + i * spacing;
                    var x = Chart.ScaleX(val, plotArea);

                    // Only draw if within reasonable bounds of the plot area
                    if (x < plotArea.Left - 1 || x > plotArea.Right + 1) continue;

                    SKTextAlign textAlign = SKTextAlign.Center;
                    if (i == 0) {
                        textAlign = SKTextAlign.Left;
                    }
                    else if (i == totalLabels - 1) {
                        textAlign = SKTextAlign.Right;
                    }

                    canvas.DrawText(val.ToString("0.#"), x, yLine + 14, textAlign, textFont, textPaint);
                }
            }
            else {
                var labelCount = 5;
                for (var i = 0; i < labelCount; i++) {
                    var t = i / (float)(labelCount - 1);
                    var val = xMinReal + (t * (xMaxReal - xMinReal));
                    var x = Chart.ScaleX(val, plotArea);

                    // Only draw if within reasonable bounds of the plot area
                    if (x < plotArea.Left - 1 || x > plotArea.Right + 1) continue;

                    SKTextAlign textAlign = SKTextAlign.Center;
                    if (i == 0) {
                        textAlign = SKTextAlign.Left;
                    }
                    else if (i == labelCount - 1) {
                        textAlign = SKTextAlign.Right;
                    }

                    canvas.DrawText(val.ToString("0.#"), x, yLine + 14, textAlign, textFont, textPaint);
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
            if (ValuesToShow != null || Chart.IsCategoricalY) {
                var allY = ValuesToShow ?? Chart.GetAllYValues();
                if (allY.Any()) {
                    for (var i = 0; i < allY.Count; i++) {
                        var val = allY[i];
                        var y = Chart.ScaleY(Chart.GetScaledYValue(val), plotArea);

                        // Only draw if within reasonable bounds of the plot area
                        if (y < plotArea.Top - 1 || y > plotArea.Bottom + 1) continue;

                        float yOffset = 5;
                        if (i == 0) {
                            yOffset = 0;
                        }
                        else if (i == allY.Count - 1) {
                            yOffset = 10;
                        }

                        canvas.DrawText(val.ToString("0.#"), xLine - 5, y + yOffset, SKTextAlign.Right, textFont, textPaint);
                    }
                }
            }
            else if (Chart.UseNiceNumbers) {
                var (niceMin, niceMax, spacing) = Chart.CalculateNiceScaling(yMinReal, yMaxReal);
                int totalLabels = (int)Math.Round((niceMax - niceMin) / spacing) + 1;
                for (int i = 0; i < totalLabels; i++) {
                    double val = niceMin + i * spacing;
                    var y = Chart.ScaleY(val, plotArea);

                    // Only draw if within reasonable bounds of the plot area
                    if (y < plotArea.Top - 1 || y > plotArea.Bottom + 1) continue;

                    float yOffset = 5;
                    if (i == 0) {
                        yOffset = 0;
                    }
                    else if (i == totalLabels - 1) {
                        yOffset = 10;
                    }

                    canvas.DrawText(val.ToString("0.#"), xLine - 5, y + yOffset, SKTextAlign.Right, textFont, textPaint);
                }
            }
            else {
                var labelCount = 5;
                for (var i = 0; i < labelCount; i++) {
                    var t = i / (float)(labelCount - 1);
                    var val = yMinReal + (t * (yMaxReal - yMinReal));
                    var y = plotArea.Bottom - ((float)((val - yMin) / (yMax - yMin)) * plotArea.Height);

                    // Only draw if within reasonable bounds of the plot area
                    if (y < plotArea.Top - 1 || y > plotArea.Bottom + 1) continue;

                    float yOffset = 5;
                    if (i == 0) {
                        yOffset = 0;
                    }
                    else if (i == labelCount - 1) {
                        yOffset = 10;
                    }

                    canvas.DrawText(val.ToString("0.#"), xLine - 5, y + yOffset, SKTextAlign.Right, textFont, textPaint);
                }
            }

            if (!string.IsNullOrEmpty(Title)) {
                var xTitle = totalArea.Left + 8;
                var yTitle = plotArea.Top + (plotArea.Height / 2);
                canvas.Save();
                canvas.RotateDegrees(-90, xTitle, yTitle);
                canvas.DrawText(Title, xTitle, yTitle, SKTextAlign.Center, titleFont, titlePaint);
                canvas.Restore();
            }
        }
    }
}
