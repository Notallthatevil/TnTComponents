using Microsoft.AspNetCore.Components;
using TnTComponents.Core;
using TnTComponents.Ext;
using TnTComponents.Interfaces;
using TnTComponents.Scheduler.Events;

namespace TnTComponents.Scheduler.Infrastructure;

public abstract class ScheduleViewBase<TEventType> : TnTComponentBase, IDisposable where TEventType : TnTEvent {

    public abstract DateOnly IncrementDate(DateOnly src);
    public abstract DateOnly DecrementDate(DateOnly src);
    [CascadingParameter]
    protected TnTScheduler<TEventType> Scheduler { get; private set; } = default!;

    protected override void OnInitialized() {
        base.OnInitialized();

        if (Scheduler is null) {
            throw new InvalidOperationException($"A {GetType().Name} must be used within a {nameof(TnTScheduler<TEventType>)} component.");
        }

        Scheduler.AddScheduleView(this);
    }

    public void Dispose() {
        Scheduler.RemoveScheduleView(this);
    }

    public abstract void Refresh();
}