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

    [Inject]
    protected IJSRuntime JSRuntime { get; set; } = default!;

    protected SKPoint? LastMousePosition { get; private set; }

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
        // Handle click hit-testing in subclasses if needed
    }

    protected virtual void OnMouseMove(MouseEventArgs e) => LastMousePosition = new SKPoint((float)e.OffsetX, (float)e.OffsetY);

    protected virtual void OnMouseOut(MouseEventArgs e) => LastMousePosition = null;

    /// <summary>
    ///     Handles the paint surface event from the SkiaSharp view.
    /// </summary>
    /// <param name="e">The paint surface event arguments.</param>
    protected virtual void OnPaintSurface(SKPaintGLSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        canvas.Clear(GetThemeColor(BackgroundColor));

        var info = e.Info;
        var totalArea = new SKRect(Margin.Left, Margin.Top, info.Width - Margin.Right, info.Height - Margin.Bottom);
        var plotArea = totalArea;

        // Pass 1: Measure axes and update plotArea
        foreach (var axis in Axes.Where(a => a.Visible))
        {
            plotArea = axis.Measure(plotArea);
        }

        // Pass 2: Render axes using the final plotArea
        foreach (var axis in Axes.Where(a => a.Visible))
        {
            axis.Render(canvas, plotArea, totalArea);
        }

        canvas.Save();
        canvas.ClipRect(plotArea);
        foreach (var series in Series)
        {
            series.Render(canvas, plotArea);
        }
        canvas.Restore();
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