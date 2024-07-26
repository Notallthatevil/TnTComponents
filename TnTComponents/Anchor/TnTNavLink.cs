using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Routing;
using System.Diagnostics;
using TnTComponents.Core;
using TnTComponents.Interfaces;

namespace TnTComponents;

public class TnTNavLink : NavLink, ITnTComponentBase, ITnTInteractable, ITnTStyleable {

    [Parameter]
    public virtual TnTColor? ActiveBackgroundColor { get; set; }

    [Parameter]
    public virtual TnTColor? ActiveTextColor { get; set; }

    [Parameter]
    public AnchorAppearance Appearance { get; set; }

    [Parameter]
    public bool? AutoFocus { get; set; }

    [Parameter]
    public virtual TnTColor BackgroundColor { get; set; } = TnTColor.Primary;

    [Parameter]
    public virtual TnTBorderRadius? BorderRadius { get; set; } = new(2);

    [Parameter]
    public bool Disabled { get; set; }

    public ElementReference Element { get; protected set; }

    public virtual string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass(CssClass)
        .AddClass("tnt-nav-link")
        .AddTnTInteractable(this)
        .AddTnTStyleable(this)
        .AddFilled(Appearance == AnchorAppearance.Filled)
        .AddOutlined(Appearance == AnchorAppearance.Outlined)
        .AddUnderlined(Appearance == AnchorAppearance.Underlined)
        .AddClass("active-fg-color", ActiveTextColor.HasValue)
        .AddClass("active-bg-color", ActiveBackgroundColor.HasValue)
        .Build();

    [Parameter]
    public string? ElementId { get; set; }

    [Parameter]
    public string? ElementLang { get; set; }

    public string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddVariable("active-bg-color", ActiveBackgroundColor.GetValueOrDefault(), ActiveBackgroundColor.HasValue)
        .AddVariable("active-fg-color", ActiveTextColor.GetValueOrDefault(), ActiveTextColor.HasValue)
        .Build();

    [Parameter]
    public string? ElementTitle { get; set; }

    [Parameter]
    public int Elevation { get; set; }

    public bool EnableRipple => true;

    [Parameter]
    public string? Name { get; set; }

    [Parameter]
    public TnTColor? OnTintColor { get; set; }

    [Parameter]
    public TextAlign? TextAlignment { get; set; }

    [Parameter]
    public virtual TnTColor TextColor { get; set; } = TnTColor.OnBackground;

    [Parameter]
    public TnTColor? TintColor { get; set; } = TnTColor.SurfaceTint;

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
        builder.CloseElement();
    }

    protected override void OnParametersSet() {
        base.OnParametersSet();
        if (Disabled && AdditionalAttributes?.ContainsKey("href") == true) {
            var attributes = new Dictionary<string, object>(AdditionalAttributes);
            attributes.Remove("href");
            AdditionalAttributes = attributes;
        }
    }
}