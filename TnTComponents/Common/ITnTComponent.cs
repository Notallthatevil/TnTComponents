using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Common;
public interface ITnTComponent {
    string BaseCssClass { get; set; }
    string Theme { get; set; }
    IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    string GetCssClass();

}

