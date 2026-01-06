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
    ///     Gets the coordinate system used by this series.
    /// </summary>
    public abstract ChartCoordinateSystem CoordinateSystem { get; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        if (Chart is null)
        {
            throw new ArgumentNullException(nameof(Chart), $"Series must be used within a {nameof(NTChart<TData>)}.");
        }
        Chart.AddSeries(this);
    }

    public abstract void Render(SKCanvas canvas, SKRect renderArea);

    public void Dispose()
    {
        Chart?.RemoveSeries(this);
    }
}
