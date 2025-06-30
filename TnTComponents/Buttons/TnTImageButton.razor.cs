using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TnTComponents.Core;

namespace TnTComponents;

/// <summary>
///     Represents an image button component with customizable icon and optional badge.
/// </summary>
public partial class TnTImageButton {

    /// <inheritdoc />
    [Parameter]
    public virtual ButtonAppearance Appearance { get; set; }

    /// <inheritdoc />
    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.Primary;

    /// <summary>
    ///     The size of the button.
    /// </summary>
    [Parameter]
    public Size ButtonSize { get; set; } = Size.Small;

    /// <summary>
    ///     The icon to be rendered inside the button.
    /// </summary>
    [Parameter, EditorRequired]
    public TnTIcon Icon { get; set; } = default!;

    /// <inheritdoc />
    [Parameter]
    public bool Disabled { get; set; }

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddBackgroundColor(BackgroundColor)
        .AddForegroundColor(TextColor)
        .AddTextAlign(TextAlignment)
        .AddClass("tnt-filled", Appearance == ButtonAppearance.Filled || Appearance == ButtonAppearance.Elevated)
        .AddClass("tnt-outlined", Appearance == ButtonAppearance.Outlined)
        .AddClass("tnt-text", Appearance == ButtonAppearance.Text)
        .AddClass("tnt-elevated", Appearance == ButtonAppearance.Elevated)
        .AddClass("tnt-button-square", Shape == ButtonShape.Square)
        .AddClass("tnt-image-button-round", ImageButtonAppearance == ImageButtonAppearance.Round)
        .AddClass("tnt-image-button-wide", ImageButtonAppearance == ImageButtonAppearance.Wide)
        .AddClass("tnt-image-button-narrow", ImageButtonAppearance == ImageButtonAppearance.Narrow)
        .AddSize(ButtonSize)
        .AddTnTInteractable(this)
        .Build();

    /// <inheritdoc />
    [Parameter]
    public string? ElementName { get; set; }

    /// <inheritdoc />
    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    /// <inheritdoc />
    [Parameter]
    public bool EnableRipple { get; set; } = true;

    /// <inheritdoc />
    [Parameter]
    public EventCallback<MouseEventArgs> OnClickCallback { get; set; }

    /// <inheritdoc />
    [Parameter]
    public TnTColor? OnTintColor { get; set; }

    /// <summary>
    ///     The shape of the button, which can be rounded or square
    /// </summary>
    [Parameter]
    public ButtonShape Shape { get; set; } = ButtonShape.Round;

    /// <summary>
    ///     When set, the click event will not propagate to parent elements.
    /// </summary>
    [Parameter]
    public bool StopPropagation { get; set; }

    /// <inheritdoc />
    [Parameter]
    public TextAlign? TextAlignment { get; set; }

    /// <inheritdoc />
    [Parameter]
    public virtual TnTColor TextColor { get; set; } = TnTColor.OnPrimary;

    /// <inheritdoc />
    [Parameter]
    public TnTColor? TintColor { get; set; } = TnTColor.SurfaceTint;

    /// <summary>
    ///     The type of the button.
    /// </summary>
    [Parameter]
    public ButtonType Type { get; set; }

    /// <summary>
    ///  The appearance of the image button, which can be Round, Wide, or Narrow.
    /// </summary>
    [Parameter]
    public ImageButtonAppearance ImageButtonAppearance { get; set; } = ImageButtonAppearance.Round;
}

/// <summary>
/// Specifies the appearance of the image button.
/// </summary>
public enum ImageButtonAppearance {
    /// <summary>
    /// The button has a round appearance.
    /// </summary>
    Round,
    /// <summary>
    /// The button has a wide rectangular appearance.
    /// </summary>
    Wide,
    /// <summary>
    /// The button has a narrow rectangular appearance.
    /// </summary>
    Narrow
}
