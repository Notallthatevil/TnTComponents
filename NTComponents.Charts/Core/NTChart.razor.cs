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

    [Parameter]
    public bool EnableHardwareAcceleration { get; set; } = true;

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
    public ChartMargin Margin { get; set; } = ChartMargin.All(10);

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
        TnTColor.PrimaryFixed,
        TnTColor.SecondaryFixed,
        TnTColor.TertiaryFixed,
        TnTColor.Primary,
        TnTColor.Secondary,
        TnTColor.Tertiary
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
    public float LegendItemSpacing { get; set; } = 15.0f;

    [Inject]
    protected IJSRuntime JSRuntime { get; set; } = default!;

    protected SKPoint? LastMousePosition { get; private set; }

    protected string CanvasStyle => $"cursor: {(_isHovering ? "pointer" : "default")};";

    private bool _isHovering;

    internal NTBaseSeries<TData>? HoveredSeries { get; private set; }

    internal int? HoveredPointIndex { get; private set; }

    internal TData? HoveredDataPoint { get; private set; }

    private List<NTAxis<TData>> Axes { get; } = [];

    private List<NTBaseSeries<TData>> Series { get; } = [];

    private readonly Dictionary<TnTColor, SKColor> _resolvedColors = new();

    private List<double>? _cachedAllX;

    private IJSObjectReference? _themeListener;
    private DotNetObjectReference<NTChart<TData>>? _objRef;

    private float _density = 1.0f;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _density = await JSRuntime.InvokeAsync<float>("eval", "window.devicePixelRatio || 1");
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
        var point = new SKPoint((float)e.OffsetX * _density, (float)e.OffsetY * _density);

        if (ShowLegend && LegendPosition != LegendPosition.None)
        {
            // Always hit-test the legend on click, regardless of HoveredSeries state
            var clickedItem = GetLegendItemAtPoint(point);

            if (clickedItem != null)
            {
                if (clickedItem.Series != null)
                {
                    clickedItem.Series.ToggleLegendItem(clickedItem.Index);
                    StateHasChanged();
                }
                return;
            }
        }
    }

    private LegendItemInfo<TData>? GetLegendItemAtPoint(SKPoint point)
    {
        if (!ShowLegend || LegendPosition == LegendPosition.None) return null;

        using var font = new SKFont { Size = LegendFontSize };
        var totalArea = new SKRect(Margin.Left, Margin.Top, _lastWidth - Margin.Right, _lastHeight - Margin.Bottom);
        if (!string.IsNullOrEmpty(Title))
        {
            totalArea = new SKRect(totalArea.Left, totalArea.Top + 30, totalArea.Right, totalArea.Bottom);
        }

        var (plotArea, legendDrawArea) = MeasureLegendWithArea(totalArea);

        // Handle Horizontal (Top/Bottom)
        if (LegendPosition == LegendPosition.Top || LegendPosition == LegendPosition.Bottom)
        {
            var rows = GetLegendRows(font, legendDrawArea.Width);
            float rowHeight = LegendFontSize + 10;
            for (int r = 0; r < rows.Count; r++)
            {
                var rowItems = rows[r];
                float totalRowWidth = rowItems.Sum(i => font.MeasureText(i.Label) + LegendIconSize + 10) + (rowItems.Count - 1) * LegendItemSpacing;
                float startX = (LegendPosition == LegendPosition.Bottom)
                    ? plotArea.Left + (plotArea.Width - totalRowWidth) / 2
                    : legendDrawArea.Left + (legendDrawArea.Width - totalRowWidth) / 2;

                float y = legendDrawArea.Top + 5 + LegendFontSize + (r * rowHeight);
                float currentX = startX;

                foreach (var item in rowItems)
                {
                    var itemWidth = font.MeasureText(item.Label) + LegendIconSize + 10;
                    var itemRect = new SKRect(currentX, y - LegendFontSize, currentX + itemWidth, y + 5);
                    if (itemRect.Contains(point)) return item;
                    currentX += itemWidth + LegendItemSpacing;
                }
            }
        }
        else if (LegendPosition == LegendPosition.Left || LegendPosition == LegendPosition.Right)
        {
            float x = legendDrawArea.Left + 5;
            float currentY = legendDrawArea.Top + 20;
            var items = Series.SelectMany(s => s.GetLegendItems()).ToList();
            foreach (var item in items)
            {
                var label = item.Label;
                float itemWidth = font.MeasureText(label) + LegendIconSize + 10;
                var itemRect = new SKRect(x - 2, currentY - LegendFontSize, x + itemWidth, currentY + 5);
                if (itemRect.Contains(point)) return item;
                currentY += LegendFontSize + 10;
            }
        }
        else if (LegendPosition == LegendPosition.Floating)
        {
            float x = plotArea.Right - 100;
            float y = plotArea.Top + 20;
            var items = Series.SelectMany(s => s.GetLegendItems()).ToList();
            foreach (var item in items)
            {
                var label = item.Label;
                float itemWidth = font.MeasureText(label) + LegendIconSize + 10;
                var itemRect = new SKRect(x - 2, y - LegendFontSize, x + itemWidth, y + 5);
                if (itemRect.Contains(point)) return item;
                y += LegendFontSize + 5;
            }
        }

        return null;
    }

    [Obsolete("Use GetLegendItemAtPoint instead")]
    private NTBaseSeries<TData>? GetLegendSeriesAtPoint(SKPoint point) => GetLegendItemAtPoint(point)?.Series;

    private float _lastWidth;
    private float _lastHeight;

    protected virtual void OnMouseMove(MouseEventArgs e)
    {
        LastMousePosition = new SKPoint((float)e.OffsetX * _density, (float)e.OffsetY * _density);
        StateHasChanged();
    }

    protected virtual void OnMouseOut(MouseEventArgs e)
    {
        LastMousePosition = null;
        HoveredSeries = null;
        HoveredPointIndex = null;
        HoveredDataPoint = null;
        _isHovering = false;
        StateHasChanged();
    }

    protected void OnPaintSurface(SKPaintGLSurfaceEventArgs e)
    {
        OnPaintSurface(e.Surface.Canvas, e.Info);
    }

    protected void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        OnPaintSurface(e.Surface.Canvas, e.Info);
    }

    /// <summary>
    ///     Handles the paint surface event from the SkiaSharp view.
    /// </summary>
    /// <param name="e">The paint surface event arguments.</param>
    protected void OnPaintSurface(SKCanvas canvas, SKImageInfo info)
    {
        _lastWidth = info.Width;
        _lastHeight = info.Height;

        // Reset hover state and cache for this frame calculation
        HoveredSeries = null;
        HoveredPointIndex = null;
        HoveredDataPoint = null;
        _cachedAllX = null;

        canvas.Clear(GetThemeColor(BackgroundColor));

        if (!string.IsNullOrEmpty(Title))
        {
            RenderTitle(canvas, info);
        }

        var totalArea = new SKRect(Margin.Left, Margin.Top, info.Width - Margin.Right, info.Height - Margin.Bottom);
        if (!string.IsNullOrEmpty(Title))
        {
            totalArea = new SKRect(totalArea.Left, totalArea.Top + 30, totalArea.Right, totalArea.Bottom);
        }
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

        // Pass Pass 4: Hit testing and Tooltip Prep
        if (LastMousePosition.HasValue)
        {
            var mousePoint = LastMousePosition.Value;

            // Check legend items for hover first (so they take precedence over lines)
            if (ShowLegend && LegendPosition != LegendPosition.None)
            {
                using var font = new SKFont { Size = LegendFontSize };
                using var paint = new SKPaint();

                // Handle Horizontal (Top/Bottom)
                if (LegendPosition == LegendPosition.Top || LegendPosition == LegendPosition.Bottom)
                {
                    var rows = GetLegendRows(font, legendDrawArea.Width);
                    float rowHeight = LegendFontSize + 10;
                    for (int r = 0; r < rows.Count; r++)
                    {
                        var rowItems = rows[r];
                        float totalRowWidth = rowItems.Sum(i => font.MeasureText(i.Label) + LegendIconSize + 10) + (rowItems.Count - 1) * LegendItemSpacing;
                        float startX = (LegendPosition == LegendPosition.Bottom)
                            ? plotArea.Left + (plotArea.Width - totalRowWidth) / 2
                            : legendDrawArea.Left + (legendDrawArea.Width - totalRowWidth) / 2;

                        float y = legendDrawArea.Top + 5 + LegendFontSize + (r * rowHeight);
                        float currentX = startX;

                        foreach (var item in rowItems)
                        {
                            var itemWidth = font.MeasureText(item.Label) + LegendIconSize + 10;
                            var itemRect = new SKRect(currentX, y - LegendFontSize, currentX + itemWidth, y + 5);
                            if (itemRect.Contains(mousePoint))
                            {
                                HoveredSeries = item.Series;
                                HoveredPointIndex = item.Index;
                                break;
                            }
                            currentX += itemWidth + LegendItemSpacing;
                        }
                        if (HoveredSeries != null) break;
                    }
                }
                // Handle Vertical (Left/Right)
                else if (LegendPosition == LegendPosition.Left || LegendPosition == LegendPosition.Right)
                {
                    float x = legendDrawArea.Left + 5;
                    float currentY = legendDrawArea.Top + 20;
                    var items = Series.SelectMany(s => s.GetLegendItems()).ToList();
                    foreach (var item in items)
                    {
                        var label = item.Label;
                        float itemWidth = font.MeasureText(label) + LegendIconSize + 10;
                        var itemRect = new SKRect(x - 2, currentY - LegendFontSize, x + itemWidth, currentY + 5);
                        if (itemRect.Contains(mousePoint))
                        {
                            HoveredSeries = item.Series;
                            HoveredPointIndex = item.Index;
                            break;
                        }
                        currentY += LegendFontSize + 10;
                    }
                }
                // Handle Floating
                else if (LegendPosition == LegendPosition.Floating)
                {
                    float x = plotArea.Right - 100;
                    float y = plotArea.Top + 20;
                    var items = Series.SelectMany(s => s.GetLegendItems()).ToList();
                    foreach (var item in items)
                    {
                        var label = item.Label;
                        float itemWidth = font.MeasureText(label) + LegendIconSize + 10;
                        var itemRect = new SKRect(x - 2, y - LegendFontSize, x + itemWidth, y + 5);
                        if (itemRect.Contains(mousePoint))
                        {
                            HoveredSeries = item.Series;
                            HoveredPointIndex = item.Index;
                            break;
                        }
                        y += LegendFontSize + 5;
                    }
                }
            }

            // Check series/points if legend wasn't hit
            if (HoveredSeries == null && plotArea.Contains(mousePoint))
            {
                foreach (var series in Series.AsEnumerable().Reverse().Where(s => s.Visible))
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

        // Pass 5: Render legend (Now after hit testing so it can react to hovered series)
        if (ShowLegend && LegendPosition != LegendPosition.None)
        {
            if (LegendPosition == LegendPosition.Floating)
                RenderLegend(canvas, plotArea, totalArea);
            else
                RenderLegend(canvas, plotArea, legendDrawArea);
        }

        canvas.Save();
        canvas.ClipRect(plotArea);
        // Render inactive series first
        foreach (var series in Series.Where(s => s != HoveredSeries && s.IsEffectivelyVisible))
        {
            series.Render(canvas, plotArea);
        }
        // Render active series last (on top)
        if (HoveredSeries != null && HoveredSeries.IsEffectivelyVisible)
        {
            HoveredSeries.Render(canvas, plotArea);
        }
        canvas.Restore();

        // Pass 6: Render Tooltip
        if (HoveredDataPoint != null && LastMousePosition.HasValue && HoveredSeries != null && HoveredSeries.Visible)
        {
            RenderTooltip(canvas, plotArea);
        }

        // Pass 7: Update cursor if needed
        bool currentHover = HoveredSeries != null;
        if (_isHovering != currentHover)
        {
            _isHovering = currentHover;
            _ = InvokeAsync(StateHasChanged);
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
        else if (HoveredSeries is NTCircularSeries<TData> circular)
        {
            var value = circular.ValueSelector(HoveredDataPoint);
            labelValue = string.Format(circular.DataLabelFormat, value);
            title = circular.LabelSelector?.Invoke(HoveredDataPoint) ?? title;
        }

        var tooltipText = $"{title}: {labelValue}";

        var bgColor = GetThemeColor(HoveredSeries.TooltipBackgroundColor ?? TnTColor.SurfaceContainerLowest);
        var textColor = GetThemeColor(HoveredSeries.TooltipTextColor ?? TnTColor.OnSurface);

        using var textPaint = new SKPaint
        {
            Color = textColor,
            IsAntialias = true
        };

        using var font = new SKFont
        {
            Size = 12
        };

        var textWidth = font.MeasureText(tooltipText);
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

        canvas.DrawText(tooltipText, rect.Left + 5, rect.Bottom - 5, SKTextAlign.Left, font, textPaint);
    }

    private void RenderTitle(SKCanvas canvas, SKImageInfo info)
    {
        using var paint = new SKPaint
        {
            Color = GetThemeColor(TextColor),
            IsAntialias = true,
            Style = SKPaintStyle.Fill
        };

        using var font = new SKFont
        {
            Size = 20,
            Embolden = true
        };

        float x = Margin.Left + (info.Width - Margin.Left - Margin.Right) / 2;
        float y = Margin.Top + 20;

        canvas.DrawText(Title!, x, y, SKTextAlign.Center, font, paint);
    }

    private (SKRect PlotArea, SKRect LegendArea) MeasureLegendWithArea(SKRect currentArea)
    {
        using var font = new SKFont
        {
            Size = LegendFontSize
        };

        if (LegendPosition == LegendPosition.Top || LegendPosition == LegendPosition.Bottom)
        {
            var rows = GetLegendRows(font, currentArea.Width);
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
            var items = Series.SelectMany(s => s.GetLegendItems()).ToList();
            foreach (var item in items)
            {
                var label = item.Label;
                legendWidth = Math.Max(legendWidth, font.MeasureText(label) + LegendIconSize + 15);
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

    private List<List<LegendItemInfo<TData>>> GetLegendRows(SKFont font, float maxWidth)
    {
        var rows = new List<List<LegendItemInfo<TData>>>();
        var currentRow = new List<LegendItemInfo<TData>>();
        float currentRowWidth = 0;

        var items = Series.SelectMany(s => s.GetLegendItems()).ToList();

        foreach (var item in items)
        {
            float itemWidth = font.MeasureText(item.Label) + LegendIconSize + 10;
            if (currentRow.Any() && currentRowWidth + LegendItemSpacing + itemWidth > maxWidth)
            {
                rows.Add(currentRow);
                currentRow = new List<LegendItemInfo<TData>>();
                currentRowWidth = 0;
            }

            if (currentRow.Any()) currentRowWidth += LegendItemSpacing;
            currentRow.Add(item);
            currentRowWidth += itemWidth;
        }

        if (currentRow.Any()) rows.Add(currentRow);
        return rows;
    }

    private void RenderLegend(SKCanvas canvas, SKRect plotArea, SKRect targetArea)
    {
        using var font = new SKFont
        {
            Size = LegendFontSize
        };

        if (LegendPosition == LegendPosition.Top || LegendPosition == LegendPosition.Bottom)
        {
            var rows = GetLegendRows(font, targetArea.Width);
            float rowHeight = LegendFontSize + 10;

            for (int r = 0; r < rows.Count; r++)
            {
                var rowItems = rows[r];
                float totalRowWidth = rowItems.Sum(i => font.MeasureText(i.Label) + LegendIconSize + 10) + (rowItems.Count - 1) * LegendItemSpacing;

                float startX = (LegendPosition == LegendPosition.Bottom)
                    ? plotArea.Left + (plotArea.Width - totalRowWidth) / 2
                    : targetArea.Left + (targetArea.Width - totalRowWidth) / 2;

                float y = targetArea.Top + 5 + LegendFontSize + (r * rowHeight);

                float currentX = startX;
                foreach (var item in rowItems)
                {
                    var itemWidth = font.MeasureText(item.Label) + LegendIconSize + 10;
                    var itemRect = new SKRect(currentX, y - LegendFontSize, currentX + itemWidth, y + 5);

                    var hasHover = HoveredSeries != null;
                    var isItemHovered = (item.Index.HasValue) 
                        ? (HoveredSeries == item.Series && HoveredPointIndex == item.Index.Value)
                        : (HoveredSeries == item.Series);

                    var iconColor = item.Color;
                    var currentTextColor = GetThemeColor(TextColor);

                    if (hasHover && !isItemHovered)
                    {
                        iconColor = iconColor.WithAlpha((byte)(iconColor.Alpha * 0.15f));
                        currentTextColor = currentTextColor.WithAlpha((byte)(currentTextColor.Alpha * 0.15f));
                    }

                    if (!item.IsVisible)
                    {
                        iconColor = iconColor.WithAlpha((byte)(iconColor.Alpha * 0.3f));
                        currentTextColor = currentTextColor.WithAlpha((byte)(currentTextColor.Alpha * 0.3f));
                    }

                    using var iconPaint = new SKPaint { Color = iconColor, Style = SKPaintStyle.Fill, IsAntialias = true };
                    using var currentTextPaint = new SKPaint { Color = currentTextColor, IsAntialias = true };

                    // Highlight if hovered
                    if (isItemHovered)
                    {
                        using var highlightPaint = new SKPaint { Color = item.Color.WithAlpha(40), Style = SKPaintStyle.Fill, IsAntialias = true };
                        canvas.DrawRoundRect(itemRect, 4, 4, highlightPaint);
                    }

                    canvas.DrawRect(currentX, y - LegendIconSize + 2, LegendIconSize, LegendIconSize, iconPaint);
                    canvas.DrawText(item.Label, currentX + LegendIconSize + 5, y, SKTextAlign.Left, font, currentTextPaint);
                    currentX += itemWidth + LegendItemSpacing;
                }
            }
        }
        else if (LegendPosition == LegendPosition.Left || LegendPosition == LegendPosition.Right)
        {
            float x = targetArea.Left + 5;
            float currentY = targetArea.Top + 20;

            var items = Series.SelectMany(s => s.GetLegendItems()).ToList();
            foreach (var item in items)
            {
                var label = item.Label;
                var color = item.Color;

                var itemWidth = font.MeasureText(label) + LegendIconSize + 10;
                var itemRect = new SKRect(x - 2, currentY - LegendFontSize, x + itemWidth, currentY + 5);

                var hasHover = HoveredSeries != null;
                var isItemHovered = (item.Index.HasValue) 
                    ? (HoveredSeries == item.Series && HoveredPointIndex == item.Index.Value)
                    : (HoveredSeries == item.Series);
                
                var iconColor = color;
                var currentTextColor = GetThemeColor(TextColor);

                if (hasHover && !isItemHovered)
                {
                    iconColor = iconColor.WithAlpha((byte)(iconColor.Alpha * 0.15f));
                    currentTextColor = currentTextColor.WithAlpha((byte)(currentTextColor.Alpha * 0.15f));
                }

                if (!item.IsVisible)
                {
                    iconColor = iconColor.WithAlpha((byte)(iconColor.Alpha * 0.3f));
                    currentTextColor = currentTextColor.WithAlpha((byte)(currentTextColor.Alpha * 0.3f));
                }

                // Highlight if hovered
                if (isItemHovered)
                {
                    using var highlightPaint = new SKPaint { Color = color.WithAlpha(40), Style = SKPaintStyle.Fill, IsAntialias = true };
                    canvas.DrawRoundRect(itemRect, 4, 4, highlightPaint);
                }

                using var iconPaint = new SKPaint { Color = iconColor, Style = SKPaintStyle.Fill, IsAntialias = true };
                using var currentTextPaint = new SKPaint { Color = currentTextColor, IsAntialias = true };
                canvas.DrawRect(x, currentY - LegendIconSize + 2, LegendIconSize, LegendIconSize, iconPaint);
                canvas.DrawText(label, x + LegendIconSize + 5, currentY, SKTextAlign.Left, font, currentTextPaint);
                currentY += LegendFontSize + 10;
            }
        }
        else if (LegendPosition == LegendPosition.Floating)
        {
            float x = plotArea.Right - 100;
            float y = plotArea.Top + 20;

            var items = Series.SelectMany(s => s.GetLegendItems()).ToList();
            // Draw a small background for floating
            float maxWidth = items.Any() ? items.Max(s => font.MeasureText(s.Label)) + LegendIconSize + 20 : 100;
            float totalHeight = items.Count * (LegendFontSize + 5) + 10;

            using var bgPaint = new SKPaint
            {
                Color = GetThemeColor(BackgroundColor).WithAlpha(200),
                Style = SKPaintStyle.Fill,
                IsAntialias = true
            };
            canvas.DrawRect(x - 5, y - LegendFontSize, maxWidth, totalHeight, bgPaint);

            foreach (var item in items)
            {
                var label = item.Label;
                var color = item.Color;

                var itemWidth = font.MeasureText(label) + LegendIconSize + 10;
                var itemRect = new SKRect(x - 2, y - LegendFontSize, x + itemWidth, y + 5);

                var hasHover = HoveredSeries != null;
                var isItemHovered = (item.Index.HasValue) 
                    ? (HoveredSeries == item.Series && HoveredPointIndex == item.Index.Value)
                    : (HoveredSeries == item.Series);

                var iconColor = color;
                var currentTextColor = GetThemeColor(TextColor);

                if (hasHover && !isItemHovered)
                {
                    iconColor = iconColor.WithAlpha((byte)(iconColor.Alpha * 0.15f));
                    currentTextColor = currentTextColor.WithAlpha((byte)(currentTextColor.Alpha * 0.15f));
                }

                if (!item.IsVisible)
                {
                    iconColor = iconColor.WithAlpha((byte)(iconColor.Alpha * 0.3f));
                    currentTextColor = currentTextColor.WithAlpha((byte)(currentTextColor.Alpha * 0.3f));
                }

                // Highlight if hovered
                if (isItemHovered)
                {
                    using var highlightPaint = new SKPaint { Color = color.WithAlpha(40), Style = SKPaintStyle.Fill, IsAntialias = true };
                    canvas.DrawRoundRect(itemRect, 4, 4, highlightPaint);
                }

                using var iconPaint = new SKPaint { Color = iconColor, Style = SKPaintStyle.Fill, IsAntialias = true };
                using var currentTextPaint = new SKPaint { Color = currentTextColor, IsAntialias = true };
                canvas.DrawRect(x, y - LegendIconSize + 2, LegendIconSize, LegendIconSize, iconPaint);
                canvas.DrawText(label, x + LegendIconSize + 5, y, SKTextAlign.Left, font, currentTextPaint);
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

    internal float GetBarSeriesTotalWeight() => Series.OfType<NTBarSeries<TData>>().Sum(s => s.VisibilityFactor);

    internal float GetBarSeriesOffsetWeight(NTBarSeries<TData> series)
    {
        float weight = 0;
        foreach (var s in Series.OfType<NTBarSeries<TData>>())
        {
            if (s == series) return weight;
            weight += s.VisibilityFactor;
        }
        return weight;
    }

    /// <summary>
    ///    Gets or sets whether to use a categorical scale for the X axis.
    ///    If true, every unique X value will be shown and spaced equally.
    /// </summary>
    [Parameter]
    public bool IsCategoricalX { get; set; }

    /// <summary>
    ///     Returns a list of all unique X values across all cartesian series, sorted.
    /// </summary>
    public List<double> GetAllXValues()
    {
        if (_cachedAllX != null) return _cachedAllX;

        var cartesianSeries = Series.OfType<NTCartesianSeries<TData>>().Where(s => s.IsEffectivelyVisible).ToList();
        var allX = new HashSet<double>();
        foreach (var s in cartesianSeries)
        {
            if (s.Data == null) continue;
            foreach (var item in s.Data)
            {
                allX.Add(s.XValueSelector(item));
            }
        }
        _cachedAllX = allX.OrderBy(x => x).ToList();
        return _cachedAllX;
    }

    internal double GetScaledXValue(double originalX)
    {
        if (IsCategoricalX)
        {
            var allX = GetAllXValues();
            int index = allX.IndexOf(originalX);
            return index >= 0 ? index : originalX;
        }
        return originalX;
    }

    public float ScaleX(double x, SKRect plotArea)
    {
        var (min, max) = GetXRange(true);
        var range = max - min;
        if (range <= 0) return plotArea.Left;
        return (float)(plotArea.Left + (x - min) / range * plotArea.Width);
    }

    public float ScaleY(double y, SKRect plotArea)
    {
        var (min, max) = GetYRange(true);
        var range = max - min;
        if (range <= 0) return plotArea.Bottom;
        return (float)(plotArea.Bottom - (y - min) / range * plotArea.Height);
    }

    public (double Min, double Max) GetXRange(bool padded = false)
    {
        if (IsCategoricalX)
        {
            var allX = GetAllXValues();
            if (!allX.Any()) return (0, 1);
            if (!padded) return (0, Math.Max(1, allX.Count - 1));

            var catRange = Math.Max(1, allX.Count - 1);
            return (-catRange * RangePadding, catRange + catRange * RangePadding);
        }

        var cartesianSeries = Series.OfType<NTCartesianSeries<TData>>().Where(s => s.IsEffectivelyVisible).ToList();
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
        var cartesianSeries = Series.OfType<NTCartesianSeries<TData>>().Where(s => s.IsEffectivelyVisible).ToList();
        if (!cartesianSeries.Any()) return (0, 1);

        double min = double.MaxValue;
        double max = double.MinValue;
        foreach (var s in cartesianSeries)
        {
            if (s.Data == null || !s.Data.Any()) continue;
            var values = s.Data.Select(item => s.YValueSelector(item) * s.VisibilityFactor).ToList();
            if (!values.Any()) continue;
            min = Math.Min(min, values.Min());
            max = Math.Max(max, values.Max());
        }

        if (min == double.MaxValue) return (0, 1);

        // Bar charts should generally start at 0
        if (Series.Any(s => s is NTBarSeries<TData> && s.IsEffectivelyVisible))
        {
            min = Math.Min(0, min);
        }

        if (!padded) return (min, max);

        var range = max - min;
        if (range == 0) range = 1;

        var minPad = min - (range * RangePadding);
        if (min >= 0 && minPad < 0) minPad = 0;

        return (minPad, max + range * RangePadding);
    }
}