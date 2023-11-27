namespace TnTComponents.Enum;

public enum Direction {
    Row,
    Column,
    RowReverse,
    ColumnReverse
}

public enum WrapStyle {
    Unspecified,
    NoWrap,
    Wrap
}

public enum AlignContent {
    Normal,
    Center,
    Start,
    End,
    SpaceAround,
    SpaceBetween,
    Stretch
}

public enum JustifyContent {
    Normal,
    Center,
    Start,
    End,
    SpaceAround,
    SpaceBetween,
    SpaceEvenly
}

public enum AlignItems {
    Normal,
    Center,
    Start,
    End,
    Stretch,
    Baseline
}

public enum JustifySelf {
    Start,
    End,
    Center,
    Stretch
}

internal static class EnumExt {

    public static string ToStyle(this Direction Direction) {
        return "flex-direction: " + Direction switch {
            Direction.Row => "row;",
            Direction.Column => "column;",
            Direction.RowReverse => "row-reverse;",
            Direction.ColumnReverse => "column-reverse;",
            _ => throw new NotImplementedException()
        };
    }

    public static string ToStyle(this WrapStyle Wrap) {
        if (Wrap == WrapStyle.Unspecified) {
            return string.Empty;
        }
        return "flex-wrap: " + Wrap switch {
            WrapStyle.NoWrap => "nowrap;",
            WrapStyle.Wrap => "wrap;",
            _ => throw new NotImplementedException()
        };
    }

    public static string ToStyle(this AlignContent AlignContent) {
        if (AlignContent == AlignContent.Normal) {
            return string.Empty;
        }
        return "align-content: " + AlignContent switch {
            AlignContent.Center => "center;",
            AlignContent.Start => "-start;",
            AlignContent.End => "-end;",
            AlignContent.SpaceAround => "space-around;",
            AlignContent.SpaceBetween => "space-between;",
            AlignContent.Stretch => "stretch;",
            _ => throw new NotImplementedException()
        };
    }

    public static string ToStyle(this JustifyContent JustifyContent) {
        if (JustifyContent == JustifyContent.Normal) {
            return string.Empty;
        }
        return "justify-content: " + JustifyContent switch {
            JustifyContent.Center => "center;",
            JustifyContent.Start => "-start;",
            JustifyContent.End => "-end;",
            JustifyContent.SpaceAround => "space-around;",
            JustifyContent.SpaceBetween => "space-between;",
            JustifyContent.SpaceEvenly => "space-evenly;",
            _ => throw new NotImplementedException()
        };
    }

    public static string ToStyle(this AlignItems AlignItems) {
        if (AlignItems == AlignItems.Normal) {
            return string.Empty;
        }
        return "align-items: " + AlignItems switch {
            AlignItems.Center => "center;",
            AlignItems.Start => "-start;",
            AlignItems.End => "-end;",
            AlignItems.Stretch => "stretch;",
            AlignItems.Baseline => "baseline;",
            _ => throw new NotImplementedException()
        };
    }

    public static string ToStyle(this JustifySelf justifySelf) {
        return "justify-self: " + justifySelf switch {
            JustifySelf.Start => "start;",
            JustifySelf.End => "end;",
            JustifySelf.Center => "center;",
            JustifySelf.Stretch => "stretch;",
            _ => throw new NotImplementedException()
        };
    }
}