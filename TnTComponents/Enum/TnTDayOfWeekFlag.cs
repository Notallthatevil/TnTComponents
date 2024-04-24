using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents;
[Flags]
public enum TnTDayOfWeekFlag {
    None = 0,
    Sunday = 1,
    Monday = 2,
    Tuesday = 4,
    Wednesday = 8,
    Thursday = 16,
    Friday = 32,
    Saturday = 64,
    All = Sunday | Monday | Tuesday | Wednesday | Thursday | Friday | Saturday
}

public static class TnTDayOfWeekFlagExt {
    public static bool HasDay(this TnTDayOfWeekFlag tnTDayOfWeek, DayOfWeek dayOfWeek) {
        return tnTDayOfWeek.HasFlag(dayOfWeek.ToTnTDayOfWeekFlag());
    }

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

