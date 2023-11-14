using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Layout;
public class TnTFooter : TnTHeader {
    [Parameter]
    public override string BaseCssClass { get; set; } = "tnt-footer";

    protected override void AddSelfToContext() {
        Context.Footer = this;
    }
}

