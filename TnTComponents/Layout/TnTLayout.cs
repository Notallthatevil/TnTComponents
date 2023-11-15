using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System.Text;
using TnTComponents.Common;
using TnTComponents.Infrastructure;

namespace TnTComponents.Layout;
public partial class TnTLayout : ComponentBase {

    internal const string ExpandClass = "expand";

    [Parameter]
    public RenderFragment Body { get; set; } = default!;

    [Parameter]
    public RenderFragment? Header { get; set; }

    [Parameter]
    public RenderFragment? Footer { get; set; }

    [Parameter]
    public RenderFragment? SideNav { get; set; }


    [Parameter]
    public string HeaderCssClass { get; set; } = "tnt-header";

    [Parameter]
    public string FooterCssClass { get; set; } = "tnt-footer";

    [Parameter]
    public string BodyCssClass { get; set; } = "tnt-body";

    [Parameter]
    public string SideNavCssClass { get; set; } = "tnt-side-nav";

    [Parameter]
    public string Theme { get; set; } = "default";

    public string? TnTSideNavId { get; private set; }

    public string? TntHeaderId { get; private set; }
    public string? TntFooterId { get; private set; }
    public string? TntBodyId { get; private set; }

    public bool Interactive { get; private set; }

    internal bool Expand { get; set; }

    public void Refresh() {
        StateHasChanged();
    }

    protected override void OnAfterRender(bool firstRender) {
        if (firstRender) {
            Interactive = true;
            StateHasChanged();
        }
        base.OnAfterRender(firstRender);
    }

    protected override void BuildRenderTree(RenderTreeBuilder __builder) {
        __builder.OpenComponent<CascadingValue<TnTLayout>>(0);
        __builder.AddComponentParameter(10, "Value", this);
        __builder.AddComponentParameter(20, "IsFixed", true);

        __builder.AddComponentParameter(25, "ChildContent", new RenderFragment(b => {
            if (SideNav is not null) {
                TnTSideNavId = Guid.NewGuid().ToString().Replace("-", string.Empty);
                b.OpenElement(30, "div");
                b.AddAttribute(40, "class", BuildCssClassList(SideNavCssClass));
                b.AddAttribute(50, "theme", Theme);
                if (!Interactive) {
                    b.AddAttribute(55, "id", TnTSideNavId);
                }
                b.AddContent(70, SideNav);
                b.CloseElement();
            }

            if (Header is not null) {
                TntHeaderId = Guid.NewGuid().ToString().Replace("-", string.Empty);
                b.OpenElement(80, "div");
                b.AddAttribute(90, "class", BuildCssClassList(HeaderCssClass));
                b.AddAttribute(100, "theme", Theme);
                if (!Interactive) {
                    b.AddAttribute(105, "id", TntHeaderId);
                }
                b.AddContent(120, Header);
                b.CloseElement();
            }

            TntBodyId = Guid.NewGuid().ToString().Replace("-", string.Empty);
            b.OpenElement(130, "div");
            b.AddAttribute(140, "class", BuildCssClassList(BodyCssClass));
            b.AddAttribute(150, "theme", Theme);
            if (!Interactive) {
                b.AddAttribute(105, "id", TntBodyId);
            }
            b.AddContent(170, Body);
            b.CloseElement();

            if (Footer is not null) {
                TntFooterId = Guid.NewGuid().ToString().Replace("-", string.Empty);
                b.OpenElement(180, "div");
                b.AddAttribute(190, "class", BuildCssClassList(FooterCssClass));
                b.AddAttribute(200, "theme", Theme);
                if (!Interactive) {
                    b.AddAttribute(105, "id", TntFooterId);
                }
                b.AddContent(220, Footer);
                b.CloseElement();
            }
        }));

        __builder.CloseComponent();
    }


    private string BuildCssClassList(string baseClass) {
        var strBuilder = new StringBuilder(baseClass);

        if (Expand) {
            strBuilder.Append(' ').Append("expand");
        }
        return strBuilder.ToString();
    }

}
