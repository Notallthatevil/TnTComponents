using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Enum;
public enum FlexDirection {
    Row,
    Column,
    RowReverse,
    ColumnReverse
}

public enum FlexWrap {
    Unspecified,
    NoWrap,
    Wrap
}

public enum FlexAlignContent {
    Normal,
    Center,
    Start,
    End,
    SpaceAround,
    SpaceBetween,
    Stretch
}

public enum FlexJustifyContent {
    Normal,
    Center,
    Start,
    End,
    SpaceAround,
    SpaceBetween,
    SpaceEvenly
}

public enum FlexAlignItems {
    Normal,
    Center,
    Start,
    End,
    Stretch,
    Baseline
}

internal static class FlexEnumExt {
    public static string ToStyle(this FlexDirection flexDirection) {
        return "flex-direction: " + flexDirection switch {
            FlexDirection.Row => "row;",
            FlexDirection.Column => "column;",
            FlexDirection.RowReverse => "row-reverse;",
            FlexDirection.ColumnReverse => "column-reverse;",
            _ => throw new NotImplementedException()
        };
    }

    public static string ToStyle(this FlexWrap flexWrap) {
        if(flexWrap == FlexWrap.Unspecified) {
            return string.Empty;
        }
        return "flex-wrap: " + flexWrap switch {
            FlexWrap.NoWrap => "nowrap;",
            FlexWrap.Wrap => "wrap;",
            _ => throw new NotImplementedException()
        };
    }

    public static string ToStyle(this FlexAlignContent flexAlignContent) {
        if (flexAlignContent == FlexAlignContent.Normal) {
            return string.Empty;
        }
        return "align-content: " + flexAlignContent switch {
            FlexAlignContent.Center => "center;",
            FlexAlignContent.Start => "flex-start;",
            FlexAlignContent.End => "flex-end;",
            FlexAlignContent.SpaceAround => "space-around;",
            FlexAlignContent.SpaceBetween => "space-between;",
            FlexAlignContent.Stretch => "stretch;",
            _ => throw new NotImplementedException()
        };
    }

    public static string ToStyle(this FlexJustifyContent flexJustifyContent) {
        if (flexJustifyContent == FlexJustifyContent.Normal) {
            return string.Empty;
        }
        return "justify-content: " + flexJustifyContent switch {
            FlexJustifyContent.Center => "center;",
            FlexJustifyContent.Start => "flex-start;",
            FlexJustifyContent.End => "flex-end;",
            FlexJustifyContent.SpaceAround => "space-around;",
            FlexJustifyContent.SpaceBetween => "space-between;",
            FlexJustifyContent.SpaceEvenly => "space-evenly;",
            _ => throw new NotImplementedException()
        };
    }

    public static string ToStyle(this FlexAlignItems flexAlignItems) {
        if (flexAlignItems == FlexAlignItems.Normal) {
            return string.Empty;
        }
        return "align-items: " + flexAlignItems switch {
            FlexAlignItems.Center => "center;",
            FlexAlignItems.Start => "flex-start;",
            FlexAlignItems.End => "flex-end;",
            FlexAlignItems.Stretch => "stretch;",
            FlexAlignItems.Baseline => "baseline;",
            _ => throw new NotImplementedException()
        };
    }
}
