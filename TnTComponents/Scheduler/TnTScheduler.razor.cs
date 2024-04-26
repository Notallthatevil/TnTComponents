using BlazorCalendar;
using BlazorCalendar.Models;
using Microsoft.AspNetCore.Components;
using TnTComponents.Core;
using TnTComponents.Scheduler;
using TnTComponents.Scheduler.Infrastructure;

namespace TnTComponents;

[CascadingTypeParameter(nameof(TEventType))]
partial class TnTScheduler<TEventType> : CalendarBase where TEventType : TnTEvent {

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    public string? CssClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-scheduler")
        .Build();

    public string? CssStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    public ElementReference Element { get; private set; }



    private IDictionary<Type, ScheduleViewBase<TEventType>> _scheduleViews = new Dictionary<Type, ScheduleViewBase<TEventType>>();
    private ScheduleViewBase<TEventType>? _selectedView;


    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public DateTime FirstDate { get; set; }

    [Parameter]
    public DisplayedView DisplayedView { get; set; }

    [Parameter]
    public Tasks[] TasksList { get; set; } = [];

    [Parameter]
    public bool DisableDragAndDrop { get; set; }


    public void AddScheduleView(ScheduleViewBase<TEventType> scheduleView) {
        _scheduleViews[scheduleView.GetType()] = scheduleView;
        if(_scheduleViews.Count == 1) {
            _selectedView = scheduleView;
        }
    }

    public void RemoveScheduleView(ScheduleViewBase<TEventType> scheduleView) {
        _scheduleViews.Remove(scheduleView.GetType());
    }

    public bool IsViewSelected(ScheduleViewBase<TEventType> scheduleView) {
        return _selectedView == scheduleView;
    }



}
