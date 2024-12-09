using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using TnTComponents.Core;
using TnTComponents.Interfaces;

namespace TnTComponents;

/// <summary>
///     Represents a card component with customizable appearance, background color, border radius,
///     elevation, text alignment, and text color.
/// </summary>
public class TnTCard : TnTComponentBase, ITnTStyleable {

    /// <summary>
    ///     Gets or sets the appearance of the card. Default is <see cref="CardAppearance.Filled" />.
    /// </summary>
    [Parameter]
    public CardAppearance Appearance { get; set; } = CardAppearance.Filled;

    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.SurfaceContainerLow;

    [Parameter]
    public TnTBorderRadius? BorderRadius { get; set; } = new(3);

    /// <summary>
    ///     Gets or sets the child content to be rendered inside the card.
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

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
    public int Elevation { get; set; } = 1;

    [Parameter]
    public TextAlign? TextAlignment { get; set; }

    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnSurface;

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
