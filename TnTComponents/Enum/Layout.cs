namespace TnTComponents;

public enum AlignContent {
    Normal,
    Center,
    Start,
    End,
    SpaceAround,
    SpaceBetween,
    Stretch
}

public enum AlignItems {
    Normal,
    Center,
    Start,
    End,
    Stretch,
    Baseline
}

public enum FlexDirection {
    Row,
    Column,
    RowReverse,
    ColumnReverse
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

public enum JustifySelf {
    Start,
    End,
    Center,
    Stretch
}

public enum WrapStyle {
    Unspecified,
    NoWrap,
    Wrap
}

public static class EnumExt {
    public static string ToCssString(this AlignItems alignItems) {
        return alignItems switch {
            AlignItems.Normal => "normal",
            AlignItems.Center => "center",
            AlignItems.Start => "start",
            AlignItems.End => "end",
            AlignItems.Stretch => "stretch",
            AlignItems.Baseline => "baseline",
            _ => throw new InvalidOperationException($"{alignItems} is not a valid value of {nameof(AlignItems)}")
        };
    }

    public static string ToCssString(this JustifyContent justifyContent) {
        return justifyContent switch {
            JustifyContent.Normal => "normal",
            JustifyContent.Center => "center",
            JustifyContent.Start => "start",
            JustifyContent.End => "end",
            JustifyContent.SpaceAround => "space-around",
            JustifyContent.SpaceBetween => "space-between",
            JustifyContent.SpaceEvenly => "space-evenly",
            _ => throw new InvalidOperationException($"{justifyContent} is not a valid value of {nameof(JustifyContent)}")
        };
    }

    public static string ToCssString(this AlignContent alignContent) {
        return alignContent switch {
            AlignContent.Normal => "normal",
            AlignContent.Center => "center",
            AlignContent.Start => "start",
            AlignContent.End => "end",
            AlignContent.SpaceAround => "space-around",
            AlignContent.SpaceBetween => "space-between",
            AlignContent.Stretch => "stretch",
            _ => throw new InvalidOperationException($"{alignContent} is not a valid value of {nameof(AlignContent)}")
        };
    }


    public static string ToStyle(this FlexDirection Direction) {
        return "flex-direction: " + Direction switch {
            FlexDirection.Row => "row;",
            FlexDirection.Column => "column;",
            FlexDirection.RowReverse => "row-reverse;",
            FlexDirection.ColumnReverse => "column-reverse;",
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