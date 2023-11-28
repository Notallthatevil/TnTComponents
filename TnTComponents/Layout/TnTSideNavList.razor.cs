using Microsoft.AspNetCore.Components;
using System.Text;

namespace TnTComponents.Layout;

public partial class TnTSideNavList {

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public string BaseCssClass { get; set; } = "tnt-side-nav-list";

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public string Theme { get; set; } = default!;

    public string GetCssClass() {
        var strBuilder = new StringBuilder(BaseCssClass);

        if (AdditionalAttributes?.TryGetValue("class", out var result) == true) {
            strBuilder.Append(' ').AppendJoin(' ', result);
        }

        return strBuilder.ToString();
    }
}