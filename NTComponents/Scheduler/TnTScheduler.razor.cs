using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics.CodeAnalysis;
using NTComponents.Core;
using NTComponents.Scheduler;
using NTComponents.Scheduler.Infrastructure;

namespace NTComponents;

/// <summary>
///     Represents the days of the week as a flag enumeration.
/// </summary>
[Flags]
public enum TnTDayOfWeekFlag {

    /// <summary>
    ///     No day selected.
    /// </summary>
    None = 0,

    /// <summary>
    ///     Represents Sunday.
    /// </summary>
    Sunday = 1,

    /// <summary>
    ///     Represents Monday.
    /// </summary>
    Monday = 2,

    /// <summary>
    ///     Represents Tuesday.
    /// </summary>
    Tuesday = 4,

    /// <summary>
    ///     Represents Wednesday.
    /// </summary>
    Wednesday = 8,

    /// <summary>
    ///     Represents Thursday.
    /// </summary>
    Thursday = 16,

    /// <summary>
    ///     Represents Friday.
    /// </summary>
    Friday = 32,

    /// <summary>
    ///     Represents Saturday.
    /// </summary>
    Saturday = 64,

    /// <summary>
    ///     Represents all days of the week.
    /// </summary>
    All = Sunday | Monday | Tuesday | Wednesday | Thursday | Friday | Saturday
}

/// <summary>
///     Represents a scheduler component for managing and displaying events.
/// </summary>
/// <typeparam name="TEventType">The type of the event.</typeparam>
[CascadingTypeParameter(nameof(TEventType))]
public partial class TnTScheduler< TEventType> : TnTComponentBase where TEventType : TnTEvent {

    /// <summary>
    ///     Gets or sets a value indicating whether dragging events is allowed.
    /// </summary>
    [Parameter]
    public bool AllowDraggingEvents { get; set; } = true;

    /// <summary>
    ///     Gets or sets the background color of the scheduler.
    /// </summary>
    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.SurfaceContainerLow;

    /// <summary>
    ///     Gets or sets the child content to be rendered inside the scheduler.
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    /// <summary>
    ///     Gets or sets the date displayed by the scheduler.
    /// </summary>
    [Parameter]
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    /// <summary>
    ///     Gets or sets the callback invoked when the date changes.
    /// </summary>
    [Parameter]
    public EventCallback<DateOnly> DateChangedCallback { get; set; }

    /// <summary>
    ///     Gets the day of the week for the current date.
    /// </summary>
    public DayOfWeek DayOfWeek => Date.DayOfWeek;

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-scheduler")
        .AddFilled()
        .AddBackgroundColor(BackgroundColor)
        .AddForegroundColor(TextColor)
        .Build();

    /// <inheritdoc />
    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    /// <summary>
    ///     Gets or sets the callback invoked when an event is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<TEventType> EventClickedCallback { get; set; }

    /// <summary>
    ///     Gets or sets the collection of events to be displayed in the scheduler.
    /// </summary>
    [Parameter, EditorRequired]
    public ICollection<TEventType> Events { get; set; } = [];

    /// <summary>
    ///     Gets or sets the callback invoked when an event slot is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<DateTimeOffset> EventSlotClickedCallback { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the date controls should be hidden.
    /// </summary>
    [Parameter]
    public bool HideDateControls { get; set; }

    /// <summary>
    ///     Gets or sets the background color for placeholders.
    /// </summary>
    [Parameter]
    public TnTColor PlaceholderBackgroundColor { get; set; } = TnTColor.SecondaryContainer;

    /// <summary>
    ///     Gets or sets the text color for placeholders.
    /// </summary>
    [Parameter]
    public TnTColor PlaceholderTextColor { get; set; } = TnTColor.OnSecondaryContainer;

    /// <summary>
    ///     Gets or sets the text color for the scheduler.
    /// </summary>
    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnSurface;

    private readonly Dictionary<Type, ScheduleViewBase<TEventType>> _scheduleViews = [];
    private ScheduleViewBase<TEventType>? _selectedView;

    /// <summary>
    ///     Adds a schedule view to the scheduler.
    /// </summary>
    /// <param name="scheduleView">The schedule view to add.</param>
    public void AddScheduleView(ScheduleViewBase<TEventType> scheduleView) {
        _scheduleViews[scheduleView.GetType()] = scheduleView;
        if (_scheduleViews.Count == 1) {
            _selectedView = scheduleView;
        }
    }

