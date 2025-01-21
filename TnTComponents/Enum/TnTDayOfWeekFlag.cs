using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents;
/// <summary>
/// Represents the days of the week as a flag enumeration.
/// </summary>
[Flags]
public enum TnTDayOfWeekFlag {
    /// <summary>
    /// No day selected.
    /// </summary>
    None = 0,
    /// <summary>
    /// Represents Sunday.
    /// </summary>
    Sunday = 1,
    /// <summary>
    /// Represents Monday.
    /// </summary>
    Monday = 2,
    /// <summary>
    /// Represents Tuesday.
    /// </summary>
    Tuesday = 4,
    /// <summary>
    /// Represents Wednesday.
    /// </summary>
    Wednesday = 8,
    /// <summary>
    /// Represents Thursday.
    /// </summary>
    Thursday = 16,
    /// <summary>
    /// Represents Friday.
    /// </summary>
    Friday = 32,
    /// <summary>
    /// Represents Saturday.
    /// </summary>
    Saturday = 64,
    /// <summary>
    /// Represents all days of the week.
    /// </summary>
    All = Sunday | Monday | Tuesday | Wednesday | Thursday | Friday | Saturday
}

/// <summary>
/// Extension methods for the TnTDayOfWeekFlag enumeration.
/// </summary>
public static class TnTDayOfWeekFlagExt {
    /// <summary>
    /// Determines whether the specified TnTDayOfWeekFlag includes the given day of the week.
    /// </summary>
    /// <param name="tnTDayOfWeek">The TnTDayOfWeekFlag to check.</param>
    /// <param name="dayOfWeek">The day of the week to check for.</param>
    /// <returns>True if the TnTDayOfWeekFlag includes the specified day of the week; otherwise, false.</returns>
    public static bool HasDay(this TnTDayOfWeekFlag tnTDayOfWeek, DayOfWeek dayOfWeek) {
        return tnTDayOfWeek.HasFlag(dayOfWeek.ToTnTDayOfWeekFlag());
    }

    /// <summary>
    /// Converts a DayOfWeek value to the corresponding TnTDayOfWeekFlag value.
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

