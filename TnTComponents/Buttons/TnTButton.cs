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
public class TnTButton : TnTComponentBase, ITnTStyleable, ITnTInteractable {

    /// <inheritdoc />
    [Parameter]
    public virtual ButtonAppearance Appearance { get; set; }

    /// <inheritdoc />
    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.Primary;

    /// <inheritdoc />
    [Parameter]
    public virtual TnTBorderRadius? BorderRadius { get; set; } = TnTBorderRadius.Full;

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
        .AddOutlined(Appearance == ButtonAppearance.Outlined)
        .AddFilled(Appearance == ButtonAppearance.Filled)
        .AddTnTStyleable(this, enableElevation: Appearance != ButtonAppearance.Outlined && Appearance != ButtonAppearance.Text)
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
    public virtual int Elevation { get; set; } = 1;

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

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "button");
        builder.AddMultipleAttributes(10, AdditionalAttributes);
        builder.AddAttribute(20, "class", ElementClass);
        builder.AddAttribute(30, "style", ElementStyle);
        builder.AddAttribute(40, "type", Type.ToHtmlAttribute());
        builder.AddAttribute(50, "name", ElementName);
        builder.AddAttribute(60, "disabled", Disabled);
        builder.AddAttribute(70, "autofocus", AutoFocus);
        builder.AddAttribute(80, "title", ElementTitle);
        builder.AddAttribute(90, "id", ElementId);
        if (OnClickCallback.HasDelegate) {
            builder.AddAttribute(100, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, OnClickCallback));
        }

        if (StopPropagation) {
            builder.AddEventStopPropagationAttribute(110, "onclick", true);
        }

        if (EnableRipple) {
            builder.OpenComponent<TnTRippleEffect>(120);
            builder.CloseComponent();
        }

        builder.AddElementReferenceCapture(130, __value => Element = __value);
        builder.AddContent(140, ChildContent);

        builder.CloseElement();
    }
}