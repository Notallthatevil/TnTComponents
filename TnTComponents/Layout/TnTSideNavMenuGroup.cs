using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using TnTComponents.Core;
using TnTComponents.Interfaces;

namespace TnTComponents;

public class TnTSideNavMenuGroup : TnTInteractableComponentBase, ITnTStyleable {
    [Parameter]
    public bool ExpandByDefault { get; set; } = true;

    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-side-nav-menu-group")
        .AddClass("tnt-toggle", ExpandByDefault)
        .AddDisabled(Disabled)
        .Build();

    public override string? ElementStyle { get; }
    [Parameter]
    public TextAlign? TextAlignment { get; set; }
    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.SurfaceVariant;
    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnSurfaceVariant;
    [Parameter]
    public int Elevation { get; set; }
    [Parameter]
    public TnTBorderRadius? BorderRadius { get; set; } = new(10);
    [Parameter]
    public override TnTColor? TintColor { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public TnTIcon? Icon { get; set; }

    [Parameter, EditorRequired]
    public string Label { get; set; } = default!;


    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "div");
        builder.AddMultipleAttributes(10, AdditionalAttributes);
        builder.AddAttribute(20, "class", ElementClass);
        builder.AddAttribute(30, "style", ElementStyle);
        builder.AddAttribute(40, "lang", ElementLang);
        builder.AddAttribute(50, "id", ElementId);
        builder.AddAttribute(60, "title", ElementTitle);
        builder.AddAttribute(80, "autofocus", AutoFocus);
        builder.AddElementReferenceCapture(90, e => Element = e);

        builder.OpenElement(100, "div");
        builder.AddAttribute(110, "class", CssClassBuilder.Create()
            .AddClass("tnt-side-nav-menu-group-button")
            .AddTnTInteractable(this)
            .AddTnTStyleable(this)
            .AddFilled()
            .Build()
        );
        builder.AddAttribute(120, "onclick", "TnTComponents.toggleSideNavGroup(event)");

        builder.OpenElement(121, "span");
        if (Icon is not null) {
            builder.AddContent(130, Icon.Render());
        }

        builder.AddContent(140, Label);
        builder.CloseElement();

        builder.OpenComponent<MaterialIcon>(150);
        builder.AddComponentParameter(160, nameof(MaterialIcon.Icon), MaterialIcon.ArrowDropDown);
        builder.AddAttribute(161, "class", "tnt-end-icon");
        builder.CloseComponent();

        builder.CloseElement();

        // Data permanent section
        {
            builder.OpenElement(170, "div");
            builder.AddAttribute(180, "class", "tnt-side-nav-group-toggle-indicator");
            builder.AddAttribute(190, "style", "display:none");
            builder.AddAttribute(200, "data-permanent");

            builder.OpenElement(210, "div");
            builder.AddAttribute(220, "class", $"tnt-toggle-indicator{(ExpandByDefault ? " tnt-toggle" : string.Empty)}");
            builder.CloseElement();

            builder.CloseElement();
        }

        builder.AddContent(230, ChildContent);

        builder.CloseElement();
    }

}