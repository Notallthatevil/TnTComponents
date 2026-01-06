using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using NTComponents.Charts.Core.Axes;
using NTComponents.Charts.Core.Series;
using NTComponents.Core;
using SkiaSharp;
using SkiaSharp.Views.Blazor;
using System.Diagnostics;

namespace NTComponents.Charts.Core;

/// <summary>
///     The base class for all charts in the NTComponents.Charts library.
/// </summary>
[CascadingTypeParameter(nameof(TData))]
public partial class NTChart<TData> : TnTComponentBase, IAsyncDisposable where TData : class
{

    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.Surface;

    /// <summary>
    ///     Gets or sets the default text color for the chart (titles, labels).
    /// </summary>
    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnSurface;

    /// <summary>
    ///     Gets or sets the child content.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create("nt-chart")
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    /// <inheritdoc />
    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    /// <summary>
    ///     Gets or sets the margin around the chart.
    /// </summary>
    [Parameter]
    public ChartMargin Margin { get; set; } = ChartMargin.All(20);

    /// <summary>
    ///     Gets or sets the title of the chart.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    ///    Gets or sets the padding percentage for the axis ranges (0 to 1).
    /// </summary>
    [Parameter]
    public double RangePadding { get; set; } = 0.05;

    /// <summary>
    ///     Gets or sets the default color palette for the chart.
    /// </summary>
    [Parameter]
    public List<TnTColor> Palette { get; set; } = new()
    {
        TnTColor.Primary,
        TnTColor.Secondary,
        TnTColor.Tertiary,
        TnTColor.OnPrimaryContainer,
        TnTColor.OnSecondaryContainer,
        TnTColor.OnTertiaryContainer
    };

    /// <summary>
    ///     Gets or sets whether to show the legend.
    /// </summary>
    [Parameter]
    public bool ShowLegend { get; set; } = true;

    /// <summary>
    ///    Gets or sets the position of the legend.
    /// </summary>
    [Parameter]
    public LegendPosition LegendPosition { get; set; } = LegendPosition.Bottom;

    /// <summary>
    ///     Gets or sets the font size for the legend text.
    /// </summary>
    [Parameter]
    public float LegendFontSize { get; set; } = 12.0f;

    /// <summary>
    ///     Gets or sets the size of the legend icon (square).
    /// </summary>
    [Parameter]
    public float LegendIconSize { get; set; } = 12.0f;

    /// <summary>
    ///     Gets or sets the spacing between legend items.
    /// </summary>
    [Parameter]
    public float LegendItemSpacing { get; set; } = 20.0f;

    [Inject]
    protected IJSRuntime JSRuntime { get; set; } = default!;

    protected SKPoint? LastMousePosition { get; private set; }

    internal NTBaseSeries<TData>? HoveredSeries { get; private set; }

    internal int? HoveredPointIndex { get; private set; }

    internal TData? HoveredDataPoint { get; private set; }

    private List<NTAxis<TData>> Axes { get; } = [];

    private List<NTBaseSeries<TData>> Series { get; } = [];

    private readonly Dictionary<TnTColor, SKColor> _resolvedColors = new();

