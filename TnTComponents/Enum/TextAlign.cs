using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents;
public enum TextAlign {
    Left,
    Center,
    Right,
    Justify
}

public static class TextAlignExtensions {
    public static string ToCssString(this TextAlign? textAlign) {
        return textAlign switch {
            TextAlign.Left => "left",
            TextAlign.Center => "center",
            TextAlign.Right => "right",
            TextAlign.Justify => "justify",
            _ => string.Empty
        };
    }
}

