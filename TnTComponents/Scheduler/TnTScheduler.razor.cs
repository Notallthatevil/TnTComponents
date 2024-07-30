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
    public TnTColor BackgroundColor { get; set; } = TnTColor.Surface;

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    public string? ElementClass => CssClassBuilder.Create()
            .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-scheduler")
        .AddBackgroundColor(BackgroundColor)
        .AddForegroundColor(TextColor)
        .Build();

    public string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    [Parameter]
    public EventCallback DateChangedCallback { get; set; }

    [Parameter]
    public IEnumerable<TnTDisabledDateTime> DisabledDateTimes { get; set; } = [];

    [Parameter]
    public bool DisableDragAndDrop { get; set; }

    [Parameter]
#pragma warning disable BL0007 // Component parameters should be auto properties
    public DateOnly DisplayedDate { get => _displayDate ?? DateOnly.FromDateTime(DateTime.Today); set => _displayDate = value; }
#pragma warning restore BL0007 // Component parameters should be auto properties

    public ElementReference Element { get; private set; }

    [Parameter, EditorRequired]
    public ICollection<TEventType> Events { get; set; } = [];

    [Parameter]
    public bool HideDateControls { get; set; }

    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnSurface;

    private DateOnly? _displayDate = DateOnly.FromDateTime(DateTime.Today);

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
        await UpdateDate(_selectedView?.IncrementPage(DisplayedDate));
    }

    private async Task PreviousPage() {
        await UpdateDate(_selectedView?.DecrementPage(DisplayedDate));
    }

    private async Task UpdateDate(DateOnly? date) {
        if (date.HasValue) {
            DisplayedDate = date.Value;
            StateHasChanged();
            _selectedView?.Refresh();
            await DateChangedCallback.InvokeAsync();
        }
    }
}