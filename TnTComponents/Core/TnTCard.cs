﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using TnTComponents.Core;

namespace TnTComponents;

public class TnTCard : ComponentBase {

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public AlignContent? AlignContent { get; set; }

    [Parameter]
    public AlignItems? AlignItems { get; set; }

    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.SurfaceVariant;

    [Parameter]
    public TnTBorderRadius BorderRadius { get; set; } = new(4);

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    public string? CssClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddFlexBox(Direction, AlignItems, JustifyContent, AlignContent, FlexBox)
        .AddBackgroundColor(MakeOutlined ? null : BackgroundColor)
        .AddOutlined(MakeOutlined)
        .AddForegroundColor(TextColor)
        .AddBorderRadius(BorderRadius)
        .AddElevation(Elevation)
        .AddPadding(Padding)
        .AddMargin(Margin)
        .Build();

    [Parameter]
    public LayoutDirection? Direction { get; set; }

    [Parameter]
    public int Elevation { get; set; } = 2;

    [Parameter]
    public bool FlexBox { get; set; }

    [Parameter]
    public JustifyContent? JustifyContent { get; set; }

    [Parameter]
    public bool MakeOutlined { get; set; }

    [Parameter]
    public int Margin { get; set; } = 2;

    [Parameter]
    public int Padding { get; set; } = 2;

    public string? CssStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnSurfaceVariant;

    [Parameter]
    public bool UseSpan { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, UseSpan ? "span" : "div");
        builder.AddMultipleAttributes(10, AdditionalAttributes);
        builder.AddAttribute(20, "class", CssClass);
        builder.AddAttribute(30, "style", CssStyle);
        builder.AddContent(40, ChildContent);
        builder.CloseElement();
    }
}