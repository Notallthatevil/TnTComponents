using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using TnTComponents.Core;
using TnTComponents.Interfaces;

namespace TnTComponents;

public class TnTSideNavToggle : TnTComponentBase, ITnTInteractable {

    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-side-nav-toggle")
        .AddTnTInteractable(this)
        .Build();

    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    [Parameter]
    public TnTIcon Icon { get; set; } = new MaterialIcon { Icon = MaterialIcon.Menu };

    [Parameter]
    public bool Disabled { get; set; }
    [Parameter]
    public string? ElementName { get; set; }
    [Parameter]
    public bool EnableRipple { get; set; } = true;
    [Parameter]
    public TnTColor? TintColor { get; set; } = TnTColor.SurfaceTint;
    [Parameter]
    public TnTColor? OnTintColor { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "button");
        builder.AddMultipleAttributes(10, AdditionalAttributes);
        builder.AddAttribute(20, "class", ElementClass);
        builder.AddAttribute(30, "style", ElementStyle);
        builder.AddAttribute(40, "lang", ElementLang);
        builder.AddAttribute(50, "id", ElementId);
        builder.AddAttribute(60, "title", ElementTitle);
        builder.AddAttribute(70, "onclick", "TnTComponents.toggleSideNav(event)");
        builder.AddElementReferenceCapture(80, e => Element = e);
        builder.AddContent(90, Icon.Render());

        if (EnableRipple) {
            builder.OpenComponent<TnTRippleEffect>(100);
            builder.CloseComponent();
        }

        builder.CloseElement();
    }
}