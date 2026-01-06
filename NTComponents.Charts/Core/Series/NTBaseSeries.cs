using Microsoft.AspNetCore.Components;
using NTComponents.Charts.Core;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTComponents.Charts.Core.Series;

public abstract class NTBaseSeries<TData> : ComponentBase, IDisposable where TData : class
{

    [CascadingParameter]
    protected NTChart<TData> Chart { get; set; } = default!;

    [Parameter]
    public IEnumerable<TData> Data { get; set; } = [];

    /// <summary>
    ///     Gets or sets the color of the series. If null or <see cref="TnTColor.None"/>, a color will be chosen from the chart's palette.
    /// </summary>
    [Parameter]
    public TnTColor? Color { get; set; }

    /// <summary>
    ///     Gets or sets the title of the series.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    ///     Gets or sets whether animation is enabled for this series.
    /// </summary>
    [Parameter]
    public bool AnimationEnabled { get; set; } = true;

    /// <summary>
    ///     Gets or sets the duration of the animation.
    /// </summary>
    [Parameter]
    public TimeSpan AnimationDuration { get; set; } = TimeSpan.FromMilliseconds(1000);

    /// <summary>
    ///     Gets the coordinate system used by this series.
    /// </summary>
    public abstract ChartCoordinateSystem CoordinateSystem { get; }

    protected DateTime AnimationStartTime { get; private set; } = DateTime.Now;

    protected IEnumerable<TData> PreviousData { get; private set; } = [];

    protected override void OnInitialized()
    {
        base.OnInitialized();
        if (Chart is null)
        {
            throw new ArgumentNullException(nameof(Chart), $"Series must be used within a {nameof(NTChart<TData>)}.");
        }
        Chart.AddSeries(this);
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        if (!ReferenceEquals(PreviousData, Data))
        {
            OnDataChanged();
            PreviousData = Data;
        }
    }

    /// <summary>
    ///    Called when the data reference changes.
    /// </summary>
    protected virtual void OnDataChanged()
    {
        ResetAnimation();
    }

    /// <summary>
    ///     Resets the animation to start from the beginning.
    /// </summary>
    public void ResetAnimation() => AnimationStartTime = DateTime.Now;

    protected float GetAnimationProgress()
    {
        if (!AnimationEnabled) return 1.0f;
        var elapsed = DateTime.Now - AnimationStartTime;
        var progress = (float)(elapsed.TotalMilliseconds / AnimationDuration.TotalMilliseconds);
        return Math.Clamp(progress, 0, 1);
    }

    /// <summary>
    ///     Applies an overshoot effect to the progress.
    /// </summary>
    /// <param name="t">The progress value between 0 and 1.</param>
    /// <returns>The eased progress value.</returns>
    protected float BackEase(float t)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1;
        return 1 + c3 * MathF.Pow(t - 1, 3) + c1 * MathF.Pow(t - 1, 2);
    }

    public abstract void Render(SKCanvas canvas, SKRect renderArea);

    public void Dispose()
    {
        Chart?.RemoveSeries(this);
    }
}
