using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Core;

namespace TnTComponents;
public class TnTDivider : ComponentBase {


    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public TnTColor Color { get; set; } = TnTColor.OutlineVariant;

    public string? Class => CssBuilder.Create()
        .AddBackgroundColor(Color)
        .Build();

    [Parameter]
    public string? Style { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "hr");
        builder.AddMultipleAttributes(10, AdditionalAttributes);
        builder.AddAttribute(20, "class", Class);
        builder.AddAttribute(30, "style", Style);
        builder.CloseElement();
    }
}