    /// <summary>
    ///     Gets the first visible date in the selected view.
    /// </summary>
    /// <returns>The first visible date.</returns>
    public DateOnly? GetFirstVisibleDate() => _selectedView?.GetFirstVisibleDate();

    /// <summary>
    ///     Gets the last visible date in the selected view.
    /// </summary>
    /// <returns>The last visible date.</returns>
    public DateOnly? GetLastVisibleDate() => _selectedView?.GetLastVisibleDate();

    /// <summary>
    ///     Determines whether the specified view is selected.
    /// </summary>
    /// <param name="scheduleView">The schedule view to check.</param>
    /// <returns><c>true</c> if the view is selected; otherwise, <c>false</c>.</returns>
    public bool IsViewSelected(ScheduleViewBase<TEventType> scheduleView) => _selectedView == scheduleView;

    /// <summary>
    ///     Refreshes the scheduler and the selected view.
    /// </summary>
    public void Refresh() {
        StateHasChanged();
        _selectedView?.Refresh();
    }

    /// <summary>
    ///     Removes a schedule view from the scheduler.
    /// </summary>
    /// <param name="scheduleView">The schedule view to remove.</param>
    public void RemoveScheduleView(ScheduleViewBase<TEventType> scheduleView) => _scheduleViews.Remove(scheduleView.GetType());

    /// <summary>
    ///     Updates the scheduler to display today's date.
    /// </summary>
    private Task GoToToday() => UpdateDate(DateOnly.FromDateTime(DateTimeOffset.Now.LocalDateTime));

    /// <summary>
    ///     Advances the scheduler to the next page of dates.
    /// </summary>
    private Task NextPage() => UpdateDate(_selectedView?.IncrementDate(Date));

    /// <summary>
    ///     Moves the scheduler to the previous page of dates.
    /// </summary>
    private Task PreviousPage() => UpdateDate(_selectedView?.DecrementDate(Date));

    /// <summary>
    ///     Updates the scheduler to display the specified date.
    /// </summary>
    /// <param name="date">The date to display.</param>
    private async Task UpdateDate(DateOnly? date) {
        if (date.HasValue) {
            Date = date.Value;
            _selectedView?.Refresh();
            await DateChangedCallback.InvokeAsync(date.Value);
        }
    }
}

/// <summary>
///     Extension methods for the TnTDayOfWeekFlag enumeration.
/// </summary>
public static class TnTDayOfWeekFlagExt {

    /// <summary>
    ///     Determines whether the specified TnTDayOfWeekFlag includes the given day of the week.
    /// </summary>
    /// <param name="tnTDayOfWeek">The TnTDayOfWeekFlag to check.</param>
    /// <param name="dayOfWeek">   The day of the week to check for.</param>
    /// <returns>True if the TnTDayOfWeekFlag includes the specified day of the week; otherwise, false.</returns>
    public static bool HasDay(this TnTDayOfWeekFlag tnTDayOfWeek, DayOfWeek dayOfWeek) => tnTDayOfWeek.HasFlag(dayOfWeek.ToTnTDayOfWeekFlag());

    /// <summary>
    ///     Converts a DayOfWeek value to the corresponding TnTDayOfWeekFlag value.
    /// </summary>
    /// <param name="dayOfWeek">The DayOfWeek value to convert.</param>
    /// <returns>The corresponding TnTDayOfWeekFlag value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified day of the week is not valid.</exception>
    public static TnTDayOfWeekFlag ToTnTDayOfWeekFlag(this DayOfWeek dayOfWeek) {
        return dayOfWeek switch {
            DayOfWeek.Sunday => TnTDayOfWeekFlag.Sunday,
            DayOfWeek.Monday => TnTDayOfWeekFlag.Monday,
            DayOfWeek.Tuesday => TnTDayOfWeekFlag.Tuesday,
            DayOfWeek.Wednesday => TnTDayOfWeekFlag.Wednesday,
            DayOfWeek.Thursday => TnTDayOfWeekFlag.Thursday,
            DayOfWeek.Friday => TnTDayOfWeekFlag.Friday,
            DayOfWeek.Saturday => TnTDayOfWeekFlag.Saturday,
            _ => throw new ArgumentOutOfRangeException(nameof(dayOfWeek), dayOfWeek, null)
        };
    }
}