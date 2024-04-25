using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Core;

namespace TnTComponents.Scheduler.Infrastructure;
public abstract class ScheduleViewBase<TEventType> : ComponentBase, ITnTComponentBase where TEventType : TnTEvent {
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }
    [Parameter] public bool? AutoFocus { get; set; }

    public string? CssClass => null;
    [Parameter]
    public bool Disabled { get; set; }

    [CascadingParameter]
    protected TnTScheduler<TEventType> Scheduler { get; set; } = default!;

    public ElementReference Element { get; protected set; }

    public string? Id { get; private set; }

    public string? CssStyle => null;
    [Parameter]
    public string HeaderStyle { get; set; }
    protected override void OnInitialized() {
        base.OnInitialized();
        Id ??= TnTComponentIdentifier.NewId();
    }

    protected override void OnParametersSet() {
        base.OnParametersSet();
        if(Scheduler is null) {
            throw new InvalidOperationException($"{GetType().Name} requires a parent of type {nameof(TnTScheduler<TEventType>)}.");
        }
    }
}

