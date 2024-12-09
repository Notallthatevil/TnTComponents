using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using TnTComponents.Core;
using TnTComponents.Interfaces;

namespace TnTComponents;

public class TnTTabChild : TnTComponentBase, ITnTInteractable {

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-tab-child")
        .AddDisabled(Disabled)
        .Build();

    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    [Parameter]
    public TnTIcon? Icon { get; set; }

    [Parameter]
    public RenderFragment? TabHeaderTemplate { get; set; }

    [Parameter, EditorRequired]
    public string Label { get; set; } = default!;

    [CascadingParameter]
    private TnTTabView _context { get; set; } = default!;
    [Parameter]
    public bool Disabled { get; set; }
    [Parameter]

    public string? ElementName { get; set; }
    [Parameter]

    public bool EnableRipple { get; set; }
    [Parameter]

    public TnTColor? OnTintColor { get; set; }
    [Parameter]

    public TnTColor? TintColor { get; set; }

    protected override void OnAfterRender(bool firstRender) {
        base.OnAfterRender(firstRender);
    }

    protected override void OnInitialized() {
        base.OnInitialized();
        if (_context is null) {
            throw new InvalidOperationException($"A {nameof(TnTTabChild)} must be a child of {nameof(TnTTabView)}");
        }
        _context.AddTabChild(this);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "div");
        builder.AddMultipleAttributes(10, AdditionalAttributes);
        builder.AddAttribute(20, "class", ElementClass);
        builder.AddAttribute(30, "style", ElementStyle);
        builder.AddAttribute(40, "title", ElementTitle);
        builder.AddAttribute(50, "id", ElementId);
        builder.AddAttribute(60, "lang", ElementLang);
        builder.AddAttribute(70, "name", ElementName);
        builder.AddElementReferenceCapture(80, e => Element = e);
        builder.AddContent(90, ChildContent);
        builder.CloseElement();
    }
}