using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using NTComponents.Charts.Core.Series;
using NTComponents.Core;
using SkiaSharp;
using SkiaSharp.Views.Blazor;
using System.Diagnostics;

namespace NTComponents.Charts.Core;

/// <summary>
///     The base class for all charts in the NTComponents.Charts library.
/// </summary>
public partial class NTChart<TData> : TnTComponentBase where TData : class {

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
    ///     Gets or sets the title of the chart.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    protected SKPoint? LastMousePosition { get; private set; }

    private List<NTBaseSeries<TData>> Series { get; } = [];

    protected virtual void OnClick(MouseEventArgs e) {
        // Handle click hit-testing in subclasses if needed
    }

    protected virtual void OnMouseMove(MouseEventArgs e) => LastMousePosition = new SKPoint((float)e.OffsetX, (float)e.OffsetY);

    protected virtual void OnMouseOut(MouseEventArgs e) => LastMousePosition = null;

    /// <summary>
    ///     Handles the paint surface event from the SkiaSharp view.
    /// </summary>
    /// <param name="e">The paint surface event arguments.</param>
    protected virtual void OnPaintSurface(SKPaintGLSurfaceEventArgs e) {
    }

    internal void AddSeries(NTBaseSeries<TData> series) {
        if (!Series.Contains(series)) {
            Series.Add(series);
        }
    }

    internal void RemoveSeries(NTBaseSeries<TData> series) {
        if (Series.Contains(series)) {
            Series.Remove(series);
        }
    }
}