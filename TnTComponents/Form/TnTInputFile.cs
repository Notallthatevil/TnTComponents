using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Core;

namespace TnTComponents;
public class TnTInputFile : InputFile {

    public string FormCssClass => CssClassBuilder.Create()
        .Build();

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "span");
        builder.AddAttribute(10, "class", FormCssClass);
        {
            builder.OpenRegion(20);
            base.BuildRenderTree(builder);
            builder.CloseRegion();
        }

        builder.CloseElement();
    }
}

