using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using NTComponents.Charts.Core.Axes;
using NTComponents.Charts.Core.Series;
using NTComponents.Core;
using NTComponents.Ext;
using SkiaSharp;
using SkiaSharp.Views.Blazor;

namespace NTComponents.Charts.Core;

/// <summary>
///     The base class for all charts in the NTComponents.Charts library.
/// </summary>
[CascadingTypeParameter(nameof(TData))]
public partial class NTChart<TData> : TnTComponentBase, IAsyncDisposable where TData : class {

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
    ///     Gets or sets whether panning is enabled.
    /// </summary>
    [Parameter]
    public bool EnablePan { get; set; }

    /// <summary>
    ///    Gets or sets whether zooming is enabled.
    /// </summary>
    [Parameter]
    public bool EnableZoom { get; set; }

    /// <summary>
    ///    Gets or sets whether to allow exporting the chart as a PNG.
    /// </summary>
    [Parameter]
    public bool AllowExport { get; set; } = true;

    /// <summary>
    ///     Gets or sets whether to use "nice" numbers for axis scales.
    /// </summary>
    [Parameter]
    public bool UseNiceNumbers { get; set; } = true;

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
    ///     Gets or sets the orientation of the chart (Vertical or Horizontal).
    /// </summary>
    [Parameter]
    public NTChartOrientation Orientation { get; set; } = NTChartOrientation.Vertical;

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
    public List<(TnTColor Background, TnTColor Text)> Palette { get; set; } =
    [
        (TnTColor.PrimaryFixed, TnTColor.OnPrimaryFixed),
        (TnTColor.SecondaryFixed, TnTColor.OnSecondaryFixed),
        (TnTColor.TertiaryFixed, TnTColor.OnTertiaryFixed),
        (TnTColor.Primary, TnTColor.OnPrimary),
        (TnTColor.Secondary, TnTColor.OnSecondary),
        (TnTColor.Tertiary, TnTColor.OnTertiary)
    ];

    private static readonly SKTypeface _defaultTypeface = SKTypeface.FromFamilyName("Roboto", SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);

    internal SKTypeface DefaultTypeface => _defaultTypeface;

    /// <summary>
    ///    Gets or sets the duration of the hover animation.
    /// </summary>
    [Parameter]
    public TimeSpan HoverAnimationDuration { get; set; } = TimeSpan.FromMilliseconds(250);

    [Inject]
    protected IJSRuntime JSRuntime { get; set; } = default!;

    protected SKPoint? LastMousePosition { get; private set; }

    protected SKRect LastPlotArea { get; private set; }

    protected SKRect LastLegendDrawArea { get; private set; }

    protected string CanvasStyle {
        get {
            if (HoveredSeries != null) {
                return "cursor: pointer;";
            }

            if (_isDraggingLegend || _isHoveringLegend) {
                return "cursor: move;";
            }

            if (_isPanning) {
                return "cursor: grabbing;";
            }

            return EnablePan ? "cursor: grab;" : "cursor: default;";
        }
    }

    private bool _isHovering;
    private bool _isHoveringLegend;

    internal NTBaseSeries<TData>? HoveredSeries { get; private set; }

    internal int? HoveredPointIndex { get; private set; }

    internal TData? HoveredDataPoint { get; private set; }

    private double? _viewXMin;
    private double? _viewXMax;
    private double? _viewYMin;
    private double? _viewYMax;

    private bool _isPanning;
    private SKPoint _panStartPoint;
    private (double Min, double Max)? _panStartXRange;
    private (double Min, double Max)? _panStartYRange;

    private bool _isDraggingLegend;
    private bool _hasDraggedLegend;
    private SKPoint _legendMouseOffset;
    private SKPoint _legendDragStartMousePos;

    internal List<NTBaseSeries<TData>> Series { get; } = [];

    internal NTLegend<TData>? Legend { get; private set; }

    private readonly Dictionary<TnTColor, SKColor> _resolvedColors = [];

    private readonly Dictionary<NTBaseSeries<TData>, SKRect> _treeMapAreas = [];

    private List<double>? _cachedAllX;

    private List<double>? _cachedAllY;

    private IJSObjectReference? _themeListener;
    private DotNetObjectReference<NTChart<TData>>? _objRef;

