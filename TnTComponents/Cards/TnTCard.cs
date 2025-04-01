using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using TnTComponents.Core;
using TnTComponents.Interfaces;

namespace TnTComponents;

/// <summary>
///     Represents a card component with customizable appearance, background color, border radius, elevation, text alignment, and text color.
/// </summary>
public class TnTCard : TnTComponentBase, ITnTStyleable {

    /// <summary>
    ///     The appearance of the card. Default is <see cref="CardAppearance.Filled" />.
    /// </summary>
    [Parameter]
    public CardAppearance Appearance { get; set; } = CardAppearance.Filled;

    /// <inheritdoc />
    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.SurfaceContainerLow;

    /// <inheritdoc />
    [Parameter]
    public TnTBorderRadius? BorderRadius { get; set; } = new(3);

    /// <summary>
    ///     The child content to be rendered inside the card.
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddTnTStyleable(this)
        .AddFilled(Appearance == CardAppearance.Filled)
        .AddOutlined(Appearance == CardAppearance.Outlined)
        .Build();

    /// <inheritdoc />
    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    /// <inheritdoc />
    [Parameter]
    public int Elevation { get; set; } = 1;

    /// <inheritdoc />
    [Parameter]
    public TextAlign? TextAlignment { get; set; }

    /// <inheritdoc />
    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnSurface;

    /// <inheritdoc />
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