    private IJSObjectReference? _themeListener;
    private DotNetObjectReference<NTChart<TData>>? _objRef;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _objRef = DotNetObjectReference.Create(this);
            _themeListener = await JSRuntime.InvokeAsync<IJSObjectReference>("NTComponents.onThemeChanged", _objRef);
            await ResolveColorsAsync();
            StateHasChanged();
        }
    }

    [JSInvokable]
    public async Task OnThemeChanged()
    {
        await ResolveColorsAsync();
        StateHasChanged();
    }

    private async Task ResolveColorsAsync()
    {
        var colorsToResolve = Enum.GetValues<TnTColor>().ToList();
        foreach (var color in colorsToResolve)
        {
            if (color == TnTColor.None || color == TnTColor.Transparent)
            {
                _resolvedColors[color] = SKColors.Transparent;
                continue;
            }
            if (color == TnTColor.Black)
            {
                _resolvedColors[color] = SKColors.Black;
                continue;
            }
            if (color == TnTColor.White)
            {
                _resolvedColors[color] = SKColors.White;
                continue;
            }

            var hex = await JSRuntime.InvokeAsync<string>("NTComponents.getColorValueFromEnumName", color.ToString());
            if (!string.IsNullOrEmpty(hex) && SKColor.TryParse(hex, out var skColor))
            {
                _resolvedColors[color] = skColor;
            }
            else
            {
                // Fallback for primary/secondary etc if not found in CSS
                _resolvedColors[color] = color switch
                {
                    TnTColor.Primary => SKColors.RoyalBlue,
                    TnTColor.Secondary => SKColors.Gray,
                    _ => SKColors.Gray
                };
            }
        }
    }

    internal SKColor GetSeriesColor(NTBaseSeries<TData> series)
    {
        var color = series.Color ?? TnTColor.None;
        if (color == TnTColor.None)
        {
            var index = GetSeriesIndex(series);
            if (index >= 0)
            {
                color = Palette[index % Palette.Count];
            }
        }

        return _resolvedColors.TryGetValue(color, out var skColor) ? skColor : SKColors.Gray;
    }

    internal SKColor GetThemeColor(TnTColor color)
    {
        return _resolvedColors.TryGetValue(color, out var skColor) ? skColor : SKColors.Gray;
    }

    public async ValueTask DisposeAsync()
    {
        _objRef?.Dispose();
        if (_themeListener != null)
        {
            await _themeListener.InvokeVoidAsync("dispose");
            await _themeListener.DisposeAsync();
        }
    }

    protected virtual void OnClick(MouseEventArgs e)
    {
        Console.WriteLine("Clicked");
        // Handle click hit-testing in subclasses if needed
    }

    protected virtual void OnMouseMove(MouseEventArgs e)
    {
        Console.WriteLine($"Mouse Move: {e.OffsetX}, {e.OffsetY}");
    }

    protected virtual void OnMouseOut(MouseEventArgs e)
    {
        Console.WriteLine("Mouse Out");
        LastMousePosition = null;
        HoveredSeries = null;
        HoveredPointIndex = null;
        HoveredDataPoint = null;
    }

    /// <summary>
    ///     Handles the paint surface event from the SkiaSharp view.
    /// </summary>
    /// <param name="e">The paint surface event arguments.</param>
    protected virtual void OnPaintSurface(SKPaintGLSurfaceEventArgs e)
    {
        // Reset hover state for this frame calculation
        HoveredSeries = null;
        HoveredPointIndex = null;
        HoveredDataPoint = null;

        var canvas = e.Surface.Canvas;
        canvas.Clear(GetThemeColor(BackgroundColor));

        var info = e.Info;
        var totalArea = new SKRect(Margin.Left, Margin.Top, info.Width - Margin.Right, info.Height - Margin.Bottom);
        var plotArea = totalArea;
        var accessibleArea = totalArea; // Area available for axes and legend

        // Pass 1: Measure legend and update plotArea/accessibleArea
        SKRect legendDrawArea = default;
        if (ShowLegend && LegendPosition != LegendPosition.None && LegendPosition != LegendPosition.Floating)
        {
            var (newPlotArea, legendArea) = MeasureLegendWithArea(plotArea);
            plotArea = newPlotArea;
            legendDrawArea = legendArea;

            // Adjust accessibleArea for axes so they don't overlap legend
            if (LegendPosition == LegendPosition.Bottom)
                accessibleArea = new SKRect(accessibleArea.Left, accessibleArea.Top, accessibleArea.Right, legendArea.Top);
            else if (LegendPosition == LegendPosition.Top)
                accessibleArea = new SKRect(accessibleArea.Left, legendArea.Bottom, accessibleArea.Right, accessibleArea.Bottom);
            else if (LegendPosition == LegendPosition.Left)
                accessibleArea = new SKRect(legendArea.Right, accessibleArea.Top, accessibleArea.Right, accessibleArea.Bottom);
            else if (LegendPosition == LegendPosition.Right)
                accessibleArea = new SKRect(accessibleArea.Left, accessibleArea.Top, legendArea.Left, accessibleArea.Bottom);
        }

        // Pass 2: Measure axes and update plotArea
        foreach (var axis in Axes.Where(a => a.Visible))
        {
            plotArea = axis.Measure(plotArea);
        }

        // Pass 3: Render axes using the final plotArea and the adjusted accessibleArea
        foreach (var axis in Axes.Where(a => a.Visible))
        {
            axis.Render(canvas, plotArea, accessibleArea);
        }

        // Pass 4: Render legend
        if (ShowLegend && LegendPosition != LegendPosition.None)
        {
            if (LegendPosition == LegendPosition.Floating)
                RenderLegend(canvas, plotArea, totalArea);
            else
                RenderLegend(canvas, plotArea, legendDrawArea);
        }

        // Pass 5: Hit testing and Tooltip Prep
        if (LastMousePosition.HasValue)
        {
            var mousePoint = LastMousePosition.Value;
            
            // Check series/points first
            if (plotArea.Contains(mousePoint))
            {
                foreach (var series in Series.AsEnumerable().Reverse())
                {
                    var hit = series.HitTest(mousePoint, plotArea);
                    if (hit != null)
                    {
                        HoveredSeries = series;
                        HoveredPointIndex = hit.Value.Index;
                        HoveredDataPoint = hit.Value.Data;
                        break;
                    }
                }
            }
        }

        canvas.Save();
        canvas.ClipRect(plotArea);
        // Render inactive series first
        foreach (var series in Series.Where(s => s != HoveredSeries))
        {
            series.Render(canvas, plotArea);
        }
        // Render active series last (on top)
        if (HoveredSeries != null)
        {
            HoveredSeries.Render(canvas, plotArea);
        }
        canvas.Restore();

        // Pass 6: Render Tooltip
        if (HoveredDataPoint != null && LastMousePosition.HasValue)
        {
            RenderTooltip(canvas, plotArea);
        }
    }

    private void RenderTooltip(SKCanvas canvas, SKRect plotArea)
    {
        if (HoveredSeries == null || HoveredDataPoint == null || !LastMousePosition.HasValue) return;

        var mousePoint = LastMousePosition.Value;
        var title = HoveredSeries.Title ?? "Series";
        
        // Use reflection or cast to get Values if it's Cartesian
        string labelValue = "";
        if (HoveredSeries is NTCartesianSeries<TData> cartesian)
        {
            var yValue = cartesian.YValueSelector(HoveredDataPoint);
            labelValue = string.Format(cartesian.DataLabelFormat, yValue);
        }

        var tooltipText = $"{title}: {labelValue}";

        var bgColor = GetThemeColor(HoveredSeries.TooltipBackgroundColor ?? TnTColor.SurfaceContainerLowest);
        var textColor = GetThemeColor(HoveredSeries.TooltipTextColor ?? TnTColor.OnSurface);

        using var textPaint = new SKPaint
        {
            Color = textColor,
            TextSize = 12,
            IsAntialias = true
        };

        var textWidth = textPaint.MeasureText(tooltipText);
        var rect = new SKRect(mousePoint.X + 10, mousePoint.Y - 25, mousePoint.X + 20 + textWidth, mousePoint.Y - 5);

        // Keep tooltip within canvas
        if (rect.Right > plotArea.Right + Margin.Right)
        {
            rect.Offset(-(rect.Width + 20), 0);
        }

        using var bgPaint = new SKPaint
        {
            Color = bgColor.WithAlpha(230), // Slightly more opaque since it's "lowest"
            Style = SKPaintStyle.Fill
        };
        canvas.DrawRoundRect(rect, 4, 4, bgPaint);
        
        // Draw border
        using var borderPaint = new SKPaint
        {
            Color = GetThemeColor(TnTColor.OutlineVariant),
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 1,
            IsAntialias = true
        };
        canvas.DrawRoundRect(rect, 4, 4, borderPaint);

        canvas.DrawText(tooltipText, rect.Left + 5, rect.Bottom - 5, textPaint);
    }

    private (SKRect PlotArea, SKRect LegendArea) MeasureLegendWithArea(SKRect currentArea)
    {
        using var paint = new SKPaint
        {
            TextSize = LegendFontSize,
            IsAntialias = true
        };

        if (LegendPosition == LegendPosition.Top || LegendPosition == LegendPosition.Bottom)
        {
            var rows = GetLegendRows(paint, currentArea.Width);
            float legendHeight = rows.Count * (LegendFontSize + 10);

            if (LegendPosition == LegendPosition.Top)
            {
                var legendArea = new SKRect(currentArea.Left, currentArea.Top, currentArea.Right, currentArea.Top + legendHeight);
                var plotArea = new SKRect(currentArea.Left, currentArea.Top + legendHeight, currentArea.Right, currentArea.Bottom);
                return (plotArea, legendArea);
            }
            else
            {
                var legendArea = new SKRect(currentArea.Left, currentArea.Bottom - legendHeight, currentArea.Right, currentArea.Bottom);
                var plotArea = new SKRect(currentArea.Left, currentArea.Top, currentArea.Right, currentArea.Bottom - legendHeight);
                return (plotArea, legendArea);
            }
        }
        else if (LegendPosition == LegendPosition.Left || LegendPosition == LegendPosition.Right)
        {
            float legendWidth = 0;
            // Estimate vertical legend width
            foreach (var series in Series)
            {
                var label = series.Title ?? $"Series {GetSeriesIndex(series) + 1}";
                legendWidth = Math.Max(legendWidth, paint.MeasureText(label) + LegendIconSize + 15);
            }
            legendWidth += 10; // padding

            if (LegendPosition == LegendPosition.Left)
            {
                var legendArea = new SKRect(currentArea.Left, currentArea.Top, currentArea.Left + legendWidth, currentArea.Bottom);
                var plotArea = new SKRect(currentArea.Left + legendWidth, currentArea.Top, currentArea.Right, currentArea.Bottom);
                return (plotArea, legendArea);
            }
            else
            {
                var legendArea = new SKRect(currentArea.Right - legendWidth, currentArea.Top, currentArea.Right, currentArea.Bottom);
                var plotArea = new SKRect(currentArea.Left, currentArea.Top, currentArea.Right - legendWidth, currentArea.Bottom);
                return (plotArea, legendArea);
            }
        }

        return (currentArea, SKRect.Empty);
    }

    private List<List<LegendItemInfo>> GetLegendRows(SKPaint paint, float maxWidth)
    {
        var rows = new List<List<LegendItemInfo>>();
        var currentRow = new List<LegendItemInfo>();
        float currentRowWidth = 0;

        var items = Series.Select(s => new LegendItemInfo
        {
            Label = s.Title ?? $"Series {Series.IndexOf(s) + 1}",
            Color = GetSeriesColor(s),
            Series = s
        }).ToList();

        foreach (var item in items)
        {
            float itemWidth = paint.MeasureText(item.Label) + LegendIconSize + 10;
            if (currentRow.Any() && currentRowWidth + LegendItemSpacing + itemWidth > maxWidth)
            {
                rows.Add(currentRow);
                currentRow = new List<LegendItemInfo>();
                currentRowWidth = 0;
            }

            if (currentRow.Any()) currentRowWidth += LegendItemSpacing;
            currentRow.Add(item);
            currentRowWidth += itemWidth;
        }

        if (currentRow.Any()) rows.Add(currentRow);
        return rows;
    }

    private class LegendItemInfo
    {
        public string Label { get; set; } = string.Empty;
        public SKColor Color { get; set; }
        public NTBaseSeries<TData>? Series { get; set; }
    }

    [Obsolete("Use MeasureLegendWithArea instead")]
    private SKRect MeasureLegend(SKRect currentArea)
    {
        return MeasureLegendWithArea(currentArea).PlotArea;
    }

    private void RenderLegend(SKCanvas canvas, SKRect plotArea, SKRect targetArea)
    {
        using var textPaint = new SKPaint
        {
            Color = GetThemeColor(TextColor),
            TextSize = LegendFontSize,
            IsAntialias = true
        };

        if (LegendPosition == LegendPosition.Top || LegendPosition == LegendPosition.Bottom)
        {
            var rows = GetLegendRows(textPaint, targetArea.Width);
            float rowHeight = LegendFontSize + 10;

            for (int r = 0; r < rows.Count; r++)
            {
                var rowItems = rows[r];
                float totalRowWidth = rowItems.Sum(i => textPaint.MeasureText(i.Label) + LegendIconSize + 10) + (rowItems.Count - 1) * LegendItemSpacing;

                float startX;
                if (LegendPosition == LegendPosition.Bottom)
                {
                    startX = plotArea.Left + (plotArea.Width - totalRowWidth) / 2;
                }
                else
                {
                    startX = targetArea.Left + (targetArea.Width - totalRowWidth) / 2;
                }

                float y = targetArea.Top + 5 + LegendFontSize + (r * rowHeight);
                // If bottom, we need to adjust y to start from bottom up or just stay within targetArea
                if (LegendPosition == LegendPosition.Bottom)
                {
                    y = targetArea.Top + 5 + LegendFontSize + (r * rowHeight);
                }

                float currentX = startX;
                foreach (var item in rowItems)
                {
                    var itemWidth = textPaint.MeasureText(item.Label) + LegendIconSize + 10;
                    var itemRect = new SKRect(currentX, y - LegendFontSize, currentX + itemWidth, y + 5);
                    
                    if (LastMousePosition.HasValue && itemRect.Contains(LastMousePosition.Value))
                    {
                        HoveredSeries = item.Series;
                    }

                    using var iconPaint = new SKPaint { Color = item.Color, Style = SKPaintStyle.Fill };
                    
                    // Highlight if hovered
                    if (HoveredSeries == item.Series)
                    {
                        using var highlightPaint = new SKPaint { Color = item.Color.WithAlpha(40), Style = SKPaintStyle.Fill };
                        canvas.DrawRoundRect(itemRect, 4, 4, highlightPaint);
                    }

                    canvas.DrawRect(currentX, y - LegendIconSize + 2, LegendIconSize, LegendIconSize, iconPaint);
                    canvas.DrawText(item.Label, currentX + LegendIconSize + 5, y, textPaint);
                    currentX += itemWidth + LegendItemSpacing;
                }
            }
        }
        else if (LegendPosition == LegendPosition.Left || LegendPosition == LegendPosition.Right)
        {
            float x = targetArea.Left + 5;
            float currentY = targetArea.Top + 20;

            foreach (var series in Series)
            {
                var label = series.Title ?? $"Series {Series.IndexOf(series) + 1}";
                var color = GetSeriesColor(series);
                
                var itemWidth = textPaint.MeasureText(label) + LegendIconSize + 10;
                var itemRect = new SKRect(x - 2, currentY - LegendFontSize, x + itemWidth, currentY + 5);

                if (LastMousePosition.HasValue && itemRect.Contains(LastMousePosition.Value))
                {
                    HoveredSeries = series;
                }

                // Highlight if hovered
                if (HoveredSeries == series)
                {
                    using var highlightPaint = new SKPaint { Color = color.WithAlpha(40), Style = SKPaintStyle.Fill };
                    canvas.DrawRoundRect(itemRect, 4, 4, highlightPaint);
                }

                using var iconPaint = new SKPaint { Color = color, Style = SKPaintStyle.Fill };
                canvas.DrawRect(x, currentY - LegendIconSize + 2, LegendIconSize, LegendIconSize, iconPaint);
                canvas.DrawText(label, x + LegendIconSize + 5, currentY, textPaint);
                currentY += LegendFontSize + 10;
            }
        }
        else if (LegendPosition == LegendPosition.Floating)
        {
            float x = plotArea.Right - 100;
            float y = plotArea.Top + 20;

            // Draw a small background for floating
            float maxWidth = Series.Any() ? Series.Max(s => textPaint.MeasureText(s.Title ?? "Series")) + LegendIconSize + 20 : 100;
            float totalHeight = Series.Count * (LegendFontSize + 5) + 10;

            using var bgPaint = new SKPaint
            {
                Color = GetThemeColor(BackgroundColor).WithAlpha(200),
                Style = SKPaintStyle.Fill
            };
            canvas.DrawRect(x - 5, y - LegendFontSize, maxWidth, totalHeight, bgPaint);

            foreach (var series in Series)
            {
                var label = series.Title ?? $"Series {Series.IndexOf(series) + 1}";
                var color = GetSeriesColor(series);

                var itemWidth = textPaint.MeasureText(label) + LegendIconSize + 10;
                var itemRect = new SKRect(x - 2, y - LegendFontSize, x + itemWidth, y + 5);

                if (LastMousePosition.HasValue && itemRect.Contains(LastMousePosition.Value))
                {
                    HoveredSeries = series;
                }

                // Highlight if hovered
                if (HoveredSeries == series)
                {
                    using var highlightPaint = new SKPaint { Color = color.WithAlpha(40), Style = SKPaintStyle.Fill };
                    canvas.DrawRoundRect(itemRect, 4, 4, highlightPaint);
                }

                using var iconPaint = new SKPaint { Color = color, Style = SKPaintStyle.Fill };
                canvas.DrawRect(x, y - LegendIconSize + 2, LegendIconSize, LegendIconSize, iconPaint);
                canvas.DrawText(label, x + LegendIconSize + 5, y, textPaint);
                y += LegendFontSize + 5;
            }
        }
    }

    internal void AddAxis(NTAxis<TData> axis)
    {
        if (!Axes.Contains(axis))
        {
            Axes.Add(axis);
        }
    }

    internal void AddSeries(NTBaseSeries<TData> series)
    {
        if (Series.Count > 0 && Series[0].CoordinateSystem != series.CoordinateSystem)
        {
            throw new InvalidOperationException($"Cannot combine series with different coordinate systems. Currently using {Series[0].CoordinateSystem}, but tried to add {series.CoordinateSystem}.");
        }

        if (!Series.Contains(series))
        {
            Series.Add(series);
        }
    }

    internal void RemoveAxis(NTAxis<TData> axis)
    {
        if (Axes.Contains(axis))
        {
            Axes.Remove(axis);
        }
    }

    internal void RemoveSeries(NTBaseSeries<TData> series)
    {
        if (Series.Contains(series))
        {
            Series.Remove(series);
        }
    }

    internal int GetSeriesIndex(NTBaseSeries<TData> series) => Series.IndexOf(series);

    public (double Min, double Max) GetXRange(bool padded = false)
    {
        var cartesianSeries = Series.OfType<NTCartesianSeries<TData>>().ToList();
        if (!cartesianSeries.Any()) return (0, 1);

        double min = double.MaxValue;
        double max = double.MinValue;
        foreach (var s in cartesianSeries)
        {
            if (s.Data == null || !s.Data.Any()) continue;
            var values = s.Data.Select(s.XValueSelector).ToList();
            if (!values.Any()) continue;
            min = Math.Min(min, values.Min());
            max = Math.Max(max, values.Max());
        }

        if (min == double.MaxValue) return (0, 1);
        if (!padded) return (min, max);

        var range = max - min;
        if (range == 0) range = 1;
        return (min - range * RangePadding, max + range * RangePadding);
    }

    public (double Min, double Max) GetYRange(bool padded = false)
    {
        var cartesianSeries = Series.OfType<NTCartesianSeries<TData>>().ToList();
        if (!cartesianSeries.Any()) return (0, 1);

        double min = double.MaxValue;
        double max = double.MinValue;
        foreach (var s in cartesianSeries)
        {
            if (s.Data == null || !s.Data.Any()) continue;
            var values = s.Data.Select(s.YValueSelector).ToList();
            if (!values.Any()) continue;
            min = Math.Min(min, values.Min());
            max = Math.Max(max, values.Max());
        }

        if (min == double.MaxValue) return (0, 1);
        if (!padded) return (min, max);

        var range = max - min;
        if (range == 0) range = 1;
        return (min - range * RangePadding, max + range * RangePadding);
    }
}