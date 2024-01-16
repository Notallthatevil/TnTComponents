using Microsoft.AspNetCore.Components;
using TnTComponents.Core;
using TnTComponents.Enum;

namespace TnTComponents;

public partial class TnTCard {

    [Parameter]
    public CardAppearance Appearance { get; set; }

    [Parameter]
    public LayoutDirection Direction { get; set; } = LayoutDirection.Vertical;

    [Parameter]
    public AlignItems? AlignItems { get; set; }

    [Parameter]
    public JustifyContent? JustifyContent { get; set; }

    [Parameter]

    public AlignContent? AlignContent { get; set; }

    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.SurfaceVariant;

    public TnTColor TextColor { get; set; } = TnTColor.OnSurfaceVariant;

    public override string? Class => CssBuilder.Create()
        .AddOutlined(Appearance == CardAppearance.Outlined)
        .AddFilled(Appearance == CardAppearance.Filled)
        .AddBackgroundColor(Appearance == CardAppearance.Filled ? BackgroundColor : null)
        .AddForegroundColor(TextColor)
        .AddElevation(Elevation)
        .AddBorderRadius(BorderRadius)
        .AddFlexBox(Direction, AlignItems, JustifyContent, AlignContent)
        .Build();

    [Parameter]
    public int Elevation { get; set; } = 0;

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public TnTBorderRadius BorderRadius { get; set; } = new(2);
}
