using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Color;
public class TnTColor(string hexColor) {
    public string Value => ColorTranslator.ToHtml(_color);
    private System.Drawing.Color _color = ColorTranslator.FromHtml(hexColor);
}

