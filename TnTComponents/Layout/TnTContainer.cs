﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Core;

namespace TnTComponents;
public class TnTContainer : ComponentBase, ITnTComponentBase {
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }
    public string? Class => CssBuilder.Create()
        .AddClass("tnt-container")
        .Build();
    public ElementReference Element { get; }
    [Parameter]
    public string? Id { get; set; }
    [Parameter]
    public string? Style { get; set; }
    [Parameter]
    public bool? AutoFocus { get; set; }
    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "div");
        builder.AddMultipleAttributes(10, AdditionalAttributes);
        builder.AddAttribute(20, "class", Class);
        builder.AddAttribute(30, "style", Style);
        builder.AddContent(40, ChildContent);
        builder.CloseElement();
    }
}

