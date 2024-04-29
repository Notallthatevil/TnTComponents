using Microsoft.AspNetCore.Components;
using TnTComponents.Core;
using TnTComponents.Scheduler;
using TnTComponents.Scheduler.Infrastructure;

namespace TnTComponents;

[CascadingTypeParameter(nameof(TEventType))]
public partial class TnTScheduler<TEventType> where TEventType : TnTEvent {

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    public string? CssClass => CssClassBuilder.Create()
            .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-scheduler")
        .Build();

    public string? CssStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    [Parameter]
    public bool DisableDragAndDrop { get; set; }

    public ElementReference Element { get; private set; }

    [Parameter, EditorRequired]
    public ICollection<TEventType> Events { get; set; } = [];

    [Parameter]
    public DateTime FirstDate { get; set; }

    private IDictionary<Type, ScheduleViewBase<TEventType>> _scheduleViews = new Dictionary<Type, ScheduleViewBase<TEventType>>();
    private ScheduleViewBase<TEventType>? _selectedView;

    public void AddScheduleView(ScheduleViewBase<TEventType> scheduleView) {
        _scheduleViews[scheduleView.GetType()] = scheduleView;
        if (_scheduleViews.Count == 1) {
            _selectedView = scheduleView;
        }
    }

    public bool IsViewSelected(ScheduleViewBase<TEventType> scheduleView) {
        return _selectedView == scheduleView;
    }

    public void RemoveScheduleView(ScheduleViewBase<TEventType> scheduleView) {
        _scheduleViews.Remove(scheduleView.GetType());
    }
}