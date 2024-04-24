using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using TnTComponents.Core;
using TnTComponents.Scheduler;

namespace TnTComponents;

[CascadingTypeParameter(nameof(TEventType))]
public partial class TnTScheduler<TEventType> where TEventType : TnTEvent, new() {

    [Parameter]
    public ICollection<TEventType> Events { get; set; } = [];

    public void AddEvent(TEventType @event) {
        Events.Add(@event);
    }
    public void RemoveEvent(TEventType @event) {
        Events.Remove(@event);
    }


    [Parameter]
    public RenderFragment<TEventType>? EventTemplate { get; set; }

    protected override void OnParametersSet() {
        base.OnParametersSet();
        if (MaxDateTime is not null && MinDateTime is not null && MaxDateTime < MinDateTime) {
            throw new InvalidOperationException($"{nameof(MaxDateTime)} must be greater than {nameof(MinDateTime)}");
        }
        else if (MaxDateTime is not null && MinDateTime is null) {
            throw new InvalidOperationException($"{nameof(MinDateTime)} must be provided if {nameof(MaxDateTime)} is provided");
        }
        else if (MaxDateTime is null && MinDateTime is not null) {
            throw new InvalidOperationException($"{nameof(MaxDateTime)} must be provided if {nameof(MinDateTime)} is provided");
        }
    }
}
