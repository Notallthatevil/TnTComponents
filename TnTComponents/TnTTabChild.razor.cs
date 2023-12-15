using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Globalization;
using System.Web;
using TnTComponents.Common;
using TnTComponents.Enum;
using TnTComponents.Infrastructure;

namespace TnTComponents;
public partial class TnTTabChild : IDisposable {
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter, EditorRequired]
    public string Title { get; set; } = default!;

    [Parameter]
    public string TabButtonClass { get; set; } = "tnt-tab-view-button";

    [Parameter]
    public override string? Class { get; set; } = "tnt-tab-view-content";

    [Parameter]
    public string? Icon { get; set; }

    [Parameter]
    public IconType IconType { get; set; }

    [Parameter]
    public bool Active { get; set; } = false;

    [Parameter]
    public bool Disabled { get; set; }

    [CascadingParameter]
    private TabViewContext _context { get; set; } = default!;

    [Parameter]
    public EventCallback<TnTTabChild> TabClickedCallback { get; set; }

    public void Dispose() {
        GC.SuppressFinalize(this);
        _context.RemoveChild(this);
    }

    public override string GetClass() => base.GetClass() + " " + (Active ? "active" : string.Empty);

    protected override void OnParametersSet() {
        base.OnParametersSet();
        if (_context is null) {
            throw new InvalidOperationException($"A {nameof(TnTTabChild)} must be a child of {nameof(TnTTabView)}");
        }

        _context.AddChild(this);
    }

    public RenderFragment RenderTabHeader() {
        return new RenderFragment(builder => {
            builder.OpenElement(0, "button");
            builder.AddAttribute(5, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, async (args) => {
                await TabClickedCallback.InvokeAsync(this);
            }));
            builder.AddAttribute(10, "class", $"{TabButtonClass} {(Active ? "active" : string.Empty)}");
            builder.AddAttribute(20, "disabled", Disabled);
            builder.AddAttribute(30, "type", "button");
            builder.AddAttribute(35, TnTCustomIdentifier, ComponentIdentifier);

            builder.AddContent(40, TnTIconComponent.RenderIcon(IconType, Icon));
            builder.AddContent(50, Title);
            builder.CloseElement();
        });
    }

    public RenderFragment RenderTabContent() {
        return new RenderFragment(builder => {
            builder.OpenElement(0, "div");
            builder.AddMultipleAttributes(10, AdditionalAttributes);
            builder.AddAttribute(20, "class", GetClass());
            builder.AddAttribute(30, "style", Style);
            builder.AddAttribute(35, TnTCustomIdentifier, ComponentIdentifier);
            builder.AddAttribute(40, "theme", Theme);
            builder.AddContent(45, ChildContent);
            builder.AddElementReferenceCapture(50, (e) => Element = e);
            builder.CloseElement();
        });
    }

}
