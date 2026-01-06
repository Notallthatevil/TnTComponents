using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using NTComponents.Charts.Core.Animations;
using NTComponents.Core;
using SkiaSharp;
using SkiaSharp.Views.Blazor;
using System.Diagnostics;

namespace NTComponents.Charts.Core;

/// <summary>
///     The base class for all charts in the NTComponents.Charts library.
/// </summary>
public abstract partial class NTChartBase(IJSRuntime jsRuntime) : TnTComponentBase {

    /// <summary>
    ///     Gets or sets whether animations are enabled.
    /// </summary>
    [Parameter]
    public bool AnimationsEnabled { get; set; } = true;

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create("nt-chart")
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    /// <inheritdoc />
    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    /// <summary>
    ///     Gets or sets the legend configuration.
    /// </summary>
    [Parameter]
    public Legend Legend { get; set; } = new();

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

    protected SKPoint? LastMousePosition { get; private set; }

    /// <summary>
    ///     Gets the animation used for initial loading.
    /// </summary>
    protected ChartAnimation LoadingAnimation { get; } = new(1000, Easing.InOutCubic);

    protected ChartThemeHelper ThemeHelper { get; private set; } = default!;
    protected readonly IJSRuntime JSRuntime = jsRuntime;
    private readonly Stopwatch _stopwatch = new();
    private long _lastFrameTime;

    protected virtual void OnClick(MouseEventArgs e) {
        // Handle click hit-testing in subclasses if needed
    }

    protected override void OnInitialized() {
        base.OnInitialized();
        ThemeHelper = new ChartThemeHelper(JSRuntime);
    }

    protected virtual void OnMouseMove(MouseEventArgs e) => LastMousePosition = new SKPoint((float)e.OffsetX, (float)e.OffsetY);

    protected virtual void OnMouseOut(MouseEventArgs e) => LastMousePosition = null;

    /// <summary>
    ///     Handles the paint surface event from the SkiaSharp view.
    /// </summary>
    /// <param name="e">The paint surface event arguments.</param>
    protected virtual void OnPaintSurface(SKPaintGLSurfaceEventArgs e) {
        if (!_stopwatch.IsRunning) {
            _stopwatch.Start();
            _lastFrameTime = 0;
        }

        var currentTime = _stopwatch.ElapsedMilliseconds;
        var elapsed = currentTime - _lastFrameTime;
        _lastFrameTime = currentTime;

        if (AnimationsEnabled) {
            LoadingAnimation.Update(elapsed);
        }
        else {
            if (!LoadingAnimation.IsCompleted) {
                LoadingAnimation.Update(1000);
            }
        }

        Render(e.Surface.Canvas, e.Info);
    }

    /// <summary>
    ///     Renders the chart onto the SkiaSharp surface.
    /// </summary>
    /// <param name="canvas">The canvas to draw on.</param>
    /// <param name="info">  The image info of the surface.</param>
    protected abstract void Render(SKCanvas canvas, SKImageInfo info);
}