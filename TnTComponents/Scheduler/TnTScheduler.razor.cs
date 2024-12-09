using Microsoft.AspNetCore.Components;
using TnTComponents.Core;
using TnTComponents.Scheduler;
using TnTComponents.Scheduler.Infrastructure;

namespace TnTComponents;

[CascadingTypeParameter(nameof(TEventType))]
public partial class TnTScheduler<TEventType> where TEventType : TnTEvent {

    [Parameter]
    public bool AllowDraggingEvents { get; set; } = true;

    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.SurfaceContainerLow;

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [Parameter]
    public EventCallback<DateOnly> DateChangedCallback { get; set; }

    public DayOfWeek DayOfWeek => Date.DayOfWeek;

    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-scheduler")
        .AddFilled()
        .AddBackgroundColor(BackgroundColor)
        .AddForegroundColor(TextColor)
        .Build();

    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    [Parameter]
    public EventCallback<TEventType> EventClickedCallback { get; set; }

    [Parameter, EditorRequired]
    public ICollection<TEventType> Events { get; set; } = [];

    [Parameter]
    public EventCallback<DateTimeOffset> EventSlotClickedCallback { get; set; }

    [Parameter]
    public bool HideDateControls { get; set; }

    [Parameter]
    public TnTColor PlaceholderBackgroundColor { get; set; } = TnTColor.SecondaryContainer;

    [Parameter]
    public TnTColor PlaceholderTextColor { get; set; } = TnTColor.OnSecondaryContainer;

    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnSurface;

    [Inject]
    private ITnTDialogService _dialogService { get; set; } = default!;

    private IDictionary<Type, ScheduleViewBase<TEventType>> _scheduleViews = new Dictionary<Type, ScheduleViewBase<TEventType>>();
    private ScheduleViewBase<TEventType>? _selectedView;

    public void AddScheduleView(ScheduleViewBase<TEventType> scheduleView) {
        _scheduleViews[scheduleView.GetType()] = scheduleView;
        if (_scheduleViews.Count == 1) {
            _selectedView = scheduleView;
        }
    }

    public DateOnly? GetFirstVisibleDate() => _selectedView?.GetFirstVisibleDate();

    public DateOnly? GetLastVisibleDate() => _selectedView?.GetLastVisibleDate();

    public bool IsViewSelected(ScheduleViewBase<TEventType> scheduleView) => _selectedView == scheduleView;

    public void Refresh() {
        StateHasChanged();
        _selectedView?.Refresh();
    }

    public void RemoveScheduleView(ScheduleViewBase<TEventType> scheduleView) => _scheduleViews.Remove(scheduleView.GetType());

    private Task GoToToday() => UpdateDate(DateOnly.FromDateTime(DateTimeOffset.Now.LocalDateTime));

    private Task NextPage() => UpdateDate(_selectedView?.IncrementDate(Date));

    private Task PreviousPage() => UpdateDate(_selectedView?.DecrementDate(Date));

    private async Task UpdateDate(DateOnly? date) {
        if (date.HasValue) {
            Date = date.Value;
            _selectedView?.Refresh();
            await DateChangedCallback.InvokeAsync(date.Value);
        }
    }
}
