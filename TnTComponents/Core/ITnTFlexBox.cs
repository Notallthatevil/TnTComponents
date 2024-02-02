using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Core;
public interface ITnTFlexBox {
    LayoutDirection? Direction { get; set; }
    JustifyContent? JustifyContent { get; set; }
    AlignItems? AlignItems { get; set; }
    AlignContent? AlignContent { get; set; }
}

