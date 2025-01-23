using Microsoft.AspNetCore.Components;
using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTBadge {

    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.Error;

    [Parameter]
    public TnTBorderRadius? BorderRadius { get; set; } = TnTBorderRadius.Full;

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-badge")
        .AddFilled()
        .AddTnTStyleable(this)
        .Build();

    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddStyle("border", $"1px solid tnt-color-{TextColor.ToCssClassName()}")
        .Build();

    [Parameter]
    public int Elevation { get; set; } = 2;

    [Parameter]
    public TextAlign? TextAlignment { get; set; } = TextAlign.Center;

    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnError;
}