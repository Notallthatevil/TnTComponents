namespace TnTComponents;

/// <summary>
///     Specifies the alignment of content within a flex container.
/// </summary>
public enum AlignContent {

    /// <summary>
    ///     Represents the default alignment value where items are packed in their default position.
    /// </summary>
    Normal,

    /// <summary>
    ///     Centers items along the cross-axis with equal space before and after them.
    /// </summary>
    Center,

    /// <summary>
    ///     Places items at the start of the cross-axis.
    /// </summary>
    Start,

    /// <summary>
    ///     Places items at the end of the cross-axis.
    /// </summary>
    End,

    /// <summary>
    ///     Distributes items evenly with equal space around each line.
    /// </summary>
    SpaceAround,

    /// <summary>
    ///     Distributes items evenly with the first item at the start and the last item at the end.
    /// </summary>
    SpaceBetween,

    /// <summary>
    ///     Stretches items to fill the container along the cross-axis.
    /// </summary>
    Stretch
}

/// <summary>
///     Specifies the alignment of items within a flex container.
/// </summary>
public enum AlignItems {

    /// <summary>
    ///     Represents the default alignment value where items are packed in their default position.
    /// </summary>
    Normal,

    /// <summary>
    ///     Centers items along the cross-axis with equal space before and after them.
    /// </summary>
    Center,

    /// <summary>
    ///     Places items at the start of the cross-axis.
    /// </summary>
    Start,

    /// <summary>
    ///     Places items at the end of the cross-axis.
    /// </summary>
    End,

    /// <summary>
    ///     Stretches items to fill the container along the cross-axis.
    /// </summary>
    Stretch,

    /// <summary>
    ///     Aligns items such that their baselines align.
    /// </summary>
    Baseline
}

/// <summary>
///     Specifies the direction of the flex container's main axis.
/// </summary>
public enum FlexDirection {

    /// <summary>
    ///     The main axis of the flex container is defined to be the same as the text direction (left to right). The main start and end points correspond to the start and end of the writing mode.
    /// </summary>
    Row,

    /// <summary>
    ///     The main axis of the flex container is defined to be vertical (top to bottom). The main start and end points correspond to the start and end of the vertical direction.
    /// </summary>
    Column,

    /// <summary>
    ///     Same as Row but in the opposite direction (right to left). The main start and end points are swapped from the Row direction.
    /// </summary>
    RowReverse,

    /// <summary>
    ///     Same as Column but in the opposite direction (bottom to top). The main start and end points are swapped from the Column direction.
    /// </summary>
    ColumnReverse
}

/// <summary>
///     Specifies the alignment of flex items along the main axis of the container.
/// </summary>
public enum JustifyContent {

    /// <summary>
    ///     Represents the default alignment value where items are packed in their default position.
    /// </summary>
    Normal,

    /// <summary>
    ///     Centers items along the cross-axis with equal space before and after them.
    /// </summary>
    Center,

    /// <summary>
    ///     Places items at the start of the cross-axis.
    /// </summary>
    Start,

    /// <summary>
    ///     Places items at the end of the cross-axis.
    /// </summary>
    End,

    /// <summary>
    ///     Distributes items evenly with equal space around each line.
    /// </summary>
    SpaceAround,

    /// <summary>
    ///     Distributes items evenly with the first item at the start and the last item at the end.
    /// </summary>
    SpaceBetween,

    /// <summary>
    ///     Distributes items evenly with equal space between them.
    /// </summary>
    SpaceEvenly
}

/// <summary>
///     Specifies the alignment of a single flex item along the main axis of the container.
/// </summary>
public enum JustifySelf {

    /// <summary>
    ///     Places the item at the start of the main axis.
    /// </summary>
    Start,

    /// <summary>
    ///     Places the item at the end of the main axis.
    /// </summary>
    End,

    /// <summary>
    ///     Centers the item along the main axis.
    /// </summary>
    Center,

    /// <summary>
    ///     Stretches the item to fill the container along the main axis.
    /// </summary>
    Stretch
}

/// <summary>
///     Specifies whether flex items are forced into a single line or can be wrapped onto multiple lines.
/// </summary>
public enum WrapStyle {

    /// <summary>
    ///     Default value indicating that no specific wrap style has been specified.
    /// </summary>
    Unspecified,

    /// <summary>
    ///     Specifies that flex items are laid out in a single line and will not wrap.
    /// </summary>
    NoWrap,

    /// <summary>
    ///     Specifies that flex items will wrap onto multiple lines if needed.
    /// </summary>
    Wrap
}

/// <summary>
///     Provides extension methods for converting enum values to CSS strings and styles.
/// </summary>
public static class EnumExt {

    /// <summary>
    ///     Converts an <see cref="AlignItems" /> value to its corresponding CSS string.
    /// </summary>
    /// <param name="alignItems">The <see cref="AlignItems" /> value.</param>
    /// <returns>The corresponding CSS string.</returns>
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

    /// <summary>
    ///     Converts a <see cref="JustifyContent" /> value to its corresponding CSS string.
    /// </summary>
    /// <param name="justifyContent">The <see cref="JustifyContent" /> value.</param>
    /// <returns>The corresponding CSS string.</returns>
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

    /// <summary>
    ///     Converts an <see cref="AlignContent" /> value to its corresponding CSS string.
    /// </summary>
    /// <param name="alignContent">The <see cref="AlignContent" /> value.</param>
    /// <returns>The corresponding CSS string.</returns>
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

    /// <summary>
    ///     Converts a <see cref="FlexDirection" /> value to its corresponding CSS style.
    /// </summary>
    /// <param name="Direction">The <see cref="FlexDirection" /> value.</param>
    /// <returns>The corresponding CSS style.</returns>
    public static string ToStyle(this FlexDirection Direction) {
        return "flex-direction: " + Direction switch {
            FlexDirection.Row => "row;",
            FlexDirection.Column => "column;",
            FlexDirection.RowReverse => "row-reverse;",
            FlexDirection.ColumnReverse => "column-reverse;",
            _ => throw new NotImplementedException()
        };
    }

    /// <summary>
    ///     Converts a <see cref="WrapStyle" /> value to its corresponding CSS style.
    /// </summary>
    /// <param name="Wrap">The <see cref="WrapStyle" /> value.</param>
    /// <returns>The corresponding CSS style.</returns>
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

    /// <summary>
    ///     Converts an <see cref="AlignContent" /> value to its corresponding CSS style.
    /// </summary>
    /// <param name="AlignContent">The <see cref="AlignContent" /> value.</param>
    /// <returns>The corresponding CSS style.</returns>
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

    /// <summary>
    ///     Converts a <see cref="JustifyContent" /> value to its corresponding CSS style.
    /// </summary>
    /// <param name="JustifyContent">The <see cref="JustifyContent" /> value.</param>
    /// <returns>The corresponding CSS style.</returns>
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

    /// <summary>
    ///     Converts an <see cref="AlignItems" /> value to its corresponding CSS style.
    /// </summary>
    /// <param name="AlignItems">The <see cref="AlignItems" /> value.</param>
    /// <returns>The corresponding CSS style.</returns>
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

    /// <summary>
    ///     Converts a <see cref="JustifySelf" /> value to its corresponding CSS style.
    /// </summary>
    /// <param name="justifySelf">The <see cref="JustifySelf" /> value.</param>
    /// <returns>The corresponding CSS style.</returns>
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