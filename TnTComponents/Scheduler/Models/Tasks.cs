namespace BlazorCalendar.Models;

using System.Diagnostics;
using TnTComponents;

[DebuggerDisplay("{ID} {Code} {DateStart}")]
public sealed record class Tasks {
    public int ID { get; set; }
    public string? Key { get; set; }
    public string Caption { get; set; }
    public string Code { get; set; }
    public string Color { get; set; }
    public string? ForeColor { get; set; } = null;
    public FillStyleEnum FillStyle { get; set; }
    public string? Comment { get; set; } = null;
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public bool NotBeDraggable { get; set; }
    public int Type { get; set; }
    public TnTColor BackgroundColor { get; set; } = TnTColor.Tertiary;
    public TnTColor ForegroundColor { get; set; } = TnTColor.OnTertiary;
}

internal class EventComparer : IComparer<Tasks> {
    public int Compare(Tasks? x, Tasks? y) {
        if(x is null && y is null) {
            return 0;
        }

        if(x is not null && y is null) {
            return 1;
        }

        if(x is null && y is not null) {
            return -1;
        }

        if(x.DateStart == y.DateStart) {
            return x.DateEnd.CompareTo(y.DateEnd);
        }

        return x.DateStart.CompareTo(y.DateStart);
    }
}
