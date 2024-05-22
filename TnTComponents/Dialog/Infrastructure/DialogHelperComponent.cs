using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Dialog.Infrastructure;
internal class DialogHelperComponent : ComponentBase {
    [Parameter, EditorRequired]
    public RenderFragment ChildContent { get; set; } = default!;

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.AddContent(0, ChildContent);
    }
}

