using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Routing;
using System.Diagnostics;
using TnTComponents.Core;
using TnTComponents.Interfaces;

namespace TnTComponents;

/// <summary>
///     Represents a navigation link (anchor) component with customizable styles and behaviors.
/// </summary>
public class TnTNavLink : NavLink, ITnTComponentBase, ITnTInteractable, ITnTStyleable {

    /// <inheritdoc />
    [Parameter]
    public virtual TnTColor? ActiveBackgroundColor { get; set; }

    /// <inheritdoc />
    [Parameter]
    public virtual TnTColor? ActiveTextColor { get; set; }

    /// <summary>
    ///     The appearance of the anchor.
    /// </summary>
    [Parameter]
    public AnchorAppearance Appearance { get; set; }

    /// <inheritdoc />
    [Parameter]
    public bool? AutoFocus { get; set; }

    /// <inheritdoc />
    [Parameter]
    public virtual TnTColor BackgroundColor { get; set; } = TnTColor.Primary;

    /// <inheritdoc />
    [Parameter]
    public virtual TnTBorderRadius? BorderRadius { get; set; } = new(2);

    /// <inheritdoc />
    [Parameter]
    public bool Disabled { get; set; }

    /// <inheritdoc />
    public ElementReference Element { get; protected set; }

    /// <inheritdoc />
    public virtual string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass(CssClass)
        .AddClass("tnt-nav-link")
        .AddTnTInteractable(this, enableTint: Appearance is AnchorAppearance.Filled or AnchorAppearance.Outlined, enable: Appearance is AnchorAppearance.Filled or AnchorAppearance.Outlined)
        .AddTnTStyleable(this, enableElevation: Appearance is AnchorAppearance.Filled or AnchorAppearance.Outlined)
        .AddFilled(Appearance == AnchorAppearance.Filled)
        .AddOutlined(Appearance == AnchorAppearance.Outlined)
        .AddUnderlined(Appearance == AnchorAppearance.Underlined)
        .AddClass("active-fg-color", ActiveTextColor.HasValue)
        .AddClass("active-bg-color", ActiveBackgroundColor.HasValue)
        .Build();

    /// <inheritdoc />
    [Parameter]
    public string? ElementId { get; set; }

    /// <inheritdoc />
    [Parameter]
    public string? ElementLang { get; set; }

    /// <inheritdoc />
    [Parameter]
    public string? ElementName { get; set; }

    /// <inheritdoc />
    public string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddVariable("active-bg-color", ActiveBackgroundColor.GetValueOrDefault(), ActiveBackgroundColor.HasValue)
        .AddVariable("active-fg-color", ActiveTextColor.GetValueOrDefault(), ActiveTextColor.HasValue)
        .Build();

    /// <inheritdoc />
    [Parameter]
    public string? ElementTitle { get; set; }

    /// <inheritdoc />
    [Parameter]
    public int Elevation { get; set; }

    /// <inheritdoc />
    [Parameter]
    public bool EnableRipple { get; set; } = true;

    /// <inheritdoc />
    [Parameter]
    public TnTColor? OnTintColor { get; set; }

    /// <inheritdoc />
    [Parameter]
    public TextAlign? TextAlignment { get; set; }

    /// <inheritdoc />
    [Parameter]
    public virtual TnTColor TextColor { get; set; } = TnTColor.OnBackground;

    /// <inheritdoc />
    [Parameter]
    public TnTColor? TintColor { get; set; } = TnTColor.SurfaceTint;

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "a");
        builder.AddMultipleAttributes(10, AdditionalAttributes);
        builder.AddAttribute(20, "class", ElementClass);
        builder.AddAttribute(30, "style", ElementStyle);
        builder.AddAttribute(40, "lang", ElementLang);
        builder.AddAttribute(50, "id", ElementId);
        builder.AddAttribute(60, "title", ElementTitle);
        builder.AddAttribute(80, "autofocus", AutoFocus);
        builder.AddElementReferenceCapture(90, e => Element = e);
        builder.AddContent(100, ChildContent);

        if (EnableRipple) {
            builder.OpenComponent<TnTRippleEffect>(110);
            builder.CloseComponent();
        }

        builder.CloseElement();
    }

    /// <inheritdoc />
    protected override void OnParametersSet() {
        base.OnParametersSet();
        if (Disabled && AdditionalAttributes?.ContainsKey("href") == true) {
            var attributes = new Dictionary<string, object>(AdditionalAttributes);
            attributes.Remove("href");
            AdditionalAttributes = attributes;
        }
    }
}