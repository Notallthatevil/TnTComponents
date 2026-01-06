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
public partial class NTChart<TData> : TnTComponentBase where TData : class
{

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

    protected SKPoint? LastMousePosition { get; private set; }

    private List<NTAxis<TData>> Axes { get; } = [];

    private List<NTBaseSeries<TData>> Series { get; } = [];

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
        canvas.Clear();

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

        foreach (var series in Series)
        {
            series.Render(canvas, plotArea);
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

    public (double Min, double Max) GetXRange()
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
        return min == double.MaxValue ? (0, 1) : (min, max);
    }

    public (double Min, double Max) GetYRange()
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
        return min == double.MaxValue ? (0, 1) : (min, max);
    }
}