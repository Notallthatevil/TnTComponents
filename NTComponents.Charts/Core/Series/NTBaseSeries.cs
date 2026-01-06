using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTComponents.Charts.Core.Series;

public abstract class NTBaseSeries<TData> : ComponentBase, IDisposable where TData : class {

    [CascadingParameter]
    protected NTChart<TData> Chart { get; set; } = default!;

    override protected void OnInitialized() {
        base.OnInitialized();
        if (Chart is null) {
            throw new ArgumentNullException(nameof(Chart), $"Series must be used within a {nameof(NTChart<TData>)}.");
        }
        Chart.AddSeries(this);
    }


    public void Dispose() {
        Chart?.RemoveSeries(this);
    }
}
