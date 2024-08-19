using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using TnTComponents.Core;
using TnTComponents.Interfaces;

namespace TnTComponents;

public class TnTCard : TnTComponentBase, ITnTStyleable {
    [Parameter]
    public CardAppearance Appearance { get; set; } = CardAppearance.Filled;
    [Parameter]
    public TextAlign? TextAlignment { get; set; }
    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.SurfaceContainerLow;
    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnSurface;
    [Parameter]
    public int Elevation { get; set; } = 1;
    [Parameter]
    public TnTBorderRadius? BorderRadius { get; set; } = new(3);
    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddTnTStyleable(this)
        .AddFilled(Appearance == CardAppearance.Filled)
        .AddOutlined(Appearance == CardAppearance.Outlined)
        .Build();
    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "div");
        builder.AddMultipleAttributes(10, AdditionalAttributes);
        builder.AddAttribute(20, "class", ElementClass);
        builder.AddAttribute(30, "style", ElementStyle);
        builder.AddAttribute(40, "lang", ElementLang);
        builder.AddAttribute(50, "title", ElementTitle);
        builder.AddAttribute(60, "id", ElementId);
        builder.AddContent(70, ChildContent);
        builder.CloseElement();
    }
}