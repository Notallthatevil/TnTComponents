using Microsoft.AspNetCore.Components;
using System.Text;
using TnTComponents.Infrastructure;

namespace TnTComponents.Layout;
public partial class TnTLayout {

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

    internal string TnTSideNavId = Guid.NewGuid().ToString();

    private string _tntHeaderId = Guid.NewGuid().ToString();
    private string _tntFooterId = Guid.NewGuid().ToString();
    private string _tntBodyId = Guid.NewGuid().ToString();

    public bool Interactive { get; private set; }

    internal bool Expand { get; set; }

    public void Refresh() {
        StateHasChanged();
    }

    protected override void OnAfterRender(bool firstRender) {
        if (firstRender) {
            Interactive = true;
        }
        base.OnAfterRender(firstRender);
    }


    private string BuildCssClassList(string baseClass) {
        var strBuilder = new StringBuilder(baseClass);

        if (Expand) {
            strBuilder.Append(' ').Append("expand");
        }
        return strBuilder.ToString();
    }

}
