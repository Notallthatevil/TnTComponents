﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Ext;
internal static class DateTimeExt {
    public static bool Overlaps(DateTime startA, DateTime EndA, DateTime startB, DateTime endB) {
        return startA < endB && EndA > startB;
    }

    public static TimeOnly RoundToNearestMinuteInterval(this TimeOnly time, TimeSpan interval) {
        return new TimeOnly(time.Hour, (int)(Math.Round(time.Minute / (double)interval.Minutes) * interval.Minutes));
    }

    public static DateTime ToDateTime(this DateOnly date) {
        return new DateTime(date, default);
    }
}