    private float _density = 1.0f;

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        if (firstRender) {
            _density = await JSRuntime.InvokeAsync<float>("eval", "window.devicePixelRatio || 1");
            _objRef = DotNetObjectReference.Create(this);
            _themeListener = await JSRuntime.InvokeAsync<IJSObjectReference>("NTComponents.onThemeChanged", _objRef);
            await ResolveColorsAsync();
            StateHasChanged();
        }
    }

    [JSInvokable]
    public async Task OnThemeChanged() {
        await ResolveColorsAsync();
        StateHasChanged();
    }

    private async Task ResolveColorsAsync() {
        var colorsToResolve = Enum.GetValues<TnTColor>().ToList();
        foreach (var color in colorsToResolve) {
            if (color is TnTColor.None or TnTColor.Transparent) {
                _resolvedColors[color] = SKColors.Transparent;
                continue;
            }
            if (color == TnTColor.Black) {
                _resolvedColors[color] = SKColors.Black;
                continue;
            }
            if (color == TnTColor.White) {
                _resolvedColors[color] = SKColors.White;
                continue;
            }

            var hex = await JSRuntime.InvokeAsync<string>("NTComponents.getColorValueFromEnumName", color.ToString());
            if (!string.IsNullOrEmpty(hex) && SKColor.TryParse(hex, out var skColor)) {
                _resolvedColors[color] = skColor;
            }
            else {
                // Fallback for primary/secondary etc if not found in CSS
                _resolvedColors[color] = color switch {
                    TnTColor.Primary => SKColors.RoyalBlue,
                    TnTColor.Secondary => SKColors.Gray,
                    _ => SKColors.Gray
                };
            }
        }
    }

    internal SKColor GetSeriesColor(NTBaseSeries<TData> series) {
        var color = series.Color ?? TnTColor.None;
        if (color == TnTColor.None) {
            var index = GetSeriesIndex(series);
            if (index >= 0) {
                color = Palette[index % Palette.Count].Background;
            }
        }

        return _resolvedColors.TryGetValue(color, out var skColor) ? skColor : SKColors.Gray;
    }

    internal SKColor GetSeriesTextColor(NTBaseSeries<TData> series) {
        if (series.TextColor.HasValue && series.TextColor != TnTColor.None) {
            return GetThemeColor(series.TextColor.Value);
        }

        var index = GetSeriesIndex(series);
        if (index >= 0) {
            var color = Palette[index % Palette.Count].Text;
            if (_resolvedColors.TryGetValue(color, out var skColor)) {
                return skColor;
            }
        }

        // Final fallback: Use black or white based on series background color brightness
        var bgColor = GetSeriesColor(series);
        var luminance = (0.2126f * bgColor.Red) + (0.7152f * bgColor.Green) + (0.0722f * bgColor.Blue);
        return luminance > 128 ? SKColors.Black : SKColors.White;
    }

    internal SKColor GetPaletteColor(int index) => _resolvedColors.TryGetValue(Palette[index % Palette.Count].Background, out var skColor) ? skColor : SKColors.Gray;

    internal SKColor GetPaletteTextColor(int index) {
        var color = Palette[index % Palette.Count].Text;
        if (_resolvedColors.TryGetValue(color, out var skColor)) {
            return skColor;
        }

        var bgColor = GetPaletteColor(index);
        var luminance = (0.2126f * bgColor.Red) + (0.7152f * bgColor.Green) + (0.0722f * bgColor.Blue);
        return luminance > 128 ? SKColors.Black : SKColors.White;
    }

    internal SKColor GetThemeColor(TnTColor color) => _resolvedColors.TryGetValue(color, out var skColor) ? skColor : SKColors.Gray;

    public async ValueTask DisposeAsync() {
        _objRef?.Dispose();
        if (_themeListener != null) {
            await _themeListener.InvokeVoidAsync("dispose");
            await _themeListener.DisposeAsync();
        }
    }

    /// <summary>
    ///     Resets the view to the default range.
    /// </summary>
    public void ResetView() {
        _viewXMin = null;
        _viewXMax = null;
        _viewYMin = null;
        _viewYMax = null;
        StateHasChanged();
    }

    protected virtual void OnClick(MouseEventArgs e) {
        if (_hasDraggedLegend) {
            _hasDraggedLegend = false;
            return;
        }

        var point = new SKPoint((float)e.OffsetX * _density, (float)e.OffsetY * _density);

        if (Legend != null && Legend.Visible && Legend.Position != LegendPosition.None) {
            var clickedItem = Legend.GetItemAtPoint(point, LastPlotArea, LastLegendDrawArea);

            if (clickedItem != null) {
                if (clickedItem.Series != null) {
                    clickedItem.Series.ToggleLegendItem(clickedItem.Index);
                    StateHasChanged();
                }
                return;
            }
        }
    }

    private float _lastWidth;
    private float _lastHeight;

    protected virtual void OnMouseMove(MouseEventArgs e) {
        LastMousePosition = new SKPoint((float)e.OffsetX * _density, (float)e.OffsetY * _density);

        if (_isDraggingLegend && Legend != null && LastPlotArea != default) {
            var currentPoint = LastMousePosition.Value;

            // Check for a small movement threshold to allow clicking
            if (!_hasDraggedLegend) {
                var dx = currentPoint.X - _legendDragStartMousePos.X;
                var dy = currentPoint.Y - _legendDragStartMousePos.Y;
                if (Math.Sqrt((dx * dx) + (dy * dy)) > 5) {
                    _hasDraggedLegend = true;
                }
            }

            if (_hasDraggedLegend) {
                var newX = currentPoint.X - _legendMouseOffset.X - LastPlotArea.Left;
                var newY = currentPoint.Y - _legendMouseOffset.Y - LastPlotArea.Top;
                Legend.FloatingOffset = new SKPoint(newX, newY);
                StateHasChanged();
            }
            return;
        }

        if (_isPanning && LastPlotArea != default) {
            var currentPoint = LastMousePosition.Value;
            var dx = _panStartPoint.X - currentPoint.X;
            var dy = currentPoint.Y - _panStartPoint.Y; // Y is inverted in screen coords

            if (_panStartXRange.HasValue) {
                var xRangeSize = _panStartXRange.Value.Max - _panStartXRange.Value.Min;
                var dataDx = (Orientation == NTChartOrientation.Vertical)
                    ? dx / LastPlotArea.Width * xRangeSize
                    : -(dy / LastPlotArea.Height) * xRangeSize;
                _viewXMin = _panStartXRange.Value.Min + dataDx;
                _viewXMax = _panStartXRange.Value.Max + dataDx;
            }

            if (_panStartYRange.HasValue) {
                var yRangeSize = _panStartYRange.Value.Max - _panStartYRange.Value.Min;
                var dataDy = (Orientation == NTChartOrientation.Vertical)
                    ? dy / LastPlotArea.Height * yRangeSize
                    : dx / LastPlotArea.Width * yRangeSize;
                _viewYMin = _panStartYRange.Value.Min + dataDy;
                _viewYMax = _panStartYRange.Value.Max + dataDy;
            }
        }

        StateHasChanged();
    }

    protected virtual void OnMouseDown(MouseEventArgs e) {
        var point = new SKPoint((float)e.OffsetX * _density, (float)e.OffsetY * _density);

        if (Legend != null && Legend.Visible && Legend.Position == LegendPosition.Floating) {
            var rect = Legend.GetFloatingRect(LastPlotArea);
            if (rect.Contains(point)) {
                _isDraggingLegend = true;
                _hasDraggedLegend = false;
                _legendDragStartMousePos = point;
                _legendMouseOffset = new SKPoint(point.X - rect.Left, point.Y - rect.Top);
                return;
            }
        }

        if (EnablePan) {
            _isPanning = true;
            _panStartPoint = point;
            _panStartXRange = GetXRange(true);
            _panStartYRange = GetYRange(true);
        }
    }

    protected virtual void OnMouseUp(MouseEventArgs e) {
        _isPanning = false;
        _isDraggingLegend = false;
    }

    protected virtual void OnWheel(WheelEventArgs e) {
        if (!EnableZoom || LastPlotArea == default) {
            return;
        }

        var mousePoint = new SKPoint((float)e.OffsetX * _density, (float)e.OffsetY * _density);
        if (!LastPlotArea.Contains(mousePoint)) {
            return;
        }

        var zoomFactor = e.DeltaY > 0 ? 1.1 : 0.9;

        // Use ScaleXInverse/ScaleYInverse which already handle orientation
        var xVal = ScaleXInverse(Orientation == NTChartOrientation.Vertical ? mousePoint.X : mousePoint.Y, LastPlotArea);
        var yVal = ScaleYInverse(Orientation == NTChartOrientation.Vertical ? mousePoint.Y : mousePoint.X, LastPlotArea);

        var (xMin, xMax) = GetXRange(true);
        var (yMin, yMax) = GetYRange(true);

        var newXRange = (xMax - xMin) * zoomFactor;
        var newYRange = (yMax - yMin) * zoomFactor;

        var xPct = (xVal - xMin) / (xMax - xMin);
        var yPct = (yVal - yMin) / (yMax - yMin);

        _viewXMin = xVal - (newXRange * xPct);
        _viewXMax = xVal + (newXRange * (1 - xPct));
        _viewYMin = yVal - (newYRange * yPct);
        _viewYMax = yVal + (newYRange * (1 - yPct));

        StateHasChanged();
    }

    protected virtual void OnMouseOut(MouseEventArgs e) {
        LastMousePosition = null;
        HoveredSeries = null;
        HoveredPointIndex = null;
        HoveredDataPoint = null;
        _isHovering = false;
        _isHoveringLegend = false;
        StateHasChanged();
    }

    protected void OnPaintSurface(SKPaintGLSurfaceEventArgs e) => OnPaintSurface(e.Surface.Canvas, e.Info);

    protected void OnPaintSurface(SKPaintSurfaceEventArgs e) => OnPaintSurface(e.Surface.Canvas, e.Info);

    /// <summary>
    ///     Handles the paint surface event from the SkiaSharp view.
    /// </summary>
    protected void OnPaintSurface(SKCanvas canvas, SKImageInfo info) {
        _lastWidth = info.Width;
        _lastHeight = info.Height;

        // Reset hover state and cache for this frame calculation
        HoveredSeries = null;
        HoveredPointIndex = null;
        HoveredDataPoint = null;
        _isHoveringLegend = false;
        _treeMapAreas.Clear();
        _cachedAllX = null;
        _cachedAllY = null;

        canvas.Clear(GetThemeColor(BackgroundColor));

        if (!string.IsNullOrEmpty(Title)) {
            RenderTitle(canvas, info);
        }

        var totalArea = new SKRect(Margin.Left, Margin.Top, info.Width - Margin.Right, info.Height - Margin.Bottom);
        if (!string.IsNullOrEmpty(Title)) {
            totalArea = new SKRect(totalArea.Left, totalArea.Top + 30, totalArea.Right, totalArea.Bottom);
        }
        var plotArea = totalArea;
        var accessibleArea = totalArea; // Area available for axes and legend

        // Pass 1: Measure legend and update plotArea/accessibleArea
        SKRect legendDrawArea = default;
        if (Legend != null && Legend.Visible && Legend.Position != LegendPosition.None && Legend.Position != LegendPosition.Floating) {
            var (newPlotArea, legendArea) = Legend.Measure(plotArea);
            plotArea = newPlotArea;
            legendDrawArea = legendArea;

            // Adjust accessibleArea for axes so they don't overlap legend
            if (Legend.Position == LegendPosition.Bottom) {
                accessibleArea = new SKRect(accessibleArea.Left, accessibleArea.Top, accessibleArea.Right, legendArea.Top);
            }
            else if (Legend.Position == LegendPosition.Top) {
                accessibleArea = new SKRect(accessibleArea.Left, legendArea.Bottom, accessibleArea.Right, accessibleArea.Bottom);
            }
            else if (Legend.Position == LegendPosition.Left) {
                accessibleArea = new SKRect(legendArea.Right, accessibleArea.Top, accessibleArea.Right, accessibleArea.Bottom);
            }
            else if (Legend.Position == LegendPosition.Right) {
                accessibleArea = new SKRect(accessibleArea.Left, accessibleArea.Top, legendArea.Left, accessibleArea.Bottom);
            }
        }

        LastLegendDrawArea = legendDrawArea;

        // Pass 2: Measure series (axes etc) and update plotArea
        var measured = new HashSet<object>();
        foreach (var series in Series.Where(s => s.Visible)) {
            plotArea = series.Measure(plotArea, measured);
        }

        LastPlotArea = plotArea;
        CalculateTreeMapAreas(plotArea);

        // Pass 3: Render axes using the final plotArea and the adjusted accessibleArea
        var rendered = new HashSet<object>();
        foreach (var series in Series.Where(s => s.Visible)) {
            series.RenderAxes(canvas, plotArea, accessibleArea, rendered);
        }

        // Pass 4: Hit testing and Tooltip Prep
        if (LastMousePosition.HasValue) {
            var mousePoint = LastMousePosition.Value;

            // Check legend items for hover first (so they take precedence over lines)
            if (Legend != null && Legend.Visible && Legend.Position != LegendPosition.None) {
                var item = Legend.GetItemAtPoint(mousePoint, plotArea, legendDrawArea);
                if (item != null) {
                    HoveredSeries = item.Series;
                    HoveredPointIndex = item.Index;
                }
                else if (Legend.Position == LegendPosition.Floating) {
                    var rect = Legend.GetFloatingRect(plotArea);
                    if (rect.Contains(mousePoint)) {
                        _isHoveringLegend = true;
                    }
                }
            }

            // Check series/points if legend wasn't hit
            if (HoveredSeries == null && plotArea.Contains(mousePoint)) {
                foreach (var series in Series.AsEnumerable().Reverse().Where(s => s.Visible)) {
                    var seriesRenderArea = GetSeriesRenderArea(series, plotArea, totalArea);
                    var hit = series.HitTest(mousePoint, seriesRenderArea);
                    if (hit != null) {
                        HoveredSeries = series;
                        HoveredPointIndex = hit.Value.Index;
                        HoveredDataPoint = hit.Value.Data;
                        break;
                    }
                }
            }
        }

        // Pass 5: Render Series
        canvas.Save();

        // Inflate the clip rect slightly so we don't cutoff line thickness or point markers
        var clipArea = plotArea;
        clipArea.Inflate(2, 2);
        canvas.ClipRect(clipArea);

        // Render inactive series first
        foreach (var series in Series.Where(s => s != HoveredSeries && s.IsEffectivelyVisible)) {
            var seriesRenderArea = GetSeriesRenderArea(series, plotArea, totalArea);
            series.Render(canvas, seriesRenderArea);
        }
        // Render active series last (on top)
        if (HoveredSeries != null && HoveredSeries.IsEffectivelyVisible) {
            var seriesRenderArea = GetSeriesRenderArea(HoveredSeries, plotArea, totalArea);
            HoveredSeries.Render(canvas, seriesRenderArea);
        }
        canvas.Restore();

        // Pass 6: Render TreeMap group labels if needed
        RenderTreeMapGroupLabels(canvas);

        // Pass 7: Render Tooltip
        if (HoveredDataPoint != null && LastMousePosition.HasValue && HoveredSeries != null && HoveredSeries.Visible) {
            RenderTooltip(canvas, plotArea);
        }

        // Pass 8: Render legend (Now after hit testing so it can react to hovered series)
        if (Legend != null && Legend.Visible && Legend.Position != LegendPosition.None) {
            if (Legend.Position == LegendPosition.Floating) {
                Legend.Render(canvas, plotArea, totalArea);
            }
            else {
                Legend.Render(canvas, plotArea, legendDrawArea);
            }
        }

        // Pass 9: Update cursor if needed
        var currentHover = HoveredSeries != null || _isHoveringLegend || _isPanning;
        if (_isHovering != currentHover) {
            _isHovering = currentHover;
            _ = InvokeAsync(StateHasChanged);
        }
    }

    private void RenderTooltip(SKCanvas canvas, SKRect plotArea) {
        if (HoveredSeries == null || HoveredDataPoint == null || !LastMousePosition.HasValue) {
            return;
        }

        var mousePoint = LastMousePosition.Value;
        var lines = HoveredSeries.GetTooltipLines(HoveredDataPoint);

        if (lines.Count == 0) {
            return;
        }

        var bgColor = HoveredSeries.TooltipBackgroundColor.HasValue
            ? GetThemeColor(HoveredSeries.TooltipBackgroundColor.Value)
            : GetSeriesColor(HoveredSeries);
        var textColor = HoveredSeries.TooltipTextColor.HasValue
            ? GetThemeColor(HoveredSeries.TooltipTextColor.Value)
            : GetSeriesTextColor(HoveredSeries);

        using var textPaint = new SKPaint {
            Color = textColor,
            IsAntialias = true
        };

        using var font = new SKFont {
            Size = 12,
            Typeface = DefaultTypeface
        };

        var lineHeight = font.Size + 4;
        var textWidth = lines.Max(l => font.MeasureText(l));
        var totalHeight = (lines.Count * lineHeight) + 5;

        // X starts at mousePoint + 10. Text starts at rect.Left + 10.
        // So we add 10 to the left and provide 15 on the right.
        var rect = new SKRect(mousePoint.X + 10, mousePoint.Y - totalHeight - 5, mousePoint.X + 20 + textWidth + 15, mousePoint.Y - 5);

        // Keep tooltip within canvas
        if (rect.Right > plotArea.Right + Margin.Right) {
            rect.Offset(-(rect.Width + 20), 0);
        }
        if (rect.Top < plotArea.Top) {
            rect.Offset(0, rect.Height + 10);
        }

        using var bgPaint = new SKPaint {
            Color = bgColor,
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            ImageFilter = SKImageFilter.CreateDropShadow(2, 2, 4, 4, SKColors.Black.WithAlpha(80))
        };

        canvas.DrawRoundRect(rect, 6, 6, bgPaint);

        using var borderPaint = new SKPaint {
            Color = GetThemeColor(TnTColor.Outline),
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 1,
            IsAntialias = true
        };

        canvas.DrawRoundRect(rect, 6, 6, borderPaint);

        var y = rect.Top + lineHeight;
        foreach (var line in lines) {
            canvas.DrawText(line, rect.Left + 10, y, SKTextAlign.Left, font, textPaint);
            y += lineHeight;
        }
    }

    private SKRect GetSeriesRenderArea(NTBaseSeries<TData> series, SKRect plotArea, SKRect totalArea) {
        if (series.CoordinateSystem == ChartCoordinateSystem.TreeMap) {
            if (_treeMapAreas.TryGetValue(series, out var area)) {
                var treeMapSeries = Series.Where(s => s.CoordinateSystem == ChartCoordinateSystem.TreeMap && s.IsEffectivelyVisible).ToList();
                if (treeMapSeries.Count > 1) {
                    // Shave off top for series title
                    return new SKRect(area.Left, area.Top + 20, area.Right, area.Bottom);
                }
                return area;
            }
        }

        if (series.CoordinateSystem == ChartCoordinateSystem.Circular &&
            Legend != null && Legend.Visible && (Legend.Position == LegendPosition.Left || Legend.Position == LegendPosition.Right)) {

            var centerX = totalArea.MidX;
            var centerY = totalArea.MidY;

            var dx = Math.Min(centerX - plotArea.Left, plotArea.Right - centerX);
            var dy = Math.Min(centerY - plotArea.Top, plotArea.Bottom - centerY);

            return new SKRect(centerX - dx, centerY - dy, centerX + dx, centerY + dy);
        }
        return plotArea;
    }

    private record SeriesLayoutItem(NTBaseSeries<TData> Series, double Value);

    private void CalculateTreeMapAreas(SKRect plotArea) {
        _treeMapAreas.Clear();
        var treeMapSeries = Series.Where(s => s.CoordinateSystem == ChartCoordinateSystem.TreeMap && s.IsEffectivelyVisible).ToList();
        if (!treeMapSeries.Any()) {
            return;
        }

        if (treeMapSeries.Count == 1) {
            _treeMapAreas[treeMapSeries[0]] = plotArea;
            return;
        }

        var seriesData = treeMapSeries.Select(s => new SeriesLayoutItem(s, s.GetTotalValue())).ToList();
        var totalValue = seriesData.Sum(s => s.Value);
        if (totalValue <= 0) {
            totalValue = seriesData.Count;
            seriesData = treeMapSeries.Select(s => new SeriesLayoutItem(s, 1.0)).ToList();
        }

        PartitionArea(seriesData, plotArea, totalValue, true);
    }

    private void PartitionArea(List<SeriesLayoutItem> items, SKRect area, double totalValue, bool horizontal) {
        if (!items.Any()) {
            return;
        }

        if (items.Count == 1) {
            _treeMapAreas[items[0].Series] = area;
            return;
        }

        var mid = items.Count / 2;
        var leftItems = items.Take(mid).ToList();
        var rightItems = items.Skip(mid).ToList();

        var leftValue = leftItems.Sum(x => x.Value);
        var rightValue = rightItems.Sum(x => x.Value);
        var total = leftValue + rightValue;

        if (total <= 0) {
            return;
        }

        if (horizontal) {
            var leftWidth = (float)(area.Width * (leftValue / total));
            var leftArea = new SKRect(area.Left, area.Top, area.Left + leftWidth, area.Bottom);
            var rightArea = new SKRect(area.Left + leftWidth, area.Top, area.Right, area.Bottom);
            PartitionArea(leftItems, leftArea, leftValue, !horizontal);
            PartitionArea(rightItems, rightArea, rightValue, !horizontal);
        }
        else {
            var topHeight = (float)(area.Height * (leftValue / total));
            var topArea = new SKRect(area.Left, area.Top, area.Right, area.Top + topHeight);
            var bottomArea = new SKRect(area.Left, area.Top + topHeight, area.Right, area.Bottom);
            PartitionArea(leftItems, topArea, leftValue, !horizontal);
            PartitionArea(rightItems, bottomArea, rightValue, !horizontal);
        }
    }

    private void RenderTreeMapGroupLabels(SKCanvas canvas) {
        var treeMapSeries = Series.Where(s => s.CoordinateSystem == ChartCoordinateSystem.TreeMap && s.IsEffectivelyVisible).ToList();
        if (treeMapSeries.Count <= 1) {
            return;
        }

        foreach (var series in treeMapSeries) {
            if (!_treeMapAreas.TryGetValue(series, out var area)) {
                continue;
            }

            using var paint = new SKPaint {
                Color = GetSeriesTextColor(series).WithAlpha((byte)(255 * series.VisibilityFactor)),
                IsAntialias = true,
                Style = SKPaintStyle.Fill
            };

            using var font = new SKFont {
                Size = 14,
                Embolden = true,
                Typeface = DefaultTypeface
            };

            var title = series.Title ?? "Series";
            // Measure text to make sure it fits
            var textWidth = font.MeasureText(title);
            if (area.Width < textWidth + 10 || area.Height < 20) {
                continue;
            }

            // Draw at top left of the area with a small offset
            canvas.DrawText(title, area.Left + 5, area.Top + 15, SKTextAlign.Left, font, paint);
        }
    }

    private void RenderTitle(SKCanvas canvas, SKImageInfo info) {
        using var paint = new SKPaint {
            Color = GetThemeColor(TextColor),
            IsAntialias = true,
            Style = SKPaintStyle.Fill
        };

        using var font = new SKFont {
            Size = 20,
            Embolden = true,
            Typeface = DefaultTypeface
        };

        var x = Margin.Left + ((info.Width - Margin.Left - Margin.Right) / 2);
        var y = Margin.Top + 20;

        canvas.DrawText(Title!, x, y, SKTextAlign.Center, font, paint);
    }

    internal void AddSeries(NTBaseSeries<TData> series) {
        if (Series.Count > 0 && Series[0].CoordinateSystem != series.CoordinateSystem) {
            throw new InvalidOperationException($"Cannot combine series with different coordinate systems. Currently using {Series[0].CoordinateSystem}, but tried to add {series.CoordinateSystem}.");
        }

        if (!Series.Contains(series)) {
            Series.Add(series);
        }
    }

    internal void RemoveSeries(NTBaseSeries<TData> series) {
        if (Series.Contains(series)) {
            Series.Remove(series);
        }
    }

    internal void SetLegend(NTLegend<TData> legend) => Legend = legend;

    internal void RemoveLegend(NTLegend<TData> legend) {
        if (Legend == legend) {
            Legend = null;
        }
    }

    internal int GetSeriesIndex(NTBaseSeries<TData> series) => Series.IndexOf(series);

    internal float GetBarSeriesTotalWeight() => Series.OfType<NTBarSeries<TData>>().Sum(s => s.VisibilityFactor);

    internal float GetBarSeriesOffsetWeight(NTBarSeries<TData> series) {
        float weight = 0;
        foreach (var s in Series.OfType<NTBarSeries<TData>>()) {
            if (s == series) {
                return weight;
            }

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
    ///    Gets or sets whether to use a categorical scale for the Y axis.
    ///    If true, every unique Y value will be shown and spaced equally.
    /// </summary>
    [Parameter]
    public bool IsCategoricalY { get; set; }

    /// <summary>
    ///     Returns a list of all unique X values across all cartesian series, sorted.
    /// </summary>
    public List<double> GetAllXValues() {
        if (_cachedAllX != null) {
            return _cachedAllX;
        }

        var allX = new HashSet<double>();
        foreach (var s in Series.Where(s => s.IsEffectivelyVisible)) {
            s.RegisterXValues(allX);
        }

        _cachedAllX = allX.OrderBy(x => x).ToList();
        return _cachedAllX;
    }

    /// <summary>
    ///     Returns a list of all unique Y values across all cartesian series, sorted.
    /// </summary>
    public List<double> GetAllYValues() {
        if (_cachedAllY != null) {
            return _cachedAllY;
        }

        var allY = new HashSet<double>();
        foreach (var s in Series.Where(s => s.IsEffectivelyVisible)) {
            s.RegisterYValues(allY);
        }

        _cachedAllY = allY.OrderBy(y => y).ToList();
        return _cachedAllY;
    }

    public double GetScaledXValue(double originalX) {
        if (IsCategoricalX) {
            var allX = GetAllXValues();
            var index = allX.IndexOf(originalX);
            return index >= 0 ? index : originalX;
        }
        return originalX;
    }

    public double GetScaledYValue(double originalY) {
        if (IsCategoricalY) {
            var allY = GetAllYValues();
            var index = allY.IndexOf(originalY);
            return index >= 0 ? index : originalY;
        }
        return originalY;
    }

    /// <summary>
    ///    Gets the scale used for the X axis.
    /// </summary>
    public NTAxisScale GetXScale() => Series.OfType<NTCartesianSeries<TData>>().FirstOrDefault(s => s.XAxis != null)?.XAxis!.Scale ?? NTAxisScale.Linear;

    /// <summary>
    ///    Gets the scale used for the Y axis.
    /// </summary>
    public NTAxisScale GetYScale() => Series.OfType<NTCartesianSeries<TData>>().FirstOrDefault(s => s.YAxis != null)?.YAxis!.Scale ?? NTAxisScale.Linear;

    public float ScaleX(double x, SKRect plotArea) {
        var (min, max) = GetXRange(true);
        var scale = GetXScale();

        double t;
        if (scale == NTAxisScale.Logarithmic) {
            min = Math.Max(0.000001, min);
            max = Math.Max(min * 1.1, max);
            x = Math.Max(min, x);
            t = (Math.Log10(x) - Math.Log10(min)) / (Math.Log10(max) - Math.Log10(min));
        }
        else {
            var range = max - min;
            if (range <= 0) {
                return (Orientation == NTChartOrientation.Vertical) ? plotArea.Left : plotArea.Bottom;
            }
            t = (x - min) / range;
        }

        const float p = 3f; // 3 pixels of air
        if (Orientation == NTChartOrientation.Vertical) {
            var left = plotArea.Left + p;
            var width = plotArea.Width - (p * 2);
            return (float)(left + (t * width));
        }
        else {
            var bottom = plotArea.Bottom - p;
            var height = plotArea.Height - (p * 2);
            return (float)(bottom - (t * height));
        }
    }

    public float ScaleY(double y, SKRect plotArea) {
        var (min, max) = GetYRange(true);
        var scale = GetYScale();

        double t;
        if (scale == NTAxisScale.Logarithmic) {
            min = Math.Max(0.000001, min);
            max = Math.Max(min * 1.1, max);
            y = Math.Max(min, y);
            t = (Math.Log10(y) - Math.Log10(min)) / (Math.Log10(max) - Math.Log10(min));
        }
        else {
            var range = max - min;
            if (range <= 0) {
                return (Orientation == NTChartOrientation.Vertical) ? plotArea.Bottom : plotArea.Left;
            }
            t = (y - min) / range;
        }

        const float p = 3f; // 3 pixels of air
        if (Orientation == NTChartOrientation.Vertical) {
            var bottom = plotArea.Bottom - p;
            var height = plotArea.Height - (p * 2);
            return (float)(bottom - (t * height));
        }
        else {
            var left = plotArea.Left + p;
            var width = plotArea.Width - (p * 2);
            return (float)(left + (t * width));
        }
    }

    /// <summary>
    ///     Converts a screen coordinate back to a data X value.
    /// </summary>
    public double ScaleXInverse(float coord, SKRect plotArea) {
        var (min, max) = GetXRange(true);
        var scale = GetXScale();

        const float p = 3f;
        double t;
        if (Orientation == NTChartOrientation.Vertical) {
            var left = plotArea.Left + p;
            var width = plotArea.Width - (p * 2);
            t = width <= 0 ? 0 : (coord - left) / width;
        }
        else {
            var bottom = plotArea.Bottom - p;
            var height = plotArea.Height - (p * 2);
            t = height <= 0 ? 0 : (bottom - coord) / height;
        }

        if (scale == NTAxisScale.Logarithmic) {
            min = Math.Max(0.000001, min);
            max = Math.Max(min * 1.1, max);
            return Math.Pow(10, Math.Log10(min) + (t * (Math.Log10(max) - Math.Log10(min))));
        }
        return min + (t * (max - min));
    }

    /// <summary>
    ///     Converts a screen coordinate back to a data Y value.
    /// </summary>
    public double ScaleYInverse(float coord, SKRect plotArea) {
        var (min, max) = GetYRange(true);
        var scale = GetYScale();

        const float p = 3f;
        double t;
        if (Orientation == NTChartOrientation.Vertical) {
            var bottom = plotArea.Bottom - p;
            var height = plotArea.Height - (p * 2);
            t = height <= 0 ? 0 : (bottom - coord) / height;
        }
        else {
            var left = plotArea.Left + p;
            var width = plotArea.Width - (p * 2);
            t = width <= 0 ? 0 : (coord - left) / width;
        }

        if (scale == NTAxisScale.Logarithmic) {
            min = Math.Max(0.000001, min);
            max = Math.Max(min * 1.1, max);
            return Math.Pow(10, Math.Log10(min) + (t * (Math.Log10(max) - Math.Log10(min))));
        }
        return min + (t * (max - min));
    }

    public (double Min, double Max) GetXRange(bool padded = false) {
        if (_viewXMin.HasValue && _viewXMax.HasValue) {
            return (_viewXMin.Value, _viewXMax.Value);
        }

        if (IsCategoricalX) {
            var allX = GetAllXValues();
            if (!allX.Any()) {
                return (0, 1);
            }

            if (!padded) {
                return (0, Math.Max(1, allX.Count - 1));
            }

            var catRange = Math.Max(1, allX.Count - 1);
            return (-catRange * RangePadding, catRange + (catRange * RangePadding));
        }

        var min = double.MaxValue;
        var max = double.MinValue;

        foreach (var s in Series.Where(s => s.IsEffectivelyVisible)) {
            var seriesRange = s.GetXRange();
            if (seriesRange.HasValue) {
                min = Math.Min(min, seriesRange.Value.Min);
                max = Math.Max(max, seriesRange.Value.Max);
            }
        }

        if (min == double.MaxValue) {
            return (0, 1);
        }

        if (!padded) {
            return (min, max);
        }

        if (UseNiceNumbers) {
            var (niceMin, niceMax, _) = CalculateNiceScaling(min, max);
            return (niceMin, niceMax);
        }

        var range = max - min;
        if (range == 0) {
            range = 1;
        }

        return (min - (range * RangePadding), max + (range * RangePadding));
    }

    public (double Min, double Max) GetYRange(bool padded = false) {
        if (_viewYMin.HasValue && _viewYMax.HasValue) {
            return (_viewYMin.Value, _viewYMax.Value);
        }

        if (IsCategoricalY) {
            var allY = GetAllYValues();
            if (!allY.Any()) {
                return (0, 1);
            }

            if (!padded) {
                return (0, Math.Max(1, allY.Count - 1));
            }

            var catRange = Math.Max(1, allY.Count - 1);
            return (-catRange * RangePadding, catRange + (catRange * RangePadding));
        }

        var min = double.MaxValue;
        var max = double.MinValue;

        foreach (var s in Series.Where(s => s.IsEffectivelyVisible)) {
            var seriesRange = s.GetYRange();
            if (seriesRange.HasValue) {
                min = Math.Min(min, seriesRange.Value.Min);
                max = Math.Max(max, seriesRange.Value.Max);
            }
        }

        if (min == double.MaxValue) {
            return (0, 1);
        }

        // Bar charts should generally start at 0
        if (Series.Any(s => s is NTBarSeries<TData> && s.IsEffectivelyVisible)) {
            min = Math.Min(0, min);
        }

        if (!padded) {
            return (min, max);
        }

        if (UseNiceNumbers) {
            var (niceMin, niceMax, _) = CalculateNiceScaling(min, max);
            return (niceMin, niceMax);
        }

        var range = max - min;
        if (range == 0) {
            range = 1;
        }

        var minPad = min - (range * RangePadding);
        if (min >= 0 && minPad < 0) {
            minPad = 0;
        }

        return (minPad, max + (range * RangePadding));
    }

    public (double Min, double Max, double Spacing) CalculateNiceScaling(double min, double max, int maxTicks = 5) {
        if (min == max) {
            max = min + 1;
        }

        var range = CalculateNiceNumber(max - min, false);
        var tickSpacing = CalculateNiceNumber(range / (maxTicks - 1), true);

        var niceMin = Math.Floor(min / tickSpacing) * tickSpacing;
        var niceMax = Math.Ceiling(max / tickSpacing) * tickSpacing;

        return (niceMin, niceMax, tickSpacing);
    }

    private double CalculateNiceNumber(double range, bool round) {
        var exponent = Math.Floor(Math.Log10(range));
        var fraction = range / Math.Pow(10, exponent);
        double niceFraction;

        if (round) {
            if (fraction < 1.5) {
                niceFraction = 1;
            }
            else {
                niceFraction = fraction < 3 ? 2 : fraction < 7 ? 5 : 10;
            }
        }
        else {
            if (fraction <= 1) {
                niceFraction = 1;
            }
            else {
                niceFraction = fraction <= 2 ? 2 : fraction <= 5 ? 5 : 10;
            }
        }

        return niceFraction * Math.Pow(10, exponent);
    }

    /// <summary>
    ///     Exports the current chart as a PNG image.
    /// </summary>
    /// <param name="fileName">The name of the file to download. Defaults to "[Title].png" or "chart.png".</param>
    /// <returns>A <see cref="Task" /> representing the export operation.</returns>
    public async Task ExportAsPngAsync(string? fileName = null) {
        if (_lastWidth <= 0 || _lastHeight <= 0) {
            return;
        }

        var info = new SKImageInfo((int)_lastWidth, (int)_lastHeight);
        using var surface = SKSurface.Create(info);
        if (surface == null) {
            return;
        }

        OnPaintSurface(surface.Canvas, info);

        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        if (data == null) {
            return;
        }

        using var stream = data.AsStream();
        await JSRuntime.DownloadFileFromStreamAsync(stream, fileName ?? $"{Title ?? "chart"}.png");
    }
}