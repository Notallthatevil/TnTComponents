﻿using TnTComponents.Scheduler.Infrastructure;

namespace TnTComponents.Scheduler;

/// <summary>
///     Represents an abstract base class for defining disabled date and time rules.
/// </summary>
public abstract class TnTDisabledDateTime {

    /// <summary>
    ///     Creates an instance representing a disabled date.
    /// </summary>
    /// <param name="date">The date to be disabled.</param>
    /// <returns>
    ///     An instance of <see cref="TnTDisabledDateTime" /> representing the disabled date.
    /// </returns>
    public static TnTDisabledDateTime DisabledDate(DateOnly date) => new DisabledDateOnly(date);

    /// <summary>
    ///     Creates an instance representing a disabled date range.
    /// </summary>
    /// <param name="disabledStart">The start of the disabled date range.</param>
    /// <param name="disabledEnd">  The end of the disabled date range.</param>
    /// <returns>
    ///     An instance of <see cref="TnTDisabledDateTime" /> representing the disabled date range.
    /// </returns>
    public static TnTDisabledDateTime DisabledDateRange(DateTimeOffset disabledStart, DateTimeOffset disabledEnd) => new DisabledDateRange(disabledStart, disabledEnd);

    /// <summary>
    ///     Creates an instance representing a disabled day of the week with a specific time range.
    /// </summary>
    /// <param name="dayOfWeekFlag">    The day of the week to be disabled.</param>
    /// <param name="disabledStartTime">The start time of the disabled period.</param>
    /// <param name="disabledEndTime">  The end time of the disabled period.</param>
    /// <returns>
    ///     An instance of <see cref="TnTDisabledDateTime" /> representing the disabled day of the
    ///     week with the specified time range.
    /// </returns>
    public static TnTDisabledDateTime DisabledDayOfWeek(TnTDayOfWeekFlag dayOfWeekFlag, TimeOnly disabledStartTime, TimeOnly disabledEndTime) => new DisabledDayOfWeek(dayOfWeekFlag, disabledStartTime, disabledEndTime);

    /// <summary>
    ///     Determines whether the specified date and time is disabled.
    /// </summary>
    /// <param name="dateTimeOffset">The date and time to check.</param>
    /// <returns><c>true</c> if the specified date and time is disabled; otherwise, <c>false</c>.</returns>
    internal abstract bool IsDateTimeDisabled(DateTimeOffset dateTimeOffset);

    /// <summary>
    ///     Determines whether the specified date is disabled.
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <returns><c>true</c> if the specified date is disabled; otherwise, <c>false</c>.</returns>
    internal abstract bool IsDayDisabled(DateOnly date);

    /// <summary>
    ///     Determines whether the specified time slot on a given day of the week is disabled.
    /// </summary>
    /// <param name="dayOfWeek">The day of the week to check.</param>
    /// <param name="timeSlot"> The time slot to check.</param>
    /// <returns>
    ///     <c>true</c> if the specified time slot on the given day of the week is disabled;
    ///     otherwise, <c>false</c>.
    /// </returns>
    internal abstract bool IsTimeSlotDisabled(DayOfWeek dayOfWeek, TimeOnly timeSlot);
}