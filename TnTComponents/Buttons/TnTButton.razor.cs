using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Reflection.Metadata;
using System.Xml.Linq;
using TnTComponents.Core;
using TnTComponents.Interfaces;

namespace TnTComponents;

/// <summary>
///     Represents a customizable button component.
/// </summary>
public partial class TnTButton {

    /// <inheritdoc />
    [Parameter]
    public virtual ButtonAppearance Appearance { get; set; }

    /// <summary>
    /// The shape of the button, which can be rounded or square
    /// </summary>
    [Parameter]
    public ButtonShape Shape { get; set; } = ButtonShape.Round;

    /// <inheritdoc />
    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.Primary;

    /// <summary>
    ///     The size of the button.
    /// </summary>
    [Parameter]
    public Size ButtonSize { get; set; } = Size.Small;

    /// <summary>
    ///     The content to be rendered inside the button.
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

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
}