using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Core;

namespace TnTComponents.Scheduler.Infrastructure;
public abstract class TnTSchedulerBase : ComponentBase {
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    public ElementReference Element { get; protected set; }

    public string? CssClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-scheduler")
        .Build();

    public string? CssStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [Parameter]
    public DateTimeOffset? MaxDateTime { get; set; }

    [Parameter]
    public DateTimeOffset? MinDateTime { get; set; }

    [Parameter]
    public TimeOnly StartTime { get; set; } = new(0, 0);

    [Parameter]
    public TimeOnly EndTime { get; set; } = new(23, 59);


}
