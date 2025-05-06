using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Core;

namespace TnTComponents;
/// <summary>
/// Represents a navigation link (anchor) component specifically designed for side navigation menus.
/// </summary>
public class TnTSideNavLink : TnTNavLink {

    /// <inheritdoc />
    [Parameter]
    public override TnTColor? ActiveBackgroundColor { get; set; } = TnTColor.SecondaryContainer;

    /// <inheritdoc />
    [Parameter]
    public override TnTColor? ActiveTextColor { get; set; } = TnTColor.OnSecondaryContainer;

    /// <inheritdoc />
    [Parameter]
    public override TnTColor BackgroundColor { get; set; } = TnTColor.Transparent;

    /// <inheritdoc />
    [Parameter]
    public override TnTBorderRadius? BorderRadius { get; set; } = new(10);

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass(CssClass)
        .AddClass("tnt-side-nav-link")
        .AddTnTInteractable(this)
        .AddTnTStyleable(this)
        .AddClass("active-fg-color", ActiveTextColor.HasValue)
        .AddClass("active-bg-color", ActiveBackgroundColor.HasValue)
        .Build();

    /// <summary>
    ///     An option Icon for the link
    /// </summary>
    [Parameter]
    public TnTIcon? Icon { get; set; }

    /// <summary>
    ///     The label for the link.
    /// </summary>
    [Parameter, EditorRequired]
    public string Label { get; set; } = default!;

    /// <inheritdoc />
    [Parameter]
    public override TnTColor TextColor { get; set; } = TnTColor.OnSurfaceVariant;

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

        if (Icon is not null) {
            builder.AddContent(100, Icon.Render());
        }

        builder.AddContent(110, Label);

        if (EnableRipple) {
            builder.OpenComponent<TnTRippleEffect>(120);
            builder.CloseComponent();
        }

        builder.CloseElement();
    }
}