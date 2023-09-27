using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Common {
    public class TnTBaseComponent : ComponentBase {

        [Parameter]
        public string Class { get; set; } = string.Empty;

        [Parameter]
        public virtual string Theme { get; set; } = "default";

    }
}
