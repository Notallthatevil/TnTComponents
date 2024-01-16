using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents;
public enum LayoutDirection {
    Vertical,
    Horizontal
}

public static class LayoutDirectionExt {
    public static string ToCssString(this LayoutDirection direction) {
        return direction switch {
            LayoutDirection.Vertical => "vertical",
            LayoutDirection.Horizontal => "horizontal",
            _ => throw new InvalidOperationException($"{direction} is not a valid value of {nameof(LayoutDirection)}")
        };
    }
}
