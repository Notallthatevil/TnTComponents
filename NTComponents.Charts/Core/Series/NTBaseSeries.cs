using Microsoft.AspNetCore.Components;
using SkiaSharp;

namespace NTComponents.Charts.Core.Series;

public abstract class NTBaseSeries<TData> : ComponentBase, IDisposable where TData : class {

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
    ///     Gets or sets the background color of the tooltip for this series.
    /// </summary>
    [Parameter]
    public TnTColor? TooltipBackgroundColor { get; set; }

    /// <summary>
    ///    Gets or sets the text color of the tooltip for this series.
    /// </summary>
    [Parameter]
    public TnTColor? TooltipTextColor { get; set; }

    /// <summary>
    ///     Gets or sets whether animation is enabled for this series.
    /// </summary>
    [Parameter]
    public bool AnimationEnabled { get; set; } = true;

    /// <summary>
    ///     Gets or sets whether the series is visible.
    /// </summary>
    [Parameter]
    public bool Visible { get; set; } = true;

    private bool _lastVisible = true;

    /// <summary>
    ///     Gets or sets the duration of the animation.
    /// </summary>
    [Parameter]
    public TimeSpan AnimationDuration { get; set; } = TimeSpan.FromMilliseconds(500);

    /// <summary>
    ///     Gets the start time of the animation.
    /// </summary>
    protected DateTime AnimationStartTime { get; set; } = DateTime.Now;

    /// <summary>
    ///     Gets or sets the previous data.
    /// </summary>
    protected IEnumerable<TData>? PreviousData { get; set; }

    /// <summary>
    ///     Gets the coordinate system of the series.
    /// </summary>
    public abstract ChartCoordinateSystem CoordinateSystem { get; }

    private float _currentVisibility = 1f;
    private float _startVisibility = 1f;
    private DateTime? _visibilityAnimationStartTime;

    private void OnVisibilityChanged() {
        _startVisibility = VisibilityFactor;
        _visibilityAnimationStartTime = DateTime.Now;

        // We also want to reset the primary data animation if we are appearing
        if (Visible) {
            ResetAnimation();
        }
    }

    /// <summary>
    ///     Gets the current visibility factor (0.0 to 1.0) for animation.
    /// </summary>
    public float VisibilityFactor {
        get {
            if (!AnimationEnabled) {
                return Visible ? 1f : 0f;
            }

            if (_visibilityAnimationStartTime == null) {
                return Visible ? 1f : 0f;
            }

            var elapsed = DateTime.Now - _visibilityAnimationStartTime.Value;
            var progress = (float)(elapsed.TotalMilliseconds / AnimationDuration.TotalMilliseconds);
            progress = Math.Clamp(progress, 0, 1);

            // Use simple linear for visibility factor transition
            _currentVisibility = _startVisibility + (((Visible ? 1f : 0f) - _startVisibility) * progress);

            if (progress >= 1) {
                _visibilityAnimationStartTime = null;
            }

            return _currentVisibility;
        }
    }

    /// <summary>
    ///     Returns true if the series is visible or currently animating visibility.
    /// </summary>
    public bool IsEffectivelyVisible => Visible || (VisibilityFactor > 0.001f);

    protected override void OnInitialized() {
        base.OnInitialized();
        if (Chart is null) {
            throw new ArgumentNullException(nameof(Chart), $"Series must be used within a {nameof(NTChart<TData>)}.");
        }
        Chart.AddSeries(this);
    }

    protected override void OnParametersSet() {
        base.OnParametersSet();

        if (Visible != _lastVisible) {
            OnVisibilityChanged();
            _lastVisible = Visible;
        }

        if (!ReferenceEquals(PreviousData, Data)) {
            OnDataChanged();
            PreviousData = Data;
        }
    }

    /// <summary>
    ///    Called when the data reference changes.
    /// </summary>
    protected virtual void OnDataChanged() => ResetAnimation();

    /// <summary>
    ///     Resets the animation to start from the beginning.
    /// </summary>
    public void ResetAnimation() => AnimationStartTime = DateTime.Now;

    protected float GetAnimationProgress() {
        if (!AnimationEnabled) {
            return 1.0f;
        }

        var elapsed = DateTime.Now - AnimationStartTime;
        var progress = (float)(elapsed.TotalMilliseconds / AnimationDuration.TotalMilliseconds);
        return Math.Clamp(progress, 0, 1);
    }

    /// <summary>
    ///     Applies an overshoot effect to the progress.
    /// </summary>
    /// <param name="t">The progress value between 0 and 1.</param>
    /// <returns>The eased progress value.</returns>
    protected float BackEase(float t) {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1;
        return 1 + (c3 * MathF.Pow(t - 1, 3)) + (c1 * MathF.Pow(t - 1, 2));
    }

    public abstract void Render(SKCanvas canvas, SKRect renderArea);

    /// <summary>
    ///     Returns the legend items for this series.
    /// </summary>
    /// <returns>The legend items.</returns>
    internal virtual IEnumerable<LegendItemInfo<TData>> GetLegendItems()
    {
        yield return new LegendItemInfo<TData>
        {
            Label = Title ?? $"Series {Chart.GetSeriesIndex(this) + 1}",
            Color = Chart.GetSeriesColor(this),
            Series = this,
            IsVisible = Visible
        };
    }

    /// <summary>
    ///     Toggles the visibility of a legend item.
    /// </summary>
    /// <param name="index">The index of the item, if applicable.</param>
    internal virtual void ToggleLegendItem(int? index)
    {
        if (index == null)
        {
            Visible = !Visible;
        }
    }

    /// <summary>
    ///     Performs a hit test on the series.
    /// </summary>
    /// <param name="point">The mouse point.</param>
    /// <param name="renderArea">The plot area.</param>
    /// <returns>The index and data of the hit point, or null if no hit.</returns>
    public abstract (int Index, TData? Data)? HitTest(SKPoint point, SKRect renderArea);

    public void Dispose() {
        GC.SuppressFinalize(this);
        Chart?.RemoveSeries(this);
    }
}
