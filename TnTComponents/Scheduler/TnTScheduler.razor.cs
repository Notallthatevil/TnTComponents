﻿using Microsoft.AspNetCore.Components;
using TnTComponents.Core;
using TnTComponents.Scheduler;
using TnTComponents.Scheduler.Infrastructure;

namespace TnTComponents;

[CascadingTypeParameter(nameof(TEventType))]
public partial class TnTScheduler<TEventType> where TEventType : TnTEvent {

    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.SurfaceContainerLow;

    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnSurface;

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public EventCallback DateChangedCallback { get; set; }

    [Parameter]
    public IEnumerable<TnTDisabledDateTime> DisabledDateTimes { get; set; } = [];

    [Parameter]
    public bool DisableDragAndDrop { get; set; }

    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-scheduler")
        .AddBackgroundColor(BackgroundColor)
        .AddForegroundColor(TextColor)
        .Build();

    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    [Parameter, EditorRequired]
    public ICollection<TEventType> Events { get; set; } = [];

    [Parameter]
    public bool HideDateControls { get; set; }

    [Parameter]
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    public DayOfWeek DayOfWeek => Date.DayOfWeek;


    private IDictionary<Type, ScheduleViewBase<TEventType>> _scheduleViews = new Dictionary<Type, ScheduleViewBase<TEventType>>();
    private ScheduleViewBase<TEventType>? _selectedView;

    public void AddScheduleView(ScheduleViewBase<TEventType> scheduleView) {
        _scheduleViews[scheduleView.GetType()] = scheduleView;
        if (_scheduleViews.Count == 1) {
            _selectedView = scheduleView;
        }
    }

    public DateOnly? GetFirstVisibleDate() {
        return _selectedView?.GetVisibleDates().First();
    }

    public DateOnly? GetLastVisibleDate() {
        return _selectedView?.GetVisibleDates().Last();
    }

    public bool IsViewSelected(ScheduleViewBase<TEventType> scheduleView) {
        return _selectedView == scheduleView;
    }

    public void RemoveScheduleView(ScheduleViewBase<TEventType> scheduleView) {
        _scheduleViews.Remove(scheduleView.GetType());
    }

    private async Task GoToToday() {
        await UpdateDate(DateOnly.FromDateTime(DateTime.Today));
    }

    private async Task NextPage() {
        await UpdateDate(_selectedView?.IncrementPage(Date));
    }

    private async Task PreviousPage() {
        await UpdateDate(_selectedView?.DecrementPage(Date));
    }

    private async Task UpdateDate(DateOnly? date) {
        if (date.HasValue) {
            StateHasChanged();
            _selectedView?.Refresh();
            await DateChangedCallback.InvokeAsync();
        }
    }
}