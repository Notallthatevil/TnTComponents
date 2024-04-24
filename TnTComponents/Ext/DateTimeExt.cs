using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Ext;
internal static class DateTimeExt {
    public static bool Overlaps(DateTime startA, DateTime EndA, DateTime startB, DateTime endB) {
        return startA < endB && EndA > startB;
    }
}

