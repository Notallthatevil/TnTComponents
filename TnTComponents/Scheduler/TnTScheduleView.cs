using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Core;
using TnTComponents.Dialog;
using TnTComponents.Snackbar;

namespace TnTComponents.Scheduler;

public abstract class TnTScheduleView<TEventType> : TnTPageScriptComponent<TnTScheduleView<TEventType>> where TEventType : TnTEvent, new() {

    [CascadingParameter]
    public TnTScheduler<TEventType> Scheduler { get; set; } = default!;
    [Inject]
    protected TnTDialogService DialogService { get; set; } = default!;

    protected override void OnParametersSet() {
        base.OnParametersSet();
        if (Scheduler is null) {
            throw new ArgumentNullException(nameof(Scheduler), $"{GetType().Name} must be a child of {nameof(TnTScheduler<TEventType>)}");
        }
        if (DialogService is null) {
            throw new ArgumentNullException(nameof(TnTDialogService), $"{nameof(TnTDialogService)} was not able to be injected, did you remember to call {nameof(TnTServicesExt.AddTnTServices)} in your Program.cs?");
        }
    }

    protected static IEnumerable<TimeOnly> GetTimeSlots(TimeOnly startTime, TimeOnly endTime) {
        if (startTime >= endTime) {
            throw new InvalidOperationException($"{nameof(startTime)} cannot be must be less then {nameof(endTime)}");
        }
        if (startTime.Minute % 30 != 0) {
            throw new InvalidOperationException($"{nameof(startTime)} must be an interval of every half hour");
        }

        var timeSlots = new List<TimeOnly>();

        var currentTime = startTime;
        do {
            timeSlots.Add(currentTime);
            currentTime = currentTime.Add(TimeSpan.FromMinutes(30));

        }
        while (currentTime < endTime && startTime < currentTime);

        return timeSlots;
    }

    public void AddEvent(TEventType @event) {
        Scheduler.AddEvent(@event);
    }
}

