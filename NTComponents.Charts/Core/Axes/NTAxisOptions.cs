using SkiaSharp;

namespace NTComponents.Charts.Core.Axes;

/// <summary>
///     Base class for all chart axis options.
/// </summary>
public abstract class NTAxisOptions<TChartData> : IDisposable where TChartData : class {

    /// <summary>
    ///     Gets or sets the format string for labels.
    /// </summary>
    public string? LabelFormat { get; set; }

    /// <summary>
    ///     Gets or sets the scale used by the axis.
    /// </summary>
    public NTAxisScale Scale { get; set; } = NTAxisScale.Linear;

    /// <summary>
    ///     Gets or sets the title of the axis.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    ///     Gets or sets whether the axis is visible.
    /// </summary>
    public bool Visible { get; set; } = true;

    protected NTChart<TChartData> Chart { get; private set; } = default!;

    protected NTAxisOptions(NTChart<TChartData> chart) {
        Chart = chart;
    }

    protected NTAxisOptions() {
    }

    public abstract void Dispose();

    public abstract void Refresh();

    /// <summary>
    ///     Measures the axis and returns the remaining area.
    /// </summary>
    /// <param name="renderArea">The current available area.</param>
    /// <returns>The remaining area after the axis has taken its space.</returns>
    internal abstract SKRect Measure(SKRect renderArea);

    /// <summary>
    ///     Renders the axis on the canvas.
    /// </summary>
    /// <param name="canvas">   The canvas to render on.</param>
    /// <param name="plotArea"> The area of the chart content.</param>
    /// <param name="totalArea">The total area of the chart.</param>
    /// <param name="tickValues">The values to render as ticks on the axis.</param>
    internal abstract void Render(SKCanvas canvas, SKRect plotArea, SKRect totalArea, IEnumerable<double> tickValues);

    internal void SetChart(NTChart<TChartData> chart) {
        Chart = chart;
        Refresh();
    }
}