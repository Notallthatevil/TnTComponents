using Microsoft.AspNetCore.Components;
using NTComponents.Charts.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTComponents.Charts.Core.Series;

public abstract class NTCartesianSeries<TData> : NTBaseSeries<TData> where TData : class
{

    /// <inheritdoc />
    public override ChartCoordinateSystem CoordinateSystem => ChartCoordinateSystem.Cartesian;

    [Parameter, EditorRequired]
    public Func<TData, double> XValueSelector { get; set; } = default!;

    [Parameter, EditorRequired]
    public Func<TData, double> YValueSelector { get; set; } = default!;
}
