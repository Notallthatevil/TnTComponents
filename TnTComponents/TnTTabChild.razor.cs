using Microsoft.AspNetCore.Components;
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

    [Inject]
    private NavigationManager _navMan { get; set; } = default!;

    //private string tabChildParam => $"{Uri.EscapeDataString(_context.Name)}";

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

        //var url = new Uri(_navMan.Uri);

        //var queryParams = HttpUtility.ParseQueryString(url.Query);

        //if (queryParams[tabChildParam] == Uri.EscapeDataString(Title)) {
        //    Active = true;
        //}

    }

    public RenderFragment RenderTabHeader() {
        //    var url = new Uri(_navMan.Uri);

        //    var queryParams = HttpUtility.ParseQueryString(url.Query);

        //    queryParams[tabChildParam] = Uri.EscapeDataString(Title);

        //    url = new UriBuilder(_navMan.Uri) {
        //        Query = queryParams.ToString()
        //    }.Uri;

        return new RenderFragment(builder => {
            builder.OpenElement(0, "button");
            builder.AddAttribute(10, "class", $"{TabButtonClass} {(Active ? "active" : string.Empty)}");
            builder.AddAttribute(20, "disabled", Disabled);
            builder.AddAttribute(30, "type", "button");

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
            builder.AddAttribute(40, "theme", Theme);
            builder.AddContent(50, ChildContent);
            builder.CloseElement();
        });
    }

}
